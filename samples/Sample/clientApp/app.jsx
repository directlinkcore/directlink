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