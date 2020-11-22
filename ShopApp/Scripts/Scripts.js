function ShowImagePreview(imageUploader, previewImage) {
    if (imageUploader.files && imageUploader.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            jQuery(previewImage).attr('src', e.target.result);
        }
        reader.readAsDataURL(imageUploader.files[0]);
    }
}

function jQueryAjaxPost(form) {
    
    jQuery.validator.unobtrusive.parse(form);
    if (jQuery(form).valid()) {

        var formData = new FormData(form);
        $.ajax({
            type: "POST",
            url: form.action,
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                console.log(formData);
            }
        });

        //if (jQuery(form).attr('enctype') == "multipart/form-data") {
        //    ajaxConfig["contentType"] == false;
        //    ajaxConfig["processData"] == false; 
        //}

   
    }
    return false;
}


jQuery(document).ready(function () {
   
    jQuery('.product-search').on('input change', function () {
        var offers = jQuery('.offer-box');
        var value = jQuery(this).val().toLowerCase();
        jQuery(offers).each(function () {
            var text = jQuery(this).find('h2').text().toLowerCase();
            if (text.includes(value)) jQuery(this).removeClass('hidden');
            else jQuery(this).addClass('hidden');
        })
      
        
    });

})

$(document).ready(function () {
    $imgSrc = $('#imgProfile').attr('src');
    function readURL(input) {

        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('#imgProfile').attr('src', e.target.result);
            };

            reader.readAsDataURL(input.files[0]);
        }
    }
    $('#btnChangePicture').on('click', function () {
        // document.getElementById('profilePicture').click();
        if (!$('#btnChangePicture').hasClass('changing')) {
            $('#profilePicture').click();
        }
        else {
            // change
        }
    });
    $('#profilePicture').on('change', function () {
        readURL(this);
        $('#btnChangePicture').addClass('changing');
        $('#btnChangePicture').attr('value', 'Confirm');
        $('#btnDiscard').removeClass('d-none');
        // $('#imgProfile').attr('src', '');
    });
    $('#btnDiscard').on('click', function () {
        // if ($('#btnDiscard').hasClass('d-none')) {
        $('#btnChangePicture').removeClass('changing');
        $('#btnChangePicture').attr('value', 'Change');
        $('#btnDiscard').addClass('d-none');
        $('#imgProfile').attr('src', $imgSrc);
        $('#profilePicture').val('');
        // }
    });
});

$(".imgAdd").click(function () {
    $(this).closest(".row").find('.imgAdd').before('<div class="form-group"> <div class= "col-sm-4 imgUp mt-4" ><div class="imagePreview"></div> <label class="btn btn-outline-warning btn-lg btn-block btn-circle"> <i class="fa fa-plus imgAdd"></i> <input type="file" class="uploadFile img" value="Upload Photo" style="width: 0px;height: 0px;overflow: hidden;"> </label></div>');
});
$(document).on("click", "i.del", function () {
    $(this).parent().remove();
});
$(function () {
    $(document).on("change", ".uploadFile", function () {
        var uploadFile = $(this);
        var files = !!this.files ? this.files : [];
        if (!files.length || !window.FileReader) return; // no file selected, or no FileReader support

        if (/^image/.test(files[0].type)) { // only image file
            var reader = new FileReader(); // instance of the FileReader
            reader.readAsDataURL(files[0]); // read the local file

            reader.onloadend = function () { // set image data as background of div
                //alert(uploadFile.closest(".upimage").find('.imagePreview').length);
                uploadFile.closest(".imgUp").find('.imagePreview').css("background-image", "url(" + this.result + ")");

                const imageGroup = $("#imageGroupTemplate").clone();
                imageGroup.removeAttr('id');
                console.log(imageGroup);
                $("#imageGroupTemplate").before(imageGroup);
                imageGroup.show();

                $('.uploadFile').each(function (i) {
                    $(this).attr('name', `ProductImage${i+1}`);
                });
            }
        }

    });
});





