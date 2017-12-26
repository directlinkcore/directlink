import 'home.jsx';

components.Routing = class Routing extends React.Component {
    constructor(props) {
        super(props);
        directlink.init(this);
    }

    componentWillUnmount() {
        directlink.dispose(this);
    }

    render() {
        return (
            <div>
                <nav className="navbar navbar-expand-lg navbar-light bg-light">
                    <Link className="navbar-brand" href="/">Routing</Link>
                    <button className="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                        <span className="navbar-toggler-icon"></span>
                    </button>

                    <div className="collapse navbar-collapse" id="navbarSupportedContent">
                        <ul className="navbar-nav mr-auto">
                            {this.state.Links.map(link =>
                                <li key={link.Name} className="nav-item">
                                    <Link href={link.Href} className="nav-link">{link.Name}</Link>
                                </li>
                            )}
                        </ul>
                    </div>
                </nav>
                <div className="jumbotron">
                    <Render component={this.state.Component} />
                </div>
            </div>
        );
    }
};