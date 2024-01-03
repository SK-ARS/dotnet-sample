$(document).ready(function () {
    searchButton();
    expandSearchClick();
});

function ajaxSearch() {
    var search = $('#Search').val();
    var searchData = [];
    var url = $('#SearchUrl').val();
    $('.left-sub').each(function () {
        if ($(this).find('input:radio').is(':checked')) {
            $(this).find('.left-input-sub input,select').each(function () {
                var item = $(this).attr('id');
                var value = $(this).val();
                var pushData = { SearchItem: item, SearchValue: value }
                searchData.push(pushData);
            });
        }
    });
    var searchValue = { SearchItem: 'Search', SearchValue: search };
    searchData.push(searchValue);
    var data = { SearchCollection: searchData };
    $.ajax({
        type: 'POST',
        dataType: 'json',
        //contentType: 'application/json; charset=utf-8',
        url: url,
        data: JSON.stringify(data),
        success: function (Result) {
            var dataCollection = Result;
            if (dataCollection.result.length > 0) {
                $('#search-result').append('<select id="search-select" multiple="multiple"></select>');
                for (var i = 0; i < dataCollection.result.length; i++) {
                    $('#search-select').append('<option value="' + dataCollection.result[i] + '">' + dataCollection.result[i] + '</option>');
                }
            }
        },
        error: function (ex, e, err) {
            alert(err);
        }
    });
}

function searchButton() {
    $('#btn_Search').click(function () {
        ajaxSearch();
        return false;
    });
}


function expandSearch(searchValue) {
    var url = $('#ExpandedSearchUrl').val();
    $.ajax({
        type: 'POST',
        dataType: 'html',
        //contentType: 'application/json; charset=utf-8',
        url: url,
        data: '{ searchValue: "'+searchValue+'" }',
        success: function (Result) {
            $('.left-panel').hide();
            $('#search-details-div').show();
            $('#search-details-div').html(Result);
        },
        error: function (ex, e, err) {
            
        }
    });
}



function expandSearchClick() {
    $('#search-select').live('change', function () {
        var searchValue = $(this).val();
        expandSearch(searchValue);
    });
}