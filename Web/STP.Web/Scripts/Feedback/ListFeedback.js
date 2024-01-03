
let feedId;
var feedName;



$(document).ready(function () {
    //var pageSize = $(this).val();
    //$('#pageSizeVal').val(pageSize);
    SelectMenu(8);
    ChangeSearchCriteria();
    $(".view-feedbackDetails").on('click', ViewFeedbackdetail);
    $('body').on('change', '#DDsearchFeedback', function (e) {
        e.preventDefault();
        SetSearchText(this);
    });
    $('body').on('click', '#btn_ClearFilter', function (e) {
        e.preventDefault();
        ClearFilter(this);
    });
    $('body').on('click', '.view-feedbackDetails', function (e) {
        e.preventDefault();
        $(".view-feedbackDetails").on('click', ViewFeedbackdetail);
    });
    $('body').on('click', '#SearchFilter', function (e) {
        e.preventDefault();
        SearchFilter();
    });
   
    $('body').on('click', '#filterimage', function (e) {
        e.preventDefault();
        ClearFilter(this);
    });
    $('body').on('click', '.view-feedback', function (e) {
        e.preventDefault();
        ViewFeedback(this);
    });
    $('body').on('click', '.contact-details', function (e) {
        e.preventDefault();
        ViewContact(this);
    });
    /* $(".view-feedback").on('click', ViewContactDetail);*/




});
$('body').on('click', '.deletefdback', function (e) {

    e.preventDefault();
    var FeedBackId = $(this).data('feedbackid');
    var FeedbackTypeName = $(this).data('feedbacktypename');

    DeleteConfirmation(this, FeedBackId, FeedbackTypeName);
});
function ViewFeedbackdetail(data) {

    var FeedbackId = data.currentTarget.attributes.FeedbackId.value;
    var OpenCheck = data.currentTarget.attributes.OpenCheck.value;
    ViewFeedbackdetails(FeedbackId, OpenCheck);
}
function ViewFeedbackdetails(id, chk) {

    startAnimation();
    var options = { "backdrop": "static", keyboard: true };
    $.ajax({
        type: "GET",
        url: "../Feedback/ViewFeedbackPopup",
        //contentType: "application/json; charset=utf-8",
        data: { feedid: id, openChk: chk },
        datatype: "json",
        success: function (data) {
            $('#generalPopupContent').html(data);
            $('#generalPopup').modal(options);
            $('#generalPopup').modal('show');
            $('.loading').hide();
            stopAnimation();
        },
        error: function () {
            location.reload();
        }
    });
}
function ViewFeedback(data) {

    let FeedbackId = data.currentTarget.attributes.FeedBackId.value;
    let OpenCheck = data.currentTarget.attributes.OpenCheck.value;
    ViewFeedbacks(FeedbackId, OpenCheck);
}
function ViewContact(data) {

    var ContactId = data.currentTarget.attributes.ContactId.value;

    ViewContactDetail(ContactId);
}
function SetSearchText() {

    var index = $('#DDsearchFeedback option:selected').val();
    if (index == 0) {
        $('#Searchtext').attr('placeholder', 'All');
    }
    if (index == 1) {
        $('#Searchtext').attr('placeholder', 'Complaint');
    }
    if (index == 2) {
        $('#Searchtext').attr('placeholder', 'Suggestion');
    }
    if (index == 3) {
        $('#Searchtext').attr('placeholder', 'General complaint');
    }
    if (index == 4) {
        $('#Searchtext').attr('placeholder', 'Fault');
    }
}
function ChangeSearchCriteria() {
    SetSearchText();
}

function ViewFeedbacks(id, chk) {
    startAnimation();
    let options = { "backdrop": "static", keyboard: true };
    $.ajax({
        type: "GET",
        url: "../Feedback/ViewFeedbackPopup",
        contentType: "application/json; charset=utf-8",
        data: { feedid: id, openChk: chk },
        datatype: "json",
        success: function (data) {
            $('#generalPopupContent').html(data);
            $('#generalPopup').modal(options);
            $('#generalPopup').modal('show');
            $('.loading').hide();
            stopAnimation();
        },
        error: function () {
            location.reload();
        }
    });
}

function DeleteConfirmation(_this, FeedBackId, FeedbackTypeName) {
    feedid = $(_this).attr('id');
    feedName = $(_this).attr('name');
    let Msg = "Do you want to delete '" + "" + "'" + FeedbackTypeName + "'" + "" + "' ?";
    ShowWarningPopup(Msg, "DeleteFeedback");

}
function DeleteFeedback() {
    CloseWarningPopup();
    $.ajax({
        async: false,
        type: "POST",
        url: '../Feedback/DeleteFeedbackDetails',
        dataType: "json",
        //contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ feedBackId: feedid, FeedbackTypeName: feedName, }),
        beforeSend: function () {
            startAnimation();
        },
        processdata: true,
        success: function (result) {

            stopAnimation();
            if (result) {

                ShowModalPopup("'" + feedName + "'" + " " + "deleted successfully");
            }
            else {
                ShowErrorPopup("Deletion failed");
            }
        },
        error: function (result) {
            alert("Error")
            stopAnimation();
            ShowErrorPopup("Error on the page");
        }
    });

}

function ClearFilter() {
    $('#Searchtext').val('');

    $("#DDsearchFeedback").prop('selectedIndex', 0);
    $('#Searchtext').attr('placeholder', 'All');
    SearchFilter(isClear = true, isSort = false);
}





function SearchFilter(isClear = false, isSort = true) {
    var type = $('#DDsearchFeedback').val();
    var text = $('#Searchtext').val();

    if ($('#DDsearchFeedback').val() != 'Select' /*&& $('#Searchtext').val() != ''*/) {
        var url = '../Feedback/ListFeedback';
        $.ajax({
            url: url,
            type: 'POST',
            cache: false,
            data: {
                searchType: type, searchString: text, page: (isSort ? $('#pageNum').val() : 1), pageSize: $('#pageSizeVal').val(),
                sortType: $('#filters #SortTypeValue').val(), sortOrder: $('#filters #SortOrderValue').val(),
                isClear: isClear
            },
            beforeSend: function () {
                startAnimation();
            },
            success: function (response) {
                $("#manage-user").html('');
                $("#manage-user").html($(response).find('#manage-user').html());
                var filters = $('#manage-user').find('div#filters');
                closeFilters();
            },
            error: function (result) {
                location.readload();
            },
            complete: function () {
                stopAnimation();
            }
        });
    }
    else {
        ShowErrorPopup("Error in filter");
    }

}

function ViewFeedbackdetails(id, chk) {
    $("#overlay").show();
    startAnimation();
    $("#causionReport").load('../Feedback/ViewFeedbackPopup?feedid=' + id + '&openChk=' + chk, function () {
        $('#contactDetails').modal({ keyboard: false, backdrop: 'static' });
        $('#contactDetails').modal('show');
        stopAnimation();
    });
}

function ViewContactDetail(id) {
    $("#dialogue").html('');
    startAnimation();
    $("#dialogue").load('../Application/ViewContactDetails?ContactId=' + id, function () {
        $('#contactDetails').modal({ keyboard: false, backdrop: 'static' });
        $('#contactDetails').modal('show');
        stopAnimation();
        removescroll();

        $("#dialogue").show();
        $("#overlay").show();
    });

}
var sortTypeGlobal = 1;//0-asc
var sortOrderGlobal = 1;//name
function FeedbackSort(event, param) {
    sortOrderGlobal = param;
    sortTypeGlobal = event.classList.contains('sorting_asc') ? 1 : 0;
    $('#SortTypeValue').val(sortTypeGlobal);
    $('#SortOrderValue').val(sortOrderGlobal);
    SearchFilter(false, isSort = true);
}

//$('body').on('click', '#feedbackpaginator a', function (e) {
//    e.preventDefault();
//    startAnimation();
//    var page = getUrlParameterByName("page", this.href);
//    $('#pageNum').val(page);
//    SearchFilter(false, isSort = true);
//});

$('body').on('change', '.feedbackpaginator #pageSizeSelect', function () {
    var pageSize = $(this).val();
    $('#pageSizeVal').val(pageSize);
    var page = getUrlParameterByName("page", this.href);
    $('#pageNum').val(page);
  
    SearchFilter(false, isSort = true);
});
