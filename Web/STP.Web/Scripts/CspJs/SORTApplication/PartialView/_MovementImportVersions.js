    $('.tree li').each(function () {
        if ($(this).children('ol').length > 0) {
            $(this).addClass('parent');
        }
    });
    $('.tree li').each(function () {
        $(this).toggleClass('active');
        $(this).children('ol').slideToggle('fast');
    });
    $('.tree li.parent > a').click(function () {
        $(this).parent().toggleClass('active');
        $(this).parent().children('ol').slideToggle('fast');
    });

