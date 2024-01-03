var StructureIDVal = $('#hf_StructureID').val();
var StructureID1Val = $('#hf_StructureId1').val();
var CautionIDVal = $('#hf_CautionID').val();
var SectionIDVal = $('#hf_SectionID').val();
var SectionId1Val = $('#hf_sectionId1').val();

$(document).ready(function () {
$('body').on('click','.CreateContact', function(e) { 
  e.preventDefault();
  CreateStructureContact(this);
}); 
$('body').on('click','.contactView', function(e) { 
  e.preventDefault();
  StructureContactViewDetail(this);
}); 
$('body').on('click','.editContact', function(e) { 
  e.preventDefault();
  EditStructureContactFn(this);
}); 
$('body').on('click','.deleteContact', function(e) { 
  e.preventDefault();
  DeleteStructureContact(this);
}); 
$('body').on('click','#btn_back', function(e) { 
  e.preventDefault();
  Back(this);
}); 

});

$('#reviewContactPaginator').on('click', 'a', function (e) {
    if (this.href == '') {
        return false;
    }
    else {
        $.ajax({
            url: this.href,
            type: 'GET',
            cache: false,
            success: function (result) {
                $('#generalSettingsId').html(result);
            }
        });
        return false;
    }
});
var randomNumber = Math.random();
function CreateStructureContact() {
    $("#generalSettingsId").hide();
    startAnimation();
    var randomNumber = Math.random();
    $("#causionReport").load('../Structures/ManageStructureContact?structureId=' + StructureIDVal + '&cautionId=' + CautionIDVal + '&contactNo=0&mode=add&sectionId=' + SectionIDVal + '&random=' + randomNumber, function () {
        stopAnimation();
    });

}
function Back() {
    startAnimation();
    $("#generalSettingsId").empty();
    var randomNumber = Math.random();

    $("#generalSettingsId").load('../Structures/ReviewCautionsList?structureId=' + StructureIDVal + "&sectionId=" + SectionIDVal + '&random=' + randomNumber,
        function () {
            $('#manageCautionId').show();
            stopAnimation();
            ReviewCautionsListInit();
        }
        );
}
function ViewCausionList() {
    startAnimation();
    $("#generalSettingsId").empty();

    $("#generalSettingsId").load('../Structures/ReviewCautionsList?structureId=' + StructureID1Val + "&sectionId=" + SectionId1Val + '&random=' + randomNumber,
        function () {
            $('#manageCautionId').show();
            stopAnimation();
            ReviewCautionsListInit();
        }
        );
}
function CreateStructureContact() {
    startAnimation();
    $("#generalSettingsId").hide();
    $("#causionReport").load('../Structures/ManageStructureContact?structureId=' + StructureIDVal +'&cautionId=' + CautionIDVal + '&contactNo=0&mode=add&sectionId=' + SectionIDVal + '&random=' + randomNumber, function () {
        $('#addContactId').show();
        stopAnimation();
    });

}
function StructureContactView(ContactNo, CautionID) {
    var randomNumber = Math.random();
    startAnimation();
    $("#causionReport").load('../Structures/ManageStructureContact?structureId=' + StructureIDVal +'&cautionId=' + CautionIDVal + '&contactNo=' + ContactNo + '&mode=view&sectionId=' + SectionIDVal + '&random=' + randomNumber, function () {
        $('#ownerContactDetails').modal('show');
        stopAnimation();
    });

}
function EditStructureContact(ContactNo, CautionID) {
    var randomNumber = Math.random();
    startAnimation();
    $("#generalSettingsId").hide();
    $("#causionReport").load('../Structures/ManageStructureContact?structureId=' + StructureIDVal +'&cautionId=' + CautionID + '&contactNo=' + ContactNo + '&mode=edit&sectionId=' + SectionIDVal + '&random=' + randomNumber, function () {
        $('#addContactId').show();
        stopAnimation();
    });
}
var delete_ContactNo;
function DeleteStructureContactWarning(deletebutton, CautionID) {
    deleteContactButton = deletebutton;
    deleted_ContactName = deletebutton.name;
    delete_ContactNo = deletebutton.id;
    var Msg = "Do you want to delete '" + "" + "'" + deleted_ContactName + "'" + "" + "' ?";
    ShowWarningPopup(Msg, 'DeleteContact');
}
function DeleteContact() {
    $('#WarningPopup').modal('hide');
        var params = '{"cautionId":"' + CautionIDVal + '","contactNo":"' + delete_ContactNo + '"}';
        $.ajax({
        async: false,
        type: "POST",
        url: '../Structures/DeleteStructureContact',
        dataType: "json",
        //contentType: "application/json; charset=utf-8",
        data: params,
        processdata: true,
        success: function (result) {
            if (result == 'true') {
                var message = '"' + deleted_ContactName + '"  deleted successfully';
                ShowSuccessModalPopup(message, 'ViewContactList');
            }
        },
        error: function (result) {
        }
    });
}
function ViewContactList() {
    CloseSuccessModalPopup();
    startAnimation();
    $("#generalSettingsId").load('../Structures/ViewStructureContactsList?structureId=' + StructureIDVal + '&cautionId=' + CautionIDVal + '&sectionId=' + SectionIDVal + '&random=' + randomNumber,
        function () {
            $("#viewContactListId").show();
            $("#generalSettingsId").show();
            stopAnimation();
        }
        );
}

function StructureContactViewDetail(e) {
    var contactNo =$(e).attr("contactno");
    var contactId =$(e).attr("contactid");
    StructureContactView(contactNo, contactId);
}
function EditStructureContactFn(e) {
    var contactNo =$(e).attr("contactno");
    var cautionId =$(e).attr("cautionid");
    EditStructureContact(contactNo, cautionId);
}
function DeleteStructureContact(e) {
    var contactId =$(e).attr("contactid");
    DeleteStructureContactWarning(e, contactId);
}
