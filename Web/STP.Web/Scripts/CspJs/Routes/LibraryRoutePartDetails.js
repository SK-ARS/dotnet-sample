        // $('.dropdown-toggle').dropdown();

        $(document).ready(function () {
            $("#SearchPanelForm").empty();
        });
        function myFunction(item) {
            if (item.style.backgroundColor === "lightgrey") {
                item.style.backgroundColor = "white";
            }
            else {
                item.style.backgroundColor = "lightgrey";
            }
        }
        function onremove() {
            if (document.getElementById("onfocus1").style.backgroundColor === "red" ||
                document.getElementById("onfocus2").style.backgroundColor === "red" ||
                document.getElementById("onfocus2").style.backgroundColor === "red") {
                document.getElementById("onfocus1").style.backgroundColor === "white"
                document.getElementById("onfocus2").style.backgroundColor === "white"
                document.getElementById("onfocus3").style.backgroundColor === "white"
            }
        }
        // showing user-setting-info-filter
        function openNav() {
            document.getElementById("mySidenav").style.width = "320px";
            document.getElementById("banner").style.filter = "brightness(0.5)";
            document.getElementById("banner").style.background = "white";
            document.getElementById("navbar").style.filter = "brightness(0.5)";
            document.getElementById("navbar").style.background = "white";
            function myFunction(x) {
                if (x.matches) { // If media query matches
                    document.getElementById("mySidenav").style.width = "200px";
                }
            }
            var x = window.matchMedia("(max-width: 992px)")
            myFunction(x) // Call listener function at run time
            x.addListener(myFunction)

        }

        function closeNav() {
            document.getElementById("mySidenav").style.width = "0";
            document.getElementById("banner").style.filter = "unset"
            document.getElementById("navbar").style.filter = "unset";
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

        // showing filter-settings
        function openRouteFilters() {
            if (document.getElementById("planRouteFilter") !== null) {
                $('.planRouteFilter').css("width", "450px");
            }
            else {
                document.getElementById("filters").style.width = "450px";
            }
            $("#overlay").show();
            $("#overlay").css("background", "rgba(0, 0, 0, 0)");
            $("#overlay").css("z-index", "0");
            document.getElementById("banner").style.filter = "brightness(0.5)";
            document.getElementById("banner").style.background = "white";
            document.getElementById("navbar").style.filter = "brightness(0.5)";
            document.getElementById("navbar").style.background = "white";
            function myFunction(x) {
                if (x.matches) { // If media query matches
                    
                    if (document.getElementById("planRouteFilter") !== null) {
                        $('.planRouteFilter').css("width", "200px");
                    }
                    else {
                        document.getElementById("filters").style.width = "200px";
                    }
                }
            }
            var x = window.matchMedia("(max-width: 770px)")
            myFunction(x) // Call listener function at run time
            x.addListener(myFunction)
        }
        function closeRouteFilters() {
            $("#overlay").hide();
            $("#overlay").css("z-index", "");
            if (document.getElementById("planRouteFilter") !== null) {
                $('.planRouteFilter').css("width", "0px");
                document.getElementById("planRouteFilter").style.filter = "unset";
            }
            else {
                document.getElementById("filters").style.width = "0";
            }
            //document.getElementById("banner").style.filter = "";
            document.getElementById("banner").style.filter = "unset"
            document.getElementById("navbar").style.filter = "unset";
       
        }
        //  on save in add-caution
        function onSave() {
            // if (document.getElementById('check-caution').style.display !== "none") {
            document.getElementById('check-caution').style.display = "block"
            document.getElementById('add-caution').style.display = "none"
        }
        // on save in check-caution
        function closecaution() {
            document.getElementById('check-caution').style.display = "none"
            document.getElementById('add-caution').style.display = "block"
        }

        function failed() {
            document.getElementById('add-caution').style.display = "block"
            document.getElementById('check-caution').style.display = "none"
        }
     
        function opennewNav() {
            document.getElementById("mySidenav1").style.width = "400px";
            document.getElementById("mySidenav1").style.display = "block"
            document.getElementById("route-content-1").style.display = "block"
            document.getElementById("route-content-2").style.display = "none"
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
        }

        function closenewNav() {
            document.getElementById("mySidenav1").style.width = "0";
            document.getElementById("route-content-1").style.display = "none"
            document.getElementById("card-swipe2").style.display = "none";
            document.getElementById("card-swipe1").style.display = "block";
        }
        function closeRoutenav() {
            document.getElementById("mySidenav1").style.width = "0";
            //document.getElementById("route-content-2").style.display = "none"
            // document.getElementById("esdaldclaimer").style.display = "none"
            document.getElementById("card-swipe2").style.display = "none";
            //document.getElementById("card-swipe1").style.display = "block";
        }
        // search structures/constraints in filter
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
        //search map features
        function viewMapFeatures() {
            if (document.getElementById('viewmapfeat').style.display !== "none") {
                document.getElementById('viewmapfeat').style.display = "none"
                document.getElementById('chevlon-up-icon212').style.display = "none"
                document.getElementById('chevlon-down-icon212').style.display = "block"

            }
            else {
                document.getElementById('viewmapfeat').style.display = "block"
                document.getElementById('chevlon-up-icon212').style.display = "block"
                document.getElementById('chevlon-down-icon212').style.display = "none"

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

        function Clearall() {
			startAnimation();
            document.getElementById('All').checked = false;
            document.getElementById('Affected').checked = false;
            $("#toggleAllAffected").prop('checked', true);
			document.getElementById('Structs').checked = false;
			document.getElementById('Underbridge').checked = false;
			document.getElementById('Overbridge').checked = false;
			document.getElementById('UnderAndOverbridge').checked = false;
			document.getElementById('LevelCrossing').checked = false;
			document.getElementById('Constraints').checked = false;
			document.getElementById('PoliceBoundaries').checked = false;
			document.getElementById('LABoundaries').checked = false;
			document.getElementById('NHBoundaries').checked = false;
			document.getElementById('TfLRoads').checked = false;
			document.getElementById('WelshTrunkRoads').checked = false;
            document.getElementById('ScottishTrunkRoads').checked = false;
            document.getElementById('ParkAndRec').checked = false;
            document.getElementById('TravelDest').checked = false;
            document.getElementById('TransHub').checked = false;
            document.getElementById('AutMob').checked = false;
            document.getElementById('EduIn').checked = false;
            document.getElementById('Shopping').checked = false;
            document.getElementById('CMSServ').checked = false;
            document.getElementById('BuisFeci').checked = false;
            document.getElementById('Entertainment').checked = false;
            document.getElementById('FiInst').checked = false;
            document.getElementById('Parkbay').checked = false;
            document.getElementById('Hospital').checked = false;
            document.getElementById('Restaurants').checked = false;

            document.getElementById('AILStructs').checked = false;
            NHBoundaries(false);
            PoliceBoundaries(false);
            LABoundaries(false);
            TfLRoads(false);
            ParkAndRec(false);
            TravelDest(false);
            TransHub(false);
            AutMob(false);
            EduIn(false);
            Shopping(false);
            CMSServ(false);
            BuisFeci(false);
            Entertainment(false);
            FiInst(false);
            Parkbay(false);
            Hospital(false);
            Restaurants(false);
			showStructBoundsA2B();
			stopAnimation();
        }

        function ApplyFilterMap() {
			startAnimation();
			if (document.getElementById('NHBoundaries').checked==true) {
				NHBoundaries(true);
			}
			else {
				NHBoundaries(false);
            }
			if (document.getElementById('PoliceBoundaries').checked==true) {
				PoliceBoundaries(true);
			}
			else {
				PoliceBoundaries(false);
            }
			if (document.getElementById('LABoundaries').checked==true) {
				LABoundaries(true);
			}
			else {
				LABoundaries(false);
            }
			if (document.getElementById('TfLRoads').checked==true) {
				TfLRoads(true);
			}
			else {
				TfLRoads(false);
            }
            showStructBoundsA2B();
			stopAnimation();
		}

    $(document).ready(function () {
        if (document.getElementById("planRouteFilter") !== null) {
            var filterData = $('#planRouteFilterDiv').html();
            $('#planRouteFilterDiv').html('');
            $('#planRouteFilter').html(filterData);
        }

        $("#IDEnLargeMap").on('click', EnLargeMap);
        $("#IDopenRouteFilters").on('click', openRouteFilters);
        $("#card-swipe1").on('click', openRoutenav);
        $("#card-swipe2").on('click', closeRoutenav);
        $("#deletepath").on('click', confirmRemovePath);
        $("#IDswap").on('click', swap);
        $("#IDClearall").on('click', Clearall);
        $("#IDviewMapFeatures").on('click', viewMapFeatures);
        $("#IDviewRoads").on('click', viewRoads);
        $("#IDviewBoundaries").on('click', viewBoundaries);
        $("#IDviewStructuresConstraints").on('click', viewStructuresConstraints);
        
            document.getElementById('All').checked = true;
            document.getElementById('Affected').checked = false;
            document.getElementById('Affected').disabled = true;
            $("#toggleAllAffected").prop('checked', true);
            $("#All").change();
            $("#Affected").change();
        if ('@Session["RouteFlag"]' != 2 && '@Session["RouteFlag"]' != 1 && '@Session["RouteFlag"]' != 3) {
            if ($('#PortalType').val() == 696001) {
                SelectMenu(4);
            }
            if ($('#PortalType').val() == 696008) {
                SelectMenu(2);
            }
            mapfromroutesmenu = true;
        }
        removescroll();
            $("#map").html('');

            $("#map").load('@Url.Action("A2BPlanning", "Routes", new {routeID = ViewBag.routeID})', function () {
            LoadLeftPanel();

        });
        $('#btn_back').click(function (e) {
           location.href = '@Url.Content("/Routes/RoutePartLibrary")';
        });

		$('#PoliceBoundaries').change(function () {
			startAnimation();
			if (this.checked) {
				PoliceBoundaries(true);
			}
			else {
				PoliceBoundaries(false);
			}
			stopAnimation();
		});

		$('#LABoundaries').change(function () {
			startAnimation();
			if (this.checked) {
				LABoundaries(true);
			}
			else {
				LABoundaries(false);
			}
			stopAnimation();
		});

		$('#NHBoundaries').change(function () {
			startAnimation();
			if (this.checked) {
				NHBoundaries(true);
			}
			else {
				NHBoundaries(false);
			}
			stopAnimation();
		});

		$('#TfLRoads').change(function () {
			startAnimation();
			if (this.checked) {
				TfLRoads(true);
			}
			else {
				TfLRoads(false);
			}
			stopAnimation();
		});
        $('#Restaurants').change(function () {
            startAnimation();
            if (this.checked) {
                Restaurants(true);
            }
            else {
                Restaurants(false);
            }
            stopAnimation();
        });
        $('#Hospital').change(function () {
            startAnimation();
            if (this.checked) {
                Hospital(true);
            }
            else {
                Hospital(false);
            }
            stopAnimation();
        });
        $('#Parkbay').change(function () {
            startAnimation();
            if (this.checked) {
                Parkbay(true);
            }
            else {
                Parkbay(false);
            }
            stopAnimation();
        });
        $('#FiInst').change(function () {
            startAnimation();
            if (this.checked) {
                FiInst(true);
            }
            else {
                FiInst(false);
            }
            stopAnimation();
        });
        $('#Entertainment').change(function () {
            startAnimation();
            if (this.checked) {
                Entertainment(true);
            }
            else {
                Entertainment(false);
            }
            stopAnimation();
        });
        $('#BuisFeci').change(function () {
            startAnimation();
            if (this.checked) {
                BuisFeci(true);
            }
            else {
                BuisFeci(false);
            }
            stopAnimation();
        });
        $('#CMSServ').change(function () {
            startAnimation();
            if (this.checked) {
                CMSServ(true);
            }
            else {
                CMSServ(false);
            }
            stopAnimation();
        });
        $('#Shopping').change(function () {
            startAnimation();
            if (this.checked) {
                Shopping(true);
            }
            else {
                Shopping(false);
            }
            stopAnimation();
        });
        $('#EduIn').change(function () {
            startAnimation();
            if (this.checked) {
                EduIn(true);
            }
            else {
                EduIn(false);
            }
            stopAnimation();
        });
        $('#AutMob').change(function () {
            startAnimation();
            if (this.checked) {
                AutMob(true);
            }
            else {
                AutMob(false);
            }
            stopAnimation();
        });
        $('#TransHub').change(function () {
            startAnimation();
            if (this.checked) {
                TransHub(true);
            }
            else {
                TransHub(false);
            }
            stopAnimation();
        });
        $('#TravelDest').change(function () {
            startAnimation();
            if (this.checked) {
                TravelDest(true);
            }
            else {
                TravelDest(false);
            }
            stopAnimation();
        });
        $('#ParkAndRec').change(function () {
            startAnimation();
            if (this.checked) {
                ParkAndRec(true);
            }
            else {
                ParkAndRec(false);
            }
            stopAnimation();
        });
    });
    function ToggleAllAffected() {
        if ($("#toggleAllAffected").prop('checked') == true) {
            $('#toggleAllAffected').val(true);
            document.getElementById('All').checked = true;
            document.getElementById('Affected').checked = false;
            $("#All").change();
            $("#Affected").change();

        }
        else if ($("#toggleAllAffected").prop('checked') == false ) {
            if (document.getElementById('Affected').disabled == false) {
                $('#toggleAllAffected').val(false);
                document.getElementById('Affected').checked = true;
                document.getElementById('All').checked = false;
                $("#All").change();
                $("#Affected").change();
            }
            else {
                $("#toggleAllAffected").prop('checked', true);
                $('#toggleAllAffected').val(true);
                document.getElementById('All').checked = true;
                document.getElementById('Affected').checked = false;
                $("#All").change();
                $("#Affected").change();
            }
        }
    }
    function LoadLeftPanel() {
        var retlegflag = false;
if($('#hf_ShowReturnLeg').val() ==  2)
        { retlegflag = true; }
        var url = '@Url.Action("A2BLeftPanel", "Routes", new { routeID = ViewBag.routeID, val=ViewBag.PageFlag})';
        $.ajax({
            url: url,
            type: 'POST',
            data:{ShowReturnLegFlag:retlegflag},
            beforeSend: function () {
                startAnimation();
            },
            success: function (page) {
                $('#leftpanel').html(page);
                $('#leftpanel').show();
if($('#hf_ShowReturnLeg').val() ==  1) {
                    $('#ReturnLegForNotif').hide();

                }
                else {
                    $('#ReturnLegForNotif').show();
                }
                CheckSessionTimeOut();
                openRoutenav();
            },
            complete: function () {
                stopAnimation();
                if (typeof $("#hIs_NEN").val() != "undefined" && $("#hIs_NEN").val() == "true"){
                    var tableis = document.getElementById("tabA2Bpanel");
                tableis.style.marginTop = '10px';}
            }
        });
    }

    function EnLargeMap() {
        var url = geturl(location.href);
        if (url == 'SORTApplicationSORTListMovemnets') {
            EnLargeMapCandidate();
        }
        else if(url == 'NENNotificationNE_Notification') {
            NENRouteFullScreen();
        }
        else {
            EnLargeMapNotification();
        }

    }

    function NENRouteFullScreen() {
        if ($("#Minimzeicon").is(":visible")) { //mode changing to full screen
            $("#navbar").hide();
            $("#Minimzeicon").hide();
            $("#MaxmizeIocn").show();
            $("#banner-container").removeClass("container-fluid");
            $("#banner").removeClass("vehicle-setting");
            $("#vehicles").removeClass("flowComponent");
            $("#vehicles").removeClass("vehicles");
            document.getElementById('vehicles').id = 'veh';
            document.getElementById('banner-container').id = 'div';
            $("#footerdiv").hide();
            $("#back_btn").hide();
            $("#progressBar").hide();
            $("#RoteHeading").hide();
            $("#sort-menu-list").hide();
            $("#NEHeading").hide();
            $("#main-sort-content").removeClass("main-sort-content");
            $("#main-sort-content").addClass("nenfulls");
            $('body').css('overflow-y', 'hidden');
            $(".tableHead").hide()
            scroll();


        } else { //mode changes to minimze
            //closeFullscreen();
            $("#navbar").show();
            $("#Minimzeicon").show();
            $("#MaxmizeIocn").hide();
            $("#banner-container").addClass("container-fluid");
            $("#banner").addClass("vehicle-setting");
            $("#vehicles").addClass("flowComponent");
            $("#progressBar").show();
            $("#footerdiv").show();
            $("#back_btn").show();
            $("#RoteHeading").show();
            $("#sort-menu-list").show();
            $("#NEHeading").show();
            document.getElementById('veh').id = 'vehicles';
            document.getElementById('div').id = 'banner-container';
            $('body').css('overflow-y', 'scroll');
            $('map').css('height', '650px');
            $("#main-sort-content").addClass("main-sort-content");
            $("#main-sort-content").removeClass("nenfulls");
            $(".tableHead").show()

        }

    }

    function scroll() {
        document.body.scrollTop = 0;
        document.documentElement.scrollTop = 0;
    }
    function EnLargeMapCandidate() {

        if ($("#Minimzeicon").is(":visible")) { //mode changing to full screen  
          
            $("#navbar").hide();
            $("#Minimzeicon").hide();
            $("#MaxmizeIocn").show();
            $("#banner-container").hide();
            $("#banner").hide();
            $("#back_btn_Rt").hide();
            $("#sort-flow").hide();
            $("#progressBar").hide();
            $("#RoteHeading").hide();
            $("#vehicles").show();
            $("#backbutton").hide();
            $("#footerdiv").hide();
            $("#banner-container").removeClass("container-fluid");
            $("#banner1").removeClass("map-icons");
            $("#banner-container").addClass("bannercfulls");
            $("#banner1").addClass("banner1");
            $('body').css('overflow-y', 'hidden');
            scroll();
            mapResize();
        } else { //mode changes to minimze
          
            $("#navbar").show();
            $("#Minimzeicon").show();
            $("#MaxmizeIocn").hide();
            $("#banner-container").show();
            $("#banner").show();
            $("#progressBar").show();
            $("#backbutton").show();
            $("#banner-container").addClass("container-fluid");
            $("#banner1").addClass("map-icons");
            $("#banner-container").removeClass("bannercfulls");
            $("#banner1").removeClass("banner1");
            $("#footerdiv").show();
            $("#back_btn_Rt").show();
            $("#sort-flow").show();
            $("#RoteHeading").show();
            $('body').css('overflow-y', 'scroll');
        }
    }

    function EnLargeMapNotification() {

        if ($("#Minimzeicon").is(":visible")) { //mode changing to full screen
            $("body").addClass('mapClassl');
            $("#navbar").hide();
            $("#footer").hide();
            $("#back_btn_Altered").hide();
            $("#RoteHeading").hide();
            $("#Minimzeicon").hide();
            $("#MaxmizeIocn").show();
            $("#banner-container").removeClass("container-fluid");
            $("#banner").removeClass("vehicle-setting");
            $("#vehicles").removeClass("flowComponent");
            $("#vehicles").removeClass("vehicles");
            document.getElementById('vehicles').id = 'veh';
            document.getElementById('banner-container').id = 'div';
            $("#footerdiv").hide();
            $("#back_btn").hide();
            $("#progressBar").hide();
            $("#RoteHeading").hide();
            $('body').css('overflow-y', 'hidden');
            $("#sort-menu-list").hide();
            $("#title").hide();
            scroll();


        } else { //mode changes to minimze
            //closeFullscreen();
            $("body").removeClass('mapClassl');
            $("#navbar").show();
            $("#footer").show();
            $("#back_btn_Altered").show();
            $("#RoteHeading").show();
            $("#Minimzeicon").show();
            $("#MaxmizeIocn").hide();
            $("#banner-container").addClass("container-fluid");
            //$("#banner").addClass("vehicle-setting");
            $("#vehicles").addClass("flowComponent");
            $("#progressBar").show();
            $("#footerdiv").show();
            //$("#back_btn").show();
            $("#RoteHeading").show();
            document.getElementById('veh').id = 'vehicles';
            document.getElementById('div').id = 'banner-container';
            $('body').css('overflow-y', 'scroll');
            $('map').css('height', '650px');
            $("#sort-menu-list").show();
            $("#title").show();
        }
    }



