function boldfunc() {
    document.getElementById("redactor_content").style.fontStyle = "normal";
    document.getElementById("redactor_content").style.fontWeight = "900";
}
function italicfunc() {
    document.getElementById("redactor_content").style.fontStyle = "italic";
    document.getElementById("redactor_content").style.fontWeight = "200";
}
function underlinefunc() {
    document.getElementById("redactor_content").style.fontStyle = "normal";
    document.getElementById("redactor_content").style.textDecoration = "underline";
    document.getElementById("redactor_content").style.fontWeight = "500";
}
function suppressBackspace(evt) {
    evt = evt || window.event;
    var target = evt.target || evt.srcElement;

    if (evt.keyCode == 8 && !/input|textarea|/i.test(target.nodeName)) {
        return false;
    }
}
var keyStr;
var Array_replace = {};
$(document).on("keydown", function (e) { suppressBackspace(); });
function HaulierNotesInit() {
    $('#Mov_Withdraw').hide();
    $('#btndistributemovement').hide();

    keyStr = "ABCDEFGHIJKLMNOP" +
        "QRSTUVWXYZabcdef" +
        "ghijklmnopqrstuv" +
        "wxyz0123456789+/" +
        "=";
    var verdistributed = $('#VersionDistributed').val();
    noBack();
    var string = $("#txthauliernotes").val();
    if (string.length > 0) {
        $('#redactor_content').text(string);
        $('.redactor_editor').html(string);
        $('#savedhn').html(string);
    }
    var pstatus = $('#AppStatusCode').val();
    var chk_status = $('#CheckerStatus').val() ? $('#CheckerStatus').val() : 0;
    var sort_user_id = $('#SortUserId').val() ? $('#SortUserId').val() : 0;
    var checker_id = $('#CheckerId').val() ? $('#CheckerId').val() : 0;
    //301002- checking , 301008 - QA Checking , 301005-Checking final
    if ((chk_status == 301002 || chk_status == 301008 || chk_status == 301005) &&
        (sort_user_id != checker_id) && $('#VR1Applciation').val() == "False") {
        $('.BtnSaveNotes2Haulier').hide();
        $('#redactor_content').hide();
        $('#page1').css('margin-top', 0);
    }
    
    //301006-Checked final positively, 
    if (chk_status == 301006 && $('#VR1Applciation').val() == "False" && pstatus != 307011) {//307011-revised
        $('.BtnSaveNotes2Haulier').hide();
        $('#page1').css('margin-top', 0);
    }
    var verstratus = $('#versionStatus').val();
    var latestversionno = $('#MovLatestVer').val();
    var prjstatuscode = $('#AppStatusCode').val();
    if (latestversionno == versionno) {
        if ((verstratus == 305004 || verstratus == 305006) && verdistributed == 1) {//305004-agreed ,  305006-agreed recleared
            //if (prjstatuscode == 307014) {
            $('#redactor_content').redactor({ toolbar: false });
            $('.redactor_box').remove();
            $('.BtnSaveNotes2Haulier').hide();
        }
        else if (chk_status != 301008) {//For QA checking
            $('#redactor_content').redactor({ buttons: ['bold', 'italic', 'underline', '|', 'unorderedlist'] });
            $('#savedhn').hide();
        }
        //301002- checking , 301008 - QA Checking , 301005-Checking final
        else if ((chk_status == 301002 || chk_status == 301008 || chk_status == 301005) && sort_user_id == checker_id) {//&& $('#VR1Applciation').val() == "False"
            $('#redactor_content').redactor({ buttons: ['bold', 'italic', 'underline', '|', 'unorderedlist'] });
            $('#savedhn').hide();
        }
        else {
            
        }
    }
    else {
        if ($('#SortStatus').val() == "CandidateRT" || $('#SortStatus').val() == "ViewProj") {
            $('#redactor_content').redactor({ buttons: ['bold', 'italic', 'underline', '|', 'unorderedlist'] });
            $('#savedhn').hide();
        }
        else {
            $('#redactor_content').redactor({ toolbar: false });
            $('.redactor_box').remove();
            $('.BtnSaveNotes2Haulier').hide();
        }
    }
}

$(document).ready(function () {
    $('body').on('click', '#btn-savenote', BtnSaveNotes2Haulier);
    $('.modal-content').css('background', 'white');
});
//function for save
function BtnSaveNotes2Haulier() {
   
    $('.modal-content').css('background', 'white');
    var version_id = $("#versionId").val();
    var string = $('#redactor_content').val();
    var cleanText = string.replace(/<\/?[^>]+(>|$)/g, "");
    if (cleanText != "") {
        Array_replace = initarray(Array_replace);
        string = parsingtoxml(string, Array_replace);
       var parser = new DOMParser();
        parser.parseFromString(string, "text/xml");

        if (version_id != null && string != " ") {
            $.ajax({
                url: "../sortapplication/savehauliernotes",
                type: 'post',
                //async: false,
                data: { versionid: version_id, xmlhauliernotes: string },
                beforeSend: function () {
                    startAnimation();
                },
                success: function (data) {
                    if (data == true)
                        showToastMessage({message: 'Notes saved successfully',type: "success"})
                    else
                        showToastMessage({message: 'The changes to haulier notes have not been saved',type: "error"})
                },
                error: function () {
                    showToastMessage({ message: 'The changes to haulier notes have not been saved', type: "error" });
                    stopAnimation();
                },
                complete: function () {
                    stopAnimation();
                }
            });
        }
    }
    else {
        showToastMessage({message: 'Please enter the notes before saving',type: "error"})
    }

}
//function for init()
function initarray() {
    var Array_replace = {};
    Array_replace = [
        ["<br>", "<Br></Br>"],
        [/<b([^>]*)>/g, "<Bold xmlns='http://www.esdal.com/schemas/core/formattedtext'>"],
        [/<i([^>]*)>/g, "<Italic xmlns='http://www.esdal.com/schemas/core/formattedtext'>"],
        [/<u([^>]*)>/g, "<Underscore xmlns='http://www.esdal.com/schemas/core/formattedtext'>"],
        [/<ul([^>]*)>/g, "<Underscore xmlns='http://www.esdal.com/schemas/core/formattedtext'>"],
        [/<li([^>]*)>/g, "<BulletedText xmlns='http://www.esdal.com/schemas/core/formattedtext'>"],
        [/<p([^>]*)>/g, "<Para xmlns='http://www.esdal.com/schemas/core/formattedtext'>"],
        [/<span([^>]*)>/g, "<Para xmlns='http://www.esdal.com/schemas/core/formattedtext'>"],
        [/<div([^>]*)>/g, "<Div xmlns='http://www.esdal.com/schemas/core/formattedtext'>"],

        ["<b>", "<Bold xmlns='http://www.esdal.com/schemas/core/formattedtext'>"],
        ["<i>", "<Italic xmlns='http://www.esdal.com/schemas/core/formattedtext'>"],
        ["<u>", "<Underscore xmlns='http://www.esdal.com/schemas/core/formattedtext'>"],
        ["<ul>", "<Underscore xmlns='http://www.esdal.com/schemas/core/formattedtext'>"],
        ["<li>", "<BulletedText xmlns='http://www.esdal.com/schemas/core/formattedtext'>"],
        ["<p>", "<Para xmlns='http://www.esdal.com/schemas/core/formattedtext'>"],
        ["<span>", "<Para xmlns='http://www.esdal.com/schemas/core/formattedtext'>"],
        ["<div>", "<Div xmlns='http://www.esdal.com/schemas/core/formattedtext'>"],

        [/<\/b>/g, "</Bold>"],
        [/<\/i>/g, "</Italic>"],
        [/<\/u>/g, "</Underscore>"],
        [/<\/ul>/g, "</Underscore>"],
        [/<br>/g, " <Br></Br>"],
        [/<\/div>/g, " </Div>"],
        [/<\/p>/g, " </Para>"],
        [/<\/span>/g, " </Para>"],
        [/<\/li>/g, "</BulletedText>"],
        [/&nbsp;/g, " "]

    ];
    return Array_replace;
}
//converts a html string value to xml
function ds(str, Array_replace) {
    var string = str;
    var replace = ' style = "line-height: 1.45em; "';
    string = string.replace(new RegExp(replace, 'g'), "");
    //string = string.replaceAll(replace, "");
    for (var i = 0; i < Array_replace.length; i++) {
        string = string.replace(new RegExp(Array_replace[i][0], 'g'), Array_replace[i][1]);
        //string = string.replaceAll(Array_replace[i][0], Array_replace[i][1]);
    }
    string = "<?xml version='1.0' encoding='UTF-8'?><movementversion:NotesForHaulier xmlns:movementversion='http://www.esdal.com/schemas/common/movementversion'>" + string + "</movementversion:NotesForHaulier>";
    return string;
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
//converts a html string value to xml
function parsingtoxml(str, Array_replace) {
    for (var i = 0; i < Array_replace.length; i++) {
        str = str.replaceAll(Array_replace[i][0], Array_replace[i][1]);
    }
    str = "<?xml version='1.0' encoding='UTF-8'?><movementversion:NotesForHaulier xmlns:movementversion='http://www.esdal.com/schemas/common/movementversion'>" + str + "</movementversion:NotesForHaulier>";
    console.log(str);
    return str;
}
