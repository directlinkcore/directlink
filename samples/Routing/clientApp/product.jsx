components.Product = class Product extends React.Component {
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
                    product id = <span className="badge badge-primary">{this.state.args.id}</span>
                </div>
                <div className="card-body">
                    <img src={`/img/products/${this.state.args.id}.png`} />
                </div>
            </div>
        );
    }
};