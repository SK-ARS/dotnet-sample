    var feedId;
    var feedName;

    $(document).ready(function () {
        SelectMenu(8);
        ChangeSearchCriteria();
    });

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
