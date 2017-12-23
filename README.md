# [DirectLink](https://directlink.info/)

Library for building single-page web applications that makes the world easier for [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/) developers who use [React](https://reactjs.org).

## Features

- Server-side rendering
- Real-time communications:
    - invoke .NET methods from React components
    - set state of components from .NET world
- Flexible server-side routing

## Installation

Create `ASP.NET Core` project and add package:
```sh
dotnet new web -o Sample && cd Sample
dotnet add package DirectLink.Core.React
```

Initialize npm and install packages:
```sh
npm init -y
npm install directlink-react
npm install -D babel-core babel-loader webpack
npm install -D babel-preset-es2015 babel-preset-react babel-plugin-transform-class-properties
```

<details>
    <summary><code>package.json</code></summary>

```json
{
    "name": "sample",
    "version": "1.0.0",
    "description": "",
    "scripts": {
        "build": "webpack"
    },
    "keywords": [],
    "author": "",
    "license": "ISC",
    "dependencies": {
        "directlink-react": "1.0.1"
    },
    "devDependencies": {
        "babel-core": "6.26.0",
        "babel-loader": "7.1.2",
        "babel-plugin-transform-class-properties": "6.24.1",
        "babel-preset-es2015": "6.24.1",
        "babel-preset-react": "6.24.1",
        "webpack": "3.10.0"
    }
}
```
</details>

<details>
    <summary>add <code>webpack.config.js</code></summary>

```js
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
```
</details>
<br>

Add service and configure pipeline in `Startup.cs`:
```csharp
services.AddDirectLink<App>(tags => tags.AddDefaultTemplateTags(title: "Sample"));
```
```csharp
app.UseDirectLink(components => components.Map<App>(script: "/dist/app.js"));
```

<details>
    <summary><code>Startup.cs</code></summary>

```csharp
using DirectLinkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Sample
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDirectLink<App>(tags => tags.AddDefaultTemplateTags(title: "Sample"));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseDirectLink(components => components.Map<App>(script: "/dist/app.js"));
        }
    }
}
```
</details>

## Usage

Add `app.jsx` to `clientApp` folder:
```js
components.App = class App extends React.Component {
    constructor(props) {
        super(props);
        directlink.init(this);
    }

    componentWillUnmount() {
        directlink.dispose(this);
    }

    sendMessage = () => {
        this.AddMessage(this.state.message) //here we invoke App.AddMessage method
            .then(() => this.setState({ message: '' }));
    }

    onChange = (event) => {
        this.setState({ [event.target.name]: event.target.value });
    }

    onKeyPress = (event) => {
        if (event.key === 'Enter') {
            this.sendMessage();
        }
    }

    render() {
        return (
            <div className="jumbotron">
                <div className="row mb-3">
                    <div className="col-12 col-sm-8 col-md-9 col-lg-10 mb-3 mb-sm-0">
                        <input type="text" className="form-control" name="message"
                            placeholder="message" autoComplete="off"
                            value={this.state.message} onChange={this.onChange} onKeyPress={this.onKeyPress} />
                    </div>
                    <div className="col-12 col-sm-4 col-md-3 col-lg-2">
                        <button type="button" className="btn btn-primary w-100"
                            onClick={this.sendMessage}>Send</button>
                    </div>
                </div>
                <ul className="list-unstyled">
                    {this.state.Messages.slice().reverse().map(message =>
                        <li key={message.Id}>
                            <div className='alert alert-primary'>{message.Text}</div>
                        </li>)}
                </ul>
            </div>
        );
    }
};
```

Add `Message.cs`:
```csharp
using System;

namespace Sample
{
    public class Message
    {
        public Guid Id { get; }

        public string Text { get; }

        public Message(string text) => (Id, Text) = (Guid.NewGuid(), text);
    }
}
```

Add `App.cs`:
```csharp
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using DirectLinkCore;

namespace Sample
{
    public class App : DirectLinkDispatcher<AppViewModel>
    {
        private static ConcurrentQueue<Message> _messages = new ConcurrentQueue<Message>();

        public IReadOnlyCollection<Message> GetMessages() => _messages;

        public async Task AddMessage(string text)
        {
            _messages.Enqueue(new Message(text));
            await SetStateAsync(new { Messages = _messages }); //here we setState of App component
        }
    }
}
```

Add `AppViewModel.cs`:
```csharp
using System.Collections.Generic;
using DirectLinkCore;

namespace Sample
{
    public class AppViewModel : ViewModel
    {
        public IReadOnlyCollection<Message> Messages { get; }

        public AppViewModel(App app) => Messages = app.GetMessages();
    }
}
```

Build client and run project:
```sh
npm run build
dotnet run
```

Open http://localhost:5000 in several tabs and check that all clients can text each other.

## Documentation

See [live samples](https://directlink.info/samples) and [documentation](https://directlink.info/docs) for more details.

## License

Apache 2.0