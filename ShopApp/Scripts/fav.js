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
    $(".btn-group").click(function (e) {
        console.log(this);
        if ($(e.target).hasClass("switch-offer")) {
            $(".switch-bundle").removeClass("on");
            $(".switch-offer").addClass("on");
            $(".item-row").removeClass("d-none");
            $(".bundle-row").addClass("d-none");
        } else {
            $(".switch-offer").removeClass("on");
            $(".switch-bundle").addClass("on");
            $(".item-row").addClass("d-none");
            $(".bundle-row").removeClass("d-block");
        }
    })
});