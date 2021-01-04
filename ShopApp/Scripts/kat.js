$(document).ready(function () {
    $("#dslider-range").slider({
        range: true,
        min: 0,
        max: 5000,
        values: [0, 5000],
        slide: function (event, ui) {
            $("#damount").val(ui.values[0] + " zł - " + ui.values[1] + " zł");
        }
    });
    $("#damount").val($("#dslider-range").slider("values", 0) + " zł - " + $("#dslider-range").slider("values", 1) + " zł");

    $("#mslider-range").slider({
        range: true,
        min: 0,
        max: 5000,
        values: [0, 5000],
        slide: function (event, ui) {
            $("#mamount").val(ui.values[0] + " zł - " + ui.values[1] + " zł");
        }
    });
    $("#mamount").val($("#mslider-range").slider("values", 0) + " zł - " + $("#mslider-range").slider("values", 1) + " zł");

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
})