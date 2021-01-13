(function () {

    // FILTERS

    //Offers in UserPanel
    $("#offerNameFilter").keyup(function (e) {
        var offers = jQuery('.offer-row');
        var value = jQuery(this).val().toLowerCase();

        $(offers).each(function () {
            var text = jQuery(this).find('td').eq(0).text().toLowerCase();

            if (text.includes(value))
                jQuery(this).removeClass('hidden');
            else
                jQuery(this).addClass('hidden');
        })
    })

    //Bundles in UserPanel
    $("#bundleNameFilter").keyup(function (e) {
        var offers = jQuery('.bundle-row');
        var value = jQuery(this).val().toLowerCase();

        $(offers).each(function () {
            var text = jQuery(this).find('td').eq(0).text().toLowerCase();

            if (text.includes(value))
                jQuery(this).removeClass('hidden');
            else
                jQuery(this).addClass('hidden');
        })
    })

    //Offers in AddBundle
    $("#createBundleOfferName").keyup(function (e) {
        var offers = jQuery('.offer-row');
        var value = jQuery(this).val().toLowerCase();

        $(offers).each(function () {
            var text = jQuery(this).find('td').eq(2).text().toLowerCase();

            if (text.includes(value))
                jQuery(this).removeClass('hidden');
            else
                jQuery(this).addClass('hidden');
        })
    })

    $('body').on("click", '.btn-deactivate', function () {
        var elementID = this.id;

        console.log("elementID: " + elementID);

        var userPanelMethod = "DeactivateOffer";
        var methodArgument = "offerID";
        var deactivate = "deactivateOffer-";
        var status = "offerStatus-";
        var statusInnerHtml = "<b>Nieaktywna</b>";
        var elementRow = "#offerRow-";
        var elementA = "<a class=\"btn btn-outline-info\" href=\"@Url.Action(\"Index\", \"Offer\", new { OfferID = " + elementID + " })\"> Zobacz ofertę </a>";
        var alertSuccess = "Pomyślnie dezaktywowano ofertę!";
        var alertFail = "Nie udało się zdezaktywować oferty!";

        var whatToDeactivate = this.title.split(" ")[1];

        if (whatToDeactivate == "Bundle") {
            userPanelMethod = "DeactivateBundle";
            methodArgument = "bundleID";
            deactivate = "deactivateBundle-";
            status = "bundleStatus-";
            statusInnerHtml = "<b>Nieaktywny</b>";
            elementRow = "#bundleRow-";
            elementA = "<a class=\"btn btn-outline-info\" href=\"@Url.Action(\"Bundle\", \"Offer\", new { BundleID = " + elementID + " })\"> Zobacz zestaw </a>";
            alertSuccess = "Pomyślnie dezaktywowano zestaw!";
            alertFail = "Nie udało się zdezaktywować zestawu!";
        }

        $.ajax({
            url: "/UserPanel/" + userPanelMethod,
            method: 'POST',
            dataType: 'json',
            data: '{"' + methodArgument + '":"' + elementID + '"}',
            contentType: 'application/json; charset=utf-8',
            success: function (returnData) {
                $("#warningBeforeDeactivate_" + elementID).modal('hide');
                if (returnData == true) {

                    document.getElementById("warningBeforeDeactivate_" + elementID).remove();
                    //var editID = "editOffer-" + offerID;
                    var deactivateID = deactivate + elementID;
                    var elementStatusId = status + elementID;

                    //document.getElementById(editID).remove();
                    document.getElementById(deactivateID).remove();

                    document.getElementById(elementStatusId).classList.remove("text-succses");
                    document.getElementById(elementStatusId).classList.add("text-danger");
                    document.getElementById(elementStatusId).innerHTML = statusInnerHtml;

                    var elementRowID = elementRow + elementID;

                    $(elementRowID).append(
                        "<td class=\"td-text\">"
                        + elementA
                        + "</td>"
                    );

                    alert(alertSuccess);
                }
                else {
                    alert(alertFail);
                }
            },
            // error handling
        });
    })
})()

//SORTING
function sortOffersBy(sortByID) {

    if (document.getElementById(sortByID) == null)
        return false;

    $.ajax({
        url: '/UserPanel/SortOffers',
        type: 'POST',
        dataType: 'json',
        data: '{"sortBy":"' + sortByID + '"}',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var sortByArray = sortByID.split("-");
            var sortSign = "↑";

            if (sortByArray[1] == "Asc") {
                sortSign = "↓";
                sortByArray[1] = "Dsc";
            }
            else
                sortByArray[1] = "Asc";

            var titleDiv = document.getElementById(sortByID);

            if (titleDiv != null) {

                var oldSorted = document.getElementsByClassName("now-sorted");

                if (oldSorted.length > 0) {
                    for (let i = 0; i < oldSorted.length; i++) {
                        const oldSortedDiv = oldSorted[i];
                        var oldSortedText = oldSortedDiv.innerHTML;
                        oldSortedDiv.innerHTML = oldSortedText.split(" ")[0];
                    }
                }
                titleDiv.classList.add("now-sorted");

                titleDiv.id = sortByArray[0] + "-" + sortByArray[1];

                var titleTextArray = titleDiv.innerHTML;
                var texts = titleTextArray.split(" ");
                titleDiv.innerHTML = texts[0] + " " + sortSign;
            }

            var offers = jQuery('.offer-row');
            $(offers).each(function () {
                jQuery(this).remove();
            })

            for (let i = 0; i < data.length; i++) {
                const offer = data[i];
                var offerTR = createOfferDiv(offer);
                $("#offersTableBody").eq(0).append(offerTR);
            }
        },
        error: function () {
            alert("Nie udało się :/");
        }
    });
}

function createOfferDiv(offer) {

    var filterValue = $("#offerNameFilter").val().toLowerCase();
    var offerTitle = offer.Title.toLowerCase();

    var isHiddenClass = "hidden";
    if (offerTitle.includes(filterValue))
        isHiddenClass = "";

    var titleTD = "<td class=\"td-title\">"
        + "<a class=\"text-warning\" href=\"@Url.Action(\"Index\",\"Offer\",new { OfferID =" + offer.OfferID + "})\"><b><i>" + offer.Title + "</i></b></a>"
        + "</td>";

    var categoryTD = "<td class=\"td-text\">"
        + offer.Category
        + "</td >";

    var statusColorClass = offer.Status ? "text-success" : "text-danger";
    var status = offer.Status ? "Aktywna" : "Nieaktywna";

    var statusTD = "<td class=\"" + statusColorClass + " td-text\">"
        + "<b>" + status + "</b>"
        + "</td >";

    var dateTD = "<td class=\"td-text\">"
        + offer.CreationDate
        + "</td >";

    var priceTD = "<td class=\"td-number\">"
        + offer.Price.toFixed(2)
        + "</td >";

    var soldTD = "<td class=\"td-text\">"
        + offer.Sold
        + "</td >";

    var leftTD = "<td class=\"td-text\">"
        + offer.Left
        + "</td >";

    var settingsTD = "";

    if (offer.Status == true) {

        var offerWarning = "#warningBeforeDeactivate_" + offer.OfferID;
        settingsTD = "<td class=\"td-text\">"
            + "<button class=\"btn btn-outline-warning\"> Edytuj </button>"
            + "</td >"
            + "<td class=\"td-text\">"
            + " <button data-toggle=\"modal\" data-target=\"" + offerWarning + "\" class=\"btn btn-outline-danger\"> Dezaktywuj </button>"
            + "</td >";
    }
    else {
        settingsTD = "<td colspan=\"2\" class=\"td-text\">"
            + "<a class=\"btn btn-outline-info\" href=\"@Url.Action(\"Index\", \"Offer\", new { OfferID = " + offer.OfferID + "})\"> Zobacz ofertę </a>"
            + "</td >";
    }

    var offerTR =
        "<tr class=\"offer-row " + isHiddenClass + "\">"
        + titleTD
        + categoryTD
        + statusTD
        + dateTD
        + priceTD
        + leftTD
        + soldTD
        + settingsTD
        + "</tr>";

    return offerTR;
}

function sortsBundlesBy(sortByID) {

    if (document.getElementById(sortByID) == null)
        return false;

    $.ajax({
        url: '/UserPanel/SortBundles',
        type: 'POST',
        dataType: 'json',
        data: '{"sortBy":"' + sortByID + '"}',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var sortByArray = sortByID.split("-");
            var sortSign = "↑";

            if (sortByArray[1] == "Asc") {
                sortSign = "↓";
                sortByArray[1] = "Dsc";
            }
            else
                sortByArray[1] = "Asc";

            var titleDiv = document.getElementById(sortByID);

            if (titleDiv != null) {

                var oldSorted = document.getElementsByClassName("now-sorted");

                if (oldSorted.length > 0) {
                    for (let i = 0; i < oldSorted.length; i++) {
                        const oldSortedDiv = oldSorted[i];
                        var oldSortedText = oldSortedDiv.innerHTML;
                        oldSortedDiv.innerHTML = oldSortedText.split(" ")[0];
                    }
                }
                titleDiv.classList.add("now-sorted");

                titleDiv.id = sortByArray[0] + "-" + sortByArray[1];

                var titleTextArray = titleDiv.innerHTML;
                var texts = titleTextArray.split(" ");
                titleDiv.innerHTML = texts[0] + " " + sortSign;
            }

            var bundles = jQuery('.bundle-row');
            $(bundles).each(function () {
                jQuery(this).remove();
            })

            for (let i = 0; i < data.length; i++) {
                const bundle = data[i];
                var bundleTR = createBundleDiv(bundle);
                $("#bundlesTableBody").eq(0).append(bundleTR);
            }
        },
        error: function () {
            alert("Nie udało się :/");
        }
    });
}

function createBundleDiv(bundle) {

    var filterValue = $("#bundleNameFilter").val().toLowerCase();
    var bundleTitle = bundle.Title.toLowerCase();

    var isHiddenClass = "hidden";
    if (bundleTitle.includes(filterValue))
        isHiddenClass = "";

    var titleTD = "<td class=\"td-title\">"
        + "<a class=\"text-warning\" href=\"@Url.Action(\"Index\",\"Bundle\",new { BundleID =" + bundle.BundleID + "})\"><b><i>" + bundle.Title + "</i></b></a>"
        + "</td>";

    var priceTD = "<td class=\"td-number\">"
        + bundle.BundlePrice.toFixed(2)
        + "</td >";

    var statusColorClass = bundle.Status ? "text-success" : "text-danger";
    var status = bundle.Status ? "Aktywny" : "Nieaktywny";

    var statusTD = "<td class=\"td-text\">"
        + "<span class=\"d-inline-block\" tabindex=\"0\" data-toggle=\"tooltip\" title=\"Zdezaktywowana oferta jest możliwa tylko do przeglądania, nie można jej ani modyfikować ani kupić.\">"
        + "<a class=\"" + statusColorClass + "\" style=\"pointer-events: none;\"><b>" + status + "</b></a>"
        + "</span>"
        + "</td >";

    var dateTD = "<td class=\"td-text\">"
        + bundle.CreationDate
        + "</td >";

    var settingsTD = "";

    if (bundle.Status == true) {

        var bundleWarning = "#warningBeforeDeactivate_" + bundle.BundleID;
        settingsTD = "<td class=\"td-text\">"
            + "<button class=\"btn btn-outline-warning\"> Edytuj </button>"
            + "</td >"
            + "<td class=\"td-text\">"
            + " <button data-toggle=\"modal\" data-target=\"" + bundleWarning + "\" class=\"btn btn-outline-danger\"> Dezaktywuj </button>"
            + "</td >";
    }
    else {
        settingsTD = "<td colspan=\"2\" class=\"td-text\">"
            + "<a class=\"btn btn-outline-dark\" href=\"@Url.Action(\"Index\", \"Bundle\", new { BundleID = " + bundle.BundleID + "})\"> Zobacz zestaw </a>"
            + "</td >";
    }

    var bundleTR =
        "<tr class=\"bundle-row " + isHiddenClass + "\">"
        + titleTD
        + priceTD
        + statusTD
        + dateTD
        + settingsTD
        + "</tr>";

    return bundleTR;
}