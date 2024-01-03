    $(document).ready(function () {
        $("#map").html('');
        $("#map").load('@Url.Action("A2BPlanning", "Routes", new {routeID = 0})', function () {
                loadmap('ROADOWNERSHIP_VIEWANDEDIT');
        });
        SelectMenu(3);
        $("#txtOrgSearch").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '../RoadOwnership/ListRoadContactList',
                    dataType: "json",
                    data: {
                        SearchString: request.term,
                        searchFlag: 0
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
                $('#txtOrgSearch').val(ui.item.label); // display the selected text
                $('#hdnOrgId').val(ui.item.value); // save selected id to input
                $('#searchFlag').val("0");//newly added
                SelectOrgnaisation(ui.item.label, ui.item.value);
                return false;
            },
            focus: function (event, ui) {
                $("#txtOrgSearch").val(ui.item.label);
                return false;
            }
        });

    });
    function roadOwnerShip_leftPanel() {
        var randomNumber = Math.random();
        console.log();
        var link = "../RoadOwnership/SearchRoadOwnershipOrg?randomnumber=" + randomNumber;

        $('#leftpanel').load(encodeURI(link));
    }

    /*Functions are starting from here*/
    function ClearAllSelectedRoads() {
        clearAllSelectedRoads();
    }
    function unassignedRoads() {
        if ($("#unassigned").is(":checked")) {
            showUnassignedRoads();
        }
        else {
            hideUnassignedRoads();
        }
    }
    function ownenedRoads() {
        if ($("#owned").is(":checked")) {
            selectAllOwnedRoads(true);
        }
        else {
            deSelectAllOwnedRoads(true);
        }
    }
    function SaveRoadOwnership() {
        closeOwnershipFilter();
        ClearPopUp();
        var randomNumber = Math.random();
        $("#dialogue").load('../../RoadOwnership/CreateRoadOwnership?&random=' + randomNumber, function (responseText, textStatus, req) {
            Resize_PopUp(550);
            $('.loading').hide();
            $("#dialogue").show();
            if (textStatus == "error") {
                location.reload();
            }
        });
        $("#overlay").show();
        $("#dialogue").show();
    }


    function openFilters() {
        document.getElementById("filters").style.width = "450px";
        document.getElementById("banner").style.filter = "brightness(0.5)";
        document.getElementById("banner").style.background = "white";
        document.getElementById("navbar").style.filter = "brightness(0.5)";
        document.getElementById("navbar").style.background = "white";
        function myFunction(x) {
            if (x.matches) { // If media query matches
                document.getElementById("filters").style.width = "200px";
            }
        }
        var x = window.matchMedia("(max-width: 770px)")
        myFunction(x) // Call listener function at run time
        x.addListener(myFunction)
    }
    function closeOwnershipFilter() {
        document.getElementById("filters").style.width = "0";
        document.getElementById("banner").style.filter = "unset"
        document.getElementById("navbar").style.filter = "unset";
        ClearPopUp();

    }
    function viewOrganisation() {
        if (document.getElementById('vieworganisation').style.display !== "none") {
            document.getElementById('vieworganisation').style.display = "none"
            document.getElementById('chevlon-up-icon').style.display = "none"
            document.getElementById('chevlon-down-icon').style.display = "block"
        }
        else {
            document.getElementById('vieworganisation').style.display = "block"
            document.getElementById('chevlon-up-icon').style.display = "block"
            document.getElementById('chevlon-down-icon').style.display = "none"
        }
    }
    function viewRoads() {
        if (document.getElementById('viewroads').style.display !== "none") {
            document.getElementById('viewroads').style.display = "none"
            document.getElementById('chevlon-up-icon1').style.display = "none"
            document.getElementById('chevlon-down-icon1').style.display = "block"
        }
        else {
            document.getElementById('viewroads').style.display = "block"
            document.getElementById('chevlon-up-icon1').style.display = "block"
            document.getElementById('chevlon-down-icon1').style.display = "none"
        }
    }
    function SelectOrgnaisation(orgName, orgid) {
        $('#hdnorganisationId').val(orgid);
        var Flag = "0";

        switch (Flag) {
            case "0":
                $('#pageheader').find('h3').text(orgName);
                $('#pageheader').css({height:"25px"});
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
        var url = '@Url.Action("GetRoadOwnedDetails", "RoadOwnership")';

        $.ajax({
            url: url,
            type: 'POST',
            data: { organisationId: orgid },
            beforeSend: function () {
                startAnimation();
            },
            success: function (result) {
                showLinkIdsOnMap(result, orgid);
            },
            complete: function () {
                stopAnimation();
            }
        });
    }
    function showLinkIdsOnMap(LinkIds, orgid) {
        var index = 0;
        while (index < LinkIds.length) {
            if (LinkIds[index] == 0) {
                LinkIds.splice(index, 1);
            }
            index++;
        }

        var organisationId = orgid;

        if (LinkIds.length > 0) {

            addToDelegatedRoadLinks(LinkIds);
            showOrganisationRoads(organisationId);
            zoomInToOwnedRoad(LinkIds, organisationId, function () {
                document.getElementById("showowned").style.display = 'flex';
               // stopAnimation();
            });
            $('#leftDiv').show();
            $('#searchLink').show();
        }
        else {
            showNotification("The selected organisation doesn't own any roads");
            clearAllOrgData();
            $('#clearOrgData').show();
        }

        $(".slidingpanelstructures").removeClass("show").addClass("hide");

        $('#divMap').show();

        CheckSessionTimeOut();
        Map_size_fit();
        mapResize();
    }
    function handleSearch(e, obj) {
        if (e.keyCode === 13) {
            var text = $.trim($(obj).val());
            searchLinks(text);

            document.getElementById("clearsearchsegments").style.display = 'block';
        }
    }
    function ClearSearchSegments() {
        clearSearchSegments();
    }
    function clearOrgData() {//CLear all Organisation data from map
        clearAllOrgData();
        $("#txtOrgSearch").val('');
    }
    function searchID() {
        var text = $.trim($("#txtLinkSearch").val());
        searchLinks(text);

        document.getElementById("clearsearchsegments").style.display = 'block';
    }
    function EnLargeMap() {
       
        if ($("#Minimzeicon").is(":visible")) { //mode changing to full screen 
            //fullscreenMode();
            $("#navbar").hide();
            $("#Minimzeicon").hide();
            $("#MaxmizeIocn").show();

        } else { //mode changes to minimze
            //closeFullscreen();
            $("#navbar").show();
            $("#Minimzeicon").show();
            $("#MaxmizeIocn").hide();
        }
    }
  

    /* When the openFullscreen() function is executed, open the map in fullscreen.
    Note that we must include prefixes for different browsers, as they don't support the requestFullscreen property yet */
    function fullscreenMode() {
        /* Get the element you want displayed in fullscreen mode*/
        var elem = document.getElementById("banner");
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
    $('#FilterRoadBtn').click(function () {
        openFilters();
    });
    $('#DrgDiv').click(function () {
        EnLargeMap();
    });
    $('#CloseFilter').click(function () {
        closeOwnershipFilter();
    });
    $('#ViewOrgDiv').click(function () {
        viewOrganisation();
    });
    $('#clearOrgData').click(function () {
        clearOrgData();
    });
    $('#ViewRodDiv').click(function () {
        viewRoads();
    });
    $('#unassigned').click(function () {
        unassignedRoads();
    });
    $("#txtLinkSearch").keypress(function (event) {
        handleSearch(event, this);
    });
    $('#showowned').click(function () {
        ownenedRoads();
    });
    $('#searchID').click(function () {
        searchID();
    });
    $('#clearsearchsegments').click(function () {
        ClearSearchSegments();
    });
    $('#assignroads').click(function () {
        SaveRoadOwnership();
    });
    $('#clearSelectedRoads').click(function () {
        ClearAllSelectedRoads();
    });
