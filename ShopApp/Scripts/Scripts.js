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
    

    jQuery('table tr').on('click', function () {
        jQuery(this).remove();
    })

})