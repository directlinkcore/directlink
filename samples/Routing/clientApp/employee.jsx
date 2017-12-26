components.Employee = class Employee extends React.Component {
    constructor(props) {
        super(props);
        directlink.init(this);
        this.state.links = [
            { name: 'Jennifer', href: 'employee/1' },
            { name: 'John', href: 'employee/John' },
            { name: 'Alice', href: 'employee/3' },
            { name: 'Bill', href: 'employee/Bill' },
            { name: 'Angela', href: 'employee/42' }
        ]
    }

    componentWillUnmount() {
        directlink.dispose(this);
    }

    render() {
        return (
            <div className="card">
                <div className="card-header d-inline">
                    Employee {this.state.args && <Link href={''}>back</Link>}
                </div>
                <div className="card-body">
                    {this.state.args && <div>
                        <div className="mb-3">
                            id = <div className="badge badge-info mr-3">{this.state.args.id || 'null'}</div>
                            name = <div className="badge badge-info">{this.state.args.name || 'null'}</div>
                        </div>
                        <img src={`/img/employees/${this.state.args.id || this.state.args.name}.png`}
                            className="img-thumbnail rounded" style={{ maxWidth: 200 }} />
                    </div>}
                    {!this.state.args && this.state.links
                        .map(link => <Link key={link.name} href={link.href} className="nav-link">{link.name}</Link>)
                    }
                </div>
            </div>
        );
    }
};