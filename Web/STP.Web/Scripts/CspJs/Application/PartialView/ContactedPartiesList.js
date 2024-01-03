
    var pageSizeTemp  = $('#hf_pageSize').val(); 

    function ViewContactedParties(id) {

        removescroll();
        //$("#dialogue").load("../Application/ViewContactDetails?ContactId=" + id);
        //$("#dialogue").show();
        //$("#overlay").show();
        $("#dialogue").load("../Application/ViewContactDetails", { ContactId: id }, function () {
            stopAnimation();
            $('#contactDetails').modal({ keyboard: false, backdrop: 'static' });
            $("#dialogue").show();
            $("#overlay").show();
            $('#contactDetails').modal('show');

        });
    }

    $(document).ready(function () {
       
        $(".viewcontactparties").on('click', ViewContactParties);
        
        $(".contactonbehalfofid").on('click', ViewContactParty );
        selectedmenu('Movements');
        fillPageSizeSelect();
        removeHLinks();
        PaginationAjax();
    });
    function ViewContactParties(e) {
        var arg1 = e.currentTarget.dataset.arg1;

        ViewContactedParties(arg1);
    }
    function ViewContactParty(e) {
        var arg1 = e.currentTarget.dataset.arg1;

        ViewContactedParties(arg1);
    }
    function fillPageSizeSelect() {
        var selectedVal = $('#pageSizeVal').val();
        $('#pageSizeSelect').val(selectedVal);
    }

    function changePageSize(_this) {

        $("#overlay").show();
        $('.loading').show();

        var pageSize = $(_this).val();
        var aID = @ViewBag.analysisID;

        $.ajax({
            url: '../Contact/ContactedPartiesList',
            type: 'GET',
            cache: false,
            async: false,
            data: {pageSize: pageSize , analysisID: aID },
            beforeSend: function () {
                startAnimation();
            },
            success: function (result) {
                $('#div_list').html(result);
                $('#pageSizeVal').val(pageSize);
                $('#pageSizeSelect').val(pageSize);
                var x = fix_tableheader();
                if (x == 1) $('#tableheader').show();
            },
            error: function (xhr, textStatus, errorThrown) {

            },
            complete: function () {
                $("#overlay").hide();
                $('.loading').hide();

            }
        });
    }

    function removeHLinks() {
        $('.pagination').find('li a').removeAttr('href').css("cursor", "pointer");
    }



    function PaginationAjax() {
        $('.pagination').find('li').not('.active, .PagedList-skipToLast, .PagedList-skipToNext, .PagedList-skipToFirst, .PagedList-skipToPrevious').find('a').click(function () {

            var pageNum = $(this).html();
            ContactPagination(pageNum);
        });
        PaginateToLastPage();
        PaginateToFirstPage();
        PaginateToNextPage();
        PaginateToPrevPage();

        //$('.pagination').find('li:not(.active, .disabled)').find('a').live('click', function () {
        //    var _this = $(this);
        //    //this_element = $(this);
        //    var url = _this.attr('href');
        //    if (url != undefined) {
        //        var pagenum = _this.attr('href').split('page=');
        //        $('#form_pagination').attr('action', url);
        //        $('#form_pagination').find('#page').val(pagenum[pagenum.length - 1].match(/\d+/g)[0]);
        //        $('#form_pagination').submit();
        //    }
        //    return false;
        //});
    }
    function ContactPagination(pageNo) {
        var analysisId = $('#analysisId').val();
        var url = '@Url.Action("ContactedPartiesList", "Contact")';
            $.ajax({
                url: url,
                type: 'POST',
                data: {
                    page: pageNo, pageSize: @pageNum, analysisID:@ViewBag.analysisID
                },
                beforeSend: function () {
                    startAnimation();
                },
                success: function (page) {
                    $('#tab_8').html(page);
                    $("#tab_8").show();
                    CheckSessionTimeOut();
                },
                error: function (err, ex, xhr) {
                    
                    showWarningPopDialog('An error occured. Please try again.', 'Ok', '', 'WarningCancelBtn', '', 1, 'error');
                },
                complete: function () {
                    stopAnimation();
                    contactedParties = true;
                }
            });
    }
    function PaginateToLastPage() {
        $('.PagedList-skipToLast').click(function () {
            var pageCount = $('#TotalPages').val();
            ContactPagination(pageCount);
        });
    }
    function PaginateToFirstPage() {
        $('.PagedList-skipToFirst').click(function () {
            ContactPagination(1);
        });
    }
    function PaginateToNextPage() {
        $('.PagedList-skipToNext').click(function () {
            var thisPage = $('.active').find('a').html();
            var nextPage = parseInt(thisPage) + 1;
            ContactPagination(nextPage);
        });
    }
    function PaginateToPrevPage() {
        $('.PagedList-skipToPrevious').click(function () {
            var thisPage = $('.active').find('a').html();
            var prevPage = parseInt(thisPage) - 1;
            ContactPagination(prevPage);
        });
    }
