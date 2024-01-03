var selectedVal = $('#pageSizeVal').val();
$('#pageSizeSelect').val(selectedVal);
$(document).ready(function () {
    searchStringOnKeyPress();
$('body').on('click','.SelectOrgnaisation', function(e) { 
  e.preventDefault();
  SelectOrgnaisationFront(this);
}); 
});
function searchStringOnKeyPress() {
    $("#searchKey").focus();
    $("#searchKey").live("keypress", function (event) {
        if (event.which == 13) {
            SearchOrganisation();
        }
    });
}

$('#span-close1').click(function () {
    if ($('#hf_origin').val() == "LeftpanelSearch") {
        $("#dialogue").html('');
        $('#overlay').hide();
        resetdialogue();
        addscroll();
    }
    else {
        Resize_PopUp(550);
        $("#header").show();
        $("#Create_RoadOwnership").show();
        $("#ContactList").hide();

    }
});
function SelectOrgnaisationFront(e) {
    var orgName =$(e).attr("dataorganisationname");
    var orgid =$(e).attr("dataorganisationid");
    SelectOrgnaisation(orgName, orgid);
}

function SelectOrgnaisation(orgName, orgid) {
    $('#hdnorganisationId').val(orgid);
    var Flag = $('#searchFlag').val();
    switch (Flag) {
        case "0":
            $('#pageheader').find('h3').text(orgName);
            $('#pageheader').css({ height: "25px" });
            $('#clearOrgData').show();
            GetOrgLinkIds(orgName, orgid);
            break;
        case "1":
            setManagerDetails(orgName, orgid);
            break;
        case "2":
            setLADetails(orgName, orgid);
            break;
        case "3":
            setHaMacDetails(orgName, orgid);
            break;
        case "4":
            setPoliceDetails(orgName, orgid);
            break;
        default:
            break;
    }
}

function GetOrgLinkIds(orgName, orgid) {
    var url = '../RoadOwnership/GetRoadOwnedDetails';
    $.ajax({
        url: url,
        type: 'POST',
        data: { organisationId: orgid },
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
            showLinkIdsOnMap(result);
        },
        complete: function () {
            stopAnimation();
        }
    });
}

function resizeDlg() {
    Resize_PopUp(550);
    $("#header").show();
    $("#Create_RoadOwnership").show();
}

function showLinkIdsOnMap(LinkIds) {
    var index = 0;
    while (index < LinkIds.length) {
        if (LinkIds[index] == 0) {
            LinkIds.splice(index, 1);
        }
        index++;
    }

    var organisationId = $('#hdnorganisationId').val();

    if (LinkIds.length > 0) {

        addToDelegatedRoadLinks(LinkIds);
        zoomInToOwnedRoad(LinkIds, organisationId, function () {
            document.getElementById("showowned").style.display = 'flex';
        });
        $('#leftDiv').show();
        $('#searchLink').show();
    }
    else {
        showNotification("The selected organisation doesn't own any roads");
        clearAllOrgData();
    }

    $(".slidingpanelstructures").removeClass("show").addClass("hide");

    $('#divMap').show();

    CheckSessionTimeOut();
    Map_size_fit();
    mapResize();
}
$('#span-Edit_help').click(function () {
    EditHelp_popup();
});

