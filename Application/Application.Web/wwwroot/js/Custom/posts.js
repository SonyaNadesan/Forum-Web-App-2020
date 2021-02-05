var postsListener = new signalR.HubConnectionBuilder().withUrl('/postshub').build();
postsListener.start();

postsListener.on('NotifyUserOfPost', function (postParentType, post_Id, post_UserFirstName, post_Content, post_threadId) {
    var totalNotifications = Number(document.getElementById('notificationCount').innerHTML);
    var notificationList = document.getElementById('notificationList');

    var newNotification = document.createElement('li');
    var newLink = document.createElement('a');
    newLink.href = '/Forum/IndividualPost?postId=' + post_Id;
    newLink.id = 'notification_postToThread_' + post_id;
    newLink.innerText = post_UserFirstName + ' commented on your ' + postParentType;
    newNotification.appendChild(newLink);
    notificationList.appendChild(newNotification);
    document.getElementById('notificationCount').innerHTML = totalNotifications + 1;
});

function submitPost(parentPostId) {
    fetch('http://localhost:55931/Forum/CreatePost/', {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.getElementsByName('__RequestVerificationToken')[0].value
        },
        body: JSON.stringify({
            'content': document.getElementById('txtContent_' + parentPostId).value,
            'threadId': document.getElementById('hdnThreadId_' + parentPostId).value,
            'parentPostId': parentPostId
        })
    })
    .then(data => data.json())
        .then(response => new function () {
            console.log(response);
        postsListener.invoke('SendMessage', response.id)
            .catch(function (err) {
                return console.error(err.toString());
            });
    });
}