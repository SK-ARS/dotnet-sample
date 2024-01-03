$(document).ready(function () {
    var pageSize = $('#pageSizeVal').val();
    $('#pageSizeSelect').val(pageSize);

    $('body').on('change', '.chk-cl-check-all', function () {
        checkboxSelectionCheck(this);
    });

    $('body').on('click', '.btn-cl-show-map-constraints', function () {
        var ConstraintId = $(this).data('constraintid');
        var FEasting = $(this).data('feasting');
        var FNorthing = $(this).data('fnorthing');
        ShowmapConstr(ConstraintId, FEasting, FNorthing);
    });

    $('body').on('click', '#btn_cancel', function () {
        RedirectLocation();
    });
    $('body').on('click', '.btn-cl-undelegated-structures', function () {
        UnDelegatedStructures();
    });
    $('body').on('click', '.btn-cl-save', function () {
        addStructure();
    });

    //--------------- FIlters
    $("#btn_ClearFilter").click(function () {
        ClearFilter();
    });
    $("#filterimage").click(function () {
        $('#filters form').submit();
    });

});

function ClearFilter() {
    $('#ConstraintName').val('');
    $('#ConstraintCode').val('');
    $("#ConstraintType").prop('selectedIndex', 0);
    $("#ChCkIsOwnerContact").prop('checked', false);
    $("#chckIsValid").prop('checked', false);
}

function ConstraintSort(event, param) {
    sortOrderGlobal = param;
    sortTypeGlobal = event.classList.contains('sorting_asc') ? 1 : 0;
    $('#filters #SortTypeValue').val(sortTypeGlobal);
    $('#filters #SortOrderValue').val(sortOrderGlobal);
    /* $('#btnsearchform').submit();*/
    $('#filters form').submit();
}

function DisplayListOnPagesize(_this, mode) {
    var pageSize = $(_this).val();
    window.location.href = "../Constraint/GetConstraintList" + EncodedQueryString("Mode=" + mode + "&pageSize=" + pageSize);


}
function ClearFilterss() {
    $('#ConstraintName').val('');
    $('#ConstraintCode').val('');
    $("#ConstraintType").prop('selectedIndex', 0);
    $("#ChCkIsOwnerContact").prop('checked', false);
    $("#chckIsValid").prop('checked', false);
}
function linkStructureCodeClick(structId) {
    window.location.href = "../Structures/ReviewSummary" + EncodedQueryString("structureId=" + structId);
}
$(document).ready(function () {
    if ($('#PortalType').val() == 696002) {
        SelectMenu(4);
    }
    else {
        SelectMenu(3);
    }
});
function checkboxSelectionCheck(obj) {
    $(".checkallheader").prop('checked', $(obj).prop('checked'));
    $(".childcheckbox").prop('checked', $(obj).prop('checked'));
}
function checkboxselection() {
    //// change child checkbox as per header check box selection
    //$('#checkall').change(function () {
    //    $(".childcheckbox").prop('checked', $(this).prop('checked'));
    //});

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
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(paramListData),
            beforeSend: function () {
                startAnimation();
            },
            processdata: true,
            success: function (result) {
                if (result == 'valid') {
                    stopAnimation();

                    paramListData = null;
                    if (buttonName == 'save') {
                        ShowSuccessModalPopup("Structure(s) selected  successfully", "RedirectLocation")
                    }
                    else {
                        ShowModalPopup("Structure(s) selected  successfully");
                        Nothingtodo();
                    }
                }
                else if (result == 'invalid') {
                    paramList = null;
                    ShowErrorPopup("Structure(s) saving  failed");
                }
                else {
                    ShowErrorPopup("Structure(s) saving  failed");

                }
            },
            error: function () {
                ShowErrorPopup("Structure(s) saving  failed");
            },
            complete: function () {

            }
        });
    }
    else {

        ShowErrorPopup("Error");
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


function ShowmapConstr(CnstrId, Feasting, Fnorthing) {
    sessionStorage.setItem("AuthorizeGeneralUrl", null);
    var OSGREast, OSGRNorth;
    OSGREast = Feasting;// $("#OSGREast").val();
    OSGRNorth = Fnorthing//$("#OSGRNorth").val();
    window.location.href = "../Structures/MyStructures" + EncodedQueryString("x=" + OSGREast + "&y=" + OSGRNorth + "&ConstrId=" + CnstrId);
}

$(document).ready(function () {

    $("#btn_ClearFilter").click(function () {
        ClearFilter();
    });
    $('body').on('click', '#filterimage', function () {
        ClearFilter();
    });

    function ClearFilter() {
        $('#ConstraintName').val('');
        $('#ConstraintCode').val('');
        $("#ConstraintType").prop('selectedIndex', 0);
        $("#ChCkIsOwnerContact").prop('checked', false);
        $("#chckIsValid").prop('checked', false);
        SoaConstraintSearch(isSort = false);
    }

    $("#btn_SearchConstraint").click(function () {
        SoaConstraintSearch(isSort = false);
    });

});
function ConstraintSort(event, param) {
    sortOrderGlobal = param;
    sortTypeGlobal = event.classList.contains('sorting_asc') ? 1 : 0;
    $('#filters #SortTypeValue').val(sortTypeGlobal);
    $('#filters #SortOrderValue').val(sortOrderGlobal);
    /* $('#btnsearchform').submit();*/
    SoaConstraintSearch(isSort = true);//$('#filters form').submit();
}

$('body').on('change', '.ConstraintListPagination #pageSizeSelect', function () {
    var pageSize = $(this).val();
    $('#pageSizeVal').val(pageSize);
    SoaConstraintSearch(isSort = false);
});
function SoaConstraintSearch(isSort = false) {
    var inputModel = {};
    inputModel.ConstraintName = $("#ConstraintName").val();
    inputModel.ConstraintType = $("#ConstraintType").val();
    inputModel.ConstraintCode = $("#ConstraintCode").val();
    inputModel.IsValid = $("#chckIsValid").prop('checked') == true;
    inputModel.IsOwnerContact = $("#ChCkIsOwnerContact").prop('checked') == true;
    closeFilters();
    $.ajax({
        url: '../Constraint/SaveConstSearch',
        type: 'POST',
        //cache: false,
        //async: false,
        data: {
            objSearch: inputModel, page: isSort ? $('#pageNum').val() : 1, pageSize: $('#pageSizeVal').val(),
            sortType: $('#filters #SortTypeValue').val(), sortOrder: $('#filters #SortOrderValue').val()
            , Mode: $('#Mode').val(), Flag: $('#Flag').val()
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