    var myStructureArray = null;
    var otherStructureArray = null;
    var length;
    var myStructTimeFlag = 0, otherStructTimeFlag = 0, myConstTimeFlag = 0, otherConstTimeFlag = 0;
    var structPanTimeFlag = 0;
    var animationText = '';

    $(document).ready(function () {
        //

        $("#btnenlargemap").on('click', EnLargeMap);
        $("#opnfiltr").on('click', openFilters);
        $("#card-swipe1").on('click', closenewNav);
        $("#card-swipe2").on('click', opennewNav);
        $("#PointConstraintImg").on('click', createPointConstraintFn);
        $("#LinearConstraintImg").on('click', createLinearConstraintFn);
        $("#AreaConstraintImg").on('click', createAreaConstraintFn);
        $("#closefbtn").on('click', closeFilters);
        $("#viewstructconstraints").on('click', viewStructuresConstraints);
        $("#boundaries").on('click', viewBoundaries);
        $("#divviewroads").on('click', viewRoads);
        $(".lineconstraintclose").on('click', lineconstraintwarningpopupClose);
        $(".constraintcreationfailed").on('click', ConstraintcreationfailedPopup);
        $(".closepoints").on('click', closePoinsandlinesnotonroad);
        $(".backclick").on('click', BackClicking);
        $(".backclickconstr").on('click', BackClickConstr);
if($('#hf_IsAgreedAppl').val() ==  "True") {
            $("#IsAgreedAppl").val("Yes");
            $("#HFStruCode").val('@ViewBag.StructureCode');
        }
        var PortalType = $("#PortalType").val();
        if (PortalType == "696002") {
            SelectMenu(4);
        }
        else {
            SelectMenu(3);
        }
        if ($("#IsAgreedAppl").val() == "Yes") {
            ShowAgreedApplStructureOnMap();
        }
        else {
if($('#hf_StructureId').val() ==  "")
            {
                $("#banner").prepend('<div class="button main-button mr-0 mb-2 mt-4 " style="position:absolute; top:10px; left:10px; z-index:999;"><button class="btn btn-outline-primary btn-normal backclick" role="button" aria-pressed="true" style="width: 9rem !important;" arg1="@ViewBag.StructureId">BACK</button></div>');
            }
if($('#hf_IsConstrId').val() ==  "") {
                $("#banner").prepend('<div class="button main-button mr-0 mb-2 mt-4 " style="position:absolute; top:10px; left:10px; z-index:999;"><button class="btn btn-outline-primary btn-normal backclickconstr" role="button" aria-pressed="true" style="width: 9rem !important;" >BACK</button></div>');

            }
if($('#hf_X').val() ==  '' && '@ViewBag.X' != null && '@ViewBag.Y' != '' && '@ViewBag.Y' != null) {
if($('#hf_IsConstrId').val() ==  "") { //Call from constraint
                     loadmap('STRUCTURES');
                setZoomTo('@ViewBag.X', '@ViewBag.Y', 9);

                var boundsAndZoom = getCurrentBoundsAndZoom();
                document.getElementById('OwnedByMe').checked = true;
                document.getElementById('Constraints').checked = true;
                setBounds(boundsAndZoom.bounds);
                showStructBounds();
                createContextMenu();
                }
                else {
                loadmap('STRUCTURES');
                setZoomTo('@ViewBag.X', '@ViewBag.Y', 9);

                var boundsAndZoom = getCurrentBoundsAndZoom();
                document.getElementById('OwnedByMe').checked = true;
                document.getElementById('Structs').checked = true;
                //document.getElementById('Constraints').checked = true;
                document.getElementById('Underbridge').checked = true;
                document.getElementById('Overbridge').checked = true;
                document.getElementById('UnderAndOverbridge').checked = true;
                document.getElementById('LevelCrossing').checked = true;


                setBounds(boundsAndZoom.bounds);
                showStructBounds();
                createContextMenu();
                }


            }
            else {
if($('#hf_x1').val() ==  '') {
                    var region  = $('#hf_x1', y1: '@ViewBag.y1', x2: '@ViewBag.x2', y2: '@ViewBag.y2').val(); 
                    //loadmap('STRUCTURES', null, null, null, region);
                    //$("#map").html('');

                    //$("#map").load('@Url.Action("A2BPlanning", "Routes", new { routeID = 0 })', function () {
                        //LoadLeftPanel();
                    loadmap('STRUCTURES', null, null, null, region);
                        selectedmenu('Structures');
                        mapcontextMenuOn = true;
                    createContextMenu();


                    //});
                    objifxStpMap.olMap.zoomIn();
                    objifxStpMap.olMap.zoomIn();
                    objifxStpMap.olMap.zoomIn();
                }
                else {
                    loadmap('STRUCTURES');
                }
            }

        }
        @*if (@ViewBag.ConstraintID != 0) {
            startAnimation();
            var flageditmode = $('#flageditmode').val();
            $("#dialogue").hide();
            var randomNumber = Math.random();
            $("#dialogue").load('../Constraint/ViewConstraint?ConstraintID=' + @ViewBag.ConstraintID +'&flageditmode=' + flageditmode + '&random=' + randomNumber);
        }*@
    });
    function BackClicking(e) {
        var arg1 = e.currentTarget.dataset.arg1;
        BackClick(arg1);
    }
    $('#OwnedByMe').change(function () {
        if (this.checked) {
            var boundsAndZoom = getCurrentBoundsAndZoom();
            if (boundsAndZoom.zoom < 7 && (document.getElementById('Structs').checked == true || document.getElementById('Constraints').checked == true)) {
                //ESDAL4 - showNotification("Zoom-in to view structures/constraints");
                return;
            }
        }
        showStructBounds();
    });
    function BackClickConstr(){
        history.back();
    }
    $('#OwnedByOtherOrganisations').change(function () {
        if (this.checked) {
            var boundsAndZoom = getCurrentBoundsAndZoom();
            if (boundsAndZoom.zoom < 7 && (document.getElementById('Structs').checked == true || document.getElementById('Constraints').checked == true)) {
                //ESDAL4 - showNotification("Zoom-in to view structures/constraints");
                return;
            }
        }
        showStructBounds();
    });

    $('#Structs').change(function () {
        if (this.checked) {
                document.getElementById('Underbridge').checked = true;
                document.getElementById('Overbridge').checked = true;
                document.getElementById('LevelCrossing').checked = true;
                document.getElementById('UnderAndOverbridge').checked = true;
                var boundsAndZoom = getCurrentBoundsAndZoom();
            if (boundsAndZoom.zoom < 7 && (document.getElementById('OwnedByMe').checked == true || document.getElementById('OwnedByOtherOrganisations').checked == true)) {
                //ESDAL4 - showNotification("Zoom-in to view structures/constraints");
                myFunction();
                return;
            }
        }
        else {
            document.getElementById('Underbridge').checked = false;
            document.getElementById('Overbridge').checked = false;
            document.getElementById('UnderAndOverbridge').checked = false;
            document.getElementById('LevelCrossing').checked = false;
        }
        showStructBounds();
    });

    $('#Constraints').change(function () {
        if (this.checked) {
            var boundsAndZoom = getCurrentBoundsAndZoom();
            if (boundsAndZoom.zoom < 7 && (document.getElementById('OwnedByMe').checked == true || document.getElementById('OwnedByOtherOrganisations').checked == true)) {
                //ESDAL4 - showNotification("Zoom-in to view structures/constraints");
                myFunction();
                return;
            }
        }
        showStructBounds();
    });

    function myFunction() {
        var x = document.getElementById("snackbar");
        x.className = "show";
        setTimeout(function () { x.className = x.className.replace("show", ""); }, 3000);
    }

    $('#Underbridge').change(function () {
        //ESDAL4 - startAnimation();
        var boundsAndZoom = getCurrentBoundsAndZoom();
        if (this.checked) {
            document.getElementById('Structs').checked = true;
            if (document.getElementById('OwnedByMe').checked == true && myStructureArray != undefined) {
                filterStructures(myStructureArray.underBridge, true, false, boundsAndZoom.zoom);
            }
            if (document.getElementById('OwnedByOtherOrganisations').checked == true && otherStructureArray != undefined) {
                filterStructures(otherStructureArray.underBridge, true, true, boundsAndZoom.zoom);
            }
        }
        else {
            if (document.getElementById('Structs').checked == true) {
                if (document.getElementById('OwnedByMe').checked == true && myStructureArray != undefined) {
                    filterStructures(myStructureArray.underBridge, false, false, boundsAndZoom.zoom);
                }
                if (document.getElementById('OwnedByOtherOrganisations').checked == true && otherStructureArray != undefined) {
                    filterStructures(otherStructureArray.underBridge, false, true, boundsAndZoom.zoom);
                }
            }
        }
        //ESDAL4 - stopAnimation();
    });

        $('#Overbridge').change(function () {
        //ESDAL4 - startAnimation();
        var boundsAndZoom = getCurrentBoundsAndZoom();
        if (this.checked) {
            document.getElementById('Structs').checked = true;
            if (document.getElementById('OwnedByMe').checked == true && myStructureArray != undefined) {
                filterStructures(myStructureArray.overBridge, true, false, boundsAndZoom.zoom);
            }
            if (document.getElementById('OwnedByOtherOrganisations').checked == true && otherStructureArray != undefined) {
                filterStructures(otherStructureArray.overBridge, true, true, boundsAndZoom.zoom);
            }
        }
        else {
            if (document.getElementById('Structs').checked == true) {
                if (document.getElementById('OwnedByMe').checked == true && myStructureArray != undefined) {
                    filterStructures(myStructureArray.overBridge, false, false, boundsAndZoom.zoom);
                }
                if (document.getElementById('OwnedByOtherOrganisations').checked == true && otherStructureArray != undefined) {
                    filterStructures(otherStructureArray.overBridge, false, true, boundsAndZoom.zoom);
                }
            }
        }
        //ESDAL4 - stopAnimation();
    });

            $('#UnderAndOverbridge').change(function () {
        //ESDAL4 - startAnimation();
        var boundsAndZoom = getCurrentBoundsAndZoom();
        if (this.checked) {
            document.getElementById('Structs').checked = true;
            if (document.getElementById('OwnedByMe').checked == true && myStructureArray != undefined) {
                filterStructures(myStructureArray.underAndOverBridge, true, false, boundsAndZoom.zoom);
            }
            if (document.getElementById('OwnedByOtherOrganisations').checked == true && otherStructureArray != undefined) {
                filterStructures(otherStructureArray.underAndOverBridge, true, true, boundsAndZoom.zoom);
            }
        }
        else {
            if (document.getElementById('Structs').checked == true) {
                if (document.getElementById('OwnedByMe').checked == true && myStructureArray != undefined) {
                    filterStructures(myStructureArray.underAndOverBridge, false, false, boundsAndZoom.zoom);
                }
                if (document.getElementById('OwnedByOtherOrganisations').checked == true && otherStructureArray != undefined) {
                    filterStructures(otherStructureArray.underAndOverBridge, false, true, boundsAndZoom.zoom);
                }
            }
        }
        //ESDAL4 - stopAnimation();
    });

                $('#LevelCrossing').change(function () {
        //ESDAL4 - startAnimation();
        var boundsAndZoom = getCurrentBoundsAndZoom();
        if (this.checked) {
            document.getElementById('Structs').checked = true;
            if (document.getElementById('OwnedByMe').checked == true && myStructureArray != undefined) {
                filterStructures(myStructureArray.levelCrossing, true, false, boundsAndZoom.zoom);
            }
            if (document.getElementById('OwnedByOtherOrganisations').checked == true && otherStructureArray != undefined) {
                filterStructures(otherStructureArray.levelCrossing, true, true, boundsAndZoom.zoom);
            }
        }
        else {
            if (document.getElementById('Structs').checked == true) {
                if (document.getElementById('OwnedByMe').checked == true && myStructureArray != undefined) {
                    filterStructures(myStructureArray.levelCrossing, false, false, boundsAndZoom.zoom);
                }
                if (document.getElementById('OwnedByOtherOrganisations').checked == true && otherStructureArray != undefined) {
                    filterStructures(otherStructureArray.levelCrossing, false, true, boundsAndZoom.zoom);
                }
            }
        }
        //ESDAL4 - stopAnimation();
    });

    function createLinearConstraintFn() {

        var zoom = objifxStpMap.olMap.getZoom();
        if (zoom <= 8) {
            createLinearConstraint.deactivate();
            objifxStpmapStructures.clearMarkerConstriants();

            $('#lineconstraintwarningpopup').addClass('show').removeClass('fade');
            $('#lineconstraintwarningpopup').show();


            //showWarningPopDialog('Constraint creation is allowed only at zoom level 9 or above.', 'Ok', '', 'close_alert', '', 1, 'info');
        }
        else {

            $("#mySidenav1").css("display", "block");
            $("#card-swipe1").css("display", "block");
            $("#card-swipe2").css("display", "block");


            //alert('INSTRUCTIONS\n\n1) While creating lines ensure to follow the roads as much as possible.\n2) Ensure that all points and connecting lines are always on road.\n3) Select the highest zoom level for better accuracy.\n\nNOTE\nYou can pan the map even though line constraint creation tool is active by holding left mouse button down and moving.');
        }
        if (objifxStpMap.eventList['DEACTIVATECONTROL'] != undefined && typeof (objifxStpMap.eventList['DEACTIVATECONTROL']) === "function") {
            objifxStpMap.eventList['DEACTIVATECONTROL'](1, "STRUCTURES");
        }

        createAreaConstraint.deactivate();
        createPointConstraint.deactivate();
        var color = $("#LinearConstraintImg").css("background-color");
        if (color == "rgb(128, 128, 128)") {
            $("#PointConstraintImg").css('background', 'none');
            $("#LinearConstraintImg").css('background', 'none');
            $("#AreaConstraintImg").css('background', 'none');
            objifxStpmapStructures.clearMarkerConstriants();
            createLinearConstraint.deactivate();
        }
        else {
            $("#PointConstraintImg").css('background', 'none');
            $("#LinearConstraintImg").css('background', 'gray');
            $("#AreaConstraintImg").css('background', 'none');
            createLinearConstraint.activate();
        }

    }

    function createPointConstraintFn() {
        if (objifxStpMap.eventList['DEACTIVATECONTROL'] != undefined && typeof (objifxStpMap.eventList['DEACTIVATECONTROL']) === "function") {
            objifxStpMap.eventList['DEACTIVATECONTROL'](0, "STRUCTURES");
        }
        createLinearConstraint.deactivate();
        createAreaConstraint.deactivate();

        var color = $("#PointConstraintImg").css("background-color");
        if (color == "rgb(128, 128, 128)") {
            $("#PointConstraintImg").css('background', 'none');
            $("#LinearConstraintImg").css('background', 'none');
            $("#AreaConstraintImg").css('background', 'none');
            objifxStpmapStructures.clearMarkerConstriants();
            createPointConstraint.deactivate();
        }
        else {
            $("#PointConstraintImg").css('background', 'gray');
            $("#LinearConstraintImg").css('background', 'none');
            $("#AreaConstraintImg").css('background', 'none');
            createPointConstraint.activate();
        }
    }

    function createAreaConstraintFn() {
        if (objifxStpMap.eventList['DEACTIVATECONTROL'] != undefined && typeof (objifxStpMap.eventList['DEACTIVATECONTROL']) === "function") {
            objifxStpMap.eventList['DEACTIVATECONTROL'](2, "STRUCTURES");
        }

        createLinearConstraint.deactivate();
        createPointConstraint.deactivate();
        var color = $("#AreaConstraintImg").css("background-color");
        if (color == "rgb(128, 128, 128)") {
            $("#PointConstraintImg").css('background', 'none');
            $("#LinearConstraintImg").css('background', 'none');
            $("#AreaConstraintImg").css('background', 'none');
            objifxStpmapStructures.clearMarkerConstriants();
            createAreaConstraint.deactivate();
        }
        else {
            $("#PointConstraintImg").css('background', 'none');
            $("#LinearConstraintImg").css('background', 'none');
            $("#AreaConstraintImg").css('background', 'gray');
            createAreaConstraint.activate();
        }



    }

    function structPanChanged() {
        structPanTimeFlag = 1;
    }

    function ConstraintcreationfailedPopup() {
        $('#ConstraintcreationfailedPopup').addClass('fade').removeClass('show');
        $('#ConstraintcreationfailedPopup').hide();
        $("#PointConstraintImg").css('background', 'none');
        $("#LinearConstraintImg").css('background', 'none');
        $("#AreaConstraintImg").css('background', 'none');
        objifxStpmapStructures.clearMarkerConstriants();
        createAreaConstraint.deactivate();

    }

    function lineconstraintwarningpopupClose() {

        $('#lineconstraintwarningpopup').addClass('fade').removeClass('show');
        $('#lineconstraintwarningpopup').hide();
        $("#PointConstraintImg").css('background', 'none');
        $("#LinearConstraintImg").css('background', 'none');
        $("#AreaConstraintImg").css('background', 'none');
        objifxStpmapStructures.clearMarkerConstriants();
        createLinearConstraint.deactivate();
        createAreaConstraint.deactivate();
        createPointConstraint.deactivate();

    }

    function closePoinsandlinesnotonroad() {
        $('#Poinsandlinesnotonroad').addClass('fade').removeClass('show');
        $("#Poinsandlinesnotonroad").hide();

    }

    function closenewNav() {
        //document.getElementById("MakingcontraintsInfo").style.width = "0";
        document.getElementById("mySidenav1").style.width = "0";
        document.getElementById("card-swipe1").style.display = "none"
        document.getElementById("card-swipe2").style.display = "block"

    }

    function opennewNav() {
        document.getElementById("mySidenav1").style.width = "345px";
        document.getElementById("card-swipe1").style.display = "block"
        document.getElementById("card-swipe2").style.display = "block"
        function myFunction(x) {
            if (x.matches) { // If media query matches
                document.getElementById("mySidenav1").style.width = "auto";
            }
        }
        var x = window.matchMedia("(max-width: 410px)")
        myFunction(x) // Call listener function at run time
        x.addListener(myFunction)
        //function myFunction(x) {
        //    if (x.matches) { // If media query matches
        //        document.getElementById("mySidenav1").style.width = "auto";
        //    }
        //}
        //var x = window.matchMedia("(max-width: 410px)")
        //myFunction(x) // Call listener function at run time
        //x.addListener(myFunction)
    }

    function fetchStructBounds(otherOrg, boundsAndZoom) {
        //ESDAL4 - startAnimation(animationText);
        if (otherOrg == 0)
            myStructTimeFlag = 1;
        else
            otherStructTimeFlag = 1;
        $.ajax({
            type: 'POST',
            dataType: 'json',
            //async: false,
            url: '../Structures/MyStructureInfoList',
            //contentType: 'application/json; charset=utf-8',
            data: { otherOrg: otherOrg, page: 0, left: Math.floor(boundsAndZoom.bounds.left), right: Math.ceil(boundsAndZoom.bounds.right), bottom: Math.floor(boundsAndZoom.bounds.bottom), top: Math.ceil(boundsAndZoom.bounds.top) },
            beforeSend: function (xhr) {
                //startAnimation();
            },
            success: function (result) {
                if (result.result == "session out") {
                    location.reload();
                }
                else {
                    length = result.result;
                    var resultArr = new Array();
                    for (var i = 0; i <= length / 1000; i++) {
                        $.ajax({
                            type: 'POST',
                            dataType: 'json',
                            async: false,
                            cache: true,
                            url: '../Structures/MyStructureInfoList',
                            data: { otherOrg: otherOrg, page: i + 1, left: Math.floor(boundsAndZoom.bounds.left), right: Math.ceil(boundsAndZoom.bounds.right), bottom: Math.floor(boundsAndZoom.bounds.bottom), top: Math.ceil(boundsAndZoom.bounds.top) },
                            beforeSend: function (xhr) {
                                //startAnimation();

                            },
                            success: function (result) {
                                resultArr = resultArr.concat(result.result);
                                //ESDAL4 - stopAnimation();
                            }
                        });

                    }
                    if (otherOrg == 0)
                        myStructureArray = showStructures(resultArr, false, 'MYSTRUCTURES', boundsAndZoom.zoom);
                    else
                        otherStructureArray = showStructures(resultArr, true, 'MYSTRUCTURES', boundsAndZoom.zoom);
                }
            },
            complete: function (result) {
                var str = '<div>' + result.responseText + '</div>';
                var islogin = $(str).find('#isloginPage').val();
                if (islogin == 1) {
                    location.href = '../Account/Login';
                }
                //ESDAL4 - stopAnimation();
            }

        });
    }

    function fetchConstBounds(otherOrg, boundsAndZoom) {
        //ESDAL4 - startAnimation(animationText);
        if (otherOrg == 0)
            myConstTimeFlag = 1;
        else
            otherConstTimeFlag = 1;
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: '../Constraint/GetConstraintListForOrg',
            data: { otherOrg: otherOrg, left: Math.floor(boundsAndZoom.bounds.left), right: Math.ceil(boundsAndZoom.bounds.right), bottom: Math.floor(boundsAndZoom.bounds.bottom), top: Math.ceil(boundsAndZoom.bounds.top) },
            //data: { otherOrg: 0, left: 386100, right: 386200, bottom: 159600, top: 159700 },
            beforeSend: function (xhr) {
                //startAnimation();
            },
            success: function (result) {
                if (result.result == "session out") {
                    location.reload();
                }
                else if (result.result.length > 0) {
                    if (otherOrg == 0)
                        showConstraints(result.result, false);
                    else
                        showConstraints(result.result, true);
                }
                //ESDAL4 - stopAnimation();
            },
            complete: function (result) {
                var str = '<div>' + result.responseText + '</div>';
                var islogin = $(str).find('#isloginPage').val();
                if (islogin == 1) {
                    location.href = '../Account/Login';
                }
                //ESDAL4 - stopAnimation();
            }
        });
    }

    function showStructBounds() {
        if ($('#IsMovListAvalabl').val() != "Yes") {
            if (structPanTimeFlag == 1) {
                structPanTimeFlag = 0;

                myStructTimeFlag = 0;
                otherStructTimeFlag = 0;
                myConstTimeFlag = 0;
                otherConstTimeFlag = 0;

                clearStructures();
                clearConstraints();

                removePopups();
            }

            var boundsAndZoom = getCurrentBoundsAndZoom();
            if (boundsAndZoom.zoom < 7)
                return;

            animationText = "";
            if (document.getElementById('Structs').checked == true) {
                if ((document.getElementById('OwnedByMe').checked == true && myStructTimeFlag == 0) || (document.getElementById('OwnedByOtherOrganisations').checked == true && otherStructTimeFlag == 0)) {
                    animationText = "Loading structures";
                }
            }
            if (document.getElementById('Constraints').checked == true) {
                if ((document.getElementById('OwnedByMe').checked == true && myConstTimeFlag == 0) || (document.getElementById('OwnedByOtherOrganisations').checked == true && otherConstTimeFlag == 0)) {
                    if (animationText == "")
                        animationText = "Loading constraints";
                    else
                        animationText += " and constraints";
                }
            }
            if (animationText != "")
                animationText += ", please wait...";

            if (document.getElementById('Structs').checked == true) {
                if (document.getElementById('OwnedByMe').checked == true) {
                    if (myStructTimeFlag == 0) {
                        fetchStructBounds(0, boundsAndZoom);
                        setBounds(boundsAndZoom.bounds);
                    }
                    else {
                        if (myStructureArray != undefined) {
                            if (document.getElementById('Underbridge').checked == true) {
                                filterStructures(myStructureArray.underBridge, true, false, boundsAndZoom.zoom);
                            }
                            if (document.getElementById('Overbridge').checked == true) {
                                filterStructures(myStructureArray.overBridge, true, false, boundsAndZoom.zoom);
                            }
                            if (document.getElementById('UnderAndOverbridge').checked == true) {
                                filterStructures(myStructureArray.underAndOverBridge, true, false, boundsAndZoom.zoom);
                            }
                            if (document.getElementById('LevelCrossing').checked == true) {
                                filterStructures(myStructureArray.levelCrossing, true, false, boundsAndZoom.zoom);
                            }
                        }
                    }
                }
                else {
                    if (myStructTimeFlag == 1 && myStructureArray != undefined) {
                        if (document.getElementById('Underbridge').checked == true) {
                            filterStructures(myStructureArray.underBridge, false, false, boundsAndZoom.zoom);
                        }
                        if (document.getElementById('Overbridge').checked == true) {
                            filterStructures(myStructureArray.overBridge, false, false, boundsAndZoom.zoom);
                        }
                        if (document.getElementById('UnderAndOverbridge').checked == true) {
                            filterStructures(myStructureArray.underAndOverBridge, false, false, boundsAndZoom.zoom);
                        }
                        if (document.getElementById('LevelCrossing').checked == true) {
                            filterStructures(myStructureArray.levelCrossing, false, false, boundsAndZoom.zoom);
                        }
                    }
                }
                if (document.getElementById('OwnedByOtherOrganisations').checked == true) {
                    if (otherStructTimeFlag == 0) {
                        fetchStructBounds(1, boundsAndZoom);
                        setBounds(boundsAndZoom.bounds);
                    }
                    else {
                        if (otherStructureArray != undefined) {
                            if (document.getElementById('Underbridge').checked == true) {
                                filterStructures(otherStructureArray.underBridge, true, true, boundsAndZoom.zoom);
                            }
                            if (document.getElementById('Overbridge').checked == true) {
                                filterStructures(otherStructureArray.overBridge, true, true, boundsAndZoom.zoom);
                            }
                            if (document.getElementById('UnderAndOverbridge').checked == true) {
                                filterStructures(otherStructureArray.underAndOverBridge, true, true, boundsAndZoom.zoom);
                            }
                            if (document.getElementById('LevelCrossing').checked == true) {
                                filterStructures(otherStructureArray.levelCrossing, true, true, boundsAndZoom.zoom);
                            }
                        }
                    }
                }
                else {
                    if (otherStructTimeFlag == 1 && otherStructureArray != undefined) {
                        if (document.getElementById('Underbridge').checked == true) {
                            filterStructures(otherStructureArray.underBridge, false, true, boundsAndZoom.zoom);
                        }
                        if (document.getElementById('Overbridge').checked == true) {
                            filterStructures(otherStructureArray.overBridge, false, true, boundsAndZoom.zoom);
                        }
                        if (document.getElementById('UnderAndOverbridge').checked == true) {
                            filterStructures(otherStructureArray.underAndOverBridge, false, true, boundsAndZoom.zoom);
                        }
                        if (document.getElementById('LevelCrossing').checked == true) {
                            filterStructures(otherStructureArray.levelCrossing, false, true, boundsAndZoom.zoom);
                        }
                    }
                }
            }
            else {
                if (myStructureArray != undefined) {
                    filterStructures(myStructureArray.underBridge, false, false, boundsAndZoom.zoom);
                    filterStructures(myStructureArray.overBridge, false, false, boundsAndZoom.zoom);
                    filterStructures(myStructureArray.underAndOverBridge, false, false, boundsAndZoom.zoom);
                    filterStructures(myStructureArray.levelCrossing, false, false, boundsAndZoom.zoom);
                }
                if (otherStructureArray != undefined) {
                    filterStructures(otherStructureArray.underBridge, false, true, boundsAndZoom.zoom);
                    filterStructures(otherStructureArray.overBridge, false, true, boundsAndZoom.zoom);
                    filterStructures(otherStructureArray.underAndOverBridge, false, true, boundsAndZoom.zoom);
                    filterStructures(otherStructureArray.levelCrossing, false, true, boundsAndZoom.zoom);
                }
            }

            if (document.getElementById('Constraints').checked == true) {
                if (document.getElementById('OwnedByMe').checked == true) {
                    if (myConstTimeFlag == 0) {
                        fetchConstBounds(0, boundsAndZoom);
                        setBounds(boundsAndZoom.bounds);
                    }
                    else {
                        filterConstraints(true, false);
                    }
                }
                else {
                    if (myConstTimeFlag == 1)
                        filterConstraints(false, false);
                }
                if (document.getElementById('OwnedByOtherOrganisations').checked == true) {
                    if (otherConstTimeFlag == 0) {
                        fetchConstBounds(1, boundsAndZoom);
                        setBounds(boundsAndZoom.bounds);
                    }
                    else {
                        filterConstraints(true, true);
                    }
                }
                else {
                    if (otherConstTimeFlag == 1)
                        filterConstraints(false, true);
                }
            }
            else {
                if (myConstTimeFlag == 1)
                    filterConstraints(false, false);
                if (otherConstTimeFlag == 1)
                    filterConstraints(false, true);
            }
        }
    }

    function getTimeFlag() {
        return { myStruct: myStructTimeFlag, otherStruct: otherStructTimeFlag, myConst: myConstTimeFlag, otherConst: otherConstTimeFlag };
    }
    function openFilters() {
        document.getElementById("filters").style.width = "450px";
        $("#overlay").show();
        $("#overlay").css("background", "rgba(0, 0, 0, 0)");
        $("#overlay").css("z-index", "0");
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

    function closeFilters() {
        document.getElementById("filters").style.width = "0";
        document.getElementById("banner").style.filter = "unset"
        document.getElementById("navbar").style.filter = "unset";
        $("#overlay").hide();
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

    //  google-map-setting-start
    function initialize() {
        var myLatlng = new google.maps.LatLng(51.508742, -0.120850);
        var mapProp = {
            center: myLatlng,
            zoom: 5,
            mapTypeId: google.maps.MapTypeId.ROADMAP

        };
        var map = new google.maps.Map(document.getElementById("googleMap"), mapProp);
        document.getElementById('lat').value = 51.508742
        document.getElementById('lng').value = -0.120850
    }
    function BackClick(structId) {
        var url = sessionStorage.getItem("AuthorizeGeneralUrl");
        if (url != "null") {
            window.location.href = url;
        }
        else {
            window.location.href = "../Structures/ReviewSummary" + EncodedQueryString("structureId=" + structId);
        }

    };
    function viewStructuresConstraints() {
        if (document.getElementById('viewStructuresConstraints').style.display !== "none") {
            document.getElementById('viewStructuresConstraints').style.display = "none"
            document.getElementById('chevlon-up-icon1').style.display = "none"
            document.getElementById('chevlon-down-icon1').style.display = "block"
        }
        else {
            document.getElementById('viewStructuresConstraints').style.display = "block"
            document.getElementById('chevlon-up-icon1').style.display = "block"
            document.getElementById('chevlon-down-icon1').style.display = "none"
        }
    }

    // search boundaries in filter
    function viewBoundaries() {
        if (document.getElementById('viewBoundaries').style.display !== "none") {
            document.getElementById('viewBoundaries').style.display = "none"
            document.getElementById('chevlon-up-icon2').style.display = "none"
            document.getElementById('chevlon-down-icon2').style.display = "block"
        }
        else {
            document.getElementById('viewBoundaries').style.display = "block"
            document.getElementById('chevlon-up-icon2').style.display = "block"
            document.getElementById('chevlon-down-icon2').style.display = "none"
        }
    }

    // search roads in filter
    function viewRoads() {
        if (document.getElementById('viewroads').style.display !== "none") {
            document.getElementById('viewroads').style.display = "none"
            document.getElementById('chevlon-up-icon21').style.display = "none"
            document.getElementById('chevlon-down-icon21').style.display = "block"

        }
        else {
            document.getElementById('viewroads').style.display = "block"
            document.getElementById('chevlon-up-icon21').style.display = "block"
            document.getElementById('chevlon-down-icon21').style.display = "none"

        }
    }

    // search organisation in filter
    function viewAllstructures() {
        if (document.getElementById('viewAllstructures').style.display !== "none") {
            document.getElementById('viewAllstructures').style.display = "none"
            document.getElementById('chevlon-up-icon211').style.display = "none"
            document.getElementById('chevlon-down-icon211').style.display = "block"
        }
        else {
            document.getElementById('viewAllstructures').style.display = "block"
            document.getElementById('chevlon-up-icon211').style.display = "block"
            document.getElementById('chevlon-down-icon211').style.display = "none"
        }
    }

    //google.maps.event.addDomListener(window, 'load', initialize);
     //  google-map-setting-end
    function EnLargeMap() {

        if ($("#Minimzeicon").is(":visible")) { //mode changing to full screen
            $("body").addClass('mapClassl');
            $("#navbar").hide();
            $("#footer").hide();
            $("#Minimzeicon").hide();
            $("#MaxmizeIocn").show();
            $("#footerdiv").hide();
            $('body').css('overflow-y', 'hidden');
            scroll();

        } else { //mode changes to minimze
            $("body").removeClass('mapClassl');
            $("#navbar").show();
            $("#footer").show();
            $("#Minimzeicon").show();
            $("#MaxmizeIocn").hide();
            $("#footerdiv").show();
            $('body').css('overflow-y', 'scroll');
            $('map').css('height', '650px');
        }
    }
    function scroll() {
        document.body.scrollTop = 0;
        document.documentElement.scrollTop = 0;
    }
