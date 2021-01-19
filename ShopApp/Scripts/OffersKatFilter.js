(function () {
    $('.filterSubmit').click(function (e) {
        e.preventDefault();
        console.log("chce filtrować!");
        filter();
    });
})()

function filter() {

    var sortOption = document.getElementById("sortForm").value;

    var priceID = "amount";
    var stateNew = "statenew";
    var stateUsed = "stateold";
    var stateDamaged = "statedamaged";

    // d - big screen
    // m - small screen


    if ($("#filterButton").is(':visible')) {
        priceID = "#m" + priceID;
        stateNew = "mstatenew";
        stateUsed = "mstateold";
        stateDamaged = "mstatedamaged";
    }
    else {
        priceID = "#d" + priceID;
        stateNew = "dstatenew";
        stateUsed = "dstateold";
        stateDamaged = "dstatedamaged";
    }

    var priceValues = $(priceID).val();
    priceValues = priceValues.replaceAll(" ", "");
    priceValues = priceValues.replaceAll("zł", "");

    var minPrice = priceValues.split("-")[0];
    var maxPrice = priceValues.split("-")[1];

    var states = "";
    if (document.getElementById(stateNew).checked) {
        states += "new";
    }
    if (document.getElementById(stateUsed).checked) {
        if (states.length > 0) {
            states += ",";
        }
        states += "used";
    }
    if (document.getElementById(stateDamaged).checked) {
        if (states.length > 0) {
            states += ",";
        }
        states += "damaged";
    }

    var categoryDiv = document.getElementById("categoryName");
    var category = "";
    if (categoryDiv != undefined) {
        category = categoryDiv.innerHTML.trim();
    }

    var queryString = document.getElementById("queryString");
    var query = "";
    if (queryString != undefined) {
        query = queryString.innerHTML.trim();
        query = query.substring(query.indexOf("\"") + 1, query.lastIndexOf("\""));
    }


    $.ajax({
        url: "/Home/FilterOffersAndBundles",
        method: 'POST',
        data: { sortBy: sortOption, minPrice: minPrice, maxPrice: maxPrice, states: states, category: category, query: query },
        success: function (returnData) {
            if (returnData != false) {

                var offersToRemove = jQuery('.item-row');
                $(offersToRemove).each(function () {
                    jQuery(this).remove();
                })

                var bundlesToRemove = jQuery('.bundle-row');
                $(bundlesToRemove).each(function () {
                    jQuery(this).remove();
                })

                var Offers = returnData.Offers;
                var Bundles = returnData.Bundles;

                for (let i = 0; i < Offers.length; i++) {
                    const offer = Offers[i];

                    var offerRow = createOfferDiv(offer);
                    $("#offersContainer").eq(0).append(offerRow);

                }

                for (let i = 0; i < Bundles.length; i++) {
                    const bundle = Bundles[i];

                    var bundleRow = createBundleDiv(bundle);
                    $("#bundlesContainer").eq(0).append(bundleRow);

                }

            }
        },
        // error handling
    });
}

function createOfferDiv(offer) {

    var picture = "<div class=\"col-12 col-lg-4 text-center offer-img my-auto\">"
        + "<img class=\"img-fluid\" src=\"" + offer.PictureURL + "\">"
        + "</div>"

    var productLink = "<h3><a class=\"product-name\" href=\"" + '/Offer?OfferID=' + offer.OfferID + "\">" + offer.Title + "</a></h3>";

    var seller = "<p class=\"seller d-none d-lg-block mb-3\">Sprzedawca: <a class=\"seller-name\" href=\"" + '/User/UserInformation?userID=' + offer.UserID + "\">" + offer.UserLogin + "</a></p>";

    var price = "<h3 class=\"product-price mb-3\">" + offer.Price + " zł</h3>";

    var inStock = "<p class=\"offer-count d-none d-lg-block mb-5\">Dostępne: " + offer.InStockNow + "</p>";

    var bucket = "<a class=\"addToBucket\" data-id=\"" + offer.OfferID + "\" data-type=\"Offer\" data-quantity=\"1\">DODAJ DO KOSZYKA</a>";
    if (offer.InBucket != undefined && offer.InBucket == true) {
        bucket = "<a class=\"addToBucket in-bucket\" data-id=\"" + offer.OfferID + "\" data-type=\"Offer\" data-quantity=\"1\">OFERTA ZNAJDUJE SIĘ W KOSZYKU</a>";
    }
    else if (offer.InBucket == undefined) {
        bucket = "<a class=\"addToBucket not-logged\">DODAJ DO KOSZYKA</a>";
    }

    var fav = "<i class=\"fas fa-heart fa-fw product-fav fav-unActive\" data-type=\"Offer\" data-id=\"" + offer.OfferID + "\"></i>";
    if (offer.IsFav != undefined && offer.IsFav == true) {
        fav = "<i class=\"fas fa-heart fa-fw product-fav fav-active\" data-type=\"Offer\" data-id=\"" + offer.OfferID + "\"></i>";
    } else if (offer.IsFav == undefined) {
        fav = "<i class=\"fas fa-heart fa-fw product-fav not-logged\"></i>";
    }

    var offerRow =
        "<div class=\"row offer item-row p-2 m-0 mt-1\">"
        + "<hr class=\"w-100\">"
        + picture
        + "<div class=\"col-12 col-lg-8 offer-desc p-0\">"
        + productLink
        + seller
        + price
        + inStock
        + "<br>"
        + bucket
        + fav
        + "</div>"
        + "</div>";

    return offerRow;
}

function createBundleDiv(bundle) {

    var picture = "<div class=\"col-12 col-lg-4 text-center offer-img my-auto\">"
        + "<img class=\"img-fluid\" src=\"" + bundle.PictureURL + "\">"
        + "</div>"

    var productLink = "<h3><a class=\"product-name\" href=\"" + '/Offer/Bundle?BundleID=' + bundle.BundleID + "\">" + bundle.Title + "</a></h3>";

    var seller = "<p class=\"seller d-none d-lg-block mb-3\">Sprzedawca: <a class=\"seller-name\" href=\"" + '/User/UserInformation?userID=' + bundle.UserID + "\">" + bundle.UserLogin + "</a></p>";

    var oldPrice = "<h3 class=\"product-price mb-1\" style=\"text-decoration: line-through;\">" + bundle.OffersPriceSum + " zł</h3>";
    var newPrice = "<h3 class=\"product-price mb-5\">" + offer.BundlePrice + " zł</h3>";

    var bucket = "<a class=\"addToBucket\" data-id=\"" + bundle.BundleID + "\" data-type=\"Bundle\" data-quantity=\"1\">DODAJ DO KOSZYKA</a>";
    if (bundle.InBucket != undefined && bundle.InBucket == true) {
        bucket = "<a class=\"addToBucket in-bucket\" data-id=\"" + bundle.BundleID + "\" data-type=\"Bundle\" data-quantity=\"1\">ZESTAW ZNAJDUJE SIĘ W KOSZYKU</a>";
    }
    else if (bundle.InBucket == undefined) {
        bucket = "<a class=\"addToBucket not-logged\">DODAJ DO KOSZYKA</a>";
    }

    var fav = "<i class=\"fas fa-heart fa-fw product-fav fav-unActive\" data-type=\"Bundle\" data-id=\"" + bundle.BundleID + "\"></i>";
    if (bundle.IsFav != undefined && bundle.IsFav == true) {
        fav = "<i class=\"fas fa-heart fa-fw product-fav fav-active\" data-type=\"Bundle\" data-id=\"" + bundle.BundleID + "\"></i>";
    } else if (bundle.IsFav == undefined) {
        fav = "<i class=\"fas fa-heart fa-fw product-fav not-logged\"></i>";
    }

    var offerRow =
        "<div class=\"row offer bundle-row d-none p-2 m-0 mt-1\">"
        + "<hr class=\"w-100\">"
        + picture
        + "<div class=\"col-12 col-lg-8 offer-desc p-0\">"
        + productLink
        + seller
        + oldPrice
        + newPrice
        + "<br>"
        + bucket
        + fav
        + "</div>"
        + "</div>";

    return offerRow;
}