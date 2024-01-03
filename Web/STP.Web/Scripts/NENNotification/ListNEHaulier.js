$(document).ready(function () {
    loadFilter();
    selectedmenu(8);
    $('body').on('click', ".IsEnable", function (e) {
        e.preventDefault();
        var AuthKeyid = $(this).attr('authkeyid');
        var HaulName = $(this).attr('haulname');
        var IsEnab = $(this).attr('isenab');
        var Limit = $(this).attr('limit');
        var Orgnam = $(this).attr('orgnam');
        EnableConfirm(AuthKeyid, HaulName, IsEnab, Limit, Orgnam);
    });
    $('body').on('click', ".EditNeUser", function (e) {
        e.preventDefault();
        var KeyId = $(this).attr('keyid');
        EditNeUser(KeyId);
    });
    $('body').on('click', ".ViewUser", function (e) {
        e.preventDefault();
        var KeyId = $(this).attr('keyid');
        ViewUser(KeyId);
    });
    $('body').on('click', ".clearNeUserFilter", function (e) {
        e.preventDefault();
        clearNeUserFilter();
    });
    $('body').on('click', "#newUserButton", function (e) {
        e.preventDefault();
        CreateNeUser();
    });
    $('body').on('click', ".btnSaveNeUserEdit", function (e) {
        e.preventDefault();
        var KeyId = $(this).data('keyid');
        SaveNeUserEdit(KeyId);
    });
    $('body').on('click', ".btnEditReset", function (e) {
        e.preventDefault();
        var KeyId = $(this).data('keyid');
        EditReset(KeyId);
    });
    $('body').on('click', ".btnViewDescription", function (e) {
        e.preventDefault();
        var neauthkey = $(this).data('neauthkey');
        viewDescription(neauthkey);
    });
    $('body').on('click', ".btnReload", function (e) {
        e.preventDefault();
        ReloadNeUsers();
    });
    $('body').on('click', ".btn-search-ne", function (e) {
        e.preventDefault();
        SearchNeUser();
    });
    $('body').on('click', ".btnClearNeUserFilter", function (e) {
        e.preventDefault();
        clearNeUserFilter();
    });
    $('body').on('click', "#ShowValid", function (e) {
        ShowValidNe();
    });

});
function EnableConfirm(AuthKeyid, HaulName, IsEnab, Limit, Orgnam) {            //function for setting a ne user valid/ no valid  1.set is valid, 0. set no valid (means authkey is empty)
    if (IsEnab == 0) // function for disabling
    {
        var Name = "Do you want to disable '" + "" + "'" + HaulName + "'" + "" + "' ?";
        ShowWarningPopup(Name, 'Disable', 'CloseWarningPopup',AuthKeyid,HaulName,IsEnab,Limit,Orgnam)
    }
    else {
        var Name = "Do you want to enable '" + "" + "'" + HaulName + "'" + "" + "' ?";
        ShowWarningPopup(Name, 'Enable', 'CloseWarningPopup', AuthKeyid, HaulName, IsEnab, Limit, Orgnam)       //function for enabling
    }
}
function Disable(AuthKeyid, HaulName, IsEnab, Limit, Orgnam) {         //the function is used for set the authentication key is to null
    $.ajax({
        url: '../NENNotification/EnableUser',
        type: 'GET',
        cache: false,
        async: false,
        data: { userName: HaulName, KeyId: AuthKeyid },
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
            if (result == 1) {
                ShowSuccessModalPopup('Disabled successfully', 'ReloadNeUsers');
                CloseWarningPopup();
            }
            else {
                ShowErrorPopup('Update Failed');

            }

        },
        error: function (xhr, textStatus, errorThrown) {
            //other stuff
            location.reload();
        },
        complete: function () {
            stopAnimation();

        }
    });
}
function Enable(AuthKeyid, HaulName, IsEnab, Limit, Orgnam) {    //generate the encryption key

    var Encrypt = btoa(HaulName);
    Encrypt = RandomEncrypt(Encrypt);
    Encrypt = Encrypt.substring(0, 20);
    var AuthKey = Encrypt;

    $.ajax({
        url: '../NENNotification/EnableUser',
        type: 'GET',
        cache: false,
        async: false,
        data: { userName: HaulName, KeyId: AuthKeyid, authKey: AuthKey },
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
            if (result == 1) {
                ShowSuccessModalPopup('Enabled successfully', 'ReloadNeUsers');
                CloseWarningPopup();
            }
            else {
                ShowErrorPopup('Update Failed');

            }

        },
        error: function (xhr, textStatus, errorThrown) {
            //other stuff
            location.reload();
        },
        complete: function () {
            stopAnimation();

        }
    });
}
function ReloadNeUsers() {
    CloseSuccessModalPopup();
    location.reload();
}
function RandomEncrypt(possible) {   // function for making a random string to appent
    var text = "";

    for (var i = 0; i < 20; i++) {
        text += possible.charAt(Math.floor(Math.random() * possible.length));
    }
    text = text.substring(0, 20);
    return text;
}
function loadFilter() {
    $('#NeFilterDiv').load('../NENNotification/SearchNeHaulierPanel');
}
function SearchNeUser(isClear = false, isSort = false) {
    if ($("#ShowValid").is(':checked')) {
        IsValid = 1

    } else {
        IsValid = 0;
    }
    var pageSize = $('#pageSizeVal').val();
    var SearchString = $('#SearchString').val();
    $.ajax({
        url: '../NENNotification/ListNEHaulier',
        type: 'GET',
        cache: false,
        async: false,
        data: {
            page: (isSort ? $('#pageNum').val() : 1), pageSize: pageSize, SearchString: SearchString, Isval: IsValid, sortType: $('#SortTypeValue').val(), sortOrder: $('#SortOrderValue').val(),
            isClear: isClear, isSearch:true
        },
        beforeSend: function () {
        },
        success: function (result) {
            $('#div_Organisation').html($(result).find('#div_Organisation').html());
            $('#pageSizeVal').val(pageSize);
            $('#pageSizeSelect').val(pageSize);
            $('#pagesize').val(pageSize);
            closeFilters();
        },
        error: function (xhr, textStatus, errorThrown) {
            //other stuff
            location.reload();
        },
        complete: function () {
        }
    });
}
function clearNeUserFilter() {
    $('#ShowValid').prop('checked', false);
    ShowValidNe();
    $('#SearchString').val("");
    SearchNeUser(true);
}
function CreateNeUser() {
    startAnimation();
    var isAccessible = $('#EditForAdmin').val();
    if (isAccessible == 'False') {
        ShowErrorPopup('You are not authorized to create');
    }
    else {
        $("#userDetails").load('/NENNotification/CreateNeuser?mode=save', function () {
            $("#manageUserMinDiv").html($("#tableDiv").html());
            $("#manageUserRow").show();
            $("#tableDiv").hide();
            $("#createNewTitle").show();
            $("#editTitle").hide();
            stopAnimation();
            CreateNeUserInit();
        });

    }
}
function EditNeUser(KeyId) {
    startAnimation();
    var isAccessible = $('#EditForAdmin').val();
    if (isAccessible == 'False') {
        ShowErrorPopup('You are not authorized to create');
        stopAnimation();
    }
    else {
        var link = '/NENNotification/CreateNeuser?AuthKeyId=id&mode=edit';
        var id = KeyId;
        link = link.replace("id", KeyId);
        $("#" + KeyId).load(link, function (data) {
            var authId = $("#" + KeyId).find('#AuthKey').val();
            if (authId != '') {
                $("#" + KeyId).find('#IsValidNe').prop("checked", true);

            }
            CreateNeUserInit();
        });
    }

    viewDescription(KeyId);
    stopAnimation();
}
function EditUser(KeyId) {

    var link = '/NENNotification/CreateNeuser?AuthKeyId=id';

    var id = KeyId;
    link = link.replace("id", KeyId);
    $("#userDetails").load(link, function () {
        if ($("#tableDiv").html() != "") {
            $("#manageUserMinDiv").html($("#tableDiv").html());
            $("#tableDiv").hide();
        }

        $("#manageUserRow").show();
        $("#createNewTitle").hide();
        $("#editTitle").show();
        CreateNeUserInit();
    });

    stopAnimation();
}
function ViewUser(KeyId) {
    startAnimation();
    var link = '/NENNotification/CreateNeuser?AuthKeyId=id&mode=View';
    var id = KeyId;
    link = link.replace("id", KeyId);
    $("#" + KeyId).load(link);

    viewDescription(KeyId);
    stopAnimation();
}
function viewDescription(KeyId) {
    if (document.getElementById(KeyId).style.display !== "none") {
        document.getElementById(KeyId).style.display = "none"
    }
    else {
        document.getElementById(KeyId).style.display = "contents"
    }
}
function SaveNeUserEdit(AuthKeyId) {
    startAnimation();
    var NehaulName = $("#" + AuthKeyId).find("#HaulName").val();
    var AuthKey = $("#" + AuthKeyId).find("#AuthKey").val();
    var OrgName = $("#" + AuthKeyId).find("#OrgNam").val();
    var NenLimit = $("#" + AuthKeyId).find("#NeLimit").val();
    var AuthKeyId = AuthKeyId;
    if (!$("#" + AuthKeyId).find("#IsValidNe").prop("checked")) {
        AuthKey = "";
    }
    if (AuthKeyId == 0) {
        var msg = "Non esdal haulier saved successfully";
    }
    else if (AuthKey == "") {
        var msg = "Non esdal haulier updated successfully";
    }
    else {
        var msg = "Non esdal haulier updated successfully and new authentication key is generated";
    }

    if (checkValidate(AuthKeyId)) {
        $.ajax({
            url: '/NENNotification/SaveNeUser',
            type: 'GET',
            cache: false,
            async: false,
            data: { Haulname: NehaulName, authKey: AuthKey, OrgName: OrgName, NeLimit: NenLimit, KeyId: AuthKeyId },
            beforeSend: function () {
                startAnimation();
            },
            success: function (result) {
                if (result == 1) {
                    ShowSuccessModalPopup(msg, 'ReloadNeUsers');
                }
                else {
                    ShowWarningPopup('Saving Failed', 'ReloadNeUsers');

                }

            },
            error: function (xhr, textStatus, errorThrown) {
                //other stuff
                location.reload();
            },
            complete: function () {
                stopAnimation();

            }
        });
    }

}
function checkValidate(AuthKeyId) {
    //IsCreate flag is 0 then we have to check duplication of haulier
    var NehaulName = $("#" + AuthKeyId).find("#HaulName").val();
    var OrgName = $("#" + AuthKeyId).find("#OrgNam").val();
    var NenLimit = $("#" + AuthKeyId).find("#NeLimit").val();
    if (NehaulName == '') {
        $("#" + AuthKeyId).find('#err_required_haulname').text('Haulier Name is required');
        $("#" + AuthKeyId).find('#err_required_haulname').show();
        if (OrgName == '') {
            $("#" + AuthKeyId).find('#err_required_orgname').text('Organisation Name is required');
            $("#" + AuthKeyId).find('#err_required_orgname').show();
        }
        if (NenLimit != "") {
            if (isNaN(NenLimit)) {
                $("#" + AuthKeyId).find('#err_number_nelimit').text('NEN limit must be a number');
                $("#" + AuthKeyId).find('#err_number_nelimit').show();
            }

        }
        stopAnimation();
        return false;

    }
    if (OrgName == '') {
        $("#" + AuthKeyId).find('#err_required_orgname').text('Organisation Name is required');
        $("#" + AuthKeyId).find('#err_required_orgname').show();
        if (NenLimit != "") {
            if (isNaN(NenLimit)) {
                $("#" + AuthKeyId).find('#err_number_nelimit').text('NEN limit must be a number');
                $("#" + AuthKeyId).find('#err_number_nelimit').show();
            }

        }
        stopAnimation();
        return false;

    }
    if (NenLimit != "") {
        if (isNaN(NenLimit)) {
            $("#" + AuthKeyId).find('#err_number_nelimit').text('NEN limit must be a number');
            $("#" + AuthKeyId).find('#err_number_nelimit').show();
            stopAnimation();
            return false;

        }

    }
    if (AuthKeyId == 0) {
        var IsValHaul = HaulierValidation(AuthKeyId);
        if (IsValHaul > 0) {
            $("#" + AuthKeyId).find('#err_required_haulname').text('Haulier Name already exists');
            $("#" + AuthKeyId).find('#err_required_haulname').show();
            stopAnimation();
            return false;

        }

    }
    return true;
}
function HaulierValidation(AuthKeyId) {
    var NehaulName = $("#" + AuthKeyId).find("#HaulName").val();
    var OrgName = $("#" + AuthKeyId).find("#OrgNam").val();
    var IsVal = 0;
    $.ajax({
        url: '/NENNotification/HaulierValid',
        type: 'GET',
        cache: false,
        async: false,
        data: { haulname: NehaulName, orgname: OrgName },
        success: function (result) {
            IsVal = result;
        },

    });
    return IsVal;
}
function EditReset(AuthKeyId) {
    $("#" + AuthKeyId).find("#HaulName").val('');
    $("#" + AuthKeyId).find("#AuthKey").val('');
    $("#" + AuthKeyId).find("#OrgNam").val('');
    $("#" + AuthKeyId).find('#IsValidNe').attr('checked', false);
    $("#" + AuthKeyId).find('#IsValidNe').prop('checked', false);
    $("#" + AuthKeyId).find("#NeLimit").val('');
    $("#" + AuthKeyId).find(".error").html("");

}
function generateAuthKey(AuthKeyId) {
    var NehaulName = $("#" + AuthKeyId).find("#HaulName").val();
    var Encrypt = btoa(NehaulName);
    Encrypt = RandomEncrypt(Encrypt,);
    Encrypt = Encrypt.substring(0, 20);
    $("#" + AuthKeyId).find("#AuthKey").val(Encrypt);
    if ($("#" + AuthKeyId).find("#AuthKey").val() != '') {
        $("#" + AuthKeyId).find('#IsValidNe').prop('checked', true);
    }
}
$('body').on('change', '.ListNEHaulier-Pag #pageSizeSelect', function () {
    var page = getUrlParameterByName("page", this.href);
    $('#pageNum').val(page);
    var pageSize = $(this).val();
    $('#pageSizeVal').val(pageSize);
    SearchNeUser(false, isSort = true);
});
var sortTypeGlobal = 1;//0-asc
var sortOrderGlobal = 1;//name
function NEHaulierSort(event, param) {
    sortOrderGlobal = param;
    sortTypeGlobal = event.classList.contains('sorting_asc') ? 1 : 0;
    $('#SortTypeValue').val(sortTypeGlobal);
    $('#SortOrderValue').val(sortOrderGlobal);
    SearchNeUser(false, isSort = true);
}