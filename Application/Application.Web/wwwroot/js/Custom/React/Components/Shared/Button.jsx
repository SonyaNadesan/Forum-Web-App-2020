export class CreateButton extends React.Component {

    render() {
        return (
            <button onClick={this.props.onClick} className={this.props.cssClass} value={this.props.value}>
                <span className={this.props.cssClassForIcon}></span> {this.props.displayText}
            </button>
        );
    }

}

export class RemoveButton extends React.Component {

    render() {
        return (
            <button onClick={this.props.onClick} className={this.props.cssClass} value={this.props.value}>
                <span className={this.props.cssClassForIcon}></span> {this.props.displayText}
            </button>
        );
    }

}

export class DeleteButton extends React.Component {

    render() {
        return (
            <button onClick={this.props.onClick} className={this.props.cssClass} value={this.props.value}>
                <span className={this.props.cssClassForIcon}></span> {this.props.displayText}
            </button>
        );
    }

}

export class ViewButton extends React.Component {

    render() {
        return (
            <button onClick={this.props.onClick} className={this.props.cssClass} value={this.props.value}>
                <span className={this.props.cssClassForIcon}></span> {this.props.displayText}
            </button>
        );
    }

}

export class EditButton extends React.Component {

    render() {
        return (
            <button onClick={this.props.onClick} className={this.props.cssClass} value={this.props.value}>
                <span className={this.props.cssClassForIcon}></span> {this.props.displayText}
            </button>
        );
    }

}

export class LoadMoreButton extends React.Component {

    render() {
        let loadMoreDisplayCss = this.props.hasMore ? "block" : "none";

        return (
            <div>
                <input type="hidden" id={"hasMoreFrom_" + this.props.id} value={this.props.from} />
                <input type="hidden" id={"excludeIdsFor_" + this.props.id} values={this.props.excludeIds} />
                <input type="hidden" id={"hasMoreTake_" + this.props.id} value={this.props.take} />
                <button type="button" style={{ display: loadMoreDisplayCss}} name="btnLoadMore" id={"btnLoadMore_" + this.props.id} value={this.props.id} onClick={this.props.loadMoreHandler}>Load More</button>
            </div>
        );
    }

}

CreateButton.defaultProps = {
    displayText: 'Create',
    cssClass: 'btn btn-link',
    cssClassForIcon: 'glyphicon glyphicon-plus',
    onClick: function () {
        alert("No event has been attached.");
    }
};

DeleteButton.defaultProps = {
    displayText: 'Delete',
    cssClass: 'btn btn-link',
    cssClassForIcon: 'glyphicon glyphicon-trash',
    onClick: function () {
        alert("No event has been attached.");
    }
};

ViewButton.defaultProps = {
    displayText: 'View',
    cssClass: 'btn btn-link',
    cssClassForIcon: 'glyphicon glyphicon-eye-open',
    onClick: function () {
        alert("No event has been attached.");
    }
};

EditButton.defaultProps = {
    displayText: 'Edit',
    cssClass: 'btn btn-link',
    cssClassForIcon: 'glyphicon glyphicon-edit',
    onClick: function () {
        alert("No event has been attached.");
    }
};

//ReactDOM.render(
//    <DeleteButton />,
//    document.getElementById('content')
//);
//ReactDOM.render(
//    <ViewButton />,
//    document.getElementById('content')
//);
//ReactDOM.render(
//    <EditButton />,
//    document.getElementById('content')
//);
//ReactDOM.render(
//    <DeleteButton />,
//    document.getElementById('content')
//);