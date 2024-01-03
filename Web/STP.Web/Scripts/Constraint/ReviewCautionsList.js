
$(document).ready(function () {
   
    
    $('#exampleModalCenter1').on('shown.bs.modal', function () {
        //
        $('.modal-backdrop').remove();
    })
    $('#exampleModalCenter2').on('shown.bs.modal', function () {
        //
        $('.modal-backdrop').remove();
    })

    //startAnimation();
    //-----------$(document).ready(function () { jQuery.Caution.js start here}------------
    //CautionsReady();
    //-----------$(document).ready(function () { jQuery.Caution.js end here}--------------
    //$('.loading').hide();
    Resize_PopUp(710);
    //stopAnimation();
    removescroll();
    stopAnimation();
    $("#overlay").show();
    $("#dialogue").show();
    //$("#overlay").show();

    $('body').on('click', '.review-caution-list .btn-create-caution', function () {
        CreateCaution();
    });

    $('body').on('click', '.review-caution-list .btn-close-caution', function () {
        closeCautionSpanReviewCaut();
    });

    $('body').on('click', '.review-caution-list .btn-caution-show-report', function () {
        var CautionId = $(this).data('cautionid');
        var ConstraintId = $(this).data('constraintid');
        CautionShowReportReviewCautionList(CautionId, ConstraintId, viewCaution=1);
    });

    $('body').on('click', '.review-caution-list .btn-edit-caution', function () {
        var CautionId = $(this).data('cautionid');
        EditCaution(CautionId);
    });

    $('body').on('click', '.review-caution-list .btn-delete-caution', function () {
        DeleteCautionWarning(this);
    });

    $('body').on('click', '.review-caution-list .btn-review-contact', function () {
        ReviewContacts();
    });

    $('body').on('click', '.review-caution-list .btn-view-history', function () {
        ViewHistory();
    });

    $('body').on('click', '.review-caution-list .btn-close-delete-warning', function () {
        closeDeleteWarning();
    });

    $('body').on('click', '.review-caution-list .btn-delete-caution-warning', function () {
        DeleteCaution();
    });

    $('body').on('click', '.review-caution-list .btn-reload-review-caution-list', function () {
        ReloadReviewCautionList();
    });
    $('body').on('click', '#reviewCausionPaginator a', function () {
       
        if (this.href == '') {
            return false;
        }
        else {
            $.ajax({
                url: this.href,
                type: 'GET',
                cache: false,
                success: function (result) {
                    $('#dialogue').html(result);
                }
            });
            return false;
        }
    });

});
function closeCautionSpanReviewCaut() {
    $("#dialogue").show();
    //startAnimation();
    var randomNumber = Math.random();
    //$(".modal-backdrop").css("height", "0px");

    //var ViewConstraintsdetails = $("#ViewConstraintsdetails").val();
    var flageditmode = $('#flageditmode').val();
    $("#dialogue").load('../Constraint/ViewConstraint?ConstraintID=' + $('#ConstraintID').val() + '&flageditmode=' + flageditmode + '&random=' + randomNumber, function () {
        ViewConstraintInit();
    });
}


function CreateCaution() {
    var randomNumber = Math.random();
    $("#dialogue").show();
    OpenDialogueReviewCautions('../Constraint/CreateCaution?ConstraintID=' + $('#ConstraintID').val() + '&CautionId=0&mode=add&random=' + randomNumber, 'create-caution');
}
function ViewCaution(Cautionid) {
    var flageditmode = $('#flageditmode').val();
    var randomNumber = Math.random();
    $("#dialogue").show();
    OpenDialogueReviewCautions('../Constraint/ViewCaution?constraintID=' + $('#ConstraintID').val() + '&cautionid=' + Cautionid + '&flageditmode=' + flageditmode + '&random=' + randomNumber
        , 'view-caution');
}
var deleted_CautionName = '';
var delete_CautionId = 0;
var deleteCautionButton;
function DeleteCautionWarning(deletebutton) {
    deleteCautionButton = deletebutton;
    deleted_CautionName = deletebutton.name;
    delete_CautionId = deletebutton.id;
    $("#exampleModalCenter1").hide();
    $('#confirmDeleteMsg').modal({ keyboard: false, backdrop: 'static' });
    $('#confirmDeleteMsg').modal('show');
    $(".modal-backdrop").removeClass("show");
    $(".modal-backdrop").removeClass("modal-backdrop");
}
function DeleteCaution() {

    var params = '{"cautionId":"' + delete_CautionId + '"}';
    $.ajax({
        async: false,
        type: "POST",
        url: '../Constraint/DeleteCaution',
        dataType: "json",
        //contentType: "application/json; charset=utf-8",
        data: params,
        processdata: true,
        success: function (result) {
            if (result == 'expire') {
                location.reload();

            }
            else if (result) {
                $('#successMsg').modal({ keyboard: false, backdrop: 'static' });
                $('#successMsg').modal('show');
                $(".modal-backdrop").removeClass("show");
                $(".modal-backdrop").removeClass("modal-backdrop");
            }
            else {
                $('#failMsg').modal({ keyboard: false, backdrop: 'static' });
                $('#failMsg').modal('show');
                $(".modal-backdrop").removeClass("show");
                $(".modal-backdrop").removeClass("modal-backdrop");
            }
        },
        error: function (result) {
        }
    });
}
function closeDeleteWarning() {
    $("#exampleModalCenter1").show();
    $('#successMsg').modal('hide');
}
function DeleteRow() {
    // $(deleteCautionButton).closest("tr").remove();
}
function EditCaution(cautionId) {
    $("#dialogue").hide();
    $("#dialogue").show();
    var randomNumber = Math.random();
    OpenDialogueReviewCautions('../Constraint/CreateCaution?ConstraintID=' + $('#ConstraintID').val() + '&CautionId=' + cautionId + '&mode=edit&random=' + randomNumber, 'create-caution');
}

function OpenDialogueReviewCautions(url,container) {
    startAnimation();
    $.ajax({
        async: false,
        type: "GET",
        url: url,
        processdata: true,
        success: function (response) {
            if (container != '' && container != undefined) {
                $("#dialogue").html($(response).closest('.' + container)[0]);
            } else {
                $("#dialogue").html(response);
            }
            stopAnimation();
            $("#dialogue").show();
            $("#overlay").show();
        },
        error: function (result) {
        }
    });
}

function CautionShowReportReviewCautionList(CautionId, ConstraintId, viewCaution=0) {
    $("#dialogue").hide();
    var randomNumber = Math.random();
    var flageditmode = $('#flageditmode').val();

    OpenDialogueReviewCautions('../Constraint/ViewCaution?constraintID=' + $('#ConstraintID').val() + '&cautionid=' + CautionId + '&flageditmode=' + flageditmode + '&random=' + randomNumber + '&viewCaution=' + viewCaution,
        'view-caution');
}

function ReloadReviewCautionList() {
    //$("#pop-warning").hide();
    $("#dialogue").hide();
    $("#dialogue").show();
    //$("#pop-warning").unload();
    //$("#dialogue").unload();
    //startAnimation();
    startAnimation("");
    var randomNumber = Math.random();
    OpenDialogueReviewCautions('../Constraint/ReviewCautionsList?ConstraintID=' + $('#ConstraintID').val() + '&random=' + randomNumber,'review-caution-list');
}
function ViewHistory() {
   
    var flageditmode = $('#flageditmode').val();
    var page = $('#page').val();
    var pagesize = $('#pagesize').val();
    $("#dialogue").show();
    //startAnimation();
    var randomNumber = Math.random();
    //window.location = '../Constraint/ConstraintHistory?ConstraintID=' + $('#ConstraintID').val() + '&flageditmode=' + flageditmode + '&random=' + randomNumber;
    //window.location = '/player_detail?username=' + name;
    OpenDialogueHistoryList('../Constraint/ConstraintHistoryPopUp?ConstraintID=' + $('#ConstraintID').val() + '&flageditmode=' + flageditmode + '&random=' + randomNumber + '&page=' + page + '&pageSize=' + pagesize, 'show-history-list');
}
function ReviewContacts() {
    var flageditmode = $('#flageditmode').val();
    //$("#dialogue").hide();
    //startAnimation();
    var randomNumber = Math.random();
    OpenDialogueReviewCautions('../Constraint/ReviewContactsList?constraintId=' + $('#ConstraintID').val() + '&flagEditMode=' + flageditmode + '&random=' + randomNumber,'review-contact-list');
}
function OpenDialogueHistoryList(url, container) {
    startAnimation();
    $.ajax({
        async: false,
        type: "GET",
        url: url,
        processdata: true,
        success: function (response) {
            if (container != '' && container != undefined) {
                $("#dialogue").html($(response).closest('.' + container)[0]);
            } else {
                $("#dialogue").html(response);
            }
            stopAnimation();
            $("#dialogue").show();
            $("#overlay").show();
        },
        error: function (result) {
        }
    });
}