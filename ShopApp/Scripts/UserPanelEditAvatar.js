$(document).ready(function () {

    var image_crop = $('#image_demo').croppie({
        viewport: {
            width: 200,
            height: 200,
            type: 'square'
        },
        boundary: {
            width: 450,
            height: 450
        }
    });

    $('#avatarFile').on('change', function () {
        var reader = new FileReader();
        reader.onload = function (event) {
            image_crop.croppie('bind', {
                url: event.target.result,
            });
        }
        reader.readAsDataURL(this.files[0]);
        $('#uploadimageModal').modal('show');
    });

    $('.crop_image').click(function (event) {
        var formData = new FormData();
        image_crop.croppie('result', { type: 'blob', format: 'png' }).then(function (blob) {

            formData.append('croppedImage', blob, 'newAvatar.jpg');
            console.log(blob);

            ajaxFormPost(formData);
        });
        $('#uploadimageModal').modal('hide');
    });

    /// Ajax Function
    function ajaxFormPost(formData) {
        $.ajax({
            url: "/UserPanel/EditAvatar",
            type: 'POST',
            data: formData,
            cache: false,
            async: true,
            processData: false,
            contentType: false,
            timeout: 5000,
            beforeSend: function () {
            },
            success: function (response) {
                createCustomAlert("Zmieniono avatar!", response);
            },
            complete: function () {
            }
        });
    }

});

function createCustomAlert(title, message) {
    $.alert({
        title: title,
        content: message,
        buttons: {
            ok: {
                text: 'ok',
                btnClass: 'btn-popout'
            }
        }
    });
}