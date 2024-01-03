                var selectedVal = $('#pageSizeVal').val();
                $('#pageSizeSelect').val(selectedVal);
    var model = @Html.Raw(Json.Encode(Model))

    $(document).ready(function () {
        Resize_PopUp(700);
        $("#btnOk").on('click', closeDeleg);
        $("#delegationDetails").on('click', showDelegDetails);
        $("#close").on('click', closeConfirmationPopUp);
        $(".clsZoomToSelectedRoad").on('click', ZoomToSelectedRoad);
        $("#saveChange").on('click', saveRoadOwnershipFinal);
        $("#Cancel").on('click', closeConfirmationPopUp);
        $("#exportCsv").on('click', exportToCsv);
    });

    function saveRoadOwnershipFinal() {
        roadOwnershipDetails.linkInfo = null;
        $.ajax({
            url: '../../RoadOwnership/saveRoadOwnership',
            //contentType: 'application/json; charset=utf-8',
            type: 'POST',
            datatype: "json",
            data: JSON.stringify({ postFlag: 0, newOwnerList: roadOwnershipDetails.newOwnerList, newManagerDelegationDetails: roadOwnershipDetails.newManagerDelegationDetails, assignedLinkInfo: null, unassignedLinkInfo: null, length: 0 }),
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
                        if (tempList.length == 0)
                            tempList = null;

                        $.ajax({
                            url: '../../RoadOwnership/saveRoadOwnership',
                            //contentType: 'application/json; charset=utf-8',
                            type: 'POST',
                            datatype: "json",
                            data: JSON.stringify({ postFlag: 1, newOwnerList: null, newManagerDelegationDetails: null, assignedLinkInfo: tempList, unassignedLinkInfo: null, length: roadOwnershipDetails.assignedLinkInfo.length }),
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
                                        if (tempList.length == 0)
                                            tempList = null;

                                        $.ajax({
                                            url: '../../RoadOwnership/saveRoadOwnership',
                                            //contentType: 'application/json; charset=utf-8',
                                            type: 'POST',
                                            datatype: "json",
                                            data: JSON.stringify({ postFlag: 2, newOwnerList: null, newManagerDelegationDetails: null, assignedLinkInfo: null, unassignedLinkInfo: tempList, length: roadOwnershipDetails.unassignedLinkInfo.length }),
                                            beforeSend: function () {
                                                startAnimation();
                                            },
                                            success: function (data) {
                                                stopAnimation();
                                                if (data.result == true) {
                                                    $('#save_anim').hide();
                                                    if (data.status == true) {
                                                        ShowSuccessModalPopup('Changes are saved successfully.','loadRefresh');
                                                        //clearAllSelectedRoads();
                                                       // clearAllOrgData();
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
        //location.reload();
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
    $('#span-close1').click(function () {
        if ($('#closeButton').val() == "true")
        {
            closeDeleg();
        }
        else
        {
        $('#overlay').hide();
        addscroll();
        resetdialogue();
        }
    });
    function closeConfirmationPopUp() {
        $('#overlay').hide();
        addscroll();
        resetdialogue();
        CloseWarningPopup();
    }

    function ZoomToSelectedRoad() {
        startAnimation();
        var linkId = e.currentTarget.dataset.DataLinkId;
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
    

