    $('body').on('click', '.manage', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var mode = $(this).data('create');
        ManageInformation(mode,id);
    });

    $('body').on('click', '.information', function (e) {
        e.preventDefault();
        var contentid = $(this).data('contentid');
        var mode = $(this).data('mode');
        ManageInformation(mode, contentid);
    });
    $('body').on('click', '#btn-delete', function (e) {
        e.preventDefault();
        var contentid = $(this).data('contentid');
        var name = $(this).data('name');
        Delete(contentid, name);
    });
    function Delete(ContentId, ContentName) {
        var Msg = "Do you want to delete '" + ContentName + "'" + "' ?";
        ShowWarningPopup(Msg, 'DeleteData(' + ContentId + ',"' + ContentName + '")');
    }

    function DeleteData(deletedContentId, deletedContentName) {
        CloseWarningPopup();
        var params = '{"deletedContactId":"' + deletedContentId + '"}';
        $.ajax({
            async: false,
            type: "POST",
            url: '@Url.Action("DeleteInformation", "Information")',
            dataType: "json",
            //contentType: "application/json; charset=utf-8",
            data: params,
            processdata: true,
            success: function (result) {
                console.log(result.Success);
                if (result.Success) {
                    ShowSuccessModalPopup('"' + deletedContentName + '"  deleted successfully', 'RedirectToMainPage');
                }
                else {
                    ShowErrorPopup('Error on the page.');
                }
            },
            error: function (result) {
            }
        });
    }

    function RedirectToMainPage() {
        CloseSuccessModalPopup();
        window.location.reload();
    }
    function InformationSort(event, param) {
        var sort_Order = param;
        var presetFilter = 3;
        if (event.classList.contains('sorting_asc')) {
            presetFilter = 1;
        }
        else if (event.classList.contains('sorting_desc')) {
            presetFilter = 3;
        }
        else if (!event.classList.contains('sorting_asc') && !event.classList.contains('sorting_desc')) {
            presetFilter = 1;
        }
        var url = '../Information/ManageNews';
         if ('@pageName' == 'news story')
             url = '../Information/ManageNews';
        else if ('@pageName' == 'info story')
             url = '../Information/ManageHelpAndInformations';
        else if ('@pageName' == 'download')
             url = '../Information/ManageDocuments';
         else if ('@pageName' == 'link')
             url = '../Information/ManageLinks';
        $.ajax({
            url: url,
            type: 'POST',
            cache: false,
            data: { sortType: presetFilter, sortOrder: sort_Order },
            beforeSend: function () {
                startAnimation();
            },
            success: function (response) {
                $('section#banner').html('');
                $('section#banner').html($(response).find('section#banner'));
                $('.esdal-table > thead .sorting').removeClass('sorting_asc sorting_desc');

                $(".esdal-table > thead .sorting").each(function () {
                    var item = $(this);
                    if ((presetFilter == 0 || presetFilter == 1) && item.find('span').attr('param') == sort_Order) {
                        item.addClass('sorting_desc');
                    }
                    else if (presetFilter == 3 && item.find('span').attr('param') == sort_Order) {
                        item.addClass('sorting_asc');
                    }
                });
            },
            error: function (result) {
                location.readload();

            },
            complete: function () {
                stopAnimation();
            }
        });
    }
    $('#newspaginator').on('click', 'a', function (e) {
        var order = -1;
        var param;
        if (this.href == '') {
            return false;
        }
        else {
            var search = '';
            if ($('.esdal-table > thead .sorting_desc').length == 1) {
                param = $('.esdal-table > thead .sorting_desc >span').attr('param');
                order = 0;
                search = '&sortType=' + order + '&sortOrder=' + param;
            }
            else if ($('.esdal-table > thead .sorting_asc').length == 1) {
                param = $('.esdal-table > thead .sorting_asc >span').attr('param');
                order = 3;
                search = '&sortType=' + order + '&sortOrder=' + param;
            }

            $.ajax({
                url: this.href + search,
                type: 'GET',
                cache: false,
                success: function (result) {
                    //
                    $('section#banner').html('');
                    $('section#banner').html($(result).find('section#banner'));

                    if (order != -1) {
                        $(".esdal-table > thead .sorting").each(function () {
                            var item = $(this);
                            if (order == 0 && item.find('span').attr('param') == param) {
                                item.addClass('sorting_desc');
                            }
                            else if (order == 3 && item.find('span').attr('param') == param) {
                                item.addClass('sorting_asc');
                            }
                        });
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
    });
