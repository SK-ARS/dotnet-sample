function ManageOrganisationList() {
    $.ajax({
        url: '../Organisation/ListOrganisation',
        data: {},
        type: 'GET',
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {

            $("#manage-user").html(result);
            let filters = $('#manage-user').find('div#filters');
            $(filters).insertAfter('section#banner');
            OrganisationListInit();
            initAutoComplete();
        },
        error: function (xhr, textStatus, errorThrown) {
            location.reload();
        },
        complete: function () {
            stopAnimation();
            $('#organisationheader').hide();
        }
    });
}
function initAutoComplete() {
    $("#SearchString").autocomplete({
        appendTo: $("#SearchString").parent(),
        source: function (request, response) {
            ;
            $.ajax({
                url: '../Dispensation/OrganisationSummary',
                dataType: "json",
                data: {
                    SearchString: request.term, page: 0, pageSize: 0
                },
                success: function (data) {
                    ;
                    response($.map(data, function (item) {
                        return { label: item.OrganisationName };
                    }));
                },
                error: function (jqXHR, exception, errorThrown) {
                    console.log(errorThrown);
                }
            });
        },
        minLength: 2,
        select: function (event, ui) {
            $('#SearchString').val(ui.item.label);
            return false;
        },
        focus: function (event, ui) {
            //$("#SearchString").val(ui.item.label);
            return false;
        }
    });
}
function OrganisationSort(event, param) {
    let sort_Order = param;
    let sort_Type = 1;
    if (event.classList.contains('sorting_asc')) {
        sort_Type = 1;
    }
    else if (event.classList.contains('sorting_desc')) {
        sort_Type = 3;
    }
    else if (!event.classList.contains('sorting_asc') && !event.classList.contains('sorting_desc')) {
        sort_Type = 1;
    }
    let url = '../Organisation/ListOrganisation';
    $.ajax({
        url: url,
        type: 'POST',
        cache: false,
        data: { sortType: sort_Type, sortOrder: sort_Order },
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {
            $('#banner-container').find('div#filters').remove();
            $("#manage-user").html('');
            $("#manage-user").html(response);
            let filters = $('#manage-user').find('div#filters');
            $(filters).insertAfter('section#banner');
            $('.esdal-table > thead .sorting').removeClass('sorting_asc sorting_desc');

            $(".esdal-table > thead .sorting").each(function () {
                let item = $(this);
                if ((sort_Type == 0 || sort_Type == 1) && item.find('span').attr('param') == sort_Order) {
                    item.addClass('sorting_desc');
                }
                else if (sort_Type == 3 && item.find('span').attr('param') == sort_Order) {
                    item.addClass('sorting_asc');
                }
            });
        },
        error: function (result) {
            location.readload();
        },
        complete: function () {
            stopAnimation();
            $('#organisationheader').hide();
        }
    });
}
$(document).ready(function () {
    if ($('#hf_IsPlanMovmentGlobal').length == 0) {
        SelectMenu(8);
        ManageOrganisationList();
    }
    $('body').on('click', '#createOrg', function (e) {
        e.preventDefault();
        CreateOrganisation(this);
    });
});
function CreateOrganisation() {

    let isAccessible = $('#EditForAdmin').val();
    if (isAccessible == 'False') {
        ShowErrorPopup('You are not authorized to create');
    }
    else {
        $("#manage-user").remove();
        startAnimation();
        $("#createUser").load("../Organisation/CreateOrganisation?mode=Save", function () {
            stopAnimation();
        });
        /*window.location.href = '../Organisation/CreateOrganisation';*/
            
       /* });*/
        $("#manage-user").hide();
    }
}
function BackToManageOrg() {
    startAnimation();
    location.reload();
}