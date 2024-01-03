$(document).ready(function () {
    collapseRightPane();
    expandRightPane();
    rightPaneExpandPos();

    rightPanePos();
});

var rightPane = 0;


var contentWidth = $('.content').outerWidth(true);
var rightPaneWidth = $('.right-bar').outerWidth(true);
function collapseRightPane() {
    $('.right-pane-collapse').click(function () {
        
        rightPane = rightPaneWidth;
        var contentNewWidth = parseInt(contentWidth) + parseInt(rightPaneWidth)-30;

        $('.right-bar').children().hide();
        $('.right-pane-expand').show();
        $('.right-bar').animate({
            width: '30px',
        }, 500);
        $('.right-pane-expand').animate({
            width: '28px',
        }, 500);
        $('.content').animate({
            width: contentNewWidth + 'px',            
        }, 500);
       
    });
}

function expandRightPane() {
    $('.right-pane-expand').live('click', function () {
        var contentWidth = $('.content').outerWidth(true);
        var contentNewWidth = parseInt(contentWidth) - parseInt(rightPane)+30;
        $('.right-bar').children().show('slow');
        $('.right-pane-expand').hide();
        $('.right-pane-expand').animate({
            width: '20%',
        }, 500);
        $('.right-bar').animate({
            width: rightPane,           
        }, 500);
        $('.content').animate({
            width: contentNewWidth + 'px',            
        }, 500);
    });
}

function rightPaneExpandPos() {
    var collapsePos = $('.right-bar').position().top;
    $('.right-pane-expand').attr('style', 'top:' + collapsePos + 'px');
}


function rightPanePos() {
    var rightPos = $('.right-bar').position().top + 1;
    var rightbarWidth = $('.right-bar').outerWidth(true) - 5;
    $('.right-panel').attr('style', 'top:' + rightPos + 'px;width:' + rightbarWidth + 'px');//width:' + leftbarWidth+'px;
    //$('.left-panel').attr('style', 'width:' + leftbarWidth + 'px');
}
