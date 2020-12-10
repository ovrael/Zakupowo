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
})