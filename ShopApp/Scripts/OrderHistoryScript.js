$(document).ready(function () {

    $(".btn-group").click(function (e) {
        if ($(e.target).hasClass("switch-bought")) {
            $(".switch-sold").removeClass("on");
            $(".switch-bought").addClass("on");
            $("#boughtDiv").removeClass("d-none");
            $("#soldDiv").addClass("d-none");
        } else if ($(e.target).hasClass("switch-sold")) {
            $(".switch-bought").removeClass("on");
            $(".switch-sold").addClass("on");
            $("#boughtDiv").addClass("d-none");
            $("#soldDiv").removeClass("d-none");
        }
    })

})