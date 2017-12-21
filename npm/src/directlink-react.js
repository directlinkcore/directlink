// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

/*global React, ReactDOM, components, signalR*/
let Utils = require('./utils');
let DefferedPromise = Utils.DeferredPromise;

let directlink = {
    data: {},
    instances: {},
    scripts: {},
    styles: {},
    promises: {},
    nocache: false,
    hmr: false,

    renderApp: (container, data) => {
        directlink.container = container;
        directlink.hub.start()
            .then(() => {
                if (data) {
                    directlink.render(data);
                } else {
                    directlink.requestData(window.location.pathname);
                }
                window.onpopstate = directlink.history.popState;
                if ('scrollRestoration' in history) {
                    history.scrollRestoration = 'manual';
                }
                directlink.history.current = { path: location.pathname, hash: location.hash };
            })
            .catch(error => {
                container.innerHTML = '';
                throw error || 'start signalr connection failed';
            });
    },

    render: (data, requestAssets) => {
        let requests = [];
        if (requestAssets) {
            let timestamp = directlink.nocache ? '?_=' + Date.now() : '';
            for (let name in data.Scripts) {
                if (directlink.scripts[name] !== data.Scripts[name]) {
                    requests.push(Utils.getScript(data.Scripts[name] + timestamp));
                }
            }
            for (let name in data.Styles) {
                if (directlink.styles[name] !== data.Styles[name]) {
                    requests.push(Utils.getStyle(data.Styles[name] + timestamp));
                }
            }
        }
        Promise.all(requests).then(() => {
            directlink.scripts = Object.assign(directlink.scripts, data.Scripts);
            directlink.styles = Object.assign(directlink.styles, data.Styles);
            directlink.data = data;
            document.title = data.Title;
            if (!directlink.rendered) {
                let render = directlink.container.childElementCount ? ReactDOM.hydrate : ReactDOM.render;
                if (directlink.hmr) {
                    directlink.instances.HMR = render(React.createElement(HMR, { component: data.App }), directlink.container, directlink.onRendered);
                } else {
                    render(React.createElement(components[data.App]), directlink.container, directlink.onRendered);
                }
            } else {
                for (let key in data.States) {
                    let instance = directlink.instances[key];
                    if (instance) {
                        instance.setState(data.States[key]);
                    }
                }
                if (directlink.forceUpdate) {
                    directlink.instances.HMR.forceUpdate();
                    delete directlink.forceUpdate;
                } else {
                    directlink.scrollIntoView(location.hash);
                }
            }
        });
    },

    onRendered: () => {
        directlink.rendered = true;
        directlink.linked = Promise['resolve']();
        directlink.scrollIntoView(location.hash);
    },

    init: instance => {
        instance.name = Utils.getClassName(instance);
        instance.scope = instance.props.scope || instance.context && instance.context.scope;
        if (instance.scope) {
            instance.fullname = instance.scope + '.' + instance.name;
        }
        if (instance.props.fullname) {
            instance.fullname = instance.props.fullname;
        }
        if (!instance.fullname) {
            instance.fullname = instance.name;
        }
        if (instance.props.id) {
            instance.fullname += '$' + instance.props.id;
        }

        instance.state = directlink.data.States[instance.fullname] || {};
        if (instance.name === directlink.data.App) {
            instance.state.online = true;
        }

        if (global.signalR) {
            directlink.instances[instance.fullname] = instance;

            instance.invoke = (method, args) => {
                let invocationId = Utils.getGuid();
                let promise = new DefferedPromise((resolve, reject) => {
                    directlink.online()
                        .then(() => directlink.linked)
                        .then(() => {
                            if (args && !Array.isArray(args)) {
                                args = [args];
                            }
                            directlink.hub.instance
                                .invoke("invoke", invocationId, instance.fullname, instance.name, method, args)
                                .catch(error => {
                                    delete directlink.promises[invocationId];
                                    reject(error);
                                });
                        })
                        .catch(error => reject(error));
                });
                directlink.promises[invocationId] = promise;
                return promise;
            };

            for (var i in directlink.data.Methods[instance.name]) {
                var method = directlink.data.Methods[instance.name][i];
                instance[method] = ((method) => (...args) => instance.invoke(method, [...args]))(method);
            }

            if (directlink.data.Bidirectional.indexOf(instance.name) !== -1) {
                instance.bidirectional = true;
                directlink.link(instance);
            }

            instance.updateId = (id) => {
                directlink.unlink(instance);
                delete directlink.instances[instance.fullname];
                if (instance.fullname.indexOf('$') > 0) {
                    instance.fullname = instance.fullname.replace('$' + instance.props.id, '$' + id);
                }
                directlink.instances[instance.fullname] = instance;
                directlink.link(instance);
            };
        }
    },

    link: instance => {
        let props = Object.assign({}, instance.props);
        delete props.scope;
        delete props.fullname;
        directlink.hub.instance.invoke("link", instance.fullname, instance.name, props);
    },

    unlink: instance => directlink.hub.instance.invoke("unlink", instance.fullname, instance.name),

    dispose: instance => {
        if (!directlink.hmr) {
            if (instance.bidirectional) {
                directlink.unlink(instance);
            }
            delete directlink.instances[instance.fullname];
        }
    },

    requestData: (path, referer) => directlink.hub.instance.invoke("requestData", path, referer),

    hub: {
        instance: null,
        connectionId: () => directlink.hub.instance && directlink.hub.instance.connection.connectionId,
        init: () => {
            let hub = new signalR.HubConnection('/directlink');

            hub.on("SetState", (fullname, state) => {
                if (directlink.instances.hasOwnProperty(fullname)) {
                    directlink.instances[fullname].setState(state);
                }
            });
            hub.on("Invoke", (fullname, method, args) => {
                if (directlink.instances.hasOwnProperty(fullname)) {
                    if (args) {
                        directlink.instances[fullname][method](...args);
                    } else {
                        directlink.instances[fullname][method]();
                    }
                }
            });
            hub.on("DataResponse", result => {
                if (result.Data) {
                    directlink.render(result.Data, true);
                    return;
                }
                throw result.StatusCode;
            });
            hub.on("InvokeResult", (invocationId, response) => {
                let promise = directlink.promises[invocationId];
                delete directlink.promises[invocationId];

                if (response && response.StatusCode === 200) {
                    if (response.Result !== undefined) {
                        promise.resolve(response.Result);
                    } else {
                        promise.resolve();
                    }
                    return;
                }
                promise.reject(response && response.StatusCode || 'unexpected result');
            });
            hub.on("AssetsUpdate", () => {
                directlink.forceUpdate = true;
                directlink.requestData(window.location.pathname);
            });

            let onClose = error => {
                if (error) {
                    console.error(error);
                }
                directlink.instances[directlink.data.App].setState({ online: false });
                directlink.hub.reconnect('closed');
            };
            if (hub.onclose) {
                hub.onclose(onClose);
            } else {
                hub.onClosed = onClose;
            }
            directlink.hub.instance = hub;
        },
        state: '',
        start: () => {
            directlink.hub.init();
            return directlink.hub.instance.start()
                .then(() => {
                    directlink.hub.reconnectCount = 0;
                    directlink.hub.state = 'connected';
                    if (directlink.rendered) {
                        directlink.linked = new Promise(resolve => {
                            for (let key in directlink.instances) {
                                let instance = directlink.instances[key];
                                if (instance.bidirectional) {
                                    directlink.link(instance);
                                }
                            }
                            resolve();
                        });
                    }
                    if (directlink.rendered) {
                        directlink.instances[directlink.data.App].setState({ online: true });
                    }
                })
                .catch(error => {
                    setTimeout(() => { directlink.hub.reconnect('failed'); }, 0);
                    throw error || 'start signalr connection failed';
                });
        },
        stop: () => directlink.hub.instance.connection.stop(),
        reconnect: state => {
            let hub = directlink.hub;
            hub.state = state;
            if (state === 'closed' || state === 'waited') {
                hub.state = 'reconnecting';
                hub.reconnectPromise = hub.start();
            }
            if (state === 'failed') {
                hub.state = 'waiting';
                let maxIndex = hub.reconnectTimeouts.length - 1;
                directlink.hub.reconnectTimer = setTimeout(() => {
                    if (hub.state === 'waiting') {
                        if (hub.reconnectCount < maxIndex) {
                            hub.reconnectCount++;
                        }
                        hub.reconnect('waited');
                    }
                }, hub.reconnectTimeouts[Math.min(hub.reconnectCount, maxIndex)]);
            }
        },
        reconnectPromise: null,
        reconnectTimer: null,
        reconnectTimeouts: [1000, 5000, 20000, 60000],
        reconnectCount: 0
    },

    online: () => new Promise((resolve, reject) => {
        let hub = directlink.hub;
        if (hub.instance.connection.connectionState === 2) {
            resolve();
            return;
        }
        if (hub.state === 'reconnecting') {
            hub.reconnectPromise
                .then(() => resolve())
                .catch(() => reject('not connected'));
        }
        if (hub.state === 'waiting') {
            clearTimeout(hub.reconnectTimer);
            hub.reconnect('waited');
            hub.reconnectPromise
                .then(() => resolve())
                .catch(() => reject('not connected'));
        }
        setTimeout(() => reject('not connected'), 10000);
    }),

    history: {
        current: { path: null, hash: null },
        pushState: (state, title, path) => {
            let hash = '';
            let idx = path.indexOf('#');
            if (idx === 0) {
                hash = path;
                if (directlink.history.current.hash !== hash) {
                    history.pushState(state, title, directlink.history.current.path + hash);
                }
                directlink.scrollIntoView(hash);
                return;
            }
            if (!path.startsWith('/')) {
                path = '/' + path;
                if (idx > 0) idx++;
            }
            if (idx > 0) {
                hash = path.substring(idx);
                path = path.substring(0, idx);
                if (directlink.history.current.path === path) {
                    if (directlink.history.current.hash !== hash) {
                        history.pushState(state, title, path + hash);
                    }
                    directlink.scrollIntoView(hash);
                    return;
                }
            }
            history.pushState(state, title, path + hash);
            directlink.history.dispatch(path, hash);
        },
        popState: () => {
            if (directlink.history.current.path === location.pathname) {
                directlink.scrollIntoView(location.hash);
            } else {
                directlink.history.dispatch(location.pathname, location.hash);
            }
        },
        dispatch: (path, hash) => {
            let referer = directlink.history.current.path;
            directlink.history.current = { path, hash };
            directlink.online()
                .then(() => directlink.requestData(path + hash, referer))
                .catch(error => console.error(error));
        },
    },

    scrollIntoView: hash => {
        if (hash) {
            try {
                let element = document.querySelector(hash);
                if (element !== null) {
                    element.scrollIntoView({ block: 'start', behavior: 'smooth' });
                }
            } catch (error) { console.log(error); }
            return;
        }
        window.scrollTo(window.scrollX, 0);
    }
};

let Link = props => {
    if (!props.href) {
        console.error("Link requires 'href' property");
    }
    return React.createElement(
        "a", {
            style: props.style,
            className: props.className,
            href: props.href,
            onClick: event => {
                event.preventDefault();
                if (typeof props.onClick === "function") {
                    props.onClick(event);
                }
                directlink.history.pushState({}, "", props.href);
            }
        },
        props.children
    );
};

let Render = props => {
    if (!props.component) {
        console.error("Render requires 'component' property");
    }
    let _props = Object.assign({}, props);
    delete _props.component;
    _props.fullname = props.component && props.component.FullName;
    return props.component && React.createElement(components[props.component.Name], _props);
};

let HMR = class HMR extends React.Component {
    render() {
        if (!this.props.component) {
            console.error("HMR requires 'component' property");
        }
        return this.props.component && React.createElement(components[this.props.component]);
    }
};

let Scope = props => {
    if (!props.name) {
        console.error("Scope requires 'name' property");
    }
    return React.Children.map(props.children, child => {
        if (React.isValidElement(child)) {
            return React.cloneElement(child, { scope: props.name });
        }
        return child;
    });
};

module.exports = {
    components: {},
    directlink: directlink,
    Link: Link,
    Render: Render,
    HMR: HMR,
    Scope: Scope
};