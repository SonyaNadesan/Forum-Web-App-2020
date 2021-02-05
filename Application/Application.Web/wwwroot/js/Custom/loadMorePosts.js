﻿function setLoadMore() {
    let loadMoreButtons = document.getElementsByName("btnLoadMore");

    for (let i = 0; i < loadMoreButtons.length; i++) {
        document.getElementById(loadMoreButtons[i].id).onclick = loadMorePosts;
    }

    function loadMorePosts() {
        let url = '/Forum/GetRepliesOnPost';

        let postId = this.value;
        let paramFrom = document.getElementById('hasMoreFrom_' + postId).value;
        let paramTake = document.getElementById('hasMoreTake_' + postId).value;

        url = url + '?postId=' + postId + '&from=' + paramFrom + '&take=' + paramTake;

        fetch(url)
            .then(data => data.json())
            .then(response => new function () {
                loadMorePosts_updateView(response, postId, paramFrom, paramTake);
            });
    }
}

function loadMorePosts_updateView(response, postId, paramFrom, paramTake) {
    let numberOfItems = response.itemsToDisplay.length;

    for (let i = 0; i < numberOfItems; i++) {
        let item = response.itemsToDisplay[i];

        let replyToAddToContainer = document.createElement('div');
        replyToAddToContainer.style = 'margin-left:' + ((item.levelInHierarchy > 5 ? 4 : item.levelInHierarchy) * 5) + "%";

        let userDisplay = document.createElement('b');
        userDisplay.innerText = item.user.firstName + ' ' + item.user.lastName + 'commented on ' + item.dateTime;
        replyToAddToContainer.appendChild(userDisplay);

        let contentDisplay = document.createElement('p');
        contentDisplay.innerText = item.content;
        replyToAddToContainer.appendChild(contentDisplay);

        let hr = document.createElement('hr');
        replyToAddToContainer.appendChild(hr);

        let formDisplay = document.createElement('form');
        formDisplay.setAttribute('action', '/Forum/CreatePost');
        formDisplay.setAttribute('method', 'post');
        var antiForgeryField = document.createElement('input');
        antiForgeryField.setAttribute('type', 'hidden');
        antiForgeryField.setAttribute('name', '__RequestVerificationToken');
        var antiForgeryToken = document.getElementsByName('__RequestVerificationToken')[0].value;
        antiForgeryField.setAttribute('value', antiForgeryToken);
        formDisplay.appendChild(antiForgeryField);
        let commentField = document.createElement('input');
        commentField.setAttribute('type', 'text');
        commentField.setAttribute('name', 'content');
        commentField.setAttribute('placeholder', 'Enter your comment here');
        formDisplay.appendChild(commentField);
        let hiddenFieldThreadId = document.createElement('input');
        hiddenFieldThreadId.setAttribute('type', 'hidden');
        hiddenFieldThreadId.setAttribute('name', 'threadId');
        hiddenFieldThreadId.setAttribute('value', item.threadId);
        formDisplay.appendChild(hiddenFieldThreadId);
        let hiddenFieldParentPostId = document.createElement('input');
        hiddenFieldParentPostId.setAttribute('type', 'hidden');
        hiddenFieldParentPostId.setAttribute('name', 'parentPostId');
        hiddenFieldParentPostId.setAttribute('value', item.id);
        formDisplay.appendChild(hiddenFieldParentPostId);
        let btnSubmit = document.createElement('button');
        btnSubmit.setAttribute('type', 'submit');
        btnSubmit.setAttribute('class', 'btn btn-primary');
        btnSubmit.innerText = 'Submit';
        formDisplay.appendChild(btnSubmit);
        replyToAddToContainer.appendChild(formDisplay);

        document.getElementById('replies_' + postId).appendChild(replyToAddToContainer);
    }

    if (response.hasMore == true) {
        document.getElementById('hasMoreFrom_' + postId).value = response.from;
        document.getElementById('hasMoreTake_' + postId).value = response.take;
    }
    else {
        document.getElementById('btnLoadMore_' + postId).style = 'display:none';
    }
}