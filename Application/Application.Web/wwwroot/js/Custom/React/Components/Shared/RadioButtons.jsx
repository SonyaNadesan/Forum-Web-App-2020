export class RadioButtons extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        let optionsAsHtml = [];

        optionsAsHtml.push(
            <label className="checkbox-inline" key={"label_" + this.props.label + "_all"}>
                All
                <input type="radio" id={this.props.label + "_all"} name={this.props.label} value="all"
                    checked={this.props.selectedOption == "all"}
                    onChange={this.props.radioButtonChangeHandler} text="All">
                </input>
            </label>
        );

        this.props.options.map((option, index) => {
            optionsAsHtml.push(
                <label className="checkbox-inline" key={"label_" + this.props.label + "_option_" + index}>
                    {option.displayName}
                    <input type="radio" id={this.props.label + "_" + option.nameInUrl}
                        name="topic" value={option.nameInUrl} checked={option.nameInUrl == this.props.selectedOption}
                        onChange={this.props.radioButtonChangeHandler} text={option.displayName}>
                    </input>
                </label>
            );
        });

        return (
            <div className="form-group">
                <label>{this.props.label}</label>
                {optionsAsHtml}
            </div>
        );
    }
}