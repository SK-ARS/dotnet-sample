
    var esdal_ref = "";
    var Flag_App_Status = "";
    var LatestAppRevID = "";
    var newUrl = "";
        $(document).ready(function () {
            $('body').on('click', '#PrintProposedReport', PrintProposedReport);
            $('body').on('click', '#PrintAgreedReport', PrintAgreedReport);
            $('body').on('click', '#notify', NotifySOAlert);
            $('body').on('click', '#revise', Revise);
            $('body').on('click', '#clone', Clone);
$('body').on('click','#WithdrawSOApp', function() { window['WithdrawSoApplication)'](); }); 
            $('body').on('click', '.ShowHideDetails', ShowHideDetails);

        var Helpdesk_redirect  = $('#hf_Helpdesk_redirect').val(); 
            if (Helpdesk_redirect == "true") {
                $("#menu-buttons").hide();
            }
    });
    $('#back-link').click(function () {
            history.go(-1);
    });

    function Revise() {
        var _verid = '@Model.VersionId';
        if ('@Model.EnteredBySORT' == 0) {
            ShowWarningPopup('Click Yes to create a new version of @Model.ESDALReference for editing.', 'ReviseSO');
        }
        else {
            ShowErrorPopup('SORT created application cannot be revised from haulier!');
        }
        }

        function ReviseSO() {
            CloseWarningPopup();
            let URL = "../Application/ReviseSOApplication";
            let Inputdata = { apprevid: @Model.ApplicationRevId, ESDALRefCode: '@Model.ESDALReference' };
            let movement_Type = 207001;
            $.ajax({
                url: URL,
                type: 'post',
                data: Inputdata,
                beforeSend: function () {
                    startAnimation();
                },
                success: function (data) {
                    //is_replan = CheckIsBroken(0, 0, data, 0, 0, 0);
                    //var msg = "";
                    //var replan = false;
                    //var autoReplanFail = 0;
                    //var autoReplanSuccess = 0;
                    //var brokenRouteCount = 0;
                    //var specialManouer = 0;
                    //for (var i = 0; i < is_replan.length; i++) {
                    //    if (is_replan[i].isBroken > 0) {
                    //        brokenRouteCount++;
                    //        if (is_replan[i].isReplan > 1) {   //check in the existing route is broken and there exists special manouers
                    //            specialManouer++;
                    //        }
                    //        else if (is_replan[i].isReplan < 2) {
                    //            replan = ReplanBrokenRoutes(is_replan[i].plannedRouteId, 0, false);
                    //            if (replan)
                    //                autoReplanSuccess++;
                    //            else
                    //                autoReplanFail++;
                    //        }
                    //    }
                    //}
                    //if (brokenRouteCount != 0) {
                    //    if (specialManouer > 0) { //multiple routes failed to replan
                    //        $('.popup111 .message111').css({ 'height': '183px' });
                    //        $('.popup111').css({ 'height': '221px' });
                    //        msg = 'Please be aware that due to the map upgrade the route(s) in this application contain previous map data and will need to be re-planned. Please re-plan this route, import a new route or create a new route.</br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.';
                    //    }
                    //    else if (autoReplanFail > 0) { //single route failed to replan
                    //        $('.popup111 .message111').css({ 'height': '183px' });
                    //        $('.popup111').css({ 'height': '221px' });
                    //        msg = 'Please be aware that due to the map upgrade the route(s) in this application contain previous map data and will need to be re-planned. Please re-plan this route, import a new route or create a new route.</br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.';
                    //    }
                    //    else if (autoReplanSuccess > 0) { // if all routes is replanned
                    //        msg = 'Please be aware that due to the map upgrade the route(s) in this application contain previous map data. However, the route(s) have been automatically re-planned based on new map data. Please check the route before proceeding.';
                    //    }
                    //    newUrl = '../Application/ListSOMovements?cloneapprevid=' + data + "&hauliermnemonic=" + hauliermnemonic + "&esdalref=" + esdalref + "&revisionno=" + revisionno + '&pageflag=2';
                    //    showWarningPopDialogBig(msg, 'Ok', '', 'WarningBrokenPopup', '', 1, 'info');
                    //}
                    //else {
                    //    var url = '../Application/ListSOMovements?cloneapprevid=' + data + "&hauliermnemonic=" + hauliermnemonic + "&esdalref=" + esdalref + "&revisionno=" + revisionno + '&pageflag=2';
                    //    window.location = url;

                    data = EncodedQueryString("revisionId=" + data.ApplicationRevId);
                    var redirectUrl = '../Movements/OpenMovement';
                    window.location = redirectUrl + data;
                    //}
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    location.reload();
                },
                complete: function () {
                    stopAnimation();
                }
            });
        }

    function NotifySOAlert() {

        var _verid = '@Model.VersionId';
        startAnimation();
        var isBroken = [];
        isBroken = CheckIsBroken(0, _verid, 0, 0, 0, 0);

        var brokenRouteCount = 0;
        for (var i = 0; i < isBroken.length; i++) {
            if (isBroken[i].isBroken > 0) {
                brokenRouteCount++;
            }
        }
        if (brokenRouteCount > 0) {
            var msg = 'Please be aware that due to the map upgrade your proposed Special Order route contains previous map data. You may continue to notify this route, however PLEASE DO NOT EDIT YOUR CURRENT ROUTE(S) AS IT MAY NO LONGER BE VALID. Please also check the affected parties carefully to ensure that all the relevant parties are included in this notification. If not, please add any missing parties manually.If you are unsure if any affected parties are missing, please contact the ESDAL Helpdesk or Highway England Abnormal Loads Team to discuss further.';
            var msg1 = 'Please be aware that due to the map upgrade your agreed Special Order route contains previous map data. You may continue to notify this route, however please check the affected parties carefully to ensure that all the relevant parties are included in this notification. If not, please add any missing parties manually. If you are unsure if any affected parties are missing, please contact the ESDAL Helpdesk or Highway England Abnormal Loads Team to discuss further.';
            $('.POP-dialogue1').css({ 'width': '400px' });
            $('.popup1').css({ 'width': '400px' });
            $('.popup1 .message1').css({ 'width': '400px' });
            $('.pop-message').css({ 'width': '320px' });
            $('.popup1 .message1').css({ 'height': '190px' });
            $('.popup1').css({ 'height': '230px' });
            $('.popup1 .message1').css({ 'text-align': 'justify' })
            if ('@Model.VersionStatus' == 305002 || '@Model.VersionStatus' == 305003) {
                $('.popup1 .message1').css({ 'height': '210px' });
                $('.popup1').css({ 'height': '250px' });
                ShowDialogWarningPop(msg, 'Ok', '', 'NotifySOAlertmsg', '', 1, 'info');
            }
            else {
                ShowDialogWarningPop(msg1, 'Ok', '', 'NotifySO', '', 1, 'info');
            }
        }
        else {
            if ('@Model.VersionStatus' == 305002 || '@Model.VersionStatus' == 305003) {
                stopAnimation();
                ShowDialogWarningPop('You are notifying without an agreement and should notify again once the route has been agreed. The haulier should not move until the agreed route and Special Order permit has been issued by National Highways and acceptable notification has been given. Do you want to continue?', 'NO', 'YES', 'WarningCancelBtn', 'NotifySO', 1, 'warning');
            }
            else {

                NotifySO();
            }
        }
    }
    function NotifySOAlertmsg() {
        stopAnimation();
        showWarningPopDialog('You are notifying a Proposed route, but need to re-notify once the Agreed route has been received. Do you want to continue?', 'No', 'Yes', 'WarningCancelBtn', 'NotifySO', 1, 'warning');
    }
    function NotifySO()
    {
        stopAnimation();
        if ('@Model.VersionStatus' == 305002 || '@Model.VersionStatus' == 305003) {
            WarningCancelBtn();
        }
        WarningCancelBtn();
        var AppRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
       $.ajax({
            url: "../Notification/NotifyApplication",
            type: 'post',
           data: { versionId: '@Model.VersionId', MaxPieces: '@Model.NumberofPieces', MoveStartDate: '@Model.MovementDateFrom', MoveEndDate: '@Model.MovementDateTo', ApplrevisionId: AppRevId, isVR1: 0, versionStatus:'@Model.VersionStatus' },
            beforeSend: function () {
                startAnimation();
            },
            success: function (data) {
                NotifID = data.result.NotificationId;
                data = EncodedQueryString("notifId=" + NotifID);
                var redirectUrl = '../Movements/OpenMovement';
                window.location = redirectUrl + data;
            },
            error: function (jqXHR, textStatus, errorThrown) {

            },
            complete: function () {
                stopAnimation();
            }
        });
    }
    function WarningBrokenPopup() {
        window.location = newUrl;
    }
        function Clone() {
            Msg = "Click Yes to create a new application which is a cloned version of @Model.ESDALReference application";
            ShowWarningPopup(Msg, 'CloneSO');
    }
        function CloneSO() {
            CloseWarningPopup();
        var _verid = '@Model.VersionId';
        var is_replan = 0;
        var esdalrefcode = $('#EsdalNo').val();
        var AppRevId = $('#ApplicationRevId').val() ? $('#ApplicationRevId').val() : 0;
        $.ajax({
            url: "../Application/CloneSOApplication",
            type: 'post',
            data: { apprevId: AppRevId, ESDALRefCode: esdalrefcode, isHistory: $("#Historical").val() },
            beforeSend: function () {
                startAnimation();
            },
            success: function (data) {
                //is_replan = CheckIsBroken(0, 0, data, 0, 0, 0);
                //var msg = "";
                //var replan = false;
                //var autoReplanFail = 0;
                //var autoReplanSuccess = 0;
                //var brokenRouteCount = 0;
                //var specialManouer = 0;

                //for (var i = 0; i < is_replan.length; i++) {
                //    if (is_replan[i].isBroken > 0) {
                //        brokenRouteCount++;
                //        if (is_replan[i].isReplan > 1) {   //check in the existing route is broken and there exists special manouers
                //            specialManouer++;
                //        }
                //        else if (is_replan[i].isReplan < 2) {
                //            replan = ReplanBrokenRoutes(is_replan[i].plannedRouteId, 0, false);
                //            if (replan)
                //                autoReplanSuccess++;
                //            else
                //                autoReplanFail++;
                //        }
                //    }
                //}
                //var Isbig = true;
                //if (brokenRouteCount != 0) {
                //    if (specialManouer > 0) { //multiple routes failed to replan
                //        $('.popup111 .message111').css({ 'height': '183px' });
                //        $('.popup111').css({ 'height': '221px' });
                //        msg = 'Please be aware that due to the map upgrade the route(s) in this application contain previous map data and will need to be re-planned. Please re-plan this route, import a new route or create a new route.</br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.';
                //    }
                //    else if (autoReplanFail > 0) { //single route failed to replan
                //        $('.popup111 .message111').css({ 'height': '183px' });
                //        $('.popup111').css({ 'height': '221px' });
                //        msg = 'Please be aware that due to the map upgrade the route(s) in this application contain previous map data and will need to be re-planned. Please re-plan this route, import a new route or create a new route.</br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.';
                //    }
                //    else if (autoReplanSuccess > 0) { // if all routes is replanned
                //        Isbig = false;
                //        msg = 'Please be aware that due to the map upgrade the route(s) in this application contain previous map data. However, the route(s) have been automatically re-planned based on new map data. Please check the route before proceeding.';
                //    }
                //    newUrl = '../Application/ListSOMovements?cloneapprevid=' + data + '&pageflag=2';
                //    if (Isbig) {
                //        showWarningPopDialogBig(msg, 'Ok', '', 'WarningBrokenPopup', '', 1, 'info');
                //    }
                //    else {
                //        showWarningPopDialog(msg, 'Ok', '', 'WarningBrokenPopup', '', 1, 'info');
                //    }
                //}
                //else {
                   // var url = '../Application/ListSOMovements?cloneapprevid=' + data + '&pageflag=2';
                data = EncodedQueryString("revisionId=" + data.ApplicationRevId);
                var redirectUrl = '../Movements/OpenMovement';
                window.location = redirectUrl + data;
                //}
            },
            error: function (jqXHR, textStatus, errorThrown) {
                location.reload();
            },
            complete: function () {
                stopAnimation();
            }
        });
    }
    function WithdrawSoApplication() {
        deleteFlag = 1;
        //var project_id = $('#Haul_Proj_ID').val() ? $('#Haul_Proj_ID').val() : 0;
        if ('@Model.EnteredBySORT' == 0) {
            if ('@Model.ESDALReference' != null && '@Model.ESDALReference' != 0) {
                esdal_ref = '@Model.ESDALReference';
            }

            Msg = "Do you want to withdraw application @Model.ESDALReference ?";
            ShowWarningPopup(Msg, 'WithdrawSoApp');
        }
        else {
            ShowErrorPopup('SORT created application cannot be withdrawn from haulier!');
        }
    }
        function WithdrawSoApp() {
            CloseWarningPopup();
            var project_id = @Model.ProjectId;//$('#Haul_Proj_ID').val() ? $('#Haul_Proj_ID').val() : 0;
            CheckLatestAppStatus(project_id);// && app status checked for #7855

        //var AppRevId = $('#ApplicationRevId').val() ? $('#ApplicationRevId').val() : 0;

        if (Flag_App_Status != 308001) {//app status checked for #7855
            $.ajax({
                url: "../Application/WithdrawApplication",
                type: 'POST',
                cache: false,
                async: false,
                data: { Project_ID: @Model.ProjectId, Doc_type: 'Special Order', EsdalRefNumber: '@Model.ESDALReference', app_rev_id: @Model.ApplicationRevId },
                beforeSend: function () {
                    startAnimation();
                },
                success: function (result) {
                    if (result.Success) {
                        var Msg = "\"" + esdal_ref + '\" application withdraw successfully';
                        ShowSuccessModalPopup(Msg, 'NavigateToSamePage');
                    }
                    else {
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    location.reload();
                },
                complete: function () {
                    stopAnimation();
                }
            });
        }
        else {
            var Msg = "A WIP application exists for the current project and hence cannot be withdrawn. Please delete the WIP application to continue.";
            ShowErrorPopup(Msg);
        }
    }
    function CheckLatestAppStatus(Proj_ID) {// && app status checked for #7855
        $.ajax({
            url: "../Application/CheckLatestAppStatus",
            type: 'POST',
            cache: false,
            async: false,
            data: { Project_ID: Proj_ID},
                beforeSend: function () {
                    startAnimation();
                },
                success: function (Result) {
                    var dataCollection = Result;
                    if (dataCollection.result.ApplicationStatus > 0) {
                        Flag_App_Status = dataCollection.result.ApplicationStatus;
                        LatestAppRevID = dataCollection.result.ApplicationRevId;
                    }
                    else {
                        ShowErrorPopup("Error occurred while withdraw.");
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    //other stuff
                    location.reload();
                },
                complete: function () {
                    stopAnimation();
                    //cntTr.remove();
                }
         });
    }

        function PrintProposedReport(data) {
            var ESDALREf = data.currentTarget.attributes.ESDALREf.value;
            var ContactId = data.currentTarget.attributes.ContactId.value;
            var OrganisationId = data.currentTarget.attributes.OrganisationId.value;

            var link = "../Notification/HaulierProposedRouteDocument?esdalRefNo=" + ESDALREf + "&organisationId=" + OrganisationId + "&contactId=" + ContactId + "";
            window.open(link, '_blank');
    }

        function PrintAgreedReport(data) {
            var ESDALREf = data.currentTarget.attributes.ESDALREf.value;
            var ContactId = data.currentTarget.attributes.ContactId.value;
            var OrderNo = data.currentTarget.attributes.OrderNo.value;


            var link = "../Notification/HaulierAgreedRouteDocument?esDALRefNo=" + ESDALREf + "&order_no=" + OrderNo + "&contactId=" + ContactId + "";
            window.open(link, '_blank');
    }
        function ShowHideDetails(data) {
            var target = data.currentTarget.attributes.target.value;
                       switch (target) {
                case 1:
                     //contact-details
                    if (document.getElementById('contact-details').style.display !== "none") {
                        document.getElementById('contact-details').style.display = "none"
                        document.getElementById('chevlon-up-icon1').style.display = "none"
                        document.getElementById('chevlon-down-icon1').style.display = "block"
                    }
                    else {
                        document.getElementById('contact-details').style.display = "block"
                        document.getElementById('chevlon-up-icon1').style.display = "block"
                        document.getElementById('chevlon-down-icon1').style.display = "none"
                    }
                    break;
                case 2:
                    //div_notes_from_ha
                    if (document.getElementById('div_notes_from_ha').style.display !== "none") {
                        document.getElementById('div_notes_from_ha').style.display = "none"
                        document.getElementById('chevlon-up-icon2').style.display = "none"
                        document.getElementById('chevlon-down-icon2').style.display = "block"
                    }
                    else {
                        document.getElementById('div_notes_from_ha').style.display = "block"
                        document.getElementById('chevlon-up-icon2').style.display = "block"
                        document.getElementById('chevlon-down-icon2').style.display = "none"
                    }
                    break;
                case 3:
                    //div_notes_to_ha
                    if (document.getElementById('div_notes_to_ha').style.display !== "none") {
                        document.getElementById('div_notes_to_ha').style.display = "none"
                        document.getElementById('chevlon-up-sog-icon3').style.display = "none"
                        document.getElementById('chevlon-down-sog-icon3').style.display = "block"
                    }
                    else {
                        document.getElementById('div_notes_to_ha').style.display = "block"
                        document.getElementById('chevlon-up-sog-icon3').style.display = "block"
                        document.getElementById('chevlon-down-sog-icon3').style.display = "none"
                    }
                    break;
                case 4:
                    //div_distribution_notes
                    if (document.getElementById('div_distribution_notes').style.display !== "none") {
                        document.getElementById('div_distribution_notes').style.display = "none"
                        document.getElementById('chevlon-up-sog-icon4').style.display = "none"
                        document.getElementById('chevlon-down-sog-icon4').style.display = "block"
                    }
                    else {
                        document.getElementById('div_distribution_notes').style.display = "block"
                        document.getElementById('chevlon-up-sog-icon4').style.display = "block"
                        document.getElementById('chevlon-down-sog-icon4').style.display = "none"
                    }
                    break;
                case 5:
                    //req-notes
                    if (document.getElementById('req-notes').style.display !== "none") {
                        document.getElementById('req-notes').style.display = "none"
                        document.getElementById('chevlon-up-sog-icon5').style.display = "none"
                        document.getElementById('chevlon-down-sog-icon5').style.display = "block"
                    }
                    else {
                        document.getElementById('req-notes').style.display = "block"
                        document.getElementById('chevlon-up-sog-icon5').style.display = "block"
                        document.getElementById('chevlon-down-sog-icon5').style.display = "none"
                    }
                    break;
                case 6:
                    //req-notes
                    if (document.getElementById('special-orders').style.display !== "none") {
                        document.getElementById('special-orders').style.display = "none"
                        document.getElementById('chevlon-up-sog-icon6').style.display = "none"
                        document.getElementById('chevlon-down-sog-icon6').style.display = "block"
                    }
                    else {
                        document.getElementById('special-orders').style.display = "block"
                        document.getElementById('chevlon-up-sog-icon6').style.display = "block"
                        document.getElementById('chevlon-down-sog-icon6').style.display = "none"
                    }
                    break;
            }

        }

        function NavigateToSamePage() {
           // window.location.reload();
            window.location.href = '/Movements/MovementList';
        }
