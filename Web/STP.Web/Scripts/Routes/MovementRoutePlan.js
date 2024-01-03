var RouteFlag = $('#hf_RouteFlag').val();
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
    if (RouteFlag != 2 && RouteFlag != 1 && RouteFlag != 3) {
        mapfromroutesmenu = true;
    }
    removescroll();
    $("#map").html('');

    $("#map").load('../Routes/A2BPlanning?routeID=123', function () {
        LoadLeftPanel();
    });
    $('#btn_back').click(function (e) {
        location.href = '../Routes/RoutePartLibrary';
    });
$('body').on('click','#card-swipe1', function(e) { 
  e.preventDefault();
  opennewNav(this);
}); 
$('body').on('click','#card-swipe2', function(e) { 
  e.preventDefault();
  closenewNav(this);
}); 
});
function LoadLeftPanel() {
    var retlegflag = false;
    var url = '../Routes/A2BLeftPanel?routeID=123&val=""';
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
            if ($('#hf_ShowReturnLeg').val() == 1 || $('#hf_ShowReturnLeg1').val() == '1') {
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

