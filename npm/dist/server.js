// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

module.exports = (callback, wwwroot, data, contextData, cookies) => {
    getCookie = cookie => cookies[cookie];
    let dataJson = JSON.stringify(data);
    directlink.data = data;
    for (let name in data.Scripts) {
        if (!components.hasOwnProperty(name)) {
            require(wwwroot + data.Scripts[name]);
        }
    }
    let app = directlink.hmr ? React.createElement(HMR, { component: data.App }) : React.createElement(components[data.App]);
    let content = ReactDOMServer.renderToStaticMarkup(
        React.createElement('div', {
            id: 'content',
            dangerouslySetInnerHTML: {
                __html: ReactDOMServer.renderToString(app)
            }
        })
    );
    let html = renderIndexHtml(Object.assign({ content, data: dataJson }, contextData));
    callback(null, html);
    directlink.data = null;
};