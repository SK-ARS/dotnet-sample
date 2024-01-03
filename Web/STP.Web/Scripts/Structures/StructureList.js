var seacrchSummary = null;
$(document).ready(function () {
    var pageSize = $('#pageSizeVal').val();
    $('#pageSizeSelect').val(pageSize);
    $('body').on('click', '#filterimage', function (e) {
        e.preventDefault();
        ClearFilter(this);
    });
    $('body').on('click', '.structureCode', function (e) {
        e.preventDefault();
        StructureCodeClick(this);
    });
    $('body').on('click', '#btn_cancel', function (e) {
        e.preventDefault();
        RedirectLocation(this);
    });
    $('body').on('click', '#btnUnDelegated', function (e) {
        
        e.preventDefault();
        UnDelegatedStructures(this);
    });
    $('body').on('click', '#btn_save', function (e) {
        e.preventDefault();
        AddStructureFunction(this);
    });
    $('body').on('click', '#btn_saveandcontinue', function (e) {
        e.preventDefault();
        AddStructureFunction(this);
    });
    $('body').on('change', '#checkall', function (e) {
        SelectStructure(this);
    });
    $("#btn_ClearFilter").click(function () {
        ClearFilter();
    });

});
function DisplayListOnPagesize(_this, mode) {
    var pageSize = $(_this).val();
    window.location.href = "../Structures/StructureList" + EncodedQueryString("Mode=" + mode + "&pageSize=" + pageSize);
}
function linkStructureCodeClick(structId) {
    window.location.href = "../Structures/ReviewSummary" + EncodedQueryString("structureId=" + structId);
}
// showing filter-settings
$(document).ready(function () {
    SelectMenu(3);
});
function checkboxSelectionCheck(obj) {
    $(".checkallheader").prop('checked', $(obj).prop('checked'));
    $(".childcheckbox").prop('checked', $(obj).prop('checked'));
}
function checkboxselection() {
    // change header checkbox as per child check box selection
    $('.childcheckbox').change(function () {
        if ($(this).is(':checked')) {
            if ($('.childcheckbox').length == $('.childcheckbox:checked').length) {
                $(".checkallheader").prop('checked', true);
            }
        }
        else {
            $(".checkallheader").prop('checked', false);
        }
    });
}
window.onload = function () {


};
function addStructure(buttonName) {
    $('#validSelect').hide();
    $('#error').hide();
    if ($('.childcheckbox:checked').length > 0 || $('#structureInDelegList').val() != '') {
        var StructCodes = '';
        var PreviousStructCodes = '';
        var combineBy = ','
        var CountOfSelectedCheckbox = $('.childcheckbox:checked').length;
        for (var i = 0; i < CountOfSelectedCheckbox; i++) {
            StructCodes += $('.childcheckbox:checked')[i].value + combineBy;
        }

        var CountOfPreviousSelectedCheckbox = $('.childcheckboxHide:checked').length;
        for (var i = 0; i < CountOfPreviousSelectedCheckbox; i++) {
            PreviousStructCodes += $('.childcheckboxHide:checked')[i].value + combineBy;
        }

        var paramList = {
            StructCodes: StructCodes,
            PreviousStructCodes: PreviousStructCodes,
            BindBy: combineBy,
            ButtonName: buttonName
        }

        var paramListData = {
            structureSummary: paramList,
            EditOrgId: $('#EditOrgId').val(),
            orgId: $('#orgId').val(),
            arrId: $('#arrId').val()
        }
        console.log(paramListData);

        $.ajax({
            async: false,
            type: "POST",
            url: '/Structures/AddStructure',
            dataType: "json",
            data: paramListData,
            beforeSend: function () {
                startAnimation();
            },
            processdata: true,
            success: function (result) {
                stopAnimation();
                if (result == 'valid') {
                    paramListData = null;
                    if (buttonName == 'save') {
                        ShowSuccessModalPopup("Structure(s) selected  successfully", "RedirectLocation")
                    }
                    else {
                        ShowSuccessModalPopup("Structure(s) selected  successfully", "CloseSuccessModalPopup")
                        Nothingtodo();
                    }
                }
                else if (result == 'invalid') {
                    paramList = null;
                    ShowErrorPopup("Structure(s) saving  failed");
                }
                else {
                    ShowErrorPopup("Please select atleast one structure");

                }
            },
            error: function () {
                ShowErrorPopup("Please select atleast one structure");
            },
            complete: function () {
                stopAnimation();
            }
        });
    }
    else {

        ShowErrorPopup("Please select atleast one structure");
        return false;
    }
}
function Nothingtodo() {
    $('#pop-warning').hide();
    return false;
}
function RedirectLocation() {
    CloseSuccessModalPopup();
    window.location.href = "../Structures/CreateDelegation" + EncodedQueryString("Mode=Edit");
}
function UnDelegatedStructures() {
    
    var OrganisationId = $('#orgId').val();
    window.location.href = "../Structures/StructureNotInDelegationList" + EncodedQueryString("OrgId=" + OrganisationId);
}
function SoastructureSort(event, param) {
    var sortTypeGlobal = 0;//asc
    var sortOrderGlobal = 2;//name
    sortOrderGlobal = param;
    sortTypeGlobal = event.classList.contains('sorting_asc') ? 1 : 0;//1 -desc            //0-asc

    $('#filters #SortTypeValue').val(sortTypeGlobal);
    $('#filters #SortOrderValue').val(sortOrderGlobal);
    SoastructureSearch(true);
}
function ClearFilter() {

    $('#SearchSummaryName').val('');
    $('#AlternateName').val('');
    $('#SearchSummaryId').val('');
    $('#Description').val('');
    $('#Carries').val('');
    $('#Crosses').val('');
    $('#DelegateName').val('');
    $("#StructureType").prop('selectedIndex', 0);
    $("#ICAMethod").prop('selectedIndex', 0);
    SoastructureSearch();//$('#filters form').submit();
}
$('body').on('change', '#pageSizeSelects', function () {
    var pageSize = $(this).val();
    $('#pageSizeVal').val(pageSize);
    SoastructureSearch(isSort = false);
});
function SoastructureSearch(isSort = false) {
    
    var StrcutNotInDel = $("#StructureNotIndelegation").val();
    var inputModel = {};
    inputModel.SearchSummaryId = $("#SearchSummaryId").val();
    inputModel.SearchSummaryName = $("#SearchSummaryName").val();
    inputModel.AlternateName = $("#AlternateName").val();
    inputModel.Description = $("#Description").val();
    inputModel.Carries = $("#Carries").val();
    inputModel.Crosses = $("#Crosses").val();
    inputModel.ICAMethod = $("#ICAMethod").val();
    inputModel.StructureType = $("#StructureType").val();
    seacrchSummary = inputModel.SearchSummaryId;
    closeFilters();
    var url;
        url = '../Structures/SaveStructSearch';
    $.ajax({
        url: url,
        type: 'POST',
        cache: false,
        async: false,
        data: {
            objSearch: inputModel, page: isSort ? $('#pageNum').val() : 1, pageSize: $('#pageSizeVal').val(), Mode: $('#Mode').val(),
            sortType: $('#filters #SortTypeValue').val(), sortOrder: $('#filters #SortOrderValue').val(), StrcutNotInDel: StrcutNotInDel
        },
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {
            
            var result = $(response).find('section#banner').html();
            $('section#banner').html(result);
        },
        error: function (xhr, textStatus, errorThrown) {
            location.reload();
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function StructureCodeClick(e) {
    var structureId = $(e).attr("structureid");
    linkStructureCodeClick(structureId);
}
function AddStructureFunction(e) {
    var mode = $(e).attr("mode");
    addStructure(mode);
}
function SelectStructure(e) {
    checkboxSelectionCheck(e);
}
