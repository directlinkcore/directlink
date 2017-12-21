module.exports = (callback, index, hmr) => {
    let _ = require('lodash');
    renderIndexHtml = _.template(index);
    React = require('react');
    ReactDOM = require('react-dom');
    ReactDOMServer = require('react-dom/server');
    Object.assign(global, require('directlink-react'));
    if (hmr) {
        directlink.hmr = true;
    }
    callback(null, 'warmed up');
};