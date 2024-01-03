(function ($) {

    $.fn.extend({

        IntellizenzContext: function (option) {

            var defaults = {
                columnType: [{ colDetails: [{ type: 'label', name: '', src: '' }], seperator: false, func: function () { } }],
            }

            var thisdiv = this;

            var options = $.extend(defaults, option);

            var opt = options;

            var rowCount = opt.columnType.length;
            var colCount = 0;

            var returnPointX;
            var returnPointY;
            
            //console.log($.fn.IntellizenzContext.func);
            optn = opt;
            
            $(document).click(function () {
                //$('#map').click(function () {
                $('#intellizenz-ctxmenu').remove();
                mapcontextMenuOn = false;
                currentMouseOverFeature = null;

                //$(document).off('click', '#ctxtable tr');
                //$('#ghost-cover').remove();
            });


            $(this).addClass('context-wrap');

            //$(document).on('mousemove', 'body', function (e) {
            $(document).on('mousemove', 'body', function (e) {
                var className = $(e.target).attr('class');
                //console.log($(e.target).prop('class'));
                if (typeof className === 'undefined') {
                    if (className != 'context-wrap') {
                        // ... (Do some cool stuff here) ...
                        $('#intellizenz-ctxmenu').remove();
                        //$(window).unbind('mousemove');
                    }
                }
            });

            //$('#ghost-cover').live('contextmenu', function (e) {
            //    $('#intellizenz-ctxmenu').remove();
            //    $('#ghost-cover').remove();
            //});


            //$(document).on('contextmenu', this, function (e) {
            $('#divMap').on('contextmenu', this, function (e) {
              
              
                $('#intellizenz-ctxmenu').remove();

                returnPointX = e.clientX + document.body.scrollLeft + document.documentElement.scrollLeft;
                returnPointY = e.clientY + document.body.scrollTop + document.documentElement.scrollTop;
               
                $('#ghost-cover').remove();
                e.preventDefault();
                if (rowCount > 0) {
                    //$('body').append('<div id="ghost-cover"></div>');
                    //$('#divMap').find($('body #map')).append('<div id="intellizenz-ctxmenu" class="context-wrap"><table id="ctxtable"></table></div>');
                    $('body').append('<div id="intellizenz-ctxmenu" class="context-wrap"><table id="ctxtable"></table></div>');
                    colCount = opt.columnType[0].colDetails.length;
                }

                for (var i = 0; i < rowCount; i++) {
                    $('#ctxtable').append('<tr class="click-ars-link-data" data-returnpointx="' + returnPointX + ' "data-returnpointy="' + returnPointY + '"data-index="' + i +'"></tr>');
                    for (var j = 0; j < colCount; j++) {
                        if (typeof opt.columnType[i].seperator !== 'undefined') {
                            if (opt.columnType[i].seperator) {
                                $('#ctxtable tr').eq(i).attr('style', 'border-bottom:1px solid');
                            }
                        }

                        var columnType = opt.columnType[i].colDetails[j].type;
                        var columnName = opt.columnType[i].colDetails[j].name;
                        var columnSrc = '';
                        if (typeof opt.columnType[i].colDetails[j].src !== 'undefined') {
                            columnSrc = opt.columnType[i].colDetails[j].src;
                        }
                        if (columnType == 'label') {
                            $('#ctxtable tr').eq(i).append('<td><span>' + columnName + '</span></td>');
                        }
                        else if (columnType == 'image') {
                            $('#ctxtable tr').eq(i).append('<td><span><img src="' + columnSrc + '"/></span></td>');
                        }
                    }

                    $('#intellizenz-ctxmenu table,tr,td,input,select,img,span').addClass('context-wrap');
                }
                showContextMenu();
                var menuWidth = $('#intellizenz-ctxmenu').width();
                var menuHeight = $('#intellizenz-ctxmenu').height();
                
                //var parentWidth = $(this).width();
                //var parentHeight = $(this).height();

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

                //$('#intellizenz-ctxmenu').children().addClass('context-wrap');
                //$('#intellizenz-ctxmenu').attr('style', 'top:'+e.pageY+'px;left:'+e.pageX+'px;');

                
                //disable_contextmenu(3);

                
            });


            //click event
            //$(document).on('click', '#ctxtable tr', function () {
            ////$('#map').on('click', '#ctxtable tr', function () {
            //    var col = $(this).parent().children().index($(this));
            //    if (typeof opt.columnType[col].func === 'function') {
            //        opt.columnType[col].func({ xcord: returnPointX, ycord: returnPointY });
            //        //return returnPointX;
            //    }
            //});            

        }
    });    
})(jQuery);

$('body').on('click', '.click-ars-link-data', function (e) {
    var index = $(this).data('index');
    var returnPointX = $(this).data('returnpointx');
    var returnPointY = $(this).data('returnpointy');
    ClickIntellizenzLinkData(index, returnPointX, returnPointY);
});

var optn;
function ClickIntellizenzLinkData(i, x, y) {
    //console.log(optn);
    optn.columnType[i].func({ xcord: x, ycord: y });
}



