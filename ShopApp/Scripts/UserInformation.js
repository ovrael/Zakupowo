$(document).ready(function () {
    $(".btn-group").click(function (e) {
        if ($(e.target).hasClass("switch-offer")) {
            $(".switch-bundle").removeClass("on");
            $(".switch-offer").addClass("on");
            $(".item-row").removeClass("d-none");
            $(".bundle-row").addClass("d-none");
        } else if ($(e.target).hasClass("switch-bundle")) {
            $(".switch-offer").removeClass("on");
            $(".switch-bundle").addClass("on");
            $(".item-row").addClass("d-none");
            $(".bundle-row").removeClass("d-none");
        }
    })
});