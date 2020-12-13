$(document).ready(function () {
    $(".product-fav").click(function (e) {
        if ($(e.target).hasClass("fav-active")) {
            $(e.target).closest(".offer").remove();
            $.alert({
                title: 'Pomyślnie usunięto',
                content: 'Usunąłeś "' + $(e.target).closest(".offer").find("h3").find(".product-name").text() + '" z ulubionych',
                buttons: {
                    ok: {
                        text: 'ok',
                        btnClass: 'btn-popout'
                    }
                }
            });
        }
    })
});