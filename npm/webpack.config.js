'use strict';
let webpack = require('webpack');
let path = require('path');

module.exports = [{
    entry: {
        'directlink-react': 'directlink-react.js',
        'directlink-react.min': 'directlink-react.js',
    },
    output: {
        libraryTarget: 'window',
        path: path.join(__dirname, 'dist/browser'),
        filename: '[name].js'
    },
    resolve: {
        modules: ['src', 'node_modules']
    },
    module: {
        loaders: [{
            test: /\.jsx?$/,
            exclude: /node_modules/,
            loader: 'babel-loader',
            query: { presets: ['es2015'] }
        }]
    },
    plugins: [
        new webpack.optimize.UglifyJsPlugin({ include: /\.min\.js$/ })
    ]
}];