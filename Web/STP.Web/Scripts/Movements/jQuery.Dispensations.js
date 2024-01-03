function viewdispensation() {
    if (document.getElementById('viewdispensation').style.display !== "none") {
        document.getElementById('viewdispensation').style.display = "none"
        document.getElementById('chevlon-up-icon1').style.display = "none"
        document.getElementById('chevlon-down-icon1').style.display = "block"
    }
    else {
        document.getElementById('viewdispensation').style.display = "block"
        document.getElementById('chevlon-up-icon1').style.display = "block"
        document.getElementById('chevlon-down-icon1').style.display = "none"
    }
}
function clearSearchListDispenstaion() {
    $(':input', '#filters')
        .not(':button, :submit, :reset, :hidden')
        .val('')
        .removeAttr('selected')
        .removeAttr('checked');
    $('input:checked').each(function () {
        $(this).removeAttr('checked');
    });
    SearchDispensationList();
}

function SearchDispensationList(isSort = false,pageNum = 1) {
    var dispensationSearchItems = {};
    let isExpired = false;
    $('input:checked').each(function () {
        isExpired = $(this).attr("value");
    });
    dispensationSearchItems.Expired = isExpired;
    dispensationSearchItems.Criteria = $("#txtCriteria").val();
    dispensationSearchItems.SearchType = $('#DDsearchCriteria option:selected').val();
    dispensationSearchItems.SearchName = $('#DDsearchCriteria option:selected').text();
    dispensationSearchItems.SortOrderValue = $('#SortOrderValue').val();
    dispensationSearchItems.SortTypeValue = $('#SortTypeValue').val();

    var granterId = $('#GranterID').val();
    var hidelayout = $('#hideLayout').val();
    closeFilters();
    $.ajax({
        url: '../Dispensation/SetDispensationSearch',
        type: 'POST',
        cache: false,
        data: {
            objDispSearch: dispensationSearchItems, granterId: granterId, hideLayout: hidelayout, page: pageNum, pageSize: $('#pageSizeVal').val()
        },
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {
            $("#manage-dispensations").html(response);
            //$("#dispensationBody").html(response);
        },

        error: function (xhr, textStatus, errorThrown) {
            //location.reload();
        },
        complete: function () {

            stopAnimation();

        }
    });
}

function CreateDispensations() {
    var url = "../Dispensation/CreateDispensation";
    window.location.href = url;
}

function DisplayVehicleRestrication() {
    $.ajax({
        url: '../Dispensation/VehicleRestricationDetails',
        data: {},
        type: 'GET',
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
            $("#vehicle-restrication-details").show();
            $("#vehicle-restrication-details").html('');
            $("#vehicle-restrication-details").html(result);



            var grossval = $('#lblGross').text();
            if ((grossval != "" && grossval != "0")) {
                $('#Gross_text').val(grossval.trim());
            }
            else {
                $('#Gross_text').val('');
            }
            var axleval = $('#lblAxle').text();
            if ((axleval != "" && axleval != "0")) {
                $('#Axle_text').val(axleval.trim());
            }
            else {
                $('#Axle_text').val('');
            }
            var widthval = $('#lblWidth').text();
            if ((widthval != "" && widthval != "0")) {
                $('#Width_text').val(widthval.trim());
            }
            else {
                $('#Width_text').val('');
            }
            var lengthval = $('#lblLength').text();
            if ((lengthval != "" && lengthval != "0")) {
                $('#Length_text').val(lengthval.trim());
            }
            else {
                $('#Length_text').val('');
            }
            var heightval = $('#lblHeight').text();
            if ((heightval != "" && heightval != "0")) {
                $('#Height_text').val(heightval.trim());
            }
            else {
                $('#Height_text').val('');
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            location.reload();
        },
        complete: function () {
            stopAnimation();
        }
    });

}

var dispId;
var DRefNo;
function Delete(DispensationId, DispensationRef) {
    dispId = DispensationId;
    DRefNo = DispensationRef;
    var Msg = "Do you want to delete '" + "" + "'" + DispensationRef + "'" + "" + "' ?";
    ShowWarningPopup(Msg, 'DeleteDispensation');
}
function DeleteDispensation() {
    CloseWarningPopup();
    $.ajax({
        url: '../Dispensation/DeleteDispensation',
        type: 'POST',
        cache: false,
        data: { dispId: dispId },
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
            if (result.Success) {
                ShowSuccessModalPopup('Dispensation reference number  "' + DRefNo + '"  deleted successfully', 'ReloadLocation');
            }
            else {
                ShowErrorPopup("Deletion failed");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            ShowErrorPopup("Deletion failed");
            location.reload();
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function ReloadLocation() {
    CloseSuccessModalPopup();
    location.reload();

}
var flag = 0;
var notifFlag = $('#hdnNotifFlag').val() ? $('#hdnNotifFlag').val() : 0;
function SaveVehicleRestrications() {

    var gross = $('#Gross_text').val();
    var axle = $('#Axle_text').val();
    var length = $('#Length_text').val();
    var width = $('#Width_text').val();
    var height = $('#Height_text').val();
    if ((gross == "" || gross == "0") && (axle == "" || axle == "0") && (length == "" || length == "0") && (width == "" || width == "0") && (height == "" || height == "0")) {
        $('#div_nonespecified').show();
        $('#div_gross').hide();
        $('#div_axle').hide();
        $('#div_length').hide();
        $('#div_width').hide();
        $('#div_height').hide();
    }
    else {
        $('#div_nonespecified').hide();
        if ((gross != "" && gross != "0")) {
            $('#lblGross').text(gross);
            $('#Gross').val(gross);
            $('#div_gross').show();
        }
        if ((axle != "" && axle != "0")) {
            $('#lblAxle').text(axle);
            $('#Axle').val(axle);
            $('#div_axle').show();
        }
        if ((length != "" && length != "0")) {
            $('#lblLength').text(length);
            $('#Length').val(length);
            $('#div_length').show();
        }
        if ((width != "" && width != "0")) {
            $('#lblWidth').text(width);
            $('#Width').val(width);
            $('#div_width').show();
        }
        if ((height != "" && height != "0")) {
            $('#lblHeight').text(height);
            $('#Height').val(height);
            $('#div_height').show();
        }
    }
    $("#vehicle-restrication-details").hide();
}
$('body').on('click', '#edit', function (e) {

    e.preventDefault();
    var DispensationId = $(this).data('edit');
    Edit(DispensationId);
});
$('body').on('click', '#delete', function (e) {

    e.preventDefault();
    var DispensationId = $(this).data('dispensid');
    var DispensationReferenceNo = $(this).data('dispensationreferenceno');
    Delete(DispensationId, DispensationReferenceNo);
});

function Edit(DispensationId) {
    dispId = DispensationId;
    var url = "../Dispensation/EditDispensation" + EncodedQueryString("dispId=" + dispId);
    window.location.href = url;
}
function ShowVehicleRestrications() {

    var gross = $('#Gross').val();
    var axle = $('#Axle').val();
    var length = $('#Length').val();
    var width = $('#Width').val();
    var height = $('#Height').val();

    if ((gross != "" && gross != "0"))
        $('#Gross').val(gross);
    else
        $('#Gross').val('');

    if ((axle != "" && axle != "0"))
        $('#Axle').val(axle);
    else
        $('#Axle').val('');

    if ((length != "" && length != "0"))
        $('#Length').val(length);
    else
        $('#Length').val('');

    if ((width != "" && width != "0"))
        $('#Width').val(width);
    else
        $('#Width').val('');

    if ((height != "" && height != "0"))
        $('#Height').val(height);
    else
        $('#Height').val('');
}


function ChangeSearchCriteria() {
    SetSearchText();
}
$('body').on('click', '#view-dis', function (e) {

    e.preventDefault();
    var DispensationId = $(this).data('dispensationid');
    viewDiscription(DispensationId);
});
function viewDiscription(DispensationId) {
    let descriptionId = 'description_' + DispensationId;
    if (document.getElementById(descriptionId).style.display !== "none") {
        document.getElementById(descriptionId).style.display = "none"
    }
    else {
        document.getElementById(descriptionId).style.display = "revert"
        LoadDispensationDetails(DispensationId);
    }
}

function LoadDispensationDetails(DispensationId) {
    let descriptionId = '#description_' + DispensationId;
    let dispensation_divId = '#dispensation_details_' + DispensationId;
    console.log('dispensation_divId', dispensation_divId);
    var url = '../Dispensation/ViewDispensation';
    $.ajax({
        url: url,
        type: 'POST',
        cache: false,
        data: { dispId: DispensationId },
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {

            var dispensationDetails = $(descriptionId).find('#description').find(dispensation_divId);
            $(dispensationDetails).html(response);

        },
        error: function (result) {
        },
        complete: function () {
            stopAnimation();
        }
    });
}


jQuery(document).ready(function ($) {
    $(document).on('click', '.bs-canvas-overlay', function () {
        $('.bs-canvas-overlay').remove();
        $("#filters").css('margin-right', "-400px");
        return false;
    });
});


function clearSearchListDispenstaion() {
    $(':input', '#filters')
        .not(':button, :submit, :reset, :hidden')
        .prop('checked', false);
    $('#filters').find('option:selected').prop("selected", false)
    var index = $('#DDsearchCriteria option:selected').val();
    if (index == 1) {
        $('#txtCriteria').val('');
        $('#txtCriteria').attr('placeholder', 'DRN');
    }
    SearchDispensationList();

    //$('#filterimage').hide();
    //$('#filterimageDRN').hide();
    //$('#filterimageBrief').hide();
    //$('#filterimageissuedby').hide();


}
var sortTypeGlobal = 1;//1-desc
var sortOrderGlobal = 1;//DRN
function DispensationSort(event, param) {
    sortOrderGlobal = param;
    sortTypeGlobal = event.classList.contains('sorting_asc') ? 1 : 0;
    $('#SortTypeValue').val(sortTypeGlobal);
    $('#SortOrderValue').val(sortOrderGlobal);
    var pageNum = $('#pageNum').val();
    SearchDispensationList(isSort = true,pageNum);
}

$('body').on('change', '.pagination-ManageDispensation #pageSizeSelect', function () {
    var page = getUrlParameterByName("page", this.href);
    $('#pageNum').val(page);
    var pageSize = $(this).val();
    $('#pageSizeVal').val(pageSize);
    SearchDispensationList(isSort = true);
});


$(document).ready(function () {

    $('body').on('change', '.dispensation-searchDDL', function (e) {
        SetSearchText();
    });
    $('body').on('click', '.dispensation-table #filterimage', function (e) {
        e.preventDefault();
        clearSearchListDispenstaion(this);
    });
    $('body').on('click', '#save-vehicle', function (e) {
        e.preventDefault();
        SaveVehicleRestrications(this);
    });
    ChangeSearchCriteria();
    var dispensationSearchItems = {};
    let isExpired = false;
    $('input:checked').each(function () {
        isExpired = $(this).attr("value");
    });
    dispensationSearchItems.Expired = isExpired;
    dispensationSearchItems.Criteria = $("#txtCriteria").val();
    dispensationSearchItems.SearchType = $('#DDsearchCriteria option:selected').val();
    dispensationSearchItems.SearchName = $('#DDsearchCriteria option:selected').text();
    if ((dispensationSearchItems.SearchType == '') && (dispensationSearchItems.SearchName != '') && (dispensationSearchItems.Criteria == '')) {
        $('#filterimage').hide();
    }
    else if ((dispensationSearchItems.Criteria != '')) {
        $('#filterimage').show();
    }
    if ((dispensationSearchItems.SearchType == 1) && (dispensationSearchItems.Criteria != '')) {
        $('#filterimageDRN').show();
    }
    if ((dispensationSearchItems.SearchType == 2) && (dispensationSearchItems.Criteria != '')) {
        $('#filterimageBrief').show();
    }
    if ((dispensationSearchItems.SearchType == 4) && (dispensationSearchItems.Criteria != '')) {
        $('#filterimageBrief').show();
    }


    if ((dispensationSearchItems.SearchType == 3) && (dispensationSearchItems.SearchName == "Authority") && (dispensationSearchItems.Criteria != '')) {
        $('#filterimageissuedby').show();
    }
    if ((dispensationSearchItems.SearchType == 3) && (dispensationSearchItems.SearchName == "Haulier")) {
        $('#filterimageissuedto').show();
    }

    closeFilters();
});
$('body').on('click', '.pagination-ManageDispensation a', function (e) {
    e.preventDefault();
    var page = getUrlParameterByName("page", this.href);
    $('#pageNum').val(page);
    SearchDispensationList(isSort = true, page);
});


function SetSearchText() {
    var index = $('#DDsearchCriteria option:selected').val();
    if (index == 1) {
        $('#txtCriteria').attr('placeholder', 'DRN');
    }
    if (index == 2) {
        $('#txtCriteria').attr('placeholder', 'Brief description');
    }
    if (index == 3 && ($('#hfUserTypeID').val() == '696002' || $('#hfUserTypeID').val() == '696007')) {
        $('#txtCriteria').attr('placeholder', 'Haulier name');
    }
    if (index == 3 && $('#hfUserTypeID').val() == '696001') {
        $('#txtCriteria').attr('placeholder', 'Authority name');
    }
    if (index == 4) {
        $('#txtCriteria').attr('placeholder', 'Dispensation description');
    }
}
