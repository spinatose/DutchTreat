
$(document).ready(function () {
    console.log("ready!");
    //var msgForm = $("#msgForm");
    $("#btnBuy").on("click", () => console.log("clicked the button"));

    //msgForm.hide();

    var prodinfo = $(".product-props li");
    prodinfo.on("click", function() {
        console.log("You clickec " + $(this).text());
    });

    var logintog = $("#loginToggle");
    var popupfrm = $(".popup-form");

    logintog.on("click", () => popupfrm.toggle(1000)); // 1000 ms to toggle animation
});
