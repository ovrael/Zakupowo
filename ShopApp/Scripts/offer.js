$(document).ready(function () {
    // Add-to-favourites button handler
    $(".product-fav").on("click", function () {

        var id = jQuery(this).attr('id');
        
        if ($(".product-fav").hasClass("fav-active")) {
          

            $.ajax({
                url: '/Home/UnFav',
                type: 'POST',
                data: {
                    id: id
                },
                success: function (data) {
                    if (data.length == 0)// No errors
                    {
                        $(".product-fav").addClass("fav-unActive");
                        $(".product-fav").removeClass("fav-active");
                    }
                  
                },
            });
           
        }
        if ($(".product-fav").hasClass("fav-unActive")) {
            $.ajax({
                url: '/Home/Fav',
                type: 'POST',
                data: {
                    id: id
                },
                success: function (data) {
                    if (data.length == 0)
                    {
                        $(".product-fav").addClass("fav-active");
                        $(".product-fav").removeClass("fav-unActive");

                    }// No errors
            
                },

            });
           
        }
        if ($(".product-fav").hasClass("not-logged")) {
            alert("You have to be logged in to add an offer to favourites!");
        }
    })
});


