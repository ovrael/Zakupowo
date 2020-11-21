$( document ).ready(function() {
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
        var url = jQuery(this).attr('url');
        console.log(id);
        if ($(".product-fav").hasClass("fav-active")) {
          

            $.ajax({
                url: url,
                type: 'POST',
                data: {
                    id: id
                },
                success: function (data) {
                    if (data.length == 0) // No errors
                        $(".product-fav").removeClass("fav-active");
                  
                },
            });
           
        } else {
            $.ajax({
                url: url,
                type: 'POST',
                data: {
                    id: id
                },
                success: function (data) {
                    if (data.length == 0) // No errors
                        $(".product-fav").addClass("fav-active");
            
                },

            });
           
        }
    })
});


