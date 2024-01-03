let selectedVal = $('#pageSizeVal').val();
$('#pageSizeSelect').val(selectedVal);
$(document).ready(function () {
    searchStringOnKeyPress();
    $('body').on('click', '.SelectOrgnaisation', function (e) {
        e.preventDefault();
        SelectOrgnaisationFront(this);
    });

    $('body').on('click', '.btn-show-delegate-details', function (e) {
        showDelegDetails();
    });
});


function searchStringOnKeyPress() {
    $("#SearchString").focus();
    $("#SearchString").live("keypress", function (event) {
        if ($("#LastOpenPage").val() == '' || $("#LastOpenPage").val() == 'organisation') {
            if (event.which == 13) {
                SearchOrganisation();
            }
        }
    });
}
function SelectOrgnaisationFront(e) {
    let orgName = $(e).attr("dataorganisationname");
    let orgid = $(e).attr("dataorganisationid");
    SelectOrgnaisation(orgName, orgid);
}
function SelectOrgnaisation(orgName, orgid) {
    GetOrgLinkIds(orgid);
}
function GetOrgLinkIds(orgid) {
    let url = '../RoadOwnership/GetOrgLinkID';
    $.ajax({
        url: url,
        type: 'POST',
        data: { orgid: orgid },
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
            showLinkIdsOnMap(result.linkIdList);
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function showLinkIdsOnMap(linkInfo) {
    let index = 0;
    while (index < linkInfo.length) {
        if (linkInfo[index] == 0) {
            linkInfo.splice(index, 1);
        }
        index++;
    }
    let items = [];
    for (const element of linkInfo) {
        var linkobj = {};
        items.push({ linkId: element });
        linkobj["linkId"] = element;
        items.push(linkobj);
    }
    let organisationId = $('#hdnorganisationId').val();
    $("#divMap").load('../Routes/A2BPlanning', { routeID : 0 }, function () {
        loadmap('ROADOWNERSHIP_VIEWANDEDIT');
        if (items != null && linkInfo != items) {
            addToDelegatedRoadLinks(items);
            let linkIndex = Math.round(items.length / 2);
            zoomInToOwnedRoad(items[linkIndex], organisationId, function () {
            });
            $('#leftDiv').show();
            $('#searchLink').show();
        }
        else {
            showNotification('No data available.');
        }

        $(".slidingpanelstructures").removeClass("show").addClass("hide");
        $("#pageheader").hide();
        $('#divMap').show();

        $('#divMap').append('<button class="btn_reg_back next btngrad btnrds btnbdr btn-show-delegate-details" type="button" data-icon="" aria-hidden="true">Back</button>');
        CheckSessionTimeOut();
        Map_size_fit();
        mapResize();
    });
}
$('#span-Edit_help').click(function () {
    EditHelp_popup();
});
$('#IDSearchOrganisation').click(function () {
    SearchOrganisation();
});

