var deleted_DelegaArrangName = '';
var delete_DelegaArrangId = 0;
var deleteDelegaArrangButton;
$(document).ready(function () {
    SelectMenu(3);
    //load_leftpanel();
    //fillPageSizeSelect();

    $('#viewdeleg').click(function () {
        window.location.href = '../Structures/ReviewDelegation';
    });
    $('body').on('change', '.MyDelegationArrangement-Pag #pageSizeSelect', function () {
        var pageSize = $(this).val();
        $('#pageSizeVal').val(pageSize);
        SearchFilter(isSort = false);
    });
    $("#btn_ClearDelegationFilter").click(function () {
        ClearFilter();
    });
    $("#btn_SearchDelegationFilter").click(function () {
        SearchFilter(isSort = false);
    });
    $('body').on('click', '#filterimage', function () {
        ClearFilter();
    });
    $('body').on('click', '#btnCreateNew', function () {
        Create();
    });
    $('body').on('click', '.editdelegation', function () {
        EditDelegationtDetails(this);
    });
    $('body').on('click', '.linkstructurecodeclick', function () {
        linkStructureCodeClicking(this);
    });
    $('body').on('click', '.deleteconfirmation', function () {
        DeletingConfirmation(this);
    });
});
function EditDelegationtDetails(_this) {
    var param1 = $(_this).attr("arg1");
    var param2 = $(_this).attr("arg2");
    var param3 = $(_this).attr("arg3");
    var param4 = $(_this).attr("arg4");
    EditDelegation(param1, param2, param3, param4);
}
function linkStructureCodeClicking(_this) {
    var param1 = $(_this).attr("arg1");
    var param2 = $(_this).attr("arg2");
    var param3 = $(_this).attr("arg3");
    linkStructureCodeClick(param1, param2, param3);
}
function DeletingConfirmation(_this) {
    var param1 = $(_this).attr("arg1");
    var param2 = $(_this).attr("arg2");
    DeleteConfirmation(_this, param1, param2);
}
function linkStructureCodeClick(ArrangementId, OrgToId, OrgFromID) {

    window.location.href = "../Structures/StructuresInDelegation" + EncodedQueryString("arrangId=" + ArrangementId + "&organisationId=" + OrgToId + "&OrgFromId=" + OrgFromID);
}
function ClearFilter() {
    $('#Searchtext').val('');
    $("#SearchType").prop('selectedIndex', 0);
    SearchFilter(isSort = false, isClear = true);
}
function SearchFilter(isSort = false, isClear=false) {
    var type = $('#SearchType').val();
    var text = $('#Searchtext').val();

    var sortType = $('#SortTypeValue').val();
    var sortOrder = $('#SortOrderValue').val();

    var url = '../Structures/MyDelegationArrangement';
    $.ajax({
        url: url,
        type: 'POST',
        cache: false,
        data: {
            searchType: type, searchValue: text, sortOrder, sortType,
            page: isSort ? $('#pageNum').val() : 1, pageSize: $('#pageSizeVal').val(), isClear: isClear       },
        beforeSend: function () {
            //startAnimation();
        },
        success: function (result) {


            $('section#banner').html('');
            $('section#banner').html($(result).find('section#banner').html());

            closeFilters();
        },
        error: function (result) {

            location.readload();
        },
        complete: function () {
            stopAnimation();
        }
    });
    //}
    //else {
    //    ShowErrorPopup("Error in filter");
    //}

}

// showing user-setting inside vertical menu
function showuserinfo() {
    if (document.getElementById('user-info').style.display !== "none") {
        document.getElementById('user-info').style.display = "none"
    }
    else {
        document.getElementById('user-info').style.display = "block";
        document.getElementsById('userdetails').style.overFlow = "scroll";
    }
}
function DeleteConfirmation(_this, DelegaArrangId, IsRoadDelegation) {

    startAnimation();
    arrid = $(_this).attr('id');
    arrangementName = $(_this).attr('name');

    if (IsRoadDelegation == 0) {
        var Msg = "Do you want to delete '" + "" + "'" + arrangementName + "'" + "" + "' ?";
        ShowWarningPopup(Msg, "DeleteDelegationArrangement");
    }
    else {
        showWarningPopDialog('You does not have permission to delete the record. Only Delegating Organisation is Delete the record', 'Close', '', WarningCancelBtn, 'DeleteDelegationArrangement', 1, 'warning');
    }
    stopAnimation();

}
function DeleteDelegationArrangement() {
    CloseWarningPopup();

    $.ajax({
        async: false,
        type: "POST",
        url: '../Structures/DeleteDelegationArrangement',
        dataType: "json",
        //contentType: "application/json;",
        data: JSON.stringify({ arrangId: arrid }),
        beforeSend: function () {
            startAnimation();
        },
        processdata: true,
        success: function (result) {

            stopAnimation();
            if (result) {

                ShowModalPopup("'" + arrangementName + "'" + " " + "deleted successfully");
            }
            else {
                ShowErrorPopup("Deletion failed");
            }
        },
        error: function (result) {

            stopAnimation();
            ShowErrorPopup("Error on the page");
        }
    });

}
function Create() {
    window.location.href = "../Structures/CreateDelegation" + EncodedQueryString("Flag=Create");
}
function EditDelegation(DelegaArrangId, orgID, OrgFromId, pageSize) {

    var organisationId = $('#orgId').val();

    window.location.href = "../Structures/EditDelegation?arrangId=" + DelegaArrangId + "&organisationId=" + orgID + "&OrgFromId=" + OrgFromId + "&pageSize=" + pageSize;
}
function DelegationArrangementSort(event, param) {
    sortOrderGlobal = param;
    sortTypeGlobal = event.classList.contains('sorting_asc') ? 1 : 0;
    $('#filters #SortTypeValue').val(sortTypeGlobal);
    $('#filters #SortOrderValue').val(sortOrderGlobal);
    SearchFilter(isSort = true);
}