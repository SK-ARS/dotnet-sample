let hf_AssoFile1 = $('#hf_AssoFile1').val();
let hf_MswordFile = $('#hf_MswordFile').val();
let hf_AssoFile2 = $('#hf_AssoFile2').val();
let hf_PdfFile = $('#hf_PdfFile').val();
let hf_AssoFile3 = $('#hf_AssoFile3').val();
let hf_VideoFile = $('#hf_VideoFile').val();
let hf_AssoFile4 = $('#hf_AssoFile4').val();
let hf_AssoFile5 = $('#hf_AssoFile5').val();
let hf_PageName = $('#hf_PageName').val();
let hf_DateTimeToday = $('#hf_DateTimeToday').val();

function NewsDetailsInit() {
    hf_AssoFile1 = $('#hf_AssoFile1').val();
    hf_MswordFile = $('#hf_MswordFile').val();
    hf_AssoFile2 = $('#hf_AssoFile2').val();
    hf_PdfFile = $('#hf_PdfFile').val();
    hf_AssoFile3 = $('#hf_AssoFile3').val();
    hf_VideoFile = $('#hf_VideoFile').val();
    hf_AssoFile4 = $('#hf_AssoFile4').val();
    hf_AssoFile5 = $('#hf_AssoFile5').val();
    hf_PageName = $('#hf_PageName').val();
    hf_DateTimeToday = $('#hf_DateTimeToday').val();
    if ($("#ContentId").val() == 0 && hf_PageName == 'news story') {
        $("#Priority").prop('selectedIndex', 1);
    }
    validation();
    if ($('#hf_mode').val() != 'Edit')
        $('#EndDate').val(hf_DateTimeToday);

    $('#EndDate').datepicker({
        dateFormat: "dd-M-yy",
        changeMonth: true,
        changeYear: true,
        inline: true,
        minDate: 0,
        beforeShow: function (a, b) {
            let cnt = 0;
            var interval = setInterval(function () {
                cnt++;
                if (b.dpDiv.is(":visible")) {
                    let parent = b.input;
                    b.dpDiv.position({ my: "left top", at: "left bottom", of: parent });
                    clearInterval(interval);
                } else if (cnt > 50) {
                    clearInterval(interval);
                }
            }, 10);
        }
    });
}

$(document).ready(function () {
    NewsDetailsInit();
    $('body').on('change', '#DownloadType', function (e) { e.preventDefault(); $("#DownTypeValidation").hide(); });
    $('body').on('click', '#btnback', function (e) { e.preventDefault(); RedirectToMainPage(); });
    //$('body').on('click', '.hideerr', function (e) { e.preventDefault(); hideError(); });
    //$('body').on('click', '.hideerr5', function (e) { e.preventDefault(); hideError(); });
});


$('body').on('click', '#btnRemoveAttachment1', function (e) {

    e.preventDefault();
    let type = $(this).data('type');
    RemoveAttachment(type);
    return false;
});
$('body').on('click', '#btnRemoveAttachment3', function (e) {
    e.preventDefault();
    let type = $(this).data('type');
    RemoveAttachment(type);
    return false;
});
$('body').on('click', '#btnRemoveAttachment4', function (e) {
    e.preventDefault();
    let type = $(this).data('type');
    RemoveAttachment(type);
    return false;
});
$('body').on('click', '#btnRemoveAttachment5', function (e) {
    e.preventDefault();
    let type = $(this).data('type');
    RemoveAttachment(type);
    return false;
});
$('body').on('click', '#btnRemovePDFAttachment', function (e) {
    e.preventDefault();
    let type = $(this).data('type');
    RemoveAttachment(type);
    return false;
});
$('body').on('click', '#btnRemoveAttachment2', function (e) {
    e.preventDefault();
    let type = $(this).data('type');
    RemoveAttachment(type);
    return false;
});
$('body').on('click', '#btnRemoveVideoFileAttachment', function (e) {
    e.preventDefault();
    let type = $(this).data('type');
    RemoveAttachment(type);
    return false;
});
$('body').on('click', '#btnRemoveMSWordAttachment', function (e) {
    e.preventDefault();
    let type = $(this).data('type');
    RemoveAttachment(type);
    return false;
});
function RemoveAttachment(fileType) {
    let associatedFile = "";

    let currentContentID = $("#ContentId").val();

    if (fileType == "htmlfile") {
        associatedFile = hf_AssoFile1;
    }
    else if (fileType == "docfile") {
        associatedFile = hf_MswordFile;
    }

    else if (fileType == "thumbnail") {
        associatedFile = hf_AssoFile2;
    }
    else if (fileType == "pdffile") {
        associatedFile = hf_PdfFile;
    }

    else if (fileType == "file3") {
        associatedFile = hf_AssoFile3;
    }
    else if (fileType == "video") {
        associatedFile = hf_VideoFile;
    }

    else if (fileType == "file4") {
        associatedFile = hf_AssoFile4;
    }
    else if (fileType == "file5") {
        associatedFile = hf_AssoFile5;
    }

    $.ajax({
        async: false,
        type: "POST",
        url: '../Information/RemoveAttachment',
        dataType: "json",
        data: JSON.stringify({ fileType: fileType, fileName: associatedFile }),
        processdata: true,
        success: function (result) {
            if (result.Success) {

                if (fileType == "htmlfile") {
                    $("#AssociatedHtmlDiv").hide();
                }
                else if (fileType == "docfile") {
                    $("#AssociatedWordDiv").hide();
                }

                else if (fileType == "thumbnail") {
                    $("#AssociatedImageDiv").hide();
                }
                else if (fileType == "pdffile") {
                    $("#AssociatedPDFDiv").hide();
                }

                else if (fileType == "file3") {
                    $("#AssociatedFile3Div").hide();
                }
                else if (fileType == "video") {
                    $("#AssociatedVideoDiv").hide();
                }

                else if (fileType == "file4") {
                    $("#AssociatedFile4Div").hide();
                }
                else if (fileType == "file5") {
                    $("#AssociatedFile5Div").hide();
                }
            }
        },
        error: function () {
            location.reload();
        },
        complete: function () {
            stopAnimation();
        }
    });
}

function validation() {

    $("#AssoFileHTMLValidation").removeClass("invalid");
    $("#AssoFileHTMLSizeValid").removeClass("invalid");
    $("#AssoFileImg3ValidationCommon").removeClass("invalid");
    $("#AssoFileImg3SizeValidCommon").removeClass("invalid");
    $("#AssoFileThumbImgValidation").removeClass("invalid");
    $("#AssoFileThumbImgSizeValid").removeClass("invalid");
    $("#MswordFileValidation").removeClass("invalid");
    $("#MswordFileSizeValid").removeClass("invalid");
    $("#PDFFileValidation").removeClass("invalid");
    $("#PDFFileSizeValid").removeClass("invalid");
    $("#VideoValidation").removeClass("invalid");
    $("#VideoSizeValid").removeClass("invalid");
    $("#DownTypeValidation").removeClass("invalid");

    $("#AssoFileImg1Validation").hide();
    $("#AssoFileImg2Validation").hide();
    $("#AssoFileImg3Validation").hide();
    $("#PortalError").hide();
    $("#DownTypeValidation").hide();
    $("#Documentselect").hide();

    $("#PortalError").hide();

    $("#Title").keyup(function () {

        let PageNameValid = hf_PageName;
        if (PageNameValid == "link" || PageNameValid == "download") {
            if ($('#Title').val() == '') {
                validationForTitle.innerHTML = "Display name is required";
                validationForTitle.style.display = 'block';
            }
            else if ($('#Title').val().length < 10) {
                validationForTitle.innerHTML = "Display name should contain at least 10 to 100 characters";
                validationForTitle.style.display = 'block';
            }
            else { validationForTitle.style.display = 'none' }
        }
    });
    $("#Link_url").keyup(function () {

        let PageNameValid = hf_PageName;
        if (PageNameValid == "link") {
            if ($('#Link_url').val() == '') {
                validationForURL.innerHTML = "URL is required";
                validationForURL.style.display = 'block';
            }
            else { validationForURL.style.display = 'none' }
        }
    });

    $("form").submit(function (e) {
        //Reset validation message
       
        $("#AssoFileHTMLValidation").removeClass("invalid");
        $("#AssoFileHTMLSizeValid").removeClass("invalid");
        $("#AssoFileImg3ValidationCommon").removeClass("invalid");
        $("#AssoFileImg3SizeValidCommon").removeClass("invalid");
        $("#AssoFileThumbImgValidation").removeClass("invalid");
        $("#AssoFileThumbImgSizeValid").removeClass("invalid");
        $("#MswordFileValidation").removeClass("invalid");
        $("#MswordFileSizeValid").removeClass("invalid");
        $("#PDFFileValidation").removeClass("invalid");
        $("#PDFFileSizeValid").removeClass("invalid");
        $("#VideoValidation").removeClass("invalid");
        $("#VideoSizeValid").removeClass("invalid");
        $("#DownTypeValidation").removeClass("invalid");

        $("#AssoFileImg1Validation").hide();
        $("#AssoFileImg2Validation").hide();
        $("#AssoFileImg3Validation").hide();
        $("#PortalError").hide();
        $("#DownTypeValidation").hide();
        $("#Documentselect").hide();

        let PageNameValid = hf_PageName;
        let Smallfilesize = 52428800; //check  size 52428800 bytes = 50 mb
        let flgReturn = true;

        //Hot news check
        if (PageNameValid == 'news story') {
            let CONTENT_ID = $("#ContentId").val();
            let HOT_NEWS_CONTENT_ID = $("#HotNewsContentId").val();
            let HOT_NEWS_NAME = $("#HotNewsName").val();
            if ($("#Priority :selected").text().toLowerCase() == 'hot') {
                //$('#newNewsIcon').show();
                if (HOT_NEWS_CONTENT_ID != 0) {
                    if (CONTENT_ID == 0) {
                        flgReturn = false;
                        ShoHotNewsPopup(HOT_NEWS_NAME);

                    }
                    else if (CONTENT_ID != HOT_NEWS_CONTENT_ID) {
                        flgReturn = false;
                        ShoHotNewsPopup(HOT_NEWS_NAME);
                    }
                }
            }

        }
        if (PageNameValid == "link" || PageNameValid == "download") {
            if ($('#Title').val() == '') {
                validationForTitle.innerHTML = "Display name is required";
                validationForTitle.style.display = 'block';
                flgReturn = false;
            }
            else if ($('#Title').val().length < 10) {
                validationForTitle.innerHTML = "Display name should contain at least 10 to 100 characters";
                validationForTitle.style.display = 'block';
                flgReturn = false;
            }
            else { validationForTitle.style.display = 'none;' }
        }

        if (PageNameValid == "link") {
            if ($('#Link_url').val() == '') {
                validationForURL.innerHTML = "URL is required";
                validationForURL.style.display = 'block';
                flgReturn = false;
            }
            else { validationForURL.style.display = 'none' }
            if ($('#Link_url-error').text() != '') {
                flgReturn = false;
            }
        }
        //
        let AssoFileHTML = $('#AssoFileHTML').val();
        let AssoFileImg1 = $('#AssoFileImg1').val();
        let AssoFileImg2 = $('#AssoFileImg2').val();
        let AssoFileImg3 = $('#AssoFileImg3').val();
        let AssoFileThumbImg = $('#AssoFileThumbImg').val();
        let MswordFile = $('#MswordFile').val();
        let PDFFile = $('#PDFFile').val();
        let VideoFile = $('#VideoFile').val();

        let extension = "";
        if (AssoFileHTML != null && AssoFileHTML != "") {
            extension = AssoFileHTML.replace(/^.*\./, '');
            if (extension.toLowerCase() != 'html' && extension.toLowerCase() != 'htm') {
                $("#AssoFileHTMLValidation").addClass("invalid");
                flgReturn = false;
            }

            //check  size 52428800 bytes = 50 mb
            if ($('#AssoFileHTML') != null && $('#AssoFileHTML').length > 0) {
                if ($('#AssoFileHTML').prop("files")[0].size > Smallfilesize) {
                    $("#AssoFileHTMLSizeValid").addClass("invalid");
                    flgReturn = false;
                }
            }

        }
        if (AssoFileImg1 != null && AssoFileImg1 != "") {
            extension = "";
            extension = AssoFileImg1.replace(/^.*\./, '');
            if (extension.toLowerCase() != 'png'
                && extension.toLowerCase() != 'jpg'
                && extension.toLowerCase() != 'jpeg'
                && extension.toLowerCase() != 'bmp'
                && extension.toLowerCase() != 'doc'
                && extension.toLowerCase() != 'docx'
                && extension.toLowerCase() != 'txt'
                && extension.toLowerCase() != 'pdf') {
                $("#AssoFileImg1Validation").show();
                $("#AssoFileImg3ValidationCommon").addClass("invalid")
                flgReturn = false;
            }
            //check  size 52428800 bytes = 50 mb
            if ($('#AssoFileImg1') != null && $('#AssoFileImg1').length > 0) {
                if ($('#AssoFileImg1').prop("files")[0].size > Smallfilesize) {
                    $("#AssoFileImg3SizeValidCommon").addClass("invalid");
                    $("#AssoFileImg1Validation").show();
                    flgReturn = false;
                }
            }
        }
        if (AssoFileImg2 != null && AssoFileImg2 != "") {
            extension = "";
            extension = AssoFileImg2.replace(/^.*\./, '');
            if (extension.toLowerCase() != 'png'
                && extension.toLowerCase() != 'jpg'
                && extension.toLowerCase() != 'jpeg'
                && extension.toLowerCase() != 'bmp'
                && extension.toLowerCase() != 'doc'
                && extension.toLowerCase() != 'docx'
                && extension.toLowerCase() != 'txt'
                && extension.toLowerCase() != 'pdf') {
                $("#AssoFileImg2Validation").show();
                $("#AssoFileImg3ValidationCommon").addClass("invalid")
                flgReturn = false;
            }
            //check  size 52428800 bytes = 50 mb
            if ($('#AssoFileImg2') != null && $('#AssoFileImg2').length > 0) {
                if ($('#AssoFileImg2').prop("files")[0].size > Smallfilesize) {
                    $("#AssoFileImg3SizeValidCommon").addClass("invalid");
                    $("#AssoFileImg2Validation").show();
                    flgReturn = false;
                }
            }
        }
        if (AssoFileImg3 != null && AssoFileImg3 != "") {
            extension = "";
            extension = AssoFileImg3.replace(/^.*\./, '');
            if (extension.toLowerCase() != 'png'
                && extension.toLowerCase() != 'jpg'
                && extension.toLowerCase() != 'jpeg'
                && extension.toLowerCase() != 'bmp'
                && extension.toLowerCase() != 'doc'
                && extension.toLowerCase() != 'docx'
                && extension.toLowerCase() != 'txt'
                && extension.toLowerCase() != 'pdf') {
                $("#AssoFileImg3Validation").show();
                $("#AssoFileImg3ValidationCommon").addClass("invalid")
                flgReturn = false;
            }
            //check  size 52428800 bytes = 50 mb
            if ($('#AssoFileImg3') != null && $('#AssoFileImg3').length > 0) {
                if ($('#AssoFileImg3').prop("files")[0].size > Smallfilesize) {
                    $("#AssoFileImg3SizeValidCommon").addClass("invalid");
                    $("#AssoFileImg3Validation").show();
                    flgReturn = false;
                }
            }
        }
        if (AssoFileThumbImg != null && AssoFileThumbImg != "") {
            extension = "";
            extension = AssoFileThumbImg.replace(/^.*\./, '');
            if (extension.toLowerCase() != 'png'
                && extension.toLowerCase() != 'jpg'
                && extension.toLowerCase() != 'jpeg') {
                $("#AssoFileThumbImgValidation").addClass("invalid");
                flgReturn = false;
            }

            //check  size 52428800 bytes = 50 mb
            if ($('#AssoFileThumbImg') != null && $('#AssoFileThumbImg').length > 0) {
                if ($('#AssoFileThumbImg').prop("files")[0].size > Smallfilesize) {
                    $("#AssoFileThumbImgSizeValid").addClass("invalid");
                    flgReturn = false;
                }
            }
        }
        if (MswordFile != null && MswordFile != "") {

            extension = "";
            extension = MswordFile.replace(/^.*\./, '');
            if (extension.toLowerCase() != 'doc'
                && extension.toLowerCase() != 'docx') {
                $("#MswordFileValidation").addClass("invalid");
                flgReturn = false;
            }

            //check  size 52428800 bytes = 50 mb
            if ($('#MswordFile') != null && $('#MswordFile').length > 0) {

                if ($('#MswordFile').prop("files")[0].size > Smallfilesize) {
                    $("#MswordFileSizeValid").addClass("invalid");;
                    flgReturn = false;
                }
            }
        }

        if (PDFFile != null && PDFFile != "") {
            extension = "";
            extension = PDFFile.replace(/^.*\./, '');
            if (extension.toLowerCase() != 'pdf') {
                $("#PDFFileValidation").addClass("invalid");
                flgReturn = false;
            }
            //check  size 52428800 bytes = 50 mb
            if ($('#PDFFile') != null && $('#PDFFile').length > 0) {
                if ($('#PDFFile').prop("files")[0].size > Smallfilesize) {
                    $("#PDFFileSizeValid").addClass("invalid");;
                    flgReturn = false;
                }
            }
        }
        let selectedPortal = 0;
        selectedPortal = $(".checkH:checked").length;
        if (selectedPortal < 1) {
            $("#PortalError").show();
            flgReturn = false;
        }

        if (PageNameValid == "download") {
            let ddlFruits = document.getElementById("DownloadType");
            if (ddlFruits.value == 0) {
                $("#DownTypeValidation").show();
                flgReturn = false;
            }



            //check upload file
            if (MswordFile == "" && PDFFile == "" && VideoFile == "" && hf_MswordFile == "" && hf_PdfFile == "" && hf_VideoFile == "") {
                $("#Documentselect").show();
                flgReturn = false;
            }
        }
        if (VideoFile != null && VideoFile != "") {
            extension = "";
            extension = VideoFile.replace(/^.*\./, '');

            if (extension.toLowerCase() != 'mp4') {
                $("#VideoValidation").addClass("invalid");
                flgReturn = false;
            }
            //check video size 104857600 bytes = 100 mb
            if ($('#VideoFile') != null && $('#VideoFile').length > 0) {
                if ($('#VideoFile').prop("files")[0].size > 104857600) {
                    $("#VideoSizeValid").addClass("invalid");;
                    flgReturn = false;
                }
            }
        }

        if (flgReturn == true) {
            //startAnimation();
        }

        return flgReturn;
    });
}

function ShoHotNewsPopup(HOT_NEWS_NAME) {
    ShowErrorPopup('Hot news "' + HOT_NEWS_NAME + '" already exists. Please change priority or delete existing hot news');
}

function NothingRedirectHotnews() {
    CloseWarningPopup();
}
function hideError() {
    $("#PortalError").hide();
}
function RedirectToMainPage() {
    
    if (hf_PageName == 'info story') {
        window.location.href = "/Information/ManageHelpAndInformations";
    }
    else if (hf_PageName == 'news story') {
        window.location.href = "/Information/ManageNews";
    }
    else if (hf_PageName == 'download') {
        window.location.href = "/Information/ManageDocuments";
    }
    else if (hf_PageName == 'link') {
        window.location.href = "/Information/ManageLinks";
    }
}

// Add the following code if you want the name of the file appear on select
$(".custom-file-input").on("change", function () {
    let fileName = $(this).val().split("\\").pop();
    $(this).parent('.custom-file').find(".custom-file-label").addClass("selected").html(fileName);
});
$("#MswordFile").on("change", function () {

    $("#MswordFileValidation").removeClass("invalid");
    let fileName = $(this).val().split("\\").pop();
    let MswordFile = $('#MswordFile').val();
    extension = "";
    extension = MswordFile.replace(/^.*\./, '');
    if (extension.toLowerCase() == 'doc'
        || extension.toLowerCase() == 'docx') {
        $(this).siblings(".custom-file-label5").addClass("selected").html(fileName);
        $("#AssociatedFile5Div").show();
        $("#btnRemoveAttachment5").show();
        $("#AssociatedFile5Div").css('display', 'block');
    }
    else if (extension != "") {
        $("#MswordFileValidation").addClass("invalid");
    }
});

$("#PDFFile").on("change", function () {
    $("#PDFFileValidation").removeClass("invalid");
    let fileName = $(this).val().split("\\").pop();
    let PDFFile = $('#PDFFile').val();
    extension = "";
    extension = PDFFile.replace(/^.*\./, '');
    if (extension.toLowerCase() == 'pdf'
        || extension.toLowerCase() == 'PDF') {
        $(this).siblings(".custom-file-label5").addClass("selected").html(fileName);
        $("#AssociatedFile5Div").show();
        $("#btnRemoveAttachment5").show();
        $("#AssociatedFile5Div").css('display', 'block');
    }
    else if (extension != "") {
        $("#PDFFileValidation").addClass("invalid");
    }
});
$("#VideoFile").on("change", function () {
    $("#VideoValidation").removeClass("invalid");
    let fileName = $(this).val().split("\\").pop();
    let VideoFile = $('#VideoFile').val();
    extension = "";
    extension = VideoFile.replace(/^.*\./, '');
    if (extension.toLowerCase() == 'mp4'
        || extension.toLowerCase() == 'MP4') {
        $(this).siblings(".custom-file-label5").addClass("selected").html(fileName);
        $("#AssociatedFile5Div").show();
        $("#btnRemoveAttachment5").show();
        $("#AssociatedFile5Div").css('display', 'block');
    }
    else if (extension != "") {
        $("#VideoValidation").addClass("invalid");
    }
});



