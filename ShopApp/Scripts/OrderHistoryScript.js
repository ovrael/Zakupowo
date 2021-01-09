$(document).ready(function () {

    $(".sold-bought-switcher").click(function (e) {
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

    $(".manage-transaction").click(function (e) {

        var isAccepted;
        var collapse = true;
        var transactionID = this.id;
        var buttonsToRemove = this.parentNode;

        if ($(e.target).hasClass("btn-accept")) {
            isAccepted = true;
            collapse = false;
        } else if ($(e.target).hasClass("btn-discard")) {
            isAccepted = false;
            collapse = false;
        }

        if (!collapse) {
            $.ajax({
                url: "/UserPanel/ManageTransaction",
                method: 'POST',
                dataType: 'json',
                data: '{"transactionID":"' + transactionID + '","isAccepted":"' + isAccepted + '"}',
                contentType: 'application/json; charset=utf-8',
                success: function (returnData) {
                    if (returnData != "ERROR") {
                        buttonsToRemove.remove();

                        if (returnData.length > 0) {
                            alert(returnData);
                        }

                        var transactionStatusColor = isAccepted ? "green" : "red";
                        var transactionStatus = isAccepted ? "Potwierdzona" : "Odrzucona";
                        var collapseID = "soldCollapse-" + transactionID;

                        $("#transaction-row-" + transactionID).append(
                            "<div class=\"col-10 col-lg-2 col-md-2  pb-4\" style=\"color:" + transactionStatusColor + "\">Status: " + transactionStatus + "</div>"
                            + "<div class=\"col-2 col-lg-2 pb-3\">"
                            + "<a class=\"btn btn-warning\" data-toggle=\"collapse\" href=\"#" + collapseID + "\" role=\"button\" aria-expanded=\"false\" aria-controls=\"collapseExample\">"
                            + "<i class=\"fas fa-chevron-down\"></i>"
                            + "</a>"
                            + "</div>"
                        );
                    }
                },
                // error handling
            });
        }


    })

})