import { ForumIndexFilterBlock } from '../Components/Forum/ThreadFilters.jsx';
import { Table } from '../Components/Shared/Table.jsx';
import { CreateButton } from '../Components/Shared/Button.jsx';
import { ViewButton } from '../Components/Shared/Button.jsx';
import { Pagination } from '../Components/Shared/Pagination.jsx';
import { ForumThread } from '../Renderings/Forum_Thread.jsx';
import { FileUpload } from '../Components/Shared/FileUpload.jsx';

export class ForumIndex extends React.Component {

    constructor(props) {
        super(props);

        this.state = {
            topic: "",
            categories: [],
            categoriesForQueryParam: "",
            categoryOptions: [],
            topicOptions: [],
            pagination: {}
        }

        this.topicChangeHandler = this.topicChangeHandler.bind(this);
        this.categoryChangeHandler = this.categoryChangeHandler.bind(this);
        this.filterHandler = this.filterHandler.bind(this);
        this.pageClickHandler = this.pageClickHandler.bind(this);
    }

    componentDidMount() {
        fetch('../../../../../Forum/Index')
            .then(dataFromApi => dataFromApi.json())
            .then(response => {
                this.update(response);
            });
    }

    render() {
        let headings = ["Title", "Posted By", "Date", "View"];

        return (
            <div>
                <ForumIndexFilterBlock
                    topic={this.state.topic}
                    categories={this.state.categories}
                    topicOptions={this.state.topicOptions}
                    categoryOptions={this.state.categoryOptions}
                    topicChangeHandler={this.topicChangeHandler}
                    categoryChangeHandler={this.categoryChangeHandler}
                    onClick={this.filterHandler}
                />
                <br />
                <Table data={this.state.pagination.itemsToDisplay} headings={headings} />
                <br />
                <CreateButton />
                <Pagination
                    startPage={this.state.pagination.startPage}
                    lastPage={this.state.pagination.lastPage}
                    numberOfPages={this.state.pagination.numberOfPages}
                    currentPage={this.state.pagination.currentPage}
                    maxNumberOfPagesToShowOnEachRequest={this.state.pagination.maxNumberOfPagesToShowOnEachRequest}
                    pageClickHandler={this.pageClickHandler}
                />
            </div>
        );
    }

    pageClickHandler(event) {
        fetch('../../../../../Forum/Index?page=' + event.target.value + "&topic=" + this.state.topic + "&categories=" + this.state.categoriesForQueryParam)
            .then(dataFromApi => dataFromApi.json())
            .then(response => {
                this.update(response);
            });
    }

    filterHandler() {
        fetch('../../../../../Forum/Index?page=1&topic=' + this.state.topic + '&categories=' + this.state.categoriesForQueryParam)
            .then(dataFromApi => dataFromApi.json())
            .then(response => {
                this.update(response);
            });
    }

    topicChangeHandler(event) {
        if (event.target.value == null || event.target.value == undefined || event.target.value == "undefined") {
            this.setState({ topic: "" });
        }
        else {
            this.setState({ topic: event.target.value });
        }
    }

    categoryChangeHandler(event) {
        let selected = this.state.categories;

        if (selected == null || selected == undefined || selected == "undefined" || selected.length == 0) {
            selected = [];
        }

        if (event.target.value == null || event.target.value == undefined || event.target.value == "undefined") {
            selected = [];
        }
        else {
            let index = selected.indexOf(event.target.value);

            if (index > -1) {
                selected.splice(index, 1);
            }
            else {
                selected.push(event.target.value);
            }

            this.setState({ categories: selected });
        }

        let categoriesValue = "";

        for (let i = 0; i < selected.length; i++) {
            categoriesValue = categoriesValue + selected[i] + "+";
        }

        categoriesValue = categoriesValue.substring(0, categoriesValue.length - 1);

        this.setState({ categoriesForQueryParam: categoriesValue });
    }

    update(response) {
        let json = JSON.parse(response);

        let items = json.pagination.itemsToDisplay;
        let itemsToDisplay = [];

        for (let i = 0; i < items.length; i++) {
            let item = items[i];
            let link = <ViewButton value={item.threadId} onClick={this.navigateToThread} />;

            itemsToDisplay.push(
                {
                    Heading: item.heading,
                    PostedBy: item.userFirstName + " " + item.userLastName,
                    DateTime: item.dateTime,
                    Link: link
                }
            );
        }

        json.pagination.itemsToDisplay = itemsToDisplay;

        this.setState({
            categoryOptions: json.categoryOptions,
            topicOptions: json.topicOptions,
            pagination: json.pagination
        });
    }

    navigateToThread(event) {
        fetch('../../../../../Forum/Thread?threadId=' + event.target.value)
            .then(dataFromApi => dataFromApi.json())
            .then(response => {
                var json = JSON.parse(response);

                ReactDOM.render(
                    <ForumThread
                        userFullName={json.pageData.user.firstName + " " + json.pageData.user.lastName}
                        categories={json.pageData.categories}
                        heading={json.pageData.heading}
                        dateTime={json.pageData.dateTime}
                        topic={json.pageData.topic}
                        body={json.pageData.body}
                        posts={json.paginationData.itemsToDisplay}
                        threadId={json.pageData.id}
                    />,
                    document.getElementById('content')
                );
            });
    }
}