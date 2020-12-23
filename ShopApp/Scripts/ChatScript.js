function scrollToBottom(clickedElementID) {

    var userID = clickedElementID.substring(0, clickedElementID.indexOf("Conversation-tab"));
    var conversationID = "#" + "v-pills-" + userID + "Conversation";
    var actualChatBox = document.getElementsByClassName("chat-box active")[0];

    if (actualChatBox != undefined) {
        actualChatBox.classList.remove("active");
    }
    $(conversationID)[0].classList.add("active");

    $(conversationID)[0].scrollTop = $(conversationID)[0].scrollHeight;

    //console.log("Zjezdzam na dół: " + $(conversationID)[0].id);
}

function createMessageWindow() {

    var userLogin = $("#create-message-user").val();
    //console.log("CHCE STWORZYĆ OKNO DO UŻYTKOWNIKA: " + xxx);

    $.ajax({
        url: '/UserPanel/GetUserIdFromName',
        type: 'POST',
        data: {
            userLogin: userLogin
        },
        success: function (data) {
            // CREATE CHAT WITH USER

            if (data == false) {
                alert("Nie znaleziono użytkownika o podanym loginie!");
            }
            else {
                var idHelper = data.userID + "Conversation";
                var tabID = idHelper + "-tab";
                var pID = idHelper + "-p";
                var dateID = idHelper + "-date";
                var conversationID = "v-pills-" + idHelper;
                var ariaLabelled = conversationID + "-tab";
                var hrefLink = "#" + conversationID;

                var actualUserBox = document.getElementsByClassName("user-last-msg active")[0];

                if (actualUserBox != undefined) {
                    actualUserBox.classList.remove("active");
                }
                $("#div-user-boxes").append(
                    "<a onclick=\"scrollToBottom(this.id)\" id=\"" + tabID + "\" data-toggle=\"pill\" href=\"" + hrefLink + "\" role=\tab\" class=\"user-last-msg list-group-item list-group-item-action list-group-item-light rounded-0 active nav-link\">"
                    + "<div class=\"media\">"
                    + "<img src=\"" + data.userAvatarURL + "\" alt=\"user\" width=\"50\" class=\"rounded-circle\">"
                    + "<div class=\"media-body ml-4\">"
                    + "<div class=\"d-flex align-items-center justify-content-between mb-1\">"
                    + "<h6 class=\"mb-0\">" + userLogin + "</h6><small id=\"" + dateID + "\" class=\"small font-weight-bold\"></small>"
                    + "</div>"
                    + "<p style=\"color:lightskyblue\" id=\"" + pID + "\" class=\"font-italic mb-0 text-small\"></p>"
                    + "</div>"
                    + "</div>"
                    + "</a>"
                );

                var actualChatBox = document.getElementsByClassName("chat-box active")[0];

                if (actualChatBox != undefined) {
                    actualChatBox.classList.remove("active");
                }

                $("#div-chat-boxes").append(
                    "<div class=\"px-4 py-5 chat-box bg-white tab-pane active\" id=\"" + conversationID + "\" role=\"tabpanel\" aria-labelledby=\"" + ariaLabelled + "\">"
                    + "</div>"
                );
            }
        },
        error: function () {
            alert("Nie znaleziono użytkownika o podanym loginie!");
        }
    });


    //console.log("Zjezdzam na dół: " + $(conversationID)[0].id);
    $("#create-message-user").val('');
}

(function () {


    var actualChatBox = document.getElementsByClassName("chat-box active")[0];
    if (actualChatBox != undefined) {
        actualChatBox.scrollTop = actualChatBox.scrollHeight;
    }


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
        console.log("Dostałem wiadomość");

        var chatConversationID = "#" + senderID + "Conversation-tab";
        console.log("Muszę znaleźć: " + chatConversationID);
        var chat = $(chatConversationID)[0];
        if (chat == undefined) {
            console.log("Nie masz konwy z takim użytkownikiem i muszę to zrobić.");
            var idHelper = senderID + "Conversation";
            var tabID = idHelper + "-tab";
            var pID = idHelper + "-p";
            var dateID = idHelper + "-date";
            var conversationID = "v-pills-" + idHelper;
            var ariaLabelled = conversationID + "-tab";
            var hrefLink = "#" + conversationID;

            $("#div-user-boxes").prepend(
                "<a onclick=\"scrollToBottom(this.id)\" id=\"" + tabID + "\" data-toggle=\"pill\" href=\"" + hrefLink + "\" role=\tab\" class=\"user-last-msg list-group-item list-group-item-action list-group-item-light rounded-0 nav-link\">"
                + "<div class=\"media\">"
                + "<img src=\"" + avatarImmageURL + "\" alt=\"user\" width=\"50\" class=\"rounded-circle\">"
                + "<div class=\"media-body ml-4\">"
                + "<div class=\"d-flex align-items-center justify-content-between mb-1\">"
                + "<h6 class=\"mb-0\">" + senderName + "</h6><small id=\"" + dateID + "\" class=\"small font-weight-bold\"></small>"
                + "</div>"
                + "<p style=\"color:lightskyblue\" id=\"" + pID + "\" class=\"font-italic mb-0 text-small\"></p>"
                + "</div>"
                + "</div>"
                + "</a>"
            );

            $("#div-chat-boxes").append(
                "<div class=\"px-4 py-5 chat-box bg-white tab-pane\" id=\"" + conversationID + "\" role=\"tabpanel\" aria-labelledby=\"" + ariaLabelled + "\">"
                + "</div>"
            );
        }

        writeReceivedMessage(message, senderID, senderName, avatarImmageURL);
    }

    $.connection.hub.start()
        .done(function () {
            console.log("Hub connected!");

            // chatHub.client.connected = function (id) {
            //     console.log("contextID?: " + id);
            // };
            // WYSYŁANIE WIADOMOŚCI



            // NASŁUCHIWANIE WIADOMOŚCI
            // chatHub.client.receiveMessage = function (message, senderID) {
            //     console.log("Dostano wiadomość!");

            //     writeReceivedMessage(message, senderID);
            // }
        })
        .fail(function () { alert("ERROR"); })

    $("#send-message-btn").on("click", function () {

        console.log("Wysyłam wiadomość");
        var message = $("#message-content").val();
        if (message != "") {
            var user = document.getElementsByClassName("user-last-msg active");
            var receiverID = user[0].id.substring(0, user[0].id.indexOf("Conversation"));

            writeSentMessage(message, receiverID);
            chatHub.server.sendMessage(message, receiverID);
        }
    });

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

        if (paragraph != undefined) {
            paragraph.innerHTML = senderName + ": " + message;
        }

        if (date != undefined) {
            date.innerHTML = hours + ":" + minutes;

        }


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

        if (actualChatBox != undefined && receivedMessageChatBox != undefined && actualChatBox.id == receivedMessageChatBox.id) {
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
