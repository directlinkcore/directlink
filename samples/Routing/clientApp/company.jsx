import 'product.jsx';

components.Company = class Company extends React.Component {
    constructor(props) {
        super(props);
        directlink.init(this);
    }

    componentWillUnmount() {
        directlink.dispose(this);
    }

    render() {
        return (
            <div className="card">
                <div className="card-header">
                    Company {this.state.BackLink && <Link href={this.state.BackLink.Href}>{this.state.BackLink.Name}</Link>}
                </div>
                <div className="card-body">
                    <div className="mb-3">
                        Id = <div className="badge badge-primary mr-3">{this.state.Id || 'null'}</div>
                        Name = <div className="badge badge-info mr-3">{this.state.Name || 'null'}</div>
                        View = <div className="badge badge-secondary mr-3">{this.state.View || 'null'}</div>
                    </div>
                    {this.state.Name && <i className={`text-primary fab fa-${this.state.Name.toLowerCase()} fa-3x mb-3`}></i>}
                    <div className="row">
                        {this.state.Companies && <div className="col-12 col-md-6 mb-3">
                            <div className="card">
                                <div className="card-header">
                                    Companies
                                </div>
                                <ul className="list-group list-group-flush">
                                    {this.state.Companies.map(link =>
                                        <li className="list-group-item" key={link.Name}>
                                            <Link href={link.Href} className="nav-link">{link.Name}</Link>
                                        </li>)}
                                </ul>
                            </div>
                        </div>}
                        {this.state.Products && <div className="col-12 col-md-6 mb-3">
                            <div className="card">
                                <div className="card-header">
                                    Products
                                </div>
                                <ul className="list-group list-group-flush">
                                    {this.state.Products.map(link =>
                                        <li className="list-group-item" key={link.Name}>
                                            <Link href={link.Href} className="nav-link">{link.Name}</Link>
                                        </li>)}
                                </ul>
                            </div>
                        </div>}
                    </div>
                    <Render component={this.state.Component} />
                </div>
            </div>
        );
    }
};

components.CompanyHome = class CompanyHome extends React.Component {
    constructor(props) {
        super(props);
        directlink.init(this);
    }

    componentWillUnmount() {
        directlink.dispose(this);
    }

    render() {
        return (
            <div className="badge badge-secondary">
                {this.state.Path}
            </div>
        );
    }
};