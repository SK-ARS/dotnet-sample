        $(document).ready(function () {
            //removescroll();
            if (document.getElementById("filters") !=null) {
               $("#flexRadioDefault11").attr('checked', 'checked').trigger("click");
            }
            
            $("#map").html('');
            $("#map").load('@Url.Action("A2BPlanning", "Routes", new {routeID = 0})', function () {
                loadmap('DISPLAYONLY');

            });
            $('body').on('click', '#btnenlarge', EnLargeMap);
            $('body').on('change', '#gpxRoute', ViewGPXRoute);
            $('body').on('click', '#movementMapFilterIcon', openAuthorizeMovFilters);
        });

    function EnLargeMap() {
            if ($("#Minimzeicon").is(":visible")) { //mode changing to full screen
                $('#movementMapFilterIcon').show();
                $("#navbar").hide();
                $("#generalmap").hide();
                $("#listitem").hide();
                $("#printbutton").hide();
                $("#movement-details").hide();
                $("#footerdiv").hide();
                $("#Minimzeicon").hide();
                $("#MaxmizeIocn").show();
                $("#banner-container").addClass("bannercfulls");
                $("#container-sub").addClass("containerfulls");
                $("#helpdeskDelegation").addClass("helpdeskfulls");
                $("#movement-map").addClass("movementfulls");
                $("#map").addClass("mapfulls");
                $("#OpenLayers_Control_Panel_200").addClass("horizontalMapRoad");
                $("#btnmap").addClass("iconfulls");
                $("#helpdeskDelegation").removeClass("row");
                $("#banner-container").removeClass("container-fluid");
                $("#movement-map").removeClass("col-lg-6 col-md-12 col-sm-12 col-xs-12");
                mapResize();


            } else { //mode changes to minimze
                $('#movementMapFilterIcon').hide();
                $("#navbar").show();
                $("#Minimzeicon").show();
                $("#MaxmizeIocn").hide();
                $("#printbutton").show();
                $("#listitem").show();
                $("#movement-details").show();
                $("#map").removeClass("mapfulls");
                $("#container-sub").removeClass("containerfulls");
                $("#helpdeskDelegation").removeClass("helpdeskfulls");
                $("#movement-map").removeClass("movementfulls");
                $("#mvpmap").removeClass("mvpmapfulls");
                $("#OpenLayers_Control_Panel_200").removeClass("horizontalMapRoad");
                $("#banner-container").removeClass("bannercfulls");
                $("#helpdeskDelegation").addClass("row");
                $("#banner-container").addClass("container-fluid");
                $("#movement-map").addClass("col-lg-6 col-md-12 col-sm-12 col-xs-12");
                $("#overlay_load").addClass("col-lg-6 col-md-12 col-sm-12 col-xs-12");
                $("#btnmap").removeClass("iconfulls");
                $("#generalmap").show();
                $("#footerdiv").show();
            }
    }
    function openAuthorizeMovFilters() {

        document.getElementById("filters").style.width = "400px";
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
    function closeAuthorizeMovFilters() {
        document.getElementById("filters").style.width = "0";
        document.getElementById("banner").style.filter = "unset"
        document.getElementById("navbar").style.filter = "unset";
        $("#overlay").hide();
    }
    $('body').on('click', '#flexRadioDefault11', function (e) {
        var RouteId = $(this).data("RouteId");
        var RouteType = $(this).data("RouteType");
        var VIsSortPortal = $(this).data("VIsSortPortal");
      
        RouteView(RouteId, RouteType, VIsSortPortal);
    });
  
