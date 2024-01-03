
    $(document).ready(function () {

        $("#btnDelete").on('click', DeleteSpecialOrder_Click);
        $("#btnGenerateDoc").on('click', GenrateDoc);

    });

    //-----------------$(document).ready(function () { EXTERNAL SCRIPT START }----------------------
    var hauliermnemonic = $('#hauliermnemonic').val();
    var esdalref = $('#sedalNo').val(); Plannerid
    var PlannerID = $('#Plannerid').val();
	var projstatus = $('#SProjstatus').val();
    var revisionno = $('#revisionno').val();
    var versionno = $('#versionno').val();
    var versionId = $('#versionId').val();
    var revisionId = $('#revisionId').val();
    var VR1Applciation = $('#VR1Applciation').val();
    var ApprevId = $('#ApprevId').val();
    var projectid = $('#projectid').val();
    var status = $('#SortStatus').val();
    var movementId = $('#movementId').val();
    var analysis_id = $('#analysis_id').val();
    var Pageflag = $('#Pageflag').val();
    var OrgID = $('#OrganisationId').val();//3393;//
    var Notification_Code = $('#Notification_Code').val();
    var esdal_history = $('#esdal_history').val();
    var reduceddetailed = $('#Reduceddetailed').val();
    var MovLatestVer = $('#MovLatestVer').val();
    var cloneapprevid = $('#cloneapprevid').val();
    var AppEdit = $('#AppEdit').val();
    var Owner = $('#Owner').val();
    var project_status = $('#Proj_Status').val();
    var collab = false;
    var flag;
    var _projectstatus = "";
    var _checkerid = "";
    var _checkerstatus = "";
    jQuery(document).ready(function () {
        var name = $('#Msg').val();
        var saveFlagSOGeneral = $('#saveFlagSOGeneral').val();
        var tabStatus = $('#tabStatus').val();
        var chk_status  = $('#hf_CheckingStatus').val(); 
        var sort_user_id  = $('#hf_SortUserId').val(); 
        var checker_id  = $('#hf_Checkerid').val(); 
        var proj_status = $('#SProjstatus').val() ? $('#SProjstatus').val() : 0;
        if ((chk_status == 301002 || chk_status == 301008 || chk_status == 301005) && (sort_user_id != checker_id)) {
            $('#btnEdit').hide();
            $('#btnDelete').hide();
        }
        if (chk_status == 301006) {
            $('#btnEdit').hide();
            $('#btnDelete').hide();
        }
        if (status == "ViewProj") {
            $('#showwhyso').hide();
            $('#validationSucced').hide();
            DisplayProjView();
        }
        else if (status == "Revisions") {
            $('#validationSucced').hide();
            $('#showwhyso').hide();
        }
        else if (status == "CreateSO") {
            if (AppEdit == "True") {
            } else {
                ShowSOHaulierOrgPage();
                if (name != "") {
                    showWarningPopDialog('Organisation "' + name + '" saved successfully.', 'Ok', '', 'CloseOrgSavePopup', '', 1, 'info');
                }
                else {
                    if (revisionId == 0) {
                        $(".t[id=2], .t[id=3], .t[id=4]").unbind("click");
                        $(".t[id=2], .t[id=3], .t[id=4]").removeClass('t');
                    }
                }
            }
        }
        else if (status == "CreateVR1") {
            if (AppEdit == "True") {
            } else {
                ShowSOHaulierOrgPage();
                if (name != "") {
                    showWarningPopDialog('Organisation "' + name + '" saved successfully.', 'Ok', '', 'CloseOrgSavePopup', '', 1, 'info');
                }
                else {
                    if (OrgID == 0) {
                        $(".t[id=2], .t[id=3]").unbind("click");
                        $(".t[id=2], .t[id=3]").removeClass('t');
                    }
                }
            }
        }
        $(".t[id=0]").click(function () {
            DisplayProjView();
            $('#showwhyso').hide();
            $('#showwhyvr1').hide();
            $('#validationSucced').hide();
        });




        $(".t[id=1]").click(function () {
            $('#pageheader').find('h3').text(title + ' - ' + $("li[id=1]").find('.tab_centre').text());
            if (status == "ViewProj" || status == "MoveVer") {
            } else if (status == "Revisions") {
            }
            else {
                if (AppEdit == "True") {
                    $('#validationSucced').show();
                    ShowSOHaulierOrgPage();
                } else {
                    if ($("#OrgName").val() != "" || $("#OrgName").val() != null) {

                        $("#HaulOrg").show();
                        if (revisionId == 0) {
                            $("#leftpanel").html('');
                            $("#leftpanel").hide();
                            $("#leftpanel").load('../SORTApplication/SORTLeftPanel', { Display: "HaulOrg", pageflag: 2 },
                           function () {
                               $('#leftpanel_quickmenu').html('');
                               $("#leftpanel").show();
                           });
                        } else {
                            $("#leftpanel").html('');
                        }
                    }
                    else {
                        ShowSOHaulierOrgPage();
                    }
                }
            }

            if ($("#hdnApplicationStatus").val() == 308001) {
                if (status == "CreateSO") {
                    SOvalidationfun1();
                }
                else if (status == "CreateVR1") {
                    VR1validationfun1();
                }
            }
        });


        $(".t[id=2]").click(function () {
            $('#pageheader').find('h3').text(title + ' - ' + $("li[id=2]").find('.tab_centre').text());
            if (status == "ViewProj" || status == "MoveVer") {
            } else if (status == "Revisions") {
            }
            else {
                if ($("#General input").length > 0) {
                    $("#leftpanel").hide();
                    $('#leftpanel_div').hide();
                    $('#leftpanel').hide()
                    $("#General").show();
                }
                else {
                    if (status == "CreateSO") {
                        ShowSOGeneralPage();
                    }
                    else if (status == "CreateVR1") {
                        ShowVR1GeneralPage();
                    }
                }
            }
            if (AppEdit == "True") {
                $('#validationSucced').show();
            }
        });
        //-----------------$(document).ready(function () { EXTERNAL SCRIPT END }----------------------


        var pstatus = $('#SProjectStatus').val();
        var cstatus = $('#SChecingStatus').val();
        var plannerid = $('#Plannerid').val();

        var randomNum=Math.random();
        
        jQuery('#back-link').click(function () {  
            var crntUrl = $('#hf_CurrentUrl').val(); 
            crntUrl=crntUrl.replace(/\%/g, '&');
            window.location=crntUrl;
        });
        $('#btnEdit').click(function () {

			var soNumber = $('#SoNumber').val();
            var versionId = $('#VersionId').val();
			var esdalrefno = $('#sedalNo').val();
			var projectid = $('#ProjectId').val();
			var _plannerId = $('#Plannerid').val() ? $('#Plannerid').val() : 0;
			var _project_status = $('#SProjstatus').val() ? $('#SProjstatus').val():0;
            window.location = "/SORTApplication/CreateSpecialOrder" + EncodedQueryString("sedalno=" + esdalrefno + "&ProjectId=" + projectid + "&VersionId=" + versionId + "&SONumber=" + soNumber + '&PlannerId=' + _plannerId + '&ProjectStatus=' + _project_status);
        });
    });

    function GenrateDoc(){
        var doctype='@Model.Template';
        var esdalRef=$('#Concate').val();
        var SOnumber = '@Model.SONumber';
        var doc="SO"+doctype;
        if(doctype!=""){
            window.location.href = '../SORTApplication/GenrateDocuments' + EncodedQueryString('esdalRef=' + esdalRef + '&SOnumber=' + SOnumber + '&doctype=' + doc);
        }else{
            showWarningPopDialog('Document type is not there for this special order.', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
        }       
    }


    function DistributeToHaulier() {
        $('#form_distribute_haul').submit();
    }

    function DistributeHaulStatus(result) {
        if (result) {
            $('.tr_distribute_to_haul').remove();
            showWarningPopDialog('Special order distributed to haulier successfully.', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
        }
        else {
            showWarningPopDialog('Distribution is not completed, Please try again later.', 'Ok', '', 'WarningCancelBtn', '', 1, 'error');
        }
    }
    //Delete Special order
    function DeleteSpecialOrder_Click(e)
    {

		var orderno = $('#SoNumber').val();
		var esdalref = $('#sedalNo').val();

        if(orderno!=null||orderno!="")
        {
            $.post('/SORTApplication/DeleteSpecialOrder?OrderNo=' + orderno + '&EsdalRef=' + esdalref, function (data) {
                if(data==true)
                {
                    ShowSuccessModalPopup('The Special order "' + orderno + '" successfully deleted.',  'RedirectProjectOverview');
                }
                else
                {
                    ShowWarningPopup('The Special order "' + orderno + '" is not deleted successfully, Please try again.', 'Ok','WarningCancelBtn');
                }
            });
        }
    }
    function RedirectProjectOverview()
    {
        var crntUrl = $('#hf_CurrentUrl').val(); 
        crntUrl=crntUrl.replace(/\%/g, '&');
        window.location=crntUrl;
    }
