(function(e, a) { for(var i in a) e[i] = a[i]; }(window, /******/ (function(modules) { // webpackBootstrap
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/
/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId]) {
/******/ 			return installedModules[moduleId].exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			i: moduleId,
/******/ 			l: false,
/******/ 			exports: {}
/******/ 		};
/******/
/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/
/******/ 		// Flag the module as loaded
/******/ 		module.l = true;
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// define getter function for harmony exports
/******/ 	__webpack_require__.d = function(exports, name, getter) {
/******/ 		if(!__webpack_require__.o(exports, name)) {
/******/ 			Object.defineProperty(exports, name, {
/******/ 				configurable: false,
/******/ 				enumerable: true,
/******/ 				get: getter
/******/ 			});
/******/ 		}
/******/ 	};
/******/
/******/ 	// getDefaultExport function for compatibility with non-harmony modules
/******/ 	__webpack_require__.n = function(module) {
/******/ 		var getter = module && module.__esModule ?
/******/ 			function getDefault() { return module['default']; } :
/******/ 			function getModuleExports() { return module; };
/******/ 		__webpack_require__.d(getter, 'a', getter);
/******/ 		return getter;
/******/ 	};
/******/
/******/ 	// Object.prototype.hasOwnProperty.call
/******/ 	__webpack_require__.o = function(object, property) { return Object.prototype.hasOwnProperty.call(object, property); };
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "";
/******/
/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(__webpack_require__.s = 0);
/******/ })
/************************************************************************/
/******/ ([
/* 0 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(global) {

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

function _toConsumableArray(arr) { if (Array.isArray(arr)) { for (var i = 0, arr2 = Array(arr.length); i < arr.length; i++) { arr2[i] = arr[i]; } return arr2; } else { return Array.from(arr); } }

// Copyright (c) 2018 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

/*global React, ReactDOM, components, signalR*/
var Utils = __webpack_require__(2);
var DefferedPromise = Utils.DeferredPromise;

var directlink = {
    data: {},
    instances: {},
    scripts: {},
    styles: {},
    promises: {},
    nocache: false,
    hmr: false,

    renderApp: function renderApp(container, data) {
        directlink.container = container;
        directlink.hub.start().then(function () {
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
        }).catch(function (error) {
            container.innerHTML = '';
            throw error || 'start signalr connection failed';
        });
    },

    render: function render(data, requestAssets) {
        var requests = [];
        if (requestAssets) {
            var timestamp = directlink.nocache ? '?_=' + Date.now() : '';
            for (var name in data.Scripts) {
                if (directlink.scripts[name] !== data.Scripts[name]) {
                    requests.push(Utils.getScript(data.Scripts[name] + timestamp));
                }
            }
            for (var _name in data.Styles) {
                if (directlink.styles[_name] !== data.Styles[_name]) {
                    requests.push(Utils.getStyle(data.Styles[_name] + timestamp));
                }
            }
        }
        Promise.all(requests).then(function () {
            directlink.scripts = Object.assign(directlink.scripts, data.Scripts);
            directlink.styles = Object.assign(directlink.styles, data.Styles);
            directlink.data = data;
            document.title = data.Title;
            if (!directlink.rendered) {
                var render = directlink.container.childElementCount ? ReactDOM.hydrate : ReactDOM.render;
                if (directlink.hmr) {
                    directlink.instances.HMR = render(React.createElement(HMR, { component: data.App }), directlink.container, directlink.onRendered);
                } else {
                    render(React.createElement(components[data.App]), directlink.container, directlink.onRendered);
                }
            } else {
                for (var key in data.States) {
                    var instance = directlink.instances[key];
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

    onRendered: function onRendered() {
        directlink.rendered = true;
        directlink.linked = Promise['resolve']();
        directlink.scrollIntoView(location.hash);
    },

    init: function init(instance) {
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

            instance.invoke = function (method, args) {
                var invocationId = Utils.getGuid();
                var promise = new DefferedPromise(function (resolve, reject) {
                    directlink.online().then(function () {
                        return directlink.linked;
                    }).then(function () {
                        if (args && !Array.isArray(args)) {
                            args = [args];
                        }
                        directlink.hub.instance.invoke("invoke", invocationId, instance.fullname, instance.name, method, args).catch(function (error) {
                            delete directlink.promises[invocationId];
                            reject(error);
                        });
                    }).catch(function (error) {
                        return reject(error);
                    });
                });
                directlink.promises[invocationId] = promise;
                return promise;
            };

            for (var i in directlink.data.Methods[instance.name]) {
                var method = directlink.data.Methods[instance.name][i];
                instance[method] = function (method) {
                    return function () {
                        for (var _len = arguments.length, args = Array(_len), _key = 0; _key < _len; _key++) {
                            args[_key] = arguments[_key];
                        }

                        return instance.invoke(method, [].concat(args));
                    };
                }(method);
            }

            if (directlink.data.Bidirectional.indexOf(instance.name) !== -1) {
                instance.bidirectional = true;
                directlink.link(instance);
            }

            instance.updateId = function (id) {
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

    link: function link(instance) {
        var props = Object.assign({}, instance.props);
        delete props.scope;
        delete props.fullname;
        directlink.hub.instance.invoke("link", instance.fullname, instance.name, props);
    },

    unlink: function unlink(instance) {
        return directlink.hub.instance.invoke("unlink", instance.fullname, instance.name);
    },

    dispose: function dispose(instance) {
        if (!directlink.hmr) {
            if (instance.bidirectional) {
                directlink.unlink(instance);
            }
            delete directlink.instances[instance.fullname];
        }
    },

    requestData: function requestData(path, referer) {
        return directlink.hub.instance.invoke("requestData", path, referer);
    },

    hub: {
        instance: null,
        connectionId: function connectionId() {
            return directlink.hub.instance && directlink.hub.instance.connection.connectionId;
        },
        init: function init() {
            var hub = new signalR.HubConnectionBuilder().withUrl("/directlink").configureLogging(signalR.LogLevel.Information).build();

            hub.on("SetState", function (fullname, state) {
                if (directlink.instances.hasOwnProperty(fullname)) {
                    directlink.instances[fullname].setState(state);
                }
            });
            hub.on("Invoke", function (fullname, method, args) {
                if (directlink.instances.hasOwnProperty(fullname)) {
                    if (args) {
                        var _directlink$instances;

                        (_directlink$instances = directlink.instances[fullname])[method].apply(_directlink$instances, _toConsumableArray(args));
                    } else {
                        directlink.instances[fullname][method]();
                    }
                }
            });
            hub.on("DataResponse", function (result) {
                if (result.Data) {
                    directlink.render(result.Data, true);
                    return;
                }
                throw 'status code ' + result.StatusCode;
            });
            hub.on("InvokeResult", function (invocationId, response) {
                var promise = directlink.promises[invocationId];
                delete directlink.promises[invocationId];

                if (response && response.StatusCode === 200) {
                    if (response.Result !== undefined) {
                        promise.resolve(response.Result);
                    } else {
                        promise.resolve();
                    }
                    return;
                }
                promise.reject(response && response.StatusCode ? 'status code ' + response.StatusCode : 'unexpected result');
            });
            hub.on("AssetsUpdate", function () {
                if (directlink.hmr) {
                    directlink.forceUpdate = true;
                    directlink.requestData(window.location.pathname);
                }
            });

            var onClose = function onClose(error) {
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
        start: function start() {
            directlink.hub.init();
            return directlink.hub.instance.start().then(function () {
                directlink.hub.reconnectCount = 0;
                directlink.hub.state = 'connected';
                if (directlink.rendered) {
                    directlink.linked = new Promise(function (resolve) {
                        for (var key in directlink.instances) {
                            var instance = directlink.instances[key];
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
            }).catch(function (error) {
                setTimeout(function () {
                    directlink.hub.reconnect('failed');
                }, 0);
                throw error || 'start signalr connection failed';
            });
        },
        stop: function stop() {
            return directlink.hub.instance.connection.stop();
        },
        reconnect: function reconnect(state) {
            var hub = directlink.hub;
            hub.state = state;
            if (state === 'closed' || state === 'waited') {
                hub.state = 'reconnecting';
                hub.reconnectPromise = hub.start();
            }
            if (state === 'failed') {
                hub.state = 'waiting';
                var maxIndex = hub.reconnectTimeouts.length - 1;
                directlink.hub.reconnectTimer = setTimeout(function () {
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

    online: function online() {
        return new Promise(function (resolve, reject) {
            var hub = directlink.hub;
            if (hub.instance.connection.connectionState === 1) {
                resolve();
                return;
            }
            if (hub.state === 'reconnecting') {
                hub.reconnectPromise.then(function () {
                    return resolve();
                }).catch(function () {
                    return reject('not connected');
                });
            }
            if (hub.state === 'waiting') {
                clearTimeout(hub.reconnectTimer);
                hub.reconnect('waited');
                hub.reconnectPromise.then(function () {
                    return resolve();
                }).catch(function () {
                    return reject('not connected');
                });
            }
            setTimeout(function () {
                return reject('not connected');
            }, 10000);
        });
    },

    history: {
        current: { path: null, hash: null },
        pushState: function pushState(state, title, path) {
            var hash = '';
            var idx = path.indexOf('#');
            if (idx === 0) {
                hash = path;
                if (directlink.history.current.hash !== hash) {
                    history.pushState(state, title, directlink.history.current.path + hash);
                }
                directlink.scrollIntoView(hash);
                return;
            }
            if (!path.startsWith('/')) {
                var current = directlink.history.current.path;
                var base = current.substring(0, current.lastIndexOf('/') + (path.length > 0 ? 1 : 0));
                path = base + path;
                if (idx > 0) {
                    idx += base.length;
                }
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
        popState: function popState() {
            if (directlink.history.current.path === location.pathname) {
                directlink.scrollIntoView(location.hash);
            } else {
                directlink.history.dispatch(location.pathname, location.hash);
            }
        },
        dispatch: function dispatch(path, hash) {
            var referer = directlink.history.current.path;
            directlink.history.current = { path: path, hash: hash };
            directlink.online().then(function () {
                return directlink.requestData(path + hash, referer);
            }).catch(function (error) {
                return console.error(error);
            });
        }
    },

    scrollIntoView: function scrollIntoView(hash) {
        if (hash) {
            try {
                var element = document.querySelector(hash);
                if (element !== null) {
                    element.scrollIntoView({ block: 'start', behavior: 'smooth' });
                }
            } catch (error) {
                console.log(error);
            }
            return;
        }
        window.scrollTo(window.scrollX, 0);
    }
};

var Link = function Link(props) {
    return React.createElement("a", {
        style: props.style,
        className: props.className,
        href: props.href,
        onClick: function onClick(event) {
            event.preventDefault();
            if (typeof props.onClick === "function") {
                props.onClick(event);
            }
            directlink.history.pushState({}, "", props.href);
        }
    }, props.children);
};

var Render = function Render(props) {
    if (!props.component) {
        console.error("Render requires 'component' property");
    }
    var _props = Object.assign({}, props);
    delete _props.component;
    _props.fullname = props.component && props.component.FullName;
    return props.component && React.createElement(components[props.component.Name], _props);
};

var HMR = function (_React$Component) {
    _inherits(HMR, _React$Component);

    function HMR() {
        _classCallCheck(this, HMR);

        return _possibleConstructorReturn(this, (HMR.__proto__ || Object.getPrototypeOf(HMR)).apply(this, arguments));
    }

    _createClass(HMR, [{
        key: 'render',
        value: function render() {
            if (!this.props.component) {
                console.error("HMR requires 'component' property");
            }
            return this.props.component && React.createElement(components[this.props.component]);
        }
    }]);

    return HMR;
}(React.Component);

var Scope = function Scope(props) {
    if (!props.name) {
        console.error("Scope requires 'name' property");
    }
    return React.Children.map(props.children, function (child) {
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
/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))

/***/ }),
/* 1 */
/***/ (function(module, exports) {

var g;

// This works in non-strict mode
g = (function() {
	return this;
})();

try {
	// This works if eval is allowed (see CSP)
	g = g || Function("return this")() || (1,eval)("this");
} catch(e) {
	// This works if the window reference is available
	if(typeof window === "object")
		g = window;
}

// g can still be undefined, but nothing to do about it...
// We return undefined, instead of nothing here, so it's
// easier to handle this case. if(!global) { ...}

module.exports = g;


/***/ }),
/* 2 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

var chars = '0123456789abcdef'.split('');

var getGuid = function getGuid() {
    var uuid = [],
        rnd = Math.random,
        r = void 0;
    uuid[8] = uuid[13] = uuid[18] = uuid[23] = '-';
    uuid[14] = '4';

    for (var i = 0; i < 36; i++) {
        if (!uuid[i]) {
            r = 0 | rnd() * 16;
            uuid[i] = chars[i == 19 ? r & 0x3 | 0x8 : r & 0xf];
        }
    }

    return uuid.join('');
};

var getScript = function getScript(url) {
    return new Promise(function (resolve, reject) {
        var script = document.createElement('script');
        script.type = 'text/javascript';
        script.src = url;
        script.addEventListener('load', function () {
            return resolve(script);
        }, false);
        script.addEventListener('error', function () {
            return reject(script);
        }, false);
        document.body.appendChild(script);
    });
};

var getStyle = function getStyle(url) {
    return new Promise(function (resolve, reject) {
        var link = document.createElement('link');
        link.rel = 'stylesheet';
        link.href = url;
        link.addEventListener('load', function () {
            return resolve(link);
        }, false);
        link.addEventListener('error', function () {
            return reject(link);
        }, false);
        document.body.appendChild(link);
    });
};

var getClassName = function getClassName(instance) {
    if (instance.constructor.name) {
        return instance.constructor.name;
    }
    var regex = new RegExp(/^\s*function\s*(\S*)\s*\(/);
    var getClassName = function getClassName(instance) {
        return instance.constructor.toString().match(regex)[1];
    };
    return getClassName(instance);
};

var DeferredPromise = function DeferredPromise(executor) {
    var _this = this;

    _classCallCheck(this, DeferredPromise);

    this._promise = new Promise(function (resolve, reject) {
        _this.resolve = resolve;
        _this.reject = reject;
        executor(resolve, reject);
    });
    this.then = this._promise.then.bind(this._promise);
    this.catch = this._promise.catch.bind(this._promise);
};

module.exports = {
    getGuid: getGuid,
    getScript: getScript,
    getStyle: getStyle,
    getClassName: getClassName,
    DeferredPromise: DeferredPromise
};

/***/ })
/******/ ])));