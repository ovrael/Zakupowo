$(document).ready(function () {
    $("#quantity").on("change", function () {
        var value = jQuery(this).val();
        $('.addToBucket').data('data-quantity',value);            
                }
            )});