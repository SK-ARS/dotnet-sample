                        var selectedVal = $('#pageSizeVal').val();
                        $('#pageSizeSelect').val(selectedVal);
    $(document).ready(function () {
        searchStringOnKeyPress();
        $(".SelectOrgnaisation").on('click', SelectOrgnaisationFront);
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
        var orgName = e.currentTarget.dataset.DataorganisationName;
        var orgid = e.currentTarget.dataset.DataorganisationId;
        SelectOrgnaisation(orgName, orgid);
    }
    function SelectOrgnaisation(orgName, orgid) {
        GetOrgLinkIds(orgid);
    }
    function GetOrgLinkIds(orgid){
        var url = '@Url.Action("GetOrgLinkID", "RoadOwnership")';
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
        var index = 0;
        while (index < linkInfo.length) {
            if (linkInfo[index] == 0) {
                linkInfo.splice(index, 1);
            } 
            index++;
        }
        var items =[];
        for (var i = 0; i < linkInfo.length;i++)
        {
            var linkobj = {};
            items.push({linkId:linkInfo[i]});
            linkobj["linkId"] = linkInfo[i];
            items.push(linkobj);
        }
        var organisationId = $('#hdnorganisationId').val();
        //showNotification('Show LinkIds on Map - not implemented', 'Ok', '', WarningCancelBtn, '', 1, 'info');
        $("#divMap").load('@Url.Action("A2BPlanning", "Routes", new {routeID = 0})', function () {  
                loadmap('ROADOWNERSHIP_VIEWANDEDIT');
                if (items != null && linkInfo != items) {
                    addToDelegatedRoadLinks(items);
                    var linkIndex = Math.round(items.length / 2);
                    zoomInToOwnedRoad(items[linkIndex], organisationId, function () {
                    });
                    $('#leftDiv').show();
                    $('#searchLink').show();
                }
                else {
                    showNotification('No data available.');
                }
          
            $(".slidingpanelstructures").removeClass("show").addClass("hide");
            // $('#slidingpanelstructures').hide();
            $("#pageheader").hide();
            $('#divMap').show();

            $('#divMap').append('<button class="btn_reg_back next btngrad btnrds btnbdr" onclick="showDelegDetails()" type="button" data-icon="" aria-hidden="true">Back</button>');
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
    
