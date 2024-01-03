            //$('.dropdown-toggle').dropdown();

            //function myFunction(item) {
            //    if (item.style.backgroundColor === "lightgrey") {
            //        item.style.backgroundColor = "white";
            //    }
            //    else {
            //        item.style.backgroundColor = "lightgrey";
            //    }
            //}
            //function onremove() {
            //    if (document.getElementById("onfocus1").style.backgroundColor === "red" ||
            //        document.getElementById("onfocus2").style.backgroundColor === "red" ||
            //        document.getElementById("onfocus2").style.backgroundColor === "red") {
            //        document.getElementById("onfocus1").style.backgroundColor === "white"
            //        document.getElementById("onfocus2").style.backgroundColor === "white"
            //        document.getElementById("onfocus3").style.backgroundColor === "white"
            //    }
            //}
            //// showing user-setting-info-filter
            //function openNav() {
            //    document.getElementById("mySidenav").style.width = "320px";
            //    document.getElementById("banner").style.filter = "brightness(0.5)";
            //    document.getElementById("banner").style.background = "white";
            //    document.getElementById("navbar").style.filter = "brightness(0.5)";
            //    document.getElementById("navbar").style.background = "white";
            //    function myFunction(x) {
            //        if (x.matches) { // If media query matches
            //            document.getElementById("mySidenav").style.width = "200px";
            //        }
            //    }
            //    var x = window.matchMedia("(max-width: 992px)")
            //    myFunction(x) // Call listener function at run time
            //    x.addListener(myFunction)

            //}

            //function closeNav() {
            //    document.getElementById("mySidenav").style.width = "0";
            //    document.getElementById("banner").style.filter = "unset"
            //    document.getElementById("navbar").style.filter = "unset";
            //}

            //// showing user-setting inside vertical menu
            //function showuserinfo() {
            //    if (document.getElementById('user-info').style.display !== "none") {
            //        document.getElementById('user-info').style.display = "none"
            //    }
            //    else {
            //        document.getElementById('user-info').style.display = "block";
            //        document.getElementsById('userdetails').style.overFlow = "scroll";
            //    }
            //}

            //// showing filter-settings
            //function openFilters() {
            //    document.getElementById("filters").style.width = "400px";
            //    document.getElementById("banner").style.filter = "brightness(0.5)";
            //    document.getElementById("banner").style.background = "white";
            //    document.getElementById("navbar").style.filter = "brightness(0.5)";
            //    document.getElementById("navbar").style.background = "white";
            //    function myFunction(x) {
            //        if (x.matches) { // If media query matches
            //            document.getElementById("filters").style.width = "200px";
            //        }
            //    }
            //    var x = window.matchMedia("(max-width: 770px)")
            //    myFunction(x) // Call listener function at run time
            //    x.addListener(myFunction)
            //}
            //function closeFilters() {
            //    document.getElementById("filters").style.width = "0";
            //    document.getElementById("banner").style.filter = "unset"
            //    document.getElementById("navbar").style.filter = "unset";

            //}
            //  on save in add-caution
            //function onSave() {
            //    // if (document.getElementById('check-caution').style.display !== "none") {
            //    document.getElementById('check-caution').style.display = "block"
            //    document.getElementById('add-caution').style.display = "none"
            //}
            //// on save in check-caution
            //function closecaution() {
            //    document.getElementById('check-caution').style.display = "none"
            //    document.getElementById('add-caution').style.display = "block"
            //}

            //function failed() {
            //    document.getElementById('add-caution').style.display = "block"
            //    document.getElementById('check-caution').style.display = "none"
            //}



            ////  google-map-setting-start
            //function initialize() {
            //    const map = new google.maps.Map(document.getElementById("googleMap"), {
            //        zoom: 5,
            //        center: { lat: 24.886, lng: -70.268 },
            //        mapTypeId: "terrain",
            //    });
            //    // sample example to Define the LatLng coordinates for the polygon's path.
            //    const triangleCoords = [
            //        { lat: 25.774, lng: -80.19 },
            //        { lat: 18.466, lng: -66.118 },
            //        { lat: 32.321, lng: -64.757 },
            //    ];
            //    // Construct the polygon.
            //    const bermudaTriangle = new google.maps.Polygon({
            //        paths: triangleCoords,
            //        strokeColor: "#FF0000",
            //        strokeOpacity: 0.8,
            //        strokeWeight: 2,
            //        fillColor: "#FF0000",
            //        fillOpacity: 0.35,
            //    });

            //    bermudaTriangle.setMap(map);
            //}
            //google.maps.event.addDomListener(window, 'load', initialize);
            ////  google-map-setting-end
            //$('#exampleModalCenter1').on('shown.bs.modal', function () {
            //    //
            //    $('.modal-backdrop').remove();
            //})
            //$('#exampleModalCenter2').on('shown.bs.modal', function () {
            //    //
            //    $('.modal-backdrop').remove();
            //})
            //function opennewNav() {
            //    document.getElementById("mySidenav1").style.width = "350px";
            //    document.getElementById("mySidenav1").style.display = "block"
            //    document.getElementById("route-content-1").style.display = "block"
            //    document.getElementById("card-swipe1").style.display = "block"
            //    document.getElementById("card-swipe2").style.display = "block"
            //    function myFunction(x) {
            //        if (x.matches) { // If media query matches
            //            document.getElementById("mySidenav1").style.width = "auto";
            //        }
            //    }
            //    var x = window.matchMedia("(max-width: 410px)")
            //    myFunction(x) // Call listener function at run time
            //    x.addListener(myFunction)
            //}
            function openRoutenav() {
                document.getElementById("mySidenav1").style.width = "350px";
                document.getElementById("mySidenav1").style.display = "block"
                document.getElementById("route-content-2").style.display = "block"
                document.getElementById("route-content-1").style.display = "none"
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
                document.getElementById("route-content-2").style.display = "none"
                document.getElementById("card-swipe2").style.display = "none";
                document.getElementById("card-swipe1").style.display = "block";
            }
            //function viewContactDetails() {
            //    document.getElementById("exampleModalCenter1").style.display = "none";
            //    document.getElementById("exampleModalCenter2").style.display = "block";
            //}
            //function closeContactdetails() {
            //    document.getElementById("exampleModalCenter2").style.display = "none";
            //}

            // Attach listener function on state changes
    $(document).ready(function () {
        if ('@Session["RouteFlag"]' != 2 && '@Session["RouteFlag"]' != 1 && '@Session["RouteFlag"]' != 3) {
            mapfromroutesmenu = true;
        }
        removescroll();
        $("#map").html('');

        $("#map").load('@Url.Action("A2BPlanning", "Routes", new {routeID = 123})', function () {
            LoadLeftPanel();
        });
        $('#btn_back').click(function (e) {
            location.href = '@Url.Content("/Routes/RoutePartLibrary")';
        });
        $("#card-swipe1").on('click', opennewNav);
        $("#card-swipe2").on('click', closenewNav);
    });
    function LoadLeftPanel() {
        var retlegflag = false;
        var url = '@Url.Action("A2BLeftPanel", "Routes", new { routeID = 123, val=""})';
        $.ajax({
            url: url,
            type: 'POST',
            data: { ShowReturnLegFlag: retlegflag },
            beforeSend: function () {
                startAnimation();
            },
            success: function (page) {
                $('#leftpanel').html(page);
                $('#leftpanel').show();
if($('#hf_ShowReturnLeg').val() ==  1 || $('#hf_ShowReturnLeg1').val() ==  '1') {
                    $('#ReturnLegForNotif').hide();
                }
                else {
                    $('#ReturnLegForNotif').show();
                }
                CheckSessionTimeOut();
            },
            complete: function () {
                stopAnimation();
                if (typeof $("#hIs_NEN").val() != "undefined" && $("#hIs_NEN").val() == "true") {
                    var tableis = document.getElementById("tabA2Bpanel");
                    tableis.style.marginTop = '10px';
                }
            }
        });
    }

