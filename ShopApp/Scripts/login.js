$().ready(function () {
    $("#signinForm").validate({
        errorPlacement: function (error, element) {
            error.appendTo($("#" + element.attr('id') + "_validation"));
        },
        rules: {
            passwordInput: {
                required: true,
                minlength: 8
            },
        },
        messages: {
            passwordInput: {
                required: "Podaj swoje hasło",
                minlength: "Hasło musi posiadać co najmniej 8 znaków"
            },
        },
    });
});