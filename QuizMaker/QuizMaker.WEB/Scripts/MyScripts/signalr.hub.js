// --- outside of the user controls, e.g. on the container page ---
//$(function () {
    var hubStart = null;
    var hub = $.connection.groupHub;

    window.startHub = function () {
        if (hubStart === null) {
            hubStart = $.connection.hub.start();
        }
        return hubStart;
    };
//});

//// --- in your other pages ---
//$(function () {
//    window.startHub().done(function () {
//        // call hub method
//    });
//});