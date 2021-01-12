var contentSum = 0.0;
var beforeDiscountSumDiv = document.getElementById("beforeDiscount");
var afterDiscountSumDiv = document.getElementById("afterDiscount");

function changeOfferPrice(offerCheckbox) {

    if (document)
        var multiplicationFactor = -1;
    var offerPriceID = "OfferPrice_" + offerCheckbox.id.split("_")[1];
    var offerPrice = document.getElementById(offerPriceID).innerHTML.trim().split(" ")[0];

    if (offerCheckbox.checked)
        multiplicationFactor = 1;

    calculateSumBeforeDiscount(multiplicationFactor, offerPrice);
}

function changeDiscount(discountCheckBox) {

    if (discountCheckBox.id == "Percent") {
        var discount = $("#percentDiscount").val();

        if (discount >= 0 && discount <= 100) {
            sumDivFloat = contentSum - contentSum * parseInt(discount, 10) / 100;
            afterDiscountSumDiv.innerHTML = sumDivFloat.toFixed(2) + " zł";
        }

        if (!discount || 0 === discount.length) {
            afterDiscountSumDiv.innerHTML = contentSum.toFixed(2) + " zł";
        }
    }

    if (discountCheckBox.id == "Currency") {
        var discount = $("#currencyDiscount").val();

        if (discount >= 0 && discount < contentSum) {
            sumDivFloat = contentSum - discount;
            afterDiscountSumDiv.innerHTML = sumDivFloat.toFixed(2) + " zł";
        }

        if (!discount || 0 === discount.length) {
            afterDiscountSumDiv.innerHTML = contentSum.toFixed(2) + " zł";
        }
    }

    if (discountCheckBox.id == "Lack") {

        sumDivFloat = contentSum;
        afterDiscountSumDiv.innerHTML = sumDivFloat.toFixed(2) + " zł";
    }
}


function calculateSumBeforeDiscount(addOrRemove, price) {
    var sumDivFloat = parseFloat(beforeDiscountSumDiv.innerHTML.trim().split(" ")[0]);

    // If I get 1 then I add price else if I get -1 then I subtract price
    if (addOrRemove == 1 || addOrRemove == -1) {
        sumDivFloat += price * addOrRemove;
        contentSum = sumDivFloat;
        beforeDiscountSumDiv.innerHTML = sumDivFloat.toFixed(2) + " zł";

        var radioButtons = document.getElementsByClassName("discount-radio");
        var discountBtn;

        for (let i = 0; i < radioButtons.length; i++) {
            const element = radioButtons[i];
            if (element.checked == true)
                discountBtn = element;
        }

        changeDiscount(discountBtn);
    }
}

(function () {
    $(':checkbox:checked').prop('checked', false);

    $("#percentDiscount").keyup(function (e) {

        if (document.getElementById("Percent").checked == true) {
            var discount = $("#percentDiscount").val();

            if (discount >= 0 && discount <= 100) {
                sumDivFloat = contentSum - contentSum * parseInt(discount, 10) / 100;
                afterDiscountSumDiv.innerHTML = sumDivFloat.toFixed(2) + " zł";
            }

            if (!discount || 0 === discount.length) {
                afterDiscountSumDiv.innerHTML = contentSum.toFixed(2) + " zł";
            }
        }
    })

    $("#currencyDiscount").keyup(function (e) {

        if (document.getElementById("Currency").checked == true) {
            var discount = $("#currencyDiscount").val();

            if (discount >= 0 && discount < contentSum) {
                sumDivFloat = contentSum - discount;
                afterDiscountSumDiv.innerHTML = sumDivFloat.toFixed(2) + " zł";
            }

            if (!discount || 0 === discount.length) {
                afterDiscountSumDiv.innerHTML = contentSum.toFixed(2) + " zł";
            }
        }
    })

    function readURL(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                document.getElementById("UploadImageButton").style.backgroundImage = "url(" + e.target.result + ")";
                //$('#previewHolder').attr('src', e.target.result);
            }
            reader.readAsDataURL(input.files[0]);
        } else {
            document.getElementById("UploadImageButton").style.backgroundImage = "url(../../Images/DodajZdjecie.png)";
        }
    }

    $("#UploadImageInput").change(function () {
        readURL(this);
        //console.log("dodaje zdjęcie");
    });

})()
