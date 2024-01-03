var ConstraintID = parseInt($("#ConstraintID").val());
function ViewConstraintInit() {
    ConstraintID = parseInt($("#ConstraintID").val());
    $("#dialogue").css("display", "block");
    stopAnimation();
    $("#overlay").css("display", "block");
    $("#overlay").css('background-color', 'rgba(0, 0, 0, 0.6)');
}
$(document).ready(function () {
    //ViewConstraintInit();

    $('body').on('click', '.view-constraint .btn-vc-edit-constraint', function () {
        EditConstraint();
        return false;
    });

    $('body').on('click', '.view-constraint .btn-vc-closs-view-dtls', function () {
        ClossViewDtls();
        return false;
    });

    $('body').on('click', '.view-constraint .btn-vc-delete', function () {
        var ConstraintName = $(this).data('constraintname');
        Delete(ConstraintName);
        return false;
    });

    $('body').on('click', '.view-constraint .btn-vc-review-cautions', function () {
        ReviewCautions();
        return false;
    });

    $('body').on('click', '.view-constraint .btn-vc-review-cantacts', function () {
        ReviewContacts();
        return false;
    });

    $('body').on('click', '.view-constraint .btn-vc-view-history', function () {
        ViewHistory();
        return false;
    });

    $('body').on('click', '.view-constraint .btn-vc-close-delete-popup', function () {
        closeDeletePopup();
        return false;
    });

    $('body').on('click', '.view-constraint .btn-vc-delete-data', function () {
        DeleteData();
        return false;
    });

});
function Delete(Name) {
    $("#CloseViewCons").css("display", "none");
    deletedContactName = Name;
    var Msg = "Do you want to delete <br />'" + "" + "'" + Name + "' and the related cautions" + "" + "' ?";
    $('#DeleteConstPopup').modal({ keyboard: false, backdrop: 'static' });
    $("#overlay").css("display", "block");
    $('#DeleteConstPopup').show();
    $(".modal-backdrop").removeClass("show");
    $(".modal-backdrop").removeClass("modal-backdrop");
}
function EditConstraint() {
    var randomNumber = Math.random();
    $("#dialogue").hide();
    OpenDialogueViewConstraint('../Constraint/EditConstraint?ConstraintID=' + ConstraintID + '&=random=' + randomNumber, 'edit-constraint', function () {
        EditConstraintInit();
    });
}
function OpenDialogueViewConstraint(url, container,callBackFn) {
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
            if (callBackFn != null && callBackFn != undefined && callBackFn != '') {
                callBackFn();
            }
        },
        error: function (result) {
        }
    });
}

function ClossViewDtls() {
    $("#CloseViewCons").css("display", "none");
    $("#dialogue").hide();
    $("#overlay").hide();


}
function closeDeletePopup() {
    $("#CloseViewCons").css("display", "block");
    $("#DeleteConstPopup").css("display", "none");

    $('#DeleteConstPopup').modal('hide');
}
function DeleteData() {
    var params = '{"ConID":"' + ConstraintID + '"}';
    $.ajax({
        async: false,
        type: "POST",
        url: '../Constraint/DeleteConstraint',
        dataType: "json",
        //contentType: "application/json; charset=utf-8",
        data: params,
        processdata: true,
        success: function (Success) {
            if (Success) {
                //alert("Deleted");
                $("#CloseViewCons").css("display", "none");
                $("#overlay").css("display", "none");
                $("#dialogue").hide();
                $('#DeleteConstPopup').modal('hide');
                ShowModalPopup("Constraint/Caution deleted successfully");
                $("#CloseSuccessPopup").attr("onclick", "CloseModalPopupRef()");

            }
            else {
                ShowErrorPopup("Delete Constraint failed");
            }


            ConstraintDeletionflag(true, ConstraintID);

},
error: function (result) {
}
        });
    }

function ViewHistory() {
    var flageditmode = $('#flageditmode').val();
    var randomNumber = Math.random();
    var urlObject = new URL(window.location.href);
    sessionStorage.removeItem('backurl');
    sessionStorage.setItem('backurl', urlObject);
    //window.location = '../Constraint/ConstraintHistory?ConstraintID=' + $('#ConstraintID').val() + '&flageditmode=' + flageditmode + '&random=' + randomNumber + '&from=viewconstrainst';

    $("#displayConstraintHistory").load('../Constraint/ConstraintHistory?ConstraintID=' + $('#ConstraintID').val() + '&flageditmode=' + flageditmode + '&random=' + randomNumber + '&from=viewconstrainst', function () {
        $("#vehicles").hide();
        $("#displayConstraintHistory").show();
        $("#dialogue").hide();
        $("#overlay").hide();

    });

    //window.location = '/player_detail?username=' + name;
}
function ReviewCautions() {
    var flageditmode = $('#flageditmode').val();
    var randomNumber = Math.random();
    OpenDialogueViewConstraint('../Constraint/ReviewCautionsList?constraintId=' + $('#ConstraintID').val() + '&flageditmode=' + flageditmode + '&random=' + randomNumber,
        'review-caution-list');
}
function ReviewContacts() {
    var flageditmode = $('#flageditmode').val();
    var randomNumber = Math.random();
    OpenDialogueViewConstraint('../Constraint/ReviewContactsList?constraintId=' + $('#ConstraintID').val() + '&flagEditMode=' + flageditmode + '&random=' + randomNumber,
    'review-contact-list');
}
