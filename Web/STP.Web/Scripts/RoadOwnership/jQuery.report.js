


$(document).ready(function () {
    removeHLinksRdOwner();

    PaginateGridRdOwner();

    fillPageSizeSelectRdOwner();
    $('#btn_bck').click(function (e) {
        location.href = '@Url.Content("~/RoadOwnership/showRoadOwnership/")';
    });

    $('#span-close').click(function () {
        addscroll();//----------->> add scrollbar to the body
        $('#overlay').hide();
    });
});


function removeHLinksRdOwner() {
    $('#div_dispensation_org').find('.pagination').find('li a').removeAttr('href').css("cursor", "pointer");
    $('#div_dispensation_org').find('.pagination').find('.active a').css("z-index", 0);
}

function fillPageSizeSelectRdOwner() {
    var selectedVal = $('#pageSizeVal').val();
    $('#pageSizeSelect').val(selectedVal);
}

function SearchOrganisation() {
    
    var randomNumber = Math.random();
    var searchFlag = $('#searchFlag').val();
    var pageSize = $('#pageSizeVal').val();
    var pageNum = $('#pageNum').val();
    var searchKey = $('#orgName').val();
    $.ajax({
        url: '../../RoadOwnership/RoadOwnerReport',
        type: 'POST',
        data: { page: pageNum, pageSize: pageSize },
        cache: false,
        async: false,
        beforeSend: function () {
        },
        success: function (result) {

            $('#div_grid_org_list').html($(result).find('#div_grid_org_list').html());
            RoadOwnerReportInit();
        },
        error: function (jq, status, message) {

            alert('A jQuery error has occurred. Status: ' + status + ' - Message: ' + message);
        },
        complete: function () {
            removeHLinksRdOwner();
            addscroll();
        }
    });
    PaginateGridRdOwner();
}

function getInfoList2(pageNo1, pageSize1) {
    var pageNo = Number(pageNo1);
    var pageSize = Number(pageSize1);
    var startIndex = (pageNo - 1) * pageSize;
    var endIndex = (pageNo - 1) * pageSize + pageSize;
    var linkInfolist = roadOwnershipDetails.assignedLinkInfo.slice(startIndex, endIndex);
    return linkInfolist;

}
function changePageSizeDiffPopup(PageSize) {
    ;
    var randomNumber = Math.random();
    var linkInfoLen = roadOwnershipDetails.assignedLinkInfo.length;
    var PageSize = Number(PageSize);
    var linkInfolist = getInfoList2(1, PageSize);
    $.ajax({
        url: '../../RoadOwnership/RoadOwnerReport',
        //contentType: 'application/json; charset=utf-8',
        type: 'POST',
        data: JSON.stringify({
            assignedLinkInfoArray: linkInfolist, newOwnerList: roadOwnershipDetails.newOwnerList, assignedLinkInfoCount: linkInfoLen, page: 1, pageSize: PageSize
        }),
        cache: false,
        async: false,
        beforeSend: function () {
            $('.loading').show();
        },
        success: function (result) {

            $('#div_dispensation_org').html($(result).find('#div_dispensation_org').html());
            RoadOwnerReportInit();
            $('#pageSizeVal').val(PageSize);
            $('#pageSizeSelect').val(PageSize);
            var x = fix_tableheader();
   
            if (x == 1) $('#tableheader').show();

        },
        error: function (xhr, textStatus, errorThrown) {

        },
        complete: function () {
            $('.loading').hide();
            removeHLinksRdOwner();
            PaginateGridRdOwner();
        }
    });
}


function PaginateGridRdOwner() {
   
    //method to paginate through page numbers
    $('#div_dispensation_org .pagination').on('click', 'li a', function () {

        ;
        var smt = $(this).closest('li').attr('class');
        switch (smt) {
            case 'active':
                break;
            case 'PagedList-skipToLast':
                var pageCount = $('#div_dispensation_org').find('#TotalPages').val();
                AjaxPaginationRdOwner(pageCount);
                break;
            case 'PagedList-skipToNext':
                var thisPage = $('#div_dispensation_org').find('.active').find('a').html();
                var nextPage = parseInt(thisPage) + 1;
                AjaxPaginationRdOwner(nextPage);
                break;
            case 'PagedList-skipToFirst':
                AjaxPaginationRdOwner(1);
                break;
            case 'PagedList-skipToPrevious':
                var thisPage = $('#div_dispensation_org').find('.active').find('a').html();
                var prevPage = parseInt(thisPage) - 1;
                AjaxPaginationRdOwner(prevPage);
                break;
            default:
                var pageNum = $(this).html();
                AjaxPaginationRdOwner(pageNum);
                break;
        }
    });
}

//function Ajax call fro pagination
function AjaxPaginationRdOwner(pageNum) {
    ;
    var randomNumber = Math.random();
    var pageSize = $('#pageSizeVal').val();
    var keyword = $('#SearchString').val();
    var linkInfoLen = roadOwnershipDetails.assignedLinkInfo.length;
    var searchFlag = $('#searchFlag').val();
    var linkInfolist = getInfoList2(pageNum,pageSize);
    $.ajax({
        url: '../../RoadOwnership/RoadOwnerReport',
        //contentType: 'application/json; charset=utf-8',
        type: 'POST',
        data: JSON.stringify({
            assignedLinkInfoArray: linkInfolist, newOwnerList: roadOwnershipDetails.newOwnerList, assignedLinkInfoCount: linkInfoLen, page: pageNum, pageSize: pageSize
        }),
        //data: { page: pageNum, pageSize: pageSize },
        cache: false,
        async: false,
        beforeSend: function () {
            $('.loading').show();
        },
        success: function (result) {

            $('#div_dispensation_org').html($(result).find('#div_dispensation_org').html());
            RoadOwnerReportInit();
            //var x = fix_tableheader();

            //if (x == 1) $('#tableheader').show();

        },
        error: function (xhr, textStatus, errorThrown) {

        },
        complete: function () {

            $('.loading').hide();
            $('#SearchString').val(keyword);

            removeHLinksRdOwner();
            PaginateGridRdOwner();
        }
    });
}
function msieversion() {
    var ua = window.navigator.userAgent;
    var msie = ua.indexOf("MSIE ");
    if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) // If Internet Explorer, return true
    {
        return true;
    } else { // If another browser,
  return false;
}
return false;
}
function exportToCsv() {
    ;
    csvImportFlag = true;
    var linkInfoLen = roadOwnershipDetails.assignedLinkInfo.length;
    var linkInfolist = roadOwnershipDetails.assignedLinkInfo;
    $.ajax({
        url: '../../RoadOwnership/RoadOwnerReport',
        //contentType: 'application/json; charset=utf-8',
        type: 'POST',
        data: JSON.stringify({
            assignedLinkInfoArray: linkInfolist, newOwnerList: roadOwnershipDetails.newOwnerList, assignedLinkInfoCount: linkInfoLen, page: 1, pageSize: 5, view: false
        }),
        //data: { page: pageNum, pageSize: pageSize },
        cache: false,
        async: false,
        beforeSend: function () {
            $('.loading').show();
        },
        success: function (result) {
            var model = result.pagedRoadOwnershipObj;

            if (model[0].ManagerName != null) {
                var A = [['Link ID', 'Old manager organisation name', 'New manager organisation name']];
                for (var i = 0; i < model.length; i++) {
                    A.push([model[i].LinkId, model[i].HdnManagerName, model[i].ManagerName]);
                }
            }
            else if (model[0].LocalAuthorityName != null) {
                var A = [['Link ID', 'Old local authority organisation name', 'New local authority organisation name']];
                for (var i = 0; i < model.length; i++) {
                    A.push([model[i].LinkId, model[i].HdnLocalAuthorityName, model[i].LocalAuthorityName]);
                }
            }
            else if (model[0].HaMacName != null) {
                var A = [['Link ID', 'Old NH MAC organisation name', 'New NH MAC organisation name']];
                for (var i = 0; i < model.length; i++) {
                    A.push([model[i].LinkId, model[i].HdnHaMacName, model[i].HaMacName]);
                }
            }
            else if (model[0].PoliceName != null) {
                var A = [['Link ID', 'Old police organisation name', 'New police organisation name']];
                for (var i = 0; i < model.length; i++) {
                    A.push([model[i].LinkId, model[i].HdnPoliceName, model[i].PoliceName]);
                }
            }
            var csvRows = [];

            for (var i = 0, l = A.length; i < l; ++i) {
                csvRows.push(A[i].join(','));
            }
           
            if (msieversion()) {
                var csvString = csvRows.join("\r\n");
                var IEwindow = window.open();
                IEwindow.document.write(csvString);
                IEwindow.document.close();
                IEwindow.document.execCommand('SaveAs', true, 'Details.csv');
                IEwindow.close();
            } else
            {
                var csvString = csvRows.join("%0A");
                var a = document.createElement('a');
                a.href = 'data:attachment/csv,' + csvString;
                a.target = '_blank';
                a.download = 'LinkDetails.csv';
                document.body.appendChild(a);
                a.click();
            }
            RoadOwnerReportInit();
        },
        error: function (xhr, textStatus, errorThrown) {

        },
        complete: function () {
            csvImportFlag = false;
            $('.loading').hide();
        }
    });
}
