var structid = $(".review-caution-list #hf_structureid").val();
var sectionid = $(".review-caution-list #hf_sectionid").val();
var sectionids = $(".review-caution-list #hf_sectionids").val();
function ReviewCautionsListInit() {
    structid = $(".review-caution-list #hf_structureid").val();
    sectionid = $(".review-caution-list #hf_sectionid").val();
    sectionids = $(".review-caution-list #hf_sectionids").val();
}
$(document).ready(function () {
    $('body').on('click', '.review-caution-list .viewcaution', function (e) {
        e.preventDefault();
        ViewCautiondetails(this);
    });
    $('body').on('click', '.review-caution-list .viewstructurecaution', function (e) {
        e.preventDefault();
        ViewStructureCautionContactsdetails(this);
    });
    $('body').on('click', '.review-caution-list .editcaution', function (e) {
        e.preventDefault();
        EditCautiondetails(this);
    });
    $('body').on('click', '.review-caution-list .deletecaution', function (e) {
        e.preventDefault();
        DeleteCautionWarningdetails(this);
    });
    $('body').on('click', '.review-caution-list .addcausion', function (e) {
        e.preventDefault();
        addingCausion(this);
    });
    $('body').on('click', '.review-caution-list .openhistory', function (e) {
        e.preventDefault();
        openingHistory(this);
    });
    $('body').on('click', '.review-caution-list #reviewCausionPaginator a', function (e) {
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
                    $('#manageCautionId').show();
                },
                error(err) {
                    console.error(err);
                }
            });
            return false;
        }
    });
});
function ViewCautiondetails(e) {
    var arg1 = $(e).attr("arg1");
    ViewCaution(arg1);
}
function EditCautiondetails(e) {
    var arg1 = $(e).attr("arg1");
    EditCaution(arg1);
}
function ViewStructureCautionContactsdetails(e) {
    var arg1 = $(e).attr("arg1");
    ViewStructureCautionContacts(arg1);
}
function DeleteCautionWarningdetails(e) {
    var arg1 = $(e).attr("arg1");
    DeleteCautionWarning(e, arg1);
}
function addingCausion(e) {
    var arg1 = $(e).attr("arg1");
    addCausion(arg1);
}
function openingHistory(e) {
    var arg1 = $(e).attr("arg1");
    openHistory(arg1);
}

function ViewStructureCautionContacts(cautionId) {
    startAnimation();
    var randomNumber = Math.random();
    $("#generalSettingsId").load('../Structures/ViewStructureContactsList?structureId=' + structid + '&cautionId=' + cautionId + '&sectionId=' + sectionid + '&random=' + randomNumber,
        function () {
            $("#viewContactListId").show();
            stopAnimation();
        }
    );
}
function ViewCaution(Cautionid) {
    var randomNumber = Math.random();
    startAnimation();
    $("#causionReport").load('../Structures/ViewCaution?structureId=' + structid + '&cautionId=' + Cautionid + '&sectionId=' + sectionid + '&random=' + randomNumber,
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
        url: '../Structures/DeleteCaution',
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
                var msg = '"' + deleted_CautionName + '"' + "deleted successfully";
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

    $("#generalSettingsId").load('../Structures/ReviewCautionsList?structureId=' + structid + "&sectionId=" + sectionids + '&random=' + randomNumber,
        function () {
            $('#manageCautionId').show();
            stopAnimation();
            ReviewCautionsListInit();
        }
    );
};
function openHistory(e) {
    var randomNumber = Math.random();
    startAnimation();
    $("#generalSettingsId").load('../Structures/StructureCautionHistory?structureId=' + structid + '&sectionId=' + sectionid + '&random=' + randomNumber,
        function () {
            $("#" + e).css("display", 'flex');
            stopAnimation();
            StructureCautionHistoryInit();
        });
}
function addCausion(e) {
    var randomNumber = Math.random();
    $("#generalSettingsId").load('../Structures/CreateCaution?structureId=' + structid + '&cautionId=0&mode=add&sectionId=' + sectionid + '&random=' + randomNumber,
        function () {
            $("#" + e).css("display", 'flex');
            CreateCautionInit();
        });
}
function EditCaution(cautionId) {
    startAnimation();
    var randomNumber = Math.random();
    $("#generalSettingsId").load('../Structures/CreateCaution?StructureID=' + structid + '&CautionId=' + cautionId + '&mode=edit&SectionId=' + sectionid + '&random=' + randomNumber,
        function () {
            $('#newCaution').css("display", 'flex');
            stopAnimation();
            CreateCautionInit();
        });
}


