$(document).ready(function () {
    var valueOnStart = $('#dropdownRoadsByArea').val();
    getRoadByArea(valueOnStart);
    $('#dropdownRoadsByArea').change(function () {        
        var searchId = $(this).val();
        getRoadByArea(searchId);
    });
});

function getRoadByArea(searchId) {
    var url = $('#GetRoads').val();
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: url,
        data: {searchValue: searchId},
        beforeSend: function (xhr) {
            //$('#search-select').append('<div>Hi</div>');
            //xhr.overrideMimeType("text/plain; charset=x-user-defined");
        }
    }).done(function (Result) {
        $('#roadselect option').remove();
        var dataCollection = Result;
        if (dataCollection.result.length > 0) {
            for (var i = 0; i < dataCollection.result.length; i++) {
                //$('#roadselect').append('<option value="' + dataCollection.result[i].StringNameId + '">' + dataCollection.result[i].Name + '</option>');
                //Commented to include Values as Name as StoreProc input is name
                $('#roadselect').append('<option value="' + dataCollection.result[i].Name + '">' + dataCollection.result[i].Name + '</option>');
            }
        }
    }).fail(function (error, a, b) {
        
    }).always(function (xhr) {
       
    });
    
}