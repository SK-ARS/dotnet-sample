        $(document).ready(function () {

            $(".ViewCollaboration").on('click', ViewCollaboration);
            $(".ViewContactedPartiesByNotification").on('click', ViewContactedPartiesByNotification);
            $(".ViewContactedPartiesByProposal").on('click', ViewContactedPartiesByProposal);
            $(".ViewIndemnityConfirmation").on('click', ViewIndemnityConfirmation);
            $(".ViewContactedParties").on('click', ViewContactedParties);
            $("#printbutton").on('click', printOptions);
            $(".PrintReport").on('click', PrintReport);
            $(".PrintReducedReport").on('click', PrintReducedReport);
            $(".viewVehicleSummary").on('click', viewVehicleSummary);
            $(".vehicleSummary").on('click', vehicleSummary);
            $(".routeOverview").on('click', routeOverview);
            $(".routeRoads").on('click', routeRoads);
            $(".affectedstructure").on('click', affectedstructure);
            $(".drivingInstruction").on('click', drivingInstruction);
            $(".notesFromHaulier").on('click', notesFromHaulier);
            $(".relatedCom").on('click', relatedCom);
            $(".RelatedCommunication").on('click', RelatedCommunication);
            $(".notesOnEscort").on('click', notesOnEscort);
            $(".internalNotes").on('click', internalNotes);
            $("#SaveNotes").on('click', SaveNotes);
            $(".ShowContactList").on('click', ShowContactList);
            $(".predefinedCautions").on('click', predefinedCautions);
            $(".reviewProcHighway").on('click', reviewProcHighway);
            $(".showaudit").on('click', showaudit);
            $(".openDispensationList").on('click', openDispensationList);
            $(".closeAuthorizeMovFilters").on('click', closeAuthorizeMovFilters);
            $(".SaveEscortNotes").on('click', SaveEscortNotes);

        });
    




        var sortFlag = '@(ViewBag.SortMovement != null ? true : false)';
        $('#HFSortFlag').val(sortFlag);
        function UnDisplayPrintOptions() {
        if (document.getElementById('printOptions').style.display == "block") {
                document.getElementById('printOptions').style.display = "none"
                $("#MarginTop").css('margin-top', 0);
                $("#printOptions").css("z-index", "0");
            }
        }
        function ToggleAllAffected() {
            if ($("#toggleAllAffected").prop('checked') == true) {
                $('#toggleAllAffected').val(true);
                document.getElementById('All').checked = true;
                document.getElementById('Affected').checked = false;
                $("#All").change();
                $("#Affected").change();

            }
            else if ($("#toggleAllAffected").prop('checked') == false) {
                $('#toggleAllAffected').val(false);
                document.getElementById('Affected').checked = true;
                document.getElementById('All').checked = false;
                $("#All").change();
                $("#Affected").change();
            }
        }
        if ($('#printoptions').hasClass('clicked')) {
            UnDisplayPrintOptions();
        }
        $("#movement-details").click(function () {
            UnDisplayPrintOptions();
        });
        $("#listitem").click(function () {
            UnDisplayPrintOptions();
        });
        $("#MarginTop").click(function () {
            UnDisplayPrintOptions();
        });
        $("#navbar").click(function () {
            UnDisplayPrintOptions();
        });
        $("#banner").click(function (event) {
            if (event.target.id == "banner-container" || event.target.id == "printButtonDiv" || event.target.id == "helpdeskDelegation" || event.target.id == "container-sub" || event.target.id == "movement-map") {
               UnDisplayPrintOptions();
            }

        });
        SelectMenu(2);
        var IS_NEN = $('#HFNEN_ID').val() != null ? $('#HFNEN_ID').val() : 0;
        var V_INBOX_ID = $('#HdnInboxID').val();

        var vehicleInitialLoad = 0;
        var ViewContactlist=0;
        var viewcontactdetailsInitialLoad = 0;
        $(document).ready(function () {
            $("#toggleAllAffected").prop('checked', true);
            ToggleAllAffected();
            vehicleInitialLoad = 1;
            ViewContactlist = 1;
            viewcontactdetailsInitialLoad = 1;
            var Helpdesk_redirect  = $('#hf_Helpdesk_redirect').val(); 
            if (Helpdesk_redirect == "true") {
                $("#SOA").css("display", "none");
                $("#police").css("display", "none");
                $("#user-info-filter").css("display", "none");
            }
            if ($("#userid").val() == 696002) {
                $("#ShowAllAffectStruct").val(true);
            }
        });

        // showing filter-settings

        //  google-map-setting-start
        //function initialize() {
        //    var myLatlng = new google.maps.LatLng(51.508742, -0.120850);
        //    var mapProp = {
        //        center: myLatlng,
        //        zoom: 5,
        //        mapTypeId: google.maps.MapTypeId.ROADMAP

        //    };
        //    var map = new google.maps.Map(document.getElementById("googleMap"), mapProp);
        //    document.getElementById('lat').value = 51.508742
        //    document.getElementById('lng').value = -0.120850
        //}
        //google.maps.event.addDomListener(window, 'load', initialize);
        //  google-map-setting-end

        function ViewIndemnityConfirmation(data) {
            var ID = data.currentTarget.attributes.NotificationId.value;
            var StartTime = data.currentTarget.attributes.MoveStartDate.value;
            var EndTime = data.currentTarget.attributes.MoveEndDate.value;
            startAnimation();
            var random = Math.random();
            //$("#").load("../Application/ViewIndemnityConfirmation?NotificationId=" + ID + "&random=" + random);


            var indemnity = {};

            indemnity.NotificationId = ID;
            //indemnity.OnBehalfOf = $('#ActingOnBehalfOf').val();
            indemnity.FirstMoveDate = StartTime;
            indemnity.LastMoveDate = EndTime;

            $("#dialogue").load("../Application/ViewIndemnityConfirmation", { indemnityConfirmation: indemnity }, function () {/*?NotificationId = " + ID + " & random=" + random + "*/
                $('#contactDetails').modal({ keyboard: false, backdrop: 'static' });
                $('#contactDetails').modal('show');
                stopAnimation();
            });

            //$("#dialogue").load("../Application/ViewIndemnityConfirmation?NotificationId=" + ID + "&random=" + random + "", function () {
            //    $('#contactDetails').modal('show');
            //    stopAnimation();
            //});
        }



        function ViewCollaboration(id, inboxId, analysisId) {
        var id = data.currentTarget.attributes.DocumentId.value;
        var inboxId = data.currentTarget.attributes.InboxId.value;
        var analysisId = data.currentTarget.attributes.AnalysisId.value;
        var randomNumber = Math.random();

        var loggedInContactId = "@contactId";
        var loggedInOrgId = "@organisationId";
          var IS_MOST_RECENT = $('#IS_MOST_RECENT').val();

        //RM#3919 - add routeOriginal parameter;
        var NextNoLongerAffected  = $('#hf_NextNoLongerAffected').val(); 
          startAnimation();

            $("#popupDialogue").load("../Movements/ViewCollaborationStatusAndNotes?random=" + randomNumber + "&Notificationid=@ViewBag.Notificationid&documentid=" + id + "&InboxId=" + inboxId + "&analysisId=" + analysisId + "&email=" + $('#EMail').val().replace(/ /g, '+') + "&esdalRef=" + $('#ESDAL_Reference').val() + "&contactId=" + loggedInContactId + "&route=@routeType.Replace(" ", "")&IS_MOST_RECENT=" + IS_MOST_RECENT + "&routeOriginal=@routeType.Replace(" ", "_")" + "&NextNoLongerAffected=" + NextNoLongerAffected, function () {
                stopAnimation();
                $('#viewCollabModal').modal({ keyboard: false, backdrop: 'static' });
                $("#viewCollabModal").modal('show');
                $("#overlay").css("display", "block");
                $("#popupDialogue").css("display", "block");
          });

    }

        function viewVehicleSummary() {
            if (document.getElementById('vehiclesummary').style.display !== "none") {
                document.getElementById('vehiclesummary').style.display = "none"
                document.getElementById('up-chevlon6').style.display = "none"
                document.getElementById('down-chevlon6').style.display = "block"
            }
            else {
                document.getElementById('vehiclesummary').style.display = "block"
                document.getElementById('up-chevlon6').style.display = "block"
                document.getElementById('down-chevlon6').style.display = "none"
            }
        }

        // view vehicle configuration summary
        function vehicleSummary(data) {
            var id = data.currentTarget.attributes.VehicleId.value;
            var classCode = data.currentTarget.attributes.VehicleClassification.value;
            startAnimation();
            var vehicleId = Math.round(id);
            var IsLoaded = $("#Vehicle_" + vehicleId).val();
            if (IsLoaded == "true") {
                stopAnimation();
                if ($('#vehicleconfig_' + vehicleId).css('display') !== 'none') {
                    $("#vehicleconfig_" + vehicleId).css("display", "none");
                    $("#chevlon-up-icon_" + vehicleId).css("display", "none");
                    $("#chevlon-down-icon_" + vehicleId).css("display", "block");
                }
                else {
                    $("#vehicleconfig_" + vehicleId).css("display", "block");
                    $("#chevlon-up-icon_" + vehicleId).css("display", "block");
                    $("#chevlon-down-icon_" + vehicleId).css("display", "none");
                }
            }
            else {
                //stopAnimation();
                viewVehicleDetails(id, classCode);
            }
        }


        function ShowContactList(id) {
            var id = data.currentTarget.attributes.AnalysisId.value;
            if (ViewContactlist == 1) {
                ContactList(id);
            }
            else {
                if ($('#contact-list').css('display') !== 'none') {
                    $("#contact-list").css("display", "none");
                    $("#down-chevlon-icon2").css("display", "block");
                    $("#up-chevlon-icon2").css("display", "none");
                }
                else {
                    $("#contact-list").css("display", "block");
                    $("#down-chevlon-icon2").css("display", "none");
                    $("#up-chevlon-icon2").css("display", "block");
                }
            }
        }

        function ContactList(ID, sortOrder = 1, sortType = 0,pageNum=1,pageSize=10) {
            var random = Math.random();
            var notiId  = $('#hf_Notificationid').val(); 

            var routeDetails = '@routeType';
            
            var notificationId = $('#NOTIFICATION_ID').val();
            var esdalRefNumber = $('#ESDAL_Reference').val();
            startAnimation();

            //resetdialogue();
            //RM#3919 - add toLowerCase()
            routeDetails = routeDetails.toLowerCase().replace("amendment to agreement", "amendment");
            routeDetails = routeDetails.toLowerCase().replace("no longer affected", "nolongeraffected");
            routeDetails = routeDetails.toLowerCase().replace("ne agreed notification", "notification");//Added For NEN project
            routeDetails = routeDetails.toLowerCase().replace("ne notification", "notification");//Added For NEN project
            routeDetails = routeDetails.toLowerCase().replace("ne renotification", "notification");//Added For NEN project
            routeDetails = routeDetails.toLowerCase().replace("ne agreed renotification", "notification");//Added For NEN project

            $.ajax({
                url: '../Contact/AuthoriseMovementShowContactList',
                type: 'POST',
                cache: false,
                async: false,
                beforeSend: function () {
                    startAnimation();
                },
                data: {
                    analysisID: ID, NotificationID: notificationId, ESDALRefNumber: esdalRefNumber, Type: routeDetails, random: random,
                    sortOrder: sortOrder, sortType: sortType,page:pageNum,pageSize:pageSize
                },
                success: function (response) {
                    stopAnimation();
                    var result = $(response).find('div#contact-list').html();
                    $('div#contact-list').html(result);
                    ViewContactlist = 0;
                    $("#contact-list").css("display", "block");
                    $("#down-chevlon-icon2").css("display", "none");
                    $("#up-chevlon-icon2").css("display", "block");
                }
            });
           removeHrefContactLinks();
            PaginateListContact();
        }

        function ContactSorting(event, param) {
            debugger
            sortOrder = param;
            sortType = event.classList.contains('sorting_asc') ? 1 : 0;
            var hAnalysisId = $('#hAnalysisId').val();
            pageNum = pageNum;
            pageSize = pageSize;
            var pageSize = $('#pageSizeVal').val();
            var pageNum = $('#pageNum').val()
            ContactList(hAnalysisId, sortOrder, sortType,pageNum,pageSize);
        }


        // view vehicle configuration summary
        function viewVehicleDetails(id, classCode) {
            //startAnimation();
            $.ajax({
                url: '../VehicleConfig/ViewConfigDetails',
                type: 'POST',
                cache: false,
                async: false,
                beforeSend: function () {
                    startAnimation();
                },
                data: {
                    vehicleID: id, movementId: 0, isRoute: true, flag: "VR1", isPolice: true, isNotif : @(Model.NotificationId>0? "true":"false")
                },
                success: function (response) {
                    var result = $(response).find('div#vehicleconfig').html();
                    $('div#vehicleconfig_' + id).html(result);
                    vehicleInitialLoad = 0;
                    $("#Vehicle_" + id).val("true");
                    $("#vehicleconfig_" + id).css("display", "block");
                    $("#chevlon-up-icon_" + id).css("display", "block");
                    $("#chevlon-down-icon_" + id).css("display", "none");
                    stopAnimation();
                }
            });
        }


        // View Contacted Parties By Notification
        function ViewContactedPartiesByNotification(data) {
            var id = data.currentTarget.attributes.NotificationId.value;
            startAnimation();

            var isNEN = 0;
            if ($('#HFNEN_ID').val() != 0) { isNEN = 1; }
            if (viewcontactdetailsInitialLoad == 1) {
                $("#popupDialogue").load("../Application/GetHaulierContactDetails", { notificationNumber: id, isNEN: isNEN }, function () {
                    stopAnimation();
                    $('#contactDetails').modal({ keyboard: false, backdrop: 'static' });

                    $("#popupDialogue").show();
                    $("#overlay").show();
                    $('#contactDetails').modal('show');
                });
            }
            else {
                if (document.getElementById('viewcontactdetails').style.display !== "none") {
                    document.getElementById('viewcontactdetails').style.display = "none"
                    document.getElementById('down-chevlon-icon2').style.display = "none"
                    document.getElementById('up-chevlon-icon2').style.display = "block"
                }
                else {
                    document.getElementById('viewcontactdetails').style.display = "block"
                    document.getElementById('down-chevlon-icon2').style.display = "block"
                    document.getElementById('up-chevlon-icon2').style.display = "none"
                }
            }
        }

        function ViewContactedPartiesByProposal(DocumentNumber, ModelRoute) {
            //removescroll();
            var DocumentNumber = data.currentTarget.attributes.DocumentId.value;
            var ModelRoute = data.currentTarget.attributes.Route.value;
            var random = Math.random();
            startAnimation();

            $("#dialogue").load("../Application/GetHaulierContactDetailsForProposal?documentNumber=" + DocumentNumber + "&modelRoute=" + encodeURIComponent(ModelRoute), function () {
                $('#contactDetails').modal({ keyboard: false, backdrop: 'static' });
                $("#dialogue").css("display", "block");
                $('#contactDetails').modal('show');

              stopAnimation();
              $("#overlay").css("display", "block");
          });


        }

        function ViewContactedParties(ID) {
            var ID = data.currentTarget.attributes.HAContactId.value;      
            removescroll();
            var random = Math.random();



            $("#dialogue").load("../Application/ViewContactDetails?ContactId=" + ID + "&random=" + random, function () {
                $('#contactDetails').modal({ keyboard: false, backdrop: 'static' });
                $("#dialogue").css("display", "block");
                $('#contactDetails').modal('show');

                stopAnimation();
                $("#overlay").css("display", "block");
            });

        }

        // view vehicle configuration summary

        // show print options
        //function printOptions() {
        //    if ($('#printOptions').css('display') == 'block') {
        //        $("#printOptions").css("display", "none");
        //    }
        //    else {
        //        $("#printOptions").css("display", "block");
        //    }
        //}
        function notesOnEscort() {
            if (document.getElementById('notesOnEscort').style.display !== "none") {
                document.getElementById('notesOnEscort').style.display = "none"
                document.getElementById('up-chevlon1').style.display = "none"
                document.getElementById('down-chevlon1').style.display = "block"
            }
            else {
                $('#updateEscortNotes').hide();
                document.getElementById('notesOnEscort').style.display = "block"
                document.getElementById('up-chevlon1').style.display = "block"
                document.getElementById('down-chevlon1').style.display = "none"
            }
        }
        function internalNotes() {
            if (document.getElementById('internalNotes').style.display !== "none") {
                document.getElementById('internalNotes').style.display = "none"
                document.getElementById('up-chevlon2').style.display = "none"
                document.getElementById('down-chevlon2').style.display = "block"
            }
            else {
                $('#updatenotes').hide();
                document.getElementById('internalNotes').style.display = "block"
                document.getElementById('up-chevlon2').style.display = "block"
                document.getElementById('down-chevlon2').style.display = "none"
            }
        }
        function notesFromHaulier() {
            if (document.getElementById('notesFromHaulier').style.display !== "none") {
                document.getElementById('notesFromHaulier').style.display = "none"
                document.getElementById('up-chevlon3').style.display = "none"
                document.getElementById('down-chevlon3').style.display = "block"
            }
            else {
                document.getElementById('notesFromHaulier').style.display = "block"
                document.getElementById('up-chevlon3').style.display = "block"
                document.getElementById('down-chevlon3').style.display = "none"
            }
            stopAnimation();
        }
        function predefinedCautions() {
            if (document.getElementById('predefinedCautions').style.display !== "none") {
                document.getElementById('predefinedCautions').style.display = "none"
                document.getElementById('up-chevlon4').style.display = "none"
                document.getElementById('down-chevlon4').style.display = "block"
            }
            else {
                document.getElementById('predefinedCautions').style.display = "block"
                document.getElementById('up-chevlon4').style.display = "block"
                document.getElementById('down-chevlon4').style.display = "none"
            }
        }
        function relatedCom() {

            if (document.getElementById('relatedCom').style.display !== "none") {
                document.getElementById('relatedCom').style.display = "none"
                document.getElementById('up-chevlon5').style.display = "none"
                document.getElementById('down-chevlon5').style.display = "block"
            }
            else {
                document.getElementById('relatedCom').style.display = "block"
                document.getElementById('up-chevlon5').style.display = "block"
                document.getElementById('down-chevlon5').style.display = "none"
            }
        }
        function SaveEscortNotes() {
        $('#validateEscortNotes').hide();
        $('#updateEscortNotes').hide();
        var inboxId  = $('#hf_InboxId').val(); 
        var esdalRefNo = $('#ESDAL_Reference').val();
            if ($('#DescriptionOnEscort').val().trim() == '') {
                $('#validateEscortNotes').show();
            return false;
            }
            startAnimation();
        var paramList = {
            NotificationId: $('#NOTIFICATION_ID').val(),
            RevisionId: $('#REVISION_ID').val(),
            NotesOnEscort: $('#DescriptionOnEscort').val(),
            InboxId: inboxId,
            NotificationCode: esdalRefNo
        }

        $.ajax({
            async: false,
            type: "POST",
            url: '@Url.Action("ManageNotesOnEscort", "Movements")',
            dataType: "json",
            //contentType: "application/json; charset=utf-8",
            data: JSON.stringify(paramList),
            processdata: true,
            success: function (result) {
                if (result == 'true') {
                    $('#updatedEscortNotes').html('');
                    $('#updatedEscortNotes').html($('#DescriptionOnEscort').val());
                    $('#updateEscortNotes').show();
                }
                else if (result == 'expire') {
                    RedirectWhenExpire();
                }
                else if (result == 'false') {
                    RedirectWhenExpire();
                }
            },
            error: function () {
            },
            complete: function () {
                stopAnimation();
            }
        });

        }
        function SaveNotes() {

        $('#validateInternalNotes').hide();
        $('#updatenotes').hide();
        var inboxId  = $('#hf_InboxId').val(); 
        var esdalRefNo = $('#ESDAL_Reference').val();
        if ($('#Description').val().trim() == '') {
            $('#validateInternalNotes').show();
            return false;
        }

            startAnimation();

        var paramList = {
            DocumentId: $('#DOCUMENT_ID').val(),
            RevisionId: $('#REVISION_ID').val(),
            InternalNotes: $('#Description').val(),
            InboxId: inboxId,
            NotificationCode: esdalRefNo
        }

        $.ajax({
            async: false,
            type: "POST",
            url: '@Url.Action("ManageInternalNotes", "Movements")',
            dataType: "json",
            //contentType: "application/json; charset=utf-8",
            data: JSON.stringify(paramList),
            processdata: true,
            success: function (result) {

                if (result == 'true') {
                    $('#updatenotes').show();
                }
                else if (result == 'expire') {
                    RedirectWhenExpire();
                }
                else if (result == 'false') {
                    RedirectWhenExpire();
                }
            },
            error: function () {
            },
            complete: function () {
                stopAnimation();
            }
        });
        }

        function ViewContactDetails(ContactId, isClosed) {
            ;
            var random = Math.random();
            $("#overlay").show();
            startAnimation();
            $("#dialogue").load('../Application/ViewContactDetails?ContactId=' + ContactId + "&EsdalRefNo=&structOwner=&isClosed=true&random=" + random, function () {
                $('#contactDetails').modal({ keyboard: false, backdrop: 'static' });
                $('#contactDetails').modal('show');
                stopAnimation();
            });

        }


        function RelatedCommunication(data) {
            var NotificationId = data.currentTarget.attributes.NotificationId.value;
            var EsdalRefNumber = data.currentTarget.attributes.EsdalRefNumber.value;
            var EncryptedRefNumber = data.currentTarget.attributes.EncryptedRefNumber.value;
        startAnimation();
        var link = "";
        $.ajax({
            async: false,
            type: "GET",
            url: '@Url.Action("GetInboxDetails", "Movements")',
            dataType: "json",
            data: { esdalRefNumber: EsdalRefNumber },
            //contentType: "application/json; charset=utf-8",
            processdata: true,
            beforeSend: function () {

            },
            success: function (result) {
                var resultArray = result.split('|');

                var encryptInBoxId = resultArray[0];

                var encryptRoute = resultArray[1];
                var Status = resultArray[2];
                var NEN_ID = resultArray[3];
                var StatusName = resultArray[4];

                //check added for whether the notification is confirmed or not
                if (Status == "NE Notification" || Status == "NE Renotification") {
                    link = "../NENNotification/NE_Notification?NEN_ID=" + NEN_ID + "&Notificationid=" + NotificationId + "&esdal_ref=" + EncryptedRefNumber + "&route=" + encryptRoute + "&inboxId=" + encryptInBoxId + "&inboxItemStat=" + StatusName + "";

                }
                else {
                    link = "../Movements/AuthorizeMovementGeneral?Notificationid=" + NotificationId + "&esdal_ref=" + EncryptedRefNumber + "&route=" + encryptRoute + "&inboxId=" + encryptInBoxId + "&inboxItemStat=" + Status + "&FromInboxflag=" + 1 + "";
                }
            },
            error: function () {
            },
            complete: function () {
                stopAnimation();
            }
        });

        window.location.href = link;
        }
        function showaudit() {
            if (document.getElementById('showaudit').style.display !== "none") {
                document.getElementById('showaudit').style.display = "none"
                document.getElementById('up-chevlon8').style.display = "none"
                document.getElementById('down-chevlon8').style.display = "block"
            }
            else {
                document.getElementById('showaudit').style.display = "block"
                document.getElementById('up-chevlon8').style.display = "block"
                document.getElementById('down-chevlon8').style.display = "none"
                ShowAuditLog();
            }
        }

        function openDispensationList() {
            if (document.getElementById('dispensations').style.display !== "none") {
                document.getElementById('dispensations').style.display = "none"
                document.getElementById('up-chevlon3').style.display = "none"
                document.getElementById('down-chevlon3').style.display = "block"
            }
            else {
                document.getElementById('dispensations').style.display = "block"
                document.getElementById('up-chevlon3').style.display = "block"
                document.getElementById('down-chevlon3').style.display = "none"
            }
            stopAnimation();
        }

        var pageNum = 1;
        var pageSize = 10;
        function ShowAuditLog() {
            SelectNENAuditLog(pageNum, pageSize);
        }
        function SelectNENAuditLog(pageNum, pageSize) {
            var NEN_Notif = $('#ESDAL_Reference').val();
            startAnimation();
            $("#showaudit").load('../Logging/NENAuditLogPopup?pageNum=' + pageNum + '&pageSize=' + pageSize + '&NEN_Notif_No=' + NEN_Notif, {},
                function () {
                });
        }
        function affectedstructure(analysisId) {
            var analysisId = data.currentTarget.attributes.AnalysisId.value;
                if (document.getElementById('affectedstructure').style.display !== "none") {
                    document.getElementById('affectedstructure').style.display = "none"
                    document.getElementById('up-chevlon10').style.display = "none"
                    document.getElementById('down-chevlon10').style.display = "block"
                }
                else {
                    document.getElementById('affectedstructure').style.display = "block"
                    document.getElementById('up-chevlon10').style.display = "block"
                    document.getElementById('down-chevlon10').style.display = "none"
                    if (analysisId != 0)
                    {
                        DisplayAffectedStructure(analysisId)
                    }
                }
        }
        function DisplayAffectedStructure(analysisId)
        {
            var revisionId = $('#REVISION_ID').val();
            var sortFlag = '@(ViewBag.SortMovement != null ? true : false)';
            var showall = $("#ShowAllAffectStruct").val();
            $("#Analysis_id").val(analysisId);
            var test = $("#showallstruct").val();
            if (test == 'on' && firsttime < 1 )
            {

                $("#Firsttime").val("Firsttime");

                    firsttime = firsttime + 2;
                    var a = $("#StructAnalysisID").val();
                    if ($("#showallstruct").is(":checked")) {
                        $("#ShowAllAffectStruct").val(true);
                        DisplayAffectedStructure(a);
                    }
                    else {
                        $("#ShowAllAffectStruct").val(false);
                        DisplayAffectedStructure(a);
                    }
            }
            else
            {

                $("#Firsttime").val("secondTime");
                startAnimation();
                $("#Affected_Struct_Auth_Mov").show();
                $("#structcheckbox").show();
                //ShowAllStruct = true - hardcoded for demo. it will be removed after implementing filter
                $.ajax({
                    type: "GET",
                    url: "../RouteAssessment/ListAffectedStructures",
                    //contentType: "application/json; charset=utf-8",
                    data: { analysisID: analysisId, revisionId: revisionId, SORTflag: sortFlag, isAuthMove: true, ShowAllStruct: showall },
                    datatype: "json",
                    success: function (result) {
                        $("#affectedstructure").html(result);
                        $("#Affected_Structures").show();
                        $("#StructureGenDet").hide();
                        $("#Driving_Instruction").hide();
                        $("#AffectedRoads").hide();// div related to affected roads
                        $("#GeneralDetails").hide();
                        $("#BackAfftStruct").hide();
                        stopAnimation();
                    },
                    error: function () {
                        location.reload();
                    }
                });
            }
        }


        // show print options
        function printOptions() {
            buttonClicked = 1;
            if (document.getElementById('printOptions').style.display !== "none") {
                document.getElementById('printOptions').style.display = "none";
                $("#printOptions").removeClass('clicked');
                $("#MarginTop").css('margin-top', 0);
                $("#printOptions").css("z-index", "0");
                 }
            else {
                document.getElementById('printOptions').style.display = "block"
                $("#printOptions").css("z-index", "1");
                $("#printOptions").addClass('clicked');
                var val = $("#DivHeight").val(); //by id
                if (val == 1) {
                    $("#MarginTop").css('margin-top', -80);
                }
                else {
                    $("#MarginTop").css('margin-top', -40);
                    $("#printOptions").css('height', 40);
                }
            }
        }


      function RouteMapDetail()
      {

            if ($('#RouteId').val() != 0) {

                var rtid = $('#RouteId').val();
                var rttype = $('#RouteType').val();
                RouteView(rtid, rttype, false);
            }
        }

        function ViewGPXRoute() {
            var routeId = $("#RouteId").val();
            var notifId = $("#NOTIFICATION_ID").val();
            var movementType = 207003;
            if ($("#gpxRoute").prop('checked') == true) {
                //New function to show GPX geometry using Geoserver
                ShowGpxRoute(notifId, routeId, movementType);
            }
            else if ($("#gpxRoute").prop('checked') == false) {
                 //New function to hide GPX geometry using Geoserver
                HideGpxRoute();
            }
        }
        $('body').on('click', '#span1-routeview', function (e) {
            e.preventDefault();
            var RouteID = $(this).data('RouteID');
            var RouteType = $(this).data('RouteType');
            var var1 = $(this).data('var1');
            RouteView(RouteID, RouteType, var1);
        });
        $('body').on('click', '#span2-routeview', function (e) {
            e.preventDefault();
            var RouteID = $(this).data('RouteID');
            var RouteType = $(this).data('RouteType');
            var var1 = $(this).data('var1');
            RouteView(RouteID, RouteType, var1);
        });
        $('body').on('click', '#span3-routeview', function (e) {
            e.preventDefault();
            var RouteID = $(this).data('RouteID');
            var RouteType = $(this).data('RouteType');
            var var1 = $(this).data('var1');
            RouteView(RouteID, RouteType, var1);
        });
        function RouteView(RouteId, RouteType, VIsSortPortal) {

            $("#GeneralDetails").hide();
            $("#ShowDetail").show();
            $("#map").empty();
            $("#leftpanel").empty();
            $('#leftpanel').empty();
            $("#RouteId").val(RouteId);
            $("#RouteType").val(RouteType);
            $('#Starting').text('')
            $('#Ending').text('')
            $('#Tabvia').empty();
            if ($("#RouteName").val() == 'NE Notification API') {
                $("#showGPXRoute").show();
            }
            var str = '';
            if (RouteType != "")
                str = RouteType;
            var HfRouteType = $('#HfRouteType').val();
            $.ajax({
                type: 'POST',
                dataType: 'json',
                //  async: false,
                url: '../Routes/GetPlannedRoute',
                data: { RouteID: RouteId, routeType: str, IsSortPortal: VIsSortPortal },
                beforeSend: function (xhr) {
                    startAnimation();
                },
                success: function (result) {
                    if (result.result != null) {
                        var count = result.result.routePathList[0].routePointList.length, strTr, flag = 0, Index = 0;

                        for (var i = 0; i < count; i++) {
                            if (result.result.routePathList[0].routePointList[i].pointGeom != null || result.result.routePathList[0].routePointList[i].linkId != null)
                                flag = 1;
                        }
                        if (flag == 1 || '@Session["RouteFlag"]' == 1 || '@Session["RouteFlag"]' == 3) {
                            $('#Tabvia').empty();
                            $("#ShowDetail").show();
                            $("#RouteName").text(result.result.routePartDetails.routeName);
                            if (result.result.routePartDetails.routeDescr != null && result.result.routePartDetails.routeDescr != "")
                                $("#RouteDesc").text(result.result.routePartDetails.routeDescr);
                            for (var i = 0; i < count; i++) {
                                if (result.result.routePathList[0].routePointList[i].pointType == 0)
                                    $('#Starting').text(result.result.routePathList[0].routePointList[0].pointDescr);
                                else if (result.result.routePathList[0].routePointList[i].pointType == 1)
                                    $('#Ending').text(result.result.routePathList[0].routePointList[1].pointDescr);

                                else if (result.result.routePathList[0].routePointList[i].pointType >= 2) {
                                    Index = Index + 1;
                                    strTr = "<div >" + Index +" . "+ result.result.routePathList[0].routePointList[i].pointDescr + "</div>"
                                    $('#Tabvia').append(strTr);
                                }
                            }

                            $("#map").empty();
                            $("#map").load('@Url.Action("A2BPlanning", "Routes", new { routeID = 0 })', function () {
                                setRouteID(RouteId);
                                var listCountSeg = 0;

                                for (var i = 0; i < result.result.routePathList.Count; i++) {
                                    listCountSeg = result.result.routePathList[i].routeSegmentList.Count;
                                    if (listCountSeg > 0)
                                        break;
                                }

                                if (listCountSeg == 0) {
                                    if (result.result.routePathList[0].routeSegmentList != null)
                                        listCountSeg = 1;
                                }
                                if (RouteType == "outline") {
                                    loadmap('DISPLAYONLY');
                                    showSketchedRoute(result.result);
                                    // loadmap(10, result.result);
                                }
                                else {

                                    //loadmap(7, result.result);
                                    loadmap('DISPLAYONLY', result.result);
                                }
                            })
                        }
                        else {

                            $("#map").empty();
                            $("#ShowDetail").show();
                            $('#trStarting').show();
                            $("#RouteName").text(result.result.routePartDetails.routeName);
                            $('#Tabvia').empty();
                            if (result.result.routePartDetails.routeDescr != null && result.result.routePartDetails.routeDescr != "")
                                $("#RouteDesc").text(result.result.routePartDetails.routeDescr);
                            for (var i = 0; i < count; i++) {

                                if (result.result.routePathList[0].routePointList[i].pointType == 0)
                                    $('#Starting').text(result.result.routePathList[0].routePointList[0].pointDescr);
                                else if (result.result.routePathList[0].routePointList[i].pointType == 1)
                                    $('#Ending').text(result.result.routePathList[0].routePointList[1].pointDescr);

                                else if (result.result.routePathList[0].routePointList[i].pointType >= 2) {
                                    Index = Index + 1;
                                    strTr = "<div><div style='width:40px'>" + Index + "</div><div>" + result.result.routePathList[0].routePointList[i].pointDescr + "</div></div>"
                                    $('#Tabvia').append(strTr);
                                }
                            }
                            $("#map").text('No visual representation of route available.');
                        }
                    }
                    else {
                        $("#RouteName").text('Route not available.');
                        document.getElementById('ShowDetail').style.display = "none";
                        document.getElementById('trHeaderDescription').style.display = "none";
                        $("#map").empty();
                        $("#map").load('@Url.Action("A2BPlanning", "Routes", new {routeID = 0})', function () {
                        loadmap('DISPLAYONLY');

                     });

                    if ($('#Starting').val() !='') {
                        $('#trRoute').show();
                        $('#trStarting').show();
                        $('#trVia').show();
                        $('#trEnding').show();
                        $('#trRoutePart').show();

                    }
                    else {
                        document.getElementById('ShowDetail').style.display = "none";
                        //$('#ShowDetail').hide();
                        $('#trRoute').hide();
                        $('#trStarting').hide();
                        $('#trVia').hide();
                        $('#trEnding').hide();
                        $('#trRoutePart').hide();
                    }
                    //if (!$('#RouteDesc').val().length >0) {
                    //    $('#trHeaderDescription').show();
                    //    $('#trdesc').show();
                    //}
                    //else {
                    //    $('#trHeaderDescription').hide();
                    //    $('#trdesc').hide();
                    //}

                    }



                    stopAnimation();
                },
                complete: function () {
                    stopAnimation();

                },
                error: function () {

                    $("#map").empty();
                    $("#map").load('@Url.Action("A2BPlanning", "Routes", new {routeID = 0})', function () {
                          loadmap('DISPLAYONLY');

                         });
                }

            });
        }


        function reviewProcHighway() {
            if (document.getElementById('reviewproc').style.display !== "none") {
                document.getElementById('reviewproc').style.display = "none"
                document.getElementById('up-chevlon11').style.display = "none"
                document.getElementById('down-chevlon11').style.display = "block"
            }
            else {
                document.getElementById('reviewproc').style.display = "block"
                document.getElementById('up-chevlon11').style.display = "block"
                document.getElementById('down-chevlon11').style.display = "none"
            }
        }

        function printDiv() {
            $('#divPrintReview').hide();

            var divToPrint = document.getElementById('reviewproc');

            var newWin = window.open('', 'Print-Window');

            newWin.document.write('<html><body onload="window.print()">' + divToPrint.innerHTML + '</body></html>');

            newWin.document.close();

            setTimeout(function () { newWin.close(); }, 10);
            $('#divPrintReview').Show();
        }


        function PrintReport(data) {
            var NotificationId = data.currentTarget.attributes.NotificationId.value;
            var ESDALREf = data.currentTarget.attributes.ESDALReference.value;
            var DocType = data.currentTarget.attributes.DocType.value;
            var UserType = data.currentTarget.attributes.UserType.value;
            var DRN = data.currentTarget.attributes.DRN.value;

            ESDALREf = escape(ESDALREf);
            if (DocType == 'agreed') {
                var link = "../Movements/PrintAgreedReport?esdalRefno=" + ESDALREf + "&userType=" + UserType + "&DRN=" + DRN + "&DocType=" + $('#RouteName').val() + "&VersionId=" + $('#hfVERSION_ID').val()+"";
                window.open(link, '_blank');
            }
            else if (DocType == 'amendment to agreement') {
                var link = "../Movements/PrintAmendmentToAgreementReport?esdalRefno=" + ESDALREf + "&userType=" + UserType + "&DRN=" + DRN + "&DocType=" + $('#RouteName').val() + "&VersionId=" + $('#hfVERSION_ID').val() +"";
                window.open(link, '_blank');
            }
            else if (DocType == 'proposed') {
                var link = "../Movements/PrintProposedReport?esdalRefno=" + ESDALREf + "&userType=" + UserType + "&DRN=" + DRN + "&DocType=" + $('#RouteName').val() + "";
                window.open(link, '_blank');
            }
            else if (DocType == 'reproposal') {
                var link = "../Movements/PrintReProposedReport?esdalRefno=" + ESDALREf + "&userType=" + UserType + "&DRN=" + DRN + "&DocType=" + $('#RouteName').val() + "&SORTflag=" + sortFlag + "";
                window.open(link, '_blank');
            }
            else if (DocType == 'no longer affected') {
                var link = "../Movements/PrintNoLongerAffectedReport?esdalRefno=" + ESDALREf + "&userType=" + UserType + "&DRN=" + DRN + "&DocType=" + $('#RouteName').val() + "";
                window.open(link, '_blank');
            }
            else {
                //var link = "../Movements/PrintReportPDF?notificationId=" + NotificationId + "&esdalRefno=" + ESDALREf + "&userType=" + UserType + "&DRN=" + DRN + "&ISNENVal=" + IS_NEN + "&NENInboxId=" + V_INBOX_ID + "";//added ISNENVal and NENInboxId param for NEN project
                var link = "../Movements/PrintReport?notificationId=" + NotificationId + "&esdalRefno=" + ESDALREf + "&userType=" + UserType + "&DRN=" + DRN + "&ISNENVal=" + IS_NEN + "&NENInboxId=" + V_INBOX_ID + "&DocType=" + $('#RouteName').val() + "";//added ISNENVal and NENInboxId param for NEN project
                window.open(link, '_blank');
            }
            if (document.getElementById('printOptions').style.display !== "none") {
                document.getElementById('printOptions').style.display = "none";
                $("#printOptions").removeClass('clicked');
                $("#MarginTop").css('margin-top', 0);
                $("#printOptions").css("z-index", "0");

                 }
        }


        function PrintReducedReport(data) {
            var NotificationId = data.currentTarget.attributes.NotificationId.value;
            var ESDALREf = data.currentTarget.attributes.ESDALReference.value;
            var Classification = data.currentTarget.attributes.DocType.value;
            var UserType = data.currentTarget.attributes.UserType.value;
            var DRN = data.currentTarget.attributes.DRN.value;
            ESDALREf = escape(ESDALREf);
            //var link = "../Movements/PrintIndexV?notificationId=" + NotificationId + "&esdalRefno=" + ESDALREf + "&classification=" + Classification + "&userType=" + UserType + "&DRN=" + DRN + "&ISNENVal=" + IS_NEN + "&NENInboxId=" + V_INBOX_ID + "";//added ISNENVal and NENInboxId param for NEN project

            var link = "../Movements/PrintReducedReport?notificationId=" + NotificationId + "&esdalRefno=" + ESDALREf + "&classification=" + Classification + "&userType=" + UserType + "&DRN=" + DRN + "&ISNENVal=" + IS_NEN + "&NENInboxId=" + V_INBOX_ID + "&DocType=" + $('#RouteName').val() + "";//added ISNENVal and NENInboxId param for NEN project
            window.open(link, '_blank');
            if (document.getElementById('printOptions').style.display !== "none") {
                document.getElementById('printOptions').style.display = "none";
                $("#printOptions").removeClass('clicked');
                $("#MarginTop").css('margin-top', 0);
                $("#printOptions").css("z-index", "0");

                 }
        }

        function routeRoads(data) {
            var analysisId = data.currentTarget.attributes.AnalysisId.value;
            
            if (document.getElementById('RoadsRoute').style.display !== "none") {
                document.getElementById('RoadsRoute').style.display = "none"
                document.getElementById('up-chevlon13').style.display = "none"
                document.getElementById('down-chevlon13').style.display = "block"
            }
            else {
                document.getElementById('RoadsRoute').style.display = "block"
                document.getElementById('up-chevlon13').style.display = "block"
                document.getElementById('down-chevlon13').style.display = "none"
                showRoadsOnRoute(analysisId);
            }
        }
        function routeOverview(data) {
            var analysisId = data.currentTarget.attributes.AnalysisId.value;
            var revisionId = data.currentTarget.attributes.RevisionId.value;
            if (document.getElementById('routeDescription').style.display !== "none") {
                document.getElementById('routeDescription').style.display = "none"
                document.getElementById('up-chevlon12').style.display = "none"
                document.getElementById('down-chevlon12').style.display = "block"
            }
            else {
                document.getElementById('routeDescription').style.display = "block"
                document.getElementById('up-chevlon12').style.display = "block"
                document.getElementById('down-chevlon12').style.display = "none"
                if (analysisId != 0) {
                    showRouteDescription(analysisId, revisionId);
                }
            }
        }
        function drivingInstruction(data) {
            var analysisId = data.currentTarget.attributes.AnalysisId.value;
            if (document.getElementById('drivingInstruction').style.display !== "none") {
                document.getElementById('drivingInstruction').style.display = "none"
                document.getElementById('up-chevlon14').style.display = "none"
                document.getElementById('down-chevlon14').style.display = "block"
            }
            else {
                document.getElementById('drivingInstruction').style.display = "block"
                document.getElementById('up-chevlon14').style.display = "block"
                document.getElementById('down-chevlon14').style.display = "none"
                if (analysisId != 0) {
                    ShowDrivingInstr(analysisId)
                }
            }
        }
        function showRouteDescription(analysisId, revisionId) {
            startAnimation();
            var AuthorizeFlagForDI = "true";
            var sortFlag = '@(ViewBag.SortMovement != null ? true : false)';
            routeDescription
            
            $("#routeDescription").load("../RouteAssessment/ListRouteDescription?analysisID=" + analysisId + '&anal_type=' + 2 + '&ContentRefNo=' + '@ViewBag.Content_ref_no' + '&revisionId=' + revisionId + '&IsCandidate=' + false + '&AgreedSONotif=' + 0 + '&SORTFlag=' + sortFlag, function () {
                stopAnimation();
            });
        }
        function showRoadsOnRoute(analysisId) {
            startAnimation();
            var AuthorizeFlagForDI = "true";
            var sortFlag = '@(ViewBag.SortMovement != null ? true : false)';
            routeDescription
            $("#RoadsRoute").load("../RouteAssessment/AffectedRoads?analysisID=" + analysisId + '&anal_type=' + 8 +'&notifId'+ 0 + '&contentRefNo=' + '' + '&revisionId=' + 0 + '&IsCandidate=' + false + '&isAuthMove=' + true + '&versionId=' + 0 + '&IsVR1=' + false +'&FilterByOrgID'+ 0 + '&SORTFlag=' + sortFlag, function () {
                stopAnimation();
            });
        }
        function ShowDrivingInstr(analysisId) {
            startAnimation();
        var AuthorizeFlagForDI = "true";
        var sortFlag = '@(ViewBag.SortMovement != null ? true : false)';
        //startAnimation();
        $("#Affected_Struct_Auth_Mov").show();
        $("#drivingInstruction").load("../RouteAssessment/ListDrivingInstructions?analysisID=" + analysisId + '&anal_type=' + 1 + '&ContentRefNo=' + '@ViewBag.Content_ref_no' + '&AuthorizeFlagForDI=' + AuthorizeFlagForDI + '&SORTflag=' + sortFlag, function () {
                stopAnimation();
        });
        }

        function PaginateListContact() {

            var cntrId = '#contact-list';
            $(cntrId).find('.pagination').find('li').not('.active, .PagedList-skipToLast, .PagedList-skipToNext, .PagedList-skipToFirst, .PagedList-skipToPrevious').find('a').click(function () {
                var pageNum = $(this).html();
                AjaxPaginationforContactList(pageNum);
            });

            PaginateToLastPageContactList(cntrId);
            PaginateToFirstPageContactList(cntrId);
            PaginateToNextPageContactList(cntrId);
            PaginateToPrevPageContactList(cntrId);
        }
        //method to paginate to last page
        function PaginateToLastPageContactList(ContainerId) {

            $(ContainerId).find('.PagedList-skipToLast').click(function () {
                var pageCount = $(ContainerId).find('#TotalPages').val();
                AjaxPaginationforContactList(pageCount);
            });
        }

        //method to paginate to first page
        function PaginateToFirstPageContactList(ContainerId) {

            $(ContainerId).find('.PagedList-skipToFirst').click(function () {
                AjaxPaginationforContactList(1);
            });
        }

        //method to paginate to Next page
        function PaginateToNextPageContactList(ContainerId) {

            $(ContainerId).find('.PagedList-skipToNext').click(function () {
                var thisPage = $(ContainerId).find('.active').find('a').html();
                var nextPage = parseInt(thisPage) + 1;
                AjaxPaginationforContactList(nextPage);
            });
        }
        //method to paginate to Previous page
        function PaginateToPrevPageContactList(ContainerId) {

            $(ContainerId).find('.PagedList-skipToPrevious').click(function () {
                var thisPage = $(ContainerId).find('.active').find('a').html();
                var prevPage = parseInt(thisPage) - 1;
                AjaxPaginationforContactList(prevPage);
            });
        }

        function AjaxPaginationforContactList(pageNum) {
             var random = Math.random();
            var notiId  = $('#hf_Notificationid').val(); 
            var routeDetails = '@routeType';
            var notificationId = $('#NOTIFICATION_ID').val();
            var esdalRefNumber = $('#ESDAL_Reference').val();
            var ID = $('#hAnalysisId').val();

            routeDetails = routeDetails.toLowerCase().replace("amendment to agreement", "amendment");
            routeDetails = routeDetails.toLowerCase().replace("no longer affected", "nolongeraffected");
            routeDetails = routeDetails.toLowerCase().replace("ne agreed notification", "notification");//Added For NEN project
            routeDetails = routeDetails.toLowerCase().replace("ne notification", "notification");//Added For NEN project
            routeDetails = routeDetails.toLowerCase().replace("ne renotification", "notification");//Added For NEN project
            routeDetails = routeDetails.toLowerCase().replace("ne agreed renotification", "notification");//Added For NEN project
            $.ajax({
                url: '../Contact/AuthoriseMovementShowContactList',
                type: 'POST',
                cache: false,
                async: false,
                data : { page: pageNum, analysisID: ID, NotificationID: notificationId, ESDALRefNumber: esdalRefNumber, Type: routeDetails, random: random },
                beforeSend: function () {
                    startAnimation();
                },
                success: function (response) {
                    console.log('response' + response);
                    var result = $(response).find('div#contact-list').html();
                    $('div#contact-list').html(result);
                    //$("#contact-list").css("display", "block");
                    //$("#down-chevlon-icon2").css("display", "none");
                    //$("#up-chevlon-icon2").css("display", "block");
                },
                complete: function () {
                    stopAnimation();
                }
            });

            removeHrefContactLinks(); removeHrefContactLinks();
            PaginateListContact();
        }

        function removeHrefContactLinks() {
            var div = 'div#contact-list';
            $(div).find('.pagination').find('li a').removeAttr('href').css("cursor", "pointer");
        }

