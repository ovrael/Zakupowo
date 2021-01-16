$().ready(function () {
    $("#signupForm").validate({
        errorPlacement: function (error, element) {
            error.appendTo($("#" + element.attr('id') + "_validation"));
        },
        rules: {
            Password: {
                minlength: 8
            },
            passwordRegisterInput2: {
                required: true,
                equalTo: "#passwordRegisterInput"
            },
            BirthDate: {
                required: true,
                minAge: 13
            }
        },
        messages: {
            Password: {
                minlength: "Hasło musi posiadać co najmniej 8 znaków."
            },
            passwordRegisterInput2: {
                equalTo: "Hasła się nie zgadzają."
            },
            BirthDate: {
                minAge: "Musisz mieć co najmniej 13 lat."
            }
        },
    });
});