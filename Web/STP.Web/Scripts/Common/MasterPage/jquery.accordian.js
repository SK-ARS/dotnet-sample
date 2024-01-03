$(function () {
    $('.intelli_accordian').click(function () {
        $(this).find('span').toggleClass('intelli_accordian_show');
        $(this).parent().find('.accordian_matter').slideToggle();
    });
});





