

//$('.dropdown-toggle').dropdown();
function boldfunc() {
    // document.getElementByTagName("textarea").style.fontFamily="lato_bold";
    document.getElementById("textAID").style.fontStyle = "normal";
    document.getElementById("textAID").style.fontWeight = "900";
}
function italicfunc() {
    // document.getElementByTagName("textarea").style.fontFamily="lato_bold";
    document.getElementById("textAID").style.fontStyle = "italic";
    document.getElementById("textAID").style.fontWeight = "200";
}
function underlinefunc() {
    // document.getElementByTagName("textarea").style.fontFamily="lato_bold";
    document.getElementById("textAID").style.fontStyle = "normal";
    document.getElementById("textAID").style.textDecoration = "underline";
    document.getElementById("textAID").style.fontWeight = "500";
}
function suppressBackspace(evt) {
    evt = evt || window.event;
    var target = evt.target || evt.srcElement;

    if (evt.keyCode == 8 && !/input|textarea|/i.test(target.nodeName)) {
        return false;
    }
}

var keyStr = "ABCDEFGHIJKLMNOP" +
    "QRSTUVWXYZabcdef" +
    "ghijklmnopqrstuv" +
    "wxyz0123456789+/" +
    "=";
var Array_replace = {};

var checkSaveOrNot = "";

$(document).ready(function () {
    disableContent();
    AutoResMsg();
    var verdistributed = $('#VersionDistributed').val();
    ////noBack();
    var string1 = $("#NotesforHaulier").val();
   
    /* const result = isJSON(string);*/
    
    //let htmlString = $("#NotesforHaulier").val();
    //let string = htmlString.replace(/<[^>]+>/g, '');
    //alert(string);
    //if (string.length > 0) {
        //$('#redactor_content').text(string);
        //$('.redactor_editor').text(string);
        
        //$('#savedhn').text(string);

    //}
   
    if ($('#structureData').val() == '') {
        $('#clearstruct').hide();
    }
    $('input:file').change(
        function () {
            if ($(this).val()) {
                $('.cancelorg').show();
                // or, as has been pointed out elsewhere:
                // $('input:submit').removeAttr('disabled'); 
            }
        }
    );
    var pstatus = $('#AppStatusCode').val();
    var chk_status = $('#CheckerStatus').val() ? $('#CheckerStatus').val() : 0;
    var sort_user_id = $('#SortUserId').val() ? $('#SortUserId').val() : 0;
    var checker_id = $('#CheckerId').val() ? $('#CheckerId').val() : 0;
    if ((chk_status == 301002 || chk_status == 301008 || chk_status == 301005) && (sort_user_id != checker_id) && $('#VR1Applciation').val() == "False") {

        $('.BtnSaveNotes2Haulier').hide();
        $('#page1').css('margin-top', 0);
    }
    if (chk_status == 301006 && $('#VR1Applciation').val() == "False" && pstatus != 307011) {

        $('.BtnSaveNotes2Haulier').hide();
        $('#page1').css('margin-top', 0);
    }
    var verstratus = $('#versionStatus').val();
    var latestversionno = $('#MovLatestVer').val();
    var prjstatuscode = $('#AppStatusCode').val();
    
    if (1 == 1) {
        $('#redactor_content').redactor({ buttons: ['bold', 'italic', 'underline', '|', 'unorderedlist', '|'],placeholder: 'Enter a description for this job...'});
        /*$('#redactor_content').redactor({ buttons: ['bold', 'italic', 'underline', '|', 'unorderedlist', '|', 'save'] });*/
        $('#savedhn').hide();
        
    }
    else {
        if ($('#SortStatus').val() == "CandidateRT") {
            $('#redactor_content').redactor({ buttons: ['bold', 'italic', 'underline', '|', 'unorderedlist', '|'] });
            $('#savedhn').hide();
        }
        else {
            $('#redactor_content').redactor({ toolbar: false });
            $('.redactor_box').remove();
            $('.BtnSaveNotes2Haulier').hide();
        }
    }
    $(".redactor_editor").keypress(function () {
        $("#textValue").val(1);
    });

    checkSaveOrNot = $('#redactor_content').val();

});
var ResStatus = $('#ResponseStatus').val();

function TextBoxClear(id, fileid) {
    $("#" + fileid).replaceWith($("#" + fileid).val('').clone(true));
    var textid = document.getElementById(id).value = "";
    if ($('#structureData').val() == '') {
        $('#clearstruct').hide();
    }
}

$('body').on('click', '#clearstruct', function (e) {
    e.preventDefault();
    var id = $(this).data('structuredata');
    var fileid = $(this).data('structurefile');
    TextBoxClear(id, fileid);
});
$('body').on('change', '#structurefile', function (e) {
    e.preventDefault();
    var structureData = $(this).data('structuredata');
    startRead(this.files, 1, structureData)
});
$('body').on('click', '#btnBrowse', function (e) {
    e.preventDefault();
    var fileid = $(this).data('structurefile');
    UploadFile(fileid);
});
$('body').on('click', '#ViewRes', function (e) {
    e.preventDefault();
    ViewPDF();
});
$('body').on('click', '#btn_setpr', function (e) {

    e.preventDefault();
    checkWarning();
});
$('body').on('click', '.close-popup', function (e) {
    e.preventDefault();
    closePopup();
});
$('input[type=radio][name=IN_TYPE]').change(function () {
    if (this.value == '1') {
        //alert("Allot Thai Gayo Bhai");
        $('#btnBrowse').prop('disabled', false);
        /*$('#btn_setpr').prop('disabled', false);*/
        $('#redactor_content').prop('disabled', false);
        $('#ViewRes').prop('disabled', false);
        $("#page2").removeClass("disabledbutton");


        ResStatus = 1;

    }
    else if (this.value == '0') {
        //alert("Transfer Thai Gayo");
        $('#btnBrowse').prop('disabled', true);
        /* $('#btn_setpr').prop('disabled', true);*/
        $('#redactor_content').prop('disabled', true);
        $('#ViewRes').prop('disabled', true);
        $("#page2").addClass("disabledbutton");


        ResStatus = 0;

    }
});
function UploadFile(file) {

    document.getElementById("structureData").value = "";
    var control = document.getElementById(file).click();
    var strval = $("#structureData").val();
    console.log(control);
    console.log(strval);

}

function startRead(file, type, id) {
    if (file && file.length > 0 && id) {
        document.getElementById(id).value = file[0].name;

        //checkFileExist(type);
        //fileList.push({file: file, type: type, id: id });
    }

}

function saveAutoRes() {

    var ReplyMailPath = $('#ReplyMailPath').val();

    var filesexst = $("#structurefile").get(0);
    if (filesexst === undefined) {
        // alert('b1');
        SubmitPreference();
    }
    else {
        //alert('b2');
        var files = $("#structurefile").get(0).files;

        if (files.length > 0) {
            // alert('b3');
            var filesize = files[0].size;

            var filetype = files[0].type;
            if (filetype != "application/pdf") {
                // alert('b4');
                ShowErrorPopup('Please upload only PDF with maximum 2MB file size', '');
                return false;
            }

            if (filesize < 2097152) {
                //alert('b5');

            }
            else {
                //alert('b6');
                ShowErrorPopup('Please upload only PDF with maximum 2MB file size', '');
                return false;
            }
            if (ReplyMailPath == "1") {
                //alert('b7');
                //SubmitPreference();
                //alert('The existing PDF will be replaced with the new one. Do you want to continue?');
                ShowWarningPopup('The existing PDF will be replaced with the new one. Do you want to continue?', 'SubmitPreference');
            }
            else {
                //alert('b8');
                SubmitPreference();
            }

        }
        else {
            //alert('b9');
            SubmitPreference();
        }

    }


}


function SubmitPreference() {

    // $('#pop-warning').hide();
    $('#WarningPopup').modal('hide');
    
    var USERID = 0;

    var filenull;
    var pdfExst = 0;
    var data1 = new FormData();
    var filesexst = $("#structurefile").get(0);
    if (filesexst !== undefined) {
        var files = $("#structurefile").get(0).files;

        if (files.length > 0) {
            var filesize = files[0].size;

            var filetype = files[0].type;
            if (filetype != "application/pdf") {
                ShowErrorPopup('Please upload only PDF with maximum 2MB file size', 'ToReload');
                // showWarningPopDialog('Please upload only PDF with maximum 2MB file size', '', 'OK', '', 'WarningCancelBtn', 1, 'error');
                return false;
            }

            if (filesize < 2097152) {

                pdfExst = 1;
                data1.append("HelpSectionImages", files[0]);
                filenull = "true";
            }
            else {
                ShowErrorPopup('Please upload only PDF with maximum 2MB file size', 'ToReload');
                /// showWarningPopDialog('Please upload only PDF with maximum 2MB file size', '', 'OK', '', 'WarningCancelBtn', 1, 'error');
                return false;
            }
        }
    }


    var userID = $('#UserId').val();
    var USERID = $('#UserType').val();

    var pagesize = $('#ListItemsSelect').val();
    emailtxt = $('#Emailtext1').val();
    Faxnumber = $('#Faxtext1').val();
    var NoEmail = 1;


    if (USERID == '696006') {
        VehUnits = 0;
        DrivInstr = 0;
        Enable = false;
        commMethod = 0;
        XMLEnable = false;
        emailtxt = $('#Email').val();
        Faxnumber = $('#Fax').val();
        function Validation() {
            return true;
        }
    }

    if (USERID == '696001' || USERID == '696007' || USERID == '696002') //added this code to allow to work on IE.
    {

        function Validation() {
            return true;
        }
    }
    if (Validation()) {
        startAnimation();
        if (NoEmail == 1) {
            $.ajax({
                url: '../Account/UserPrefer',
                type: 'POST',
                dataType: 'json',
                data: { UserId: userID, filenull: filenull, ResStatus: ResStatus, pdfExst: pdfExst },
                success: function (Result) {
                    if (Result.result == true) {
                        if (ResStatus == 1) {
                            if (files.length > 0) {
                                Respopup = 1;
                                $.ajax({
                                    url: '../Account/SaveResponseMsg', type: "POST", processData: false,
                                    data: data1,
                                    //dataType: 'json',
                                    contentType: false,
                                    success: function (response) {

                                        if (response != "Data Saved Successfully") {
                                            //alert('response');
                                            //ShowWarningPopup('Please upload only PDF with maximum 2MB file size', 'ToReload');
                                            ShowSuccessModalPopup(response, '');
                                            //location.reload();
                                            //return false;
                                        }
                                        else {
                                            ShowSuccessModalPopup('Data Saved Successfully', 'ToReload'); //WarningCancelBtn

                                        }
                                        if (response != null || response != '')
                                            $("#file").val('');
                                        ShowSuccessModalPopup('Data Saved Successfully', 'ToReload');
                                        stopAnimation();
                                    },
                                    error: function (er) {
                                        stopAnimation();
                                    },
                                    complete: function () {
                                        //alert('complete function');
                                        stopAnimation();
                                        $("#dialogue").html('');
                                        $('#overlay').hide();
                                        resetdialogue();
                                        //showWarningPopDialog('User preferences saved successfully.', 'Ok', '', 'ReloadLocation1', '', 1, 'info'); //WarningCancelBtn
                                        // startAnimation();
                                        addscroll();
                                    }
                                });
                            }
                            else {
                                // alert('13');
                                ShowSuccessModalPopup('Data Saved Successfully', 'ToReload');

                            }
                        } else {
                            stopAnimation();
                        }

                    }
                    else {
                        stopAnimation();
                        ShowSuccessModalPopup(Result.ResMsg, 'CloseSuccessModalPopup');
                        return false;
                    }


                },
                error: function () {
                    location.reload();
                },
                complete: function () {
                    if (ResStatus == 0) {
                        ShowSuccessModalPopup('Data Saved Successfully', 'ToReload');
                    }
                    //stopAnimation();
                }
            });
        }
        else {
            //alert('4');
            ShowSuccessModalPopup('Data Saved Successfully', 'ToReload');
            stopAnimation();
        }


    }
}


function ToReload() {
    location.reload();
}

function closePopup() {

    $('#cautionPopup').modal('hide');
    checkWarning();
    //$('#SuccessPopup').modal('hide');

}



function ViewPDF() {
    if (ResStatus == 1) {


        var url = $('#ReplyMailPathStr').val();
        if (url == "") {
            ShowModalPopup('No PDF available.', '');

        }
        else {


            var splitted = url.split('/'); //this will output ["1234", "56789"]
            url = splitted[splitted.length - 1];
            var backslash = url.split('\\');
            url = backslash[backslash.length - 1];

            var link = "../Account/ViewResponsePdfDoc?Pdf=" + url + "";
            //var link = "../DocumentConsole/HaulierNotificationDocument?notificationId=" + NotificationId + "&contactId=" + ContactId + "";

            window.open(link, '_blank');
        }

    }


};


var keyStr = "ABCDEFGHIJKLMNOP" +
    "QRSTUVWXYZabcdef" +
    "ghijklmnopqrstuv" +
    "wxyz0123456789+/" +
    "=";
var Array_replace = {};

var checkSaveOrNot = "";

//function BtnClosepreferencesNotes() {


//    var SetPreference = $('#checkHaulierNoteMsg').val();


//    var string = $('#redactor_content').val();
//    //var string1 = $('#redactor_content').val().trim();

//    //var mySubString = string.substring(string.lastIndexOf("<p>") + 1, string.lastIndexOf("</p>"));
//    var mySubString1 = string.match(new RegExp("<p>" + "(.*)" + "</p>"));

//    var new_str = string.substring(0, string.indexOf("</p>"));
//    new_str = new_str.trim();
//    var laststr = new_str.slice(-3);

//    if (laststr == "<p>") {
//        var tempStr = string.substring(string.indexOf("</p>"), string.length);
//        tempStr = tempStr.substring(4);

//        new_str = new_str.substring(0, new_str.length - 3);
//        string = new_str + tempStr;
//    }

//    var cleanText = string.replace(/<\/?[^>]+(>|$)/g, "");
//    cleanText = cleanText.trim();
//    //return false;

//    var textValue = $("#textValue").val();

//    var changedText = $('#editor1').val();

//    if (cleanText != "" && changedText != checkSaveOrNot) {

//        ShowWarningPopUp('Do you want to save your changes ?','closePreferNotes', 'BtnSaveOk', 1, 'warning');
//    }


//    else { closePreferNotes(); }
//}
function closePreferNotes() {
    $("#dialogue1").hide();
    $('#dialogue').show();
}




//function checkWarning() {
//    var data1 = document.getElementById('DisblAutoRes').checked;
//    var oldMsg = $("#txthauliernotes").val();
//    var url = $('#ReplyMailPathStr').val();
//    var string = $('#redactor_content').val();
//    var cleanText = string.replace(/<\/?[^>]+(>|$)/g, "");
//    cleanText = cleanText.trim();
//    var par = true;
//    if (oldMsg != null && data1 != true && url != null) {
//        /*    BtnSaveNotes2HaulierMsg(true);*/
//        if (cleanText == "") {
//            ShowErrorPopup("Please enter the notes before saving");
//        } else {
//            CloseErrorPopup();
//            ShowWarningPopup('The existing data will be replaced with the new one. Do you want to continue?', 'BtnSaveNotes2HaulierMsg(' + par + ')', "ToReload");
//        }
//   }
//    else {
//        CloseWarningPopup();
//        BtnSaveNotes2HaulierMsg(true);
//         }
//}
function checkWarning() {

    var SetPreference = $('#checkHaulierNoteMsg').val();
    var string = $('#redactor_content').val();
    //var string1 = $('#redactor_content').val().trim();

    //var mySubString = string.substring(string.lastIndexOf("<p>") + 1, string.lastIndexOf("</p>"));
    var mySubString1 = string.match(new RegExp("<p>" + "(.*)" + "</p>"));

    var new_str = string.substring(0, string.indexOf("</p>"));
    new_str = new_str.trim();
    var laststr = new_str.slice(-3);

    if (laststr == "<p>") {
        var tempStr = string.substring(string.indexOf("</p>"), string.length);
        tempStr = tempStr.substring(4);

        new_str = new_str.substring(0, new_str.length - 3);
        string = new_str + tempStr;
    }

    //var n1 = string.match(/<br>/g);
    //var n = string.search("<br>");
    //string = string.replace(/<br>/g, '');

    var cleanText = string.replace(/<\/?[^>]+(>|$)/g, "");
    cleanText = cleanText.trim();
    //return false;



    //if (SetPreference == "True" && cleanText != "") {

    //    // alert("Password changed successfully.");
    //    ShowWarningPopupParam('The existing PDF will be replaced with the new one. Do you want to continue?', 'BtnSaveNotes2HaulierMsg', "ToReload", true);

    //}
    //else {
    BtnSaveNotes2HaulierMsg(true);

    // }
}



function BtnSaveOk() {
    BtnSaveNotes2Haulier(true);
}
function checkResponseMsg() {
    var string = $('#redactor_content').val();
    var cleanText = string.replace(/<\/?[^>]+(>|$)/g, "");
    cleanText = cleanText.trim();
    var disableMsg = document.getElementById('DisblAutoRes').checked;

    if (cleanText != "" || disableMsg == true) {
        checkWarning();

    }
    else {
        $('#cautionPopup').modal('show');

    }
}
//function for save
function BtnSaveNotes2HaulierMsg(savcls) {


    CloseWarningPopup();
    //startAnimation();
    var string = $('#redactor_content').val();
    var cleanText = string.replace(/<\/?[^>]+(>|$)/g, "");
    cleanText = cleanText.trim();
    var disableMsg = document.getElementById('DisblAutoRes').checked;

    if (cleanText != "" || disableMsg == true) {
        string = string.replace(/style="[^"]*"/g, "");
        if (string != " ") {
            $.ajax({
                url: "../SORTApplication/SavepreferenceNotes",
                type: 'post',
                async: false,
                data: { XmlHaulierNOtes: string, saveorCncl: savcls },
                success: function (data) {

                    startAnimation();
                    if (savcls == true) {
                        if (data == true) {
                            $('#checkHaulierNoteMsg').val("True");
                            saveAutoRes();
                        }
                        else {
                            saveAutoRes();
                        }

                    }
                    else {
                        saveAutoRes();
                    }
                    stopAnimation();
                },
                error: function () {
                    //alert('error');
                    //showWarningPopDialog('Please remove the special characters!', '', 'OK', '', 'WarningCancelBtn', 1, 'error');
                    ShowErrorPopup('The changes to haulier notes have not been saved');
                    stopAnimation();
                }
            });
        }
        //else {
        //    ShowErrorPopup('Please enter the notes before saving');
        //}
    }
    else {
        //alert('Please enter the notes before saving');
        stopAnimation();
        $('#cautionPopup').modal('show');
        $('#checkHaulierNoteMsg').val("false");
    }
}

function encode64(input) {
    //input = escape(input);
    var output = "";
    var chr1, chr2, chr3 = "";
    var enc1, enc2, enc3, enc4 = "";
    var i = 0;

    do {
        chr1 = input.charCodeAt(i++);
        chr2 = input.charCodeAt(i++);
        chr3 = input.charCodeAt(i++);

        enc1 = chr1 >> 2;
        enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
        enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
        enc4 = chr3 & 63;

        if (isNaN(chr2)) {
            enc3 = enc4 = 64;
        } else if (isNaN(chr3)) {
            enc4 = 64;
        }

        output = output +
            keyStr.charAt(enc1) +
            keyStr.charAt(enc2) +
            keyStr.charAt(enc3) +
            keyStr.charAt(enc4);
        chr1 = chr2 = chr3 = "";
        enc1 = enc2 = enc3 = enc4 = "";
    } while (i < input.length);

    return output;
}
function disableContent() {
    var data1 = document.getElementById('DisblAutoRes').checked;
    if (data1 == true) {
        $('#btnBrowse').prop('disabled', true);
        /* $('#btn_setpr').prop('disabled', true);*/
        $('#redactor_content').prop('disabled', true);
        $('#ViewRes').prop('disabled', true);
        $("#page2").addClass("disabledbutton");


    }

}
//method for showing help to the user
function AutoResMsg() {

    if (ResStatus == 1) {
        $('#redactor_content').text('');
        var url = geturl(location.href);
        var urlkey = $("#URLKEY").val();
        if (urlkey != "") { url = url + urlkey; }
        $('#url_html_page').val(url);
        $('#fflag').val(0);
        $.ajax({
            url: '/Account/ViewResponseMsg',
            type: 'POST',
            data: { fileName: url, flag: 0 },
            beforeSend: function () {
                startAnimation();
            },
            success: function (page) {
                ViewResMsg(page);
                stopAnimation();
            },
            error: function (xhr, textStatus, errorThrown) {
                GetPageException(errorThrown);
                stopAnimation();
            },
            complete: function () {
                //stopAnimation();
                $('.loading').hide();
                stopAnimation();
            }
        });
        //$('#form_html_fetch').submit();
    }
}



//function for load help page to the user
function ViewResMsg(result) {
    let results = result;//.replace(/<[^>]+>/g, '');
    //Remove html & body tags
    results = results.replace(/<html([^>]*)>/g, "");
    results = results.replace(/<body([^>]*)>/g, "");
    results = results.replace(/<\/html>/g, "");
    results = results.replace(/<\/body>/g, "");
    
    $('#redactor_content').text(results);
    $('.redactor_editor').html(results);
    stopAnimation();
}


