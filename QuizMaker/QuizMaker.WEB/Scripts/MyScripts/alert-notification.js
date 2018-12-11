
function Success(message) {
    $('#success-span').text(message);
    $("#msg-success").addClass("show");
    $("#msg-failed").addClass("hide");
    setTimeout(HideAll, 5000);
}
function Failed(message) {
    if (typeof message !== "undefined") {
        $('#failed-span').text(message);

        $("#msg-success").addClass("hide");
        $("#msg-failed").addClass("show");
        setTimeout(HideAll, 5000);
    } else {
        $('#failed-span').text(" Something went wrong. Please try again later or contact administrator.");

        $("#msg-success").addClass("hide");
        $("#msg-failed").addClass("show");
        setTimeout(HideAll, 5000);
    }
   
}
function HideAll() {
    if ($("#msg-success").hasClass('show'))
        $("#msg-success").removeClass('show');
    if ($("#msg-failed").hasClass('show'))
        $("#msg-failed").removeClass('show');
    if (!$("#msg-success").hasClass("hide"))
        $("#msg-success").addClass("hide");
    if (!$("#msg-failed").hasClass("hide"))
        $("#msg-failed").addClass("hide");
}