$().ready(function () {
    $("#signinForm").validate({
        errorPlacement: function (error, element) {
            error.appendTo($("#" + element.attr('id') + "_validation"));
        },
        rules: {
            EncryptedPassword: {
                required: true,
                minlength: 8
            },
            passwordRegisterInput2: {
                required: true,
                equalTo: "#passwordInput"
            }
        },
        messages: {
            EncryptedPassword: {
                minlength: "Hasło musi posiadać co najmniej 8 znaków."
            },
            passwordRegisterInput2: {
                equalTo: "Hasła się nie zgadzają."
            },
        },
    });
});