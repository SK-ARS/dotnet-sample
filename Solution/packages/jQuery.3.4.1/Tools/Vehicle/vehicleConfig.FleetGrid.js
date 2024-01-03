$(function () {
    removeHyperLinks();
    PaginateVehicleGrid();
    
        $("#txt_search").live("keypress", function (event) {
            if (event.which == 13) {
                if ($("#IS_NOTIF_VEHICLE_LIST").val() != "TRUE") 
                {
                    SearchComponent();
                }
            }
        });
    
});

//function remove href from pagination ul li
function removeHyperLinks() {
    
    $('#div_fleet_component').find('.pagination').find('li a').removeAttr('href').css("cursor", "pointer");
    $('#div_fleet_component').find('.pagination').find('.active a').css("z-index", 0);

}

//function Pagination
function PaginateVehicleGrid() {
    //method to paginate through page numbers
    //$('#div_tbl_grid').find('.pagination').find('li').not('.active, .PagedList-skipToLast, .PagedList-skipToNext, .PagedList-skipToFirst, .PagedList-skipToPrevious').find('a').live('click', function () {
        $('#div_tbl_grid .pagination').on('click','li a',function(){        
        var smt = $(this).closest('li').attr('class');
        switch (smt) {
            case 'active':
                break;
            case 'PagedList-skipToLast':
                var pageCount = $('#div_tbl_grid').find('#TotalPages').val();
                AjaxVehiclePagination(pageCount);
                break;
            case 'PagedList-skipToNext':
                var thisPage = $('#div_tbl_grid').find('.active').find('a').html();
                var nextPage = parseInt(thisPage) + 1;
                AjaxVehiclePagination(nextPage);
                break;
            case 'PagedList-skipToFirst':
                AjaxVehiclePagination(1);
                break;
            case 'PagedList-skipToPrevious':
                var thisPage = $('#div_tbl_grid').find('.active').find('a').html();
                var prevPage = parseInt(thisPage) - 1;
                AjaxVehiclePagination(prevPage);
                break;
            default:
                var pageNum = $(this).html();
                AjaxVehiclePagination(pageNum);
                break;
        }

    });
    
}


//method to paginate to last page
function PaginateToLastPage() {
    $('#div_tbl_grid').find('.PagedList-skipToLast').click(function () {
        var pageCount = $('#TotalPages').val();
        AjaxVehiclePagination(pageCount);
    });
}

//method to paginate to first page
function PaginateToFirstPage() {
    $('#div_tbl_grid').find('.PagedList-skipToFirst').click(function () {
        AjaxVehiclePagination(1);
    });
}

//method to paginate to Next page
function PaginateToNextPage() {
    $('#div_tbl_grid').find('.PagedList-skipToNext').click(function () {
        var thisPage = $('#div_tbl_grid').find('.active').find('a').html();
        var nextPage = parseInt(thisPage) + 1;
        
        AjaxVehiclePagination(nextPage);
    });
}

//method to paginate to Previous page
function PaginateToPrevPage() {
    $('#div_tbl_grid').find('.PagedList-skipToPrevious').click(function () {
        var thisPage = $('#div_tbl_grid').find('.active').find('a').html();
        var prevPage = parseInt(thisPage) - 1;
        AjaxVehiclePagination(prevPage);
    });
}


//function Ajax call fro pagination
function AjaxVehiclePagination(pageNum) {
    var isVR1 = $('#vr1appln').val();
    var componentType = '';
    if (typeof compTypeTemp !== 'undefined') {
        componentType = compTypeTemp;
    }
    //var compType = GetComponentType();
    var keyword = $('#txt_search').val();

    $('#hidden_comp_page').val(pageNum);
    $('#hidden_comp_keyword').val(keyword);


    $('#hidden_compType').remove();
    $('#form_fleet_pagination').append('<input type="hidden" name="compType" id="hidden_compType" value="' + componentType + '"><input type="hidden" name="isVR1" id="hidden_isVR1" value="' + isVR1 + '">');

    $('#form_fleet_pagination').submit();
}

//function to import component Id to config
function ImportComponent(componentId) {    
    SaveComponentOnImport(componentId);
}

//function to import component Id to config //added by ajit
function ImportAppComponent(componentId) {
    $('#IsSOCompFromFleet').val(1);
    SaveAppComponentOnImport(componentId);
}

//function to import component Id to config //added by ajit
function ImportVR1Component(componentId) {
    SaveVR1ComponentOnImport(componentId);
}


//function to search component
function SearchComponent() {
    var txt_srch = $('#txt_search').val();    
    AjaxSearchGrid(txt_srch);
}


//function Ajax call fro pagination
function AjaxSearchGrid(keyword) {
    var compType = GetComponentType();
    $.ajax({
        url: "../VehicleConfig/FleetComponentList",
        
        data: '{"compType":' + JSON.stringify(compType) + ',"keyword":' + JSON.stringify(keyword) +'}',
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        async: false,
        //data: { compType: compType, keyword: keyword },
        beforeSend: function () {
            //$("#overlay").show();
            $('.loading').show();
        },
        success: function (result) {
            $('#div_tbl_grid').html(result);
            
        },
        error: function (xhr, textStatus, errorThrown) {
            //other stuff
            //location.reload();
        },
        complete: function () {
            //$("#overlay").hide();
            $('.loading').hide();
            $('#txt_search').val(keyword);
        }
    });
}


//function to view component
function ViewComponent(id) {
    $.ajax({
        url: '../Vehicle/ViewComponent',
        type: 'GET',
        cache: false,
        async:false,
        data: { vehicleSubTypeId: 0, vehicleTypeId: 0, movementId: 0, componentId: id },
        beforeSend: function () {            
            $('.loading').show();
            //$('.dyntitle').html('View component');
        },
        success: function (result) {
            //div_view_component;   
            $('#div_view_component').html(result);
        },
        error: function (xhr, textStatus, errorThrown) {
            //other stuff
            location.reload();
        },
        complete: function () {            
            removescroll();
            //div_summarise_config
            $('#div_summarise_config').hide();
            $('#div_view_component').show();

            $('#div_view_component').append('<div><button class="next btngrad btnrds btnbdr" aria-hidden="true" data-icon="&#xe0f4;" type="button" onclick="ImportData(' + id + ')">Import</button><button class="next btngrad btnrds btnbdr" aria-hidden="true" data-icon="&#xe119;" type="button" onclick="ViewConfigSummary()">Back</button></div>')

            $('.loading').hide();
        }
    });
}


//function to Import Data
function ImportData(id) {
    var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
    var isVR1 = $('#vr1appln').val();
    var isNotify = $('#ISNotif').val();
    
    if (isNotify == 'True' || isNotify == 'true') {

        SaveVR1ComponentOnImport(id);
    }
    else if (ApplicationRevId == 0) {
        SaveComponentOnImport(id);
    }
    else if (isVR1 == 'True' || isVR1 == 'true') {
        
        SaveVR1ComponentOnImport(id);
    }
    else {
         SaveAppComponentOnImport(id);//added by ajit
    }
    ViewConfigSummary();
}

//function to view config summary
function ViewConfigSummary() {
    $('#div_summarise_config').show();
    $('#div_view_component').hide();
    $('#div_view_component').html('');
}

function FillGrid(result) {
    
    var htm = '<div>' + result + '</div>'    
    var gridhtm = $(htm).find('#div_fleet_component').html();

    $('#div_tbl_grid').html(gridhtm);
    $('#div_tbl_grid').find('.pagination').find('li a').removeAttr('href').css("cursor", "pointer");

    //PaginateComponent();

}

//function PaginateComponent() {
//    $('#div_fleet_component').find('.pagination').find('li:not(.active, .disabled)').find('a').live('click', function () {
//        var _this = $(this);
//        //this_element = $(this);
//        var url = _this.attr('href');
//        if (url != undefined) {
//            var pagenum = _this.attr('href').split('page=');
//            $('#form_pagination').attr('action', url);
//            $('#form_pagination').find('#page').val(pagenum[pagenum.length - 1].match(/\d+/g)[0]);
//            $('#form_pagination').submit();
//        }
//        return false;
//    });
//}


function SetComponentType() {
    var componentType = '';
    if (typeof compTypeTemp !== 'undefined') {
        componentType = compTypeTemp;
    }
    $('#form_fleet_pagination').append('<input type="hidden" name="compType" id="hidden_compType" value="' + componentType + '">');
}




