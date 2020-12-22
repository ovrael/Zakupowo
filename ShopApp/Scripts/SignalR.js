(function () {

    //$("#send-message-btn").on("click", function () {

    //    var user = document.getElementsByClassName("user-last-msg active");
    //    var userID = user[0].id.substring(0, user[0].id.indexOf("Conversation"));



    //    writeMessage($("#message-content").val());
    //});


    //ŁĄCZY SIĘ Z HUBEM
    var chatHub = $.connection.chatHub;
    $.connection.hub.start()
        .done(function () {
            $('#send-message-btn').click(function () {
                // Call the Send method on the hub.  
                //var connId = $("#connId").val();
                //var frndConnId = $("#frndConnId").val();
                var finalConnId = $.connection.hub.id;
                chatHub.server.send("Name xD", $('#message-content').val(), finalConnId);
                //$("#connId").val($.connection.hub.id);

                // Clear text box and reset focus for next comment.  
                $('#chat-window').append("<p>" + $('#message').val() + "</p>");

                $('#message-content').val('').focus();
            });
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

    // TU SIĘ WYSYŁA WIADOMOŚĆ (POBIERAJĄ TEŻ DANE)
    //$("#send-message-btn").on("click", function () {

    //    //alert("Wysyłam wiadomość");
    //    writeMessage($("#message-content").val());
    //});

    // FOKUS NA MESSAGE 
    $('#message-content').focus();
    $('#message-content').keypress(function (e) {
        if (e.which == 13) {//Enter key pressed
            $('#send-message-btn').trigger('click');//Trigger search button click event
        }
    });

    //var writeMessage = function (message) {
    //    // WYBIERA ELEMENT Z ID chat-window A NASTĘPNIE DODAJE POD NIM KOD HTML
    //    $("#chat-window").append("<h2>" + message + " </h2><br />");
    //}

})()
