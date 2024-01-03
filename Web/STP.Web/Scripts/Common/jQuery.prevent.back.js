$(function () {
    //PreventBackOnDialog();
    //EnableBackOnDialog();
    noBack();
});

function DisableBackButton() {
    //changed implementation
}

function EnableBackButton() {
    //changed implementation
}

function PreventBackOnDialog() {
    $(document).on("keydown", function (e) {
        if (e.which === 8 && !$(e.target).is("input, textarea") || e.which === 116) {
            if (($('#dialogue').is(':visible')) || ($('#pop-warning').is(':visible')) || ($('#page1').is(':visible'))) {
                window.onload = DisableBackButton;
                window.onpageshow = function (evt) { if (evt.persisted) DisableBackButton() }
            }
        }
    });
}

function EnableBackOnDialog() {
    $(document).on("keydown", function (e) {
        if (!($('#dialogue').is(':visible')) && !($('#pop-warning').is(':visible'))) {
            if (e.which === 8 && !$(e.target).is("input, textarea")) {
                window.history.back();
            }
            else if (e.which === 116) {
                 //location.reload();
            }
        }
    });
}
function DisableBackButton() {
    window.history.forward()
}
function noBack() {
    window.history.forward(1);
}

function suppressBackspace(evt) {
    evt = evt || window.event;
    var target = evt.target || evt.srcElement;

    if (evt.keyCode == 8 && !/input|textarea|/i.test(target.nodeName)) {
        return false;
    }
}