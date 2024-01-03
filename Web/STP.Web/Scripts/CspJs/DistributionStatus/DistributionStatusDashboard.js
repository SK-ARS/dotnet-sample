        $('.dropdown-toggle').dropdown();

        function openNav() {
            document.getElementById("mySidenav").style.width = "320px";
            document.getElementById("banner").style.filter = "brightness(0.5)";
            document.getElementById("banner").style.background = "white";
            document.getElementById("navbar").style.filter = "brightness(0.5)";
            document.getElementById("navbar").style.background = "white";
            function myFunction(x) {
                if (x.matches) { // If media query matches
                    document.getElementById("mySidenav").style.width = "200px";
                    // document.getElementById("mySidenav").getElementsByClassName("sidenav").getElementByTagName('a').style.padding="10px 5px 10px 10px"
                }
            }
            var x = window.matchMedia("(max-width: 992px)")
            myFunction(x) // Call listener function at run time
            x.addListener(myFunction)

        }

        function closeNav() {
            document.getElementById("mySidenav").style.width = "0";
            document.getElementById("banner").style.filter = "unset"
            document.getElementById("navbar").style.filter = "unset";
        }
        function showuserinfo() {
            if (document.getElementById('user-info').style.display !== "none") {
                document.getElementById('user-info').style.display = "none"
            }
            else {
                document.getElementById('user-info').style.display = "block";
                document.getElementsById('userdetails').style.overFlow = "scroll";
            }
        }
        function showmorenews() {
            if (document.getElementById('more-news').style.display !== "none") {
                document.getElementById('more-news').style.display = "none"
                document.getElementById('more-info').style.display = "block"
            }
            else {
                document.getElementById('more-news').style.display = "block";
                document.getElementById('more-info').style.display = "none"
                // document.getElementsById('userdetails').style.overFlow = "scroll";
            }
        }


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
                //contentType: "application/json; charset=utf-8",
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

        function CheckAsSOAPolice(ESDALREf, orgid, contactid, trsmission_id, checkAs) {
            startAnimation();
            $.ajax({
                type: "GET",
                url: "../Account/CheckAsSOAPoliceHaulier",
                //contentType: "application/json; charset=utf-8",
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

        function SortDistributionStatus(event, param) {
            var sort_Order = param;
            var sort_Type = 1;
            if (event.classList.contains('sorting_asc')) {
                sort_Type = 1;
            }
            else if (event.classList.contains('sorting_desc')) {
                sort_Type = 3;
            }
            else if (!event.classList.contains('sorting_asc') && !event.classList.contains('sorting_desc')) {
                sort_Type = 1;
            }

            var pgSiz = $('#pageSize').val();
            var pg = $('page').val();

            $.ajax({
                url: '../DistributionStatus/ViewDistributionStatus',
                type: 'post',
                async: false,
                data: { page: pg, pageSize: pgSiz, sortType: sort_Type, sortOrder: sort_Order },

                beforesend: function () {
                    startAnimation();
                },

                success: function (data) {
                    $('#div_tbl_grid').html($(data).find('#div_tbl_grid').html());
                    scrolltotop();
                    $('.esdal-table > thead .sorting').removeClass('sorting_asc sorting_desc');

                    $(".esdal-table > thead .sorting").each(function () {
                        var item = $(this);
                        if ((sort_Type == 0 || sort_Type == 1) && item.find('span').attr('param') == sort_Order) {
                            item.addClass('sorting_desc');
                        }
                        else if (sort_Type == 3 && item.find('span').attr('param') == sort_Order) {
                            item.addClass('sorting_asc');
                        }
                    });
                },
                error: function (xhr, textstatus, errorthrown) {

                    //location.reload();
                },
                complete: function () {
                    stopAnimation();
                }

            });

            flagcheck = 1;
            $.ajax({

                url: '../Holidays/HolidayCalender',
                type: 'GET',
                data: { flag: 1, MonthYear: MonthYear, searchType: searchType, sortType: sort_Type, sortOrder: sort_Order },
                beforeSend: function () {
                    startAnimation();
                },
                success: function (result) {
                    $('#holiday-calendar').html($(result).find('#holiday-calendar').html());
                    Datestringarray = [];
                    Datestringarray1 = [];
                    var holidaystring = $(result).find('#holidaystring').val();
                    var datearray = holidaystring.split(',')
                    for (var i = 0; i < datearray.length; i++) {
                        if (datearray[i] != '') {
                            Datestringarray.push(datearray[i].replace(/\b0(?=\d)/g, ''));
                        }
                    }

                    $('.esdal-table > thead .sorting').removeClass('sorting_asc sorting_desc');

                    $(".esdal-table > thead .sorting").each(function () {
                        var item = $(this);
                        if ((sort_Type == 0 || sort_Type == 1) && item.find('span').attr('param') == sort_Order) {
                            item.addClass('sorting_desc');
                        }
                        else if (sort_Type == 3 && item.find('span').attr('param') == sort_Order) {
                            item.addClass('sorting_asc');
                        }
                    });
                },
                error: function (xhr, textStatus, errorThrown) {
                },
                complete: function () {
                    stopAnimation();
                }
            });


        }
        // Attach listener function on state changes
    //#region
    //Below script falls under retransmission business logic
    $(document).ready(function ()
    {
        $("#tableheader").css("display", "none");
        scrolltotop();
        window.history.forward(-1);
        $('#pageSizeSelect').val('@ViewBag.pageSize');

        var url = '../DistributionStatus/ViewDistribFilter';
        loadLeftSearchPanel(url);

        //selected menu
        selectedmenu('Distribution');
if($('#hf_Isdeleted').val() ==  "True")
        {
            showWarningPopDialog('Respective application has been deleted.', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
        }
if($('#hf_ErrorMessage').val() ==  "Generate Error Occurred.")
        {
            showWarningPopDialog('Error in generating document.', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
        }
if($('#hf_ErrorMessage').val() ==  "Doc Error Occurred.")
        {
            showWarningPopDialog('Error Document not found.', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
        }
if($('#hf_ErrorMessage').val() ==  "Error Occurred.")
        {
            showWarningPopDialog('Error occured during Login.', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
        }

    });

    if("@ViewBag.NonESDALUSER"=="True")
    {
        showWarningPopDialog('Manually added contacts cannot login to ESDAL.', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
    }

        function Redirect(data) {
            console.log(data.notif_id);
            if (data.userType_ID == 696007 || data.userType_ID == 696002) {
                window.location.href = '../Movements/AuthorizeMovementGeneral' + EncodedQueryString('Notificationid=' + data.notif_id + '&esdal_ref=' + data.ESDALREf + '&route=' + data.Item_Type + '&inboxId=' + data.Inbox_Id);
            } else if (data.userType_ID == 696001) {

                if (data.IsNotification == "true") {
                    window.location.href = '../Notification/DisplayNotification' + EncodedQueryString('notificationId=' + data.notif_id + '&notificationCode=' + data.ESDALREf);
                } else {
                    var esdalRefPro = data.ESDALREf.split('/');
                    window.location.href = '../Application/ListSOMovements' + EncodedQueryString('revisionId=' + data.Revision_id + '&movementId=' + data.Veh_purpose + '&versionId=' + data.Version_id + '&hauliermnemonic=' + esdalRefPro[0] + '&esdalref=' + esdalRefPro[1] + '&revisionno=' + data.Revision_no + '&versionno=' + data.Version_no + '&apprevid=' + data.Revision_id + '&projecid=' + data.Project_id + '&pageflag=2');
                }
            }
        }

        function changePageSize(_this) {
            var pageSize = $(_this).val();
            $('#pageSize').val(pageSize);
            $(_this).closest('form').submit();
        }

        function LoadPageGrid(result) {
            $('#div_tbl_grid').html($(result).find('#div_tbl_grid').html());
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

        $('body').on('click', '#print-doc', function (e) {
            
            e.preventDefault();
            var ESDALReference = $(this).data('ESDALReference');
            var ContactId = $(this).data('ContactId');
            var OrganisationId = $(this).data('OrganisationId');
            var TransmissionId = $(this).data('TransmissionId');
            var OrganisationTypeName = $(this).data('OrganisationTypeName');
            var IsManullyAdded = $(this).data('IsManullyAdded');
            var FromOrganisationName = $(this).data('FromOrganisationName');
            PrintDocument(ESDALReference, ContactId, OrganisationId, TransmissionId, OrganisationTypeName, IsManullyAdded, FromOrganisationName);
        });
        $('body').on('click', '#check-soa', function (e) {
           
            e.preventDefault();
            var ESDALReference = $(this).data('ESDALReference');
            var OrganisationId = $(this).data('OrganisationId');
            var contactId = $(this).data('contactId');
            var TransmissionId = $(this).data('TransmissionId');
            var checkAs = $(this).data('checkAs');
             
            CheckAsSOAPolice(ESDALReference, OrganisationId, contactId, TransmissionId, checkAs);
        });
        $('body').on('click', '#check-police', function (e) {
           
            e.preventDefault();
            var ESDALReference = $(this).data('ESDALReference');
            var OrganisationId = $(this).data('OrganisationId');
            var contactId = $(this).data('contactId');
            var TransmissionId = $(this).data('TransmissionId');
            var checkAs = $(this).data('checkAs');

            CheckAsSOAPolice(ESDALReference, OrganisationId, contactId, TransmissionId, checkAs);
        });
        $('body').on('click', '#check-haulier', function (e) {
           
            e.preventDefault();
            var ESDALReference = $(this).data('ESDALReference');
            var TransmissionId = $(this).data('TransmissionId');
            

            CheckAsHaulier(ESDALReference, TransmissionId);
        });
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
        $('#retransmitPopup').modal('hide');
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
        // //location.reload();
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
