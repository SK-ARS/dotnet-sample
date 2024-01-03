var pageSize = $('#pageSizeVal').val();
$(document).ready(function () {
    $('#pageSizeSelect').val(pageSize);
    var USERID = $('#UserType').val();
    if ($('#hf_IsPlanMovmentGlobal').length == 0) {
        if (USERID == '696008') {
            SelectMenu(4);
        }
        else {
            SelectMenu(6);
        }
    }

    $('body').on('click', '#contactListContainer #close-filter', function (e) {
        e.preventDefault();
        if (typeof closeFiltersAddressBook != 'undefined')
            closeFiltersAddressBook(this);
        else
            closeFilters();
        
    });
    $('body').on('click', '#clear-address', function (e) {
        e.preventDefault();
        clearContactFilter(this);
    });
    $('body').on('click', '#search-contact', function (e) {
        e.preventDefault();
        searchContact();
    });
    
    $('body').on('click', '#contactListContainer #filterimage', function (e) {
        e.preventDefault();
        clearContactFilter(this);
    });
    $('body').on('change', '#DDsearchCriteria', function (e) {
        SetSearchText();
    });
    //ChangeSearchCriteria();
    $('body').on('keydown', '#SearchPanelForm input', function (e) {
        if (e.key == "Enter" || e.which == 13 || e.keyCode == 13) {
            e.preventDefault();
            searchContact();
        }
    });

});
function clearContactFilter() {
    if ($('#hf_FromAnnotation').val() == 'True') {
        $('#popupFilter').find("#DDsearchCriteria").val("1");
        $('#popupFilter').find("#SearchValue").val("");
        showContactListFormPopUp();
    }
    else {
        $("#DDsearchCriteria").val("1");
        $("#SearchValue").val("");
        showContactListForm();
    }

    closeFilters();
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
// showing user-setting-info-filter


jQuery(document).ready(function ($) {
    $(document).on('click', '.bs-canvas-overlay', function () {
        $('.bs-canvas-overlay').remove();
        $('#filters').css('margin', '0 -400px 0 0');
        return false;
    });
});


function SetSearchText() {

    var index = $('#DDsearchCriteria option:selected').val();
    if (index == 1) {
        $('#SearchValue').val('');
        $('#SearchValue').attr('placeholder', 'Contact name');
    }
    if (index == 2) {
        $('#SearchValue').val('');
        $('#SearchValue').attr('placeholder', 'Organisation');
    }
    if (index == 4) {
        $('#SearchValue').val('');
        $('#SearchValue').attr('placeholder', 'Phone');
    }
    if (index == 5) {
        $('#SearchValue').val('');
        $('#SearchValue').attr('placeholder', 'E-mail');
    }

}
function ChangeSearchCriteria() {
    SetSearchText();
}



function searchContact(isClear, isSort = false) {
    
    var searchColumn = $("#DDsearchCriteria").val();
    var searchValue = $("#SearchValue").val();
    var pageSize = $('#pageSizeVal').val();
    startAnimation();
    $.ajax({
        url: '../Contact/ContactList?page=' + (isSort ? $('#pageNum').val() : 1) + '&pageSize=' + pageSize +
            '&Iscontactview=true' + '&isNotifContact=true' + '&isNotifContactSearch=true' +
            '&SearchColumn=' + searchColumn + '&SearchValue=' + searchValue
            + '&sortType=' + $("#SortTypeValue").val()
            + '&sortOrder=' + $("#SortOrderValue").val()
            + '&isClear=' + isClear,
        type: 'GET',
        cache: false,
        //async: false,
        beforeSend: function () {
        },
        success: function (response) {
            closeFilters();
            stopAnimation();
            if ($("#notif_contact_list").length > 0)
                $('#notif_contact_list').html(response);
            else if ($('#portal-table').length > 0)
                $('#portal-table').html(response);
            else if ($('#General').length > 0)
                $('#General').html(response);
        }
    });
}
function clearContactFilter() {
    $("#DDsearchCriteria").val("1");
    $("#SearchValue").val("");
    //showContactListForm();
    searchContact(true);
    closeFilters();
    $('#SearchValue').attr('placeholder', 'Contact name');
    $('#filterimage').hide();
}

function showContactListForm() {
    startAnimation();
    // ddlselval = $('#MoveClass :selected').val();
    var pageSize = $('#pageSizeVal').val();
    $("#portal-table").load('../Contact/ContactList?page=' + $('#pageNum').val() + '&pageSize=' + pageSize + '&Iscontactview=true' + '&isNotifContact=true' + '&isNotifContactSearch=true', function () {
        stopAnimation();
    });
}


$(document).ready(function () {

    if ($('#FromAnnotation').val() == 'True') {
        $('#contactDivPopup').css('display', 'block');
        $('#contactDivPage').css('display', 'none');

    }
    else {
        $('#contactDivPopup').css('display', 'none');
        $('#contactDivPage').css('display', 'block');


    }
    var searchColumn = $("#DDsearchCriteria").val();
    var searchValue = $("#SearchValue").val();
});

function BacktoAffectedParties() {

    startAnimation();
    $('#leftpanel').show();
    $('#route-assessment').show();
    $("#General").hide();
    $("#backbutton").show();
    stopAnimation();
}


function ViewContactDetail(id) {

    startAnimation();
    $('#exampleModalCenter22').hide();
    $("#dialogue1").load('../Application/ViewContactDetails?ContactId=' + id + "", function () {
        removescroll();
        $('#contactDetails').modal('hide');
        $('#contactDetailsForMap').modal('show');
        $("#dialogue1").show();
        $("#overlay").show();
        //startAnimation();
    });
}
function closeContactPopup() {
    $('#contactDetailsForMap').modal('hide')
    $("#dialogue1").hide();
    $('#exampleModalCenter22').show();
}
function CloseAnnotationPopUp() {
    $("#overlay").css('display', 'block');
    $('#dialogue1').hide();
    $('#dialogue').show();

}
function CloseAnnotationErrorPopUp() {
    $("#overlay").css('display', 'block');
    $('#ErrorPopupWithAction').modal('hide');
}

$('#addressBookPopupPaginator').on('click', 'a', function (e) {
    if (this.href == '') {
        return false;
    }
    else {
        $.ajax({
            url: this.href,
            type: 'GET',
            cache: false,
            success: function (result) {
                $('#dialogue1').html(result);
                $('#exampleModalCenterCnt').modal('show');
            },

            complete: function () {
                //stopAnimation();

            }
        });
        return false;
    }
});

$('body').on('click', '#contactPagination a', function (e) {
    e.preventDefault();
    var page = getUrlParameterByName("page", this.href);
    $('#pageNum').val(page);
    searchContact(false, isSort = true);//using sorting as true to avoid page reset
});
$('body').on('change', '#contactListContainer #pageSizeSelect', function () {
    var page = getUrlParameterByName("page", this.href);
    $('#pageNum').val(page);
    var pageSize = $(this).val();
    $('#pageSizeVal').val(pageSize);
    searchContact(false, isSort = true);
});

var sortTypeGlobal = 0;//0-asc
var sortOrderGlobal = 1;//type
function ContactSort(event, param) {
    sortOrderGlobal = param;
    sortTypeGlobal = event.classList.contains('sorting_asc') ? 1 : 0;
    $('#SortTypeValue').val(sortTypeGlobal);
    $('#SortOrderValue').val(sortOrderGlobal);
    //searchContact(false, isSort = true);
    searchContact();
}
