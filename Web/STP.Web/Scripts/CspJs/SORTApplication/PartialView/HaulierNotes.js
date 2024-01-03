        //$('.dropdown-toggle').dropdown();

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
        var keyStr = "ABCDEFGHIJKLMNOP" +
            "QRSTUVWXYZabcdef" +
            "ghijklmnopqrstuv" +
            "wxyz0123456789+/" +
            "=";
        var Array_replace = {};
        $(document).on("keydown", function (e) { suppressBackspace(); });
        //document.onkeydown = suppressBackspace;
        //document.onkeypress = suppressBackspace;
        $(document).ready(function () {
            $("#btn-savenote").on('click', BtnSaveNotes2Haulier);
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
            if ((chk_status == 301002 || chk_status == 301008 || chk_status == 301005) && (sort_user_id != checker_id) && $('#VR1Applciation').val() == "False") {
                $('.BtnSaveNotes2Haulier').hide();
                $('#page1').css('margin-top', 0);
            }
            if (chk_status == 301006 && $('#VR1Applciation').val() == "False" && pstatus != 307011) {
                $('.BtnSaveNotes2Haulier').hide();
                $('#page1').css('margin-top', 0);
            }
            //var isdistributed = $('#IsMovDistributed').val();
            var verstratus = $('#versionStatus').val();
            var latestversionno = $('#MovLatestVer').val();
            var prjstatuscode = $('#AppStatusCode').val();
            if (latestversionno == versionno) {
                if ((verstratus == 305004 || verstratus == 305006) && verdistributed == 1) {
                    //if (prjstatuscode == 307014) {
                    $('#redactor_content').redactor({ toolbar: false });
                    $('.redactor_box').remove();
                    $('.BtnSaveNotes2Haulier').hide();
                }
                else {
                    $('#redactor_content').redactor({ buttons: ['bold', 'italic', 'underline', '|', 'unorderedlist'] });
                    $('#savedhn').hide();
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

        });

        //function for save
        function BtnSaveNotes2Haulier() {

            var version_Id = $("#versionId").val();
            var string = $('#redactor_content').val();
            var cleanText = string.replace(/<\/?[^>]+(>|$)/g, "");
            if (cleanText != "") {
                Array_replace = initarray(Array_replace);
                //string = '<p><br><br><u style="font-weight: bold;">HIGHWAYS AGENCY</u><br> A copy of the Special Order must be carried in the vehicle during movement.</p>';
                string = parsingtoxml(string.replace('<u style="">', '<u>'), Array_replace);

                //string = string.replace(/–/g, "-");
                //string = string.replace(/’/g, "'");

                //string = encode64(string);

                if (version_Id != null && string != " ") {
                    $.ajax({
                        url: "../SORTApplication/SaveHaulierNotes",
                        type: 'post',
                        async: false,
                        data: { VersionId: version_Id, XmlHaulierNOtes: string },
                        success: function (data) {

                            if (data == true)
                                // showWarningPopDialog('Data saved', '', 'OK', '', 'WarningCancelBtn', 1, 'info');
                                /*ShowSuccessModalPopupNotes('Notes saved successfully', 'CloseSuccessModalPopupNotes');*/
                                ShowSuccessModalPopup('Notes saved successfully', 'CloseSuccessModalPopup');
                            else
                                ShowErrorPopup('The changes to haulier notes have not been saved.', 'CloseErrorPopup');
                        },
                        error: function () {

                            //showWarningPopDialog('Please remove the special characters!', '', 'OK', '', 'WarningCancelBtn', 1, 'error');
                            ShowErrorPopup('The changes to haulier notes have not been saved.', 'CloseErrorPopup');
                        }
                    });
                }
            }
            else {
                //showWarningPopDialog('Please enter the notes before saving', '', 'OK', '', 'WarningCancelBtn', 1, 'error');
                ShowErrorPopup('Please enter the notes before saving', 'CloseErrorPopup');
            }

        }
        //function for init()
        function initarray() {
            var Array_replace = {};
            Array_replace = [
                ["<b>", "<Bold xmlns='http://www.esdal.com/schemas/core/formattedtext'>"],
                ["<b style=\"font-weight: bold;\">", "<Bold xmlns='http://www.esdal.com/schemas/core/formattedtext'>"],
                ["<i>", "<Italic xmlns='http://www.esdal.com/schemas/core/formattedtext'>"],
                ["<i style=\"font-weight: bold;\">", "<Italic xmlns='http://www.esdal.com/schemas/core/formattedtext'>"],
                ["<u>", "<Underscore xmlns='http://www.esdal.com/schemas/core/formattedtext'>"],
                ["<u style=\"font-weight: bold;\">", "<Underscore xmlns='http://www.esdal.com/schemas/core/formattedtext'>"],
                ["</b>", "</Bold>"],
                ["</i>", "</Italic>"],
                ["</u>", "</Underscore>"],
                ["<li>", "<BulletedText xmlns='http://www.esdal.com/schemas/core/formattedtext'>"],
                ["</li>", "</BulletedText>"],
                ["<br>", " <Br></Br>"],
                ["<p>", " <Para xmlns='http://www.esdal.com/schemas/core/formattedtext'>"],
                ["</p>", " </Para>"],
                ["<div>", " <Div xmlns='http://www.esdal.com/schemas/core/formattedtext'>"],
                ["</div>", " </Div>"],
                ["&nbsp;", " "]

            ];
            return Array_replace;
        }

        //converts a html string value to xml
        function ds(str, Array_replace) {
            var string = str;
            var replace = ' style = "line-height: 1.45em; "';
            string = string.replace(new RegExp(replace, 'g'), "");

            for (var i = 0; i < Array_replace.length; i++) {
                string = string.replace(new RegExp(Array_replace[i][0], 'g'), Array_replace[i][1]);
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
            var string = str;
            var replace = ' style = "line-height: 1.45em; "';
            string = string.replace(new RegExp(replace, 'g'), "");

            for (var i = 0; i < Array_replace.length; i++) {
                string = string.replace(new RegExp(Array_replace[i][0], 'g'), Array_replace[i][1]);
            }
            string = "<?xml version='1.0' encoding='UTF-8'?><movementversion:NotesForHaulier xmlns:movementversion='http://www.esdal.com/schemas/common/movementversion'>" + string + "</movementversion:NotesForHaulier>";
            return string;
        }

