var EditOrgIdVal = $('#hf_EditOrgId').val();
var orgIdVal = $('#hf_orgId').val();
var arrIdVal = $('#hf_arrId').val();

$(document).ready(function () {
    selectedmenu('Structures');
    // fillPageSizeSelect();
    var OrgFromId = $('#orgId').val();
    var arrId = $('#arrId').val();

    // var OrgFromId = $('#OrgFromId').val();
    var EditOrgId = $('#EditOrgId').val();
    var Mode = $('#Mode').val();
    $('#review_deleg').load('../Structures/ReviewDelegation?arrangId=' + arrId + '&OrgFromId=' + OrgFromId);

   /* $('#btn_backnotindelegation').click(function () {*/
     $('body').on('click', '#btn_backnotindelegation', function (e) {
        window.location.href = '../Structures/StructureList' + EncodedQueryString('Mode=' + Mode + '&orgid=' + OrgFromId + '&arrId=' + arrId + '&EditOrgId=' + EditOrgId);
    });

    $('body').on('click', '.StructureCodeClick', function (e) {
       
  e.preventDefault();
  StructureCodeClick(this);
}); 
$('body').on('click','#btn_add', function(e) { 
  e.preventDefault();
  addNonDelegatedStructures(this);
}); 
    $(".childcheckbox").on('change', CheckboxSelection);
});
function addNonDelegatedStructures() {
    $('#validSelect').hide();
    $('#error').hide();

    if ($('.childcheckbox:checked').length > 0) {
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
            BindBy: combineBy
        }

        var paramListData = {
            structureSummary: paramList,
            EditOrgId: EditOrgIdVal,
            orgId: orgIdVal,
            arrId: arrIdVal,
            flag: 'notdelegated'
        }
        // console.log(paramListData);
        $.ajax({
            async: false,
            type: "POST",
            url: '../Structures/AddStructure',
            dataType: "json",
            //contentType: "application/json; charset=utf-8",
            data: JSON.stringify(paramListData),
            processdata: true,
            success: function (result) {
                if (result == 'valid') {
                    paramListData = null;
                    window.location.href = '../Structures/StructureList' + EncodedQueryString('Mode=Edit&orgid=' + $("#orgId").val() + '&arrId=' + $("#arrId").val() + '&EditOrgId=' + $('#EditOrgId').val());
                }
                else if (result == 'invalid') {
                    paramList = null;
                    $('#validSelect').show();
                    return false;
                }
                else {
                    $('#error').show();

                }
            },
            error: function () {
                $('#error').show();
            },
            complete: function () {
                $('.loading').hide();
            }
        });
    }
    else {

        $('#validSelect').show();
        return false;
    }
}
function StructureCodeClick(e) {
   
    var structureId =$(e).attr("structureid");
    linkStructureCodeClick(structureId);
}
function CheckboxSelection(e) {
    checkboxSelectionCheck(e);
}