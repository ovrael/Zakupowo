$(document).ready(function () {
    // Add-to-favourites button handler
    $('body').on("click", '.product-fav', function () {

        var id = jQuery(this).attr('data-id');
        var type = jQuery(this).attr('data-type');

        if ($(this).hasClass("fav-active")) {
            var element = this;
            $.ajax({
                url: '/Offer/UnFav',
                type: 'POST',
                data: {
                    type: type,
                    id: id
                },
                success: function (ErrorMessage) {
                    if (ErrorMessage == "") // No errors
                    {
                        $(element).addClass("fav-unActive");
                        $(element).removeClass("fav-active");
                    }
                    else {
                        $.alert({
                            title: 'Wystąpił błąd',
                            content: ErrorMessage,
                            buttons: {
                                ok: {
                                    text: 'ok',
                                    btnClass: 'btn-popout'
                                }
                            }
                        });
                    }
                },
            });

        }
        if ($(this).hasClass("fav-unActive")) {
            var element = this;
            $.ajax({
                url: '/Offer/Fav',
                type: 'POST',
                data: {
                    type: type,
                    id: id
                },
                success: function (ErrorMessage) {
                    if (ErrorMessage == "") // No errors
                    {
                        $(element).addClass("fav-active");
                        $(element).removeClass("fav-unActive");
                    }
                    else {
                        $.alert({
                            title: 'Wystąpił błąd',
                            content: ErrorMessage,
                            buttons: {
                                ok: {
                                    text: 'ok',
                                    btnClass: 'btn-popout'
                                }
                            }
                        });
                    }

                },

            });

        }
        if (jQuery(this).hasClass("not-logged")) {
            $.alert({
                title: 'Zaloguj się',
                content: 'Musisz być zalogowany, by móc dodać ofertę do ulubionych',
                buttons: {
                    ok: {
                        text: 'ok',
                        btnClass: 'btn-popout'
                    }
                }
            });
        }
    }),


        $('body').on("click", '.addToBucket', function () {
            var id = jQuery(this).attr('data-id');
            var type = jQuery(this).attr('data-type');
            var quantity = jQuery(this).attr('data-quantity');
            var element = this;
            if (jQuery(this).hasClass("not-logged")) {
                $.alert({
                    title: 'Zaloguj się',
                    content: 'Musisz być zalogowany, by móc dodać ofertę do koszyka',
                    buttons: {
                        ok: {
                            text: 'ok',
                            btnClass: 'btn-popout'
                        }
                    }
                });
            }
            else if (!jQuery(this).hasClass('in-bucket')) {
                $.ajax({
                    url: '/Offer/AddToBucket',
                    type: 'POST',
                    data: {
                        type: type,
                        id: id,
                        quantity: quantity
                    },
                    success: function (ErrorMessage) {
                        if (ErrorMessage == "") {
                            $(element).addClass("in-bucket");
                            $(element).text("OFERTA ZNAJDUJE SIĘ W KOSZYKU");
                        } else {
                            $.alert({
                                title: 'Wystąpił błąd',
                                content: ErrorMessage,
                                buttons: {
                                    ok: {
                                        text: 'ok',
                                        btnClass: 'btn-popout'
                                    }
                                }
                            });
                        }
                    },
                });
            }

        }
        ),

        $(".item-delete").click(function () {
            var id = jQuery(this).attr('data-id');
            var type = jQuery(this).attr('data-type');
            var element = this;

            $.alert({
                title: 'Pomyślnie usunięto',
                content: 'Usunąłeś "' + $(this).closest(".item").find(".item-name").find("a").text() + '" ze swojego koszyka',
                buttons: {
                    ok: {
                        text: 'ok',
                        btnClass: 'btn-popout'
                    }
                }
            });

            $.ajax({
                url: '/Offer/RemoveFromBucket',
                type: 'POST',
                data: {
                    type: type,
                    id: id
                },
            });
        })

});