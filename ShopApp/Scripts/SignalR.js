(function ()
{
    //ŁĄCZY SIĘ Z HUBEM
    $.connection.hub.start()
        .done(function ()
        {
            console.log("Connection works!");
            // WYWOŁUJE METODE TestMessage z ChatHub po stronie serwera
            $.connection.chatHub.server.testMessage("Connected!");
        })
        .fail(function () { alert("ERROR"); })

    // WYWOŁUJE METODE TestMessage z ChatHub po stronie klienta
    $.connection.chatHub.client.testMessage = function (message)
    {
        writeMessage(message);
    }

    var writeMessage = function (message) {
        // WYBIERA ELEMENT Z ID chat-window A NASTĘPNIE DODAJE POD NIM KOD HTML
        $("#chat-window").append("<h2>" + message + " </h2><br />");
    }

})()
