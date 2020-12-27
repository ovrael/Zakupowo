
(function () {

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

    $("#createBundleOfferName").keyup(function (e) {
        var offers = jQuery('.offer-row');
        var value = jQuery(this).val().toLowerCase();

        $(offers).each(function () {
            var text = jQuery(this).find(".offer-title").text().toLowerCase();

            if (text.includes(value))
                jQuery(this).removeClass('hidden');
            else
                jQuery(this).addClass('hidden');
        })
    })

})()
