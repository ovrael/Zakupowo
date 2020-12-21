$(document).ready(function () {
    var id = $("#address-input").val();
    $(".address").each(function () {
        if ($(this).attr("id") == "Address_" + id) {
            $(this).addClass("d-block");
            $(this).removeClass("d-none");
        } else {
            $(this).addClass("d-none");
            $(this).removeClass("d-block");
        }
    })
    $("#address-input").change(function () {
        var id = $("#address-input").val();
        $(".address").each(function () {
            if ($(this).attr("id") == "Address_" + id) {
                $(this).addClass("d-block");
                $(this).removeClass("d-none");
            } else {
                $(this).addClass("d-none");
                $(this).removeClass("d-block");
            }
        })
    })
});