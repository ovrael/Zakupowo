(function () {

    $('#message-content').keypress(function (e) {
        if (e.which == 13) {//Enter key pressed
            $('#send-message-btn').trigger('click');//Trigger search button click event
        }
    });

    $("#message-send-form").keypress(function (event) {
        return event.keyCode != 13;
    });

    //$("#send-message-btn").on("click", function () {

    //    var user = document.getElementsByClassName("user-last-msg active");
    //    var userID = user[0].id.substring(0, user[0].id.indexOf("Conversation"));


    //    writeMessage($("#message-content").val());
    //});

    //ŁĄCZY SIĘ Z HUBEM
    var chatHub = $.connection.chatHub;
    $.connection.hub.start()
        .done(function () {
            console.log("Hub connected!");
            //$('#send-message-btn').click(function () {
            //    //// Call the Send method on the hub.  
            //    ////var connId = $("#connId").val();
            //    ////var frndConnId = $("#frndConnId").val();
            //    //var finalConnId = $.connection.hub.id;
            //    //chatHub.server.send("Name xD", $('#message-content').val(), finalConnId);
            //    ////$("#connId").val($.connection.hub.id);

            //    //// Clear text box and reset focus for next comment.  
            //    //$('#chat-window').append("<p>" + $('#message').val() + "</p>");

            //    //$('#message-content').val('').focus();
            //});
        })
        .fail(function () { alert("ERROR"); })

    //// WYWOŁUJE METODE TestMessage z ChatHub po stronie klienta
    //chatHub.client.testMessage = function (message) {
    //    writeMessage(message);
    //}

    //chat.client.addNewMessageToPage = function (message) {
    //    // Add the message to the page.
    //    $('#chat-window').append();
    //};

    //TU SIĘ WYSYŁA WIADOMOŚĆ (POBIERAJĄ TEŻ DANE)
    $("#send-message-btn").on("click", function () {

        var user = document.getElementsByClassName("user-last-msg active");
        var userID = user[0].id.substring(0, user[0].id.indexOf("Conversation"));

        var message = $("#message-content").val();
        //console.log("Wysyłam wiadomość");
        writeMessage(message);
        chatHub.server.sendMessage(message, userID);
    });


    var writeMessage = function (message) {

        //console.log("Chce napisać wiadomość na stronie");
        console.log("Wiadomość: " + message);
        // WYBIERA ELEMENT Z ID chat-window A NASTĘPNIE DODAJE POD NIM KOD HTML
        //var currentDateTimeForMessage;

        //var x = chatHub.server.getCurrentDateTime()
        //    .done(function (data) { return data; });
        //console.log("Czas: " + x);

        var date = new Date();
        var chatBox = document.getElementsByClassName("chat-box active");
        var conversationID = "#" + chatBox[0].id;

        $(conversationID).append(
            "<div class=\"media w-50 ml-auto mb-3 message\">"
            + "<div class=\"media-body\">"
            + "<div class=\"bg-primary rounded py-2 px-3 mb-2\" data-toggle=\"tooltip\" title=\""
            + date.getHours() + ":" + date.getMinutes()
            + "\">"
            + "<p class=\"text-small mb-0 text-white\">" + message + "</p>"
            + "</div>"
            + "</div>"
            + "</div>"
        );

        chatBox[0].scrollTop = chatBox[0].scrollHeight;

        $('#message-content').val('');
    }

    //<div class="media w-50 ml-auto mb-3 message">
    //    <div class="media-body">
    //        <div class="bg-primary rounded py-2 px-3 mb-2" data-toggle="tooltip" title="@customSentDate">
    //            <p class="text-small mb-0 text-white">@message.Content</p>
    //        </div>

    //    </div>
    //</div>

})()
