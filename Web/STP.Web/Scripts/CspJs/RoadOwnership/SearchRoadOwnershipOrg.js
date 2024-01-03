    $(document).ready(function () {
        $('#leftpanel').append('<div id="searchLink" style="margin-top:10px;margin-left:10px;width:180px;"><table style="width:240px"><tbody><tr style = "width:45px;"><td colspan="2"><input type="text" id="txtLinkSearch" style = "width:200px;" placeholder="Search road segment by ID" title="Link ID of the location. Press ENTER after entering the Link ID" onkeypress="handleSearch(event, this)"></td></tr><tr><td><button id="searchID" onclick="searchID()" style="width:77px" class="create btngrad btnrds btnbdr margintop6 marginbottom7"  aria-hidden="true" data-icon="&#xe07f;">Search</button></td><td><button id="clearsearchsegments" onclick="ClearSearchSegments()" style="width:80px; display:none" class="create btngrad btnrds btnbdr margintop6 marginbottom7"  aria-hidden="true" data-icon="">Clear</button></td></tr></tbody></table></div>'
           );
        $('#leftpanel').show();
        //fillPageSizeSelect();
        $(".slidingpanelstructures").removeClass("show").addClass("hide");
        selectedmenu('Manage roads');
        $("#searchOrganisation").on('click', SearchRoadOwnerOrg);
        $("#clearOrgData").on('click', clearOrgData);
        $("#unassigned").on('click', unassignedRoads);
        $("#owned").on('click', ownenedRoad);
        $("#assignroads").on('click', SaveRoadOwnership);
        $("#clearSelectedRoads").on('click', ClearAllSelectedRoads);
    });
    function clearOrgData() {//CLear all Organisation data from map
        clearAllOrgData();
        //$('#clearOrgData').hide();
    }
    function handleSearch(e, obj) {
        if (e.keyCode === 13) {
            var text = $.trim($(obj).val());
            searchLinks(text);

            document.getElementById("clearsearchsegments").style.display = 'block';
        }
    }

    
    
   
    function ClearAllSelectedRoads() {
        clearAllSelectedRoads();
        //document.getElementById("owned").checked = false;
        //$("#showowned").hide();
    }
    function ClearSearchSegments() {
        clearSearchSegments();
        //$("#showowned").hide();
    }
    //function display() {
    //    Display();
    //}

    function SearchRoadOwnerOrg() {
        clearAllSelectedRoads();
        $('.loading').show();
        var randomNumber = Math.random();
        resetdialogue();
        $("#dialogue").load('../../RoadOwnership/ListRoadContactList?searchFlag=0' + '&origin=LeftpanelSearch' + '&random=' + randomNumber, function (responseText, textStatus, req) {
            $('.loading').hide();
            $("#dialogue").show();
            Resize_PopUp(800);
            if (textStatus == "error") {
                 //location.reload();
            }
        });
        $("#overlay").show();
        $("#dialogue").show();
    }
    //function SaveRoadOwnership() {
    //    ClearPopUp();
    //    var randomNumber = Math.random();
    //    $("#dialogue").load('../../RoadOwnership/CreateRoadOwnership?&random=' + randomNumber, function (responseText, textStatus, req) {
    //        Resize_PopUp(550);
    //        $('.loading').hide();
    //        $("#dialogue").show();
    //        if (textStatus == "error") {
    //             //location.reload();
    //        }
    //    });
    //    $("#overlay").show();
    //    $("#dialogue").show();
    //}
    function getInfoList() {
        pageNo = 1;
        pageSize = 5;
        startIndex = (pageNo - 1) * pageSize;
        endIndex = (pageNo - 1) * pageSize + (pageSize - 1);
        var linkInfo = roadOwnershipDetails.assignedLinkInfo;
        var linkInfolist = roadOwnershipDetails.assignedLinkInfo.slice(startIndex, endIndex);
        return linkInfolist;
    }

    //function SaveRoadOwnership() {
       //ClearPopUp();
    //    var randomNumber = Math.random();
    //    $("#dialogue").load('../../RoadOwnership/CreateRoadOwnership?&random=' + randomNumber, function (responseText, textStatus, req) {
    //        Resize_PopUp(550);
    //        $('.loading').hide();
    //        $("#dialogue").show();
    //        if (textStatus == "error") {
    //            //location.reload();
    //        }
    //    });
    //    $("#overlay").show();
    //    $("#dialogue").show();
    //}

