let listContactName = [];
let DelegateAllVal = $('#hf_DelegateAll').val();
let ArrangementIdVal = $('#hf_ArrangementId').val();
let delegIdVal = $('#hf_delegId').val();
let AllowSubdelegationVal = $('#hf_AllowSubdelegation').val();
let RetainNotificationVal = $('#hf_RetainNotification').val();
let AcceptFailureVal = $('#hf_AcceptFailure').val();
let pDelObj = "";
let Popheight = 15;
$(document).ready(function () {
    SelectMenu(3);
    //$('body').on('click', '#btnsearchroads', searchRoadLinks);

    $("#map").mouseenter(mouseoveron);
    $("#map").mouseleave(mouseoveroff);
    $("#mapdata").mouseenter(mouseoveron);
    $("#mapdata").mouseleave(mouseoveroff);
    createContextMenu();
    $('body').on('click', '#btnmap', EnLargeMap);
    $('body').on('click', '#owned', ownedRoads);
    $('body').on('click', '#managed', managedRoads);
    $('body').on('click', '#btnback', BackRoadDelegationList);
    $('body').on('click', '#savedelegdata', saveRoadDelegDetails);
    //$("#btnmap").click(EnLargeMap);
    //$("#owned").click(ownedRoads);
    //$("#managed").click(managedRoads);
    //$("#btnback").click(BackRoadDelegationList);
    //$("#savedelegdata").click(saveRoadDelegDetails);
    $("#txtDelegatorOrganisation").autocomplete({

        source: function (request, response) {
            $.ajax({
                url: '../RoadDelegation/ListOrganisation',
                dataType: "json",
                data: {
                    SearchString: request.term
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
            $('#txtDelegatorOrganisation').val(ui.item.label); // display the selected text
            $('#hdnFromOrgId').val(ui.item.value); // save selected id to input
            road_from_map();
            $('#movement-map').show();
            $('#txtLinkSearch').val('');
            $('#inboxCount').show();


            return false;
        },
        focus: function (event, ui) {
            /*$("#txtDelegatorOrganisation").val(ui.item.label);*/
            return false;
        }
    });
    $("#txtDelegateOrganisation").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '../RoadDelegation/ListOrganisation',
                dataType: "json",
                data: {
                    SearchString: request.term
                },
                success: function (data) {
                    if (data.length > 0) {
                        data.forEach((element) => {
                            let innerArray = [];
                            innerArray.push(element.OrganisationId);
                            innerArray.push(element.ContactName);
                            listContactName.push(innerArray);
                        });
                    }
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
            $('#txtDelegateOrganisation').val(ui.item.label); // display the selected text
            $('#hdnToOrgId').val(ui.item.value); // save selected id to input
            listContactName.forEach((element) => {
                if (ui.item.value == element[0]) {
                    $('#toContactName').val(element[1]);

                }
            });



            return false;
        },
        focus: function (event, ui) {
            /*$("#txtDelegateOrganisation").val(ui.item.label);*/
            return false;
        }
    });
    if ($('#hf_mode').val() == "Create") {
        $('#movement-map').hide();
        $('#inboxCount').hide();

    }
    if ($('#hf_mode').val() == "View") {
        $('#savedelegdata').hide();
    }
    if ($('#hf_mode').val() == "Edit" && (ArrangementIdVal != 0)) {
        $('#savedelegdata').show();
    }
    if ($('#DelegationId').val() > 0) {

        if (AllowSubdelegationVal == 1)
            $("#sub_delegation").prop("checked", true);

        else
            $('#sub_delegation').prop('checked', false)
        if (RetainNotificationVal == 1)
            $("#reNotify").prop("checked", true);

        else
            $('#reNotify').prop('checked', false)
        if (AcceptFailureVal == 1)
            $('#rdoYes').prop('checked', true)

        else if (AcceptFailureVal == 0)
            ($('#rdoNo').prop('checked', true))

    }
    if (delegIdVal == 0) {
        $("#map").html('');
        $("#map").load('../A2BPlanning/A2BLeftPanel?routeID = 0', function () {
            loadmap('DELEGATION_VIEWANDEDIT');

        });
    }
    else {
        road_from_map();
    }
    $('#txtLinkSearch').val('');
});
$('body').on('keypress', '#txtLinkSearch', function (e) {
    e.preventDefault();
    RDhandleSearch(e, this);
});
$('body').on('click', '#search_toorgContact', function (e) {
    e.preventDefault();
    let id = $(this).attr('id');
    ContactSummary(id);
});
$('body').on('click', '#search_fromorgContact', function (e) {
    e.preventDefault();
    let id = $(this).attr('id');
    ContactSummary(id);
});
$("#delAll").change(function () {

    let fromId = $('#hdnFromOrgId').val();
    if ($('#hdnFromOrgId').val() != 0) {
        if ($("#delAll").is(":checked") == true) {
            $.ajax({
                url: '../RoadDelegation/IsDelegationAllowed',
                type: 'POST',
                datatype: 'json',
                async: true,
                data: { orgId: fromId },
                success: function (value) {

                    if (value.val == true) {
                        $('#movement-map').hide();
                        $('#inboxCount').hide();

                    }
                    else {
                        ShowWarningPopup('Sorry,please select roads from map', "CloseWarningPopup");

                        $('#delAll').prop('checked', false)
                    }

                },
                error: function () {
                },
                complete: function () {

                }
            });
        }
        else {
            $('#movement-map').show();
            $('#inboxCount').show();
            return;
        }


    }
    else {
        $('#delAll').prop('checked', false); // Unchecks it
        ShowWarningPopup('Please select delegator organisation before proceeding for road selection.', "CloseWarningPopup");
    }
});
$("#txtDelegateOrganisation").blur(function () {

    if ($("#hdnTriggerflag").val() == 0) {

        $("#hdnTriggerflag").val(1);
    }
});
function ownedRoads() {
    if ($("#owned").is(":checked")) {
        let organisationId = $('#hdnFromOrgId').val();
        ShowDelegationOwnedRoads(organisationId);
    }
    else {
        DeleteDelegationOwnedRoads();
    }
}
function searchRoadLinks() {
    let linkid = $.trim($('#txtLinkSearch').val());
    searchLinks(linkid);
}
function managedRoads() {
    if ($("#managed").is(":checked")) {
        let organisationId = $('#hdnFromOrgId').val();
        ShowDelegationManagedRoads(organisationId);
    }
    else {
        DeleteDelegationManagedRoads();
    }
}
function RDhandleSearch(e, obj) {
    if (e.keyCode === 13) {
        var text = $.trim($(obj).val());
        searchLinks(text);

        document.getElementById("clearsearchsegments").style.display = 'block';
    }
}

function road_from_map() {
    //condition to verify whether delegator organisation is selected or not
    if ($('#hdnFromOrgId').val() != 0) {

        if ($('#mapViewFlag').val() == "true" && ($('#Delegatororg').val() == $('#txtDelegatorOrganisation').val())) {
            $("#Map_deleg").show();
            mapResize();
            $('#CreateViewDiv').hide();
            $("#pageheader").hide();
            $('#leftDiv').show();
            $('#searchLink').show();
        }
        else {
            $('#mapViewFlag').val("true");
            if ($('#mode').val() == "View" || $('#mode').val() == "Edit") {
                let DelegArrangId = $('#DelegationId').val();
                let linkInfo = null;
                $.ajax({
                    async: false,
                    type: "POST",
                    url: '../RoadDelegation/GetRoadDelegationDetails',
                    dataType: "json",
                    //contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ arrangementId: DelegArrangId, fetchFlag: 0 }),
                    processdata: true,
                    success: function (result) {
                        linkInfo = result;
                        if (linkInfo.length > 0) {
                            $('#CreateViewDiv').hide();
                            showDelegationOnMap(linkInfo);
                        }
                        else {
                            location.reload();
                        }
                    },
                    error: function (result) {
                    }
                });
            }
            else {
                $('#CreateViewDiv').hide();
                showDelegationOnMap();
            }
        }
    }
    else {
        showWarningPopDialog('Please select delegator organisation before proceeding for road selection.', 'Ok', '', 'WarningCancelBtn', '', 1, 'warning')
    }
}

// showing user-setting inside vertical menu
function showuserinfo() {
    if (document.getElementById('user-info').style.display !== "none") {
        document.getElementById('user-info').style.display = "none"
    }
    else {
        document.getElementById('user-info').style.display = "block";
        document.getElementsById('userdetails').style.overFlow = "scroll";
    }
}

function BackRoadDelegationList() {

    if ($('#originpage').val() == 'SOA')
        window.location.href = '../Structures/ShowRoadDelegation';
    else
        window.location.href = '../RoadDelegation/GetRoadDelegationList';

};
function SelectByLinkRD() {
    if (objifxStpMap.eventList['DEACTIVATECONTROL'] != undefined && typeof (objifxStpMap.eventList['DEACTIVATECONTROL']) === "function") {
        objifxStpMap.eventList['DEACTIVATECONTROL'](2, "ROADDELEGATION");
    }
    btnSelectByLink.activate();
    objifxstpmap.createClickControl();
}
function showDelegationOnMap(linkInfo) {
    let organisationId = $('#hdnFromOrgId').val();
    $("#map").html('');
    $("#map").load('../Routes/A2BPlanning?routeID = 0', function () {

        if ($('#mode').val() == "View") {
            loadmap('DELEGATION_VIEWONLY');
            if (linkInfo != null && linkInfo != undefined) {
                var linkIndex;
                if (linkInfo.length == 1) {
                    linkIndex = 0;
                }
                else {
                    linkIndex = Math.round(linkInfo.length / 2);
                }
                zoomInToDelegRoad(linkInfo[linkIndex], delegIdVal);
                
                ShowDelegationManagedRoads(organisationId);
                ShowDelegationOwnedRoads(organisationId);
                
            }
            else {
                showNotification('No data available.');
            }
        }
        else if ($('#mode').val() == "Edit") {
            loadmap('DELEGATION_VIEWANDEDIT');
            if (linkInfo != null && linkInfo != undefined) {
                addToDelegatedRoadLinks(linkInfo);
                var linkIndex;
                if (linkInfo.length == 1) {
                    linkIndex = 0;
                }
                else {
                    linkIndex = Math.round(linkInfo.length / 2);
                }
                //showOrganisationRoads(organisationId);
                zoomInToDelegRoad(linkInfo[linkIndex], delegIdVal, function () {

                });
                if (document.getElementById('managed').checked)
                    ShowDelegationManagedRoads(organisationId);
                if (document.getElementById('owned').checked) {
                    ShowDelegationOwnedRoads(organisationId);
                }

            }
            else {
                showNotification('No data available.');
            }
        }
        else {
            loadmap('DELEGATION_VIEWANDEDIT');
            showAllRoads(true);

        }
        showOrganisationRoads(organisationId);
        CheckSessionTimeOut();
        Map_size_fit();
        mapResize();
    });
}
function saveRoadDelegDetails() {
    if ($('#sub_delegation').attr('checked'))
        $('#allowSubdelegation').val(1);
    else
        $('#allowSubdelegation').val(0);

    if ($("#reNotify").is(":checked") == true)
        $('#retainNotification').val(1);
    else
        $('#retainNotification').val(0);

    if ($("#delAll").is(":checked") == true)
        $('#delegateAll').val(1);
    else
        $('#delegateAll').val(0);

    if ($('#rdoYes').attr('checked'))
        $('#AcceptFailure').val(1);
    else if ($('#rdoNo').attr('checked'))
        $('#AcceptFailure').val(0);
    let val = validateFlds();
    if (val == true) {
        startAnimation("");
        getDelegationInformation(function (delObj) {
            let fromId = $('#hdnFromOrgId').val();
            if ($('#delAll').is(':checked')) {
                $.ajax({
                    url: '../../RoadDelegation/CheckPartialDelegation',
                    type: 'POST',
                    datatype: 'json',
                    async: true,
                    data: { orgId: fromId },
                    success: function (result) {
                        if (result.delegateAll == 1) {
                            saveDelegDetails(delObj);
                        }
                        else {
                            pDelObj = delObj;
                            let message = getHtmlMessage(result.delegDetails);
                            if (Popheight == 15) {
                                saveDelegDetails(pDelObj);
                            }
                            else {
                                ShowWarningPopup(message, 'saveDelegDetails', '', pDelObj);
                                document.getElementById("div_WarningMessage").innerHTML = message;
                                $("#WarningContent").css("height", Popheight + "rem");
                                stopAnimation();
                            }
                        }
                    },

                });
            }
            else {
                saveDelegDetails(delObj);
            }
        });

    }
    else
        return;
}
function getHtmlMessage(delegDetails) {
    Popheight = 15;
    let message = '<div style="text-align: left;padding-left: 1rem;padding-bottom: 0rem;" class="edit-normal">Active delegations are</div>';
    let tr = '<ul style="text-align: initial; margin-bottom: -3rem;padding-bottom:2rem;">';
    for (let i = 0; i < delegDetails.length; i++) {
        tr += '<li class="edit-normal" style="padding:0rem;">' + delegDetails[i].ArrangementName + '</li>';
        Popheight = Popheight + 2;
    }
    tr += '</ul>';

    let htmlmessage = "Following partial delegation(s) are already exists for the delegator organisation and these will be deleted. Do you want to proceed?" + message + tr;
    return htmlmessage;

}
function validateFlds() {
    let flag = true;
    if ($('#arrangName').val() == '') {
        $('#validarranName').show();
        flag = false;
        return;
    }
    else {
        $('#validarranName').hide();
        flag = true;
    }

    if ($('#txtDelegatorOrganisation').val() == '') {
        $('#fromvalidorganisation').show();
        flag = false;
        return;
    }
    else
        flag = true;


    if ($('#txtDelegateOrganisation').val() == '') {
        $('#tovalidorganisation').show();
        flag = false;
        return;
    }
    else
        flag = true;
    return flag;
}
function getDelegationInformation(callback) {
    let delObj = {
        arrangementId: $('#DelegationId').val(),
        ArrangementName: $('#arrangName').val(),
        fromOrgId: $('#hdnFromOrgId').val(),
        fromOrgName: $('#txtDelegatorOrganisation').val(),
        fromContactId: $('#fromContactId').val(),
        toOrgId: $('#hdnToOrgId').val(),
        toOrgName: $('#txtDelegateOrganisation').val(),
        toContactId: $('#toContactId').val(),
        retainNotification: $('#retainNotification').val(),
        allowSubdelegation: $('#allowSubdelegation').val(),
        acceptFailure: $('#AcceptFailure').val(),
        delegateAll: $('#delegateAll').val(),
        linkIdInfo: []
    };

    if ($('#mapViewFlag').val() == "false" && $('#mode').val() == "Edit") {
        getDelegLinkInfo(function (data) {
            delObj.linkIdInfo = data;
            callback(delObj);
        });
    }
    else {

        if ($("#delAll").is(":checked") == false)
            delObj.linkIdInfo = getDelegLinkInfoList();
        callback(delObj);
    }
}
function saveDelegDetails(delObj) {

    startAnimation("");
    $('#WarningPopup').modal('hide');
    if ($('#mode').val() == "Edit") {
        var url = '../../RoadDelegation/UpdateRoadDelegation';
        var successMsg = 'Road delegation updated successfully';
        var failureMsg = 'Updating failed. Please retry';
        var errorMsg = 'Updating failed. Please retry';
    }
    else {
        var url = '../../RoadDelegation/SaveRoadDelegation';
        var successMsg = 'Road delegation saved successfully';
        var failureMsg = 'Road delegation creation failed';
        var errorMsg = 'Saving failed. Please retry';
    }

    let newDelegObj = delObj;
    let linkIdInfo = delObj.linkIdInfo;
    delete newDelegObj.linkIdInfo;
    //condition to save when delegate all is selected
    if ($('#delegateAll').val() == 1) {

        $("#WarningContent").css("height", "15rem");

        $.ajax({
            url: url,
            type: 'POST',
            datatype: "json",
            data: JSON.stringify({ postFlag: -2, roadDelegationObject: newDelegObj, roadLinkInfo: null, len: -1 }),
            beforeSend: function () {
                startAnimation("");
            },
            success: function (result) {

                if (result.result) {
                    startAnimation();
                    if (result.value == true) {
                        stopAnimation();
                        ShowSuccessModalPopup(successMsg, "BackRoadDelegationList");
                        $("body").on('click', '.close', function () { window['BackRoadDelegationList'](); })
                        $('#SuccessPopupAction').modal({ backdrop: 'static', keyboard: false });
                    }
                    else {
                        ShowWarningPopup(successMsg, "CloseWarningPopup");
                    }
                }
            },
            error: function (xhr, error, status) {
                stopAnimation();
            },
            complete: function () {
                stopAnimation();
            }
        });
    }
    else {
        newDelegObj.LinkInfoList = linkIdInfo;
        var compressedRoutePart = pako.gzip(JSON.stringify({ RoadDelegationData:newDelegObj }));
        var roadDelegationData = btoa(
            new Uint8Array(compressedRoutePart)
                .reduce((data, byte) => data + String.fromCharCode(byte), '')
        );
        $.ajax({
            url: url,
            type: 'POST',
            datatype: "json",
            data: JSON.stringify({ roadDelegationData: roadDelegationData }),
            beforeSend: function () {
                startAnimation("");
            },
            success: function (result) {
                if (result.result) {

                    if (result.value == true) {
                        ShowSuccessModalPopup(successMsg, "BackRoadDelegationList");

                        $("body").on('click', '.close', function () { window['BackRoadDelegationList'](); });
                        stopAnimation();
                    }
                    else {
                        ShowErrorPopup(failureMsg);
                    }
                }

            },
            error: function (xhr, error, status) {
                ShowErrorPopup(errorMsg);
                stopAnimation();
            },
            complete: function () {
                stopAnimation();
            }
        });
    }
}
function EnLargeMap() {
    if ($("#Minimzeicon").is(":visible")) { //mode changing to full screen
        $("#navbar").hide();
        $("#movement-details").hide();
        $("#bottom").hide();
        $("#checkbox").hide();
        $("#soa-portal-map").hide();
        $("#Minimzeicon").hide();
        $("#footerdiv").hide();
        $("#MaxmizeIocn").show();
        $("#banner-container").addClass("bannercfulls");
        $("#container-sub").addClass("containerfulls");
        $("#helpdeskDelegation").addClass("helpdeskfulls");
        $("#movement-map").addClass("movementfulls");
        $("#mvpmap").addClass("mvpmapfulls");
        $("#map").addClass("mapfulls");
        $("#OpenLayers_Control_Panel_200").addClass("horizontalMapRoad");
        $("#btnmap").addClass("iconfulls");
        $("#helpdeskDelegation").removeClass("row");
        $("#banner-container").removeClass("container-fluid");
        $("#movement-map").removeClass("col-lg-6 col-md-12 col-sm-12 col-xs-12");
        mapResize();

    }
    else {
        //mode changes to minimze
        $("#navbar").show();
        $("#Minimzeicon").show();
        $("#MaxmizeIocn").hide();
        $("#movement-details").show();
        $("#bottom").show();
        $("#checkbox").show();
        $("#soa-portal-map").show();
        $("#map").removeClass("mapfulls");
        $("#container-sub").removeClass("containerfulls");
        $("#helpdeskDelegation").removeClass("helpdeskfulls");
        $("#movement-map").removeClass("movementfulls");
        $("#mvpmap").removeClass("mvpmapfulls");
        $("#OpenLayers_Control_Panel_200").removeClass("horizontalMapRoad");
        $("#banner-container").removeClass("bannercfulls");
        $("#helpdeskDelegation").addClass("row");
        $("#banner-container").addClass("container-fluid");
        $("#movement-map").addClass("col-lg-6 col-md-12 col-sm-12 col-xs-12");
        $("#overlay_load").addClass("col-lg-6 col-md-12 col-sm-12 col-xs-12");
        $("#btnmap").removeClass("iconfulls");
        $("#footerdiv").show();
        mapResize();

    }
}
/* When the openFullscreen() function is executed, open the map in fullscreen.
Note that we must include prefixes for different browsers, as they don't support the requestFullscreen property yet */
function fullscreenMode() {
    /* Get the element you want displayed in fullscreen mode*/
    let elem = document.getElementById("mvpmap");
    if (elem.requestFullscreen) {
        elem.requestFullscreen();
    } else if (elem.webkitRequestFullscreen) { /* Safari */
        elem.webkitRequestFullscreen();
    } else if (elem.msRequestFullscreen) { /* IE11 */
        elem.msRequestFullscreen();
    }
}
/* Close fullscreen */
function closeFullscreen() {
    if (document.exitFullscreen) {
        document.exitFullscreen();
    } else if (document.webkitExitFullscreen) { /* Safari */
        document.webkitExitFullscreen();
    } else if (document.msExitFullscreen) { /* IE11 */
        document.msExitFullscreen();
    }
}
function mouseoveron() {
    $('body').css('overflow-y', 'hidden');

}
function mouseoveroff() {
    $('body').css('overflow-y', 'auto');

}
