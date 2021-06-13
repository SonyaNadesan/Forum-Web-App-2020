import { HiddenField } from './HiddenField.jsx';

export class Pagination extends React.Component {

    render() {
        if (this.props.numberOfPages > 1) {

            let hiddenFields = [];

            for (const [key, value] of Object.entries(this.props.hiddenFields)) {
                hiddenFields.push(
                    <HiddenField keyOfKeyValuePair={key} valueOfKeyValuePair={value} key={"hidden_" + key} />
                );
            }

            return (
                <div className="inline-block form-inline">
                    <input type="hidden" name="startPage" value={this.props.startPage} />
                    <PreviousButton
                        startPage={this.props.startPage}
                        maxNumberOfPagesToShowOnEachRequest={this.props.maxNumberOfPagesToShowOnEachRequest}
                        numberOfPages={this.props.numberOfPages}
                        pageClickHandler={this.props.pageClickHandler}
                    />
                    <Pages
                        startPage={this.props.startPage}
                        lastPage={this.props.lastPage}
                        currentPage={this.props.currentPage}
                        pageClickHandler={this.props.pageClickHandler}
                    />
                    <NextButton
                        numberOfPages={this.props.numberOfPages}
                        lastPage={this.props.lastPage}
                        maxNumberOfPagesToShowOnEachRequest={this.props.maxNumberOfPagesToShowOnEachRequest}
                        pageClickHandler={this.props.pageClickHandler}
                    />
                    {hiddenFields}
                </div>
            );
        }
        else {
            return (
                <div className="inline-block"></div>
            );
        }
    }

}

class PreviousButton extends React.Component {

    render() {
        if (this.props.startPage > this.props.maxNumberOfPagesToShowOnEachRequest && this.props.numberOfPages > this.props.maxNumberOfPagesToShowOnEachRequest) {
            return (
                <div className="form-group">
                    <button type="button" className="btn btn-link" name="page" value={this.props.startPage - 1} onClick={this.props.pageClickHandler}>
                        <span className="glyphicon glyphicon-chevron-left"></span>
                    </button>
                </div>
            );
        }
        else {
            return (
                <div className="form-group"></div>
            );
        }
    }

}

class Pages extends React.Component {

    render() {
        let pages = [];

        for (let i = this.props.startPage; i <= this.props.lastPage; i++) {
            pages.push(
                <Page pageClickHandler={this.props.pageClickHandler} currentPage={this.props.currentPage} page={i} key={ "page_" + i } />
            );
        }

        return (
            <div className="form-group">
                {pages}
            </div>
        );
    }

}

class Page extends React.Component {

    render() {
        if (this.props.currentPage == this.props.page) {
            return (
                <button className="btn btn-primary" onClick={this.props.pageClickHandler} name="page" value={this.props.page}>{this.props.page}</button>
            );
        }
        else {
            return (
                <button className="btn btn-link" onClick={this.props.pageClickHandler} name="page" value={this.props.page}>{this.props.page}</button>
            );
        }
    }

}

class NextButton extends React.Component {

    render() {
        if (this.props.lastPage < this.props.numberOfPages && this.props.numberOfPages > this.props.maxNumberOfPagesToShowOnEachRequest) {
            return (
                <div className="form-group">
                    <button className="btn btn-link" name="page" value={this.props.lastPage + 1} onClick={this.props.pageClickHandler}>
                        <span className="glyphicon glyphicon-chevron-right"></span>
                    </button>
                </div>
            );
        }
        else {
            return (
                <div className="form-group"></div>
            );
        }
    }

}

Pagination.defaultProps = { hiddenFields: {} };