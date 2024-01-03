$(function () {
    removeHLinks();
    PaginateGrid();
    
    RemoveDelete();
    RemoveLinks();

    //viewConfig(1);
    //Map_back();
    
});


function removeHLinks() {

    $('#Config-body').find('.pagination').find('li a').removeAttr('href').css("cursor", "pointer");

    
}

//function Pagination
function PaginateGrid() {
    //method to paginate through page numbers
    $('#Config-body').find('.pagination').find('li').not('.active, .PagedList-skipToLast, .PagedList-skipToNext, .PagedList-skipToFirst, .PagedList-skipToPrevious').find('a').click(function () {
        //var pageCount = $('#TotalPages').val();
        var pageNum = $(this).html();
        AjaxPagination(pageNum);
        //console.log($('.active a').html());
    });
    PaginateToLastPage();
    PaginateToFirstPage();
    PaginateToNextPage();
    PaginateToPrevPage();
}


//method to paginate to last page
function PaginateToLastPage() {
    $('#Config-body').find('.PagedList-skipToLast').click(function () {
        var pageCount = $('#TotalPages').val();
        AjaxPagination(pageCount);
    });
}

//method to paginate to first page
function PaginateToFirstPage() {
    $('#Config-body').find('.PagedList-skipToFirst').click(function () {
        AjaxPagination(1);
    });
}

//method to paginate to Next page
function PaginateToNextPage() {
    $('#Config-body').find('.PagedList-skipToNext').click(function () {
        var thisPage = $('#Config-body').find('.active').find('a').html();
        var nextPage = parseInt(thisPage) + 1;
        AjaxPagination(nextPage);
    });
}

//method to paginate to Previous page
function PaginateToPrevPage() {
    $('#Config-body').find('.PagedList-skipToPrevious').click(function () {
        var thisPage = $('#Config-body').find('.active').find('a').html();
        var prevPage = parseInt(thisPage) - 1;
        AjaxPagination(prevPage);
    });
}


//function Ajax call fro pagination
function AjaxPagination(pageNum) {
    //var compType = GetComponentType();
    var Notif = $('#isNotifVehicle').val();
   
    $.ajax({
        url: "../VehicleConfig/VehicleConfigList",
        type: 'GET',
        cache: false,
        async: false,
        data: { page: pageNum, isNotifVehicle: Notif },
        beforeSend: function () {
            //$("#overlay").show();
            $('.loading').show();
        },
        success: function (result) {
            $('#Config-body').html($(result).find('#div_fleet').html());
           // $('#div_fleet').find('.head1').hide();
            removeHLinks();
            PaginateGrid();
            RemoveDelete();

            RemoveLinks_Map();
            //$('#pageSizeVal').val(pageSize);
            //$('#pageSizeSelect').val(pageSize);
            //$('#pagesize').val(pageSize);
        },
        error: function (xhr, textStatus, errorThrown) {
            //other stuff
            // //location.reload();
        },
        complete: function () {
            //$("#overlay").hide();
            $('.loading').hide();
        }
    });
    function changePageSize(_this) { alert(""); }
}


function RemoveDelete() {
    $('#tbl_vehclComp tr').each(function () {
        $(this).find('td').eq(2).remove();
       
    });

    
}

function RemoveLinks() {
    $('#tbl_vehclComp').find(".link green").removeAttr('href').css("cursor", "pointer");
}



function ViewConfiguration(id) {
    $.ajax({
        url: "../VehicleConfig/ViewConfiguration",
        type: 'GET',
        cache: false,
        async: false,
        data: { vehicleID: id },
        beforeSend: function () {
            //$("#overlay").show();
            $('.loading').show();
        },
        success: function (result) {
            $('#Config-body').html($(result).find('#div_confing').html());
            //$('#div_Route').find('.head1').hide();
            removeHLinks();
           // PaginateGrid();
            RemoveDelete();

           // RemoveLinks_Map();
            //$('#pageSizeVal').val(pageSize);
            //$('#pageSizeSelect').val(pageSize);
            //$('#pagesize').val(pageSize);
        },
        error: function (xhr, textStatus, errorThrown) {
            //other stuff
            // //location.reload();
        },
        complete: function () {
            //$("#overlay").hide();
            $('.loading').hide();
        }
    });

}


function viewConfig(id) {

    view = true;
 
    ViewConfiguration(id);

    removeHLinks();

    RemoveDelete();
}


    

