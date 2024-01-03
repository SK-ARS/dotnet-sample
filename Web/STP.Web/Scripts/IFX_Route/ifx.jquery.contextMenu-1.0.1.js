(function ($) {

    $.fn.extend({

        IntellizenzContext: function (option) {

            var defaults = {
                columnType: [{ colDetails: [{ type: 'label', name: '', src: '' }], seperator: false, func: function () { }, submenu: [{ colDetails: [{ type: 'label', name: '', src: '' }], seperator: false, func: function () { } }] }],
            }

            var thisdiv = this;

            var options = $.extend(defaults, option);

            var opt = options;

            var rowCount = opt.columnType.length;

            //for submenu
            var subRowCount = 0;
            var subColCount = 0;

            var colCount = 0;

            var returnPointX;
            var returnPointY;

            
            optn = opt;

            $(document).click(function () {
                //$('#map').click(function () {
                $('#intellizenz-ctxmenu').remove();
               
            });


            $(this).addClass('context-wrap');

            //$(document).on('mousemove', 'body', function (e) {
            $(document).on('mousemove', 'body', function (e) {
                var className = $(e.target).attr('class');
                if (typeof className === 'undefined') {
                    if (className != 'context-wrap') {
                        // ... (Do some cool stuff here) ...
                        $('#intellizenz-ctxmenu').remove();
                    }
                }
            });

           


            $('#map').on('contextmenu', this, function (e) {
                //;
                $('#intellizenz-ctxmenu').remove();

                returnPointX = e.clientX + document.body.scrollLeft + document.documentElement.scrollLeft;
                returnPointY = e.clientY + document.body.scrollTop + document.documentElement.scrollTop;

                $('#ghost-cover').remove();
                e.preventDefault();
                if (rowCount > 0) {
                    $('body').append('<div id="intellizenz-ctxmenu" class="context-wrap"><table id="ctxtable"></table></div>' );
                    colCount = opt.columnType[0].colDetails.length;
                }

                for (var i = 0; i < rowCount; i++) {
                    $('#ctxtable').append('<tr class="ctxtd cont-menu-click-ars-link-data" data-returnpointx="' + returnPointX + ' "data-returnpointy="' + returnPointY + '"data-index="' + i +'" ></tr>');
                    for (var j = 0; j < colCount; j++) {
                        if (typeof opt.columnType[i].seperator !== 'undefined') {
                            if (opt.columnType[i].seperator) {
                                //$('#ctxtable tr').eq(i).attr('style', 'border-bottom:1px solid');
                            }
                        }

                        var columnType = opt.columnType[i].colDetails[j].type;
                        var columnName = opt.columnType[i].colDetails[j].name;
                        var columnSrc = '';
                        if (typeof opt.columnType[i].colDetails[j].src !== 'undefined') {
                            columnSrc = opt.columnType[i].colDetails[j].src;
                        }
                        if (columnType == 'label') {
                            $('#ctxtable tr').eq(i).append('<td><span class="ctxtext">' + columnName + '</span></td>');
                        }
                        else if (columnType == 'image') {
                            $('#ctxtable tr').eq(i).append('<td><span><img src="' + columnSrc + '"/></span></td>');
                        }

                    }

                    
                    if (opt.columnType[i].submenu != null) {
                        subRowCount = opt.columnType[i].submenu.length;
                    }

                    if (subRowCount > 0) {
                        $('#ctxtable tr').eq(i).append('<table class="iz_submenu"></table>');
                        subColCount = opt.columnType[0].submenu[0].colDetails.length;
                    }

                    for (var si = 0; si < subRowCount; si++) {
                        $('#ctxtable tr').eq(i).find('.iz_submenu').append('<tr></tr>');
                        for (var sj = 0; sj < subColCount; sj++) {
                            var columnName1 = 'hello';
                            $('#ctxtable tr').eq(i).find('.iz_submenu').eq(si).append('<td><span>' + columnName1 + '</span></td>');
                        }
                    }


                    $('#intellizenz-ctxmenu table,tr,td,input,select,img,span').addClass('context-wrap');
                }
                var result = showContextMenu();
                if (result) {
                    var menuWidth = $('#intellizenz-ctxmenu').width();
                    var menuHeight = $('#intellizenz-ctxmenu').height();



                    var parentWidth = $('#map').width();
                    var parentHeight = $('#map').height();
                    var remainWidth = parentWidth - e.pageX;
                    var remainHeight = parentHeight - e.pageY;
                    if (remainWidth <= menuWidth) {
                        $('#intellizenz-ctxmenu').css('left', e.pageX - menuWidth);
                    }
                    else {
                        $('#intellizenz-ctxmenu').css('left', e.pageX);
                    }
                    if (remainHeight <= menuHeight) {
                        $('#intellizenz-ctxmenu').css('top', e.pageY - menuHeight);
                    }
                    else {
                        $('#intellizenz-ctxmenu').css('top', e.pageY);
                    }

                    //code for ESDAL 4
                     var x = returnPointX;
                    var y = returnPointY;
                    $('#intellizenz-ctxmenu').css('left', x);

                    if (remainHeight >= menuHeight) {
                        $('#intellizenz-ctxmenu').css('top', y);
                    }
                    else {
                        $('#intellizenz-ctxmenu').css('top', y - menuHeight);
                    }
                      
                }
                else if (result==false) {
                    $('#intellizenz-ctxmenu').remove();
                }
                


            });

        }

    });
})(jQuery);
$('body').on('click', '.cont-menu-click-ars-link-data', function (e) {
    var index = $(this).data('index');
    var returnPointX = $(this).data('returnpointx');
    var returnPointY = $(this).data('returnpointy');
    ClickIntellizenzLinkData(index, returnPointX, returnPointY);
});

var optn;
function ClickIntellizenzLinkData(i, x, y) {
    optn.columnType[i].func({ xcord: x, ycord: y });
}



