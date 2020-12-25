function SendActivationEmail() {

    var userLogin = document.getElementById("user-login").innerHTML;

    $.ajax({
        url: '/UserPanel/SendActivationEmail',
        type: 'POST',
        data: {
            userLogin: userLogin
        },
        success: function (data) {
            // CREATE CHAT WITH USER

            if (data == false) {
                alert("Błąd wysyłania emaila!");
            }
            else {

                alert("Wysłano link aktywacyjny na zarejestrowany email: " + data.email + ".");
            }
        },
        error: function () {
            alert("Błąd wysyłania emaila!");
        }
    });
}