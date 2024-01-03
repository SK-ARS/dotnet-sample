
function closeCautionSpanAddRep() {
    var randomNumber = Math.random();
    $(".modal-backdrop").css("height", "0px");
    OpenDialogueManageConstraintContact('../Constraint/ReviewCautionsList?constraintId=' + $('#ConstraintID').val() + '&random=' + randomNumber
        , 'review-caution-list');
}
function OpenDialogueManageConstraintContact(url, container) {
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

$('body').on('click', '.btn-cau-close-nav', function () {
    CloseNavCautionAddReport();
});
function CloseNavCautionAddReport() {

    $("#createConsDiv").css("width","0px");
    $("#createConsDiv").css("display","none");

    $("#card-swipecon1").css("display","inline-block");
    $("#card-swipecon2").css("display","none");


    //document.getElementById("MakingcontraintsInfo").style.width = "0";
    //document.getElementById("createConsDiv").style.width = "0";
    //document.getElementById("card-swipecon1").style.display = "none";
    //document.getElementById("card-swipecon2").style.display = "block";

}

$('body').on('click', '.btn-cau-open-nav', function () {
    OpenNavCautionAddReport()
});
function OpenNavCautionAddReport() {

    $("#createConsDiv").css("width","345px");
    $("#createConsDiv").css("display","block");

    $("#card-swipecon1").css("display","none");
    $("#card-swipecon2").css("display","inline-block");
    $("#card-swipecon2").css("margin-left","415px");

    //document.getElementById("createConsDiv").style.width = "345px";
    //document.getElementById("card-swipecon1").style.display = "block"
    //document.getElementById("card-swipecon2").style.display = "block"
    function myFunction(x) {
        if (x.matches) { // If media query matches
            document.getElementById("mySidenav1").style.width = "auto";
        }
    }
    var x = window.matchMedia("(max-width: 410px)")
    myFunction(x) // Call listener function at run time
    x.addListener(myFunction)
    //function myFunction(x) {
    //    if (x.matches) { // If media query matches
    //        document.getElementById("mySidenav1").style.width = "auto";
    //    }
    //}
    //var x = window.matchMedia("(max-width: 410px)")
    //myFunction(x) // Call listener function at run time
    //x.addListener(myFunction)
}

$('body').on('click', '.btn-cau-create-caution-from-report', function () {
    OpenCreateCautionFromReport(); return false;
});
function OpenCreateCautionFromReport() {
    $("#pop-warning").hide();
    var randomNumber = Math.random();
    OpenDialogueManageConstraintContact('../Constraint/CreateCaution?ConstraintID=' + $('#ConstraintID').val() + '&CautionId=' + $('#CautionId').val() + '&mode=return&random=' + randomNumber + '&FirstCautionFlag=' + 1
        , 'create-caution');
}

function closeFildPopup() {
    $('#createCotfailedPopup').modal('hide');

}

$('body').on('click', '.btn-reload-popup-cau', function () {
    reloadFildPopup();
});
function reloadFildPopup() {
    location.reload();

}

$('body').on('click', '.btn-cau-save-cautions-report', function () {
    SaveCautionsFromReport();
});
function SaveCautionsFromReport() {

    $.ajax({
        url: '../Constraint/SaveCautions',
        dataType: 'json',
        type: 'POST',
        data: $("#CautionInfo").serialize(),
        success: function (result) {
            if (result == "No Change") {
                showToastMessage({
                    message: "No changes to save.",
                    type: "error"
                });
                OpenReviewCautionFromReport();
            }
            else if (result) {
                ;
                if ($('#mode').val() == "add") {
                    if ($('#FirstCautionFlag').val() == 1) {
                        showToastMessage({
                            message: "Constraints and Caution added successfully",
                            type: "success"
                        });
                        OpenReviewCautionFromReport();
                    } else {
                        showToastMessage({
                            message: "Constraints and Caution added successfully",
                            type: "success"
                        });
                        OpenReviewCautionFromReport();
                    }
                }
                else {
                    showToastMessage({
                        message: "Constraint and Caution updated successfully",
                        type: "success"
                    });
                    OpenReviewCautionFromReport();
                }
            }
            else {
                if ($('#mode').val() == "add") {
                    showToastMessage({
                        message: "Caution creation failed",
                        type: "error"
                    });
                    reloadFildPopup();
                }
                else {
                    showToastMessage({
                        message: "Caution creation failed",
                        type: "error"
                    });
                    reloadFildPopup();
                }
            }
        },
        error: function (xhr, status) {
        }
    });
}

$('body').on('click', '.btn-review-caution', function () {
    OpenReviewCautionFromReport(); return false;
});
function OpenReviewCautionFromReport() {
    var randomNumber = Math.random();
    OpenDialogueManageConstraintContact('../Constraint/ReviewCautionsList?ConstraintId=' + $('#ConstraintID').val() + '&random=' + randomNumber
        , 'review-caution-list');
}

function redirectpage() {
    $("#dialogue").hide();
    $("#overlay").show();
    //startAnimation();
    window.location.href = "../Account/Login";
}
