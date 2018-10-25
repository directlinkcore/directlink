'use strict';

let path = require('path');
let AssetsPlugin = require('assets-webpack-plugin');
let WebpackCleanupPlugin = require('webpack-cleanup-plugin');

module.exports = {
    mode: 'development',
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
        rules: [{
            test: /\.jsx?$/,
            exclude: /node_modules/,
            use: {
                loader: 'babel-loader',
                options: {
                    presets: ['@babel/preset-env', '@babel/preset-react'],
                    plugins: ['@babel/plugin-proposal-class-properties']
                }
            }
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
};