
    $(document).ready(function () {
        $(".CreateContact").on('click', CreateStructureContact);
        $(".contactView").on('click', StructureContactViewDetail);
        $(".editContact").on('click', EditStructureContactFn);
        $(".deleteContact").on('click', DeleteStructureContact);
        $("#btn_back").on('click', Back);
        
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
        $("#causionReport").load('../Structures/ManageStructureContact?structureId=' + @ViewBag.StructureID +'&cautionId=' + @ViewBag.CautionID + '&contactNo=0&mode=add&sectionId=' + @ViewBag.SectionID + '&random=' + randomNumber, function () {
            stopAnimation();
        });

    }
    function Back() {
        startAnimation();
        $("#generalSettingsId").empty();
        var randomNumber = Math.random();

        $("#generalSettingsId").load('../Structures/ReviewCautionsList?structureId=' + '@ViewBag.StructureID' + "&sectionId=" + @ViewBag.SectionID + '&random=' + randomNumber,
            function () {
                $('#manageCautionId').show();
                stopAnimation();
            }
        );
    }
    function ViewCausionList() {
        startAnimation();
        $("#generalSettingsId").empty();

        $("#generalSettingsId").load('../Structures/ReviewCautionsList?structureId=' + '@ViewBag.StructureId' + "&sectionId=" + @ViewBag.sectionId + '&random=' + randomNumber,
            function () {
                $('#manageCautionId').show();
                stopAnimation();
            }
        );
    }
    function CreateStructureContact() {
        startAnimation();
        $("#generalSettingsId").hide();
        $("#causionReport").load('../Structures/ManageStructureContact?structureId=' + @ViewBag.StructureID +'&cautionId=' + @ViewBag.CautionID + '&contactNo=0&mode=add&sectionId=' + @ViewBag.SectionID + '&random=' + randomNumber, function () {
            $('#addContactId').show();
            stopAnimation();
        });

    }
    function StructureContactView(ContactNo, CautionID) {
        var randomNumber = Math.random();
        startAnimation();
        $("#causionReport").load('../Structures/ManageStructureContact?structureId=' + @ViewBag.StructureID +'&cautionId=' + @ViewBag.CautionID + '&contactNo=' + ContactNo + '&mode=view&sectionId=' + @ViewBag.SectionID + '&random=' + randomNumber, function () {
            $('#ownerContactDetails').modal('show');
            stopAnimation();
        });

    }
    function EditStructureContact(ContactNo, CautionID) {
        var randomNumber = Math.random();
        startAnimation();
        $("#generalSettingsId").hide();
        $("#causionReport").load('../Structures/ManageStructureContact?structureId=' + @ViewBag.StructureID +'&cautionId=' + CautionID + '&contactNo=' + ContactNo +'&mode=edit&sectionId=' + @ViewBag.SectionID + '&random=' + randomNumber, function () {
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
        ShowWarningPopup(Msg,'DeleteContact');
    }
    function DeleteContact() {
        $('#WarningPopup').modal('hide');
        var params  = $('#hf_CautionID + '","contactNo":"' + delete_ContactNo + '').val(); 
        $.ajax({
            async: false,
            type: "POST",
            url: '@Url.Action("DeleteStructureContact", "Structures")',
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
        $("#generalSettingsId").load('../Structures/ViewStructureContactsList?structureId=' + @ViewBag.StructureID + '&cautionId=' + @ViewBag.CautionID + '&sectionId=' + @ViewBag.SectionID + '&random=' + randomNumber,
            function () {
                $("#viewContactListId").show();
                $("#generalSettingsId").show();
                stopAnimation();
            }
        );
    }

    function StructureContactViewDetail(e) {
        var contactNo = e.currentTarget.dataset.ContactNo;
        var contactId = e.currentTarget.dataset.ContactId;
        StructureContactView(contactNo, contactId);
    }
    function EditStructureContactFn(e) {
        var contactNo = e.currentTarget.dataset.ContactNo;
        var cautionId = e.currentTarget.dataset.CautionId;
        EditStructureContact(contactNo, cautionId);
    }
    function DeleteStructureContact(e) {
        var contactId = e.currentTarget.dataset.ContactId;
        DeleteStructureContactWarning(e, contactId);
    }
