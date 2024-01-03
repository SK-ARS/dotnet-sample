$(document).ready(function () {
    var pageSize = $(this).val();
    $('#pageSizeVal').val(pageSize);
    SelectMenu(3);
    $('body').on('click', '#btnopen', openFilters);
    $('body').on('click', '#filterimage', ClearDelegation);
    $('body').on('click', '#btndelegation', CreateRoadDelegation);
    $('body').on('click', '.cleardelegation', ClearDelegation);
    $('body').on('click', '.search', function (e) {
        SearchDelegation();
    });
});

$('body').on('click', '.viewdelegation', function (e) {
    e.preventDefault();
    let ArrangementId = $(this).data('arrangementid');
    Viewdelegation(ArrangementId);
});
$('body').on('click', '.editdelegation', function (e) {
    e.preventDefault();
    let ArrangementId = $(this).data('arrangementid');
    EditDelegation(ArrangementId);
});
$('body').on('click', '.deletedelegation', function (e) {
    e.preventDefault();
    let ArrangementId = $(this).data('arrangementid');
    DeleteRoadDelegationPopup(ArrangementId);
});


function closeFilters() {
    document.getElementById("filters").style.width = "0";
    document.getElementById("banner").style.filter = "unset"
    document.getElementById("navbar").style.filter = "unset";
}

function DeleteRoadDelegationPopup(arrangementId) {
    ShowWarningPopup('Are you sure you want to delete the delegation. If you proceed, all associated sub-delegates also will be deleted', "DeleteRoadDelegation", '', arrangementId);

}

function DeleteRoadDelegation(arrangementId) {
    CloseWarningPopup();
    $.ajax({
        async: false,
        type: "POST",
        url: '../RoadDelegation/DeleteRoadDelegation',
        dataType: "json",
        //contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ arrangementId: arrangementId }),
        processdata: true,
        success: function (result) {
            if (result.value == true) {
                ShowSuccessModalPopup("Deleted successfully", "Pagereload");
            }
            else if (result.value == false) {
                ShowErrorPopup("Deletion failed");
            }
        },
        error: function (result) {
            ShowErrorPopup("Deletion failed");
            location.reload();
        }
    });

}

function Pagereload() {
    location.reload();
}

function ClearDelegation() {
    $('#searchText').val("");
    SearchDelegation(isSort = false, isClear = true);
}

function SearchDelegation(isSort = false, isClear = false) {
    let searchString = $('#searchText').val();
    closeFilters();
    $.ajax({
        url: '../RoadDelegation/GetRoadDelegationList',
        type: 'POST',
        cache: false,
        async: false,
        data: {
            searchText: searchString, sortType: $('#filters #SortTypeValue').val(), sortOrder: $('#filters #SortOrderValue').val(),
            page: (isSort ? $('#pageNum').val() : 1), pageSize: $('#pageSizeVal').val(), isClear
        },
        success: function (response) {
            let result = $(response).find('section#banner').html();
            $('section#banner').html(result);
        }
    });
}

function CreateRoadDelegation() {
    window.location.href = "../RoadDelegation/CreateRoadDelegation";
}

function EditDelegation(DelegArrangId) {
    window.location.href = "../RoadDelegation/EditRoadDelegation" + EncodedQueryString("arrangementId=" + DelegArrangId + "");
}

function Viewdelegation(DelegArrangId) {
    window.location.href = "../RoadDelegation/ViewRoadDelegation" + EncodedQueryString("arrangementId=" + DelegArrangId + '&viewFlag=0' + "");
}
$('body').on('change', '.DelegationList-Pag #pageSizeSelect', function () {
    var page = getUrlParameterByName("page", this.href);
    $('#pageNum').val(page);
    var pageSize = $(this).val();
    $('#pageSizeVal').val(pageSize);
    SearchDelegation(isSort = true);
});
let sortTypeGlobal = 0;//0-asc
let sortOrderGlobal = 1;//name
function SortRoadDelegation(event, param) {
    sortOrderGlobal = param;
    sortTypeGlobal = event.classList.contains('sorting_asc') ? 1 : 0;
    $('#SortTypeValue').val(sortTypeGlobal);
    $('#SortOrderValue').val(sortOrderGlobal);
    SearchDelegation(isSort = true);
}

