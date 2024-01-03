    $("#SearchString").val($("#hdnSearchString").val());
    if ($("#hdnSearchValid").val() == 1) {
        $('#ShowValid').prop('checked', true);
    }
    else {
        $('#ShowValid').prop('checked', false);
    }
    function ShowValidNe() {
        if ($("#Isval").is(':checked')) {
            $('#Isval').val(1);

        } else {
            $('#Isval').val(0);
        }

        @*var pageSize  = $('#hf_pageSize').val(); 
        var SearchString = $('#SearchString').val();
        $.ajax({
            url: '@Url.Action("ListNEHaulier", "NENNotification")',
            type: 'GET',
            cache: false,
            async: false,
            data: { pageSize: pageSize, SearchString: SearchString, Isval: IsValid },
            beforeSend: function () {
            },
            success: function (result) {
                $('#div_Organisation').html($(result).find('#div_Organisation').html());
                $('#pageSizeVal').val(pageSize);
                $('#pageSizeSelect').val(pageSize);
                $('#pagesize').val(pageSize);

            },
            error: function (xhr, textStatus, errorThrown) {
                //other stuff
                location.reload();
            },
            complete: function () {
            }
        });*@
        }

