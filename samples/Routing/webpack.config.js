'use strict';

let path = require('path');
let webpack = require('webpack');
let AssetsPlugin = require('assets-webpack-plugin');
let WebpackCleanupPlugin = require('webpack-cleanup-plugin');

module.exports = [{
    entry: {
        routing: 'routing.jsx',
        company: 'company.jsx',
        employee: 'employee.jsx',
    },
    output: {
        path: path.join(__dirname, 'wwwroot', 'dist'),
        publicPath: "/dist/",
        filename: '[name].[chunkhash].js'
    },
    resolve: { modules: ['clientApp', 'node_modules'] },
    externals: { 'react': 'React', 'react-dom': 'ReactDOM' },
    module: {
        loaders: [{
            test: /\.jsx?$/,
            exclude: /node_modules/,
            loader: 'babel-loader',
            query: { presets: ['es2015', 'react'], plugins: ['transform-class-properties'] }
        }]
    },
    plugins: [
        new WebpackCleanupPlugin(),
        new AssetsPlugin({
            path: __dirname,
            filename: 'assets.json',
            prettyPrint: true,
            update: true
        })
    ]
}];