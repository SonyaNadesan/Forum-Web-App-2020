export class HiddenField extends React.Component {

    render() {
        return (
            <input type="hidden" name={this.props.keyOfKeyValuePair} value={this.props.valueOfKeyValuePair} />
        );
    }

}