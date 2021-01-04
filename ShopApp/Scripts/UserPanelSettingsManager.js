(function () {

    $(".setting-link").click(function (e) {

        var activeLI = document.getElementsByClassName("links active")[0];
        activeLI.classList.remove("active");

        document.getElementById(this.id.split("-")[0] + "-li").classList.add("active");
    })

    $('#basicDataInput').click(function (e) {
        e.preventDefault();

        var formData = $("#basicDataForm").serialize();

        $.ajax({
            url: "/UserPanel/EditBasicData",
            method: 'POST',
            data: formData,
            success: function (returnData) {
                alert(returnData);
            },
            // error handling
        });
    });

    $('body').on("click", '.add-adress-btn', function (e) {
        e.preventDefault();

        var formData = $("#addNewAdressForm").serialize();
        $.ajax({
            url: "/UserPanel/AddNewShippingAdress",
            method: 'POST',
            data: formData,
            success: function (returnData) {

                if (returnData != false) {

                    var pDivs = document.getElementsByClassName("adress-p");
                    var adressNumber = -1;
                    if (pDivs.length != 0) {
                        adressNumber = pDivs[pDivs.length - 1].id.split("-")[1];
                        adressNumber = parseInt(adressNumber, 10);

                        if (adressNumber % 2 == 1) {
                            var barDiv = "<div class=\"col-12 adress-bar\">"
                                + "<hr style=\"border: 3px solid #ffb24a; border-radius: 3px\">"
                                + "</div>"
                            $("#adressContainer").eq(0).append(barDiv);
                        }
                    }

                    var adressDiv = createAdressDiv(returnData, (adressNumber + 1));
                    $("#adressContainer").eq(0).append(adressDiv);
                }
                else {
                    alert("Nie udało się dodać adresu.");
                }
            },
            // error handling
        });

        document.getElementById("addNewAdressForm").reset();
        $('#newAdressModal').modal('hide');
    });

    $('body').on("click", '.change-adress-btn', function (e) {
        e.preventDefault();

        var formID = this.id.split("-")[1];

        var formData = $("#adressForm-" + formID).serialize();

        $.ajax({
            url: "/UserPanel/EditShippingAdress",
            method: 'POST',
            data: formData,
            success: function (returnData) {
                alert(returnData);
            },
            // error handling
        });
    });

    $('body').on("click", '.delete-adress-btn', function (e) {
        e.preventDefault();

        var formID = this.id.split("-")[1];

        $.ajax({
            url: "/UserPanel/DeleteShippingAdress",
            method: 'POST',
            dataType: 'json',
            data: '{"adressID":"' + formID + '"}',
            contentType: 'application/json; charset=utf-8',
            success: function (returnData) {

                var adresses = jQuery('.adress-div');
                $(adresses).each(function () {
                    jQuery(this).remove();
                })

                var bars = jQuery('.adress-bar');
                $(bars).each(function () {
                    jQuery(this).remove();
                })

                for (let i = 0; i < returnData.length; i++) {
                    const adress = returnData[i];
                    var adressDiv = createAdressDiv(adress, i);
                    $("#adressContainer").eq(0).append(adressDiv);

                    if (i % 2 == 1 && returnData.length > 2) {
                        var barDiv = "<div class=\"col-12 adress-bar\">"
                            + "<hr style=\"border: 3px solid #ffb24a; border-radius: 3px\">"
                            + "</div>"
                        $("#adressContainer").eq(0).append(barDiv);
                    }
                }
            },
            // error handling
        });
    });

    $('#passwordInput').click(function (e) {
        e.preventDefault();
        var formData = $("#passwordForm").serialize();

        $.ajax({
            url: "/UserPanel/EditPassword",
            method: 'POST',
            data: formData,
            success: function (returnData) {
                alert(returnData);
            },
            // error handling
        });
    });

    $('#avatarInput').click(function (e) {
        e.preventDefault();

        var input = document.getElementById("avatarFile");
        var files = input.files;
        var fileData = new FormData();

        for (var i = 0; i != files.length; i++) {
            fileData.append("files", files[i]);
        }

        $.ajax({
            url: "/UserPanel/EditAvatar",
            data: fileData,
            processData: false,
            contentType: false,
            type: "POST",
            success: function (returnData) {
                alert(returnData);
            },
            // error handling
        });
    });

    $(document).on("change", "#avatarFile", function () {

        var files = this.files != null ? this.files : [];
        if (files.length <= 0 || window.FileReader == null) return;


        newAvatar = files[0];

        var reader = new FileReader();
        reader.readAsDataURL(newAvatar);

        reader.onloadend = function () {
            document.getElementsByClassName("output")[0].setAttribute("src", this.result);
        }
    });

})()

function createAdressDiv(adress, number) {

    var adressFormID = "adressForm-" + adress.AdressID;
    var adressInputID = "adressInput-" + adress.AdressID;
    var adressDeleteID = "adressDelete-" + adress.AdressID;
    var adressBodyID = "adressBody-" + adress.AdressID;

    var countryDiv = "<div class=\"form-group\">"
        + "<label class=\"col-md-4 control-label\" for=\"Country\">Kraj</label>"
        + "<div class=\"col-md-10\">"
        + "<input id=\"Country\" name=\"Country\" value=\"" + adress.Country + "\" class=\"form-control input-md\" type=\"text\" required autofocus>"
        + "</div>"
        + "</div>";

    var cityDiv = "<div class=\"form-group\">"
        + "<label class=\"col-md-4 control-label\" for=\"City\">Miasto</label>"
        + "<div class=\"col-md-10\">"
        + "<input id=\"City\" name=\"City\" value=\"" + adress.City + "\" class=\"form-control input-md\" type=\"text\" required>"
        + "</div>"
        + "</div>";

    var streetDiv = "<div class=\"form-group\">"
        + "<label class=\"col-md-4 control-label\" for=\"Street\">Ulica</label>"
        + "<div class=\"col-md-10\">"
        + "<input id=\"Street\" name=\"Street\" value=\"" + adress.Street + "\" class=\"form-control input-md\" type=\"text\" required>"
        + "</div>"
        + "</div>";

    var premisesDiv = "<div class=\"form-group\">"
        + "<label class=\"col-md-4 control-label\" for=\"PremisesNumber\">Numer lokalu*</label>"
        + "<div class=\"col-md-10\">"
        + "<input id=\"PremisesNumber\" name=\"PremisesNumber\" value=\"" + adress.PremisesNumber + "\" class=\"form-control input-md\" type=\"text\" required>"
        + "<small id=\"premisesNumberHelp\" class=\"form-text text-muted\">Pole nie jest wymagane.</small>"
        + "</div>"
        + "</div>";

    var postalDiv = "<div class=\"form-group\">"
        + "<label class=\"col-md-4 control-label\" for=\"PostalCode\">Kod pocztowy*</label>"
        + "<div class=\"col-md-10\">"
        + "<input id=\"PostalCode\" name=\"PostalCode\" value=\"" + adress.PostalCode + "\" class=\"form-control input-md\" type=\"text\" required>"
        + "<small id=\"postalCodeHelp\" class=\"form-text text-muted\">Pole nie jest wymagane.</small>"
        + "</div>"
        + "</div>";

    var buttons = "<div class=\"form-group\">"
        + "<div class=\"row\">"
        + "<div class=\"col-md-5\">"
        + "<button class=\"btn btn-success change-adress-btn\" value=\"Zmień\" id=\"" + adressInputID + "\">Zmień</button>"
        + "</div>"
        + "<div class=\"col-md-5\">"
        + "<button class=\"btn btn-danger delete-adress-btn\" value=\"Usuń\" id=\"" + adressDeleteID + "\">Usuń</button>"
        + "</div>"
        + "</div>"
        + "</div>";

    var adressDiv =
        "<div class=\"col-6 adress-div\" id=\"" + adressBodyID + "\">"
        + "<form class=\"form-horizontal adress-form\" method=\"post\" id=\"" + adressFormID + "\">"
        + "<p id=\"p-" + number + "\" class=\"adress-p\"> Adres wysyłki numer: " + (number + 1) + "</p>"
        + "<input id=\"AdressID\" name=\"AdressID\" value=\"" + adress.AdressID + "\" class=\"hidden form-control input-md\" type=\"number\" hidden>"
        + countryDiv
        + cityDiv
        + streetDiv
        + premisesDiv
        + postalDiv
        + buttons
        + "</form>"
        + "</div>";

    return adressDiv;
}