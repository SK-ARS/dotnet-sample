$(document).ready(function () {
    $('body').on('click', '.ViewContact', function (e) {
        ViewAnnotationContactDetail(e);
    });
    $('body').on('click', '#CloseAnnotationPopUp', function (e) {
        e.preventDefault();
        CloseAnnotationLibraryPopup();
    });
    $('body').on('click', '#searchAnnotationTextPopUp', function (e) {
        searchAnnotationTextPopUp();
    });
    $('body').on('click', '#clearAnnotationTextFilter,#filterimageAnnPopup', function (e) {
        clearAnnotationTextFilter();
    });
    $('body').on('click', '.closebtn', function (e) {
        closeFiltersAnnotationPopup();
    });
    $('body').on('click', '#openAnnotationFilterPopup', function (e) {
        openAnnotationFilterPopup();
    });
    $('body').on('click', '.ImportAnnotationText', function (e) {
        var annotationText = $(this).attr("AnnotationText");
        var annotType = $(this).data('annottype');
        var annotTypeVal = annotType - 250000;
        ImportAnnotationText(annotationText, annotTypeVal);
    });
    $('body').on('click', '#annotationTextLibraryPopupPaginator a', function (e) {
        e.preventDefault();
        if (this.href == undefined || this.href=="")
            return;
        var pageNumber = getUrlParameterByName("pageNumber", this.href);
        var structureId = getUrlParameterByName("structureId", this.href);
        searchAnnotationTextPopUp(structureId,pageNumber,isPagination=true);
    });
});

function openAnnotationFilterPopup() {
    //$('#popupFilter').html($('#filterAnnotationTextPopup').html());
    $('#popupFilter').find("#filters").css("width", "350px");
    $("#annotationTextmodal").css('display', 'block');
}
function CloseAnnotationLibraryPopup() {
    $("#annotationTextmodal").css('display', 'none');
    $("#annotationtextpopup").css('display', 'none');
    $("#annottaiondiv").css('display', 'block');
}
function closeFiltersAnnotationPopup() {
    $("#filterAnnotationTextPopup").find("#filters").css("width", 0);
    $('#popupFilter').find("#filters").css("width", 0);
    var div = document.getElementById("vehicles");
    if (div != null) {
        document.getElementById("banner-container").style.filter = "unset"
        document.getElementById("banner-container").style.background = "linear-gradient(0deg, rgba(255, 255, 255, 1) 0%, rgba(201, 205, 229, 0.44) 100%)";
    }
    else {
        document.getElementById("banner").style.filter = "unset"
        document.getElementById("navbar").style.filter = "unset";
    }
}
function ImportAnnotationText(annotationtext, annotTypeVal) {
    CloseAnnotationLibraryPopup();
    document.getElementById("annotText").value = annotationtext;
    document.getElementById("annotType").value = annotTypeVal;
}
function searchAnnotationTextPopUp(structureId = 0, pageNumber = 1, isPagination=false) {
    var options = { "backdrop": "static", keyboard: true };
    var parent = $('#popupFilter').find("#annotTypeFilter").length>0 ? $('#popupFilter') : $('#filterDivAddressBook');
    var annotationText = parent.find("#AnnotationTextSearchValue").val();
    var annotationType = parent.find("#annotTypeFilter").val()||0;
    var userId = parent.find("#userIdFilter").val() || 0;
    pageNumber = isPagination ? pageNumber : 1;
    $.ajax({
        type: "GET",
        url: '/Annotation/GetAnnotationsFromLibrary',
        contentType: "application/json; charset=utf-8",
        data: { "pageNumber": pageNumber, "pageSize": 10, "annotationType": annotationType, "annotationText": annotationText, "structureId": structureId, "userId": userId },
        datatype: "json",
        beforeSend: function () {
            openContentLoader('#annotationtextpopup .modal-content');
        },
        success: function (data) {
            if ($('#annotationTextmodal').is(':visible')) {
                //========
                $('#annotationtextpopup').html(data);
            } else {
                $("#annotationtextpopup").css('display', 'block');
                $('#annotationtextpopup').html(data);
                $('#annotationtextpopup').modal(options);
                $("#annottaiondiv").css('display', 'none');
                $("#annotationTextmodal").css('display', 'block');
            }
            //$('#popupFilter').find("#AnnotationTextSearchValue").val(annotationText);
            //$('#popupFilter').find("#annotTypeFilter").val(annotTypeFilter);
            //$('#popupFilter').find("#userIdFilter").val(userIdFilter);

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.error("Dynamic content load failed." + errorThrown);
        },
        complete: function () {
            closeContentLoader('#annotationtextpopup .modal-content');
        }
    });
    closeFiltersAnnotationPopup();
}
function clearAnnotationTextFilter() {
    $('#popupFilter').find("#AnnotationTextSearchValue").val("");
    $('#popupFilter').find("#annotTypeFilter").val("0");
    $('#popupFilter').find("#userIdFilter").val("0");
    searchAnnotationTextPopUp();
    closeFiltersAnnotationPopup();
}
function ViewAnnotationContactDetail(e) {
    var annotationTextId = e.currentTarget.dataset.AnnotationTextId;
    ViewContactDetail(annotationTextId);
}
function ImportAnnotationTextFn(e) {
    var annotationText = e.currentTarget.dataset.AnnotationText;
    ImportAnnotationText(annotationText);
}