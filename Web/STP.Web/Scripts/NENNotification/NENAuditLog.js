$(document).ready(function () {
    $('body').on('click', '#btnClearNENotification', function (e) { e.preventDefault(); ClearNENotification(); });
    $('body').on('click', '.imgClearAuditLogFilter', function (e) { e.preventDefault(); ClearNENotification(); });
    $('body').on('click', '#btnSearchAuditLog', function (e) { e.preventDefault(); SearchNENotification(); });
    var pageSize = $('#pageSizeVal').val();
    $('#pageSizeSelect').val(pageSize);
    if ($('#userTypeId').val() == "696002") {
        SelectMenu(8);
    }
    else if ($('#userTypeId').val() == "696007") {
        SelectMenu(8);
    }
    else {
        SelectMenu(6);
    }
});

function viewotheroptions() {
    if (document.getElementById('viewotheroptions').style.display !== "none") {
        document.getElementById('viewotheroptions').style.display = "none"
        document.getElementById('chevlon-up-icon1').style.display = "none"
        document.getElementById('chevlon-down-icon1').style.display = "block"
    }
    else {
        document.getElementById('viewotheroptions').style.display = "block"
        document.getElementById('chevlon-up-icon1').style.display = "block"
        document.getElementById('chevlon-down-icon1').style.display = "none"
    }
}
function closeFilters() {
    document.getElementById("filters").style.width = "0";
    document.getElementById("banner").style.filter = "unset"
    document.getElementById("navbar").style.filter = "unset";

}
function SearchNENotification(isSort = false, ESDAL_Num) {
    startAnimation();
    var ESDAL_Num = $('#SearchString1').val(), sortFlag_val = 1, searchItem = $('#DDsearchCriteria').val();
    if (ESDAL_Num == "")
        sortFlag_val = 0
    $.ajax({
        url: '../Notification/AuditLog',
        type: 'GET',
        cache: false,
        async: false,
        data: {
            ESDAL_ref_num: ESDAL_Num, sortFlag: sortFlag_val, pageNumber: (isSort ? $('#pageNumberVal').val() : 1), pageSize: $('#pageSizeVal').val(), searchCriteria: searchItem,
            sortType: $('#filters #SortTypeValue').val(), sortOrder: $('#filters #SortOrderValue').val()
        },
        beforeSend: function () {

        },
        success: function (result) {
            $('.div_audit_table').html($(result).find('.div_audit_table').html());
            closeFilters();
            stopAnimation();
        },
        error: function (xhr, textStatus, errorThrown) {
            location.reload();
            stopAnimation();
        },
        complete: function () {
        }
    });
}
function ClearNENotification() {
    startAnimation();
    var searchItem = $('#DDsearchCriteria').val();
    searchItem = 0;
    $.ajax({
        url: '../Notification/AuditLog',
        type: 'GET',
        cache: false,
        async: false,
        data: { ESDAL_ref_num: null, sortFlag: 0, pageNumber: 1, pageSize: $('#pageSizeVal').val(), searchCriteria: '0',IsClear:true },
        beforeSend: function () {

        },
        success: function (result) {
            $('#SearchString1').val('');
            $('#DDsearchCriteria').val(1);
            $('.div_audit_table').html($(result).find('.div_audit_table').html());
            closeFilters();
            stopAnimation();
        },
        error: function (xhr, textStatus, errorThrown) {
            location.reload();
            stopAnimation();
        },
        complete: function () {
        }
    });
}
$('body').on('change', '.AuditLog-Pag #pageSizeSelect', function () {
    var pageSize = $(this).val();
    $('#pageSizeVal').val(pageSize);
    var ESDAL_Num = $('#SearchString1').val(), sortFlag_val = 1, searchItem = $('#DDsearchCriteria').val();
    SearchNENotification(isSort = false, ESDAL_Num);
});


$('body').on('change', '#DDsearchCriteria', function () {
    var dropVal = $("#DDsearchCriteria option:selected").val();
    if (dropVal == "3") {
        $('#SearchString1').val('');
        $('#SearchString1').attr("placeholder", "DD/MM/YYYY");
    } else if (dropVal == "1") {
        $('#SearchString1').val('');
        $('#SearchString1').attr("placeholder", "Esdal Reference");
    }
    else {
        $('#SearchString1').val('');
        $('#SearchString1').attr("placeholder", "User");
    }
   
});
var sortTypeGlobal = 1;//0-asc
var sortOrderGlobal = 1;//name
function AuditlogSort(event, param) {
    sortOrderGlobal = param;
    sortTypeGlobal = event.classList.contains('sorting_asc') ? 1 : 0;
    $('#SortTypeValue').val(sortTypeGlobal);
    $('#SortOrderValue').val(sortOrderGlobal);
    var ESDAL_Num = $('#SearchString1').val(), sortFlag_val = 1, searchItem = $('#DDsearchCriteria').val();
    SearchNENotification(isSort = true,ESDAL_Num);
}