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

        let usersWhoHaveReacted = [];

        let label = '';

        if (reactions.hasLoggedOnUserReactedToThread) {
            reactions.loggedOnUser.name = 'You';
            usersWhoHaveReacted[usersWhoHaveReacted.length] = reactions.loggedOnUser;
        }

        for (let i = 0; i < reactions.usersWhoHaveReacted; i++) {
            usersWhoHaveReacted[usersWhoHaveReacted.length] = reactions.usersWhoHaveReacted[i];
        }

        if (usersWhoHaveReacted.length < 3) {
            let blankAvatarPlaceholders = 3 - usersWhoHaveReacted.length;
            let startIndexOfBlanking = (3 - blankAvatarPlaceholders) + 1;

            for (let i = startIndexOfBlanking; i <= 3; i++) {
                document.getElementById('reactionAvatarDisplaySpan' + i + '_' + reactions.threadId).style.display = 'none';
            }

            if (usersWhoHaveReacted.length == 0) {
                label = 'Be the first to react to this!';
            }
        }

        if (label == '') {
            for (let i = 0; i < usersWhoHaveReacted.length; i++) {

                let num = i + 1;

                let textToAppend = '';
                var indexOfPenultimateUser = usersWhoHaveReacted.length - 2;

                if (i == indexOfPenultimateUser) {
                    textToAppend = ' and ';
                }
                else if (i < indexOfPenultimateUser) {
                    textToAppend = ', ';
                }
                else {
                    textToAppend = ' reacted to this!';
                }

                label = label + usersWhoHaveReacted[i].name + textToAppend
                document.getElementById('reactionAvatarDisplay' + num + '_' + reactions.threadId).src = '../../../Images/' + usersWhoHaveReacted[i].avatarSrc;
            }
        }

        document.getElementById('spnReactionsCount_' + reactions.threadId).innerHTML = label;
    }
}