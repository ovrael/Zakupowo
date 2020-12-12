$(document).ready(function () {
    $(".filter-button").click(function () {
        if ($(".overlay").hasClass("d-none")) {
            $(".overlay").removeClass("d-none");
        }
    })
    $(".fa-times").click(function () {
        if (!$(".overlay").hasClass("d-none")) {
            $(".overlay").addClass("d-none");
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
})