function scrollToBottom(clickedElementID) {

    var userID = clickedElementID.substring(0, clickedElementID.indexOf("Conversation-tab"));
    var conversationID = "#" + "v-pills-" + userID + "Conversation";
    var actualChatBox = document.getElementsByClassName("chat-box active")[0];
    actualChatBox.classList.remove("active");
    $(conversationID)[0].classList.add("active");



    $(conversationID)[0].scrollTop = $(conversationID)[0].scrollHeight;
    //console.log("Zjezdzam na dół: " + $(conversationID)[0].id);
}

(function () {


    var actualChatBox = document.getElementsByClassName("chat-box active")[0];
    actualChatBox.scrollTop = actualChatBox.scrollHeight;


    $('#message-content').keypress(function (e) {
        if (e.which == 13) {//Enter key pressed
            $('#send-message-btn').trigger('click');//Trigger search button click event
        }
    });

    $("#message-send-form").keypress(function (event) {
        return event.keyCode != 13;
    });

    //ŁĄCZY SIĘ Z HUBEM
    var chatHub = $.connection.chatHub;

    chatHub.client.receiveMessage = function (message, senderID, senderName, avatarImmageURL) {
        writeReceivedMessage(message, senderID, senderName, avatarImmageURL);
    }

    $.connection.hub.start()
        .done(function () {
            console.log("Hub connected!");

            // chatHub.client.connected = function (id) {
            //     console.log("contextID?: " + id);
            // };
            // WYSYŁANIE WIADOMOŚCI

            $("#send-message-btn").on("click", function () {

                var message = $("#message-content").val();
                if (message != "") {
                    var user = document.getElementsByClassName("user-last-msg active");
                    var receiverID = user[0].id.substring(0, user[0].id.indexOf("Conversation"));

                    //console.log("Wysyłam wiadomość");
                    writeSentMessage(message, receiverID);
                    chatHub.server.sendMessage(message, receiverID);
                }
            });


            // NASŁUCHIWANIE WIADOMOŚCI
            // chatHub.client.receiveMessage = function (message, senderID) {
            //     console.log("Dostano wiadomość!");

            //     writeReceivedMessage(message, senderID);
            // }
        })
        .fail(function () { alert("ERROR"); })

    var writeReceivedMessage = function (message, senderID, senderName, avatarImageUrl) {

        var conversationID = "#" + "v-pills-" + senderID + "Conversation";
        var paragraphID = "#" + senderID + "Conversation-p";
        var dateID = "#" + senderID + "Conversation-date";

        var date = new Date();
        var minutes = date.getMinutes() >= 10 ? date.getMinutes() : "0" + date.getMinutes();
        var hours = date.getHours() >= 10 ? date.getHours() : "0" + date.getHours();

        var actualChatBox = document.getElementsByClassName("chat-box active")[0];
        var receivedMessageChatBox = $(conversationID)[0];
        var paragraph = $(paragraphID)[0];
        var date = $(dateID)[0];

        paragraph.innerHTML = senderName + ": " + message;
        date.innerHTML = hours + ":" + minutes;


        $(conversationID).append(
            "<div class=\"media w-50 mb-3 message\">"
            + "<img src=\"" + avatarImageUrl + "\" alt=\"user\" width=\"30\" class=\"rounded-circle\">"
            + "<div class=\"media-body ml-3\">"
            + "<div class=\"bg-light rounded py-2 px-3 mb-2\" data-toggle=\"tooltip\" title=\""
            + hours + ":" + minutes
            + "\">"
            + "<p class=\"text-small mb-0 text-muted\">" + message + "</p>"
            + "</div>"
            + "</div>"
            + "</div>"
        );

        if (actualChatBox.id == receivedMessageChatBox.id) {
            receivedMessageChatBox.scrollTop = receivedMessageChatBox.scrollHeight;
        }
    }


    var writeSentMessage = function (message, receiverID) {

        var date = new Date();
        var minutes = date.getMinutes() >= 10 ? date.getMinutes() : "0" + date.getMinutes();
        var hours = date.getHours() >= 10 ? date.getHours() : "0" + date.getHours();

        var chatBox = document.getElementsByClassName("chat-box active");
        var conversationID = "#" + chatBox[0].id;
        var paragraphID = "#" + receiverID + "Conversation-p";
        var dateID = "#" + receiverID + "Conversation-date";

        var paragraph = $(paragraphID)[0];
        var date = $(dateID)[0];

        date.innerHTML = hours + ":" + minutes;
        paragraph.innerHTML = "Ty: " + message;

        $(conversationID).append(
            "<div class=\"media w-50 ml-auto mb-3 message\">"
            + "<div class=\"media-body\">"
            + "<div class=\"bg-primary rounded py-2 px-3 mb-2\" data-toggle=\"tooltip\" title=\""
            + hours + ":" + minutes
            + "\">"
            + "<p class=\"text-small mb-0 text-white\">" + message + "</p>"
            + "</div>"
            + "</div>"
            + "</div>"
        );

        chatBox[0].scrollTop = chatBox[0].scrollHeight;

        $('#message-content').val('');
    }
})()
