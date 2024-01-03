
    $(document).ready(function () {

        radioButtonOnload();

        $('.minimise-left').live('click',function () {
            $('#left-panel-wrap').hide();
            $('.route-class').hide();
            $('.left-main').hide();
            $('.maximise-left').show();
            $('.minimise-left').hide();
            $('#left').animate({
                right: '98%',
            });
            $('#left-bar').animate({
                left: '2%',
                right: '2.5%',
            });
            $('#center').animate({
                left: '2.5%',
            });
            //$('#OpenLayers_Map_4_OpenLayers_Container').animate({
            //    left: '0px',
            //    right: '0px',
            //    top: '0px',
            //    bottom:'0px',
            //});
        });

        $('.maximise-left').live('click', function () {
            $('#left-panel-wrap').show();
            $('.route-class').show();
            $('.left-main').show();
            $('.maximise-left').hide();
            $('.minimise-left').show();
            $('#left').animate({
                right: '81%',
            });
            $('#left-bar').animate({
                left: '19%',
                right: '80.5%',
            });
            $('#center').animate({
                left: '19.5%',
            });
        });

        $('.minimise-right').live('click', function () {
            $('#right-panel-wrap').hide();
            $('.route-class').hide();
            //$('.left-main').hide();
            $('.maximise-right').show();
            $('.minimise-right').hide();
            $('#right').animate({
                left: '98%',
            });
            $('#right-bar').animate({
                left: '97.5%',
                right: '2%',
            });
            $('#center').animate({
                right: '2.5%',
            });
        });

        $('.maximise-right').live('click', function () {
            $('#right-panel-wrap').show();
            $('.route-class').show();
            //$('.left-main').show();
            $('.maximise-right').hide();
            $('.minimise-right').show();
            $('#right').animate({
                left: '80.5%',
            });
            $('#right-bar').animate({
                left: '80%',
                right: '19.5%',
            });
            $('#center').animate({
                right: '20%',
            });
        });
    });

function radioButtonOnload() {
    $('#left-panel-searchData').each(function () {
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

    $(document).ready(function () {
        var isdraggable = false;
        var leftMove = false;
        var rightMove = false;
        var toPercent;
        var percent;
        var ghostbar;

        $('#left-bar').mousedown(function (e) {
            if (e.which == 1) {
                isdraggable = true;
                leftMove = true;
                rightMove = false;

                var main = $('#center');
                ghostbar = $('<div>',
                                 {
                                     id: 'ghostbar',
                                     css: {
                                         height: main.outerHeight(),
                                         width: '0.5%',
                                         top: main.offset().top,
                                         left: main.offset().left,
                                     }
                                 }).appendTo('body');
                $('#ghost-div').show();

            }

            $('body').mousemove(function (e) {
                toPercent = (e.pageX / $('#body').width()) * 100;
                percent = toPercent.toFixed(1);
                ghostbar.css("left", e.pageX + 2);
                //$('#test').text(percent);

                //if (percent <= 10) {
                //    $('#left').css('right', '90%');
                //    $('#left-bar').css({ 'left': '90%', right: '89.5%' });
                //    $('#center').css('left', '10.5%');
                //    $('#ghostbar').remove();
                //    isdraggable = false;
                //}

            });

        });

        $('#right-bar').mousedown(function (e) {
            if (e.which == 1) {
                isdraggable = true;
                leftMove = false;
                rightMove = true;

                var main = $('#center');
                ghostbar = $('<div>',
                                 {
                                     id: 'ghostbar',
                                     css: {
                                         height: main.outerHeight(),
                                         width: '0.5%',
                                         top: main.offset().top,
                                         left: main.offset().left,
                                     }
                                 }).appendTo('body');
                $('#ghost-div').show();
            }

            $('body').mousemove(function (e) {
                toPercent = (e.pageX / $('#body').width()) * 100;
                percent = toPercent.toFixed(1);
                ghostbar.css("left", e.pageX + 2);

                if (percent <= 80) {                    
                    $('#ghostbar').remove();
                    isdraggable = false;
                }

            });

        });

        $('body, .olMapViewport ').mouseup(function () {
            if (isdraggable && leftMove) {
                var percentRight = 100 - percent;
                var contentWidth = parseFloat(percent) + 0.5;
                $('#left').css('right', percentRight + '%');
                $('#left-bar').css({ 'left': percent + '%', right: (percentRight - 0.5) + '%' });
                $('#center').css('left', contentWidth + '%');
                $('#ghostbar').remove();
                $('#ghost-div').hide();

                isdraggable = false;
            }
            else if (isdraggable && rightMove) {
                //var percentRight = 100 - percent;
                var rightWidth = 100 - percent;
                //var percentLeft = parseFloat(rightWidth) + 0.5;
                var centerRight = parseFloat(rightWidth) + 0.5;
                var leftbarL = parseFloat(percent) - 0.5;

                $('#right').css('left', percent + '%');
                $('#right-bar').css({ 'left': leftbarL + '%', right: rightWidth + '%' });
                $('#center').css('right', centerRight + '%');
                $('#ghostbar').remove();
                $('#ghost-div').hide();
                isdraggable = false;
            }
        });

    });

function createGhost() {
    var main = $('#center');
    var ghostbar = $('<div>',
                     {
                         id: 'ghostbar',
                         css: {
                             height: main.outerHeight(),
                             width: '0.5%',
                             top: main.offset().top,
                             left: main.offset().left,
                         }
                     }).appendTo('body');
}
