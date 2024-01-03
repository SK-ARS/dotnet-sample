$(document).ready(function () {
    $("#dialogue").css("display", "block");
    $("#overlay").css("display", "block");

    $('body').on('click', '.review-contact-list .btn-create-constraint-contact', function () {
        CreateConstraintContact();
    });

    $('body').on('click', '.review-contact-list .btn-rc-close', function () {
        onremove();
    });

    $('body').on('click', '.review-contact-list .btn-rc-edit-contact', function () {
        var ContactNo = $(this).data('contactno');
        var ConstraintId = $(this).data('constraintid');
        EditContact(ContactNo, ConstraintId);
    });

    $('body').on('click', '.review-contact-list .btn-constraint-coantact-view', function () {
        var ContactNo = $(this).data('contactno');
        var ConstraintId = $(this).data('constraintid');
        ConstraintCoantactView(ContactNo, ConstraintId);
    });

    $('body').on('click', '.review-contact-list .btn-rc-delete-contact-warning', function () {
        var ConstraintId = $(this).data('constraintid');
        DeleteContactWarning(this, ConstraintId);
    });
    $('body').on('click', '.review-contact-list .btn-rc-review-caution', function () {
        ReviewCautions();
    });
    $('body').on('click', '.review-contact-list .btn-rc-view-history', function () {
        ViewHistory();
    });


    $('body').on('click', '.review-contact-list .btn-rc-cancel-confirmation', function () {
        cancelConfirmation();
    });

    $('body').on('click', '.review-contact-list .btn-rc-delete-contact-1', function () {
        DeleteContact();
    });

    $('body').on('click', '.review-contact-list .btn-rc-reload-review-contact', function () {
        ReloadReviewContactsList();
    });
});
var ConstraintID = parseInt($("#ConstraintID").val());

$('#reviewCausionPaginator').on('click', 'a', function (e) {
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

function ViewHistory() {
    var flageditmode = $('#flageditmode').val();
    //$("#dialogue").hide();
    //startAnimation();
    var randomNumber = Math.random();
    window.location = '../Constraint/ConstraintHistory?ConstraintID=' + $('#ConstraintID').val() + '&flageditmode=' + flageditmode + '&random=' + randomNumber;
    //window.location = '/player_detail?username=' + name;
}
function ReviewCautions() {
    var flageditmode = $('#flageditmode').val();
    var randomNumber = Math.random();
    OpenDialogueReviewContactList('../Constraint/ReviewCautionsList?constraintId=' + $('#ConstraintID').val() + '&flageditmode=' + flageditmode + '&random=' + randomNumber
        , 'review-caution-list');
}
function OpenDialogueReviewContactList(url, container) {
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

function CreateContact() {
    $("#dialogue").hide();
    //startAnimation();
    var randomNumber = Math.random();
}

function CreateConstraintContact() {
    startAnimation();
    var ConstraintID = parseInt($("#ConstraintID").val());
    $("#dialogue").hide();
    var randomNumber = Math.random();
    OpenDialogueReviewContactList('../Constraint/ManageConstraintContact?ConstraintID=' + ConstraintID + '&ContactNo=0&mode=add&random=' + randomNumber
        , 'manage-constraint-contact');
}

function EditContact(ContactNo, ConstraintID) {
    startAnimation();
    var randomNumber = Math.random();
    OpenDialogueReviewContactList('../Constraint/ManageConstraintContact?ConstraintID=' + ConstraintID + '&ContactNo=' + ContactNo + '&mode=edit&random=' + randomNumber
        , 'manage-constraint-contact');
}

function ConstraintCoantactView(ContactNo, constraintID) {
    var flageditmode = $('#flageditmode').val();
    var randomNumber = Math.random();
    OpenDialogueReviewContactList('../Constraint/ManageConstraintContact?ConstraintID=' + constraintID + '&ContactNo=' + ContactNo + '&flageditmode=' + flageditmode + '&mode=view&random=' + randomNumber
        , 'manage-constraint-contact');
}

var deleted_ContactName = '';
var delete_ContactId = 0;
var deleteContactButton;
function DeleteContactWarning(deletebutton, constraintID) {
    deleteContactButton = deletebutton;
    deleted_ContactName = deletebutton.name;
    delete_ContactNo = deletebutton.id;
    $("#exampleModalCenter1").hide();
    $('#confirmDeleteMsg').modal({ keyboard: false, backdrop: 'static' });
    $('#confirmDeleteMsg').modal('show');
    $(".modal-backdrop").removeClass("show");
    $(".modal-backdrop").removeClass("modal-backdrop");
}
function cancelConfirmation() {
    $("#exampleModalCenter1").show();
}
function DeleteContact() {
    var params = '{"constraintId":"' + ConstraintID + '","contactNo":"' + delete_ContactNo + '"}';
    ;
    $.ajax({
        async: false,
        type: "POST",
        url: '../Constraint/DeleteContact',
        dataType: "json",
        //contentType: "application/json; charset=utf-8",
        data: params,
        processdata: true,
        success: function (result) {
            if (result) {
                $('#confirmDeleteMsg').modal({ keyboard: false, backdrop: 'static' });
                $('#successMsg').modal('show');
                $(".modal-backdrop").removeClass("show");
                $(".modal-backdrop").removeClass("modal-backdrop");
            }
            else if (result == 'expire') {
                window.location.href = "../Account/Login";
            }
            else {
                $('#failMsg').modal('show');
            }
        },
        error: function (result) {
        }
    });
}

function onremove() {
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

function ReloadReviewContactsList() {
    var flageditmode = $('#flageditmode').val();
    var constId = $('#ConstraintID').val();
    $("#dialogue").show();
    var randomNumber = Math.random();
    $("#dialogue").load('../Constraint/ReviewContactsList?constraintId=' + $('#ConstraintID').val() + '&flagEditMode=' + flageditmode);
}
