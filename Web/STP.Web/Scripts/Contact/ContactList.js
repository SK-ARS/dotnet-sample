$(document).ready(function () {
    $('body').on('click', '#backToAffectedParties', function () { window['BacktoAffectedParties'](); });
    $('body').on('click', '#closeannotationcontact', function (e) {
        e.preventDefault();
        CloseAnnotationPopUp(this);
    });
    //$('body').on('click', '#addressBookPaginator a', function (e) {
    //    e.preventDefault();
    //    var page = getUrlParameterByName("page", this.href);
    //    $('#pageNum').val(page);
    //    searchContact(false, true);
    //});
    $('body').on('click', '#view-contact1', function (e) {
        e.preventDefault();
        var contactid = $(this).data('viewcontact');
        ViewContactDetail(contactid);
    });
    $('body').on('click', '#view-contact2', function (e) {
        e.preventDefault();
        var contactid = $(this).data('viewcon1');
        ViewContactDetail(contactid);
        stopAnimation();
    });
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
function addressBookPaginator() {
    //$('#addressBookPaginator').on('click', 'a', function (e) {

    if (this.href == '') {
        return false;
    }
    else {
        startAnimation();
        var FromAnnotation = $("#FromAnnotation").val();
        var searchColumn = '';
        var searchValue = '';
        var search = '';
        var searchresult = '';
        if (FromAnnotation == 'True') {
            searchColumn = $('#popupFilter').find("#DDsearchCriteria").val();
            searchValue = $('#popupFilter').find("#SearchValue");
            searchresult = $('#popupFilter').find("#SearchValue").val();
        }
        else {
            searchColumn = $("#DDsearchCriteria").val();
            searchValue = $("#SearchValue");
            searchresult = $("#SearchValue").val();
        }
        if (searchresult != '' && searchresult != undefined) {
            search = '&SearchColumn=' + searchColumn + '&' + searchValue.serialize();
        }
        $.ajax({
            url: this.href + search,
            type: 'GET',
            cache: false,
            success: function (result) {

                if ($('#Isconview').val() == 'False') {

                    $('#notif_contact_list').html(result);
                }
                else {

                    $('#portal-table').html(result);
                    $('html, body').animate({ scrollTop: 0 });

                }
                stopAnimation();
            },
            error: function (xhr, status, error) {
                stopAnimation();
            },
            complete: function () {
                stopAnimation();

            }
        });
        return false;
    }
}
function removeHrefLinksContactList(div = '#exampleModalCenterCnt') {
    $(div).find('.pagination').find('li a').removeAttr('href').css("cursor", "pointer");
}
function PaginateContactList(cntrId = '#exampleModalCenterCnt') {

    $(cntrId).find('.pagination').find('li').not('.active, .PagedList-skipToLast, .PagedList-skipToNext, .PagedList-skipToFirst, .PagedList-skipToPrevious').find('a').click(function () {

        var pageNum = $(this).html();
        AjaxPaginationForContactList(pageNum);
    });
    $(cntrId).find('.PagedList-skipToLast').click(function () {

        var pageCount = $('#Contactlistpagecount').val();
        AjaxPaginationForContactList(pageCount);
    });
    $(cntrId).find('.PagedList-skipToFirst').click(function () {
        AjaxPaginationForContactList(1);
    });
    $(cntrId).find('.PagedList-skipToNext').click(function () {
        var thisPage = $(cntrId).find('.active').find('a').html();
        var nextPage = parseInt(thisPage) + 1;
        AjaxPaginationForContactList(nextPage);
    });
    $(cntrId).find('.PagedList-skipToPrevious').click(function () {
        var thisPage = $(cntrId).find('.active').find('a').html();
        var prevPage = parseInt(thisPage) - 1;
        AjaxPaginationForContactList(prevPage)

    });
}
function AjaxPaginationForContactList(pageNum) {

    var pageSize = $('#hf_pageSize').val();
    $("#dialogue1").load('../Contact/ContactList?page=' + pageNum + '&pageSize=' + pageSize + '&isNotifContact=true' + '&isNotifContactSearch=true' + '&fromAnnotation=true', function () {
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
        stopAnimation();
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
    Resize_PopUp(580);
    $('#dialogue').show();

}
function CloseAnnotationErrorPopUp() {
    $("#overlay").css('display', 'block');
    $('#ErrorPopupWithAction').modal('hide');
}
var sortTypeGlobal = 0;//0-asc
var sortOrderGlobal = 1;//type
function ContactSort(event, param) {

    sortOrderGlobal = param;
    sortTypeGlobal = event.classList.contains('sorting_asc') ? 1 : 0;
    $('#SortTypeValue').val(sortTypeGlobal);
    $('#SortOrderValue').val(sortOrderGlobal);
    searchContact(false, true);
}
