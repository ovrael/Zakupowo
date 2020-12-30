function renderPartialView(viewName) {

    var partialView = document.getElementById("partialView");
    // $(partialView).each(function () {
    //     jQuery(this).remove();
    // })

    partialView.innerHTML = "@{ Html.RenderAction(\"" + viewName + "\", \"UserPanel\"); }";
};