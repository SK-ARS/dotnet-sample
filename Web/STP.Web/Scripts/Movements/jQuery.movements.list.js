$(function () {
  
    ////var filterType = $('#ApplicationType').val();
    ////filterType = parseInt(filterType);

    ////switch (filterType) {
    ////    case 1:
    ////        //LoadSOMovemntFilter();
    ////        break;
    ////    default:
    ////        //LoadSOMovemntFilter();
    ////        break;
    ////}
    //////LoadSOMovemntFilter();
    ////var IsNot = $('#IsNotify').val();
    ////var VR1 = $('#VR1Appl').val();

    ////// var url = '../Movements/SOMovementFilter?IsNotify=' + IsNot + '&VR1Appl=' + VR1;
    ////var url = '../Movements/SOMovementFilter';
    ////url += '/?IsNotify=' + IsNot;
    var ApplicationStatus = $('#ApplicationStatus').val();
    ////var SortUserTypeId = $('#SortUserTypeId').val();
   var vr1 = $('#vr1appln').val();

    ////if (ApplicationStatus == 308001)
    ////{ }
    ////else if (ApplicationStatus == 0)
    ////{ }
    ////else if (SortUserTypeId == 696008)
    ////{ }
    ////else
    ////{
    ////    //loadLeftSearchPanel(url);
    ////}
  //  loadLeftSearchPanel(url);
    LoadAdvancedSearch();

    CheckEnabledFields();
    EnableDateFields();

    //changepageheading('Movement List');
    HasAdvancedFilter();
    //Allow numeric
    AllowNumericOnly();
   
    if (ApplicationStatus == 308001 || ApplicationStatus == 0 || vr1=='True') {
        
        selectedmenu('Applications');
    }
    else {
        selectedmenu('Movements');
    }
    //FilterApplied();

    //function for fixing the table head
    //$(".tblheaderfix").fixMe();
    
});

function LoadSOMovemntFilter() {

    $.ajax({
        url: '../Movements/SOMovementFilter',
        type: 'POST',
        beforeSend: function () {
            startAnimation();
        },
        success: function (page) {
            $('#leftpanel').html(page);            
        },
        complete: function () {
            FilterApplied();
            stopAnimation();
        }
    });
}

function ShowAdvanced() {
   
    var filterType = $('#ApplicationType').val();
    
    filterType = parseInt(filterType);

    $('#AdvancedSelected').val(filterType);
    //console.log(filterType);
    switch (filterType) {
        case 1:
            if ($('.div_so_movement_filter_advanced').is(':visible')) {
                $('.div_so_movement_filter_advanced').slideUp('slow');//1500,function () {
                   // $("html").animate({ scrollTop: $(document).height() }, 1000);
                //});
                //$('.center_align_advsearch').find('button').attr('data-icon', '&#xe111;');
            }
            else {
                $('.div_so_movement_filter_advanced').show('fast',function () {
                    $("html").animate({ scrollTop: $(document).height() }, 1000);
                });
                //$('.center_align_advsearch').find('button').attr('data-icon', '&#xe111;');
            }
            break;
        default:
            //LoadSOMovemntFilter();
            break;
    } 
    $("html,body").animate({ scrollTop: $(document).height() }, 1500);
}

function ClearAdvanced() {

    var filterType = $('#ApplicationType').val();
    filterType = parseInt(filterType);
    //console.log(filterType);
    switch (filterType) {
        case 1:
            //$('.div_so_movement_filter_advanced').hide();
            break;
        default:
            //LoadSOMovemntFilter();
            break;
    }
    ClearAdvancedData();
    ResetData();
   
    return false;
}


function LoadAdvancedSearch() {

    var advType = $('#AdvancedSelected').val();
    advType = parseInt(advType);

    switch (advType) {
        case 1:
            $('.div_so_movement_filter_advanced').show();
            break;
        default:
            $('.div_so_movement_filter_advanced').hide();
            break;
    }

}


function EnableDateFields() {
    $('#div_so_advanced_search table input:checkbox').live('click', function () {      
        if ($(this).is(':checked')) {
            $(this).closest('tr').find('input:text').attr('disabled', false);
        }
        else {
            $(this).closest('tr').find('input:text').attr('disabled', true);
            $(this).closest('tr').find('input:text').val("");
            $(this).closest('tr').find('.field-validation-error').html("");
        }
    });
}

function CheckEnabledFields() {
    $('#div_so_advanced_search table input:checkbox').each(function () {
        if ($(this).is(':checked')) {
            $(this).closest('tr').find('input:text').attr('disabled', false);
        }
        else {
            $(this).closest('tr').find('input:text').attr('disabled', true);
            
        }
    });
}

function FilterByPreset(_this) {
    var _thisValue = $(_this).val();
    AjaxFilterByPreset(_thisValue);
}

function AjaxFilterByPreset(_thisVal) {
    $.ajax({
        url: '../Movements/SetPresetFilter',
        type: 'POST',
        data: { presetFilter: _thisVal },
        beforeSend: function () {
        },
        success: function (result) {
            if (result.data) {
                location.reload();
            }
        }
    });
}

function HasAdvancedFilter() {
    var _advFilter = $('#div_so_advanced_search');
    _advFilter.find('input:text').each(function () {        
        if ($.trim($(this).val()) != '') {            
            $('.div_so_movement_filter_advanced').show();
        }
    });
    _advFilter.find('input:radio').not(':eq(0)').each(function () {
        if ($(this).is(':checked')) {
            $('.div_so_movement_filter_advanced').show();
        }
    });
}

function ClearAdvancedData() {
    
    var _advFilter = $('#div_so_advanced_search');
    _advFilter.find('input:text').each(function () {
        $(this).val('');
    });
    _advFilter.find('input:radio').eq(0).prop('checked', true);
    _advFilter.find('input:checkbox').attr('checked', false);

    _advFilter.find('input:checkbox').closest('tr').find('input:text').attr('disabled', 'disabled');   
}


function FilterApplied() {
    var spanFilters = '';
    var folder = $('#FolderName').val();
    $('input:checkbox').each(function () {
        if ($(this).is(':checked')) {
            //console.log($(this).closest('div').find('.text').text());
            //spn_move_filters
            var _thisVal = $(this).closest('div').find('.text').text();
            if (spanFilters == '') {
                spanFilters = _thisVal;
            }
            else {
                spanFilters = spanFilters +','+ _thisVal;
            }
        }
    });
    $('#spn_move_filters').text(spanFilters);
    if (folder == '') {
        $('#folder_name').text('All folders');
    }
    else {
        $('#folder_name').text(folder);
    }
    SortOrder();
}

function SortOrder() {
    //$('input:radio').each(function () {
        if ($('input:radio').eq(0).is(':checked') == true) {
            $('#filter_sort_order').text('List is ordered based on the ESDAL Movement Reference Number with the highest number listed first.');
        }
        else if ($('input:radio').eq(1).is(':checked') == true) {
            $('#filter_sort_order').text('List is ordered based on the Haulier Reference Number, with the lowest number listed first.');
        }
        else if ($('input:radio').eq(2).is(':checked') == true) {            
            $('#filter_sort_order').text('List is ordered based on the Vehicle Gross weight, with lowest weights returned first');
        }
        else if ($('input:radio').eq(3).is(':checked') == true) {
            $('#filter_sort_order').text('List is ordered based on the Vehicle Overall width, with lowest widths returned first.');
        }
        else if ($('input:radio').eq(4).is(':checked') == true) {
            $('#filter_sort_order').text('List is ordered based on the Vehicle Length, with lowest lengths returned first.');
        }
        else if ($('input:radio').eq(5).is(':checked') == true) {
            $('#filter_sort_order').text('List is ordered based on the Vehicle Height, with lowest heights returned first.');
        }
        else if ($('input:radio').eq(6).is(':checked') == true) {
            $('#filter_sort_order').text('List is ordered based on the Vehicle Axle Weight, with lowest axle weights returned first.');
        }
        else if ($('input:radio').eq(7).is(':checked') == true) {
            $('#filter_sort_order').text('List is ordered presenting movement dates in chronologically descending order.');
        }
        else if ($('input:radio').eq(8).is(':checked') == true) {
            $('#filter_sort_order').text('List is ordered presenting application dates in chronologically descending order.');
        }
        else if ($('input:radio').eq(9).is(':checked') == true) {
            $('#filter_sort_order').text('List is ordered presenting notification dates in chronologically descending order.');
        }
    //});
}

function ResetData() {
    $.ajax({
        url: '../Movements/ClearSOAdvancedFilter',
        type: 'POST',
        success: function (data) {
            //ClearAdvancedData();
        }
    });
}

function AllowNumericOnly() {
    // isnumeric
    $('.isnumeric').keypress(function (evt) {        
        var charCode = (evt.which) ? evt.which : event.keyCode;
        return !(charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57));
    });
}



//function for loading leftpanel filter menu
function filtermenu_load() {

    var filterType = $('#ApplicationType').val();
    filterType = parseInt(filterType);

    switch (filterType) {
        case 1:
            //LoadSOMovemntFilter();
            break;
        default:
            //LoadSOMovemntFilter();
            break;
    }
    //LoadSOMovemntFilter();
    var IsNot = $('#IsNotify').val();
    var VR1 = $('#VR1Appl').val();

    // var url = '../Movements/SOMovementFilter?IsNotify=' + IsNot + '&VR1Appl=' + VR1;
    var url = '../Movements/SOMovementFilter';
    url += '/?IsNotify=' + IsNot;
    var ApplicationStatus = $('#ApplicationStatus').val();
    var SortUserTypeId = $('#SortUserTypeId').val();
    var vr1 = $('#vr1appln').val();

    loadLeftSearchPanel(url);

}