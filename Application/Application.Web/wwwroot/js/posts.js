var connection = new signalR.HubConnectionBuilder().withUrl('/postshub').build();

connection.on('NotifyUserOfPost', function (postParentType, post_Id, post_UserFirstName, post_Content, post_threadId) {
    var totalNotifications = Number(document.getElementById('notificationCount').innerHTML);
    var notificationList = document.getElementById('notificationList');

    var newNotification = document.createElement('li');
    var newLink = document.createElement('a');
    newLink.href = '/Forum/Thread?threadId=' + post_threadId;
    newLink.id = 'notification_postToThread_' + post_id;
    newLink.innerText = post_UserFirstName + ' commented on your ' + postParentType;
    newNotification.appendChild(newLink);
    notificationList.appendChild(newNotification);
    document.getElementById('notificationCount').innerHTML = totalNotifications + 1;
});

async function start() {
    try {
        await connection.start();
        console.log("SignalR Connected.");
    } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
    }
};

connection.start();