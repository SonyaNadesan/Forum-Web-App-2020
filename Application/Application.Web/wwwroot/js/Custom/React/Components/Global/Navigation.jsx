export class Navigation extends React.Component {

    constructor(props) {
        super(props);

        this.state = {
            notifications: []
        };
    }

    componentDidMount() {
        var reactionsListener = new signalR.HubConnectionBuilder().withUrl('/reactionshub').build();
        reactionsListener.start();

        reactionsListener.on('NotifyUserOfReaction', function (reactionType, reaction_threadId, reaction_userId, reaction_userFFirstName, reaction_threadHeading) {
            var newNotification = {
                id: 'notification_reactionToThread_' + reaction_threadId + reaction_userId,
                href: '/Forum/Thread?threadId=' + reaction_threadId,
                displayText: reaction_userFFirstName + ' reacted (' + reactionType + ') to your thread: ' + reaction_threadHeading,
            };

            if (reactionType != null && reactionType != 'NONE') {
                this.addNotification(newNotification);
            }
        });
    }

    render() {
        let notifications = this.state.notifications.map((item, index) => {
            return (
                <NotificationListItem
                    id={item.id}
                    href={item.href}
                    displayText={item.displayText}
                />
            );
        })

        return (
            <div className="container">
                <div className="navbar-header">
                    <button type="button" className="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                        <span className="icon-bar"></span>
                        <span className="icon-bar"></span>
                        <span className="icon-bar"></span>
                    </button>
                    <a className="navbar-brand" href="/">Application name</a>
                </div>
                <div className="navbar-collapse collapse">
                    <ul className="nav navbar-nav">
                        <li><a href="/Profile">Profile</a></li>
                        <li><a href="/Forum">Forum</a></li>
                    </ul>
                    <div className="dropdown">
                        <button className="btn btn-primary dropdown-toggle" type="button" data-toggle="dropdown">
                            <span id="notificationCount">0</span>
                            <span className="caret"></span>
                        </button>
                        <ul className="dropdown-menu" id="notificationList">
                            {notifications}
                        </ul>
                    </div>
                </div>
            </div>
        );
    }

    addNotification(notification) {
        let notifications = this.state.notifications;

        notifications.push(notification);

        this.setState({ notifications: notifications });
    }

    removeNotification(notification) {
        let notifications = this.state.notifications;

        notifications.remove(notification);

        this.setState({ notifications: notifications });
    }

}

class NotificationListItem extends React.Component {

    render() {
        return (
            <li>
                <a href={this.props.href} id={this.props.id}>{this.props.displayText}</a>
            </li>
        );
    }

}

ReactDOM.render(
    <Navigation />,
    document.getElementById('navigation')
);