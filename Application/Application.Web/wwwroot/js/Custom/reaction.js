function setReaction() {
    let btnReactions = document.getElementsByName('btnReaction');
    const noOfThreads = btnReactions.length;

    for (let i = 0; i < noOfThreads; i++) {

        let button = btnReactions[i];

        document.getElementById(button.id).onclick = updateReactions;
        fetch('/Forum/GetReactions?threadId=' + button.value)
        .then(data => data.json())
        .then(response => updateView(response));
    }

    function updateReactions() {

        var threadId = this.value;

        alert(this.value);

        fetch('http://localhost:55931/Forum/UpdateReactions/', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(this.value)
        })
        .then(data => data.json())
           .then(response => new function () {
            updateView(response.value);
        });
    }

    function updateView(reactions) {

        var usersWhoHaveReacted = [];

        if (reactions[0].hasLoggedOnUserReactedToThread) {
            reactions[0].loggedOnUser.firstName = 'You';
            usersWhoHaveReacted[usersWhoHaveReacted.length] = reactions[0].loggedOnUser;
        }

    }
}