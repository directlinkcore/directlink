let path = require('path');

module.exports = {
    mode: 'development',
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
    }
};