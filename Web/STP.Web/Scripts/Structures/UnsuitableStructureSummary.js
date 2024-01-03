var StructureId = $('#hf_StructureId').val();
var sectionType = $('#hf_sectionType').val();
var sectionType1 = sectionType;
var sectionIdVal = $('#hf_section_Id').val();
var count = $('#hf_Count').val();
var objListStructureGeneralDetails = $('#hf_objListStructureGeneralDetails').val();

function UnsuitableStructureSummaryInit() {
    StructureId = $('#hf_StructureId').val();
    sectionType = $('#hf_sectionType').val();
    sectionType1 = sectionType;
    sectionIdVal = $('#hf_section_Id').val();
    count = $('#hf_Count').val();
    objListStructureGeneralDetails = $('#hf_objListStructureGeneralDetails').val();
    $("input:radio[id='" + sectionIdVal + "']").trigger("click");
    Resize_PopUp(650);
    addscroll();
}

$(document).ready(function () {
    //UnsuitableStructureSummaryInit();
    var ownerCount = count;
    $('body').on('click', '.unsuitable-structure-summary #span-close', function (e) {
        $('#overlay').hide();
        addscroll();
    });
    $('body').on('click', '.unsuitable-structure-summary .CloseStructure', function (e) {
        CloseUnsuitableStructureDetails(this);
    });
    $('body').on('click', '.unsuitable-structure-summary .DisplayContact', function (e) {
        DisplayContactUnsuitableStructureSummary(this);
    });
    $('body').on('click', '.unsuitable-structure-summary .rd_showSectionDetail', function (e) {
        ShowSectionDetailsFn(this);
    });

});
function ShowContactDetailsUnsuitableStructureSummary(contactId) {
    //$("#overlay").show();
    // $('.loading').show();
    var ownerCount = count;
    removescroll();
    $("#StructureOwnerContact").load('../Application/ViewContactDetails?ContactId=' + contactId + "&structOwner=" + 'otherOrg' + "&ownerCnt=" + ownerCount, function () {
        $("#overlay").show();
        $('#structdetail').hide();
        $('#structdetailHead').hide();
        $('#div_review_summary').hide();
        $("#StructureOwnerContact").show();
        $('.loading').hide();
    });

}
function ClosePopupRSummary() {
    $('#overlay').hide();
    addscroll();
    resetdialogue();
}
function showSectionDetails(StructureId, sectionId, flag, vehicleName) {
    $('#affctdMessage_tr').hide();
    $('#affctdMessage_td').hide();
    if (flag != 0) {
        var affect_msg = getAffectedMessage(flag, vehicleName);
        $('#affctdMessage_tr').show();
        $('#affctdMessage_td').show();
        $('#affctdMessage_td').html(affect_msg);
    }
    $("#hdnSectionID").val(sectionId);
    var sectionType = $('#' + sectionId).parent().find('input:hidden:first').val();
    $('#divStructureSectionDetails').load('../Structures/ReviewStructureSectionImposedConstraints',
        { structureId: StructureId, sectionId: sectionId, sectionType: sectionType },
        function () {
            if (objListStructureGeneralDetails != null) {
                $('#divStructureSectionDetails').show();
            }
        });
}
function getAffectedMessage(affectFlag, vehicleName) {
    var msg = "";
    if (affectFlag == 1) {
        msg = "Vehicle ( " + vehicleName + " ) gross weight exceeds structure's gross weight, so this structure is unsuitable";
    }
    else if (affectFlag == 2) {
        msg = "Vehicle ( " + vehicleName + " ) gross weight exceeds structure's signed gross weight, so this structure is unsuitable";
    }
    else if (affectFlag == 3) {
        msg = "Vehicle ( " + vehicleName + " ) maximum axle weight exceeds structure's axle weight, so this structure is unsuitable";
    }
    else if (affectFlag == 4) {
        msg = "Vehicle ( " + vehicleName + " ) maximum axle weight exceeds structure's signed axle weight, so this structure is unsuitable";
    }
    else if (affectFlag == 5) {
        msg = "Vehicle ( " + vehicleName + " ) height exceeds structure's height, so this structure is unsuitable";
    }
    else if (affectFlag == 6) {
        msg = "Vehicle ( " + vehicleName + " ) height exceeds structure's signed height, so this structure is unsuitable";
    }
    else if (affectFlag == 7) {
        msg = "Vehicle ( " + vehicleName + " ) width exceeds structure's width, so this structure is unsuitable";
    }
    else if (affectFlag == 8) {
        msg = "Vehicle ( " + vehicleName + " ) width exceeds structure's signed width, so this structure is unsuitable";
    }
    else if (affectFlag == 9) {
        msg = "Vehicle (" + vehicleName + " ) length exceeds structure's length, so this structure is unsuitable";
    }
    else if (affectFlag == 10) {
        msg = "Vehicle ( " + vehicleName + " ) length exceeds structure's signed length, so this structure is unsuitable";
    }
    else if (affectFlag == 11) {
        msg = "Vehicle ( " + vehicleName + " ) axle weight exceeds structure’s maximum weight over distance, so this structure is unsuitable";
    }
    else if (affectFlag == 12) {
        msg = "Vehicle ( " + vehicleName + " ) gross weight doesn't satisfy SV screening, so this structure is unsuitable";
    }
    return msg;
}
function CloseUnsuitableStructureDetails() {
    $('#exampleModalCenter22').hide();
    $('#overlay').hide();
    addscroll();
    resetdialogue();
    stopAnimation();
}
function DisplayContactDetails(ContactId) {
    startAnimation();
    $('#exampleModalCenter22').hide();
    $("#dialogue1").load('../Application/ViewContactDetails?ContactId=' + ContactId + "", function () {
        stopAnimation();
        removescroll();
        $('#contactDetails').modal('hide');
        $('#contactDetailsForMap').show();
        $("#dialogue1").show();
        $("#overlay").show();
    });

}
function closeContactPopup() {
    $('#contactDetailsForMap').hide();
    $("#dialogue1").hide();
    $('#exampleModalCenter22').show();
}
function DisplayContactUnsuitableStructureSummary(e) {
    var chainContactId = $(e).attr("chaincontactid");
    DisplayContactDetails(chainContactId);
}
function ShowSectionDetailsFn(e) {
    var structureId = $(e).attr("structureid");
    var id = $(e).attr("id");
    var affectFlag = $(e).attr("affectflag");
    var vehicleName = $(e).attr("vehiclename");
    showSectionDetails(structureId, id, affectFlag, vehicleName);
}
