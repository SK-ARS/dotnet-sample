$(document).ready(function () {
    SelectMenu(3);
    $('body').on('click', '#roaddeleclsfilter', function (e) { e.preventDefault(); closeFilters(); });
    $('body').on('click', '.viewdelegation', function (e) {
        e.preventDefault();
        var arrangementId = $(this).data('arg1');
        Viewdelegationdetails(arrangementId);
    });
    $('body').on('click', '#srchbtn', function (e) { e.preventDefault(); SearchDelegation(); });
});
function Viewdelegationdetails(arrangementId) {
    Viewdelegation(arrangementId);
}
function openFilters() {
    document.getElementById("filters").style.width = "450px";
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

function closeFilters() {
    document.getElementById("filters").style.width = "0";
    document.getElementById("banner").style.filter = "unset"
    document.getElementById("navbar").style.filter = "unset";
}

function DeleteRoadDelegationPopup(arrangementId) {
    ShowWarningPopup('Are you sure you want to Delete?', "DeleteRoadDelegation", '', arrangementId);

}

function DeleteRoadDelegation(arrangementId) {
    CloseWarningPopup();
    $.ajax({
        async: false,
        type: "POST",
        url: '../RoadDelegation/DeleteRoadDelegation',
        dataType: "json",
        contentType: "application/json; charset=utf-8",
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

function SearchDelegation(isSort=false) {
    var searchString = $('#searchText').val();
    closeFilters();
    $.ajax({
        url: '../Structures/ShowRoadDelegation',
        type: 'POST',
        cache: false,
        async: false,
        data: {
            searchText: searchString,page: isSort ? $('#pageNum').val() : 1, pageSize: $('#pageSizeVal').val(),
            sortType: $('#SortTypeValue').val(), sortOrder: $('#SortOrderValue').val() },
        success: function (response) {
            var result = $(response).find('section#banner').html();
            $('section#banner').html(result);
        }
    });
}

function CreateRoadDelegation() {
    window.location.href = "../RoadDelegation/CreateRoadDelegation";
}

function EditDelegation(DelegArrangId) {
    //startAnimation();
    window.location.href = "../RoadDelegation/EditRoadDelegation" + EncodedQueryString("arrangementId=" + DelegArrangId);
}

function Viewdelegation(DelegArrangId) {
    //startAnimation();
    window.location.href = "../RoadDelegation/ViewRoadDelegation" + EncodedQueryString("arrangementId=" + DelegArrangId + '&viewFlag=0' + '&orginflag=SOA');
}
function RoadDelegationSort(event, param) {
    sortOrderGlobal = param;
    sortTypeGlobal = event.classList.contains('sorting_asc') ? 1 : 0;
    $('#SortTypeValue').val(sortTypeGlobal);
    $('#SortOrderValue').val(sortOrderGlobal);
    SearchDelegation(isSort = true);
}

$('body').on('change', '.RoadDelegationList-SOA-Pag #pageSizeSelect', function () {
    var pageSize = $(this).val();
    $('#pageSizeVal').val(pageSize);
    SearchDelegation(isSort = false);
});