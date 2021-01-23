function loadMorePosts() {

    let url = '/Forum/GetRepliesOnPost';

    let postId = this.value;
    let paramFrom = document.getElementById('hasMoreFrom_' + postId).value;
    let paramTake = document.getElementById('hasMoreTake_' + postId).value;

    url = url + '?postId=' + postId + '&from=' + paramFrom + '&take=' + paramTake;

    fetch(url)
        .then(data => data.json())
        .then(response => new function () {

            let numberOfItems = response.ItemsToDisplay.length;

            for (let i = 0; i < numberOfItems; i++) {
                let item = response.ItemsToDisplay[i];

                let replyToAddToContainer = document.createElement('div');
                replyToAddToContainer.style = 'margin-left:15%';

                let userDisplay = document.createElement('b');
                userDisplay.innerText = item.User.FirstName + ' ' + item.User.LastName + 'commented on ' + item.DateTime;
                replyToAddToContainer.appendChild(userDisplay);

                let contentDisplay = document.createElement('p');
                contentDisplay.innerText = item.Content;
                replyToAddToContainer.appendChild(contentDisplay);

                let hr = document.createElement('hr');
                replyToAddToContainer.appendChild(hr);

                let formDisplay = document.createElement('form');
                formDisplay.setAttribute('action', '/Forum/CreatePost');
                formDisplay.setAttribute('method', 'post');
                //var antiForgeryField = document.createElement('input');
                let commentField = document.createElement('input');
                commentField.setAttribute('type', 'text');
                commentField.setAttribute('placeholder', 'Enter your comment here');
                formDisplay.appendChild(commentField);
                let hiddenFieldThreadId = document.createElement('input');
                hiddenFieldThreadId.setAttribute('type', 'hidden');
                hiddenFieldThreadId.setAttribute('name', 'threadId');
                hiddenFieldThreadId.setAttribute('value', item.ThreadId);
                formDisplay.appendChild(hiddenFieldThreadId);
                let hiddenFieldParentPostId = document.createElement('input');
                hiddenFieldParentPostId.setAttribute('type', 'hidden');
                hiddenFieldParentPostId.setAttribute('name', 'parentPostId');
                hiddenFieldParentPostId.setAttribute('value', item.Id);
                formDisplay.appendChild(hiddenFieldParentPostId);
                let btnSubmit = document.createElement('button');
                btnSubmit.setAttribute('type', 'submit');
                btnSubmit.setAttribute('class', 'btn btn-primary');
                btnSubmit.innerText = 'Submit';
                formDisplay.appendChild(btnSubmit);
                replyToAddToContainer.appendChild(formDisplay);

                document.getElementById('replies_' + postId).appendChild(replyToAddToContainer);
            }

            if (response.HasMore == true) {
                document.getElementById('hasMoreFrom_' + postId).value = response.From;
                document.getElementById('hasMoreTake_' + postId).value = response.Take;
            }
            else {
                document.getElementById('btnLoadMore_' + postId).style = 'display:none';
            }
        });

}