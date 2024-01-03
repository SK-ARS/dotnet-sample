var selectedVal;
var ddlselval;
var pageNum = 1;
var annotationContact = {};
var pageNumber = $('#hf_pageNum').val();
var notificationid = $('#hf_notificationid').val();
var analysisId = $('#hf_analysisId').val();
var isSortSideCall = $('#hf_IsSortSideCall').val();
var originImp = $('#hf_originImp').val();
var contactsCount = $('#hf_count').val();
var contactobj;
$(document).ready(function () {
    $('body').on('click', '#AddNonEsdalContact', AddNonEsdalContact);

    $('body').on('click', '#openannotation', function (e) {
        e.preventDefault();
        openAnnotationFilter(this);
    });
    if ($('#contactpopup').length > 0) {
        if ($('#hf_FromAnnotation').val() == 'True') {
            $('#ContactListBanner').hide();
            showContactListFormPopUp();
        }
        else {
            showContactListForm();
        }
    }
    $('body').on('click', '#CreateNonEsdalContact', function (e) {
        AddNonEsdalContact(this);
    });
    $('body').on('click', '#clearAddressBookFilter', function (e) {
        e.preventDefault();
        clearAddressBookFilter(this);
    });
    $('body').on('click', '#searchNotifContact', function (e) {
        e.preventDefault();
        searchNotifContact(this);
    });
    $('body').on('click', '#closeFiltersAddressBook', function (e) {
        e.preventDefault();
        closeFilters(this);
    });
    $('body').on('click', '#contactListContainer #filterimage_notif_import', function (e) {
        e.preventDefault();
        $("#DDsearchCriteria").val("1");
        $("#SearchValue").val("");
        $('#SearchValue').attr('placeholder', 'Contact name');
        $('#filterimage_notif_import').hide();
        searchNotifContact(this);
    });
    $('body').on('click', '#import-notif', function (e) {
        e.preventDefault();
        var HaulierContactId = $(this).data('contactid');
        ImportNotifContact(HaulierContactId);
    });
    $('body').on('click', '#notif', function (e) {
        e.preventDefault();
        var HaulierContactId = $(this).data('notif');
        ImportNotifContact(HaulierContactId);
    });
    $('body').on('click', '#import', function (e) {
        e.preventDefault();
        var HaulierContactId = $(this).data('import');
        ImportNotifContact(HaulierContactId);
    });
});
function PopUpAddressBookinit() {
    Resize_PopUp(900);
    $('#contactpopup').show();
    if ($('#hf_FromAnnotation').val() == 'True' || $('#hf_FromAnnotation').val() == 'value') {
        $('#ContactListBanner').hide();
        showContactListFormPopUp();
    }
    else {
        showContactListForm();
    }
}
function showContactListForm() {
    startAnimation();
    var pageSize = $('#hf_pageSize').val();
    $("#notif_contact_list").load('../Contact/ContactList?page=' + pageNumber + '&pageSize=' + pageSize + '&isNotifContact=true' + '&isNotifContactSearch=true', function () {
        $('#contactDivPopup').css('display', 'none');
        $('#contactDivPage').css('display', 'block');
        stopAnimation();
    });
}
function showContactListFormPopUp() {
    startAnimation();
    var pageSize = $('#hf_pageSize').val();
    $("#dialogue1").load('../Contact/ContactList?page=' + pageNumber + '&pageSize=' + pageSize + '&isNotifContact=true' + '&isNotifContactSearch=true' + '&fromAnnotation=true', function () {
        stopAnimation();
        $('#contactDivPopup').css('display', 'block');
        $('#contactDivPage').css('display', 'none');
        $('#overlay').css('display', 'block');
        $("#dialogue1").css('display', 'block');
        $('#exampleModalCenterCnt').modal('show');
        removeHrefLinksContactList();
        PaginateContactList();
    });
}
function openFiltersAddressBook() {
    $("#filterDivAddressBook").find("#filters").css("width", "350px");
    var div = document.getElementById("vehicles");
    if (div != null) {
        document.getElementById("banner-container").style.filter = "brightness(0.5)";
        document.getElementById("banner-container").style.background = "white";
    }
    else {
        document.getElementById("navbar").style.filter = "brightness(0.5)";
        document.getElementById("navbar").style.background = "white";
        document.getElementById("banner").style.filter = "brightness(0.5)";
        document.getElementById("banner").style.background = "white";
    }
    function myFunction(x) {
        if (x.matches) { // If media query matches
            $("#filterDivAddressBook").find("#filters").css("width", "200px");
        }
    }
    var x = window.matchMedia("(max-width: 770px)")
    myFunction(x) // Call listener function at run time
    x.addListener(myFunction)
}
function ImportNotifContact(contactId) {
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
        data: { "ContactID": contactId, "NotifID": notificationid, "analysisId": analysisId, "fromSort": isSortSideCall },
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
function getContactDetail(contactId) {
    $.ajax({

        url: '../Contact/ContactDetail',
        type: 'POST',
        async: false,
        dataType: 'json',
        data: { ContactID: contactId, origin: $('#hf_originImp').val() },
        beforeSend: function () {
            //startAnimation();
        },
        success: function (data) {
            contactobj = data.result;


            fillDialogue(data);
            $('.loading').hide();
        }
    });

}
function fillDialogue(data) {
    $('#dialogue1').hide();
    $('#ContactListBanner').hide();
    var str1 = data.result.ADDRESSLINE_1 + "  " + data.result.ADDRESSLINE_2 + "  " + data.result.ADDRESSLINE_3;
    var str2 = data.result.PHONENUMBER;
    //vadr email = selectedItem.EMAIL;
    $("#dialogue").show();
    Resize_PopUp(580);
    if ($('#hf_originImp').val() == "annotation") {
        $('#dialogue').find('.dyntitleConfig').text('Insert Annotation');
        $('#annothead').show();
        $('#annotbody').show();
        fillTable(data);
        $('#contactpopup').html('');
        //   $("#contactpopup").hide();

        return;
    }
    $("#dialogue").find('.body').find('#locationAddress').val(str1);
    $("#dialogue").find('.body').find('#locationContact').val(str2);
    if ($('#hf_originImp').val() == "routesave") {
        $('#dialogue').find('.dyntitleConfig').text('Planned route details');
        $('#plannedhead').show();
        $('#plannedbody').show();
    }
    else {
        $('#dialogue').find('.dyntitleConfig').text('Textual Description route details');
        $('#outlinehead').show();
        $('#outlinebody').show();
    }

    $('#contactpopup').html('');
}
function fillTable(data) {
    annotationContact.contactNo = 0;
    annotationContact.contactID = data.result.ContactId;
    annotationContact.organizationID = data.result.OrganisationId;
    annotationContact.isAdhoc = data.result.OnBehalfOf;
    annotationContact.fullName = data.result.FullName;
    annotationContact.orgName = data.result.Organisation;
    annotationContact.organizationID = data.result.OrganisationId;
    annotationContact.email = data.result.Email;
    annotationContact.phoneNumber = data.result.PhoneNumber;
    annotationContactList.push(annotationContact);
    annotationContact = {};
    $('#tableAnnotContact').find('tr:eq(' + $('#hf_count').val() + ')').find('td:eq(' + 0 + ')').text(data.result.Organisation);
    $('#tableAnnotContact').find('tr:eq(' + $('#hf_count').val() + ')').find('td:eq(' + 1 + ')').text(data.result.PhoneNumber);


}
function loadManualAddedDiv(notifId, analysisId) {
    $.ajax({
        url: '../Notification/ManualAddedParties',
        type: 'POST',
        async: false,
        data: { NotifID: notifId, analysisId: analysisId },

        beforeSend: function () {
            startAnimation();
        },

        success: function (data) {
            $('#ManuallyAddedContact').html('');
            $('#notif_section').html('');
            $('#ManuallyAddedContact').html(data);
            $('#route-assessment').show();
            $("#General").hide();
            if ($('#hf_AffectFlag').val() == 1) {
                document.getElementById("addMeAsAffected").checked = true;
            }
        },
        error: function (xhr, textStatus, errorThrown) {

            location.reload();
        },
        complete: function () {
            stopAnimation();
        }

    });
}
function loadSortSideRouteAssessment(analysisId, pageNum, pageSize) {
    $.post('/Notification/GetSortAffectedParties?analysisId=' + analysisId + "&isSORT=" + true, function (data) {
        BacktoAffectedParties();
        $('#ManuallyAddedContact').html(data);
        if ($('#PortalType').val() == 696008) {//SORT
            $('#addaddressbook').hide();
        }
    });
}
function AddContactDialog(analysisId, pageNum, pageSize) {
    resetdialogue();

    $("#General").load('../Notification/PopUpAddressBook?NotifID=0' + '&analysisId=' + analysisId + '&fromSort=true' + '&pageNum=' + pageNum + '&pageSize=' + pageSize);
    $('#leftpanel').hide();
    $("#General").show();

}
function CreateNonEsdalContact() {
    const element = document.getElementById("ScrollDownDiv");
    element.scrollIntoView();
    var randomNumber = Math.random();

    startAnimation();
    $("#dialogue").load('../AddressBook/ManageAddressBook?mode=Create&HaulierContactId=0&nonEsdalContact=true&random=' + randomNumber, function () {
        stopAnimation();
        $('#overlay').show();
        $("#dialogue").show();
        $("#dialogue").modal("show");
    });
}
function searchNotifContact() {
    startAnimation();
    if ($('#hf_FromAnnotation').val() == 'True') {
        var searchColumn = $('#popupFilter').find("#DDsearchCriteria").val();
        var searchValue = $('#popupFilter').find("#SearchValue").val();
        var pageSize = $('#pageSizeVal').val();
        $("#dialogue1").load('../Contact/ContactList?page=' + pageNumber + '&pageSize=' + pageSize + '&isNotifContact=true' + '&isNotifContactSearch=true' + '&fromAnnotation=true' + '&SearchColumn=' + searchColumn + '&' + $('#popupFilter').find("#SearchValue").serialize(), function () {
            stopAnimation();
            $('.modal-backdrop').remove();
            $('#popupFilter').find("#DDsearchCriteria").val(searchColumn);
            $('#popupFilter').find("#SearchValue").val(searchValue);
            $('#pageSizeVal').val(pageSize);
            $('#pageSizeSelect').val(pageSize);
            $('#contactDivPopup').css('display', 'block');
            $('#contactDivPage').css('display', 'none');
            $('#overlay').css('display', 'block');
            $("#dialogue1").show();
            $('#exampleModalCenterCnt').modal('show');
            closeFilters();
        });
    }
    else {
        var searchColumn = $("#DDsearchCriteria").val();
        var pageSize = $('#pageSizeVal').val();
        $("#notif_contact_list").load('../Contact/ContactList?page=' + pageNumber + '&pageSize=' + pageSize + '&isNotifContact=true' + '&isNotifContactSearch=true' + '&SearchColumn=' + searchColumn + '&' + $("#SearchValue").serialize(), function () {
            $('#pageSizeVal').val(pageSize);
            $('#pageSizeSelect').val(pageSize);
            closeFilters();
            stopAnimation();
        });

    }
    closeFilters();
}
function clearAddressBookFilter() {
    if ($('#hf_FromAnnotation').val() == 'True') {
        $('#popupFilter').find("#DDsearchCriteria").val("1");
        $('#popupFilter').find("#SearchValue").val("");
    }
    else {
        $("#DDsearchCriteria").val("1");
        $("#SearchValue").val("");
    }
    searchNotifContact();

}
function openAnnotationFilter() {
    $('#popupFilter').html($('#filterDivAddressBook').html());
    openFilters();
}
function AddNonEsdalContact() {
    $('#overlay').css('display', 'block');
    var randomNumber = Math.random();
    var options = { "backdrop": "static", keyboard: true };
    $.ajax({
        type: "GET",
        url: '/AddressBook/ManageAddressBookDetails',
        contentType: "application/json; charset=utf-8",
        data: { "mode": 'create', "HaulierContactId": 0, "nonEsdalContact": true, "random": randomNumber },
        datatype: "json",
        success: function (data) {
            ManageAddressBookDetailsInit();
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