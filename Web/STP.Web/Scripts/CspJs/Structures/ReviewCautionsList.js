    $(document).ready(function () {
        $(".viewcaution").on('click', ViewCautiondetails);
        $(".viewstructurecaution").on('click', ViewStructureCautionContactsdetails);
        $(".editcaution").on('click', EditCautiondetails);
        $(".deletecaution").on('click', DeleteCautionWarningdetails);
        $(".addcausion").on('click', addingCausion);
        $(".openhistory").on('click', openingHistory);
    });
    function ViewCautiondetails(e) {
        var arg1 = e.currentTarget.dataset.arg1;
        ViewCaution(arg1);
    }
    function EditCautiondetails(e) {
        var arg1 = e.currentTarget.dataset.arg1;
        EditCaution(arg1);
    }
    function ViewStructureCautionContactsdetails(e) {
        var arg1 = e.currentTarget.dataset.arg1;
        ViewStructureCautionContacts(arg1);
    }
    function DeleteCautionWarningdetails(e) {
        var arg1 = e.currentTarget.dataset.arg1;
        DeleteCautionWarning(e,arg1);
    }
    function addingCausion(e) {
        var arg1 = e.currentTarget.dataset.arg1;
        addCausion(arg1);
    }
    function openingHistory(e) {
        var arg1 = e.currentTarget.dataset.arg1;
        openHistory(arg1);
    }

    function ViewStructureCautionContacts(cautionId) {
        startAnimation();
        var randomNumber = Math.random();
        $("#generalSettingsId").load('../Structures/ViewStructureContactsList?structureId=' + @ViewBag.StructureID + '&cautionId=' + cautionId + '&sectionId=' + @ViewBag.SectionID + '&random=' + randomNumber,
            function () {
                $("#viewContactListId").show();
                stopAnimation();
            }
        );        
    }
    function ViewCaution(Cautionid) {
        var randomNumber = Math.random();
        startAnimation();
        $("#causionReport").load('../Structures/ViewCaution?structureId=' + @ViewBag.StructureID + '&cautionId=' + Cautionid + '&sectionId=' + @ViewBag.SectionID + '&random=' + randomNumber,
            function () {
                $('#cautionDetails').modal('show');
                stopAnimation();
            }
            );
        stopAnimation();
        stopAnimation();
        removescroll();
        $("#overlay").show();
        $("#dialogue").show();
    }
    var deleted_CautionName = '';
    var delete_CautionId = 0;
    var deleteCautionButton;
    function DeleteCautionWarning(deletebutton, CautionID) {
        deleteCautionButton = deletebutton;
        deleted_CautionName = deletebutton.name;
        delete_CautionId = deletebutton.id;
        var Msg = "Do you want to delete '" + "" + "'" + deleted_CautionName + "'" + "" + "' ?";
        ShowWarningPopup(Msg, "DeleteCaution");
    }
    function DeleteCaution() {
        var params = '{"cautionId":"' + delete_CautionId + '"}';
        $.ajax({
            async: false,
            type: "POST",
            url: '@Url.Action("DeleteCaution", "Structures")',
            dataType: "json",
            //contentType: "application/json; charset=utf-8",
            data: params,
            processdata: true,
            beforeSend: function () {
                startAnimation();
            },
            success: function (result) {
                $('#WarningPopup').modal('hide');
                stopAnimation();
                if (result == 'true') {
                    $('#pop-warning').hide();
                    var msg ='"'+ deleted_CautionName +'"' +"deleted successfully";
                    ShowSuccessModalPopup(msg, "ViewCausionList");
                }
                else if (result == 'expire') {
                    window.location.href = "../Account/Login";
                }
            },
            error: function (result) {
                stopAnimation();
                ViewCausionList();
            }
        });
    }
    function ViewCausionList() {
        CloseSuccessModalPopup();
        startAnimation();
        $("#generalSettingsId").empty();
        var randomNumber = Math.random();

        $("#generalSettingsId").load('../Structures/ReviewCautionsList?structureId=' + '@ViewBag.StructureId' + "&sectionId=" + @ViewBag.sectionId + '&random=' + randomNumber,
            function () {
                $('#manageCautionId').show();
                stopAnimation();
            }
        );
    };
    function openHistory(e) {
        var randomNumber = Math.random();
        startAnimation();
        $("#generalSettingsId").load('../Structures/StructureCautionHistory?structureId=' + @ViewBag.StructureID + '&sectionId=' + @ViewBag.SectionID + '&random=' + randomNumber,
            function () {
                document.getElementById(e).style.display = 'flex';
                stopAnimation();
            });
    }
    function addCausion(e) {
        var randomNumber = Math.random();
        $("#generalSettingsId").load('../Structures/CreateCaution?structureId=' + @ViewBag.StructureID + '&cautionId=0&mode=add&sectionId=' + @ViewBag.SectionID + '&random=' + randomNumber,
            function () {
                document.getElementById(e).style.display = 'flex';
        });
    }
    function EditCaution(cautionId) {
        startAnimation();
        var randomNumber = Math.random();
        $("#generalSettingsId").load('../Structures/CreateCaution?StructureID=' + @ViewBag.StructureID + '&CautionId=' + cautionId + '&mode=edit&SectionId=' + @ViewBag.SectionID + '&random=' + randomNumber,
            function () {
                document.getElementById('newCaution').style.display = 'flex';
                stopAnimation();
            });
    }
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
                    $('#generalSettingsId').html(result);
                }
            });
            return false;
        }
    });
    
