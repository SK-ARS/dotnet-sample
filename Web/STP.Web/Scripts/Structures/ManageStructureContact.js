var structid = $('#hf_structureid').val();
var CautionId = $('#hf_CautionId').val();
var SectionID = $('#hf_SectionID').val();


$(document).ready(function () {
$('body').on('click','#btnsavestructurecontact', function(e) { 
  e.preventDefault();
  SaveStructureContact(this);
}); 
       
$('body').on('click','#btncancelstruct', function(e) { 
  e.preventDefault();
  Cancel(this);
}); 
    });
    function Cancel() {
        startAnimation();
        $("#generalSettingsId").load('../Structures/ViewStructureContactsList?structureId=' + structid + '&cautionId=' + CautionId + '&sectionId=' + SectionID + '&random=' + randomNumber,
            function () {
                $("#causionReport").empty();
                $("#viewContactListId").show();
                $("#generalSettingsId").show();
                stopAnimation();
            }
        );
    }
    function SaveStructureContact() {

        var emailPattern = new RegExp(/^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/);
        var phonePattern = new RegExp(/^(\+)?[0-9\ -]+$/);
        var postCodePattern = new RegExp(/^[a-z]{1,2}\d[a-z\d]?\s*\d[a-z]{2}$/i);

        var orgNameValid = true;
        var emailValid = true;
        var faxValid = true;
        var postCodeValid = true;

        var telephoneValid = true;
        var extensionValid = true;
        var mobileValid = true;
        var countryValid = true;

        if ($("#OrganisationName").val() == '') {

            $("#ownerValidationMsg").show();
            orgNameValid = false;
        }
        else {
            $("#ownerValidationMsg").hide();
        }

        if ($("#Email").val() != '') {
            if (!emailPattern.test($("#Email").val())) {

                $("#emailValidationMsg").show();
                emailValid = false;
            }
            else {
                $("#emailValidationMsg").hide();
            }
        }

        if ($("#Fax").val() != '') {
            if (!phonePattern.test($("#Fax").val())) {

                $("#faxValidationMsg").show();
                faxValid = false;
            }
            else {
                $("#faxValidationMsg").hide()
            }
        }

        if ($("#PostCode").val() != '') {
            if (!postCodePattern.test($("#PostCode").val())) {

                $("#postCodeValidationMsg").show();
                postCodeValid = false;
            }
            else {
                $("#postCodeValidationMsg").hide();
            }
        }

        if ($("#Telephone").val() != '') {
            if (!phonePattern.test($("#Telephone").val())) {

                $("#telephoneValidationMsg").show();
                telephoneValid = false;
            }
            else {
                $("#telephoneValidationMsg").hide();
            }
        }

        if ($("#Extension").val() != '') {
            if (!phonePattern.test($("#Extension").val())) {

                $("#extensionValidationMsg").show();
                extensionValid = false;
            }
            else {
                $("#extensionValidationMsg").hide();
            }
        }

        if ($("#Mobile").val() != '') {
            if (!phonePattern.test($("#Mobile").val())) {

                $("#mobileValidationMsg").show();
                mobileValid = false;
            }
            else {
                $("#mobileValidationMsg").hide();
            }
        }

        if ($("#CountryId option:selected").text() == 'Select Country') {

            $("#countryValidationMsg").show();
            countryValid = false;
        }
        else {
            $("#countryValidationMsg").hide();
        }

        if (orgNameValid && emailValid && faxValid && postCodeValid && telephoneValid && extensionValid && mobileValid && countryValid) {
            $.ajax({
                url: '../Structures/SaveStructureContact',
                dataType: 'json',
                type: 'POST',
                data: $("#StructureContactInfo").serialize(),
                success: function (result) {
                    if (result.Success) {
                        if ($('#mode').val() == "add") {
                            ShowSuccessModalPopup('Contact added successfully','ViewStructureCautionContacts');
                        }
                        else {
                            ShowSuccessModalPopup('Contact updated successfully', 'ViewStructureCautionContacts');
                        }
                    }
                },
                error: function (xhr, status) {
                }
            });
        }
    }
    function ViewStructureCautionContacts() {
        CloseSuccessModalPopup();
        startAnimation();
        var randomNumber = Math.random();
        $("#generalSettingsId").load('../Structures/ViewStructureContactsList?structureId=' + structid + '&cautionId=' + CautionId + '&sectionId=' + SectionID + '&random=' + randomNumber,
            function () {
                $("#causionReport").empty();
                $("#viewContactListId").show();
                $("#generalSettingsId").show();
                stopAnimation();
            }
        );
    }
    function closeStructureContact() {
        $('#ownerContactDetails').modal('hide');
    }
