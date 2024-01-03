$(document).ready(function () {
    
        radioButtonOnload();
        leftPaneCollapse();
        leftPaneExpand();

        leftPaneExpandPos();
        leftPanePos();


    //leftPaneResize();        


});

function radioButtonOnload() {
    $('.left-sub').each(function () {
        $(this).find('select,input:not(:radio)').attr('disabled', 'disabled');
        $(this).find('input:radio').each(function () {
            if ($(this).is(':checked')) {
                $(this).parent().parent().parent().find('.left-input').show();
            }
        });

        $(this).find('input:radio').click(function () {
            if ($(this).is(':checked')) {
                if (!($(this).parent().parent().parent().find('.left-input').is(':visible'))) {
                    $('.left-input').hide();
                    $('.left-sub').find('select,input:not(:radio)').attr('disabled', 'disabled');
                    $('.left-input input:text').val('');
                    $(this).parent().parent().parent().find('select,input:not(:radio)').attr('disabled', false);
                    $(this).parent().parent().parent().find('.left-input').show();
                }
            }
        });
    });
}

var leftPane = 0;
var initialContentWidth = 0;

var contentWidth = $('.content').outerWidth(true);
var leftPaneWidth = $('.left-bar').outerWidth(true);

function leftPaneCollapse() {
    $('.left-pane-collapse').click(function () {
                        
        leftPane = leftPaneWidth;
        initialContentWidth = contentWidth;

        var contentNewWidth = parseInt(contentWidth) + parseInt(leftPaneWidth);
        $('.left-bar').children().hide();
        $('.left-pane-expand').show();        
        $('.left-bar').animate({
            width: '30px',
        }, 500);
        $('.left-pane-expand').animate({
            width: '28px',
        }, 500);
        $('.content').animate({
            width: contentNewWidth + 'px',            
        },500);
    });    
}

function leftPaneExpand() {
    $('.left-pane-expand').live('click', function () {
        var contentWidth = $('.content').outerWidth(true);
        var contentNewWidth = parseInt(contentWidth) - parseInt(leftPane)+30;
        $('.left-bar').children().show('slow');
        $('.left-pane-expand').hide();
        $('.left-pane-expand').animate({
            width: '20%',
        }, 500);
        $('.left-bar').animate({
            width: leftPane,
        }, 500);
        $('.content').animate({
            width: contentNewWidth + 'px',
        }, 500);
    });
}

function leftPaneExpandPos() {
    var collapsePos = $('.left-bar').position().top+1;
    $('.left-pane-expand').attr('style', 'top:' + collapsePos + 'px');
}

function leftPanePos() {
    var leftPos = $('.left-bar').position().top + 1;
    var leftbarWidth = $('.left-bar').outerWidth(true) - 5;
    var leftbarHeight = $('.left-bar').outerHeight(true);
    $('.left-panel').attr('style', 'top:' + leftPos + 'px;width:' + leftbarWidth + 'px;height:'+leftbarHeight+'px;overflow-y:auto;');//width:' + leftbarWidth+'px;
    //$('.left-panel').attr('style', 'width:' + leftbarWidth + 'px');
}

function leftPaneResize() {
    var ismouseclicked = false;
    var isdraggable = false;
    var mouseInitialPos = 0;

    $('html').mousedown(function (e) {
        if (e.which == 1) {
            ismouseclicked = true;
            mouseInitialPos = e.pageX;
        }
    }).mouseup(function (e) {
        ismouseclicked = false;
        isdraggable = false;
       
    });

    $('.left-bar').mousemove(function (e) {
        var leftbarRightPos = $('.left-bar').outerWidth(true);
        var contentWidth = $('.content').outerWidth(true);
        var mousePos = e.pageX;
        if ((leftbarRightPos + 3 >= mousePos) && (leftbarRightPos - 3 <= mousePos)) {
            $(this).attr('style', 'cursor:w-resize');
            isdraggable = true;
        }
        else {
            $('.left-bar').attr('style', 'cursor:default');
        }
        if (ismouseclicked && isdraggable) {
            var newPos = parseInt(mouseInitialPos) - parseInt(mousePos);
            var newWidth = parseInt(leftbarRightPos) - parseInt(newPos);
            var newContentWidth = parseInt(contentWidth) + parseInt(newPos);
            $('.left-bar').attr('style', 'width:' + mousePos + 'px');
            $('.content').attr('style', 'width:' + newContentWidth + 'px');
          
            //$('.headerimg span').text(newWidth);
            //$('.left-bar').animate({
            //    width: mousePos,
            //},0.5);
            //$('.content').animate({
            //    width: newContentWidth,
            //}, 0.5);

        }
    });
}

