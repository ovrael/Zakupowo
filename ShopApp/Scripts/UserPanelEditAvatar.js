(function () {

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
            $('.output').croppie();
        }
    });

})()