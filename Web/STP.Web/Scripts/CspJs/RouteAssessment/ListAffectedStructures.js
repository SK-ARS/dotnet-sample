
    var id = 0;
    var RoutePartId = 0;
    var revisionId  = $('#hf_revisionId').val(); 
    $(document).ready(function () {
        selectedmenu('Movements'); // for selected menu
        $('#SearchStructure').show();
        var Org_name = $('#Organisation_Name').val();
        var Application_SORT = $('#SORTApplication').val();
        if (Application_SORT == "True") {
            if (Org_name != "") {
                @* if ('@ViewBag.AffectedStruct' != null || '@ViewBag.AffectedStruct' != "") {*@
                $('#div_mylistaffstruct').show();
                $('#Spn_OrgName').show();
                $('#Spn_OrgName').text(Org_name);
                //}
            }
            }

            if ($("#UserTypeId").val() == 696007) {
                CheckAssessmentResult();
            }
            if ($('#routediv1 td').length > 0) {
                $('#divNoAffected').hide();
            }
            else {
                $('#divNoAffected').show();
        }
        if (@AccessToALSAT!= 1) {
            $('.struct_select').hide();
            $('.chk_SelectAll').hide();
        }
        stopAnimation();

        $("#PerformAssessment").on('click', CheckAssessmentCount);
        $("#IDClearAffectedStructure").on('click', ClearAffectedStructure);
        $("#IddisplayStructure").on('click', displayStructure);
        $("#IDcloseFilters").on('click', closeFilters);
    });

    $("#showallstruct").click(function () {
        var a = $("#StructAnalysisID").val();
        if ($("#showallstruct").is(":checked")) {
            $("#ShowAllAffectStruct").val(true);
            DisplayAffectedStructure(a);
        }
        else {
            $("#ShowAllAffectStruct").val(false);
            DisplayAffectedStructure(a);
        }
    });

    function openFilters() {
        document.getElementById("filters").style.width = "450px";
        //document.getElementById("banner").style.filter = "brightness(0.5)";
        document.getElementById("banner").style.background = "white";
        //document.getElementById("navbar").style.filter = "brightness(0.5)";
        document.getElementById("navbar").style.background = "white";
        function myFunction(x) {
            if (x.matches) { // If media query matches
                document.getElementById("filters").style.width = "200px";
            }
        }
        var x = window.matchMedia("(max-width: 770px)")
        myFunction(x) // Call listener function at run time
        x.addListener(myFunction)
        $("#filters button").removeAttr("card");
    }

    function closeFilters() {
        document.getElementById("filters").style.width = "0";
        document.getElementById("banner").style.filter = "unset"
        document.getElementById("navbar").style.filter = "unset";

    }
    var selectedStructList = [];
    var totalStructureCount = 0;
    function ListStructureIDs(StructID, StructCode, routeId, sectionId, structCount) {

        totalStructureCount = structCount;
        var test = "'" + StructCode + "_" + routeId + "'";
        if ($("#" + StructCode + "_" + routeId).prop("checked")) {
            var struct = { ESRN: StructCode, SectionId: sectionId, RouteId: routeId, RoutePartNo: 1 };
            selectedStructList.push(struct);
        }
        else {
            removeByAttr(selectedStructList, 'ESRN', StructCode);
        }
    }
    var removeByAttr = function (arr, attr, value) {
        var i = arr.length;
        while (i--) {
            if (arr[i]
                && arr[i].hasOwnProperty(attr)
                && (arguments.length > 2 && arr[i][attr] === value)) {

                arr.splice(i, 1);

            }
        }
        return arr;
    }
    $('#chk_SelectAll').on('change', function (e) {
        var $inputs = $('#StructureAssessment input[type=checkbox]');

        if (e.originalEvent === undefined) {
            var allChecked = true;
            $inputs.each(function () {
                allChecked = allChecked && this.checked;
            });
            this.checked = allChecked;
            if (allChecked == true) {
                SelectAllStructures();
            }
        } else {
            $inputs.prop('checked', this.checked);
        }
    });
    $('#Structs').on('change', function (e) {
        if (this.checked) {
            document.getElementById('Underbridge').checked = true;
            document.getElementById('Overbridge').checked = true;
            document.getElementById('LevelCrossing').checked = true;
        }
        else {
            document.getElementById('Underbridge').checked = false;
            document.getElementById('Overbridge').checked = false;
            document.getElementById('LevelCrossing').checked = false;
        }

    });
    $('#Underbridge').on('change', function (e) {
        if (this.checked) {
            document.getElementById('Underbridge').checked = true;
            document.getElementById('Structs').checked = false;
            document.getElementById('Overbridge').checked = false;
            document.getElementById('LevelCrossing').checked = false;
        }
        else {
            document.getElementById('Structs').checked = false;
            document.getElementById('Overbridge').checked = false;
            document.getElementById('LevelCrossing').checked = false;
        }

    });
    $('#Overbridge').on('change', function (e) {
        if (this.checked) {
            document.getElementById('Overbridge').checked = true;
            document.getElementById('Structs').checked = false;
            document.getElementById('Underbridge').checked = false;
            document.getElementById('LevelCrossing').checked = false;
        }
        else {
            document.getElementById('Structs').checked = false;
            document.getElementById('Underbridge').checked = false;
            document.getElementById('LevelCrossing').checked = false;
        }

    });
    $('#LevelCrossing').on('change', function (e) {
        if (this.checked) {
            document.getElementById('LevelCrossing').checked = true;
            document.getElementById('Overbridge').checked = false;
            document.getElementById('Structs').checked = false;
            document.getElementById('Underbridge').checked = false;
        }
        else {
            document.getElementById('Overbridge').checked = false;
            document.getElementById('Structs').checked = false;
            document.getElementById('Underbridge').checked = false;
        }

    });
    $('#StructureAssessment input[type=checkbox]').on('change', function () {
        $('#chk_SelectAll').trigger('change');
    });

    function SelectUnselectToggle() {
        if ($('#chk_SelectAll').is(':checked')) {
            selectedStructList =@Html.Raw(Json.Encode(ViewBag.StructureAssessList));
            totalStructureCount = selectedStructList.length;
        }
        else {
            totalStructureCount = 0;
            selectedStructList = [];
        }
    }

    function CheckAssessmentResult() {
            var routeId = document.getElementById("routePartId").value;
            var esdalRefNo = $("#ESDAL_Reference").val();
            var structList = [];
            structList = @Html.Raw(Json.Encode(ViewBag.StructureAssessList));
            $.ajax({
                url: '../RouteAssessment/GetStructureAssessmentResult',
                type: 'POST',
                data: '{ "MovementRefNo":' + JSON.stringify(esdalRefNo) + ', "RouteID":' + routeId +'}',
                dataType: 'json',
                //contentType: 'application/json; charset=utf-8',
                success: function (data) {

                    for(var i=0; i<data.structureJSON.EsdalStructure.length; i++)
                    {
                        for(var j=0; j<structList.length; j++)
                        {
                            if (structList[j].ESRN == data.structureJSON.EsdalStructure[i].ESRN && structList[j].RouteId == routeId)
                            {
                                var esrn = structList[j].ESRN;
                                var seq_number = data.structureJSON.EsdalStructure[i].SeqNumber;
                                var assessment_status = data.structureJSON.EsdalStructure[i].Status;
                                var status = "#status_" + esrn + "_"+routeId;
                                if(assessment_status=="Completed")
                                {
                                    $(".currently-loading").hide();
                                    $(status).addClass("completedimg");
                                    $(status).attr("title", "Assessment Status : Completed " + '\n' +"Sequence Number : " + seq_number);
                                }
                                else
                                {
                                    $(".currently-loading").hide();
                                    $(status).addClass("inprogressimg");
                                    $(status).attr("title", "Assessment Status : In Progress " + '\n' +"Sequence Number : " + seq_number);
                                }
                            }
                        }
                    }
                    $(".currently-loading").hide();
                }
            });

    }
    function SelectUnselectAll(chk_SelectAll, routePartId) {
        var routePartId = $("." + routePartId.id).val();
        if ($('#' + chk_SelectAll.id).is(':checked')) {
            SelectAllStructures(routePartId);
        }
        else {
            UnselectAllStructures(routePartId);
        }
    }
    function SelectAllStructures(routepartId) {
            var structureassesslist = @Html.Raw(Json.Encode(ViewBag.StructureAssessList));
            selectedStructList = structureassesslist;
            var routeId = routepartId;
            var selectedStructListByRoute=[];
            for (var i = 0; i < selectedStructList.length; i++) {
                var esrn = selectedStructList[i].ESRN;
                $('input:checkbox[id='+esrn+'_'+routeId+']').prop('checked',true);
            }
            totalStructureCount=selectedStructList.length;
    }
    function UnselectAllStructures(routepartId)
   {

            var structureassesslist = @Html.Raw(Json.Encode(ViewBag.StructureAssessList));
            selectedStructList = structureassesslist;
            var routeId = routepartId;
            for (var i = 0; i < structureassesslist.length; i++) {
                var esrn = structureassesslist[i].ESRN;
                $('input:checkbox[id='+esrn+'_'+routeId+']').prop('checked',false);
            }
            totalStructureCount=0;
            selectedStructList = [];
        }
    function CheckAssessmentCount() {
        var routeId = document.getElementById("routePartId").value;
        var dataToSend = JSON.stringify(selectedStructList);
        if (totalStructureCount == 0) {
            //this condition to be checked when clear all button is clicked
            ShowDialogWarningPop('Please select at least one structure to send for assessment', 'Ok', '', 'WarningCancelBtn', '', 1, 'warning');
            return false;
        }
        else if (dataToSend == "[]") {
            ShowDialogWarningPop('Please select at least one structure to send for assessment', 'Ok', '', 'WarningCancelBtn', '', 1, 'warning');
            return false;
        }
        var notificationID = $("#NOTIFICATION_ID").val();
        var esdalRefNo = $("#ESDAL_Reference").val();
        var analysisID = $("#StructAnalysisID").val();
        startAnimation("Checking for any existing assessment for the selected structures. Please wait...");
        $.ajax({
            url: '../RouteAssessment/GetStructureAssessmentCount',
            type: 'POST',
            data: '{ "StuctureList":' + JSON.stringify(selectedStructList) + ', "MovementRefNo":' + JSON.stringify(esdalRefNo) + ', "RouteID":' + routeId + '}',
            dataType: 'json',
            //contentType: 'application/json; charset=utf-8',
            success: function (result) {
                if (result.success == 1) {
                    stopAnimation();
                    ShowDialogWarningPop('One or more structure(s) are already under assessment. Do you want to perform the assessment again? ', 'Cancel', 'Ok', 'cancelPerformAssessment', 'PerformAssessment', 1, 'warning');
                }
                else {
                    stopAnimation();
                    PerformAssessment();
                }
            }
        });

    }
    //perform assessment for the selected structures
    function PerformAssessment() {
        WarningCancelBtn();
        var routeId = document.getElementById("routePartId").value;
        var dataToSend = JSON.stringify(selectedStructList);
        var notificationID = $("#NOTIFICATION_ID").val();
        var esdalRefNo = $("#ESDAL_Reference").val();
        var analysisID = $("#StructAnalysisID").val();
        startAnimation("Structure assessment has been started. Please wait…");
        $.ajax({
            url: '../RouteAssessment/PerformStructureAssessment',
            type: 'POST',
            //async: false,
            data: '{ "StuctureList":' + JSON.stringify(selectedStructList) + ', "NotificationID":' + notificationID + ', "MovementRefNo":' + JSON.stringify(esdalRefNo) + ', "AnalysisID":' + analysisID + ', "RouteID":' + routeId + '}',
            dataType: "json",
            //contentType: 'application/json; charset=utf-8',
            beforeSend: function () {

            },
            success: function (result) {
                if (result.sequenceNo > 0) {
                    for (var i = 0; i < selectedStructList.length; i++) {
                        var esrn = selectedStructList[i].ESRN;
                        var status = "#status_" + esrn + "_" + routeId;
                        $(status).removeClass("completedimg");
                        $(status).addClass("inprogressimg");
                        $(status).attr("title", "Assessment Status : In Progress " + '\n' + "Sequence Number : " + result.sequenceNo);
                    }
                    stopAnimation();
                    UnselectAllStructures();
                    ShowInfoPopup('The selected structures have been sent for assessment. The sequence number is ' + result.sequenceNo);
                }
                else {
                    stopAnimation();
                    ShowDialogWarningPop('The selected structures cannot be sent for assessment ', 'Ok', '', 'WarningCancelBtn', '', 1, 'warning');
                }
            },
            error: function () {
                //location.reload();
            },
            complete: function () {
                // stopAnimation();
                //showWarningPopDialog('Strcuture Assessment Completed ', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
            }
        });

    }
    function cancelPerformAssessment() {
        UnselectAllStructures();
        WarningCancelBtn();
    }

    function ShowNoteToHaulier(Note, structureCode) {
        //resetdialogue();
        startAnimation();
        $.ajax({
            type: 'POST',
            url: '../RouteAssessment/ViewNoteToHaulier',
            //contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({
                Note: Note, structureCode: structureCode
            }),
            beforeSend: function (xhr) {
            },
            success: function (result) {
                $('#generalPopupContent').html(result);
                //$('#generalPopup').modal(options);
                $(".modal-content").css({ "width": "32rem"});
            },
            error: function (xhr, textStatus, errorThrown) {
                location.reload();
            },
            complete: function () {
                stopAnimation();
                $('#generalPopup').modal('show');
                $('.loading').hide();
            }
        })
    }

    function ViewAssessmentComment(comment, structureCode) {
        resetdialogue();
        startAnimation();
        $.ajax({
            type: 'POST',
            url: '../RouteAssessment/ViewAssessmentComment',
            //contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({
                comment: comment, structureCode: structureCode
            }),
            beforeSend: function (xhr) {
            },
            success: function (result) {
                $('#generalPopupContent').html(result);
               // $('#generalPopup').modal(options);
                $(".modal-content").css({ "width": "32rem" });
            },
            error: function (xhr, textStatus, errorThrown) {
                location.reload();
            },
            complete: function () {
                stopAnimation();
                $('#generalPopup').modal('show');
                $('.loading').hide();
            }
        })
    }

    function DisplayStructureGENT(StructCode, routeId, sectionId) {

        $('#hfRouteID').val(routeId);
          //  var isAuthMove = @ViewBag.IsAuthMove;
        //if (@ViewBag.IsAuthMove== "False") {
            //isAuthMove = false;
        //}

        ShowStructuredetailspopup(StructCode);
        stopAnimation();
    }
    function ClearAffectedStructure(){
        document.getElementById('Structs').checked = false;
        document.getElementById('Underbridge').checked = false;
        document.getElementById('Overbridge').checked = false;
        document.getElementById('LevelCrossing').checked = false;
        displayStructure();
    }
    function displayStructure() {
        var _iscandidatert = $('#IsCandidateRT').val();
		var routpartid = $('#routeId').val();
		var analysisId = $('#AnalysisID').val();
		var Org_ID = $('#Organisation_ID').val() ? $('#Organisation_ID').val() : 0;
		if (iscandidatert == 'True') {
			revisionId = $('#revisionId').val();
			AnalysisId = $('#candAnalysisId').val();
		}
		if (SortStatus == "MoveVer") {
			AnalysisId = $('#analysis_id').val();
			revisionId = $('#revisionId').val();
		}
        if ($('#Structs').is(':checked')) {
            startAnimation();           
            $('#AffectedStructure_xslt').load('../RouteAssessment/GetAffectedStructures?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&IsCandidate=' + _iscandidatert + '&FilterByOrgID=' + Org_ID,
               // $('#AffectedStructure_xslt').load('../RouteAssessment/ListAffectedStructures?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&IsCandidate=' + _iscandidatert + '&FilterByOrgID=' + Org_ID,
                {},
                function () {
                    document.getElementById('Structs').checked = true;
                    document.getElementById('Underbridge').checked = true;
                    document.getElementById('Overbridge').checked = true;
                    document.getElementById('LevelCrossing').checked = true;
                    stopAnimation();
                });
			//stopAnimation();
		}
		else if ($('#Overbridge').is(':checked')) {
			startAnimation();
			$('#AffectedStructure_xslt').load('../RouteAssessment/ListOverBridgeStructures?analysisID=' + AnalysisId + '&IsCandidate=' + _iscandidatert + '&OrganisationID=' + Org_ID,
				{},
                function () {
                    document.getElementById('Overbridge').checked = true;
                    if ($('#routediv1 td').length > 0) {
                        $('#divNoAffected').hide();
                    }
                    else {
                        $('#divNoAffected').show();
                    }
					stopAnimation();
				});
		}
		else if ($('#Underbridge').is(':checked')) {
			startAnimation();
			$('#AffectedStructure_xslt').load('../RouteAssessment/ListUnderBridgeStructures?analysisID=' + AnalysisId + '&IsCandidate=' + _iscandidatert + '&OrganisationID=' + Org_ID,
				{},
                function () {
                    document.getElementById('Underbridge').checked = true;
                    if ($('#routediv1 td').length > 0) {
                        $('#divNoAffected').hide();
                    }
                    else {
                        $('#divNoAffected').show();
                    }
					stopAnimation();
				});
		}
		else if ($('#LevelCrossing').is(':checked')) {
			startAnimation();
			$('#AffectedStructure_xslt').load('../RouteAssessment/ListLevelCrossingStructures?analysisID=' + AnalysisId + '&IsCandidate=' + _iscandidatert + '&OrganisationID=' + Org_ID,
				{},
                function () {
                    document.getElementById('LevelCrossing').checked = true;
                    if ($('#routediv1 td').length > 0) {
                        $('#divNoAffected').hide();
                    }
                    else {
                        $('#divNoAffected').show();
                    }
                    stopAnimation();
                });

        }
        else {
            startAnimation();
            $('#AffectedStructure_xslt').load('../RouteAssessment/GetAffectedStructures?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&IsCandidate=' + iscandidatert + '&ContentRefNo=' + contentRefNo + '&versionId=' + versionid + '&IsVR1=' + isVR1Application + '&FilterByOrgID=' + Org_ID,
               // $('#affectedstructure').load('../RouteAssessment/GetAffectedStructures?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&IsCandidate=' + iscandidatert + '&ContentRefNo=' + contentRefNo + '&versionId=' + versionid + '&IsVR1=' + isVR1Application + '&FilterByOrgID=' + Org_ID,
                {},
                function () {
                    document.getElementById('Structs').checked = false;
                    document.getElementById('Underbridge').checked = false;
                    document.getElementById('Overbridge').checked = false;
                    document.getElementById('LevelCrossing').checked = false;
                    stopAnimation();
                });
            
        }
        closeFilters();
	}


        //function CheckICA(structureCode, sectionId, orgId, routePartId) {
        //    $.ajax({
        //        url: '../RouteAssessment/DetailedICA',
        //        type: 'POST',
        //        async: false,
        //        data: { structureCode: structureCode, sectionId: sectionId, orgId: orgId, routePartId: routePartId },
        //        beforeSend: function () {
        //            $("#overlay").show();
        //            $('.loading').show();
        //        },
        //        success: function (result) {
        //            $("#dialogue").html(result);
        //            $("#dialogue").show();
        //            $("#overlay").removeAttr("style");
        //            removescroll();
        //        },
        //        error: function () {
        //            location.reload();
        //        },
        //        complete: function () {
        //            $("#overlay").hide();
        //            $("#overlay").removeAttr("style");
        //            $('.loading').hide();
        //        }
        //    });
        //}

