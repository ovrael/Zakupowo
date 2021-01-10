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

    //SEARCH FEATURE
    $("#searchButton").on("click", function (e) {


        var query = $("#searchText").val();
        var trimmedQuery = query.trim();

        if (trimmedQuery == "") {
            e.preventDefault();
            console.log("Same spacje/taby?");
        }
        else {
            console.log("szukam:" + trimmedQuery + ";");

            // $.ajax({
            //     url: "/Home/Search",
            //     method: 'POST',
            //     dataType: 'json',
            //     data: '{"query":"' + trimmedQuery + '"}',
            //     contentType: 'application/json; charset=utf-8',
            //     success: function (returnData) {
            //     },
            //     // error handling
            // });
        }

    });

});
