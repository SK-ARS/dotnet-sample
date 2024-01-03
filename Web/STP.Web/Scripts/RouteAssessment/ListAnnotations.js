var Id = '';
var saveMsgVal;
var createAlertMsg;
function ListAnnotationInit() {
    selectedmenu('Movements'); // for selected menu
    fillPageSizeSelect();
    AnnotationInit();
    saveMsgVal = $('#hf_saveMsg').val();
    createAlertMsg = $('#CreateAlert').val();
    if (createAlertMsg == "True") {
        showWarningPopDialog(saveMsgVal, 'Ok', '', 'ReloadLocation', '', 1, 'info');
    }
};

function load_leftpanel() {

    console.log();
    $('#leftpanel').load('../Account/SearchUserPanel');

}

function ShowDisable() {

    var disableFlag = false;
    if ($("#chkDisable").is(':checked')) {
        disableFlag = true;
    }

    var pageSize = $('#hf_pageSize').val();
    var SearchString = $('#SearchString').val();
    var SearchType = $('#hf_UserSearchType').val();
    $.ajax({
        url: '../Account/userList',
        type: 'GET',
        cache: false,
        async: false,
        data: { pageSize: pageSize, SearchString: SearchString, SearchType: SearchType, checkBoxVal: disableFlag },
        beforeSend: function () {
            $("#overlay").show();
        },
        success: function (result) {
            $('#div_User').html($(result).find('#div_User').html());
            $('#pageSizeVal').val(pageSize);
            $('#pageSizeSelect').val(pageSize);
            $('#pagesize').val(pageSize);

            if ($("#hdnUserDisableFlag").val() == "1") {
                $("#chkDisable").prop("checked", true);
            }
            else {
                $("#chkDisable").prop("checked", false);
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            //other stuff
            location.reload();
        },
        complete: function () {
            $("#overlay").hide();
        }
    });
}

function changePageSize(_this) {
    var pageSize = $(_this).val();
    var SearchString = $('#SearchString').val();
    var SearchType = $('#hf_UserSearchType').val();
    var disableFlag = $('#hf_UserDisableFlag').val();


    $.ajax({
        url: '../Account/userList',
        type: 'GET',
        cache: false,
        async: false,
        data: { pageSize: pageSize, SearchString: SearchString, SearchType: SearchType, checkBoxVal: disableFlag },
        beforeSend: function () {
            $("#overlay").show();
        },
        success: function (result) {
            $('#div_User').html($(result).find('#div_User').html());
            $('#pageSizeVal').val(pageSize);
            $('#pageSizeSelect').val(pageSize);
            $('#pagesize').val(pageSize);
            var x = fix_tableheader();
            if (x == 1) $('#tableheader').show();
        },
        error: function (xhr, textStatus, errorThrown) {
            //other stuff
            location.reload();
        },
        complete: function () {
            $("#overlay").hide();
        }
    });
}

function fillPageSizeSelect() {
    var selectedVal = $('#pageSizeVal').val();
    $('#pageSizeSelect').val(selectedVal);
}