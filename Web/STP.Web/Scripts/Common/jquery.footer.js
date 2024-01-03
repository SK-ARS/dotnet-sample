$(document).ready(function () {    
    var bottom = $('.page').outerHeight(true) + $('.content').outerHeight(true);    
    $('.footer').attr('style', 'top:' + bottom + 'px');
});