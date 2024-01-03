var selectedVal;
var model;
function RoadOwnerReportInit() {
    selectedVal = $('#pageSizeVal').val();
    $('#pageSizeSelect').val(selectedVal);
    model = $('#hf_RawJsonModel').val();
    Resize_PopUp(700);
}
$(document).ready(function () {

    $('body').on('click', '#btnOk', function (e) {
        //e.preventDefault();
        closeDeleg(this);
    });
    $('body').on('click', '#delegationDetails', function (e) {
        //e.preventDefault();
        showDelegDetails(this);
    });
    $('body').on('click', '#close', function (e) {
        //e.preventDefault();
        closeConfirmationPopUp(this);
    });
    $('body').on('click', '.clsZoomToSelectedRoad', function (e) {
        
        //e.preventDefault();
        ZoomToSelectedRoad(this);
    });
    $('body').on('click', '#saveChange', function (e) {
        //e.preventDefault();
        saveRoadOwnershipFinal(this);
    });
    $('body').on('click', '#Cancel', function (e) {
        //e.preventDefault();
        closeConfirmationPopUp(this);
    });
    $('body').on('click', '#exportCsv', function (e) {
       
        //e.preventDefault();
        exportToCsv(this);
    });
    $('body').on('click', '#span-close1', function () {
    //$('#span-close1').click(function () {
        if ($('#closeButton').val() == "true") {
            closeDeleg();
        }
        else {
            $('#overlay').hide();
            addscroll();
            resetdialogue();
        }
    });
});

function saveRoadOwnershipFinal() {
    var datapayload = JSON.stringify({ postFlag: 0, newOwnerList: roadOwnershipDetails.newOwnerList, newManagerDelegationDetails: roadOwnershipDetails.newManagerDelegationDetails, length: 0 });
    roadOwnershipDetails.linkInfo = null;
    $.ajax({
        url: '../../RoadOwnership/saveRoadOwnership',
        type: 'POST',
        datatype: "json",
        data: datapayload,
        
        beforeSend: function () {
            startAnimation();
        },
        success: function (data) {
            $('#save_anim').show();
            if (data.result == true) {
                if (roadOwnershipDetails.assignedLinkInfo == null)
                    roadOwnershipDetails.assignedLinkInfo = [];

                for (var i = 0; i < (Math.floor(roadOwnershipDetails.assignedLinkInfo.length / 1000)) + 1; i++) {
                    if (i == Math.floor(roadOwnershipDetails.assignedLinkInfo.length / 1000))
                        var limit = (i * 1000) + (roadOwnershipDetails.assignedLinkInfo.length % 1000);
                    else
                        var limit = (i * 1000) + 1000;

                    var tempList = roadOwnershipDetails.assignedLinkInfo.slice(i * 1000, limit);
                    if (tempList.length == 0) {
                        datapayload = JSON.stringify({ postFlag: 1,length: roadOwnershipDetails.assignedLinkInfo.length });
                    }
                    else {
                        datapayload = JSON.stringify({ postFlag: 1, assignedLinkInfo: tempList, length: roadOwnershipDetails.assignedLinkInfo.length });
                    }
                    $.ajax({
                        url: '../../RoadOwnership/saveRoadOwnership',
                        type: 'POST',
                        datatype: "json",
                        data: datapayload,
                        beforeSend: function () {
                            startAnimation();
                        },
                        success: function (data) {
                            if (data.result == true) {
                                if (roadOwnershipDetails.unassignedLinkInfo == null)
                                    roadOwnershipDetails.unassignedLinkInfo = [];

                                for (var i = 0; i < (Math.floor(roadOwnershipDetails.unassignedLinkInfo.length / 1000)) + 1; i++) {
                                    if (i == Math.floor(roadOwnershipDetails.unassignedLinkInfo.length / 1000))
                                        var limit = (i * 1000) + (roadOwnershipDetails.unassignedLinkInfo.length % 1000);
                                    else
                                        var limit = (i * 1000) + 1000;
                                    
                                    var tempList = roadOwnershipDetails.unassignedLinkInfo.slice(i * 1000, limit);
                                    if (tempList.length == 0) {
                                        datapayload = JSON.stringify({ postFlag: 2, length: roadOwnershipDetails.unassignedLinkInfo.length });
                                    }
                                    else {
                                        datapayload = JSON.stringify({ postFlag: 2, unassignedLinkInfo: tempList, length: roadOwnershipDetails.unassignedLinkInfo.length });
                                    }

                                    $.ajax({
                                        url: '../../RoadOwnership/saveRoadOwnership',
                                        type: 'POST',
                                        datatype: "json",
                                        data: datapayload,
                                        beforeSend: function () {
                                            startAnimation();
                                        },
                                        success: function (data) {
                                            stopAnimation();
                                            if (data.result == true) {
                                                $('#save_anim').hide();
                                                if (data.status == true) {
                                                    ShowSuccessModalPopup('Changes are saved successfully', 'loadRefresh');

                                                }
                                                else {
                                                    ShowErrorPopup('Failed to save the changes. Try again');
                                                }
                                            }
                                        },
                                        error: function (xhr, error, status) {
                                            stopAnimation();
                                            $('#save_anim').hide();
                                            ShowErrorPopup('Failed to save the changes. Try again');
                                        },
                                        complete: function () {
                                            addscroll();

                                        }
                                    });
                                }
                            }
                        },
                        error: function (xhr, error, status) {
                            $('#save_anim').hide();
                            stopAnimation();
                            ShowErrorPopup('Failed to save the changes. Try again');
                        },
                        complete: function () {
                            stopAnimation();
                            addscroll();
                        }
                    });
                }
            }
        },
        error: function (xhr, error, status) {
            $('#save_anim').hide();
            stopAnimation();
            ShowErrorPopup('Failed to save the changes. Try again');
        },
        complete: function () {
            addscroll();

        }
    });
}
function loadRefresh() {
    CloseSuccessModalPopup();
    $("#dialogue").html('');
}
function showDelegDetails() {
    $('#Tabledata').hide();
    $('#delegationSummary').show();
    $('#headPart').hide();
    $('#pagePart').hide();
    $('#closeButton').val(true);

}
function closeDeleg() {
    $('#Tabledata').show();
    $('#delegationSummary').hide();
    $('#headPart').show();
    $('#pagePart').show();
    $('#closeButton').val(false);
}

function closeConfirmationPopUp() {
    $('#overlay').hide();
    addscroll();
    resetdialogue();
    CloseWarningPopup();
}

function ZoomToSelectedRoad(e) {
   
    startAnimation();
    var linkId = $(e).attr("DataLinkId");
    zoomInToSelectedLinkId(linkId, function () {
        stopAnimation();
        $("#dialogue").html('');
        $("#dialogue").hide();
        $("#overlay").hide();
    });
}
$('#span-Edit_help').click(function () {
    EditHelp_popup();
});


