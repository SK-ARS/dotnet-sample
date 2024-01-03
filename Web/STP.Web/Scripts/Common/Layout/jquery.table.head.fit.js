function fix_tableheader() {
    if ($('.tab_content').find('.tblheaderfix').length >= 1) { $('#tableheader').hide(); }
    else {
        var tableexist = $('section').find('.tblheaderfix').length;
        if (tableexist >= 1) {
            if ($(".tblheaderfix > tbody > tr").find('td').length > 1) {
                var tableOffset = $(".tblheaderfix").offset();
                tableOffset = tableOffset.top;
                var $header = $(".tblheaderfix > thead").clone();
                $("#header-fixed").html('');
                var $fixedHeader = $("#header-fixed").append($header);
                var count = $('.tblheaderfix > thead > tr > th').length;
                for (var i = 0; i < count; i++) {
                    $("#header-fixed > thead > tr").find('th').eq(i).css({ 'width': $(".tblheaderfix > tbody > tr").find('td').eq(i).width() })
                }
                var i = 0;
                $('.tblheaderfix').find('tr:eq(0)').find('th').each(function () {
                    var width = $(this).width();
                    $('#header-fixed').find('tr').find('th:eq(' + i + ')').width = width;
                    i++;
                });

                return 1;
            }
            else $('#tableheader').hide();
        }
        else $('#tableheader').hide();
    }
    
}

var old_scroll_top = 0;
$(document).scroll(function () {
    var current_scroll_top = $(document).scrollTop();
    var scroll_delta = current_scroll_top - old_scroll_top;
    if (current_scroll_top > 0) {
        var x = fix_tableheader();
        if (x == 1) {
            //$('#tableheader').slideDown(1000, "easeOutCirc");
            $('#tableheader').show();
            //$(".tblheaderfix > thead").hide();
        }

    }
    else {
        //$('#tableheader').slideUp(100, "easeOutCirc"); 
       // $('#tableheader').hide();
    }
});
$(document).ready(function () {
    var x = fix_tableheader();
    if (x == 1) $('#tableheader').show();
        
});