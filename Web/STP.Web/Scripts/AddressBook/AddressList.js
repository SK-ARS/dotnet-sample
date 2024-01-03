$(document).ready(function () {
    $('body').on('click', '#AddAddressDetails', function (e) {
        e.preventDefault();
        CreateAddressDetails(this);
    });

    $('body').on('click', '#button-address', function (e) {
        e.preventDefault();
        CreateAddressDetails(this);
    });
    $('body').on('click', '.address-list-table #filterimage', function (e) {
        e.preventDefault();
        ClearFilter(this);
    });
    $('body').on('click', '#clear-button', function (e) {
        e.preventDefault();
        ClearFilter(this);
    });
    $('body').on('click', '#search-button', function (e) {
        e.preventDefault();
        AddressListSearchFilter();
    });
    $("#SearchType").on('change', SetSearchText);
});
var deletedContactId = 0;
var deletedContactName = "";
var pageSizeTemp = $('#pageSize').val();
/* $('.dropdown-toggle').dropdown();*/
$('body').on('change', '.AddressBookPagination #pageSizeSelect', function () {
    var page = getUrlParameterByName("page", this.href);
    $('#pageNum').val(page);
    var pageSize = $(this).val();
    $('#pageSizeVal').val(pageSize);
    AddressListSearchFilter(false, isSort = true);
});

//Import from Plan movement affected party - Open import list div
$('body').on('click', '#addaddressbook', function (e) {
    AddressListSearchFilter();    
});
$('body').on('click', '#notifaddress', function (e) {
    e.preventDefault();
    var HaulierContactId = $(this).data('notif');
    ImportNotifAddress(HaulierContactId);
});
$('body').on('click', '.AddressBookPagination a', function (e) {
    e.preventDefault();
    if (this.href == '') {
        return false;
    }
    else {
        var page = getUrlParameterByName("page", this.href);
        $('#pageNum').val(page);
        AddressListSearchFilter(false,isSort = true);
    }
});

$('body').on('click', '#address-detail', function (e) {
    e.preventDefault();
    var HaulierContactId = $(this).data('haulierid');
    ViewAddressDetails(HaulierContactId);
});
$('body').on('click', '#edit-address', function (e) {
    e.preventDefault();
    var HaulierContactId = $(this).data('editaddress');
    EditAddressDetails(HaulierContactId);
});
$('body').on('click', '.delete-address.mouseCursor', function (e) {
    e.preventDefault();
    var HaulierContactId = $(this).data('hauliercontactid');
    var Name = $(this).data('name');
    DeleteAddress(this, HaulierContactId, Name);
});
$('body').on('click', '#description1', function (e) {

    e.preventDefault();
    var HaulierContactId = $(this).data('hauliercontactid');
    viewDiscription(HaulierContactId);
});

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



jQuery(document).ready(function ($) {
    $(document).on('click', '.bs-canvas-overlay', function () {
        $('.bs-canvas-overlay').remove();
        $('#filters').css('margin', '0 -400px 0 0');
        return false;
    });
});


// Attach listener function on state changes
function ClearFilter() {
    $('#Searchtext').val('');
    $("#SearchType").prop('selectedIndex', 0);
    $('#Searchtext').attr('placeholder', 'Select criteria type');
    AddressListSearchFilter(true);
}
function AddressListSearchFilter(isClear = false, isSort = false, searchColumn = "", searchValue = "") {
    var searchColumn = $('#SearchType').val()||'';
    var searchValue = $('#Searchtext').val()||'';
    var pageSize = $('#pageSizeVal').val();
    var isPlanMovement = $('#hf_IsPlanMovmentGlobal').length > 0;
    startAnimation();
    $.ajax({
        url: '../AddressBook/AddressList?page=' + (isSort ? $('#pageNum').val() : 1) + '&pageSize=' + pageSize +
            '&searchId=' + searchColumn + '&searchText=' + searchValue
            + '&sortType=' + $("#SortTypeValue").val()
            + '&sortOrder=' + $("#SortOrderValue").val()
            + '&isPlanMovement=' + isPlanMovement
            + '&isClear=' + isClear
            + '&isMainPage=' + true,
        type: 'GET',
        cache: false,
        async: false,
        beforeSend: function () {
            
        },
        success: function (response) {
            closeFilters();
            stopAnimation();
            if (isPlanMovement) {
                $('#General').html($(response).find('#caution').html());
                $('#General .header').remove();
                $('#filters').remove();
                //PopUpAddressBookinit();
                SubStepFlag = 5.1;
                var filters = $(response).find('div#filters');
                $(filters).insertAfter('#banner');
                $('#filters').find("#SearchType").val(searchColumn);
                $('#filters').find("#Searchtext").val(searchValue);
                $('#leftpanel').hide();
                $('#route-assessment').hide();
                $("#General").show();
                $("#backbutton").hide();
            } else {
                $('#caution').html($(response).find('#caution').html());
            }
        }
    });
}
function ImportNotifAddress(contactId) {
    var pageNum = $('#pagePopupNo').val();
    var pageSize = $('#pagePopupSize').val();
    var notificationid = $('#hf_NotificationId').val();
    var analysisId = $('#hf_AnalysisId').val();
    var isSortSideCall = $('#hf_IsSortSideCall').val();
    if ($('#hf_originImp').val() == "routesave" || $('#hf_originImp1').val() == "outlinesave" || $('#hf_originImp2').val() == "annotation") {
        var duplicateEntry = 0;
        for (var i = 0; i < annotationContactList.length; i++) {
            if (annotationContactList[i].contactID == contactId) {
                duplicateEntry = 1;
            }
        }
        if (duplicateEntry == 1) {
            $("#overlay").hide();
            ShowErrorPopup("Contact already exist", "CloseAnnotationErrorPopUp");
        }
        else {
            $('#notif_contact_list').empty();
            getContactDetail(contactId);
            $('.modal-backdrop').remove();
        }
    }
    $.ajax({

        url: "../Contact/AffectedContactDetail",
        type: 'POST',
        data: { "ContactID": contactId, "NotifID": notificationid, "analysisId": analysisId, "fromSort": isSortSideCall, haulierContact: true },
        beforeSend: function () {
            startAnimation();
        },
        success: function (data) {
            if (data.result.status != null) {
                if (data.result.sortAddedContact == 0 || data.result.sortAddedContact == "0") {
                    ShowErrorPopup('Contact already exist');
                    stopAnimation();
                }
                else {
                    ShowErrorPopup('Contact already exist');
                    stopAnimation();
                }
            }
            else {
                if (data.result.sortAddedContact == 0 || data.result.sortAddedContact == "0") {
                    loadManualAddedDiv(data.result.NotifID, data.result.analysisId);
                }
                else {
                    loadSortSideRouteAssessment(data.result.analysisId, pageNum, pageSize);
                }
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            location.reload();
        },
        complete: function () {
            $("#backbutton").show();
        }
    });
}
function ViewAddressDetails(HaulierContactId) {
  
    var cntrId = "#dialogue_" + HaulierContactId;
    var tdId = "#td_" + HaulierContactId;
    $('tr[id^="dialogue_"]').find('td[id^="td_"]').html('');
    $('tr[id^="dialogue_"]').hide();
    LoadAddressView('../AddressBook/AddressDetail?HaulierContactId=' + HaulierContactId, cntrId, tdId)
    
    
}
function LoadAddressView(url, cntrId, tdId) {
    $.ajax({
        type: "GET",
        url: url,
        success: function (result) {
            $(cntrId).find(tdId).html(result);
            $(cntrId).show();
            ManageAddressBookInit();
        },
        error: function (result) {
        }
    });
}
var randomNumber = Math.random();
function EditAddressDetails(HaulierContactId) {
    var cntrId = "#dialogue_" + HaulierContactId;
    var tdId = "#td_" + HaulierContactId;
    $('tr[id^="dialogue_"]').find('td[id^="td_"]').html('');
    $('tr[id^="dialogue_"]').hide();
    removescroll();
    LoadAddressView('../AddressBook/ManageAddressBook?mode=Edit&HaulierContactId=' + HaulierContactId + '&random=' + randomNumber, cntrId, tdId);
    $("#overlay").show();
    $(cntrId).show();
}
function DeleteAddress(_this, HaulierContactId, HaulierContactName) {

    deletedContactId = HaulierContactId;
    deletedContactName = HaulierContactName;
    var Msg = "Do you want to delete '" + "" + "'" + HaulierContactName + "'" + "" + "' ?";
    ShowWarningPopup(Msg, "DeleteData");

}
function DeleteData() {
    CloseWarningPopup();
    $.ajax({
        type: "POST",
        url: "../AddressBook/DeleteAddress",
        data: { deletedContactId: deletedContactId },
        success: function (result) {
            if (result == true) {
                var Msg = '"' + deletedContactName + '"  deleted successfully';
                ShowSuccessModalPopup(Msg, "ReloadLocation");
            }
        },
        error: function (result) {
        }
    });
}
function CreateAddressDetails() {
    closeFilters();
    $('.bottom-pagination').html('');
    $("#manage-user").load('../AddressBook/ManageAddressBook?mode=Create&HaulierContactId=0&random=' + randomNumber);
    removescroll();
    /* $("#dialogue").show();*/
    $("#AddAddressDetails").hide();
    $(".AddressBookPagination").hide();
    $("#overlay").show();
    ManageAddressBookInit();
}
function AddAddressDetails() {
    $('#overlay').addClass('displayNone');
    // const element = document.getElementById("ScrollDownDiv");
    // element.scrollIntoView();
    var randomNumber = Math.random();
    var options = { "backdrop": "static", keyboard: true };
    $.ajax({
        type: "GET",
        url: '/AddressBook/ManageAddressBookDetails',
        contentType: "application/json; charset=utf-8",
        data: { "mode": 'create', "HaulierContactId": 0, "nonEsdalContact": true, "random": randomNumber },
        datatype: "json",
        success: function (data) {
            
            $("#overlay").show();
            $("#manageAddressBookPopup").css('display', 'block');
            $('#manageAddressBookPopup').html(data);
            $('#manageAddressBookPopup').modal(options);
            $("#manageAddressBookmodal").css('display', 'block');       
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("Dynamic content load failed." + errorThrown);
        }
    });
}

function viewDiscription(id) {
    if (document.getElementById('dialogue_' + id).style.display !== "none") {
        document.getElementById('dialogue_' + id).style.display = "none";
        document.getElementById('dialogue_' + id).style.display = "none";

        // document.getElementById('chevlon-up-icon').style.display = "none"
        // document.getElementById('chevlon-down-icon').style.display = "block"
    }
    else {
        document.getElementById('dialogue_' + id).style.display = "revert";
        document.getElementById('dialogue_' + id).style.display = "revert";
        // document.getElementById('chevlon-up-icon').style.display = "block"
        // document.getElementById('chevlon-down-icon').style.display = "none"
    }
}
function SetSearchText() {
    var index = $('#SearchType option:selected').val();
    if (index == "NAME") {
        $('#Searchtext').attr('placeholder', 'Contact  name');
        $('#Searchtext').val('');
    }
    if (index == "ORGANISATION_NAME") {
        $('#Searchtext').attr('placeholder', 'Organisation name');
        $('#Searchtext').val('');
    }
    if (index == "FAX_E-mail") {
        $('#Searchtext').attr('placeholder', 'Email');
        $('#Searchtext').val('');
    }
    if (index == 0) {
        $('#Searchtext').attr('placeholder', 'Select criteria type');
        $('#Searchtext').val('');
    }
}

var sortTypeGlobal = 0;//0-asc
var sortOrderGlobal = 1;//type
function AddressSort(event, param) {
    sortOrderGlobal = param;
    sortTypeGlobal = event.classList.contains('sorting_asc') ? 1 : 0;
    $('#SortTypeValue').val(sortTypeGlobal);
    $('#SortOrderValue').val(sortOrderGlobal);
    var searchColumn = $('#SearchType').val();
    var searchValue = $('#Searchtext').val();
    AddressListSearchFilter(false, isSort = true, searchColumn, searchValue);
}
