components.Home = class Home extends React.Component {
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
                    Home
                </div>
                <div className="card-body">
                    Navigate to <Link href="/companies">Companies</Link> or <Link href="/employee">Employees</Link>
                </div>
            </div>
        );
    }
};