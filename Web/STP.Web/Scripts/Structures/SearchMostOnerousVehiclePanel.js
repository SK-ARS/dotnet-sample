var structureIdVal = $('#hf_structureId').val();
var structureNameVal = $('#hf_structureName').val();

function SearchMostOnerousVehiclePanelInit() {
    structureIdVal = $('#hf_structureId').val();
    structureNameVal = $('#hf_structureName').val();
}

$(document).ready(function () {
    $('body').on('click', '#clfilter', function (e) {
        e.preventDefault();
        closeFilters(this);
    });
    $('body').on('click', '#table-head', function (e) {
        e.preventDefault();
        viewDetails(this);
    });
    $('body').on('click', '#clr', function (e) {
        e.preventDefault();
        Clear(this);
    });
    $('body').on('click', '#srchbtn', function (e) {
        e.preventDefault();
        StoreMostOnerousFilterData(this);
    });
    $('body').on('click', '#StartDateFlag', function (e) {
        if ($(this).is(":checked") == false) {
            $('#MovementStartDate').attr("disabled", true);
            $('#MovementStartDate').val('');
        }
        else {
            $('#MovementStartDate').attr("disabled", false);
        
           

            $('#MovementStartDate').datepicker({
                dateFormat: 'dd/mm/yy',
                numberOfMonths: 1,
                changeMonth: true, changeYear: true,
               // minDate: new Date(),
                onSelect: function (selected) {
                    var startDate = selected.split("-").reverse().join("/");
                    var splitDate = selected.split("/");
                    var fromDate = new Date(splitDate[2], splitDate[1] - 1, splitDate[0]);
                    var toDate = $("#MovementEndDate").datepicker('getDate');
                    if (fromDate > toDate && toDate != null) {
                        $("#MovementEndDate").datepicker("setDate", fromDate);
                    }
                },
           
            });
        }
    });
    $('body').on('click', '#EndDateFlag', function (e) {
        if ($(this).is(":checked") == false) {
            $('#MovementEndDate').attr("disabled", true);
            $('#MovementEndDate').val('');
        }
        else {
            $('#MovementEndDate').attr("disabled", false);
        
           
            $('#MovementEndDate').datepicker({
                dateFormat: 'dd/mm/yy',
                numberOfMonths: 1,
                changeMonth: true, changeYear: true,
                //minDate: new Date(),
                beforeShow: function (textbox, instance) {
                    var startDate = $("#MovementStartDate").datepicker('getDate');
                    $("#MovementEndDate").datepicker("option", "minDate", startDate);
                    var rect = textbox.getBoundingClientRect();
                    setTimeout(function () {
                        var scrollTop = $("body").scrollTop();
                        instance.dpDiv.css({ top: rect.top + textbox.offsetHeight + scrollTop });
                    }, 0);
                }
            });
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
    $("#generalSettingsId").load('../Structures/StructureMostOnerousVehicleList?PageStatus=true&structureId=' + structureIdVal + "&structureName=" + structureNameVal,
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
