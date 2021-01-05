
(function () {

    var imagesToUpload = [];

    $(document).on("click", "i.delete-image", function () {
        deleteImageFromList($(this)[0].id);
        $(this).parent().parent().remove();
        findFirstPhoto();
    });

    $(document).on("change", "#UploadImageInput", function () {
        var files = this.files != null ? this.files : [];
        if (files.length <= 0 || window.FileReader == null) return;

        //deleteOldImages();

        var imageCounter = 0;

        var allUploadedImages = document.getElementsByClassName("imgUp");

        if (allUploadedImages.length > 0) {
            var lastImage = allUploadedImages[allUploadedImages.length - 1];
            imageCounter = parseInt(lastImage.id.substring(lastImage.id.indexOf("-") + 1));
            imageCounter += 1;
        }


        for (let i = 0; i < files.length; i++) {
            const image = files[i];

            if (/^image/.test(image.type)) {
                var reader = new FileReader();
                reader.readAsDataURL(image);

                reader.onloadend = function () {

                    if (i > 0)
                        mainPhoto = "";

                    var imageName = image.name.toLowerCase();
                    imageName = imageName.replace(" ", "");

                    $("#UploadedImagesRow").append(
                        "<div id=\"UploadOfferImageNo-" + imageCounter + "\" class=\"col-lg-3 col-md-4 col-sm-6 imgUp mt-4\" title=\"" + imageName + "\">"
                        + "<div class=\"imagePreview offerImage\" style=\"background-image:url(" + this.result + ")\">"
                        + "<i id=\"" + imageName + "\" class=\"fa fa-trash btn delete-image\" style=\"float: right; margin: 5px;\"></i>"
                        + "</div>"
                        + "</div>"
                    );

                    imagesToUpload.push(image);
                    imageCounter += 1;

                    if (i == files.length - 1) {
                        reappendAddImgBtn();
                        findFirstPhoto();
                    }
                }
            }
        }


    });

    var reappendAddImgBtn = function () {
        var uploadImageDiv = document.getElementById("UploadImageDiv");
        $("#UploadImageDiv").remove();
        $("#UploadedImagesRow").append(uploadImageDiv);
    }

    var deleteOldImages = function () {
        var oldImages = $(".delete-image");

        if (oldImages.length > 0) {
            for (let i = 0; i < oldImages.length; i++) {
                const oldImage = oldImages[i];
                oldImage.parentNode.parentNode.remove();
            }
        }
    }

    var printArray = function (array) {
        console.log("Tablica: " + array.length)
        for (let i = 0; i < array.length; i++) {
            const element = array[i];

            console.log("Nazwa: " + element.name);

        }
    }

    var deleteImageFromList = function (imageName) {

        for (let i = 0; i < imagesToUpload.length; i++) {

            if (imagesToUpload[i].name.toLowerCase().replace(" ", "") == imageName) {
                const index = imagesToUpload.indexOf(imagesToUpload[i]);
                if (index > -1) {
                    imagesToUpload.splice(index, 1);
                }
            }
        }
    }


    $("#createOffer").click(function () {

        if (window.FormData == undefined)
            alert("Error: FormData is undefined");

        else {
            var fileData = new FormData();

            for (let i = 0; i < imagesToUpload.length; i++) {
                fileData.append(imagesToUpload[i].name, imagesToUpload[i]);
            }

            $.ajax({
                url: '/UserPanel/UploadOfferImages',
                type: 'post',
                datatype: 'json',
                contentType: false,
                processData: false,
                async: false,
                data: fileData,
                success: function (response) {
                    console.log(response);
                }
            });
        }

    });

    var findFirstPhoto = function () {
        var tooltipText = "To zdjecie będzie przedstawiać ofertę. \nReszta zdjęć będzie dostępna dopiero po wejściu w odpowiednią ofertę.";

        var mainPhoto =
            "<div id=\"main-photo-label\" class=\"col-12 text-center\" data-toggle=\"tooltip\" title=\"" + tooltipText + "\" style=\"background-color:#FFB24A; position:absolute; bottom:0;\">"
            + "<h6><i><strong>" + "Zdjęcie główne oferty" + "</strong></i></h6>"
            + "</div>";

        if (document.getElementById("main-photo-label") == undefined)
            if ($(".offerImage")[0] != undefined)
                $(".offerImage").eq(0).append(mainPhoto);
    }



})()
