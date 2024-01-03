    $(document).ready(function () {
        
        $("#clfilter").on('click', closeFilters);
        $("#table-head").on('click', viewDetails);
        $("#clr").on('click', Clear);
        $("#srchbtn").on('click', StoreMostOnerousFilterData);
        $("#StartDateFlag").click(function () {
            if ($(this).is(":checked") == false) {
                $('#MovementStartDate').attr("disabled", true);
                $('#MovementStartDate').val('');
            }
            else {
                $('#MovementStartDate').attr("disabled", false);
            }
        });

        $("#EndDateFlag").click(function () {
            if ($(this).is(":checked") == false) {
                $('#MovementEndDate').attr("disabled", true);
                $('#MovementEndDate').val('');
            }
            else {
                $('#MovementEndDate').attr("disabled", false);
            }
        });



    });
    function viewDetails() {
        if (document.getElementById('viewFilterDetails').style.display !== "none") {
            document.getElementById('viewFilterDetails').style.display = "none"
            document.getElementById('chevlon-up-icon').style.display = "none"
            document.getElementById('chevlon-down-icon').style.display = "block"
        }
        else {
            document.getElementById('viewFilterDetails').style.display = "block"
            document.getElementById('chevlon-up-icon').style.display = "block"
            document.getElementById('chevlon-down-icon').style.display = "none"
        }
    }
    function closeFilters() {
        document.getElementById("filters").style.width = "0";
        document.getElementById("banner").style.filter = "unset"
        document.getElementById("navbar").style.filter = "unset";

    }
    function StoreMostOnerousFilterData() {
        $.ajax({
            url: '../Structures/StoreMostOnerousFilterData',
            dataType: 'json',
            type: 'POST',
            data: $("#SearchPanelForm").serialize(),
            success: function (result) {
                if (result == true) {

                    LoadResult();
                }

            },
            error: function (xhr, status) {
            }
        });
    }
    function LoadResult(id, idDiv) {
        startAnimation();
        closeFilters();
        $("#generalSettingsId").load('../Structures/StructureMostOnerousVehicleList?PageStatus=true&structureId=' + '@ViewBag.structureId' + "&structureName=" + '@ViewBag.structureName',
            function () {
                $('#mostOnerousId').show();
                stopAnimation();
            }
        );
    };
    function Clear() {
        $('#StartDateFlag').prop('checked', false);
        $('#MovementStartDate').attr("disabled", true);
        $('#MovementStartDate').val('');
        $('#EndDateFlag').prop('checked', false);
        $('#MovementEndDate').attr("disabled", true);
        $('#MovementEndDate').val('');
        $("#DDsearchCriteria").prop('selectedIndex', 0);
        $("#statusSearchCriteria").prop('selectedIndex', 0);
        StoreMostOnerousFilterData();
    }
