function calculate() {
    var sum = 0;
    $(".items-group").each(function (e) {
        if ($(this).hasClass("selected")) {
            $(this).find(".item").each(function (e) {
                var quantity = $(this).find(".item-quantity").find(":input").val();
                var pricetext = $(this).find(".item-price").find(".total-price").text();
                var price = pricetext.substr(0, pricetext.indexOf(' '));
                price = price.replace(',', '.');
                if (quantity != null)
                    sum += parseFloat(quantity) * parseFloat(price);
                else
                    sum += parseFloat(price);
            })
        }
    });
    return sum.toFixed(2);
}
$(document).ready(function () {
    var sum = calculate();
    $(".bucket-price-amount").text(sum + " zł");
    $('input[type ="checkbox"]').change(function (e) {
        if (this.checked) {
            $(this).closest(".items-group").addClass("selected");
        } else {
            $(this).closest(".items-group").removeClass("selected");
        }
    });
    $(":input").change(function (e) {
        sum = calculate();
        $(".bucket-price-amount").text(sum + " zł");
    })
    $(".item-delete").click(function (e) {
        var group = $(this).closest(".item").closest(".items-group");
        $(this).closest(".item").remove();
        if (group.find(".item").length === 0) {
            group.remove();
        }
        $(".bucket-price-amount").text(calculate() + " zł");
    });
})