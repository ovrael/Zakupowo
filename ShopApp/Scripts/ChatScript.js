function scrollToBottom(clickedElementID) {

    var userID = clickedElementID.substring(0, clickedElementID.indexOf("Conversation-tab"));

    var conversationID = "#" + "v-pills-" + userID + "Conversation";
    var actualChatBox = document.getElementsByClassName("chat-box active")[0];

    if (actualChatBox != undefined) {
        actualChatBox.classList.remove("active");
    }

    var userChatBox = document.getElementById(clickedElementID);
    if (userChatBox != undefined) {

        if (userChatBox.classList.contains("undread-msg")) {
            userChatBox.classList.remove("undread-msg");
            var senderID = parseInt(userID, 10);

            $.ajax({
                url: "/UserPanel/ReadMessages",
                method: 'POST',
                dataType: 'json',
                data: '{"senderID":"' + senderID + '"}',
                contentType: 'application/json; charset=utf-8',
                success: function (returnData) {
                },
                // error handling
            });
        }
    }


    $(conversationID)[0].classList.add("active");

    $(conversationID)[0].scrollTop = $(conversationID)[0].scrollHeight;
}

function createMessageWindow(userLogin) {

    var users = jQuery('.user-row');
    $(users).each(function () {
        jQuery(this).remove();
    })



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
            else if (document.getElementById(data.userID + "Conversation-tab") != undefined) {

                var activeUserBox = document.getElementsByClassName("user-last-msg active")[0];
                activeUserBox.classList.remove("active");
                var activeConversationPill = document.getElementsByClassName("chat-box active")[0];
                activeConversationPill.classList.remove("active");


                document.getElementById(data.userID + "Conversation-tab").classList.add("active");
                document.getElementById("v-pills-" + data.userID + "Conversation").classList.add("active");

                return;
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
                //$("#div-user-boxes").append(
                //    "<a onclick=\"scrollToBottom(this.id)\" id=\"" + tabID + "\" data-toggle=\"pill\" href=\"" + hrefLink + "\" role=\tab\" class=\"user-last-msg list-group-item list-group-item-action list-group-item-light rounded-0 active nav-link\">"
                //    + "<div class=\"media\">"
                //    + "<img src=\"" + data.userAvatarURL + "\" alt=\"user\" width=\"50\" class=\"rounded-circle\">"
                //    + "<div class=\"media-body ml-4\">"
                //    + "<div class=\"d-flex align-items-center justify-content-between mb-1\">"
                //    + "<h6 class=\"mb-0\">" + userLogin + "</h6><small id=\"" + dateID + "\" class=\"small font-weight-bold\"></small>"
                //    + "</div>"
                //    + "<p style=\"color:lightskyblue\" id=\"" + pID + "\" class=\"font-italic mb-0 text-small\"></p>"
                //    + "</div>"
                //    + "</div>"
                //    + "</a>"
                //);
                $("#div-user-boxes").append(
                    "<a onclick=\"scrollToBottom(this.id)\" id=\"" + tabID + "\" data-toggle=\"pill\" href=\"" + hrefLink + "\" role=\tab\" class=\"user-last-msg list-group-item list-group-item-action list-group-item-light rounded-0 active nav-link\">"
                    + "<div class=\"users-box-content ml-3\" style=\"display:grid\">"
                    + "<div class=\"row media-body\">"
                    + "<div class=\"d-flex ustify-content-between mb-1\">"
                    + "<img src=\"" + data.userAvatarURL + "\" alt=\"user\" width=\"45\" class=\"p-2 rounded-circle\">"
                    + "<h6 class=\"p-2 col-6 mb-0 pl-3\">" + userLogin + "</h6><small font-weight-bold justify-content-lg-end id=\"" + dateID + "\" class=\"p-2 mr-5  small font-weight-bold\"></small>"
                    + "</div>"
                    + "<div style=\"color:black;overflow:hidden\" id=\"" + pID + "\" class=\"col-10 short-ms pl-3 ml-5 font-italic mb-0 text-small\"></div>"
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
        if (e.which == 13) {
            if ($("#message-content").val() != "") {
                $('#send-message-btn').trigger('click');
            }
        }
    });

    $("#create-message-user").keypress(function (e) {
        if (e.which == 13) {
            if ($("#create-message-user").val() != "") {
                $("#create-message-user-btn").trigger('click');
            }
        }
    });


    $("#message-send-form").keypress(function (event) {
        return event.keyCode != 13;
    });

    $("#create-message-user-form").keypress(function (event) {
        return event.keyCode != 13;
    });

    var chatHub = $.connection.chatHub;

    // OTRZYMYWANIE WIADOMOŚCI
    chatHub.client.receiveMessage = function (message, senderID, senderName, avatarImmageURL) {

        var chatConversationID = "#" + senderID + "Conversation-tab";

        var chat = $(chatConversationID)[0];

        // JEŚLI MAM KONWERSACJĘ Z UŻTKOWNIKIEM -> Przenieś okno z użytkownikiem na górę
        if (chat != undefined) {
            var idHelper = senderID + "Conversation";
            var tabID = idHelper + "-tab";
            var userWindow = document.getElementById(tabID);

            document.getElementById(tabID).remove();
            document.getElementById("div-user-boxes").prepend(userWindow);
        }


        // JEŚLI NIE MAM KONWERSACJI Z DANYM UŻYTKOWNIKIEM MUSZĘ JĄ UTWORZYĆ
        if (chat == undefined) {

            var idHelper = senderID + "Conversation";
            var conversationID = "v-pills-" + idHelper;
            var ariaLabelled = conversationID + "-tab";

            createUserWindow(senderName, senderID, avatarImmageURL);

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
        })
        .fail(function () {
            console.log("Can't connect to hub!");
        })

    $("#send-message-btn").on("click", function () {

        var message = $("#message-content").val();
        if (message != "") {
            var user = document.getElementsByClassName("user-last-msg active");
            var receiverID = user[0].id.substring(0, user[0].id.indexOf("Conversation"));

            var chatConversationID = "#" + receiverID + "Conversation-tab";
            var chat = $(chatConversationID)[0];

            if (chat != undefined) {
                var idHelper = receiverID + "Conversation";
                var tabID = idHelper + "-tab";
                var userWindow = document.getElementById(tabID);

                document.getElementById(tabID).remove();
                document.getElementById("div-user-boxes").prepend(userWindow);
            }

            var usersBox = document.getElementsByClassName("messages-box")[0];
            usersBox.scrollTop = 0;

            writeSentMessage(message, receiverID);
            chatHub.server.sendMessage(message, receiverID);
        }
    });

    $("#createMessageBtn").on("click", function () {

        var message = $("#messageContent").val();
        if (message != "") {
            // var receiver = $(".receiver-label");
            var receiver = document.getElementsByClassName("receiver-label")[0];
            var receiverID = receiver.id.split("-")[1];

            if (receiverID != null) {
                chatHub.server.sendMessage(message, receiverID);
            }
        }
        var message = $("#messageContent").val("");
        $('#sendMessageModal').modal('hide');
    });

    var createUserWindow = function (senderName, senderID, avatarImmageURL) {
        var idHelper = senderID + "Conversation";
        var tabID = idHelper + "-tab";
        var pID = idHelper + "-p";
        var dateID = idHelper + "-date";
        var conversationID = "v-pills-" + idHelper;
        var hrefLink = "#" + conversationID;

        //$("#div-user-boxes").prepend(
        //    "<a onclick=\"scrollToBottom(this.id)\" id=\"" + tabID + "\" data-toggle=\"pill\" href=\"" + hrefLink + "\" role=\tab\" class=\"user-last-msg list-group-item list-group-item-action list-group-item-light rounded-0 nav-link\">"
        //    + "<div class=\"media\">"
        //    + "<img src=\"" + avatarImmageURL + "\" alt=\"user\" width=\"50\" class=\"rounded-circle\">"
        //    + "<div class=\"media-body ml-4\">"
        //    + "<div class=\"d-flex align-items-center justify-content-between mb-1\">"
        //    + "<h6 class=\"mb-0\">" + senderName + "</h6><small id=\"" + dateID + "\" class=\"small font-weight-bold\"></small>"
        //    + "</div>"
        //    + "<p style=\"color:lightskyblue\" id=\"" + pID + "\" class=\"font-italic mb-0 text-small\"></p>"
        //    + "</div>"
        //    + "</div>"
        //    + "</a>"
        //);
        $("#div-user-boxes").append(
            "<a onclick=\"scrollToBottom(this.id)\" id=\"" + tabID + "\" data-toggle=\"pill\" href=\"" + hrefLink + "\" role=\tab\" class=\"user-last-msg list-group-item list-group-item-action list-group-item-light rounded-0 active nav-link\">"
            + "<div class=\"users-box-content ml-3\">"
            +"<div class=\"users-box-content ml-3\" style=\"display:grid\">"
            + "<div class=\"d-flex justify-content-between mb-1\">"
            + "<img src=\"" + avatarImmageURL + "\" alt=\"user\" width=\"45\" class=\"p-2 rounded-circle img-user-box\">"
            + "<h6 class=\"p-2 col-6 mb-0 pl-3\">" + userLogin + "</h6><small  id=\"" + dateID + "\" class=\"p-2 mr-5  small font-weight-bold \"></small>"
            + "</div>"
            + "<div style=\"color:black;overflow:hidden\" id=\"" + pID + "\" class=\"col-10 short-ms pl-3 ml-5  font-italic mb-0 text-small\"></div>"
            + "</div>"
            + "</div>"
            + "</a>"
        );
    }

    var writeReceivedMessage = function (message, senderID, senderName, avatarImageUrl) {

        var conversationID = "#" + "v-pills-" + senderID + "Conversation";
        var paragraphID = "#" + senderID + "Conversation-p";
        var mediaID = senderID + "Conversation-media";
        var userChatBoxID = senderID + "Conversation-tab";
        var dateID = "#" + senderID + "Conversation-date";

        var date = new Date();
        var minutes = date.getMinutes() >= 10 ? date.getMinutes() : "0" + date.getMinutes();
        var hours = date.getHours() >= 10 ? date.getHours() : "0" + date.getHours();

        var actualChatBox = document.getElementsByClassName("chat-box active")[0];
        var receivedMessageChatBox = $(conversationID)[0];
        var paragraph = $(paragraphID)[0];
        var date = $(dateID)[0];

        if (paragraph != undefined) {
            paragraph.innerHTML = message;
        }

        if (date != undefined) {
            date.innerHTML = hours + ":" + minutes;
        }

        var userChatBox = document.getElementById(userChatBoxID);
        var actualPickedUser = document.getElementsByClassName("user-last-msg active")[0];
        if (userChatBox != undefined && userChatBox != actualPickedUser) {
            userChatBox.classList.add("undread-msg");
        }
        if (userChatBox != undefined && userChatBox == actualPickedUser) {

            var senderID = parseInt(senderID, 10);

            $.ajax({
                url: "/UserPanel/ReadMessages",
                method: 'POST',
                dataType: 'json',
                data: '{"senderID":"' + senderID + '"}',
                contentType: 'application/json; charset=utf-8',
                success: function (returnData) {
                },
                // error handling
            });

        }


        $(conversationID).append(
            "<div class=\"media w-50 mb-3 message\">"
            + "<img src=\"" + avatarImageUrl + "\" alt=\"user\" width=\"20\" class=\"rounded-circle\">"
            + "<div class=\"media-body ml-3\">"
            + "<div class=\"bg-light rounded py-2 px-3 mb-2\" style=\"word-break:break-all\" data-toggle=\"tooltip\" title=\""
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
            + "<div class=\"bg-primary rounded py-2 px-3 mb-2\" style=\"word-break:break-all\" data-toggle=\"tooltip\" title=\""
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


    $("#create-message-user").keyup(function (e) {
        var findUser = jQuery(this).val().toLowerCase();

        if (findUser != "") {
            $.ajax({
                url: '/UserPanel/UsersList',
                type: 'POST',
                dataType: 'json',
                data: '{"userLogin":"' + findUser + '"}',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {

                    clearUserList();

                    for (let i = 0; i < data.length; i++) {
                        const user = data[i];
                        var userTR = createUserTR(user);
                        $("#usersTableBody").eq(0).append(userTR);
                    }
                },
                error: function () {
                    alert("Nie udało się :/");
                }
            });
        }

        if (findUser === "") {
            clearUserList();
        }

    })

    function clearUserList() {
        var users = jQuery('.user-row');
        $(users).each(function () {
            jQuery(this).remove();
        })
    }

    function createUserTR(user) {

        var userAvatar = "<div class=\"col-3 pl-4\">"
            + "<img src=\"" + user.AvatarUrl + "\" alt=\"user\" width=\"50\" class=\"rounded-circle\">"
            + "</div>";

        //var userData = "<div style=\"color:black\" class=\"col-9\">"
        var userData = "<div style=\"color:black\" class=\"col-10 pl-4 pb-2 mr-5\">"
            + user.Login + "-" + user.FirstName
            + "</div>";

        //var userTR = "<a href=\"#\" onclick=\"createMessageWindow(this.id)\" class=\"user-row row\" id=\"" + user.Login + "\" style=\"background-color:lightblue;\">"
        var userTR = "<a href=\"#\" onclick=\"createMessageWindow(this.id)\" class=\"user-row row\" id=\"" + user.Login + "\" style=\"background-color:white;padding-bottom:10px;padding-top:10px;border:solid;border-width:thin;border-color:lightgray;\">"
            + userAvatar
            + userData
            + "</a>";
         



        return userTR;
    }

})()
