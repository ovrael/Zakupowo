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
    var totalsum = 0;
    $(".items-sum").each(function () {
        var sum = 0;
        var group = $(this).closest(".items-footer").closest(".items-group");
        group.find(".item").each(function () {
            var price = $(this).find(".item-desc").find(".product-totalprice").text();
            var value = price.substr(0, price.indexOf(" "));
            sum += parseInt(value);
        })
        totalsum += sum;
        $(this).text(sum + " zł");
    })

    $(".cash-sum").text(totalsum + " zł");
});