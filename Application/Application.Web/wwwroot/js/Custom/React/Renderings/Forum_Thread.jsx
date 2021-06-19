import { LikeFeature } from "../Components/Shared/LikeFeature.jsx";
import { Pagination } from "../Components/Shared/Pagination.jsx";
import { LoadMoreButton } from "../Components/Shared/Button.jsx";

export class ForumThread extends React.Component {

    constructor(props) {
        super(props);

        this.state = {
            pagination: {},
            reactions: {}
        }

        fetch('../../../../../Forum/Thread?threadId=' + this.props.threadId + '&page=1')
            .then(dataFromApi => dataFromApi.json())
            .then(response => {
                this.update(response);
            });

        fetch('../../../../../Forum/GetReactions?threadId=' + this.props.threadId)
            .then(dataFromApi2 => dataFromApi2.json())
            .then(response2 => {
                this.updateReactions(response2);
            });

        this.pageClickHandler = this.pageClickHandler.bind(this);
        this.loadMoreHandler = this.loadMoreHandler.bind(this);
        this.likeHandler = this.likeHandler.bind(this);
    }

    componentDidMount() {
        this.applyRichTextFeatures();
    }

    render() {
        let categoriesAsHtml = [];

        this.props.categories.map((category, i) => {
            categoriesAsHtml.push(
                <div /*style="font-family:Arial;font-weight: 600;text-align:center; height:25px;display:inline-block; background-color:#F2F2F5"*/ key={"xategoryForCurrentThread_" + i}>
                    <div /*style="padding-left:20px;padding-right:20px;"*/>
                        {category.displayName}
                    </div>
                </div>
            );
        });

        let postsAsHtml = [];

        if (this.state.pagination != undefined && this.state.pagination.itemsToDisplay != undefined) {
            this.state.pagination.itemsToDisplay.map((post, i) => {
                postsAsHtml.push(
                    <TopLevelPostView
                        key={"post_" + i}
                        content={post.topLevelPost.content}
                        postedBy={post.topLevelPost.postedBy}
                        replies={post.replies}
                        postId={post.topLevelPost.postId}
                        threadId={post.topLevelPost.threadId}
                        submitPost={this.submitPost}
                        loadMoreHandler={this.loadMoreHandler}
                        from={post.replies.from}
                        take={post.replies.take}
                        hasMore={post.replies.hasMore}
                    />
                );
            });
        }

        return (
            <div className="container">
                <div>
                    <h1>{this.props.heading}</h1>
                    <h3>By {this.props.userFullName}</h3>
                    <div /*style="font-family:Arial;font-weight: 600;text-align:center;width:100%; height:25px;background-color:#000066; color: white"*/>
                        Looking for {this.props.topic.displayName}
                    </div>
                    <br />
                    {categoriesAsHtml}
                    <h4>{this.props.body}</h4>
                </div>
                <LikeFeature
                    itemId={this.props.threadId}
                    likeHandler={this.likeHandler}
                    hasLoggedOnUserReactedToThread={this.state.reactions.hasLoggedOnUserReactedToThread}
                    totalReactions={this.state.reactions.totalReactions}
                    usersWhoHaveReacted={this.state.reactions.usersWhoHaveReacted}
                    loggedOnUser={this.state.reactions.loggedOnUser}
                />
                {postsAsHtml}
                <div>
                    <br /><br />
                    <h3>Post to this Thread</h3>
                    <textarea id={"txtContent_" + this.props.threadId} placeholder="Enter your comment here" name="content"></textarea>
                    <input type="hidden" id={"threadId_" + this.props.threadId} value={this.props.threadId} />
                    <input type="hidden" id={"postId_" + this.props.threadId} value="" />
                    <button className="btn btn-primary" onClick={this.submitPost} value={this.props.threadId}>Submit</button>
                </div>
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

    likeHandler() {
        fetch('../../../../../Forum/UpdateReactions/', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(this.props.threadId)
        })
        .then(data => data.json())
        .then(response => {

            this.updateReactions(response.value);

            let json = JSON.parse(response.value);
            console.log(json);

            this.props.reactionsListener.invoke('SendMessage', json.threadId, json.loggedOnUser.id).catch(function (err) {
                return console.error(err.toString());
            });
        });
    }

    submitPost(event) {
        fetch('../../../../../Forum/CreatePost/', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
            },
            body: JSON.stringify({
                'content': tinymce.get('txtContent_' + event.target.value).getContent(),
                'threadId': document.getElementById('threadId_' + event.target.value).value,
                'parentPostId': document.getElementById('postId_' + event.target.value).value
            })
        })
            .then(data => data.json())
            .then(response => {
                this.props.postsListener.invoke('SendMessage', response.id)
                    .catch(function (err) {
                        return console.error(err.toString());
                    });
            });
    }

    pageClickHandler(event) {
        fetch('../../../../../Forum/Thread?threadId=' + this.props.threadId + '&page=' + event.target.value + "&startPage=" + this.state.pagination.startPage + "&query=")
            .then(dataFromApi => dataFromApi.json())
            .then(response => {
                this.update(response);
            });
    }

    update(response) {
        var json = JSON.parse(response);

        this.setState({
            pagination: json.paginationData
        });

        this.applyRichTextFeatures();
    }

    updateReactions(response) {
        var json = JSON.parse(response);

        this.setState({
            reactions: json
        });
    }

    applyRichTextFeatures() {
        tinymce.init({
            selector: 'textarea',
            block_formats: 'Paragraph=p; Header 1=h1; Header 2=h2; Header 3=h3',
            plugins: 'link image table',
            contextmenu: 'link image table',
            menu: {
                edit: { title: 'Edit', items: 'undo redo | cut copy paste | selectall | searchreplace' },
                view: { title: 'View', items: 'spellchecker' },
                insert: { title: 'Insert', items: 'image link media template codesample inserttable | charmap emoticons hr | pagebreak nonbreaking anchor toc | insertdatetime' },
                format: { title: 'Format', items: 'bold italic underline strikethrough superscript subscript | fontsizes | removeformat' },
                tools: { title: 'Tools', items: 'spellchecker spellcheckerlanguage | wordcount' },
                table: { title: 'Table', items: 'inserttable | cell row column | tableprops deletetable' },
                help: { title: 'Help', items: 'help' }
            }
        });
    }

    loadMoreHandler(event) {
        let url = '/Forum/GetRepliesOnPost';

        let postId = event.target.value;

        let paramFrom = document.getElementById('hasMoreFrom_' + postId).value;
        let paramTake = document.getElementById('hasMoreTake_' + postId).value;

        let excludeThesePostIds = document.getElementById('excludeIdsFor_' + postId);
        let excludeIdsForPost = excludeThesePostIds == undefined ? excludeIds : excludeThesePostIds.value;

        url = url + '?postId=' + postId + '&from=' + paramFrom + '&take=' + paramTake + '&excludeIds=' + excludeIdsForPost;

        fetch(url)
            .then(data => data.json())
            .then(response => {

                let json = JSON.parse(response);

                let pagination = this.state.pagination;

                //find the index of top level post
                let displayedItems = pagination.itemsToDisplay;

                displayedItems.map((item, i) => {
                    if (item.topLevelPost.postId == json.id) {
                        json.itemsToDisplay.map((newItem, j) => {
                            item.replies.itemsToDisplay.push(newItem);
                            item.replies.from = json.from;
                            item.replies.take = json.take;
                            item.replies.hasMore = json.hasMore;
                        })
                    }
                });

                pagination.itemsToDisplay = displayedItems;

                this.setState({
                    pagination: pagination
                });
            });
    }
}

class PostView extends React.Component {

    constructor(props) {
        super(props);
    }

    componentDidMount() {
        tinymce.init({
            selector: 'textarea',
            block_formats: 'Paragraph=p; Header 1=h1; Header 2=h2; Header 3=h3',
            plugins: 'link image table',
            contextmenu: 'link image table',
            menu: {
                edit: { title: 'Edit', items: 'undo redo | cut copy paste | selectall | searchreplace' },
                view: { title: 'View', items: 'spellchecker' },
                insert: { title: 'Insert', items: 'image link media template codesample inserttable | charmap emoticons hr | pagebreak nonbreaking anchor toc | insertdatetime' },
                format: { title: 'Format', items: 'bold italic underline strikethrough superscript subscript | fontsizes | removeformat' },
                tools: { title: 'Tools', items: 'spellchecker spellcheckerlanguage | wordcount' },
                table: { title: 'Table', items: 'inserttable | cell row column | tableprops deletetable' },
                help: { title: 'Help', items: 'help' }
            }
        });
    }

    render() {
        let leftIndent = (this.props.levelInHierarchy * 5) + "%";

        return (
            <div style={{ marginLeft: leftIndent }}>
                <b>{this.props.postedBy} commented:</b>
                <br /><br />
                <div dangerouslySetInnerHTML={{ __html: this.props.content }}></div>
                <br /><br />
                <textarea id={"txtContent_" + this.props.postId} placeholder="Enter your comment here" name="content"></textarea>
                <input type="hidden" id={"postId_" + this.props.postId} value={this.props.postId} />
                <input type="hidden" id={"threadId_" + this.props.postId} value={this.props.threadId} />
                <button className="btn btn-primary" onClick={this.props.submitPost} value={this.props.postId}>Submit</button>
            </div>
        );
    }

}

class TopLevelPostView extends React.Component {

    constructor(props) {
        super(props);
    }

    render() {
        let replies = [];

        this.props.replies.itemsToDisplay.map((reply, i) => {
            replies.push(
                <PostView
                    postedBy={reply.postedBy}
                    content={reply.content}
                    postId={reply.postId}
                    threadId={reply.threadId}
                    submitPost={this.props.submitPost}
                    key={"post_" + reply.postId}
                    topLevelPostId={this.props.postId}
                    levelInHierarchy={reply.levelInHierarchy}
                />
            );
        });

        return (
            <div>
                <PostView
                    postedBy={this.props.postedBy}
                    content={this.props.content}
                    postId={this.props.postId}
                    threadId={this.props.threadId}
                    submitPost={this.props.submitPost}
                    topLevelPostId={this.props.postId}
                    levelInHierarchy={1}
                />
                <div id={"replies_" + this.props.postId}>
                    {replies}
                    <LoadMoreButton
                        id={this.props.postId}
                        from={this.props.from}
                        take={this.props.take}
                        hasMore={this.props.hasMore}
                        loadMoreHandler={this.props.loadMoreHandler}
                    />
                </div>
            </div>
        );
    }

}

