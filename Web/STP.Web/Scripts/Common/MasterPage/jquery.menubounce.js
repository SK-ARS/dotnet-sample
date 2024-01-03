$(document).ready(function () {
    $("#menu > li > ul").hide();
    $("#menu > li").hover(
        function () {
            $(this).children("ul").slideDown('slow', 'easeIn');
        },
        function () {
            $(this).children("ul").stop(true, true).hide();
        }
    );

    
});