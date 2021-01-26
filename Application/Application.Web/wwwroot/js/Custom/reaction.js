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
            body: JSON.stringify({ threadId : this.value })
        })
        .then(data => data.json())
           .then(response => new function () {
            updateView(response.value);
        });
    }

    function updateView(jsonObj) {
        let allUsersWhoHaveReacted = getAllUsersWhoHaveReacted(jsonObj);
        alert(jsonObj.threadId);
        displayLabel(allUsersWhoHaveReacted, 'spnReactionsCount_' + jsonObj.threadId);
        displayAvatars(jsonObj.threadId, allUsersWhoHaveReacted);
    }

    function getAllUsersWhoHaveReacted(reactionsByThreadViewModel) {
        let allUsersWhoHaveReacted = [];

        if (reactionsByThreadViewModel.hasLoggedOnUserReactedToPost) {
            reactionsByThreadViewModel.loggedOnUser.name = 'You';
            allUsersWhoHaveReacted[0] = reactionsByThreadViewModel.loggedOnUser;
        }

        const noOfReactions = reactionsByThreadViewModel.usersWhoHaveReacted.length;
        for (let i = 0; i < noOfReactions; i++) {
            allUsersWhoHaveReacted[allUsersWhoHaveReacted.length] = reactionsByThreadViewModel.usersWhoHaveReacted[i];
        }

        return allUsersWhoHaveReacted;
    }

    function displayLabel(allUsersWhoHaveReacted, spanId) {
        console.log(spanId);
        let label = generateLabel(allUsersWhoHaveReacted, getNumberOfUsernamesToDisplay(allUsersWhoHaveReacted));
        document.getElementById(spanId).innerHTML = label;
    }

    function displayAvatars(threadId, allUsersWhoHaveReacted) {
        let profilePictures = getFirstProfilePictures(allUsersWhoHaveReacted, 3);
        const noOfProfilePics = profilePictures.length;

        const underscorePostId = '_' + threadId;
        for (let i = 0; i < noOfProfilePics; i++) {
            let suffix = (i + 1) + underscorePostId;
            document.getElementById('reactionAvatarDisplay' + suffix).src = '../../../Images/' + profilePictures[i];
            document.getElementById('reactionAvatarDisplaySpan' + suffix).style.display = 'inline-block';
        }

        if (noOfProfilePics < 3) {
            const num = noOfProfilePics + 1;
            for (let j = num; j <= 3; j++) {
                document.getElementById('reactionAvatarDisplaySpan' + j + underscorePostId).style.display = 'none';
            }
        }
    }

    function getFirstProfilePictures(allUsersWhoHaveReacted, numOfPicturesToReturn) {
        let results = [];
        const noOfReactions = allUsersWhoHaveReacted.length;
        for (let i = 0; i < noOfReactions; i++) {
            if (allUsersWhoHaveReacted[i] != undefined) {
                if (allUsersWhoHaveReacted[i].profilePictureImageSrc != null) {
                    if (results.length < numOfPicturesToReturn) {
                        results[results.length] = allUsersWhoHaveReacted[i].profilePictureImageSrc;
                    }
                } else {
                    results[results.length] = 'https://i.stack.imgur.com/34AD2.jpg?width=140&crop=0,0,140,140';
                }
            }
            if (results.length == numOfPicturesToReturn) {
                i = noOfReactions;
            }
        }
        return results;
    }

    function getNumberOfUsernamesToDisplay(reactions) {
        let result = 0;
        if (reactions.length >= 2) {
            result = 2;
        } else if (reactions.length != 0) {
            result = 1;
        }
        return result;
    }

    function generateLabel(allUsersWhoHaveReacted, numOfUsernamesToDisplay) {
        let result = '';
        const noOfReactions = allUsersWhoHaveReacted.length;
        if (noOfReactions == 0) {
            result = 'Be the first to react to this';
        } else if (noOfReactions == 1) {
            result = allUsersWhoHaveReacted[0].name + ' react to this';
        } else {
            const lastIndex = noOfReactions - 1;
            const penultimateIndex = lastIndex - 1;
            for (let i = 0; i < noOfReactions; i++) {
                if (i < numOfUsernamesToDisplay) {
                    result = result + allUsersWhoHaveReacted[i].name;
                }
                if (i == penultimateIndex) {
                    result = result + ' and ';
                }
                else if (i == lastIndex) {
                    const noOfRemainingUsers = noOfReactions - numOfUsernamesToDisplay;
                    if (noOfRemainingUsers == 1) {
                        result = result + noOfRemainingUsers + ' other ';
                    }
                    else if (noOfRemainingUsers > 0) {
                        result = result + noOfRemainingUsers + ' others ';
                    }
                    result = result + ' reacted to this ';
                    i = noOfReactions;
                }
                else if (i != lastIndex) {
                    result = result + ', ';
                }
            }
        }
        return result;
    }
}