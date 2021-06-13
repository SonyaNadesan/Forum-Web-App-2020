var reactionsListener = new signalR.HubConnectionBuilder().withUrl('/reactionshub').build();
reactionsListener.start();

//reactionsListener.on('NotifyUserOfReaction', function (reactionType, reaction_threadId, reaction_userId, reaction_userFFirstName, reaction_threadHeading) {
//    var totalNoOfReactions = Number(document.getElementById('notificationCount').innerHTML);
//    var notificationList = document.getElementById('notificationList');

//    if (reactionType != null && reactionType != 'NONE') {
//        var newNotification = document.createElement('li');
//        var newLink = document.createElement('a');
//        newLink.href = '/Forum/Thread?threadId=' + reaction_threadId;
//        newLink.id = 'notification_reactionToThread_' + reaction_threadId + reaction_userId;
//        newLink.innerText = reaction_userFFirstName + ' reacted (' + reactionType + ') to your thread: ' + reaction_threadHeading;
//        newNotification.appendChild(newLink);
//        notificationList.appendChild(newNotification);
//        document.getElementById('notificationCount').innerHTML = totalNoOfReactions + 1;
//    }
//    else {
//        document.getElementById('notification_reactionToThread_' + reaction_threadId + reaction_userId).remove();
//        document.getElementById('notificationCount').innerHTML = totalNoOfReactions - 1;
//    }
//});

var postsListener = new signalR.HubConnectionBuilder().withUrl('/postshub').build();
postsListener.start();

//postsListener.on('NotifyUserOfPost', function (postParentType, post_Id, post_UserFirstName, post_Content, post_threadId) {
//    var totalNotifications = Number(document.getElementById('notificationCount').innerHTML);
//    var notificationList = document.getElementById('notificationList');
//    var newNotification = document.createElement('li');
//    var newLink = document.createElement('a');

//    newLink.href = '/Forum/IndividualPost?postId=' + post_Id;
//    newLink.id = 'notification_post' + post_Id;
//    newLink.innerText = post_UserFirstName + ' commented on your ' + postParentType;

//    newNotification.appendChild(newLink);
//    notificationList.appendChild(newNotification);

//    document.getElementById('notificationCount').innerHTML = totalNotifications + 1;
//});