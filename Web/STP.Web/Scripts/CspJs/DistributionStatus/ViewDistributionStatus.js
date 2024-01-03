    SelectMenu(2);
    //$('.dropdown-toggle').dropdown();

    function openFilters() {
        document.getElementById("filters").style.width = "400px";
        document.getElementById("banner").style.filter = "brightness(0.5)";
        document.getElementById("banner").style.background = "white";
        document.getElementById("navbar").style.filter = "brightness(0.5)";
        document.getElementById("navbar").style.background = "white";
        function myFunction(x) {
            if (x.matches) { // If media query matches
                document.getElementById("filters").style.width = "200px";
            }
        }
        var x = window.matchMedia("(max-width: 770px)")
        myFunction(x) // Call listener function at run time
        x.addListener(myFunction)
    }

    function closeFilters() {
        document.getElementById("filters").style.width = "0";
        document.getElementById("banner").style.filter = "unset"
        document.getElementById("navbar").style.filter = "unset";

    }

    function CheckAsHaulier(ESDALREf, trsmission_id) {
        var checkAs = "Haulier";
        startAnimation();
        $.ajax({
            type: "GET",
            url: "../Account/CheckAsSOAPoliceHaulier",
            contentType: "application/json; charset=utf-8",
            data: { ESDALREf: ESDALREf, trsmission_id: trsmission_id, checkAs: checkAs },
            datatype: "json",
            success: function (data) {
                stopAnimation();
                if (data != null) {
                    Redirect(data);
                }
            },
            error: function () {
                stopAnimation();
            }
        });
    }
    $('body').on('click', '#check-soa', function (e) {
        debugger;
        e.preventDefault();
        var ESDALReference = $(this).data('ESDALReference');
        var OrganisationId = $(this).data('OrganisationId');
        var contactId = $(this).data('contactId');
        var TransmissionId = $(this).data('TransmissionId');
        var checkAs = $(this).data('checkAs');

        CheckAsSOAPolice(ESDALReference, OrganisationId, contactId, TransmissionId, checkAs);
    });
    $('body').on('click', '#check-police', function (e) {
        debugger;
        e.preventDefault();
        var ESDALReference = $(this).data('ESDALReference');
        var OrganisationId = $(this).data('OrganisationId');
        var contactId = $(this).data('contactId');
        var TransmissionId = $(this).data('TransmissionId');
        var checkAs = $(this).data('checkAs');

        CheckAsSOAPolice(ESDALReference, OrganisationId, contactId, TransmissionId, checkAs);
    });
    $('body').on('click', '#check-haulier', function (e) {
        debugger;
        e.preventDefault();
        var ESDALReference = $(this).data('ESDALReference');
        var TransmissionId = $(this).data('TransmissionId');


        CheckAsHaulier(ESDALReference, TransmissionId);
    });
    function CheckAsSOAPolice(ESDALREf, orgid, contactid, trsmission_id, checkAs) {
        startAnimation();
        $.ajax({
            type: "GET",
            url: "../Account/CheckAsSOAPoliceHaulier",
            contentType: "application/json; charset=utf-8",
            data: { ESDALREf: ESDALREf, orgid: orgid, contactid: contactid, trsmission_id: trsmission_id, checkAs: checkAs },
            datatype: "json",
            success: function (data) {
                stopAnimation();
                if (data != null) {
                    Redirect(data);
                }
            },
            error: function () {
                stopAnimation();
            }
        });
    }

    // Attach listener function on state changes

    $(document).ready(function () {
        $("#open-filter").on('click', openFilters);
        $("#filterimage").on('click', ClearDistributionFilter);
        $("#close-filter").on('click', closeFilters);
        $("#btn_clr").on('click', ClearDistributionFilter);
if($('#hf_showalert').val() ==  "on") {
           $('#showalert').attr('checked', true);
       }
if($('#hf_movementData').val() ==  "0") {
            $('#movementData').val('@ViewBag.movementData');
       //}

       if ('@userTypeID' != 696008) {
            $('#movementData').val('@ViewBag.movementData');
        } else {
            $('#movementData').val(2);
        }
if($('#hf_DateFilter').val() ==  "true") {
           $('#chk_creation_date').attr('checked', true);
           $('#txt_start_time').attr('disabled', false);
            $('#txt_end_time').attr('disabled', false);
       }
       $("#tableheader").hide();

       $("#txt_start_time").datepicker({
           dateFormat: "dd-M-yy",
           changeYear: true,
           changeMonth: true,
           onSelect: function (selected) {
               var dt = new Date(selected);
               var endDate = $("#txt_end_time").datepicker('getDate');
               dt.setDate(dt.getDate() + 1);
               $("#txt_end_time").datepicker("option", "minDate", dt);
               if (dt > endDate) {
                   $("#txt_end_time").datepicker("setDate", dt);
               }
           }
       });
       $("#txt_end_time").datepicker({
           dateFormat: "dd-M-yy",
           changeYear: true,
           changeMonth: true,
           onSelect: function (selected) {
               var dt = new Date(selected);
               dt.setDate(dt.getDate() - 1);
               $("#txt_start_time").datepicker("option", "maxDate", dt);
           }
       });

    });
        $('#btn_search').click(function () {
        $("#tableheader").hide();
            $("#pageNum").val('1');
        $("#tableheader").css("display", "none");
        $("#rightpanel").find("#tableheader").css("display", "none");
    });
        function ClearDistributionFilter() {
        ClearFilterData();
        ResetFilterData();
        $('#filters form').submit();
        return true;
    }
        function ClearFilterData() {
        $('#showalert').prop('checked', false);
        $('#esdalData').val('');
        if ('@userTypeID' != 696008) {
            $('#movementData').val(0);
        } else {
            $('#movementData').val(2);
        }
        $('#Org_From').val('');
        $('#Org_To').val('');
        $('#txt_start_time').val('');
        $('#txt_end_time').val('');
        $('#chk_creation_date').prop('checked', false);
        $('#txt_start_time').attr('disabled', 'disabled');
        $('#txt_start_time').val('');
        $('#txt_end_time').attr('disabled', 'disabled');
        $('#txt_end_time').val('');
    }
        function ResetFilterData() {
        $.ajax({
            url: '../DistributionStatus/ClearHelpdeskFilter',
            type: 'POST',
            async: false,
            success: function (data) {
            }
        });
    }

    var sortTypeGlobal = 1;//1-desc
    var sortOrderGlobal = 1;//1-date
    function SortDistributionStatus(event, param) {
        sortOrderGlobal = param;
        sortTypeGlobal = event.classList.contains('sorting_asc') ? 1 : 0;
        $('#SortTypeValue').val(sortTypeGlobal);
        $('#SortOrderValue').val(sortOrderGlobal);
        $('#filters form').submit();
    }

        $('#chk_creation_date').click(function () {
        if ($('#chk_creation_date').is(':checked')) {
            $('#txt_start_time').attr('disabled', false);
            $('#txt_end_time').attr('disabled', false);
            $('#txt_start_time').val('@DateTime.Now.ToString("dd-MMM-yyyy")');
            $('#txt_end_time').val('@DateTime.Now.ToString("dd-MMM-yyyy")');
        } else {
            $('#txt_start_time').attr('disabled', 'disabled');
            $('#txt_start_time').val('');
            $('#txt_end_time').attr('disabled', 'disabled');
            $('#txt_end_time').val('');
            $('#spn_error').html('');
        }
    });


        if("@ViewBag.NonESDALUSER"=="True")
    {
        showWarningPopDialog('Manually added contacts cannot login to ESDAL.', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
    }

        function Redirect(data)
    {
        console.log(data.notif_id);
            if (data.userType_ID == 696007 || data.userType_ID == 696002) {
            window.location.href = '../Movements/AuthorizeMovementGeneral?Notificationid=' + data.notif_id + '&esdal_ref=' + data.ESDALREf + '&route=' + data.Item_Type + '&inboxId=' + data.Inbox_Id;
            } else if (data.userType_ID == 696001) {

            if (data.IsNotification=="true") {

                window.location.href = '../Notification/DisplayNotification?notificationId=' + data.notif_id + '&notificationCode=' + data.ESDALREf;
            } else {
                var esdalRefPro = data.ESDALREf.split('/');
                window.location.href = '../Application/ListSOMovements?revisionId=' + data.Revision_id + '&movementId=' + data.Veh_purpose + '&versionId=' + data.Version_id + '&hauliermnemonic=' + esdalRefPro[0] + '&esdalref=' + esdalRefPro[1] + '&revisionno=' + data.Revision_no + '&versionno=' + data.Version_no + '&apprevid=' + data.Revision_id + '&projecid=' + data.Project_id + '&pageflag=2';
            }
        }
    }

        function changePageSize(_this) {
            var pageSize = $(_this).val();
            $('#pageSize').val(pageSize);
            $(_this).closest('form').submit();
        }

        function LoadPageGrid(result) {
            closeFilters();
            $('#distribution-status').html($(result).find('#distribution-status').html());
        }

        function ResetDate() {
            $('#filterData').val('');
            return true;
        }

        function OpenRetransmissionPopup(htm) {
            $("#dialogue").attr('style', 'width:420px;');
            $("#dialogue").html(htm);
            removescroll();
        }

        function stopAnimationRetransmit() {
            stopAnimation();
            $("#dialogue").show();
            $("#overlay").show();
        }

        function ClosePopUp() {
            $("#dialogue").hide();
            $("#overlay").hide();
            $("#dialogue").html('');
            $("#dialogue").attr('style', '');
            addscroll();
        }

        function TransmitData(data) {

            if (data == 1) {
                ClosePopUp();
                $("#faxEmailValidationErr").removeClass("showValidationMsg");
                $("#faxEmailValidationErr").addClass("hideValidationMsg");
                $("#faxEmailValidationErr").text("");
                showWarningPopDialog('Movement document is retransmitted', 'Ok', '', 'CloseDistributionPopup', '', 1, 'info');
            }
            else if (data == 901) {
                $("#faxEmailValidationErr").removeClass("hideValidationMsg");
                $("#faxEmailValidationErr").addClass("showValidationMsg");
                $("#faxEmailValidationErr").text("Fax number is required.");
            }
            else if (data == 902) {
                $("#faxEmailValidationErr").removeClass("hideValidationMsg");
                $("#faxEmailValidationErr").addClass("showValidationMsg");
                $("#faxEmailValidationErr").text("Enter a valid 12 digit Fax number.");
            }
            else if (data == 903) {
                $("#faxEmailValidationErr").removeClass("hideValidationMsg");
                $("#faxEmailValidationErr").addClass("showValidationMsg");
                $("#faxEmailValidationErr").text("Enter a valid 12 digit Fax number.");
            }
            else if (data == 904) {
                $("#faxEmailValidationErr").removeClass("hideValidationMsg");
                $("#faxEmailValidationErr").addClass("showValidationMsg");
                $("#faxEmailValidationErr").text("Email address is required.");
            }
            else if (data == 905) {
                $("#faxEmailValidationErr").removeClass("hideValidationMsg");
                $("#faxEmailValidationErr").addClass("showValidationMsg");
                $("#faxEmailValidationErr").text("Enter a valid email address.");
            }
            else {
                ClosePopUp();
                $("#faxEmailValidationErr").removeClass("showValidationMsg");
                $("#faxEmailValidationErr").addClass("hideValidationMsg");
                $("#faxEmailValidationErr").text("");
                showWarningPopDialog('Failed to retransmit, please specify fax/email and try or contact helpdesk', 'Ok', '', 'CloseDistributionPopup', '', 1, 'warning');
            }
        }

        var retransmitEmail = '';
        var retransmitFax = '';

        function ChkEmailClick(_this)
        {
            if ($(_this).is(':checked'))
            {
                $('#txt_email').attr('disabled', false);
                $('#txt_fax').attr('disabled', true);
                //retransmitFax = $('#txt_fax').val();
                $('#txt_fax').val('');
                //$('#txt_email').val(retransmitEmail);
            }
        }

        function ChkFaxClick(_this)
        {
            if ($(_this).is(':checked'))
            {
                $('#txt_email').attr('disabled', true);
                $('#txt_fax').attr('disabled', false);
                //retransmitEmail = $('#txt_email').val();
                $('#txt_email').val('');
                //$('#txt_fax').val(retransmitFax);
            }
        }

        function PrintDocument(ESDALREf, ContactId, OrganisationId, trasmission_id,orgtype,Is_manually_added,fromOrgName)
        {
        if (ESDALREf.indexOf('#') > -1) {
            if (Is_manually_added == 1) {
                Is_manually_added = "true";
            }
            ESDALREf = ESDALREf.replace("#", "*");
            PrintNotificationDoc(ESDALREf, ContactId, OrganisationId, trasmission_id, orgtype, Is_manually_added, fromOrgName);
        }
        else
        { //start else
            $.ajax
            ({
                url: '../DistributionStatus/PrintDocument',
                type: 'post',
                async: false,
                data: { ESDALREf: ESDALREf, orgid: OrganisationId, contactid: ContactId, trsmission_id: trasmission_id, fromOrgName: fromOrgName },

                beforesend: function () {
                    startAnimation();
                },
                success: function (data)
                {
                    switch (data.DOCStatus)
                    {
                        case "NO DATA FOUND":
                            showWarningPopDialog('All details related to this application not found ', 'Ok', '', 'ViewDistReload', '', 1, 'info');
                            break;
                        case "305002"://proposed
                            var link = '../SORTApplication/ViewProposedReport?esdalRefno=' + ESDALREf + '&trans_id=' + trasmission_id + '&OrgId=' + OrganisationId + '&contactId=' + ContactId + '&flagpoint=' +1;
                            window.open(link, "_blank");
                            break;
                        case "305003"://reproposed
                            var link = '../SORTApplication/ViewReProposedReport?esdalRefno=' + ESDALREf + '&trans_id=' + trasmission_id + '&OrgId=' + OrganisationId + '&contactId=' + ContactId + '&flagpoint=' + 1;
                            window.open(link, "_blank");

                            break;
                        case "305004"://agreed
                            var link = '../SORTApplication/ViewAgreedReport?esdalRefno=' + ESDALREf + '&trans_id=' + trasmission_id + '&OrgId=' + OrganisationId + '&contactId=' + ContactId + '&DocType=agreement&flagpoint=' + 1;
                            window.open(link, "_blank");
                            break;
                        case "305005"://agreed revised
                            var link = '../SORTApplication/ViewAmendmentToAgreementReport?esdalRefno=' + ESDALREf + '&trans_id=' + trasmission_id + '&OrgId=' + OrganisationId + '&contactId=' + ContactId + '&flagpoint=' + 1;
                            window.open(link, "_blank");
                            break;
                        case "305006"://agreed recleared
                            var link = '../SORTApplication/ViewAmendmentToAgreementReport?esdalRefno=' + ESDALREf + '&trans_id=' + trasmission_id + '&OrgId=' + OrganisationId + '&contactId=' + ContactId + '&flagpoint=' + 1;
                            window.open(link, "_blank");
                            break;
                    }

                },
                error: function (xhr, textstatus, errorthrown) {

                    //location.reload();
                },
                complete: function () {
                    stopAnimation();
                }

            });
        }//end else

    }

        function PrintNotificationDoc(ESDALREf, ContactId, OrganisationId, transmission_id, orgtype, Is_manually_added, fromOrgName)
     {
        //ESDALREf = MD5EncryptDecrypt.EncryptDetails(ESDALREf);
       // ESDALREf = CryptoJS.MD5(ESDALREf);
         var link = '../DistributionStatus/ViewNotifDoc?ESDALREf=' + ESDALREf + '&transmission_id=' + transmission_id + '&OrganisationId=' + OrganisationId + '&ContactId=' + ContactId + '&orgtype=' + orgtype + '&Is_manually_added=' + Is_manually_added + '&fromOrgName=' + fromOrgName;
        window.open(link, "_blank");
    }
        function CloseDistributionPopup() {
        $('#pop-warning').hide();
        startAnimation();
        $('.pop-message').html('');
        $('.box_warningBtn1').html('');
        $('.box_warningBtn2').html('');
        ViewDistReload();
        stopAnimation();
    }

        function ViewDistReload() {
        WarningCancelBtn();
        //location.reload();
        var pgSiz = $('#pageSize').val();
        var pg = $('page').val();

        $.ajax({
            url: '../DistributionStatus/ViewDistributionStatus',
            type: 'post',
            async: false,
            data: { page: pg, pageSize: pgSiz },

            beforesend: function () {
                startAnimation();
            },

            success: function (data) {
                $('#div_tbl_grid').html($(data).find('#div_tbl_grid').html());
                scrolltotop();
            },
            error: function (xhr, textstatus, errorthrown) {

                //location.reload();
            },
            complete: function () {
                stopAnimation();
            }

        });
    }
    //#endregion
