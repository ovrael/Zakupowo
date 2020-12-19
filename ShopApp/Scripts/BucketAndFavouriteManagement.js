$(document).ready(function () {
    // Add-to-favourites button handler
    $(".product-fav").on("click", function () {

        var id = jQuery(this).attr('id');
        var type = jQuery(this).attr('data-type');

        if (jQuery(this).hasClass("fav-active")) {

            $.ajax({
                url: '/Home/UnFav',
                type: 'POST',
                data: {
                    type: type,
                    id: id
                },
                success: function (data) {
                    if (data.length == 0)// No errors
                    {
                        $("#" + id).addClass("fav-unActive");
                        $("#" + id).removeClass("fav-active");
                    }
                  
                },
            });
           
        }
        if (jQuery(this).hasClass("fav-unActive")) {
            $.ajax({
                url: '/Home/Fav',
                type: 'POST',
                data: {
                    type: type,
                    id: id
                },
                success: function (data) {
                    if (data.length == 0)
                    {
                        $("#" + id).addClass("fav-active");
                        $("#" + id).removeClass("fav-unActive");

                    }// No errors
            
                },

            });
           
        }
        if (jQuery(this).hasClass("not-logged")) {
            alert("You have to be logged in to add an offer to favourites!");
        }
    }),



    $(".addToBucket").on("click", function () {

        var id = jQuery(this).attr('id');
        var type = jQuery(this).attr('data-type');
        var quantity = jQuery(this).attr('data-quantity');

        if (jQuery(this).hasClass("not-logged")) {
            alert("You have to be logged in to add anything to bucket!");
        }
        if (!jQuery(this).hasClass('in-bucket'))
        $.ajax({
                url: '/Offer/AddToBucket',
                type: 'POST',
                data: {
                    type: type,
                    quantity: quantity,
                    id: id
                },
                success: function (data) {
                    if (data.length == 0)// No errors
                    {
                        $("#" + id).addClass("in-bucket");
                        $("#" + id).removeClass("addToBucket");
                    }

                },
            });
        }
    )
});


