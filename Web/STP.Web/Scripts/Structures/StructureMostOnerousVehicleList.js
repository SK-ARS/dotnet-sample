var structureIdVal = $('#hf_structureId').val();
var StructureNameVal = $('#hf_StructureName').val();
function StructureMostOnerousVehicleListInit() {
    structureIdVal = $('#hf_structureId').val();
    StructureNameVal = $('#hf_StructureName').val();
    SelectMenu(3);
    $("#filter-Section").load('../Structures/SearchMostOnerousVehiclePanel?structureId=' + structureIdVal + "&structureName=" + StructureNameVal,
        function () {
            SearchMostOnerousVehiclePanelInit();
        }
    );
}
$(document).ready(function () {
    $('body').on('click', '.showVehicleSummary', function (e) {
        e.preventDefault();
        ShowVehicleSummary(this);
    });
    $('body').on('click', '#btnBack', function (e) {
        e.preventDefault();
        ViewGeneralDetailsFn(this);
    });
    //$('#mostOnerousPaginator').on('click', 'a', function (e)
    $('body').on('click', '.bottom-pagination #mostOnerousPaginator a', function () {
       
        if (this.href == '') {
            return false;
        }
        else {
           // e.preventDefault();
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
});

function ShowVehicleSummaryDetails(esdalRef) {
    var randomNumber = Math.random();
    startAnimation();
    $("#viewDetails").load('../Application/ViewVehicleDetails?ESDALRef=' + encodeURIComponent(esdalRef) + '&random=' + randomNumber, function () {
        $('#vehicleDetails').modal('show');
        stopAnimation();
    });
}

function ShowVehicleSummary(e) {
    var ESDALRefNo = $(e).attr("esdalref");
    ShowVehicleSummaryDetails(ESDALRefNo);
}

function ViewGeneralDetailsFn() {
    ViewGeneralDetails('li1', 'generalSettingsId');
}
