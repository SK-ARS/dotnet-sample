    SelectMenu(1);
    $(document).ready(function () {
        var createAlertMsg = $('#CreateAlert').val();
        var SetPreference = $('#SetPreferenceMessage').val();
        if (createAlertMsg == "True") {
            ShowDialogWarningPop('Password changed successfully.', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
        }
        if (SetPreference == "True") {
            ShowDialogWarningPop('User preferences saved succesfully.', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');

        }

        var userTypeId = $('#userTypeId').val();
        var logged_in = $('#Logged_In').val();
        var msg = "";
        if (userTypeId == 696002)
           //soa/police
            var msgPolic = 'Dear ESDAL Users,<br/><br/>Once logged in you will now have access to the new updated ESDAL Maps. Please be aware that notifications submitted before 19 July 2021 may contain previous map data. The system will highlight these specific notifications which you can still process and respond to. Also, in the near future the ESDAL Helpdesk team will be contacting SOAs to obtain the latest structural information in their area. Please look out for any communications and provide structural details accordingly.<br/><br/>If you require any further information or assistance following the map update, please contact the ESDAL Helpdesk Team on 0300 470 3733, 8am - 6pm Monday - Friday (excluding bank holidays), email address: esdalenquiries@nationalhighways.co.uk”';
        if (logged_in != 1) {
            
            //ShowWarningPopupMapupgarde(msgPolic, "CloseBrokenPopUp");
            showWarningBrokenPopUpDialog(msgPolic, 'Ok', '', 'CloseBrokenPopUp', '', 1, 'info');
            $('#pop-warning').css('opacity', '1');
        }
    });

    function CloseBrokenPopUp() {
        CloseWarningPopup();
        $('#pop-warning').hide();
        $.ajax({
            type: "POST",
            url: "../Account/SetLoginStatus",
            data: {},
            success: function (result) {
                if (result == 1)
                    WarningCancelBtn();
            },
            error: function (xhr, status, error) {
                
            }
        });
    }
    $('.dropdown-toggle').dropdown();

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
    function showuserinfo() {
        if (document.getElementById('user-info').style.display !== "none") {
            document.getElementById('user-info').style.display = "none"
        }
        else {
            document.getElementById('user-info').style.display = "block";
            document.getElementsById('userdetails').style.overFlow = "scroll";
        }
    }
    function openFilters() {
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
    function closeNav() {
        document.getElementById("mySidenav").style.width = "0";
        document.getElementById("banner").style.filter = "unset"
        document.getElementById("navbar").style.filter = "unset";
    }
    function closeFilters() {
        document.getElementById("filters").style.width = "0";
        document.getElementById("banner").style.filter = "unset"
        document.getElementById("navbar").style.filter = "unset";

    }
    function viewDiscription() {
        if (document.getElementById('description').style.display !== "none") {
            document.getElementById('description').style.display = "none"
            document.getElementById('chevlon-up-icon').style.display = "none"
            document.getElementById('chevlon-down-icon').style.display = "block"
        }
        else {
            document.getElementById('description').style.display = "block"
            document.getElementById('chevlon-up-icon').style.display = "block"
            document.getElementById('chevlon-down-icon').style.display = "none"
        }
    }
                                 // Attach listener function on state changes
   
