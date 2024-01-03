$(function () {
    var basePath = '/Scripts/Common/Openlayers/';
    var cssPath = basePath + 'theme/default/style.css';
    var imgpath = basePath + 'img/';


    // load stylesheet
    $('<link>')
        .appendTo('head')
        .attr({ type: 'text/css', rel: 'stylesheet' })
        .attr('href', cssPath);

    // set image path
    OpenLayers.ImgPath = imgpath;
});