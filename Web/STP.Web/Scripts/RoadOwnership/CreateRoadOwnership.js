function CreateRoadOwnershipInit() {
    $("#searchHAMac").prop("disabled", true);
    $("#searchLa").prop("disabled", true);
    $("#searchPolice").prop("disabled", true);
    Resize_PopUp(600);
    if ($('#OrgId').val() > 0) {
        $("#allRoads").show();
    }
    $('#managerName').css('background-color', '#ffffff');
    DisableFields('#managerName');
    $("#local_Authorityname").css('background-color', '#f8f8f7');
    $("#ha_Mac_Name").css('background-color', '#f8f8f7');
    $("#police_Name").css('background-color', '#f8f8f7');
    //function for esdal4
    $("#managerName").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '../RoadOwnership/ListRoadContactList',
                dataType: "json",
                data: {
                    SearchString: request.term,
                    searchFlag: 1
                },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.OrganisationName, value: item.OrganisationId };
                    }));
                },
                error: function (jqXHR, exception, errorThrown) {
                    console.log(errorThrown);
                }
            });
        },
        minLength: 2,
        select: function (event, ui) {
            // Set selection
            OrgSelect(ui.item.label, ui.item.value, 1);
            return false;
        },
        focus: function (event, ui) {
            /*$("#managerName").val(ui.item.label);*/
            return false;
        }
    });

    $("#local_Authorityname").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '../RoadOwnership/ListRoadContactList',
                dataType: "json",
                data: {
                    SearchString: request.term,
                    searchFlag: 2
                },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.OrganisationName, value: item.OrganisationId };
                    }));
                },
                error: function (jqXHR, exception, errorThrown) {
                    console.log(errorThrown);
                }
            });
        },
        minLength: 2,
        select: function (event, ui) {
            // Set selection
            OrgSelect(ui.item.label, ui.item.value, 2);
            return false;
        },
        focus: function (event, ui) {
            /* $("#local_Authorityname").val(ui.item.label);*/
            return false;
        }
    });

    $("#ha_Mac_Name").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '../RoadOwnership/ListRoadContactList',
                dataType: "json",
                data: {
                    SearchString: request.term,
                    searchFlag: 3
                },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.OrganisationName, value: item.OrganisationId };
                    }));
                },
                error: function (jqXHR, exception, errorThrown) {
                    console.log(errorThrown);
                }
            });
        },
        minLength: 2,
        select: function (event, ui) {
            // Set selection
            OrgSelect(ui.item.label, ui.item.value, 3);
            return false;
        },
        focus: function (event, ui) {
            /* $("#ha_Mac_Name").val(ui.item.label);*/
            return false;
        }
    });

    $("#police_Name").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '../RoadOwnership/ListRoadContactList',
                dataType: "json",
                data: {
                    SearchString: request.term,
                    searchFlag: 4
                },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.OrganisationName, value: item.OrganisationId };
                    }));
                },
                error: function (jqXHR, exception, errorThrown) {
                    console.log(errorThrown);
                }
            });
        },
        minLength: 2,
        select: function (event, ui) {
            // Set selection
            OrgSelect(ui.item.label, ui.item.value, 4);
            return false;
        },
        focus: function (event, ui) {
            /*$("#police_Name").val(ui.item.label);*/
            return false;
        }
    });



}
$(document).ready(function () {
    $('body').on('click', '#clearManager', function (e) {
           clearManagerName();
    });
    $('body').on('click', '#clearLA', function (e) {
           clearLocal_AuthorityName();
    });

    $('body').on('click', '#clearHA', function (e) {
            clearHa_Mac_Name();
    });
    $('body').on('click', '#clearPolice', function (e) {
   
        clearPolice_Name();
    });
    $('body').on('click', '#saveownerdata', function (e) {
          saveRoadOwnerShipBtnClick();
    });
    $('body').on('click', '#GoBackBtn', function (e) {
          CancelBtnClick();
    });
    $('body').on('click', '#span-Edit_help', function (e) {
         EditHelp_popup();
    });
    $('body').on('change', "#rdoManager,#rdoLA, #rdoHaMac, #rdoPolice", function (e) {
        clearFeilds();
        var id = this.id;
        switch (id) {
            case "rdoManager":
                $("#searchManager").prop("disabled", false);
                $('#managerName').css('background-color', '#ffffff');
                DisableFields('#managerName');
                break;
            case "rdoLA":
                $("#searchLa").prop("disabled", false);
                $('#local_Authorityname').css('background-color', '#ffffff');
                DisableFields('#local_Authorityname');
                break;
            case "rdoHaMac":
                $("#searchHAMac").prop("disabled", false);
                $('#ha_Mac_Name').css('background-color', '#ffffff');
                DisableFields('#ha_Mac_Name');
                break;
            case "rdoPolice":
                $("#searchPolice").prop("disabled", false);
                $('#police_Name').css('background-color', '#ffffff');
                DisableFields('#police_Name');
                break;
            default:
                break;

        }

    });
    $('body').on('click', '#span-close2', function (e) {
        $('#overlay').hide();
        addscroll();
        resetdialogue();
    });
    $('body').on('click', '#managerName', function (e) {
        $('#validManager').hide();
    });
    $('body').on('click', '#local_Authorityname', function (e) {
        $('#validLA').hide();
    });
    $('body').on('click', '#ha_Mac_Name', function (e) {
        $('#validHA').hide();
    });
    $('body').on('click', '#police_Name', function (e) {
        $('#validPOLICE').hide();
    });
    $('body').on('click', '#linkIds', function (e) {
   
    });

});
function clearFeilds() {
    $("#clearManager").hide();
    $("#clearLA").hide();
    $("#clearHA").hide();
    $("#clearPolice").hide();
    $("#managerName").val("");
    $("#local_Authorityname").val("");
    $("#ha_Mac_Name").val("");
    $("#police_Name").val("");
    $("#manager_Id").val("");
    $("#la_Id").val("");
    $("#ha_Mac_Id").val("");
    $("#police_Id").val("");
    $("#searchManager").prop("disabled", true);
    $("#searchHAMac").prop("disabled", true);
    $("#searchLa").prop("disabled", true);
    $("#searchPolice").prop("disabled", true);
    $("#managerName").css('background-color', '#f8f8f7');
    $("#local_Authorityname").css('background-color', '#f8f8f7');
    $("#ha_Mac_Name").css('background-color', '#f8f8f7');
    $("#police_Name").css('background-color', '#f8f8f7');

}


function DisableFields(enableId) {
    $("#saveownerdata").hide();
    $("#managerName").prop("disabled", true);
    $("#local_Authorityname").prop("disabled", true);
    $("#ha_Mac_Name").prop("disabled", true);
    $("#police_Name").prop("disabled", true);
    $(enableId).prop("disabled", false);
}

function CancelBtnClick() {
    $('#overlay').hide();
    addscroll();
    resetdialogue();
}

function getManagerList() {
    var randomNumber = Math.random();
    $("#ContactList").load('../../RoadOwnership/ListRoadContactList?searchFlag=1' + '&random=' + randomNumber, function (responseText, textStatus, req) {
        $("#Create_RoadOwnership").hide();
        $("#header").hide();
        Resize_PopUp(800);
        $('.loading').hide();
        addscroll();
        $("#ContactList").show();
        if (textStatus == "error") {
            //location.reload();
        }
    });
}
function getLaList() {
    var randomNumber = Math.random();
    $("#ContactList").load('../../RoadOwnership/ListRoadContactList?searchFlag=2' + '&random=' + randomNumber, function (responseText, textStatus, req) {
        $("#Create_RoadOwnership").hide();
        $("#header").hide();
        Resize_PopUp(800);
        addscroll();
        $('.loading').hide();
        $("#ContactList").show();
        if (textStatus == "error") {
            //location.reload();
        }
    });
}
function getHaMacList() {
    var randomNumber = Math.random();
    $("#ContactList").load('../../RoadOwnership/ListRoadContactList?searchFlag=3' + '&random=' + randomNumber, function (responseText, textStatus, req) {
        $("#Create_RoadOwnership").hide();
        $("#header").hide();
        Resize_PopUp(800);
        addscroll();
        $('.loading').hide();
        $("#ContactList").show();
        $("#Create_RoadOwnership").hide();
        $("#header").hide();
        if (textStatus == "error") {
            //location.reload();
        }
    });
}
function getPolice() {
    var randomNumber = Math.random();
    $("#ContactList").load('../../RoadOwnership/ListRoadContactList?searchFlag=4' + '&random=' + randomNumber, function (responseText, textStatus, req) {
        $("#Create_RoadOwnership").hide();
        $("#header").hide();
        Resize_PopUp(800);
        $('.loading').hide();
        $("#ContactList").show();
        addscroll();
        $("#Create_RoadOwnership").hide();
        $("#header").hide();
        if (textStatus == "error") {
            //location.reload();
        }
    });
}

function validateFlds() {
    if ($('#hf_allFeilds').val() == 'True') {
        var flag = true;
        if ($('#manager_Id').val() == 0) {
            $('#validManager').show();
            flag = false;
            return;
        }
        else if ($('#la_Id').val() == 0) {
            $('#validLA').show();
            flag = false;
            return;
        }
        else if ($('#ha_Mac_Id').val() == 0) {
            $('#validHA').show();
            flag = false;
            return;
        }
        else if ($('#police_Id').val() == 0) {
            $('#validPOLICE').show();
            flag = false;
            return;
        }
        else {
            flag = true;
        }
        return flag;
    }
    else {
        var flag = true;
        if ($('#manager_Id').val() == 0) {
            $('#validManager').show();
            flag = false;
            return;
        }
        else {
            $('#validManager').hide();
            flag = true;
        }
    }
    return flag;
}
function createOwnerDetails() {
    roadOwnershipDetails = {
        newOwnerList: [],
        linkInfo: [],
        assignedLinkInfo: [],
        unassignedLinkInfo: [],
        newManagerDelegationDetails: []
    };

    var ownerdetailList = [];
    if ($('#manager_Id ').val() != 0)
        roadOwnershipDetails.newOwnerList.push({ ownerName: $('#managerName').val(), ownerId: $('#manager_Id ').val(), type: 1 });
    if ($('#la_Id ').val() != 0)
        roadOwnershipDetails.newOwnerList.push({ ownerName: $('#local_Authorityname').val(), ownerId: $('#la_Id ').val(), type: 2 });
    if ($('#ha_Mac_Id ').val() != 0)
        roadOwnershipDetails.newOwnerList.push({ ownerName: $('#ha_Mac_Name').val(), ownerId: $('#ha_Mac_Id ').val(), type: 3 });
    if ($('#police_Id ').val() != 0)
        roadOwnershipDetails.newOwnerList.push({ ownerName: $('#police_Name').val(), ownerId: $('#police_Id ').val(), type: 4 });

    roadOwnershipDetails.linkInfo = getOwnerLinkInfoList();
    // roadOwnershipDetails.linkInfo = [{ linkId: 100, fromLRS: 0, toLRS: 0 }, { linkId: 101, fromLRS: 0, toLRS: 0 }, { linkId: 26609998, fromLRS: 0, toLRS: 0 }, { linkId: 26609998, fromLRS: 0, toLRS: 0 }, { linkId: 26611375, fromLRS: 0, toLRS: 0 }, { linkId: 26610001, fromLRS: 0, toLRS: 0 }, { linkId: 26614345, fromLRS: 0, toLRS: 0 }, { linkId: 26854305, fromLRS: 0, toLRS: 0 }, { linkId: 26612542, fromLRS: 0, toLRS: 0 }, { linkId: 26610002, fromLRS: 0, toLRS: 0 }];
    return roadOwnershipDetails;
}

function saveRoadOwnerShipBtnClick() {
    $('#save_anim').show();
    createOwnerDetails();
    if (roadOwnershipDetails.newOwnerList.length < 4) {
        getUnassignedRoads(roadOwnershipDetails.linkInfo, function (rdLinkIdList) {

            roadOwnershipDetails.assignedLinkInfo = rdLinkIdList.result1;
            roadOwnershipDetails.unassignedLinkInfo = null;
            //roadOwnershipDetails.unassignedLinkInfo = rdLinkIdList.result2;
            //if (roadOwnershipDetails.unassignedLinkInfo != null && roadOwnershipDetails.unassignedLinkInfo.length > 0) {
            //    showWarningPopDialogBig('You have selected some unassigned links. For creating the ownership of unassigned links,you need to set all feilds. Do you want to proceed by excluding unassigned roads?', 'No', 'Yes', 'hideWarnig', 'closeWarningPopup', 1, 'info');
            //}
            //else
            //    saveRoadOwnershipStep1();
            if (roadOwnershipDetails.assignedLinkInfo.length > 0)
                saveRoadOwnershipStep1();
            else {
                CancelBtnClick();
                ShowErrorPopup('owner cannot be set for the selected roads');
            }

        });
    }
    else {
        getUnassignedRoads(roadOwnershipDetails.linkInfo, function (rdLinkIdList) {
            roadOwnershipDetails.assignedLinkInfo = rdLinkIdList.result1;
            roadOwnershipDetails.unassignedLinkInfo = rdLinkIdList.result2;
            saveRoadOwnershipStep1();
        });
    }

}

function hidePopup() {
    $('#pop-warning').hide();
    $("#dialogue").html("");
    $("#dialogue").hide();
    $("#overlay").hide();

}
function hideWarnig() {
    $('#pop-warning').hide();
}
function closeWarningPopup() {
    hideWarnig();
    roadOwnershipDetails.unassignedLinkInfo = null;
    saveRoadOwnershipStep1();
}
function closeWarningPopup2() {

    hidePopup();
    CloseWarningPopupRef();
    saveRoadOwnershipStep2();
}
function closeWarningPopup3() {
    hideWarnig();
    CloseWarningPopup();
    saveRoadOwnershipStep2();
}
function saveRoadOwnershipStep1() {
    //hide new div
    $("#Create_RoadOwnership").hide();
    $("#overlay").hide();
    //first get new owner details and type
    getRoadDelegationDetails(function (result) {
        if (result == 'no manager') {
            saveRoadOwnershipStep2();
        }
        else {
            if (result.delegDetails == null) {
                ShowErrorPopup('Failed to fetch road delegation details of new manager. Please try again');
                return;
            }
            else {
                if (result.delegDetails.length == 0) {
                    ShowWarningPopup("The new manager doesn't have any delegation arrangements, so the delegation arrangement of all selected roads will be deleted. Do you want to proceed?", 'closeWarningPopup3');
                    $('#save_anim').hide();
                    return;
                }
                else {
                    if (result.delegDetails.length == 1) {

                        roadOwnershipDetails.newManagerDelegationDetails = result.delegDetails;
                        ShowWarningPopup("Delegation arrangements of the new manager (Arrangement name: " + result.delegDetails[0].ArrangementName + " )  will be applied to all selected roads. Do you want to proceed?", 'closeWarningPopup2');

                    }
                    else {
                        roadOwnershipDetails.newManagerDelegationDetails = result.delegDetails;
                        var message = getHtmlMessage(result.delegDetails);
                        //showWarningPopDialogBig("The new manager has multiple delegation arrangements, so the delegation arrangement of all selected road will be deleted. Do you want to proceed?", 'No', 'Yes', 'WarningCancelBtn', 'closeWarningPopup3', 1, 'info');
                        ShowHtmlPopup(message, 'closeWarningPopup3');
                        $('#save_anim').hide();
                    }
                }
            }
        }

    });
}
function ShowHtmlPopup(Message, btn1Action, btn2Action = '') {
    $("#div_WarningMessage").html(Message);
    if (btn1Action != '')

        $("body").on('click', '#WarningSuccess', function () { window[btn1Action](); });
    if (btn2Action != '') {

        $("body").on('click', '#WarningFailure', function () { window[btn2Action](); });
    }
    else {

        $("body").on('click', '#WarningFailure', function () { window['CloseWarningPopupRef'](); });
    }
    $('#WarningPopup').modal({ keyboard: false, backdrop: 'static' });
    $('#WarningPopup').modal('show');
}
function getHtmlMessage(delegDetails) {
    var tr = '<ul style="text-align: initial; margin-bottom: 0rem;padding-left: 1rem;padding-bottom: 0rem;">';
    for (var i = 0; i < delegDetails.length; i++) {
        var j = i + 1;
        tr += '<li class="edit-normal" style="padding:0rem;">' + delegDetails[i].ArrangementName + '</li>';

    }
    tr += '</ul>';
    var message = '<div style="text-align: left;padding-left: 0rem;padding-bottom: 0rem;" class="edit-normal">Active delegations are</div>';
    var htmlmessage = "Delegation arrangements of selected roads will be deleted (if present), as there are multiple delegation arrangements for the new owner. You need to set the delegation arrangements for all selected roads manually. Do you want to proceed? </div>" + message + tr;
    return htmlmessage;

}
function getRoadDelegationDetails(callback) {
    if (roadOwnershipDetails.newOwnerList[0].type == 1) {
        $.ajax({
            url: '../../RoadOwnership/getRoadDelegationDetails',
            //contentType: 'application/json; charset=utf-8',
            type: 'POST',
            datatype: "json",
            data: JSON.stringify({
                roadOwnerID: roadOwnershipDetails.newOwnerList[0].ownerId
            }),
            success: function (data) {
                callback(data);
            },
            error: function (xhr, error, status) {
                callback(null);
            },
        });
    }
    else {
        var delegationDetails = "no manager";
        callback(delegationDetails);
    }

}
function getInfoList() {
    pageNo = 1;
    pageSize = 5;
    startIndex = (pageNo - 1) * pageSize;
    endIndex = (pageNo - 1) * pageSize + pageSize;
    //endIndex=(roadOwnershipDetails.assignedLinkInfo.length < 5) ? roadOwnershipDetails.assignedLinkInfo.length : 5
    var linkInfolist = roadOwnershipDetails.assignedLinkInfo.slice(startIndex, endIndex);
    return linkInfolist;
}
//show the difference and ask user for confirmation
function saveRoadOwnershipStep2() {
    $('#save_anim').show();
    var linkInfoLen = roadOwnershipDetails.assignedLinkInfo.length;
    var randomNumber = Math.random();
    var linkInfolist = getInfoList();
    $.ajax({
        url: '../../RoadOwnership/RoadOwnerReport',
        //contentType: 'application/json; charset=utf-8',
        type: 'POST',
        datatype: "json",
        data: JSON.stringify({
            assignedLinkInfoArray: linkInfolist, newOwnerList: roadOwnershipDetails.newOwnerList, assignedLinkInfoCount: linkInfoLen
        }),
        beforeSend: function () {
        },
        success: function (data) {
            if (data != null || data != "")
                resetdialogue();
            $("#dialogue").html(data);
            RoadOwnerReportInit();
            $("#overlay").show();
            $("#dialogue").show();
        },
        error: function (xhr, error, status) {

        },
        complete: function () {
            addscroll();
            $('#save_anim').hide();
        }
    });
}

function getUnassignedRoads(linkIdInfo, callback) {
    for (var i = 0; i < (Math.floor(linkIdInfo.length / 1000)) + 1; i++) {
        if (i == Math.floor(linkIdInfo.length / 1000))
            var limit = (i * 1000) + (linkIdInfo.length % 1000);
        else
            var limit = (i * 1000) + 1000;

        var tempList = linkIdInfo.slice(i * 1000, limit);
        if (tempList.length == 0)
            tempList = null;

        $.ajax({
            url: '../../RoadOwnership/getUnassignedRoads',
            type: 'POST',
            cache: true,
            //contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({
                linkInfoList: tempList, length: linkIdInfo.length
            }),
            success: function (val) {
                if (val.status == true)
                    callback(val);
            },
            error: function (xhr, error, status) {
                //showWarningPopDialog('Some error occured. try again', 'Ok', '', 'hideWarnig', '', 1, 'info');
            },
            complete: function () {
                //$('#save_anim').hide();
            },
        });
    }
}

function clearManagerName() {
    $('#managerName').val("");
    $("#clearManager").hide();
    if ($('#local_Authorityname').val() == "" && $('#ha_Mac_Name').val() == "" && $('#police_Name').val() == "") {
        $('#saveownerdata').hide();
    }
}

function clearLocal_AuthorityName() {
    $('#local_Authorityname').val("");
    $("#clearLA").hide();
    if ($('#managerName').val() == "" && $('#ha_Mac_Name').val() == "" && $('#police_Name').val() == "") {
        $('#saveownerdata').hide();
    }
}

function clearHa_Mac_Name() {
    $('#ha_Mac_Name').val("");
    $("#clearHA").hide();
    if ($('#managerName').val() == "" && $('#local_Authorityname').val() == "" && $('#police_Name').val() == "") {
        $('#saveownerdata').hide();
    }
}

function clearPolice_Name() {
    $('#police_Name').val("");
    $("#clearPolice").hide();
    if ($('#managerName').val() == "" && $('#local_Authorityname').val() == "" && $('#ha_Mac_Name').val() == "") {
        $('#saveownerdata').hide();
    }
}


function OrgSelect(orgName, orgid, flag) {
    $('#hdnorganisationId').val(orgid);
    var Flag = flag;
    switch (Flag) {
        case "0":
            $('#pageheader').find('h3').text(orgName);
            $('#pageheader').css({ height: "25px" });
            $('#clearOrgData').show();
            GetOrgLinkIds(orgName, orgid);
            break;
        case 1:
            setManagerDetails(orgName, orgid);
            break;
        case 2:
            setLADetails(orgName, orgid);
            break;
        case 3:
            setHaMacDetails(orgName, orgid);
            break;
        case 4:
            setPoliceDetails(orgName, orgid);
            break;
        default:
            break;

    }
}
function setManagerDetails(orgName, orgid) {
    $('#managerName').val(orgName);
    $('#manager_Id').val(orgid);
    $("#ContactList").hide();
    $("#saveownerdata").show();
    $("#clearManager").show();


}

function setHaMacDetails(orgName, orgid) {
    $('#ha_Mac_Name').val(orgName);
    $('#ha_Mac_Id').val(orgid);
    $("#ContactList").hide();
    $("#saveownerdata").show();
    $("#clearHA").show();

}

function setPoliceDetails(orgName, orgid) {
    $('#police_Name').val(orgName);
    $('#police_Id').val(orgid);
    $("#ContactList").hide();
    $("#saveownerdata").show();
    $("#clearPolice").show();

}

function setLADetails(orgName, orgid) {
    $('#local_Authorityname').val(orgName);
    $('#la_Id').val(orgid);
    $("#ContactList").hide();
    $("#saveownerdata").show();
    $("#clearLA").show();

}

