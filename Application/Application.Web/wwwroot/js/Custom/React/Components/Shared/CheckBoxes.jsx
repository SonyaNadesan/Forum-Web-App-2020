export class CheckBoxes extends React.Component{
    constructor(props) {
        super(props);
    }

    render() {
        let optionsAsHtml = [];

        if (this.props.selectedOptions != null) {
            this.props.options.map((option, index) => {
                optionsAsHtml.push(
                    <label className="checkbox-inline" key={"label_" + this.props.label + "_" + index}>
                        {option.displayName}
                        <input type="checkbox" id={this.props.label + "_" + option.nameInUrl} name={"checkbox_" + this.props.label}
                            value={option.nameInUrl} checked={this.props.selectedOptions.includes(option.nameInUrl)}
                            onChange={this.props.checkBoxChangeHandler} text={option.displayName}>
                        </input>
                    </label>
                );
            });
        }

        return (
            <div className="form-group">
                <label>{this.props.label}</label>
                {optionsAsHtml}
                <input type="hidden" name={this.props.label} id={this.props.label} />
            </div>
        );
    }
}