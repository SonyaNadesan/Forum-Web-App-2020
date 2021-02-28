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
            'content': tinymce.get('txtContent_' + parentPostId).getContent(),
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
            postContent.innerHTML = response.content;
            newPost.append(postContent);

            var frmTextbox = document.createElement('textarea');
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

            applyRichTextEditor();

            postsListener.invoke('SendMessage', response.id)
            .catch(function (err) {
                return console.error(err.toString());
            });
    });
}

function applyRichTextEditor() {
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