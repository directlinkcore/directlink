let path = require('path');

module.exports = [{
    entry: { app: 'app.jsx' },
    output: {
        path: path.join(__dirname, 'wwwroot/dist'),
        filename: '[name].js'
    },
    resolve: {
        modules: ['clientApp', 'node_modules']
    },
    externals: {
        'react': 'React',
        'react-dom': 'ReactDOM'
    },
    module: {
        loaders: [{
            test: /\.jsx?$/,
            exclude: /node_modules/,
            loader: 'babel-loader',
            query: { presets: ['es2015', 'react'], plugins: ['transform-class-properties'] }
        }],
    }
}];