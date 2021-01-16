$().ready(function () {
    $("#signinForm").validate({
        errorPlacement: function (error, element) {
            error.appendTo($("#" + element.attr('id') + "_validation"));
        },
        rules: {
            emailAddressInput: "required",
            passwordInput: {
                required: true,
                minlength: 8
            },
        },
        messages: {
            emailAddressInput: "Podaj swój e-mail",
            passwordInput: {
                required: "Podaj swoje hasło",
                minlength: "Hasło musi posiadać co najmniej 8 znaków"
            },
        },
    });
});