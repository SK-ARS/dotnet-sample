            $('#div_ListSOMovements').hide();
            $('#WrongVR1ApplDiv').show();

	 var NotifID = 0;
	 var AnalID = 0;
	 var ContentREfNo = "";
	 var VehCode = 0;
	 var clone = 0;
	 var esdal_ref = "";
	 var Flag_App_Status = "";
     var LatestAppRevID = "";
     var newUrl = '';
            var cloneRevise = 0;
	 $(document).ready(function ()
     {

		 $('#div_ListSOMovements').show();
		 $('#WrongVR1ApplDiv').hide();
         $("#loadVr1Info").on('click', LoadVr1SupplementaryInfo);
         $("#revise").on('click', ReviseVR1Appl);
         $("#clone").on('click', CloneVR1Appl);
         $("#WithdrawSOApp").on('click', WithdrawVR1Application);
if($('#hf_Helpdest_redirect').val() ==  "true")
		 {
			 $('#notify').css("display", "none");
			 $('#revise').css("display", "none");
			 $('#clone').css("display", "none");
			 $('#WithdrawSOApp').css("display", "none");
			 $('#vr1applyforfolder').css("display", "none");
			 $('#vr1IdFolderName').css("display", "none");
		 }

		 if ('@Model.ESDALReference' != 0 || '@Model.ESDALReference' != null)
		 {
			 $('#hdn_esdalrefnumberVR1').val('@Model.ESDALReference');
		 }
         var ApplicationRevId = $('#ApplicationRevisionId').val() ? $('#ApplicationRevisionId').val() : 0;
		 var url = '@Url.Action("VR1SupplementaryDetails", "Application")';
		 url += '?apprevisionId=' + ApplicationRevId;
        $("#suppinfo").load(url);
     });

    function LoadVr1SupplementaryInfo() {

        if (document.getElementById('suppinfo').style.display !== "none") {
            document.getElementById('suppinfo').style.display = "none"
            document.getElementById('chevlon-up-icon2').style.display = "none"
            document.getElementById('chevlon-down-icon2').style.display = "block"
        }
        else {
            document.getElementById('suppinfo').style.display = "block"
            document.getElementById('chevlon-up-icon2').style.display = "block"
            document.getElementById('chevlon-down-icon2').style.display = "none"
        }
    }

	 //For apply folders
	 $('#vr1applyforfolder').click(function () {
		 var projectID = $('#projid').val();
         var folderId = $('#vr1IdFolderName').val();
         var ApplicationRevId = $('#ApplicationRevisionId').val() ? $('#ApplicationRevisionId').val() : 0;
		 $.ajax({
			 url: '../Application/GetSetCommonFolderDetails',
			 type: 'POST',
			 cache: false,
			 async: false,
             data: { flag: 1, folderID: folderId, projectID: projectID, revisionID: ApplicationRevId },//flag to insert folder on perticular project Id
			beforeSend: function () {
				startAnimation();
			},
			success: function (data) {
				//if(data.folder)//
				showWarningPopDialog('Folder name changed.', 'Ok', '', 'WarningCancelBtn', 'loadFeedList', 1, 'info');

			},
			error: function (xhr, textStatus, errorThrown) {

			},
			complete: function () {

				stopAnimation();

			}
		});
	  });

	 //Notify Vr1 Application to create WIP notification n display it to send notification
	 $('#notify').click(function () {
	     var _verid  = $('#hf_versionid').val(); 
	     var isBroken = [];
         isBroken = CheckIsBroken(0, _verid, 0, 0, 0, 0);
         var brokenRouteCount = 0;
         for (var i = 0; i < isBroken.length; i++) {
             if (isBroken[i].isBroken > 0) {
                 brokenRouteCount++;
             }
         }
         if (brokenRouteCount > 0) {
	         $('.POP-dialogue1').css({ 'width': '400px' });
	         $('.popup1').css({ 'width': '400px' });
	         $('.popup1 .message1').css({ 'width': '400px' });
	         $('.pop-message').css({ 'width': '320px' });
	         $('.popup1 .message1').css({ 'height': '210px' });
	         $('.popup1').css({ 'height': '250px' });
	         $('.popup1 .message1').css({ 'text-align': 'justify' })
	         var msg = 'Please be aware that due to the map upgrade your approved VR1 route contains previous map data. You may continue to notify this route, however PLEASE DO NOT EDIT YOUR CURRENT ROUTE(S) AS IT MAY NO LONGER BE VALID. Please also check the affected parties carefully to ensure that all the relevant parties are included in this notification. If not, please add any missing parties manually.If you are unsure if any affected parties are missing, please contact the ESDAL Helpdesk or Highway England Abnormal Loads Team to discuss further.'
	         showWarningPopDialog(msg, 'Ok', '', 'notifyVr1Clck', '', 1, 'info');
	     }
	     else {
	         notifyVr1Clck();
	     }
	 });

     function notifyVr1Clck() {
         WarningCancelBtn();
         var VNotesWithAppl = $("#hdfNotesWithAppl").val();
         var verStatus = $("#VersionStatus").val();
	     $.ajax({
	         type: "POST",
             url: '../Notification/NotifyApplication',
             data: { versionId: '@ViewBag.versionid', MaxPieces: '@Model.MaxPiecesPerLoad', ApplNotes: VNotesWithAppl, MoveStartDate: '@Model.MovementDateFrom', MoveEndDate: '@Model.MovementDateTo', ApplrevisionId: '@ViewBag.revisionId', isVR1: 1, versionStatus: verStatus },
			 beforeSend: function () {
			     startAnimation();
			 },
			 success: function (data) {
                 NotifID = data.result.NotificationId;
                 data = EncodedQueryString("notifId=" + NotifID);
                 var redirectUrl = '../Movements/OpenMovement';
                 window.location = redirectUrl + data;
			 },
			 complete: function () {
			     stopAnimation();
			 }
		 });
     }
	 //Revise VR1
	 function ReviseVR1Appl() {
         ShowWarningPopup('Click Yes to create a new version of @Model.ESDALReference for editing.', 'ReviseVR1');
         clone = 0;
         cloneRevise = 2;
     }
	 //Clone VR1
	 function CloneVR1Appl() {
         ShowWarningPopup('Click Yes to create a new application which is a cloned version of @Model.ESDALReference application.', 'ReviseVR1');
         clone = 1;
         cloneRevise = 1;
	 }

            function ReviseVR1() {
                CloseWarningPopup();
        // var red_det = false;
        // var is_replan = 0;
		 if ('@Model.ReducedDetails' == 1) {
			 red_det = true;
		 }
		 else {
			 red_det = false;
		 }
                var AppRevId = $('#ApplicationRevisionId').val() ? $('#ApplicationRevisionId').val() : 0;
		 $.ajax({
			 url: "../Application/ReviseVR1Application",
			 type: 'post',
             data: { Revision_id: AppRevId, Reduced_det: '@Model.ReducedDetails', Clone_app: clone, VersionID: '@ViewBag.versionid', ESDALRefCode: '@Model.ESDALReference', isHistory: $("#Historical").val() },
             beforeSend: function () {
                 startAnimation();
             },
		     success: function (data) {
                 //is_replan = CheckIsBroken(0, data.result.VersionId, 0, 0, 0, 0);
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
                 //        msg = 'Please be aware that due to the map upgrade the route(s) in this VR1 application contain previous map data and will need to be re-planned. Please re-plan this route, import a new route or create a new route.</br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.';
                 //    }
                 //    else if (autoReplanFail > 0) { //single route failed to replan
                 //        $('.popup111 .message111').css({ 'height': '183px' });
                 //        $('.popup111').css({ 'height': '221px' });
                 //        msg = 'Please be aware that due to the map upgrade the route(s) in this VR1 application contain previous map data and will need to be re-planned. Please re-plan this route, import a new route or create a new route.</br></br>When re-planning the route on the map re-enter the start/end addresses and reconnect all waypoints, viapoints and re-add any annotations or special manoeuvres to the road network.';
                 //    }
                 //    else if (autoReplanSuccess > 0) { // if all routes is replanned
                 //        msg = 'Please be aware that due to the map upgrade the route(s) in this VR1 application contain previous map data. However, the route(s) have been automatically re-planned based on new map data. Please check the route before proceeding.';
                 //    }
                 //    newUrl = '../Application/ListSOMovements?revisionId=' + data.result.ApplicationrevId + "&pageflag=" + 1 + "&VR1Applciation=" + true + "&reduceddetailed=" + red_det + "&apprevid=" + data.result.ApplicationrevId + "&versionId=" + data.result.VersionId + "&isNotifyFlag=" + 5;
                 //    showWarningPopDialogBig(msg, 'Ok', '', 'WarningBrokenPopup', '', 1, 'info');
                 //}
                 //else {
                 //    var url = '../Application/ListSOMovements?revisionId=' + data.result.ApplicationrevId + "&pageflag=" + 1 + "&VR1Applciation=" + true + "&reduceddetailed=" + red_det + "&apprevid=" + data.result.ApplicationrevId + "&versionId=" + data.result.VersionId + "&isNotifyFlag=" + 5;
                 //    window.location = url;
                 //}
                 data = EncodedQueryString("revisionId=" + data.ApplicationRevisionId);
                 var redirectUrl = '../Movements/OpenMovement';
                 window.location = redirectUrl + data;
			 },
			 error: function () {
                 location.reload();
             },
             complete: function () {
                 stopAnimation();
             }
		 });
    }

    function WarningBrokenPopup() {
        window.location = newUrl;
    }

     function WithdrawVR1Application() {
         var project_id = $('#Project_ID').val() ? $('#Project_ID').val() : 0;
		 if ('@Model.ESDALReference' != null && '@Model.ESDALReference' != 0) {
			 esdal_ref = '@Model.ESDALReference';
		 }
	     CheckLatestAppStatus(project_id);// && app status checked for #7855
		 var Msg = "Do you want to withdraw application \"" + esdal_ref + '\" ?';
         ShowWarningPopup(Msg, 'WithdrawVR1App');
	 }

            function WithdrawVR1App() {
                CloseWarningPopup();
                var AppRevId = $('#ApplicationRevisionId').val() ? $('#ApplicationRevisionId').val() : 0;
		 var ProjectID = $('#Project_ID').val() ? $('#Project_ID').val() : 0;
		 if (Flag_App_Status != 308001) {//app status checked for #7855
		     $.ajax({
                 url: "../Application/WithdrawApplication",
		         type: 'POST',
		         cache: false,
		         async: false,
                 data: { Project_ID: ProjectID, Doc_type: 'VR-1', EsdalRefNumber: '@Model.ESDALReference', app_rev_id: AppRevId },
		         beforeSend: function () {
		             startAnimation();
		         },
		         success: function (result) {
                     if (result.Success) {
		                 var Msg = "\"" + esdal_ref + '\" Application withdrawn successfully';
                         ShowSuccessModalPopup(Msg, 'linktomovementinbox');
		             }
		         },
		         error: function (xhr, textStatus, errorThrown) {
		             //other stuff
		             location.reload();
		         },
		         complete: function () {
		             stopAnimation();
		             cntTr.remove();
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
             data: { Project_ID: Proj_ID },
             beforeSend: function () {
                 startAnimation();
                 ClearPopUp();
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

            function linktomovementinbox() {
                CloseSuccessModalPopup();
                window.location.href = '/Movements/MovementList';
            }
