﻿var connection = new signalR.HubConnectionBuilder().withUrl('/reactionshub').build();

connection.on('NotifyReaction', function (reactionType, reaction_threadId, reaction_userId, reaction_userFFirstName, totalNotifications) {
    //var response = responseFromHub.value;
    //alert(JSON.stringify(response));
    //document.getElementById('notificationCount').innerHTML = response.totalNotifications;
    //var notificationList = document.getElementById('notificationList');
    //notificationList.innerHTML = '';

    //if (response.reaction != null && response.reaction.ReactionType != null && response.reaction.ReactionType != 'NONE') {
    //    var newNotification = document.createElement('li');
    //    var newLink = document.createElement('a');
    //    newLink.href = '/Forum/Thread?threadId=' + response.reaction.ThreadId;
    //    newLink.id = 'notification_reactionToThread_' + response.reaction.ThreadId + response.reaction.UserId;
    //    newLink.innerText = response.reaction.User.FirstName + ' reacted (' + response.reaction.ReactionType + ') to your thread: ' + response.reaction.Thread.Heading;
    //    newNotification.appendChild(newLink);
    //    notificationList.appendChild(newNotification);
    //}
    //else {
    //    document.getElementById('notification_reactionToThread_' + response.reaction.ThreadId + response.reaction.UserId).remove();
    //}
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
                console.log(response.value.threadId + ", " + response.value.loggedOnUser.id + "," + response.value.totalReactions);
                connection
                    .invoke('SendMessage', response.value.threadId, response.value.loggedOnUser.id, response.value.totalReactions)
                    .catch(function (err) {
                    return console.error(err.toString());
                });
            });
    }

    function updateView(reactions) {

        let usersWhoHaveReacted = getUsersWhoHaveReacted();

        let label = '';

        blankOutUnnecessaryImagePlaceholders();

        setAvatarsAndLabel();

        function getUsersWhoHaveReacted() {
            let usersWhoHaveReacted = [];

            if (reactions.hasLoggedOnUserReactedToThread) {
                reactions.loggedOnUser.name = 'You';
                usersWhoHaveReacted[usersWhoHaveReacted.length] = reactions.loggedOnUser;
            }

            for (let i = 0; i < reactions.usersWhoHaveReacted.length; i++) {
                usersWhoHaveReacted[usersWhoHaveReacted.length] = reactions.usersWhoHaveReacted[i];
            }

            return usersWhoHaveReacted;
        }

        function blankOutUnnecessaryImagePlaceholders(){
            if (usersWhoHaveReacted.length < 3) {
                let blankAvatarPlaceholders = 3 - usersWhoHaveReacted.length;
                let startIndexOfBlanking = (3 - blankAvatarPlaceholders) + 1;

                for (let i = startIndexOfBlanking; i <= 3; i++) {
                    document.getElementById('reactionAvatarDisplaySpan' + i + '_' + reactions.threadId).style.display = 'none';
                }
            }
        }

        function setAvatarsAndLabel() {
            if (usersWhoHaveReacted.length == 0) {
                label = 'Be the first to react to this!';
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
                    usersWhoHaveReacted[i].avatarSrc = usersWhoHaveReacted[i].avatarSrc == null ? 'defaultProfilePic.jpg' : usersWhoHaveReacted[i].avatarSrc
                    document.getElementById('reactionAvatarDisplay' + num + '_' + reactions.threadId).src = '../../../Images/' + usersWhoHaveReacted[i].avatarSrc;
                }
            }

            document.getElementById('spnReactionsCount_' + reactions.threadId).innerHTML = label;
        }
    }
}