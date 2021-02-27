var postsListener = new signalR.HubConnectionBuilder().withUrl('/postshub').build();
postsListener.start();

postsListener.on('NotifyUserOfPost', function (postParentType, post_Id, post_UserFirstName, post_Content, post_threadId) {
    var totalNotifications = Number(document.getElementById('notificationCount').innerHTML);
    var notificationList = document.getElementById('notificationList');
    var newNotification = document.createElement('li');
    var newLink = document.createElement('a');

    newLink.href = '/Forum/IndividualPost?postId=' + post_Id;
    newLink.id = 'notification_post' + post_Id;
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
            'threadId': document.getElementById('hdnThreadId').value,
            'parentPostId': parentPostId
        })
    })
    .then(data => data.json())
        .then(response => new function () {
            var parentPost = document.getElementById('post_' + response.parentPostId);

            var newPost = document.createElement('div');
            newPost.setAttribute('style', 'margin-left:' + response.levelInHierarchy * 5 + "%");
            newPost.setAttribute('id', 'post_' + response.id);

            var postOwner = document.createElement('b');
            postOwner.append(response.user.firstName + ' ' + response.user.lastName + ' commented on ' + response.dateTime);
            newPost.append(postOwner);

            var postContent = document.createElement('p');
            postContent.append(response.content);
            newPost.append(postContent);

            var frmTextbox = document.createElement('input');
            frmTextbox.setAttribute('type', 'text');
            frmTextbox.setAttribute('id', 'txtContent_' + response.id);
            frmTextbox.setAttribute('placeholder', 'Enter your comment here');
            frmTextbox.setAttribute('name', 'content');
            newPost.append(frmTextbox);

            var frmHdnParentPost = document.createElement('input');
            frmHdnParentPost.setAttribute('type', 'hidden');
            frmHdnParentPost.setAttribute('id', 'hdnParentPostId_' + response.id);
            frmHdnParentPost.setAttribute('value', 'hdnParentPostId_' + response.id);
            frmHdnParentPost.setAttribute('name', 'ParentPostId');
            newPost.append(frmHdnParentPost);

            var btnSubmit = document.createElement('button');
            btnSubmit.setAttribute('class', 'btn btn-primary');
            btnSubmit.innerText = 'Submit';
            btnSubmit.addEventListener('click', function () {
                submitPost(response.id);
            });
            newPost.append(btnSubmit);

            if (!response.hasParentPost) {
                document.getElementById('topLevelPosts').append(newPost);
            }
            else {
                parentPost.after(newPost);
            }

            postsListener.invoke('SendMessage', response.id)
            .catch(function (err) {
                return console.error(err.toString());
            });
    });
}