$().ready(function () {
    $("#addOfferForm").validate({
        errorPlacement: function (error, element) {
            error.appendTo($("#" + element.attr('id') + "_validation"));
        },
        rules: {
            Name: {
                maxlength: 60
            }
            // passwordRegisterInput2: {
            //     required: true,
            //     equalTo: "#passwordRegisterInput"
            // },
            // BirthDate: {
            //     required: true,
            //     minAge: 13
            // }
        },
        messages: {
            Name: {
                maxlength: "Nazwa może zawierać maksymalnie 60 znaków."
            }
            // passwordRegisterInput2: {
            //     equalTo: "Hasła się nie zgadzają."
            // },
            // BirthDate: {
            //     minAge: "Musisz mieć co najmniej 13 lat."
            // }
        },
    });
});