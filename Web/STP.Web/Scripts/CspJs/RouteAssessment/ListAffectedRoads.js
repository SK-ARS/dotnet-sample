    {
        var id = 0;
        var RoutePartId = 0;
        $(document).ready(function () {
            selectedmenu('Movements'); // for selected menu
            var Org_name = $('#Organisation_Name').val();
            if (Org_name != "" && Org_name != undefined) {
                $('#div_mylistaffroads').show();
                $('#Spn_OrgName_Roads').show();
                $('#Spn_OrgName_Roads').text(Org_name);
               
                if ($('#roaddiv1 td').length > 0) {
                   
                    $('#NoAffectedRoads').hide();
                }
                else {
                    $('#NoAffectedRoads').show();
                }
            }
        });
        $('#flexRadioDefault11').click(function () {
            $('#RoadsRoute').css('display', 'block');
        });
    }
