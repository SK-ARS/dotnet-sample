$(document).ready(function () {
    searchButton();
    expandSearchClick();
    backToSearch();
});

function ajaxSearch() {
    /*Search Select Set height*/
    var selectTop = $('.left-panel-inner').outerHeight();
    var contentHeigth = $('#left-panel').outerHeight();
    var selectHeight = contentHeigth - selectTop-10;
    /*Ends here*/

    var search = $('#Search').val();
    var searchData = [];
    var url = $('#SearchUrl').val();
    var radioName = "";
    $('.left-sub').each(function () {
        if ($(this).find('input:radio').is(':checked')) {
            radioName = $(this).find('input:radio').val();
            $(this).find('.left-input-sub input,select').each(function () {
                var name = $(this).closest('.left-sub').find('input:radio').val();
                var item = $(this).attr('id');
                var value = $(this).val();
                var pushData = { SearchItem: item, SearchValue: value ,Name:name}
                searchData.push(pushData);

                $('#SearchType').val(name);
            });
        }
    });
    var searchValue = { SearchItem: 'Search', SearchValue: search, Name: radioName };
    searchData.push(searchValue);
    var data = { SearchCollection: searchData };
    $.ajax({
        type: 'POST',
        dataType: 'json',
        //contentType: 'application/json; charset=utf-8',
        url: url,
        data: JSON.stringify(data),
        beforeSend: function (xhr) {
            if ($('#search-select').length > 0) {
                $('#search-select').remove();
            }
            $('#search-result').append('<div id="wait-icon"><img src="../../Content/Images/loading.gif" id="wait-image"/></div>');
            //xhr.overrideMimeType("text/plain; charset=x-user-defined");
        }
    }).done(function (Result) {
        if ($('#search-select').length > 0) {
            $('#search-select').remove();
        }
        var dataCollection = Result;
        if (dataCollection.result.length > 0) {
            $('#search-result').append('<select id="search-select" multiple="multiple" style="width:100%;height:250px;"></select>');
            for (var i = 0; i < dataCollection.result.length; i++) {
                $('#search-select').append('<option value="' + dataCollection.result[i].StringNameId + '">' + dataCollection.result[i].Name + '</option>');
            }
        }
    }).fail(function (error, a, b) {
       
    }).always(function (xhr) {
        $('#wait-icon').remove();
    });
    //    success: function (Result) {
    //        if ($('#search-select').length > 0) {
    //            $('#search-select').remove();
    //        }
    //      
    //        var dataCollection = Result;
    //        if (dataCollection.result.length > 0) {
    //            $('#search-result').append('<select id="search-select" multiple="multiple" style="width:100%;height:'+selectHeight+'px;"></select>');
    //            for (var i = 0; i < dataCollection.result.length; i++) {
    //                $('#search-select').append('<option value="' + dataCollection.result[i].StringNameId + '">' + dataCollection.result[i].Name + '</option>');
    //            }
    //        }
    //    },
    //    error: function (ex, e, err) {
    //       
    //    }
    //});
}

function searchButton() {
    $('#btn_Search').click(function () {
        ajaxSearch();
        return false;
    });
}

function expandSearch(searchValue) {
    var url = $('#ExpandedSearchUrl').val();
    var searchType = $('#SearchType').val();
    $.ajax({
        type: 'POST',
        dataType: 'html',
        //contentType: 'application/json; charset=utf-8',
        url: url,
        data: '{ searchValue: "' + searchValue + '",searchType:"' + searchType + '" }',
        beforeSend: function (xhr) {
            $('.left-panel-inner').hide();
            $('#search-details-div').html("");
            $('#search-details-div').show();
            $('#search-details-div').append('<div id="wait-icon2"><img src="../../Content/Images/loading.gif" id="wait-image"/></div>');
            //xhr.overrideMimeType("text/plain; charset=x-user-defined");
        }
    }).done(function (Result) {
            if (Result != null) {
                $('#search-back').show();
                $('.left-panel-inner').hide();
                $('#search-details-div').show();
                $('#search-details-div').html(Result);

                $('#mandate-tab').tabs();
            }
    }).fail(function (error, a, b) {
       
    }).always(function (xhr) {
        $('#wait-icon').remove();
    });
}

function expandSearchClick() {
    $('body').on('change', '#search-select', function () {
        clearAllSelection();
        var searchValue = $(this).val();
        if ($('#left input:radio').eq(0).is(':checked') || $('#left input:radio').eq(1).is(':checked') || $('#left input:radio:last').is(':checked')) {            
            expandSearch(searchValue);
        }

        //----Map Search-----//
        var radioName;
        $('.left-sub').each(function () {
            if ($(this).find('input:radio').is(':checked')) {
                radioName = $(this).find('input:radio').val();
            }
        });
        switch (radioName) {
            case "Grensovergang":
                var segmentId = ConvertNoToGeoFormat(searchValue[0]);
                selectRoadSegement([segmentId]);
                break;
            case "WegenPerGebied":
                var segmentId = ConvertNoToGeoFormat(searchValue[0]);
                selectRoadSegement([segmentId]);
                break;
            case "Veerverbinding":
                var segmentId = ConvertNoToGeoFormat(searchValue[0]);
                selectRoadSegement([segmentId]);
                break;
            case "Gebied":
                //var segmentId = ConvertNoToGeoFormat(searchValue[0]);
                //selectRoadSegement([segmentId]);
                break;
            default:
                break;
        }
        //-------------------//

    });
}

function backToSearch() {
    $('#search-back').click(function () {
        $(this).hide();
        $('.left-panel-inner').show();
        $('#search-details-div').hide();
        $('#search-details-div').html("");
    });
}