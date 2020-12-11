$(document).ready(function () {
    // Add-to-favourites button handler
    $(".product-fav").on("click", function () {

        var id = jQuery(this).attr('id');
        var type = jQuery(this).attr('data-type');
        console.log(id);
        if (jQuery(this).hasClass("fav-active")) {
            console.log(id);

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
    })
});


