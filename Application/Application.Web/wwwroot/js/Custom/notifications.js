"use strict";

function reactionsNotification() {
    var connection = new signalR.HubConnectionBuilder().withUrl("/reactionshub").build();

    connection.on("NotifyReaction", function (user, message) {
        var obj = JSON.parse(message.data);

        document.getElementById("notificationCount").innerHTML = obj.length;
        var notificationList = document.getElementById("notificationList");
        notificationList.innerHTML = "";

        for (var i = 0; i < obj.length; i++) {
            var newNotification = document.createElement("li");
            var newLink = document.createElement("a");
            newLink.href = "/Forum/Thread?threadId=" + obj[i].Thread.Id;
            newLink.innerText = obj[i].User.FirstName + " reacted to your thread: " + obj[i].Thread.Heading;
            newNotification.appendChild(newLink);
            notificationList.appendChild(newNotification);
        }
    });

    connection.start().then(function () {
        document.getElementById("sendButton").disabled = false;
    }).catch(function (err) {
        return console.error(err.toString());
    });

    document.getElementById("sendButton").addEventListener("click", function (event) {
        var user = document.getElementById("userInput").value;
        var message = document.getElementById("messageInput").value;
        connection.invoke("SendMessage", user, message).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    });
}