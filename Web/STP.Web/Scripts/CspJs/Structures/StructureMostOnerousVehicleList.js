    $(document).ready(function () {
        SelectMenu(3);
        $("#filter-Section").load('../Structures/SearchMostOnerousVehiclePanel?structureId=' + '@ViewBag.structureId' + "&structureName=" + '@ViewBag.StructureName',
            function () {

            }
        );

        $("#openFilters").on('click', openFilters);
        $("#showVehicleSummary").on('click', ShowVehicleSummary);
        $("#btnBack").on('click', ViewGeneralDetailsFn);
        
    });
    $('#mostOnerousPaginator').on('click', 'a', function (e) {
        if (this.href == '') {
            return false;
        }
        else {
            startAnimation();
            $.ajax({
                url: this.href,
                type: 'GET',
                cache: false,
                success: function (result) {
                    $('#generalSettingsId').html(result);
                    $('#mostOnerousId').show();
                    stopAnimation();
                }
            });
            return false;
        }
    });
    function openFilters() {
        document.getElementById("filters").style.width = "500px";
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
    function ShowVehicleSummaryDetails(esdalRef) {
        var randomNumber = Math.random();
        startAnimation();
        $("#viewDetails").load('../Application/ViewVehicleDetails?ESDALRef=' + encodeURIComponent(esdalRef) + '&random=' + randomNumber, function () {
            $('#vehicleDetails').modal('show');
            stopAnimation();
        });
    }

    function ShowVehicleSummary(e) {
        var ESDALRefNo = e.currentTarget.dataset.ESDALRef;
        ShowVehicleSummaryDetails(ESDALRefNo);
    }

    function ViewGeneralDetailsFn() {
        ViewGeneralDetails('li1', 'generalSettingsId');
    }
