                        $("#importButton").show();
    var getValue = 0;
    var routeStatus = 0;
    var mailedCollab = 0;
    $(document).ready(function () {

        $('#updateExternalNote').hide();
        $('#ValidAgreement').hide();
        $('#ValidLatest').hide();
        $('#ValidStatus').hide();

        //Resize_PopUp(700);
        $('#span-close').click(function () {
            $('#overlay').hide();
            addscroll();
            resetdialogue();
        });
        //set status
        ;
        if ($('#COL_STATUS').val() > 0) {
            var num = $('#COL_STATUS').val();
            num=Math.round(num * 100) / 100;


            $("input[name=statusradiogroup][value="+ num +"]").attr('checked', 'checked');
        }
        else { $("input[name=statusradiogroup][value=" + 313007 + "]").attr('checked', 'checked'); }

        //----NEN portion-------
        $("#dropNEN").change(function () {
            getValue = $(this).val();
        });
if($('#hf_User_id').val() ==  '') {
            $("#dropNEN").val('@ViewBag.User_id');
            getValue = $('#dropNEN').val();
        }
        if ($("#Radio3").prop("checked")) {
            $('#tr_showscrutiny').show();
            routeStatus = 911009;
            //$('#SendMailBy').hide();
            //$('#div_emaildescp').hide();
        }
        if ($("#Radio1").prop("checked")) {
            $('#tr_showscrutiny').hide();
            routeStatus = 911007;
            //$('#SendMailBy').show();
            //$('#div_emaildescp').show();
        }
        if ($("#Radio2").prop("checked")) {
            $('#tr_showscrutiny').hide();
            routeStatus = 911008;
            //$('#SendMailBy').show();
            //$('#div_emaildescp').show();
        }
        $('body').on('click', '#SendMailBy', SendEmail);
        $('body').on('click', '#btncancel', closeViewCollab); 
        //----------------------
    });


    $("input[name=statusradiogroup]").change(function () {
        if ($(this).val() == 327003) {
            $('#tr_showscrutiny').show();
            //$('#SendMailBy').hide();
            //$('#div_emaildescp').hide();
if($('#hf_User_id').val() ==  '') {
                $("#dropNEN").val('@ViewBag.User_id');
            }
            routeStatus = 911009;
        } else {
            $('#tr_showscrutiny').hide();
            //$('#SendMailBy').show();
            //$('#div_emaildescp').show();
            if ($(this).val() == 327001) {
                routeStatus = 911007;
            } else {
                routeStatus = 911008;
            }
        }
    });

    //function for closing popup
    function closemodal() {
        $('#overlay').hide();
        addscroll();
        resetdialogue();
    }
    function UpdateStatus() {
        $('#updateExternalNote').hide();
        $('#ValidAgreement').hide();
        $('#ValidLatest').hide();
        $('#ValidStatus').hide();
        var status = '';
        ;
        status = $('input[name=statusradiogroup]:radio:checked').val();
        //this part is commented by ajit for Removing constraint that forces SOA/,police to only respond to the latest version
        if ("@ViewBag.latestversion" == "0") {

            var EsdalReference = $('#ESDAL_REF_NUMBER').val();
            if (EsdalReference.indexOf('#') > -1) {
                $('#ValidLatest').show();
            }
        }

        if ($('#Statushidden').val() == status && $('#DescriptionExternal').val().trim() == '' && $('#dropNEN').val() == $('#hdnAssignUser').val()) {
            $('#updateExternalNote').show();
            return false;
        }

        //For NEN processing collaboration---------
        if ($('#Statushidden').val() == status && $('#DescriptionExternal').val().trim() == '' && ($('#hNEN_RouteStatus').val() == '911001' || $('#hNEN_RouteStatus').val() == '911002' || $('#hNEN_RouteStatus').val() == '911003' || $('#hNEN_RouteStatus').val() == '911004' || $('#hNEN_RouteStatus').val() == '911005' || $('#hNEN_RouteStatus').val() == '911010' || $('#hNEN_RouteStatus').val() == '911011')) {
            $('#updateExternalNote').show();
            return false;
        }
        //-----------------------------------------
        if (typeof (status) === "undefined") {
            $('#ValidStatus').show();
            return false;
        }

        var to_User = '';
        if ($('#dropNEN').val() != "--Select user--" && $('#dropNEN').val() != '' && $('#dropNEN').val() != undefined) {
            var dropUserNEN = document.getElementById('dropNEN');
            to_User = dropUserNEN.options[dropUserNEN.selectedIndex].text;
        }
        //----------------------
        var wipNENProcess  = $('#hf_WipNENCollab' != null ? '@ViewBag.WipNENCollab').val(); 
        var MailHaulier = $('#NoteToLibrary').is(":checked");

        var paramList = {
            DocumentId: $('#DOCUMENT_ID').val(),
            InboxId: $('#INBOX_ID').val(),
            STATUS: status,
            ContactId: $('#CONTACT_ID').val(),
            EsdalReference: $('#ESDAL_REF_NUMBER').val(),
            NOTES: $('#DescriptionExternal').val(),
            UserId: getValue,
            NOTIFICATION_ID: '@ViewBag.Notificationid',
            NenId: $('#HFNEN_ID').val(),
            RouteStatus: routeStatus,
            NenProcess: 0,
            WIPNENProcess: wipNENProcess,
            MailedCollab: mailedCollab,
            HaulierName: to_User,
            NenFlag: MailHaulier
        }
        startAnimation();

        $.ajax({
            async: false,
            type: "POST",
            url: '@Url.Action("ManageMovementStatus", "Movements")',
            dataType: "json",
            //contentType: "application/json; charset=utf-8",
            data: JSON.stringify(paramList),
            processdata: true,
            success: function (result) {

                if (result == 'true') {
                    ShowSuccessModalPopup("Status Updated", "CloseSuccessReload()");
                    $('#exampleModalCenter2').hide();
                    $("#overlay").hide();
                }
                else {
             
                    ShowErrorPopup("Notes already exists in the library","CloseErrorPopupRef");
                    
                }

            },
            error: function () {
            },
            complete: function () {
                stopAnimation();
                removescroll();
                $('.loading').hide();
            }
        });

    }

    function CloseSuccessReload() {
        $("#popupDialogue").hide();
        CloseSuccessModalPopup();
        location.reload();
    }

    function loadPreviousLocation() {
        var encryptNotificationId = "@TempData["EncryptNotiId"]";
        var encryptRoute = "@TempData["EncryptRoute"]";

        var encryptEsdalRef = "@TempData["EncryptEsdalRef"]";
        var encryptInBoxId = "@TempData["InBoxId"]";

        var link = "../Movements/AuthorizeMovementGeneral?Notificationid=" + encryptNotificationId + "&esdal_ref=" + encryptEsdalRef + "&route=" + encryptRoute + "&inboxId=" + encryptInBoxId + "";
        //RM#3919 - start
        $('#pop-warning').hide();
        $('#span-close').click();
        $("#overlay").show();
        $('.loading').show();
        //RM#3919 - end
        window.open(link, '_self', false);

    }
    function RedirectWhenExpire() {
        window.location.href = "../Account/Login";
    }

    function Reload() {
        WarningCancelBtn();
        $("#overlay").hide();
    }

    function SendEmail() {
        if ($('#MailHaulier').is(":checked")) {
        var collaborationNote = $('#DescriptionExternal').val();

        collaborationNote = collaborationNote.replace(/#/gi, "%23");
        collaborationNote = collaborationNote.replace(/&/gi, "%26");
        collaborationNote = collaborationNote.replace(/ /gi, "%20");

        var restrictCollaborationNote = collaborationNote.substring(0, 1800);

        if (collaborationNote.length > 1800) {
            restrictCollaborationNote = restrictCollaborationNote + "...";
        }

        var emailAddress = "@eMail";
        var subject  = $('#hf_Subject').val(); 

        if (emailAddress != null) {
            window.location.href = "mailto:" + emailAddress + "?subject=" + subject + "&body=" + restrictCollaborationNote + "";
            mailedCollab = 3;
            UpdateStatus();
            }
        }
        else {

            UpdateStatus();

        }
    }

    function closeViewCollab() {
        $(".modal-backdrop").removeClass("show");
        $(".modal-backdrop").removeClass("modal-backdrop");
        $("#popupDialogue").hide();
        $('#viewCollabModal').html('');
        $('#viewCollabModal').modal('hide')
        $("#popupDialogue").css("display", "none");
        $(".modal-backdrop").removeClass("show");
        $(".modal-backdrop").removeClass("modal-backdrop");
        DisplayCollabStatus();
        $("#overlay").css("display", "none");
    }

    function ImportFromLibrary(doc_id) {
        randomNumber = Math.random();
        startAnimation();
        $("#overlay").show();
       removescroll();
        $("#dialogue").load('../Notification/CollabHistoryPopList?DocumentId=' + 1011210465 + "&randomNumber=" + randomNumber + "&SORTCollab=" + true, function () {
            $('#CollabHistoryDiv').modal({ keyboard: false, backdrop: 'static' });

            $('#CollabHistoryDiv').modal('show');
            stopAnimation();
            $("#dialogue").show();
            $("#popupDialogue").hide();
            $("#overlay").show();

        });

        //resetdialogue();
    }

    function SaveToLibrary() {
        var Notes = $("#DescriptionExternal").val();

            $.ajax({
            async: false,
            type: "GET",
            url: '@Url.Action("SaveToLibrary", "Movements")',
            dataType: "json",
            //contentType: "application/json; charset=utf-8",
            data: { Notes: Notes, userSchema: "PORTAL" },

            processdata: true,
            success: function (result) {

                if (result == 'true') {
                    ShowSuccessModalPopup("Note Saved To Library", "CloseSuccessReload()");
                }
                else {
                    ShowErrorPopup("Failed");

                }

            },
            error: function () {
            },
            complete: function () {
                removescroll();
                $('.loading').hide();
            }
        });
    }

    function ImportAssessmentResult(analysisId) {
        var desc = document.getElementById("DescriptionExternal").value;
        $("#DescriptionExternal").load("../RouteAssessment/GetAssessmentResult?analysisID=" + analysisId + "&userSchema=" + "PORTAL", function (data) {
            //$('#contactDetails').modal('show');
            if (data == "") {
                $('#assessmentWarning').text("Assessment results are not available");
            }
            else {
                if (desc == "") {
                    document.getElementById("DescriptionExternal").value = data;
                }
                else {
                    desc += "\n" + data;
                    document.getElementById("DescriptionExternal").value = desc;
                }
            }


            stopAnimation();
            $("#overlay").show();
        });
        //$.ajax
        //    ({
        //        type: "POST",
        //        url: "../RouteAssessment/GetAssessmentResult",
        //        data: { analysisID: analysisId, userSchema: "PORTAL" },
        //        beforeSend: function () {
        //            //startAnimation();
        //        },
        //        success: function (data) {
        //            var desc = document.getElementById("DescriptionExternal").value;
        //            if (desc == "") {
        //                document.getElementById("DescriptionExternal").value = data;
        //            }
        //            else {
        //                desc += "\n" + data;
        //                document.getElementById("DescriptionExternal").value = desc;
        //            }

        //        },
        //        complete: function () {
        //            stopAnimation();
        //        }

        //    });

    }
    $('body').on('click', '#btn_importfromassessment', function (e) {
        var analysisId = $(this).data("analysisid");
      
        ImportAssessmentResult(analysisId);
        return false;
    });
    $('body').on('click', '#btnImportfrmLibrary', function (e) {
        var doc_id = $(this).data("doc_id");

        ImportFromLibrary(doc_id);
        return false;
    });
