$(document).ready(function () {
    // Quantity input
    function incrementValue(e) {
        e.preventDefault();
        var fieldName = $(e.target).data('field');
        var parent = $(e.target).closest('div');
        var currentVal = parseInt(parent.find('input[name=' + fieldName + ']').val(), 10);

        if (!isNaN(currentVal)) {
            parent.find('input[name=' + fieldName + ']').val(currentVal + 1);
        } else {
            parent.find('input[name=' + fieldName + ']').val(0);
        }
    }

    function decrementValue(e) {
        e.preventDefault();
        var fieldName = $(e.target).data('field');
        var parent = $(e.target).closest('div');
        var currentVal = parseInt(parent.find('input[name=' + fieldName + ']').val(), 10);

        if (!isNaN(currentVal) && currentVal > 0) {
            parent.find('input[name=' + fieldName + ']').val(currentVal - 1);
        } else {
            parent.find('input[name=' + fieldName + ']').val(0);
        }
    }
    
    $('.input-group').on('click', '.button-plus', function(e) {
        incrementValue(e);
    });

    $('.input-group').on('click', '.button-minus', function(e) {
        decrementValue(e);
    });

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


