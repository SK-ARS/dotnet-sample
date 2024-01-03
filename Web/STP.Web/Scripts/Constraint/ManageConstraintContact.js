$(document).ready(function () {
    stopAnimation();
    $("#overlay").css("display", "block");
    $("#overlay").css('background-color', 'rgba(0, 0, 0, 0.6)');
    $("#dialogue").css("display", "contents");

    $('body').on('click', '.manage-constraint-contact .btn-save-contact-popup', function () {
        saveContactPopup();
    });

    $('body').on('click', '.manage-constraint-contact .btn-review-contact', function () {
        ReviewContacts();
    });

    $('body').on('click', '.manage-constraint-contact .btn-open-manage-constraint-nav', function () {
        OpenNavMCC();
    });

    $('body').on('click', '.manage-constraint-contact .btn-close-manage-constraint-nav', function () {
        CloseNavMCC();
    });

    $('body').on('click', '.manage-constraint-contact .btn-save-const-contact', function () {
        SaveConstraintContact();
    });

    $('body').on('click', '.manage-constraint-contact .btn-save-const-contact-close', function () {
        closePopup();
    });

    $('body').on('mouseenter', '.manage-constraint-contact #CountryId', function () {
        var selectedVal = $("#CountryId option:selected").text();
        $("#lblSelectedVal").html(selectedVal);
    });

    $('body').on('mouseleave', '.manage-constraint-contact #CountryId', function () {
        var selectedVal = $("#CountryId option:selected").text();
    });
});
function saveContactPopup() {
    var emailPattern = new RegExp(/^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/);
    var phonePattern = new RegExp(/^(\+)?[0-9\ -]+$/);

    var orgNameValid = true;
    var emailValid = true;
    var faxValid = true;
    var postCodeValid = true;

    var telephoneValid = true;
    var extensionValid = true;
    var mobileValid = true;
    var countryValid = true;

    if ($("#OrganisationName").val() == '') {

        $("#ownerValidationMsg").removeClass("success");
        $("#ownerValidationMsg").addClass("error");
        orgNameValid = false;
    }
    else {
        $("#ownerValidationMsg").removeClass("error");
        $("#ownerValidationMsg").addClass("success");
    }

    if ($("#Email").val() != '') {
        if (!emailPattern.test($("#Email").val())) {

            $("#emailValidationMsg").removeClass("success");
            $("#emailValidationMsg").addClass("error");
            emailValid = false;
        }
        else {
            $("#emailValidationMsg").removeClass("error");
            $("#emailValidationMsg").addClass("success");
        }
    }

    if ($("#Fax").val() != '') {
        if (!phonePattern.test($("#Fax").val())) {

            $("#faxValidationMsg").removeClass("success");
            $("#faxValidationMsg").addClass("error");
            faxValid = false;
        }
        else {
            $("#faxValidationMsg").removeClass("error");
            $("#faxValidationMsg").addClass("success");
        }
    }

    if ($("#PostCode").val() != '') {
        if (!phonePattern.test($("#PostCode").val())) {

            $("#postCodeValidationMsg").removeClass("success");
            $("#postCodeValidationMsg").addClass("error");
            postCodeValid = false;
        }
        else {
            $("#postCodeValidationMsg").removeClass("error");
            $("#postCodeValidationMsg").addClass("success");
        }
    }

    if ($("#Telephone").val() != '') {
        if (!phonePattern.test($("#Telephone").val())) {

            $("#telephoneValidationMsg").removeClass("success");
            $("#telephoneValidationMsg").addClass("error");
            telephoneValid = false;
        }
        else {
            $("#telephoneValidationMsg").removeClass("error");
            $("#telephoneValidationMsg").addClass("success");
        }
    }

    if ($("#Extension").val() != '') {
        if (!phonePattern.test($("#Extension").val())) {

            $("#extensionValidationMsg").removeClass("success");
            $("#extensionValidationMsg").addClass("error");
            extensionValid = false;
        }
        else {
            $("#extensionValidationMsg").removeClass("error");
            $("#extensionValidationMsg").addClass("success");
        }
    }

    if ($("#Mobile").val() != '') {
        if (!phonePattern.test($("#Mobile").val())) {

            $("#mobileValidationMsg").removeClass("success");
            $("#mobileValidationMsg").addClass("error");
            mobileValid = false;
        }
        else {
            $("#mobileValidationMsg").removeClass("error");
            $("#mobileValidationMsg").addClass("success");
        }
    }

    if ($("#CountryId option:selected").text() == '--Select Country--') {

        $("#countryValidationMsg").removeClass("success");
        $("#countryValidationMsg").addClass("error");
        countryValid = false;
    }
    else {
        $("#countryValidationMsg").removeClass("error");
        $("#countryValidationMsg").addClass("success");
    }

    if (orgNameValid && emailValid && faxValid && postCodeValid && telephoneValid && extensionValid && mobileValid && countryValid) {
        $('#exampleModalCenter').modal({ keyboard: false, backdrop: 'static' });
        $("#exampleModalCenter").modal('show');
        $(".modal-backdrop").removeClass("show");
        $(".modal-backdrop").removeClass("modal-backdrop");
    }
}
function closePopup() {
    $("#exampleModalCenter").modal('hide');
}
function SaveConstraintContact() {

    

        $.ajax({
            url: '../Constraint/SaveConstraintContact',
            dataType: 'json',
            type: 'POST',
            data: $("#ConstraintContactInfo").serialize(),
            success: function (result) {
                if (result.Success) {
                    if ($('#mode').val() == "add") {
                        OpenConstraintContact();
                        //showWarningPopDialog('Contact added successfully', 'Ok', '', 'OpenConstraintContact', '', 1, 'info');
                    }
                    else {
                        OpenConstraintContact();
                        //showWarningPopDialog('Contact updated successfully', 'Ok', '', 'OpenConstraintContact', '', 1, 'info');
                    }
                }
                else {
                    ShowErrorPopup("Contact creation failed");
                }

            },
            error: function (xhr, status) {
            }
        });
    
}
function OpenConstraintContact() {
    var flageditmode = $('#flageditmode').val();
    $("#pop-warning").hide();
    var randomNumber = Math.random();
    OpenDialogueManageConstraintContact('../Constraint/ReviewContactsList?constraintId=' + $('#ConstraintID').val() + '&flagEditMode=' + flageditmode + '&random=' + randomNumber
        , 'review-contact-list');
}
function ReviewContacts() {
    var flageditmode = $('#flageditmode').val();
    var randomNumber = Math.random();
    OpenDialogueManageConstraintContact('../Constraint/ReviewContactsList?constraintId=' + $('#ConstraintID').val() + '&flagEditMode=' + flageditmode + '&random=' + randomNumber
        , 'review-contact-list')
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
function OpenNavMCC() {

    $("#add-contact").css("width","345px");
    $("#add-contact").css("display","block");

    $("#card-swipecon1").css("display","none");
    $("#card-swipecon2").css("display","inline-block");
    $("#card-swipecon2").css("margin-left","415px");
    function myFunction(x) {
        if (x.matches) { // If media query matches
            document.getElementById("mySidenav1").style.width = "auto";
        }
    }
    var x = window.matchMedia("(max-width: 410px)")
    myFunction(x) // Call listener function at run time
    x.addListener(myFunction)

}
function CloseNavMCC() {

    $("#add-contact").css("width","0px");
    $("#add-contact").css("display","none");

    $("#card-swipecon1").css("display","inline-block");
    $("#card-swipecon2").css("display","none");

}
