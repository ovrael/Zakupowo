$(document).ready(function () {
    // Add-to-favourites button handler
    $(".product-fav").on("click", function () {

        var id = jQuery(this).attr('id');
        var type = jQuery(this).attr('data-type');

        if ($(this).hasClass("fav-active")) {
            var element = this;
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
                        jQuery(element).addClass("fav-unActive");
                        jQuery(element).removeClass("fav-active");
                    }
                  
                },
            });
           
        }
        if ($(this).hasClass("fav-unActive")) {
            var element = this;
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
                        $(element).addClass("fav-active");
                        $(element).removeClass("fav-unActive");

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
        var element = this;
        if (jQuery(this).hasClass("not-logged")) {
            alert("You have to be logged in to add anything to bucket!");
        }
        if (!jQuery(this).hasClass('in-bucket'))
        $.ajax({
                url: '/Offer/AddToBucket',
                type: 'POST',
                data: {
                    type: type,
                    id: id,
                    quantity: quantity
                },
                success: function (data) {
                    if (data.length == 0)// No errors
                    {
                        $(element).addClass("in-bucket");
                        $(element).text("OFERTA ZNAJDUJE SIĘ W KOSZYKU");
                    }

                },
            });
        }
    )
});


