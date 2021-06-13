import { CheckBoxes } from "../Shared/CheckBoxes.jsx";
import { RadioButtons } from "../Shared/RadioButtons.jsx";

export class ForumIndexFilterBlock extends React.Component {

    render() {
        return (
            <div className="container">
                <div>
                    <RadioButtons
                        selectedOption={this.props.topic}
                        radioButtonChangeHandler={this.props.topicChangeHandler}
                        options={this.props.topicOptions}
                        label="Topic"
                    />
                    <CheckBoxes
                        selectedOptions={this.props.categories}
                        options={this.props.categoryOptions}
                        checkBoxChangeHandler={this.props.categoryChangeHandler}
                        label="Categories"
                    />
                </div>
                <button onClick={this.props.onClick} className="btn btn-default">Submit</button>
            </div>
        );
    }

}