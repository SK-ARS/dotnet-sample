/**********************************************
 Developer   : Anlet
 Added on    : 16 Feb 2017
 Modified on : -
 Purpose     : For RM #6037
**********************************************/
var movClass;
var movementTypeId;
$(document).ready(function () {
    
    $('body').on('click', '.btn_CopyAll', function (e) {
        e.preventDefault();
        AxleTableMethods.AxleCopyToAll(this);
    });
    $('body').on('click', '.btn_CopyFromPrev', function (e) {
        e.preventDefault();
        AxleCopyPrevious(this);
    });
    $('body').on('blur', '.blur-valid-wheel,.blur-valid-weight,.blur-valid-tyre-size', function (e) {
        e.preventDefault();
        validationaxle(this);
    });
});
/**********************************************/
function AxleComponentInit() {
    var movClass = $('#MovementClassification').val();
    var movementTypeId = $('#movementTypeId').val();
    if (movClass == 241002 || movementTypeId == 270006) {
        $('#div_tyre_Spce_msg').show();
    }
    else {
        $('#div_tyre_Spce_msg').hide();
    }
}

function AxleCopyPrevious(e) {
    var arg1 = $(e).attr("arg1");
    AxleCopyFromPrev(arg1, e);
}

function SetHeaderHeight() {
    $('.comp').each(function () {
        var compId = $(this).attr("id");
        HeaderHeight(compId);
    });
}