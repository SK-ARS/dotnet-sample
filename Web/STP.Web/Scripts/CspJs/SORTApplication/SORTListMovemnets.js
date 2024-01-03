    let CandidateBackFlag = 0;
    let CandidateBackSubFlag = 0;
    SelectMenu(2);
    $("#sort-menu-list .card").click(function () {
        $("#sort-menu-list .active-card").each(function () {
            $(this).removeClass('active-card');
        });
        $(this).addClass('active-card');
    });
    $(document).ready(function () {
       
        $("#showwhyso").on('click', Applicationsovalidation);
        $("#showwhyvr1").on('click', Applicationvr1validation);
        $("#displaypop").on('click', displayPopupList);

    });
    function allocate() {
        document.getElementById('allocate').style.display = "block"
        document.getElementById('active-button').style.display = "block"
        document.getElementById('active-button').style.background = "#275795"
        document.getElementById('active-button').style.color = "white"
        document.getElementById('inactive-button').style.display = "none"
    }
    function removeallocate() {
        document.getElementById('allocate').style.display = "none"
        document.getElementById('active-button').style.display = "none"
        document.getElementById('inactive-button').style.display = "block"
    }

    var frmHaul = false;
    var toHaul = false;
    var distbtn = false;
    var haulReq = false;
    var cntDetails = false;
    //-----------------$(document).ready(function () { EXTERNAL SCRIPT START }----------------------
    var hauliermnemonic = $('#hauliermnemonic').val();
    var esdalref = $('#esdalref').val();
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
    var Checker = $('#Checker').val();
    var project_status = $('#Proj_Status').val();
    var collab = false;
    var flag;
    var _projectstatus = "";
    var _checkerid = "";
    var _checkerstatus = "";
    var PlannerId = $('#PlannrUserId').val();
    var Latest_SpeOrder = $('#LatestSpeOrder').val();
    var Enter_BY_SORT = $('#EnterBySort').val();

    $(document).ready(function () {
        $("#showcandidate").on('click', Show_CandidateRTVehicles);
        $("#btn-library").on('click', gotoRouteLibrary);
        $("#back-struct").on('click', BackToStruRelatedMov);
        $("#back-move").on('click', BackToMovDetails);
        $("#import-view").on('click', ImportRFromView);
        $("#can-vehicle").on('click', CandidateVehicleBackButton);
        $("#back-to-candid").on('click', BackToCandiRoutDetails);
        $("#goto-lib").on('click', gotoRouteLibrary);
        $("#route-lib").on('click', gotoRouteLibrary);
        $("#show-candidate").on('click', Show_CandidateRTVehicles);
        $("#btn-CreateNewVehicleConfiguration").on('click', CreateNewVehicleConfiguration);
        $("#btn-CreateNewVehicleConfiguration").on('click', CreateNewVehicleConfiguration);
        var _pstatus = $('#AppStatusCode').val();
        if (_pstatus == 307011 && PlannerId == '@ViewBag.SortUserId' && Enter_BY_SORT == 0) {

            //showWarningPopDialog('Application is revised, Please click on Acknowledge button.', 'Ok', '', 'load_data', '', 1, 'info');
            ShowDialogWarningPop('Application is revised, Please click on Acknowledge button.', 'Ok', '', 'load_data', '', 1, 'info');
        }
        else
            load_data();
    });
    $('#EmailId').keypress(function () {
        $('#emailFaxValid1').text('');
    });
    $('#Fax').keypress(function () {
        $('#emailFaxValid1').text('');
    });
    function Show_Advanced() {
        //$('#div_advanced_filter').show();
        if ($('#div_advanced_filter').is(':visible')) {
            $('#div_advanced_filter').slideUp('slow');
        }
        else {
            $('#div_advanced_filter').show('fast', function () {
                $("html,body").animate({ scrollTop: $(document).height() }, 1000);
            });
        }
    }

    function FillNormalFilter() {
        $('#SearchPrevMoveVeh').val("Yes");
        $('#so_movement_filter').find('input:text').each(function () {
            var id = $(this).attr('id');
            var value = $(this).val();

            $('#div_normal_hidden').append('<input type="hidden" id="' + id + '" name="' + id + '" value="' + value + '"></hidden>');
        });

        $('#so_movement_filter').find('input:checkbox').each(function () {
            var id = $(this).attr('id');
            var value = $(this).is(':checked');

            $('#div_normal_hidden').append('<input type="hidden" id="' + id + '" name="' + id + '" value="' + value + '"></hidden>');
        });

    }
    function ClearAdvancedSORT() {
        ClearAdvancedDataSORT();
        ResetDataSORT();
        $("#frmFilterMoveInboxSORT").submit();

        return false;
    }
    function ClearAdvancedDataSORT() {
        var _advInboxFilter = $('#div_sort_adv_search');
        _advInboxFilter.find('input:text').each(function () {
            $(this).val('');
        });

        _advInboxFilter.find('select').each(function () {
            $(this).val('0');
        });
    }

    function ResetDataSORT() {
        $.ajax({
            url: '../SORTApplication/ClearInboxAdvancedFilterSORT',
            type: 'POST',
            success: function (data) {
                //ClearAdvancedData();
            }
        });
    }
    function closeViewCollab() {
        $('#CollabHistoryDiv').modal('hide');
        $('#overlay').hide();
        $('#dialogue').hide();
        $(".modal-backdrop").removeClass("show");
        $(".modal-backdrop").removeClass("modal-backdrop");
        DisplayCollabStatus();
        //$('td').removeClass('rowEvenNew');
        //$('span').removeClass('rowEvenNew');
        //$('p').removeClass('rowEvenNew');
        //$('h3').removeClass('rowEvenNew');
    }

    function CreateNewVehicleConfiguration() {

        $.ajax({
            url: '../VehicleConfig/CreateVehicle',
            type: 'GET',
            cache: false,
            async: false,
            data: { isCandidate: true },
            beforeSend: function () {
                startAnimation();
            },
            success: function (response) {
                $('#haulier_details_section').hide();
                $('#select_vehicle_section').hide();
                $('#vehicle_details_section').hide();
                $('#movement_type_confirmation').hide();
                $('#select_route_section').hide();
                $('#route').html('');
                $('#route').hide();
                $('#route_vehicle_assign_section').hide();
                $('#supplimentary_info_section').hide();
                $('#overview_info_section').hide();
                $('#vehicle_edit_section').hide();
                $('#vehicle_Component_edit_section').hide();

                SubStepFlag = 1.3;
                CandidateBackFlag = 0;
                CandidateBackSubFlag = 0;
                var result = $(response).find('#vehicles');
                $('#tab_2').html('');
                $('#tab_2').html('<div id="vehicle_Create_section"></div>');
                $('#tab_2 #vehicle_Create_section').html($(response).find('#vehicles'));
                $('#tab_2').show();
                $('.new-vehicle').unwrap('#banner-container');
                $('.new-vehicle').attr("style", "padding-left:0px !important");
                $('#tab_2').append('<div id="divbtn_candidateVehicleCreate" class="row mt-4" style="float: right;"><button class="btn outline-btn-primary SOAButtonHelper mr-2 mb-2" id="can-vehicle" >BACK</button></div>');

                var scollIcon = $(response).find('div#scroll-btns');
                $(scollIcon).insertAfter('.new-vehicle');
                LoadSelectVehicleComponentV1();
                document.getElementById("scoll-right-btn").addEventListener("click", function () {
                    $('#widgetsContent').animate({ scrollLeft: "+=300px" }, "medium");
                });

                document.getElementById("scoll-left-btn").addEventListener("click", function () {
                    $('#widgetsContent').animate({ scrollLeft: "-=300px" }, "medium");
                });
            },
            complete: function () {
                stopAnimation();
            }
        });
    }


    function load_data() {
        WarningCancelBtn();
        //filtermenu_load();
        $('#revisionno').val('@ViewBag.revisionno');
        if (VR1Applciation == 'False') { $("#URLKEY").val("SO"); }
        else { $("#URLKEY").val("VR1"); }
        var name = $('#Msg').val();
        var saveFlagSOGeneral = $('#saveFlagSOGeneral').val();
        var tabStatus = $('#tabStatus').val();
        $('#ApplicationrevId').val('@ViewBag.apprevid');
        //$('#ApplicationrevId').val("10");
        var AppEdit = $('#AppEdit').val();
        var status = $('#SortStatus').val();
        var VR1App = $('#VR1Applciation').val();
        var AppRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;

        if (status == "ViewProj") {
            $('#validationSucced').hide();
            $('#showwhyso').hide();
            DisplayProjView();
        }
        else if (status == "Revisions") {
            $('#validationSucced').hide();
            $('#showwhyso').hide();
            $('#9').hide();
        }
        else if (status == "CreateSO") {
            $("#URLKEY").val("SO");
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
            $("#URLKEY").val("VR1");
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
        else if (status == "MoveVer") {
        }

        $(".t[id=0]").click(function () {
            DisplayProjView();
            $('#showwhyso').hide();
            $('#showwhyvr1').hide();
            $('#validationSucced').hide();
        });


        $(".t[id=1]").click(function ()
        {
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


        //$(".t[id=2]").click(function () {
        //    $('#pageheader').find('h3').text(title + ' - ' + $("li[id=2]").find('.tab_centre').text());
        //    if (status == "ViewProj" || status == "MoveVer") {
        //    } else if (status == "Revisions") {
        //    }
        //    else {
        //        if ($("#General input").length > 0) {
        //            $("#leftpanel").hide();
        //            $('#leftpanel_div').hide();
        //            $('#leftpanel').hide()
        //            if (($("#EmailIdSORT").val() != undefined) && ($("#hdnEmailId").val() != $("#EmailIdSORT").val())) {
        //                    $('#BtnSubmit').hide();
        //                }
        //            if (($("#EmailIdSORT").val() != undefined) && ($("#hdnFax").val() != $("#FaxSORT").val())) {
        //                    $('#BtnSubmit').hide();
        //                }
        //            $("#General").show();
        //        }
        //        else {
        //            if (status == "CreateSO") {
        //                ShowSOGeneralPage();
        //            }
        //            else if (status == "CreateVR1") {
        //                ShowVR1GeneralPage();
        //            }
        //        }
        //    }
        //    if (AppEdit == "True") {
        //        $('#validationSucced').show();
        //    }
        //});
        //-----------------$(document).ready(function () { EXTERNAL SCRIPT END }----------------------


        //createContextMenu();
        $('#tab_3').html('');
        $('#leftpanel').html('');


        if (AppRevId != 0 && '@ViewBag.VR1Applciation' == 'False') {

            // if (status == "CreateSO")// && status != "MoveVer" && status != "Revisions"
            if (AppEdit == 'True') {
                $(".tab_content1").each(function () {
                    $(this).hide();
                });
                //$('.t').addClass('nonactive');

                $('#tab_2').show();
                //$('#tab_wrapper ul li').eq(1).removeClass('nonactive');
                var test = title + '- ' + $('#tab_wrapper ul li').eq(1).find('.tab_centre').text();
                $('#pageheader').find('h3').text(test);

                $('#HaulOrg').hide();
                ShowSOHaulierOrgPage();
            }
            if (status == "CreateSO") {
                SOvalidationfun1();
            }
        } else {
            if (status == "CreateVR1" && ApprevId != 0) {
                if (AppEdit == 'True') {
                    $(".tab_content1").each(function () {
                        $(this).hide();
                    });
                    //$('.t').addClass('nonactive');

                    $('#tab_2').show();
                    //$('#tab_wrapper ul li').eq(1).removeClass('nonactive');
                    var test = title + '- ' + $('#tab_wrapper ul li').eq(1).find('.tab_centre').text();

                    $('#pageheader').find('h3').text(test);

                    $('#HaulOrg').hide();
                    ShowSOHaulierOrgPage();
                }
                if (status == "CreateVR1") {
                    VR1validationfun1();
                }
            }
        }

        //---------------------
        $('#txtSORTApp_Notes_To_HA').html($('#hdnSORTApp_Notes_To_HA').val());


        //-------------------------


        if (status == "Revisions") {

            var revision_no = $('#arev_no').val();
            var _appstatus = $('#AppStatusCode').val();
            var _checkingstatus = $('#CheckerStatus').val();
            var _vr1number = $('#Vr1Number').val();
            var isvr1app = $('#VR1Applciation').val();

            $(".tab_content1").each(function () {
                $(this).hide();
            });
            //$('.t').addClass('nonactive');

            $('#tab_2').show();
            //$('#tab_wrapper ul li').eq(1).removeClass('nonactive');
            var test = title + '- ' + $('#tab_wrapper ul li').eq(1).find('.tab_centre').text();
            $('#pageheader').find('h3').text(test);

            $('#HaulOrg').hide();

            $("#leftpanel").hide();
            $('#leftpanel_div').hide();
            $('#leftpanel').hide;
            var PlannerId = $('#PlannrUserId').val();
            if (VR1Applciation == "True") {
                var revision_id = $('#arev_Id').val();
                var Pageflag = $('#Pageflag').val();
                var projectid = $('#projectid').val();
                var Enter_BY_SORT = $('#EnterBySort').val();
                var Lat_revno = $('#arev_no').val();
                startAnimation()
               // $('#General').load('../Application/VR1GeneralDetails', { applicationrevid: AppRevId, hideflag: true },
                $('#General').load('../Application/ApplicationDetails', { appRevisionId: revision_id, appVersionId: versionId },
                   function () {

                       $('#General').show();
                       $('#General').find('#revise').hide();
                       $('#General').find('#notify').hide();
                       $('#General').find('#clone').hide();
                       $('#General').find('#tbl_foldermgmt').hide();

                       $("#leftpanel").html('');
                       $("#leftpanel").hide();
                       $('#leftpanel').load('../SORTApplication/SORTLeftPanel?Display=VR1Approval' + '&pageflag=' + Pageflag + '&AppStatus=' + _appstatus + '&CheckingStatus=' + _checkingstatus + '&Vr1Number=' + _vr1number + '&Latest_Rev_No=' + Lat_revno + '&VR1App=' + isvr1app + '&PlannerId=' + PlannerId + '&Project_ID=' + projectid + '&Rev_ID=' + revision_id + '&Enter_BY_SORT=' + Enter_BY_SORT, function () {
                           $("#leftpanel").show();
                       });
                       stopAnimation()
                       CheckSessionTimeOut();
                   });
            } else {
                var revision_id = $('#arev_Id').val();
                var Pageflag = $('#Pageflag').val();
                var projectid = $('#projectid').val();
                var sortentered = $('#EnteredBySort').val();
                startAnimation()
                $("#leftpanel").html('');
                $('#General').load('../Application/ApplicationDetails', { appRevisionId: revision_id },
                   function () {

                       $('#General').show();
                       $("#leftpanel").html('');
                       $("#leftpanel").hide();
                       if ($('#ViewFlag').val() == 0) {
                           $('#leftpanel').load('../SORTApplication/SORTLeftPanel?Display=ApplSumm&pageflag=' + Pageflag + '&PlannerId=' + PlannerId + '&Project_ID=' + projectid + '&Rev_ID=' + revision_id + '&Enter_BY_SORT=' + sortentered, function () {
                               $("#leftpanel").show();
                           });
                       }
                       stopAnimation()
                       CheckSessionTimeOut();
                   });
            }
if($('#hf_ViewFlag').val() ==  1) {
                $('#1').hide();
                $('#menu').hide();
                $('#button1').hide();
                $('#button2').hide();
                $('#button3').hide();
            }
        }
        if (status == "MoveVer") {
            $(".tab_content1").each(function () {
                $(this).hide();
            });
            //$('.t').addClass('nonactive');

            $('#tab_2').show();
            //$('#tab_wrapper ul li').eq(1).removeClass('nonactive');
            var test = title + '- ' + $('#tab_wrapper ul li').eq(1).find('.tab_centre').text();
            $('#pageheader').find('h3').text(test);

            $('#HaulOrg').hide();
            $("#leftpanel").html();
            $('#leftpanel_div').html();
            $('#leftpanel').html()
            $('#validationSucced').hide();
            var MovLatestVer = $('#MovLatestVer').val();
            var projectid = $('#projectid').val();
            var versionno = $('#versionno').val();
            var btn_show = false;
            if (MovLatestVer == versionno) {
                btn_show = true;
            }

            if (VR1Applciation == "True") {
                var status = $('#SortStatus').val();
                $("#leftpanel").hide();
                $('#leftpanel_div').hide();
                $('#leftpanel').hide()
                startAnimation()
                $('#General').load('../../SORTApplication/SORTApplicationSummary', { Status: status, RevisionId: revisionId, versionId: versionId, Project_id: projectid, Mov_btn_show: btn_show, VR1App: VR1Applciation, Organisation_ID: OrgID },
                        function () {
                            $('#HaulOrg').hide();
                            $('#pageheader').find('h3').text(test);
                            //if (MovLatestVer == versionno) {
                            //} else {
                            //    $('#Mov_Agree').hide();
                            //    $('#Mov_Unagree').hide();
                            //    $('#Mov_Withdraw').hide();
                            //}
                            $('#General').show();
                            stopAnimation()
                            CheckSessionTimeOut();
                        });

            } else {
                $("#leftpanel").hide();
                $('#leftpanel_div').hide();
                $('#leftpanel').hide();
                startAnimation();
                $('#General').load('../../SORTApplication/SORTApplicationSummary', { Status: status, RevisionId: revisionId, versionId: versionId, Project_id: projectid, Mov_btn_show: btn_show, Organisation_ID: OrgID },
                        function () {
                            $('#HaulOrg').hide();
                            $('#pageheader').find('h3').text(test);
                            $('#General').show();
                            stopAnimation()
                            CheckSessionTimeOut();
                        });
            }
if($('#hf_ViewFlag').val() ==  1) {
                $('#1').hide();
                $('#5').hide();
                $('#6').hide();
                $('#7').hide();
                $('#8').hide();
                $('#9').hide();
                $('#menu').hide();
                $('#button1').hide();
                $('#button2').hide();
                $('#button3').hide();
            }

        }
        else if (status == "CandidateRT") {
            _checkerstatus = $('#CheckerStatus').val();
            _checkerid = $('#CheckerId').val();
            $('#9').hide();
            $('#8').hide();
            $(".tab_content1").each(function () {
                $(this).hide();
            });
            $('#validationSucced').hide();


            //$('.t[id=1]').addClass('nonactive');

            ViewProjectGeneralDetails();
            //$(".t[id=4]").on("click", function (e) {
            //    e.stopImmediatePropagation();
            //});
            //$(".t[id=2]").on("click", function (e) {
            //    e.stopImmediatePropagation();
            //});
            $('#isSORTList').val(1);
if($('#hf_ViewFlag').val() ==  1) {
                $('#1').hide();
                $('#cand_rtanalysis').hide();
                $('#menu').hide();
                $('#button1').hide();
                $('#button2').hide();
                $('#button3').hide();
            }
        }
        //-------------------------


        @* var vr1 = $('#vr1appln').val();

        if (AppRevId != 0 && '@Model.ApplicationStatus' == 308001) {
             if (vr1 == 'True') {
                 VR1validationfun1('@ViewBag.reduceddetailed');
                }
                else {
                    SOvalidationfun1();
                }
            }*@

        $('#btnNextSORTGeneral').live("click", function () {
            //if ($("#OrgName").val().length > 0) {
            if (AppRevId != 0) {
                ViewGeneralTabAuto();
            } else {
                if ($("#spnOrgName").text() != "") {
                    if (($("#ddlHaulierContactName option:selected").index() != 0 && $("#ddlHaulierContactName option:selected").index() != -1) || $("#HaulierContactNameExist").val().length > 0) {

                        $("#err_ddl_ContactName").html("");
                        $("#err_select_existing_haulier").html("");
                        if ($("#EmailIdSORT").val().length > 0 || $("#FaxSORT").val().length > 0) {
                            $("#emailFaxValid").html("");
                            ViewGeneralTabAuto();
                        }
                        else {
                            $("#emailFaxValid").html("Email/Fax is required");
                        }
                        //ViewGeneralTabAuto();
                    }
                    else {
                        $("#err_ddl_ContactName").html("Haulier contact name is required");
                    }

                }
                else {
                    $("#err_select_existing_haulier").html("Please select the organisation");
                }
            }

        })

        $('#btnNextSORTGeneralSave').live("click", function () {
            var _form = $("form[id='form_Organisation']");
            var validator = $("form[id='form_Organisation']").validate(); // obtain validator
            var anyError = false;
            _form.find("input").each(function () {
                if (!validator.element(this)) { // validate every input element inside this step
                    anyError = true;
                }
            });
            if ($("#EmailId").val() == "" && $("#Fax").val() == "") {
                $("#emailFaxValid1").html("Email/Fax is required");
                anyError = true;
            }

            if (anyError)
                return false;
            else {
                ViewGeneralTabAuto();
            }
        });

        $('#pageheader').find('h3').text(title + ' - ' + $("li[id=1]").find('.tab_centre').text());

        $('#tab_10').hide();

    }




    function ViewGeneralTabAuto() {
        //$("li").each(function () {
        //    $(this).addClass("nonactive");

        //});
        $(".tab_content1").each(function () {
            $(this).hide();
        });
        //id = $(this).attr("id");
        //$("#tab_" + 2).show();

        $(".t[id=3], .t[id=4]").unbind("click");
        $(".t[id=3], .t[id=4]").removeClass('t');

        //$("li[id=2]").removeClass("nonactive");
        $("li[id=2]").addClass('t');

        //Code to Activate SORT SO General page
        $("li[id=2]").bind("click", function () {
            //$("li").each(function () {
            //    $(this).addClass("nonactive");
            //});
            $(".tab_content1").each(function () {
                $(this).hide();
            });
            //$(this).removeClass("nonactive");
            $(this).addClass('t');
            $('#pageheader').find('h3').text(title + ' - ' + $("li[id=2]").find('.tab_centre').text());


            //ShowSOGeneralPage();
            if ($("#General input").length > 0) {
                $("#leftpanel").hide();
                $('#leftpanel_div').hide();
                $('#leftpanel').hide()
                $("#emailFaxValid2").html("");
                $("#General").show();
            } else {
                if (status == "CreateSO") {
                    ShowSOGeneralPage();
                }
                else if (status == "CreateVR1") {
                    ShowVR1GeneralPage();
                }
            }


            scrolltotop();
            if ($("#hdnApplicationStatus").val() == 308001) {
                if (status == "CreateSO") {
                    SOvalidationfun1();
                }
                else if (status == "CreateVR1") {
                    VR1validationfun1();
                }
            }
        });
        //========================================

        $('#pageheader').find('h3').text(title + ' - ' + $("li[id=2]").find('.tab_centre').text());


        //ShowSOGeneralPage();
        if ($("#General input").length > 0) {
            if (($("#EmailIdSORT").val() != undefined || $("#EmailIdSORT").val() != "") && ($("#hdnEmailId").val() != $("#EmailIdSORT").val())) {
                $('#BtnSubmit').hide();
            }
            if (($("#EmailIdSORT").val() != undefined || $("#EmailIdSORT").val() != "") && ($("#hdnFax").val() != $("#FaxSORT").val())) {
                $('#BtnSubmit').hide();
            }
            $("#emailFaxValid2").html("");
            $("#General").show();
        } else {
            if (status == "CreateSO") {
                ShowSOGeneralPage();
            }
            else if (status == "CreateVR1") {
                ShowVR1GeneralPage();
            }
        }

        $("#leftpanel").hide();
        $('#leftpanel_div').hide();
        $('#leftpanel').hide()
        scrolltotop();
        if ($("#hdnApplicationStatus").val() == 308001) {
            if (status == "CreateSO") {
                SOvalidationfun1();
            }
            else if (status == "CreateVR1") {
                VR1validationfun1();
            }
        }

    }

    function ShowSuppInfoTabSORT() {
        //$("li").each(function () {
        //    $(this).addClass("nonactive");

        //});
        $(".tab_content1").each(function () {
            $(this).hide();
        });

        //$("li[id=3]").removeClass("nonactive");
        $("li[id=3]").addClass('t');
        $('#pageheader').find('h3').text(title + ' - ' + $("li[id=3]").find('.tab_centre').text());

        ShowVR1SupplInfo();
    }

    @*function routepartlist(AppRevId) {
        var hdnVRRouteTab = $("#hdnVRRouteTab").val();
        //hdnVRRouteTab = "True";
        if (hdnVRRouteTab == "True") {

            $('#tab_4').show();
            $('#RoutePart').html('');
            var url = '@Url.Action("HaulierApplRouteParts", "Application")';
            url += '?RevisionId=' + AppRevId + "&approute=" + true;
            $("#RoutePart").load(url);
            CheckSessionTimeOut();
            $("#ChkNewroute,#Chkroutefromlibrary,#ChkrouteFromMovement").attr("checked", false);

        }
        else {

            var className = $('li[id=3]').attr('class');
            if (className == 't') {
               // $('#tab_3').html('');
               // var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
                var url = '@Url.Action("HaulierApplRouteParts", "Application")';
                url += '?RevisionId=' + AppRevId + "&soapp=" + true;

                $("#tab_3").load(url);
                $("#ChkNewroute1,#ChkNewroute2,#ChkNewroute3").attr("checked", false);
            }
            var className = $('li[id=4]').attr('class');
            if (className == 't')
            {
                $('#tab_4').show();
                $('#RoutePart').html('');
               // var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
                var url = '@Url.Action("HaulierApplRouteParts", "Application")';
                url += '?RevisionId=' + AppRevId + "&approute=" + true;

                $("#RoutePart").load(url);
                CheckSessionTimeOut();
                $("#ChkNewroute,#Chkroutefromlibrary,#ChkrouteFromMovement").attr("checked", false);
            }
        }
    }*@


    function routepartlist(AppRevId) {
        $("#ShowPrevMoveSortRT").val($("#ShowPrevMoveSortRoute").val());
        var hdnVRRouteTab = $("#hdnVRRouteTab").val();
        var preEsdalRef = $('#aesdal_' + AppRevId).data('name');
        if (preEsdalRef != "" && preEsdalRef != undefined && preEsdalRef != null) {
            $('#PrevMovESDALRefNum').val(preEsdalRef);// storing previous movment Esdal ref number for #5697
        }
        //hdnVRRouteTab = "True";
        if (hdnVRRouteTab == "True") {
            $('#tab_4').show();
            $('#RoutePart').html('');
            var url = '@Url.Action("HaulierApplRouteParts", "Application")';
            url += '?RevisionId=' + AppRevId + "&approute=" + true + "&IsSort=" + true;
                $("#RoutePart").load(url, function () {
                    CheckSessionTimeOut();
                });

                $("#ChkNewroute,#Chkroutefromlibrary,#ChkrouteFromMovement").attr("checked", false);
            }
            else {
                $('#tab_3').html('');

                //var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
                var url = '@Url.Action("HaulierApplRouteParts", "Application")';
            url += '?RevisionId=' + AppRevId + "&soapp=" + true + "&IsSort=" + true;

                $("#tab_3").load(url, function () {
                    CheckSessionTimeOut();
                });

                $("#ChkNewroute1,#ChkNewroute2,#ChkNewroute3").attr("checked", false);
            }
        }

    function importapp(partid, vehid) {
        var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
        var PrevMovESDALRefNum = $('#PrevMovESDALRefNum').val();
        $.ajax({
            url: '../Application/AppVehicle_MovementList',
            type: 'POST',
            datatype: 'json',
            async: false,
            data: { vehicleId: vehid, apprevisionId: ApplicationRevId, routepartid: partid, PrevMovEsdalRefNum: PrevMovESDALRefNum },
            success: function (result) {
                startAnimation();
                //$('#tab_3').html('');
                var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
                @*var url = '@Url.Action("ListImportedVehicleConfiguration", "Application")';
                    url += '?apprevisionId=' + ApplicationRevId;
                    $("#tab_3").load(url)
                    $("#ChkNewroute1,#ChkNewroute2,#ChkNewroute3").attr("checked", false);*@
                    $('#hidden_apprevisionId').val(ApplicationRevId);
                    $('#hidden_routepartId').val();
                    $('#hidden_VRAPP').val();
                    ListVehicles();
                  //  $('#form_list_vehicle1').submit();
                },
                error: function () {
                    location.reload();
                },
                complete: function () {
                    stopAnimation();
                }
            });
    }

    function importappVehicle(partid, vehid, configName, vehtype) {
        ;
        var isVR1 = $('#vr1appln').val();
        var PrevMovESDALRefNum = $('#PrevMovESDALRefNum').val();
        var SOVersionID = $('#RevisionID').val(); //Previous Movement version id
        var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
        $('#SelectCurrentMovementsVehicle').html('');
        $("#SelectCurrentMovementsVehicle2").html('');
        $("#SelectCurrentMovementsVehicle1").html('');
        $.ajax({
            url: '../Application/AppVehicle_MovementList',
            type: 'POST',
            datatype: 'json',
            async: false,
            data: { vehicleId: vehid, apprevisionId: ApplicationRevId, routepartid: partid, IsVR1: isVR1, VehicleType: vehtype, PrevMovEsdalRefNum: PrevMovESDALRefNum, SOVersionId: SOVersionID },
            success: function (result) {
                startAnimation();
                $('#tab_3').html('');
                var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
                showWarningPopDialog('Configuration "' + configName + ' imported successfully.', 'Ok', '', 'SelectedRouteFromLibraryForVR1', '', 1, 'info');
                //ShowDialogWarningPop('Configuration "' + configName + ' imported successfully.', 'Ok', '', 'CloseModelPop', '', 1, 'info');
                $('#hidden_apprevisionId').val(ApplicationRevId);
                $('#hidden_routepartId').val();
                $('#hidden_VRAPP').val();
                //SOvalidationfun1();
                //$('#form_list_vehicle').submit();
            },
            error: function () {
                location.reload();
            },
            complete: function () {
                stopAnimation();
            }
        });
    }
    function ShowVehicleList() {
        WarningCancelBtn();
        Show_CandidateRTVehicles();

    }
    function ImportVehicle(vehicleId, vehicleName, VersionType) {
        var VR1Appl = $('#VR1Appl').val();
        var revId = $('#revisionId').val();
        var contentno  = $('#hf_ContentRefNo').val(); 
        var isNotif  = $('#hf_IsNotif').val(); 
        var iscandidate = true;
        var notifId = $('#NotificatinId').val();
        $.ajax({
            url: '../VehicleConfig/ImportVehicleFromList',
            type: 'POST',
            async: false,
            data: { vehicleId: vehicleId, ApplnRevId: revId, isNotif: isNotif, isVR1: VR1Appl, ContentRefNo: contentno, IsCandidate: iscandidate, NotificationId: notifId, VersionType: VersionType },
            beforeSend: function (result) {
                startAnimation();
            },
            success: function (result) {

                //ShowDialogWarningPop("'" + vehicleName + "' vehicle imported successfully.", '', 'OK', '', 'ShowVehicleList', 1, 'info');
               // showWarningPopDialog("'" + vehicleName + "' vehicle imported successfully.", '', 'OK', '', 'ShowVehicleList', 1, 'info');
                if (result != 0) {
                    ShowSuccessModalPopup("'" + vehicleName + "' vehicle imported successfully.", "Show_CandidateRTVehicles");
                }
                else if (iscandidate) {
                    ShowErrorPopup("The vehicle cannot be added, as it is not of special order", 'CloseErrorPopup');
                }
            },
            error: function () {
                location.reload();
            },
            complete: function () {
                stopAnimation();
            }
        });

}
    function ListVehicles() {
        var ApplicationRevId = $('#hidden_apprevisionId').val();
        var RoutePartId = $('#hidden_routepartId').val();
        var VRAPP = $('#hidden_VRAPP').val();
        $.ajax({
            url: '../Application/ListImportedVehicleConfiguration',
            type: 'POST',
            datatype: 'json',
            async: false,
            data: { apprevisionId: ApplicationRevId, routepartId: RoutePartId, VRAPP: VRAPP },
            success: function (result) {
                var className = $('li[id=3]').attr('class');
                if (className == 't') {
                    $('#tab_3').html('');
                    $('#tab_3').html(result);
                    $('#leftpanel').html('');

                    $("#leftpanel").load('../Application/SoVehicle', function () {
                        $("#leftpanel").show();
                    });
                    $("#ChkNewroute1,#ChkNewroute2,#ChkNewroute3").attr("checked", false);
                }

                var className = $('li[id=4]').attr('class');
                if (className == 't') {
                    $('#tab_4').show();
                    $('#RoutePart').html('');
                    $("#RoutePart").html(result);
                   $("#ChkNewroute1,#ChkNewroute2,#ChkNewroute3").attr("checked", false);
            }
            }
        });
    }

    function listSORTvr1veh() {
        $('#tab_3').html('');
        var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;

        //VR1validationfun1('@ViewBag.reduceddetailed');
        if (ApplicationRevId != 0) {
            var routePartID = $('#RoutePartId').val();
            if (status == "ViewProj" || status == "MoveVer") {




                $('#hidden_apprevisionId').val(ApplicationRevId);
                $('#hidden_routepartId').val(routePartID);
                $('#hidden_VRAPP').val(true);

                $('#hidden_SORTView').val(true);
                $('#hidden_status').val(status);

                $('#form_list_vehicle').submit();

                $("#leftpanel").hide();
                $('#leftpanel_div').hide();
                $('#leftpanel').hide()
                @*//show imported vehicle list
                var url = '@Url.Action("ListImportedVehicleConfiguration", "Application")';
                url += '?apprevisionId=' + ApplicationRevId + '&routepartId=' + routePartID + '&VRAPP=' + true + '&SORTView=' + true + '&status=' + status;
                $("#tab_3").load(url);*@
            }
        }
    }

    function ImportRoute(PartID, RouteName) {
        ImportAppRouteForSOApp(PartID, RouteName);
    }

    //Importing Route For SO App
    function ImportAppRouteForSOApp(PartID, RouteName) {

        var AppRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
        console.log(PartID);
        $.ajax({
            url: '../Application/SaveRouteInAppParts',
            type: 'POST',
            cache: false,
            data: { routepartId: PartID, AppRevId: AppRevId },
            beforeSend: function () {
                startAnimation();
            },
            success: function (result) {
                if (result.result != 0) {
                    $('#leftpanel').html('');
                    showWarningPopDialog('"' + RouteName + '" route imported for this application', 'Ok', '', 'SelectedRouteFromLibraryForVR1', '', 1, 'info');
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                //other stuff
                location.reload();
            },
            complete: function () {
                stopAnimation();
            }
        });
    }

    var pageNum = 1;
    var pageSize = 10;
    function Select_AllocateUser() {
        $("#dialogue").html('');
        $("#dialogue").load('../SORTApplication/HaulOrgListPopup?pageNum=1&pageSize=' + pageSize + '&listType=USERLIST', {},
        //$("#dialogue").load('../SORTApplication/HaulOrgListPopup?listType=USERLIST', {},
            function () {
                $('#pageSizeVal').val(pageSize);
                $('#Config-body #pageSizeSelect').val(pageSize);
                removescroll();
                $("#dialogue").show();
                $("#overlay").show();
            });
    }

    function ListVehicles1(result) {
        $('#tab_3').html('');
        $('#tab_3').html(result);
        $('#leftpanel').html('');

        $("#leftpanel").load('../Application/SoVehicle', function () {
            $("#leftpanel").show();
        });
        $("#ChkNewroute1,#ChkNewroute2,#ChkNewroute3").attr("checked", false);
    }

    function unhook(id, routeName) {
        $("#mapTitle").html('');
        $('#tab_4').show();
        $('#RoutePart').html('');
        $('#RoutePart').show();
        $.ajax({
            type: 'POST',
            dataType: 'json',
            async: false,
            url: '../Routes/GetPlannedRoute',
            data: { RouteID: id },
            beforeSend: function (xhr) {
                startAnimation();
            },
            success: function (result) {

                var link = '@Url.Action("A2BPlanning", "Routes", new { routeID = "_ID" })';


                    $("#RoutePart").load(link.replace("_ID", id), {}, function () {
                        CheckSessionTimeOut();
                        $("#mapTitle").append('<h4 style="color:#E30040;">' + routeName + '</h4> <br/>');
                        loadmap('DISPLAYONLY', result.result);

                        $('#leftpanel').html('');
                        $('#RoutePart').append('<button class="btn_reg_back next btngrad btnrds btnbdr" id="back-route" type="button" data-icon="" aria-hidden="true">Cancel</button>');
                    });
                    stopAnimation();
                }
            });
    }


    function displaySoRouteDescMapforNotification(RouteId, RouteType) {
        var hdnVRRouteTab = $("#hdnVRRouteTab").val();
        //hdnVRRouteTab = "True";
        if (hdnVRRouteTab == "True") {
            $("#RoutePart").hide();
            $("#ShowDetail").show();
            $("#RouteMap").html('');
            // $("#leftpanel_quickmenu").html('');
             $('#leftpanel').html('');
            var str = '';
            var HfRouteType = $('#HfRouteType').val();
            if (RouteType != "") {
                str = RouteType;
                HfRouteType = RouteType;
            }
            $.ajax({
                type: 'POST',
                dataType: 'json',
                async: false,
                url: '../Routes/GetPlannedRoute',
                data: { RouteID: RouteId, routeType: str },
                beforeSend: function (xhr) {
                    startAnimation();
                },
                success: function (result) {
                    if (result.result != null) {
                        var count = result.result.routePathList[0].routePointList.length, strTr, flag = 0, Index = 0;

                        for (var i = 0 ; i < count; i++) {
                            if (result.result.routePathList[0].routePointList[i].pointGeom != null || result.result.routePathList[0].routePointList[i].linkId != null)
                                flag = 1;
                        }
                        if (flag == 1 || '@Session["RouteFlag"]' == 1 || '@Session["RouteFlag"]' == 3) {
                                $('#Tabvia').html('');
                                $("#ShowDetail").show();
                                $("#div_Route").hide();

                                $("#RouteName").html(result.result.routePartDetails.routeName);
                                if (result.result.routePartDetails.routeDescr != null && result.result.routePartDetails.routeDescr != "")
                                    $("#RouteDesc").html(result.result.routePartDetails.routeDescr);
                                for (var i = 0 ; i < count; i++) {
                                    if (result.result.routePathList[0].routePointList[i].pointType == 0)
                                        $('#Starting').html(result.result.routePathList[0].routePointList[0].pointDescr);
                                    else if (result.result.routePathList[0].routePointList[i].pointType == 1)
                                        $('#Ending').html(result.result.routePathList[0].routePointList[1].pointDescr);

                                    else if (result.result.routePathList[0].routePointList[i].pointType >= 2) {
                                        Index = Index + 1;
                                        strTr = "<tr ><td style='width:40px'>" + Index + "</td><td>" + result.result.routePathList[0].routePointList[i].pointDescr + "</td></tr>"
                                        $('#Tabvia').append(strTr);
                                    }
                                }
                                $("#RouteMap").html('');
                                $("#RouteMap").load('@Url.Action("A2BPlanning", "Routes", new { routeID = 0 })', function () {
                                    // $("#map").addClass("context-wrap olMap");
                                    var listCountSeg = 0;
                                    //if (result.result.routePathList[0].routeSegmentList != null)
                                    //    listCountSeg = 1;
                                    for (var i = 0; i < result.result.routePathList.Count; i++) {
                                        listCountSeg = result.result.routePathList[i].routeSegmentList.Count;
                                        if (listCountSeg > 0)
                                            break;
                                    }

                                    if (listCountSeg == 0) {
                                        if (result.result.routePathList[0].routeSegmentList != null)
                                            listCountSeg = 1;
                                    }
                                    if (listCountSeg == 0)// if ('@Session["RouteFlag"]' == 2)
                                    {
                                        loadmap('DISPLAYONLY');
                                        showSketchedRoute(result.result);
                                       // loadmap(10, result.result);
                                    }
                                    else {
                                        loadmap('DISPLAYONLY', result.result);
                                    }
                                })
                            }
                            else {

                                $("#RouteMap").html('');
                                $("#ShowDetail").show();
                                $("#div_Route").hide();

                                $("#RouteName").html(result.result.routePartDetails.routeName);
                                $('#Tabvia').html('');
                                if (result.result.routePartDetails.routeDescr != null && result.result.routePartDetails.routeDescr != "")
                                    $("#RouteDesc").html(result.result.routePartDetails.routeDescr);
                                for (var i = 0 ; i < count; i++) {

                                    if (result.result.routePathList[0].routePointList[i].pointType == 0)
                                        $('#Starting').html(result.result.routePathList[0].routePointList[0].pointDescr);
                                    else if (result.result.routePathList[0].routePointList[i].pointType == 1)
                                        $('#Ending').html(result.result.routePathList[0].routePointList[1].pointDescr);

                                    else if (result.result.routePathList[0].routePointList[i].pointType >= 2) {
                                        Index = Index + 1;
                                        strTr = "<tr ><td style='width:40px'>" + Index + "</td><td>" + result.result.routePathList[0].routePointList[i].pointDescr + "</td></tr>"
                                        $('#Tabvia').append(strTr);
                                    }
                                }
                                $("#RouteMap").html('No visual representation of route available.');
                            }
                        }
                        else
                            $("#RouteName").html('Route not available.');
                        if ($('#Starting').html() == '') {
                            $('#trRoute').hide();
                            $('#trStarting').hide();
                            $('#trVia').hide();
                            $('#trEnding').hide();
                        }
                        else {
                            $('#trRoute').show();
                            $('#trStarting').show();
                            $('#trVia').show();
                            $('#trEnding').show();
                        }
                        if ($("#RouteDesc").html() != "") {
                            $('#trHeaderDescription').show();
                            $('#trdesc').show();
                        }
                        else {
                            $('#trHeaderDescription').hide();
                            $('#trdesc').hide();
                        }


                        stopAnimation();
                    }
                });

                $("#tdBtn").html('');
                // var HfRouteType = "'" + $('#HfRouteType').val() + "'";
                //HfRouteType = "planned";
            $("#tdBtn").html('<button class="btn_reg_back next btngrad btnrds btnbdr"  aria-hidden="true" data-icon="" type="button" id="displaypop" >Back</button>  <button id="importnotif" data-routeid="' + RouteId + '" data-nameroute="' + $("#RouteName").html() + '"  class="tdbutton next btngrad btnrds btnbdr"  aria-hidden="true" data-icon="&#xe0f4;" >Import</button>');

                $("#TDbtnBackToRouteList").html('<button class="btn_reg_back next btngrad btnrds btnbdr"  aria-hidden="true" data-icon="" type="button" id="goto-lib" >Back</button> ')

            $("#TDbtnBackToRouteList").html('<button class="btn_reg_back next btngrad btnrds btnbdr"  aria-hidden="true" data-icon="" type="button" id="route-lib" >Back</button>  <button id="import-inapp" data-RouteId="' + RouteId + '" data-HfRouteType="' + HfRouteType + '"  class="tdbutton next btngrad btnrds btnbdr"  aria-hidden="true" data-icon="&#xe0f4;" >Import</button>')


            }
        }
    $('body').on('click', '#importnotif', function (e) {
        e.preventDefault();
        var routeid = $(this).data('routeid');
        var nameroute = $(this).data('nameroute');
        Importrouteinnotif(routeid,nameroute);
    });
    //Candiadte Routes
    function load_routeAnalysisSORT() {
        startAnimation();
        analysis_id = $('#candAnalysisId').val();
        var iscandlastversion = $('#IsCandVersion').val();
        var plannruserid = $('#PlannrUserId').val();
        var appstatuscode = $('#AppStatusCode').val();
        var movversionno = $('#versionno').val();
        var movdistributed = $('#IsMovDistributed').val();
        var sonumber = $('#SONumber').val();
        var iscandidatert = $('#IsCandVersion').val();
        var Distributed_mov_analysisid = 0;
        if (iscandidatert == 'True') {
            Distributed_mov_analysisid = $('#DistributedMovAnalysisId').val();
            versionId = 0;
        }
        var VR1Applciation = $('#VR1Applciation').val();
        if (VR1Applciation == 'True') {
            analysis_id = $('#analysis_id').val();
            versionId = $('#versionId').val();
            $('#SelectCurrentMovements1').hide();
            $('#SelectCurrentMovements2').hide();
            $('#route').hide();
            $('#back_btn_Rt').hide();
            $('#Organisation_ID').val(0);
            $.post('/SORTApplication/CheckRouteAssessment?AnalysisId=' + analysis_id + '&APP_Rev_ID=' + revisionId + '&VR1App=' + VR1Applciation + '&MovAnalysisId=' + Distributed_mov_analysisid, function (data) {

                if (data == 1) {
                    $.ajax({
                        url: '../RouteAssessment/GenerateRouteAssessment',
                        type: 'POST',
                        cache: false,
                        async: true,
                        data: { versionId: versionId, esdalref: null, revisionId: revisionId, notificationId: 0, analysisId: analysis_id, prevAnalysisId: Distributed_mov_analysisid, VSOType: null },
                        beforeSend: function () {
                            startAnimation();
                        },
                        success: function (result) {
                            stopAnimation();
                            HandleRouteAssessmentView(analysis_id, revisionId, true, true, _checkerid, _checkerstatus, iscandlastversion, plannruserid, appstatuscode, movversionno, movdistributed, sonumber);
                        },
                        error: function (xhr, textStatus, errorThrown) {
                            stopAnimation();
                        },
                        complete: function () {
                            stopAnimation();
                        }
                    });
                }
                else {
                    HandleRouteAssessmentView(analysis_id, revisionId, true, true, _checkerid, _checkerstatus, iscandlastversion, plannruserid, appstatuscode, movversionno, movdistributed, sonumber);
                }
            });
        }
        else {
            $.post('/SORTApplication/ChkVehAsgnToAllRoutPrts?revisionId=' + revisionId, function (result) {
                if (result.dataval == null) {
                    $('#SelectCurrentMovements1').hide();
                    $('#SelectCurrentMovements2').hide();
                    $('#route').hide();
                    $('#back_btn_Rt').hide();
                    $('#Organisation_ID').val(0);

                    $.post('/SORTApplication/CheckRouteAssessment?AnalysisId=' + analysis_id + '&APP_Rev_ID=' + revisionId + '&VR1App=' + VR1Applciation + '&MovAnalysisId=' + Distributed_mov_analysisid, function (data) {

                        if (data == 1) {
                            $.ajax({
                                url: '../RouteAssessment/GenerateRouteAssessment',
                                type: 'POST',
                                cache: false,
                                async: true,
                                data: { versionId: versionId, esdalref: null, revisionId: revisionId, notificationId: 0, analysisId: analysis_id, prevAnalysisId: Distributed_mov_analysisid, VSOType: null },
                                beforeSend: function () {
                                    startAnimation();
                                },
                                success: function (result) {
                                    stopAnimation();
                                    HandleRouteAssessmentView(analysis_id, revisionId, true, true, _checkerid, _checkerstatus, iscandlastversion, plannruserid, appstatuscode, movversionno, movdistributed, sonumber);
                                },
                                error: function (xhr, textStatus, errorThrown) {
                                    stopAnimation();
                                },
                                complete: function () {
                                    stopAnimation();
                                }
                            });
                        }
                        else {
                            HandleRouteAssessmentView(analysis_id, revisionId, true, true, _checkerid, _checkerstatus, iscandlastversion, plannruserid, appstatuscode, movversionno, movdistributed, sonumber);
                        }
                    });
                }
                else if (result.dataval == "Please assign candidate vehicles to all route parts!") {
                    stopAnimation();
                    $("#cand_rtanalysis").find(".card").removeClass("active-card");
                    $("#2").find(".card").addClass("active-card");
                    ShowInfoPopup(result.dataval, "Show_CandidateRTVehicles");
                }
            });
        }
    }

    function HandleRouteAssessmentView(analysisId, revisionId, sortFlag, isCandidate, checkerId, checkerStatus,isCandidateLastVersion,plannerUserId,appStatusCode,movVersionNumber, movDistributed, soNumber) {
              $(".tab_content1").each(function () {
            $(this).hide();
        });
        $('#leftpanel_div').hide();
        $('#leftpanel').hide();
        $("#overlay").show();
        $("#leftpanel").html('');
        $('#leftpanel').load('../Application/RouteAnalysisPanel', {
                        analysisId: analysisId, RivisionId: revisionId, SORTflag: sortFlag, IsCandidate: isCandidate, CheckerId: checkerId, CheckerStatus: checkerStatus, IsCandLastVersion: isCandidateLastVersion, planneruserId: plannerUserId, appStatusCode: appStatusCode, MovVersionNo: movVersionNumber, IsDistributed: movDistributed, SONumber: soNumber

        }, function () {
            $("#leftpanel_div").show();
            $('#leftpanel').show();
            $("#overlay").hide();
        });
    }
    //create vehicle configuration
    function Show_CandidateRTVehicles() {
        CloseSuccessModalPopup();
        CloseInfoPopup('InfoPopup');
        CandidateBackFlag = 0;
        CandidateBackSubFlag = 0;
        $('#StruRelatedMov').hide();
        $('#back_btn_Rt').hide();
        $('#RoutePart').show();
        $('#divCurrentMovement').hide();
        $('#SelectCurrentMovements1').hide();
        $('#SelectCurrentMovements2').hide();
        $('#route').hide();
        startAnimation();
        var rtrevisionId = $('#revisionId').val();
        var iscandlastversion = $('#IsCandVersion').val();
        var plannruserid = $('#PlannrUserId').val();
        var appstatuscode = $('#AppStatusCode').val();
        var movversionno = $('#versionno').val();
        var movdistributed = $('#IsMovDistributed').val();
        var sonumber = $('#SONumber').val();

        if (rtrevisionId == 0) {
            $('#leftpanel').hide();
        }
        else {
            $(".tab_content1").each(function () {
                $(this).hide();
            });

            $('#tab_2').show();

            //load left panel
            startAnimation();
            $('#tab_2').load('../SORTApplication/ListImportCandidateRouteVehicles?revisionid=' + rtrevisionId + '&CheckerId=' + _checkerid + '&CheckerStatus=' + _checkerstatus + '&IsCandLastVersion=' + iscandlastversion + '&planneruserId=' + plannruserid + '&appStatusCode=' + appstatuscode + '&SONumber=' + sonumber + '&MovVersionNo=' + movversionno + '&IsDistributed=' + movdistributed, function () {
                $("#leftpanel").html('');
                $("#leftpanel").hide();
                $('#leftpanel').html('');
                $('#btnfinishcopy').hide();
                $('#btnnavigatenext').hide();
                stopAnimation();
            });

        }
    }
    function ViewProjectGeneralDetails1() {
        var VR1Applciation = $('#VR1Applciation').val();
        var Owner = $('#Owner').val();
        var Checker = $('#Checker').val();
        var revisionId = $('#revisionId').val();
        var apprevisionid = $('#ApprevId').val();
        var hauliermnemonic = $('#hauliermnemonic').val();
        var esdalref = $('#esdalref').val();
        var prjstatus = $('#Proj_Status').val();
        //$("#leftpanel").hide();
        //$('#leftpanel_div').hide();
        //$('#leftpanel_quickmenu').hide()
        Owner = Owner.replace(/ /g, '%20');
        Checker = Checker.replace(/ /g, '%20');

        startAnimation();
        $('#tab_General').load('../SORTApplication/SortGenerelDetails?hauliermnemonic=' + hauliermnemonic + '&esdalref=' + esdalref + '&revisionno=' + revisionno + '&versionno=' + versionno + '&Rev_ID=' + apprevisionid + '&Checker=' + Checker + '&OwnerName=' + Owner + '&ProjectId=' + projectid + '&VR1App=' + VR1Applciation, {},
           function () {
               $('#tab_General').show();
               stopAnimation();
               //$("#leftpanel").html('');
               //$("#leftpanel").hide();
               //$('#leftpanel').html('');
               //$('#btnfinishcopy').hide();
               //$('#btnnavigatenext').hide();

           });
    }
    function ViewProjectGeneralDetails() {
        startAnimation();
        $('#SelectCurrentMovements1').hide();
        $('#SelectCurrentMovements2').hide();
        $('#route').hide();
        $('#back_btn_Rt').hide();
        var VR1Applciation = $('#VR1Applciation').val();
        var Owner = $('#Owner').val();
        var Checker = $('#Checker').val();
        var revisionId = $('#revisionId').val();
        var apprevisionid = $('#ApprevId').val();
        var hauliermnemonic = $('#hauliermnemonic').val();
        var esdalref = $('#esdalref').val();
        var prjstatus = $('#Proj_Status').val();
        Owner = Owner.replace(/ /g, '%20');
        Checker = Checker.replace(/ /g, '%20');

        startAnimation();

        $('#tab_3').show();

        $('#leftpanel').html('');
        $('#leftpanel').hide();
        $('#sort11').hide();
        $('#information').hide();
        $('#btn_cancel').hide();

        $.ajax({
            url: "../SORTApplication/SortGenerelDetails",
            type: 'post',
            async: false,
            data: { hauliermnemonic: hauliermnemonic, esdalref: esdalref, revisionno: revisionno, versionno: versionno, Rev_ID: apprevisionid, Checker: Checker, OwnerName: Owner, ProjectId: projectid, VR1App: VR1Applciation },
            success: function (data) {

                $('#tab_3').html(data);
                stopAnimation();
                CheckSessionTimeOut();
                addscroll();

            },
            complete: function () {
                $('#leftpanel').html('');
                $('#leftpanel').hide();
                $('#sort11').hide();
                $('#tab_3').show();
                //$("li").each(function () {
                //    $(this).addClass("nonactive");

                //});
                $("li[id='3']").show();
                //$("li[id='3']").removeClass("nonactive");
                $("li[id='3']").addClass('t');

                stopAnimation();
                $('#overlay').hide();
            },
            error: function () {
            }
        });



    }
    //Candidate route binding.
    function BindRouteParts() {
        CloseSuccessModalPopup();//closing success modal popup
        $('#StruRelatedMov').hide();
        $('#RoutePart').show();
        $('#divCurrentMovement').hide();
        $('#SelectCurrentMovements1').hide();
        $('#SelectCurrentMovements2').hide();
        $("#divCandiRouteDeatils").html('');
        $('#divbtn_prevmove1').remove();
        startAnimation();
        $("li[id='4']").show();
        $("li[id='4']").addClass('t');

        var rtrevisionId = $('#revisionId').val();
        var iscandlastversion = $('#IsCandVersion').val();
        var plannruserid = $('#PlannrUserId').val();
        var appstatuscode = $('#AppStatusCode').val();
        var movversionno = $('#versionno').val();
        var movdistributed = $('#IsMovDistributed').val();
        var sonumber = $('#SONumber').val();
        var hauliermnemonic = $('#hauliermnemonic').val();
        var esdalref = $('#esdalref').val();
        var prjstatus = $('#Proj_Status').val();
        if (rtrevisionId == 0) {
            $('#leftpanel').hide();
        }
        else {

            $(".tab_content1").each(function () {
                $(this).hide();
            });
            $.ajax({
                url: "../SORTApplication/ShowCandidateRoutes",
                type: 'post',
                async: false,
                data: { routerevision_id: rtrevisionId, CheckerId: _checkerid, CheckerStatus: _checkerstatus, IsCandLastVersion: iscandlastversion, planneruserId: plannruserid, appStatusCode: appstatuscode, SONumber: sonumber },
                success: function (data) {
                    $('#tab_4').show();
                    $('#RoutePart').html('');
                    $('#RoutePart').html(data);
                    $('#leftpanel').html('');
                    $('#leftpanel').hide();
                    $('#sort11').hide();
                    $('#information').hide();
                    $('#btn_cancel').hide();
                    //if ($('#ViewFlag').val() == 0) {
                        //$("#leftpanel").load('../SORTApplication/SortRoutes?CheckerId=' + _checkerid + '&CheckerStatus=' + _checkerstatus + '&IsCandLastVersion=' + iscandlastversion + '&planneruserId=' + plannruserid + '&appStatusCode=' + appstatuscode + '&MovVersionNo=' + movversionno + '&IsDistributed=' + movdistributed + '&SONumber=' + sonumber, function () {
                        //    $("#leftpanel").show();
                        //    stopAnimation();
                        //    $('#pop-warning').hide();
                        //});
                    //}
                    stopAnimation();


                    var prjheader  = $('#hf_CandName' + " - Version " + '@ViewBag.CandVersionNo' + ") - Route').val(); 
                    $('div#pageheader').html('');
                    $('div#pageheader').append('<h3>' + prjheader + '</h3>');
                    $('#route').html('');
                    $('#route').hide();//hide map
                    $('#back_btn_Rt').hide();
                    CheckSessionTimeOut();
                },
                error: function () {
                }
            });
            addscroll();
        }
    }

    //Check Deffrences
    function Cand_Cdefference_Click() {

        $('#leftpanel').html('');
        $('#leftpanel').hide();
        //$("#leftpanel_quickmenu").html('');
        //$("#leftpanel_quickmenu").hide();
        $(".tab_content1").each(function () {
            $(this).hide();
        });
        $('#RouteAssesment').html('');
        $('#RouteAssesment').show();
        var _orgId = $('#OrganisationId').val();
        /*$("#Cand_CheckDeffrence").load('../SORTApplication/CheckingDeffernce?appRevisionId=' + ApprevId+'&OrganisationId='+_orgId, function (responseText, textStatus, req) {
            $('.loading').hide();
            if (textStatus == "error") {
                location.reload();
            }

            $('#Cand_CheckDeffrence').show();

            $('#leftpanel').html(updateStushtml);
            $('#leftpanel').show();
        });
        $('#leftpanel_quickmenu').hide();*/

    }
    //Sending for checker.
    function CandidateWorkAllocation_Click(atype, typename, isCheckingStatusCheck = false) {
        startAnimation();
        $("#overlay").hide();
        var checkingstatus = 1;
        IsVR1Applciation = $('#VR1Applciation').val();
        var CandRevisionID = $('#LastCandRevisionId').val();
        if (typename == "QAChecking") {
            var _sonumber = $('#SONumber').val();
            var _projstatus = $('#Proj_Status').val();
            if (_projstatus == "Agreed Recleared") {
                _sonumber = $('#sonum').val();
            }
            if (_sonumber == "") {
                stopAnimation();
                checkingstatus = 0;
                if (_projstatus == "Agreed Recleared") {
                    ShowWarningPopup('Do you want to create special order before send for QA checking?', 'CreateNewSpecialOrder', 'UpdateOldSpecialOrder');
                }
                else {
                    ShowInfoPopup('Please create special order before send for QA checking!');
                }
            }
        };
        if (checkingstatus == 1) {
            $.post('/SORTApplication/ChkVehAsgnToAllRoutPrts?revisionId=' + CandRevisionID, function (result) {
                if (result.dataval == null) {//condition has been checked for #7592
                    var analysis_id = $('#candAnalysisId').val();
                    var mov_analysis_id = $('#analysis_id').val();
                    var AppRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
                    $.post('/SORTApplication/CheckAffPartBeforeSending?AnalysisId=' + analysis_id + '&APP_Rev_ID=' + AppRevId + '&VR1App=' + IsVR1Applciation + '&CType=' + typename + '&MovAnalysisId=' + mov_analysis_id, function (data) {
                        if (data == 1) {
                            $("#overlay").show();
                            $('.loading').show();
                            var options = { "backdrop": "static", keyboard: true };
                            $.ajax({
                                type: "GET",
                                url: "../SORTApplication/ViewCandidateCheckerUsers",
                                //contentType: "application/json; charset=utf-8",
                                data: { AllocationType: atype },
                                datatype: "json",
                                success: function (data) {
                                    $('#generalPopupContent').html(data);
                                    $('#generalPopup').modal(options);
                                    $('#generalPopup').modal('show');
                                    $('.loading').hide();
                                    stopAnimation();
                                },
                                error: function () {
                                    location.reload();
                                }
                            });
                        }
                        else if (data == 2) {
                            stopAnimation();
                            ShowInfoPopup('Please fill in the project details by clicking the edit button!');
                        }
                        else {
                            stopAnimation();
                            switch (data) {
                                case 3:
                                    var msg = 'Please generate driving instructions before send for checking from last candidate route version!';
                                    if (typename == 'QAChecking')
                                        msg = 'Please generate driving instructions before send for qa checking from last movement version!';
                                    else if (typename == 'FChecking')
                                        msg = 'Please generate driving instructions before send for final checking from last movement version!';
                                    ShowInfoPopup(msg);
                                    break;
                                case 4:
                                    ShowInfoPopup('Please generate affected structures before send for checking from last candidate route version!');
                                    break;
                                case 5:
                                    ShowInfoPopup('Please generate route description before send for checking from last candidate route version!');
                                    break;
                                case 6:
                                    ShowInfoPopup('Please generate affected parties before send for checking from last candidate route version!');
                                    break;
                                case 7:
                                    ShowInfoPopup('Please generate affected roads before send for checking from last candidate route version!');
                                    break;
                            }
                        }
                    });
                }
                else if (result.dataval == "Please assign candidate vehicles to all route parts!") {

                    stopAnimation();
                    $("#cand_rtanalysis").find(".card").removeClass("active-card");
                    $("#2").find(".card").addClass("active-card");
                    if (isCheckingStatusCheck==false) {
                        ShowInfoPopup(result.dataval, "Show_CandidateRTVehicles");
                    }
                    else {
                        ShowInfoPopup(result.dataval);
                    }

                }
            });
        }

    }
    //function for show broken route message
    /* Blocking    typename
    send for checking :  Schecking
    CompleteCHecking:  QAchecking
    Warning
     send for checking :  Schecking   for vr1
    CompleteCHecking:  QAchecking     for vr1

    Send for QA check: Q Achecking
    Send For Final CHeck: Sighnoff
    CompleteQAchecking:  QAchecking
    Complete Final: Sighnoff
    */
    var check_Type;
    var check_Type_Name;
    function VerifyBrokenRouteAndProceed(atype, typename) {
        var _verno = $('#versionno').val();
        var CandRevisionID = 0;
        var mov_verid = 0;
        var FlagValue = 0;
        var specManouerCount = 0;
        var brokenRouteCount = 0;
        var isBrokenData = [];  // result return from isBroken check function
        IsVR1Applciation = $('#VR1Applciation').val();
        if (atype == "checking" && IsVR1Applciation != "True") {// condition check for whether it is a send for checking or complete cheking and not a vr1.. in this case we can block the functionality
            CandRevisionID = $('#LastCandRevisionId').val();
            isBrokenData = CheckIsBroken(0, 0, 0, 0, 0, CandRevisionID);
            for (var i = 0; i < isBrokenData.length; i++) {
                if (isBrokenData[i].isBroken > 0) {
                    brokenRouteCount++;
                    if (isBrokenData[i].isReplan > 1) {   //check in the existing route is broken and there exists special manouers
                        specManouerCount++;
                    }
                }
            }
            if (typename == "SChecking") {
                if (brokenRouteCount > 0) {
                    if (specManouerCount > 0) {
                        FlagValue = 1;
                    }
                    else {
                        FlagValue = 13;
                    }
                }
            }
            else {
                if (brokenRouteCount > 0) {
                    if (specManouerCount > 0) {
                        FlagValue = 10;
                    }
                    else {
                        FlagValue = 12;
                    }
                }
            }
        }
        else {
            mov_verid = $('#versionId').val();
            isBrokenData = CheckIsBroken(0, mov_verid, 0, 0, 0, 0);
            for (var i = 0; i < isBrokenData.length; i++) {
                if (isBrokenData[i].isBroken > 0) {
                    brokenRouteCount++;
                }
            }
            if (brokenRouteCount > 0) {
                FlagValue = 5;
            }
        }
        $.ajax({
            url: '../SORTApplication/ShowBrokenRoutes',
            type: 'POST',
            cache: false,
            async: false,
            data: { App_revision_id: 0, can_revision_id: CandRevisionID, mov_version_id: mov_verid, FlagValue: FlagValue },
            beforeSend: function () {
                if (FlagValue == 10 || FlagValue == 12 || FlagValue == 1 || FlagValue == 13) {
                    startAnimation();
                    $("#overlay").show();
                    $("#dialogue").show();
                    $('.loading').show();
                }
            },
            success: function (data) {
                check_Type = atype;
                check_Type_Name = typename;
                var msg = '';
                if (data == "") {
                    if (typename == "SChecking" || typename == "QAChecking" || typename == "FChecking") {    //checking whether it is send for /complete status
                        CandidateWorkAllocation_Click(atype, typename, true);
                    }
                    else {
                        UpdateCheckingStatus_Click(atype, typename);
                    }
                }
                else if (FlagValue == 10 || FlagValue == 12 || FlagValue == 1 || FlagValue == 13) {
                    $('#dialogue').html(data);
                }
                else if (atype == "checking") {
                    if (typename == "SChecking") {
                        msg = 'Please be aware that due to the map upgrade route(s) in VR1 movement version (' + _verno + ') contain previous map data and have been updated since the haulier applied for the movement.';
                        showWarningPopDialog(msg, 'Ok', '', 'CandidateWorkAllocation_Click1', '', 1, 'info');
                    }
                    else {
                        msg = 'Please be aware that due to the map upgrade route(s) in VR1 movement version (' + _verno + ') contain previous map data and have been updated since the haulier applied for the movement.';
                        showWarningPopDialog(msg, 'Ok', '', 'UpdateCheckingStatus_Click1', '', 1, 'info');
                    }
                }
                else if (atype == "QAChecking") {
                    if (typename == "QAChecking") {
                        msg = 'Please be aware that due to the map upgrade route(s) in the agreed/agreed re-cleared movement version (' + _verno + ') contain previous map data. Please review before sending for QA checking.';
                        showWarningPopDialog(msg, 'Ok', '', 'CandidateWorkAllocation_Click1', '', 1, 'info');
                    }
                    else {
                        msg = 'Please be aware that due to the map upgrade route(s) in the agreed/agreed re-cleared movement version (' + _verno + ') contain previous map data. Please review before completing QA checking';
                        showWarningPopDialog(msg, 'Ok', '', 'UpdateCheckingStatus_Click1', '', 1, 'info');
                    }
                }
                else if (atype == "signoff") {
                    if (typename == "FChecking") {
                        msg = 'Please be aware that due to the map upgrade route(s) in the agreed/agreed re-cleared movement version (' + _verno + ') contain previous map data. Please review before sending for final checking.';
                        showWarningPopDialog(msg, 'Ok', '', 'CandidateWorkAllocation_Click1', '', 1, 'info');
                    }
                    else {
                        msg = 'Please be aware that due to the map upgrade route(s) in the agreed/agreed re-cleared movement version (' + _verno + ') contain previous map data. Please review route before completing final checking.';
                        showWarningPopDialog(msg, 'Ok', '', 'UpdateCheckingStatus_Click1', '', 1, 'info');
                    }
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                //other stuff
                location.reload();
            },
            complete: function () {
                if (FlagValue == 10 || FlagValue == 12 || FlagValue == 1 || FlagValue == 13) {
                    removescroll();
                    $('.loading').hide();
                    //stopAnimation();
                }
                //else {
                //    stopAnimation();
                //    $("#dialogue").hide();
                //    $("#overlay").hide();
                //    $('.loading').hide();
                //}
            }
        });
    }

    /* Written for esdal4 to avoid broken routes check */
    function CandidateWorkAllocation(atype, typename) {
        if (typename == "SChecking" || typename == "QAChecking" || typename == "FChecking") {      //checking whether it is send for /complete status
            CandidateWorkAllocation_Click(atype, typename, true);
        }
        else {
            UpdateCheckingStatus_Click(atype, typename);
        }
    }
    function CandidateWorkAllocation_Click1() {
        CandidateWorkAllocation_Click(check_Type, check_Type_Name);
    }
    function UpdateCheckingStatus_Click1() {
        UpdateCheckingStatus_Click(check_Type, check_Type_Name);
    }
    //Update checking status
    function UpdateCheckingStatus_Click(atype, typename) {
        //WarningCancelBtn();
        startAnimation();
        var analysis_id = $('#candAnalysisId').val();
        var mov_analysis_id = $('#analysis_id').val();
        IsVR1Applciation = $('#VR1Applciation').val();
        var AppRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
        $.post('/SORTApplication/CheckAffPartBeforeSending?AnalysisId=' + analysis_id + '&APP_Rev_ID=' + AppRevId + '&VR1App=' + IsVR1Applciation + '&CType=' + typename + '&MovAnalysisId=' + mov_analysis_id, function (data) {
            if (data == 1) {
                $("#overlay").show();
                $('.loading').show();
                var _orgId = $('#OrganisationId').val();
                var options = { "backdrop": "static", keyboard: true };
                $.ajax({
                    type: "GET",
                    url: "../SORTApplication/UpdateCheckingStatusPopup",
                    //contentType: "application/json; charset=utf-8",
                    data: { ApprsionId: ApprevId, AllocationType: atype, OrganisationId: _orgId, VersionId: versionId },
                    datatype: "json",
                    success: function (data) {
                        $('#generalPopupContent').html(data);
                        $('#generalPopup').modal(options);
                        $('#generalPopup').modal('show');
                        $('.loading').hide();
                        stopAnimation();
                    },
                    error: function () {
                        location.reload();
                    }
                });
            }
            else if (data == 2) {
                //showWarningPopDialog('Please fill in the project details by clicking the edit button!', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
                ShowInfoPopup('Please fill in the project details by clicking the edit button!');
            }
            else {
                switch (data) {
                    case 3:
                        var msg = 'Please generate driving instructions before checking is complete';
                        if (typename == 'CQAChecking')
                            msg = 'Please generate driving instructions before complete qa checking from last movement version!';
                        if (typename == 'CFChecking')
                            msg = 'Please generate driving instructions before complete final checking from last movement version!';
                        //showWarningPopDialog(msg, 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
                        ShowInfoPopup(msg);
                        break;
                    case 4:
                        //showWarningPopDialog('Please generate affected structures before send for checking from last candidate route version!', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
                        ShowInfoPopup('Please generate affected structures before send for checking from last candidate route version!');
                        break;
                    case 5:
                        //showWarningPopDialog('Please generate route description before send for checking from last candidate route version!', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
                        ShowInfoPopup('Please generate route description before send for checking from last candidate route version!');
                        break;
                    case 6:
                        //showWarningPopDialog('Please generate affected parties before send for checking from last candidate route version!', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
                        ShowInfoPopup('Please generate affected parties before send for checking from last candidate route version!');
                        break;
                    case 7:
                        //showWarningPopDialog('Please generate affected roads before send for checking from last candidate route version!', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
                        ShowInfoPopup('Please generate affected roads before send for checking from last candidate route version!');
                        break;
                }
            }
        });
    }
    //view vehicles in popup
    function ViewVehicle(id) {
        var movementId = $('#MoveId').val();
        $("#dialogue").html('');
        $("#dialogue").load('../VehicleConfig/PartialViewLayout', function () {
            DynamicTitle('View configuration');
            $.ajax({
                url: '../VehicleConfig/ViewConfiguration',
                type: 'GET',
                cache: false,
                async: false,
                data: { vehicleID: id, isRoute: true, movementId: movementId },
                beforeSend: function () {
                    startAnimation();
                    $("#overlay").show();
                    $("#dialogue").hide();
                    $('.loading').show();
                },
                success: function (result) {
                    $('#Config-body').html(result);
                },
                error: function (xhr, textStatus, errorThrown) {
                    //other stuff
                    location.reload();
                },
                complete: function () {
                    stopAnimation();
                    $("#dialogue").show();
                    $("#overlay").show();
                    $('.loading').hide();
                    removescroll();
                }
            });

        });
    }
    function CreateCandidateRT_Click() {

        $("#overlay").show();
        $('.loading').show();
        $("#dialogue").html('');

        $("#dialogue").load('../SORTApplication/CreateCandidateRoute?CandRevisioId=' + '@ViewBag.LatestCandidateRevisionId', function (responseText, textStatus, req) {
            $('.loading').hide();
            if (textStatus == "error") {
                location.reload();
            }
        });
        removescroll();
        resetdialogue();
        $("#overlay").show();
        $("#dialogue").show();
    }
    //Create new candidate version
    function CandidateCreateversion_Click() {
        startAnimation();
        $("#inactive-button-candidate").css("display", "none");
        $("#active-button-candidate").css("display", "block");
        $("#active-button-candidate").css("background", "#275795");
        $("#active-button-candidate").css("color", "white");
        $('#CreateNewCandidate').load('../SORTApplication/CreateCandidateRouteVersion', {},
            function () {
                $("#overlay").hide();
                stopAnimation();
            });
    }
    function removeCandidateButton() {
        $('#CreateNewCandidate').html("");
        $("#active-button-candidate").css("display", "none");
        $("#inactive-button-candidate").css("display", "block");
    }

    //Create candidate route new version from revised application versions
    function Createcandidateversion_Click() {
        var lastcandrouteid = $('#LastCandRouteId').val();
        //var lastcandrtreversionid = $('#LastCandRevisionId').val();
        if (lastcandrouteid != 0) {
            startAnimation();
            $.post('/SORTApplication/CreateCandidateVersion?CandRouteId=' + lastcandrouteid + '&CloneType=application&AppRevId=' + ApprevId, function (data) {
                if (data != null) {
                    var result = JSON.parse(data);
                    //$('#RoutePart').hide();
                    stopAnimation();
                    if (result.newrivisionId != 0 && result.newversionNo != 0 && result.analysisid != 0) {
                        $('#candAnalysisId').val(result.analysisid);
                        $('#LastCandRevisionId').val(result.newrivisionId);
                        $('#LastCandVersion').val(result.newversionNo);

                        showWarningPopDialog('New candidate route version is successfully created from revised application revision.', 'Ok', '', 'Redirect_ProjectOverview', '', 1, 'info');

                    }
                    else {
                        showWarningPopDialog('The new version is not created.', 'Ok', '', 'WarningCancelBtn', '', 1, 'error');
                    }
                }
            });
        }
        else {
            $('#trvalidcandrt').show();
        }
    }

    function GetSortSORoute(pointOfCommunication, dataModelPassed) {
        var data = {
            module: 'SOP', pointOfCommunication: pointOfCommunication, dataModel: JSON.stringify(dataModelPassed)
        };
        return ExecutePostAjax("../Workflow/Index", data);
    }
    //Creat new movement version
    function Createmovementversion_Click() {
        var revisionId = $('#LastCandRevisionId').val();
        removescroll();
        // startAnimation();
        //$("#dialogue").load('../SORTApplication/ShowBrokenRoutes?App_revision_id=0&can_revision_id=' + revisionId + '&mov_version_id=0&FlagValue=11', function (data) {
        //    if (data != "") {
        //        $("#overlay").show();
        //        $("#dialogue").show();
        //        $('.loading').hide();
        //    }
        //    else {
        ShowDialogWarningPop('Do you want to create new movement version?', 'NO', 'YES', 'WarningCancelBtn', 'Createnewmovementversion', 1, 'warning');
        // }
        //});
    }
    function WarningCancelBtnMov() {
        WarningCancelBtn();
        $("#overlay").hide();
        $('.loading').hide();
    }
    function Createnewmovementversion() {
        //WarningCancelBtn();
        //startAnimation('Checking validations');
        $('#pop-warning').modal('hide');
        startAnimation();
        analysis_id = $('#candAnalysisId').val();
        $.post('/SORTApplication/CheckRouteAsseBeforeDistributing?AnalysisId=' + analysis_id, function (data) {
            //stopAnimation();
            switch (data) {
                case 1:
                    //startAnimation('Creating new movement version');
                    var appref = hauliermnemonic + "/" + esdalref + "-" + revisionno;
                    var movversionno = $('#versionno').val();
                    var sorthauliermnemonic = hauliermnemonic;
                    var sortesdalrefno = esdalref;
                    var newmovversionno = ++movversionno;
                    var vr1Application = false;
                    if ($('#VR1Applciation').val() === "True") {
                        vr1Application = true;
                    }

                    var dataModelPassed = { ProjectId: projectid, AppRevisionId: ApprevId, AnalysisId: analysis_id, RouteRevisionId: revisionId, AppRef: appref, MovVersionNo: movversionno, Haulnemonic: sorthauliermnemonic, Esdalrefno: sortesdalrefno, isVr1Application: vr1Application, isWorkflow:true };

                    var result = GetSortSORoute(4, dataModelPassed);
                    if (result == null || result != "NOROUTE") {
                    $.ajax({
                        url: result.route, ///SORTApplication/CreateMovementVersion
                        type: 'POST',
                        async: true,
                        data: {
                            createMovementVersionCntrlModel: result.dataJson
                        },
                        beforeSend: function () {

                        },
                        success: function (data) {
                            stopAnimation();
                            var result = JSON.parse(data);
                            if (result.MVersionId != 0 && result.MAnalysisId != 0) {
                                $('#RoutePart').hide();
                                $('#versionId').val(result.MVersionId);
                                versionId = result.MVersionId;
                                $('#versionno').val(newmovversionno);
                                versionno = newmovversionno;
                                $('#analysis_id').val(result.MAnalysisId);
                                analysis = result.MAnalysisId;
                                ShowInfoPopup('New movement version created successfully', 'Redirect_ProjectOverview');
                            }
                            else {
                                ShowInfoPopup('The new movement version is not created', 'WarningCancelBtn');
                            }
                        },
                        error: function () {
                            stopAnimation();
                            ShowInfoPopup('The new movement version is not created', 'WarningCancelBtn');
                        }
                    });
					}
                    //$.post('/SORTApplication/CreateMovementVersion?ProjectId=' + projectid + '&AppRevisionId=' + ApprevId + '&AnalysisId=' + analysis_id + '&RouteRevisionId=' + revisionId + '&AppRef=' + appref + '&MovVersionNo=' + movversionno + '&Haulnemonic=' + sorthauliermnemonic + '&Esdalrefno=' + sortesdalrefno, function (data) {
                    //    stopAnimation();

                    //    var result = JSON.parse(data);
                    //    if (result.MVersionId != 0 && result.MAnalysisId != 0) {
                    //        $('#RoutePart').hide();
                    //        $('#versionId').val(result.MVersionId);
                    //        versionId = result.MVersionId;
                    //        $('#versionno').val(newmovversionno);
                    //        versionno = newmovversionno;
                    //        $('#analysis_id').val(result.MAnalysisId);
                    //        analysis = result.MAnalysisId;
                    //        ShowInfoPopup('New movement version created successfully', 'Redirect_ProjectOverview');
                    //    }
                    //    else {
                    //        ShowInfoPopup('The new movement version is not created', 'WarningCancelBtn');
                    //    }
                    //});
                    break;
                case 2:
                    //showWarningPopDialog('Please generate driving instructions before creating movement version from last candidate route version!', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
                    ShowInfoPopup('Please generate driving instructions before creating movement version from last candidate route version!', 'WarningCancelBtn');
                    break;
                case 3:
                    //showWarningPopDialog('Please generate affected structures before creating movement version from last candidate route version!', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
                    ShowInfoPopup('Please generate affected structures before creating movement version from last candidate route version!', 'WarningCancelBtn');
                    break;
                //case 4:
                //    showWarningPopDialog('Please generate annotations before creating movement version from last candidate route version!', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
                //    break;
                //case 5:
                //    showWarningPopDialog('Please generate cautions before creating movement version from last candidate route version!', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
                //    break;
                //case 6:
                //    showWarningPopDialog('Please generate before creating movement version from last candidate route version!', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
                //    break;
                case 7:
                    //showWarningPopDialog('Please generate route description before creating movement version from last candidate route version!', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
                    ShowInfoPopup('Please generate route description before creating movement version from last candidate route version!', 'WarningCancelBtn');
                    break;
                case 8:
                    //showWarningPopDialog('Please generate affected parties before creating movement version from last candidate route version!', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
                    ShowInfoPopup('Please generate affected parties before creating movement version from last candidate route version!', 'WarningCancelBtn');
                    break;
                case 9:
                    // showWarningPopDialog('Please generate affected roads before creating movement version from last candidate route version!', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
                    ShowInfoPopup('Please generate affected roads before creating movement version from last candidate route version!', 'WarningCancelBtn');
                    break;
            }
        });
    }



    var selectedVal;
    var pageNum = 1;
    //Movements related to Structure
    function showStructRelatedMovements() {
        $('#leftpanel').hide();
        $('#tab_10').show();
        $('#tab_10 #pageSizeSelect').change(function () {
            changePageSizeStructMov($(this));
        });
    }

    function load_structRelatedMovements(structureID) {

        $("li").each(function () {
            $(this).addClass("nonactive");

        });

        $("#hdnStructID").val(structureID);
        var url='@Url.Action("SORTInbox","SORTApplication")';
        $.ajax({
            url: url,
            type: 'POST',
            cache: false,
            //async: false,

            data: { structID: structureID },
            beforeSend: function () {
                startAnimation('Loading related movements...');
            },
            success: function (result) {
                $(".tab_content1").hide();
                $('#leftpanel').hide();
                $('#tab_10').show();

                var label = "<h4 style='color: #e80040; dispaly:block'>Movement inbox related to " + $("#hdnStructCode").val() + "</h4><br />";
                $('#tab_10').html(label);
                $('#tab_10').append($(result).find('#div_sort_inbox').html());
                //$('#tab_10').html($(result).find('#div_sort_inbox').html());
                //stopAnimation();
                $("li[id='10']").show();
                //$("li[id='10']").removeClass("nonactive");
                $("li[id='10']").addClass('t');
                fillPageSizeSelectStructMov();
                removeHLinksStructMov();
                PaginateGridStructMov();
                //RemoveDeleteStructMov();
                RemoveLinksStructMov();
                $('#tab_10 #pageSizeSelect').change(function () {
                    changePageSizeStructMov($(this));
                });

                CheckSessionTimeOut();
            },
            complete: function () {
            stopAnimation();
        }
        });

    }


    function changePageSizeStructMov(_this) {
        var pageSize = $(_this).val();
        $('#tab_10 #pageSize').val(pageSize);
        $('#tab_10 #pageSizeSelect').val(pageSize);
        loadStructRelatedMov(pageNum, pageSize);
    }

    function fillPageSizeSelectStructMov() {
        selectedVal = $('#tab_10 #pageSize').val();
        $('#tab_10 #pageSizeSelect').val(selectedVal);
    }

    function removeHLinksStructMov() {
        $('#tab_10').find('.pagination').find('li a').removeAttr('href').css("cursor", "pointer");
    }

    //function Pagination
    function PaginateGridStructMov() {
        //method to paginate through page numbers
        $('#tab_10').find('.pagination').find('li').not('.active, .PagedList-skipToLast, .PagedList-skipToNext, .PagedList-skipToFirst, .PagedList-skipToPrevious').find('a').click(function () {
            //var pageCount = $('#TotalPages').val();
            pageNum = $(this).html();
            AjaxPaginationStructMov(pageNum);
            //console.log($('.active a').html());
        });
        PaginateToLastPageStructMov();
        PaginateToFirstPageStructMov();
        PaginateToNextPageStructMov();
        PaginateToPrevPageStructMov();
    }


    //method to paginate to last page
    function PaginateToLastPageStructMov() {
        $('#tab_10').find('.PagedList-skipToLast').click(function () {
            var pageCount = $('#tab_10 #TotalPages').val();
            AjaxPaginationStructMov(pageCount);
        });
    }

    //method to paginate to first page
    function PaginateToFirstPageStructMov() {
        $('#tab_10').find('.PagedList-skipToFirst').click(function () {
            AjaxPaginationStructMov(1);
        });
    }

    //method to paginate to Next page
    function PaginateToNextPageStructMov() {
        $('#tab_10').find('.PagedList-skipToNext').click(function () {
            var thisPage = $('#tab_10').find('.active').find('a').html();
            var nextPage = parseInt(thisPage) + 1;
            AjaxPaginationStructMov(nextPage);
        });
    }

    //method to paginate to Previous page
    function PaginateToPrevPageStructMov() {
        $('#tab_10').find('.PagedList-skipToPrevious').click(function () {
            var thisPage = $('#tab_10').find('.active').find('a').html();
            var prevPage = parseInt(thisPage) - 1;
            AjaxPaginationStructMov(prevPage);
        });
    }

    //function Ajax call fro pagination
    function AjaxPaginationStructMov(pageNum) {
        var structureID = $("#hdnStructID").val();
        var pageSize = $('#tab_10 #pageSizeSelect').val();
        var url = '@Url.Action("SORTInbox","SORTApplication")';
        $.ajax({
            url: url,
            type: 'POST',
            cache: false,
            //async: false,
            data: { page: pageNum, structID: structureID },
            beforeSend: function () {
                startAnimation('Loading related movements...');
            },
            success: function (result) {
                var label = "<h4 style='color: #e80040; dispaly:block'>Movement inbox related to " + $("#hdnStructCode").val() + "</h4><br />";
                $('#tab_10').html(label);
                $('#tab_10').append($(result).find('#div_sort_inbox').html());
                //$('#tab_10').html($(result).find('#div_sort_inbox').html());
                $('#tab_10 #pageSizeSelect').val(pageSize);
                $('#tab_10 #pageSizeSelect').change(function () {
                    changePageSizeStructMov($(this));
                });
                removeHLinksStructMov();
                PaginateGridStructMov();

                CheckSessionTimeOut();
            },
            error: function (xhr, textStatus, errorThrown) {
            },
            complete: function () {
                stopAnimation();
            }
        });
        //function changePageSizeStructMov(_this) { }
    }

    function RemoveLinksStructMov() {
        $('#tblSortInbox').find(".link green").removeAttr('href').css("cursor", "pointer");
    }

    function loadStructRelatedMov(pageNum, pageSize) {
        var structureID = $("#hdnStructID").val();
        var url = '@Url.Action("SORTInbox","SORTApplication")';

        $.ajax({
            url: url,
            type: 'POST',
            cache: false,
            //async: false,
            data: { page: pageNum, pageSize: pageSize, structID: structureID },
            beforeSend: function () {
                startAnimation('Loading related movements...');
            },
            success: function (result) {
                var label = "<h4 style='color: #e80040; dispaly:block'>Movement inbox related to " + $("#hdnStructCode").val() + "</h4><br />";
                $('#tab_10').html(label);
                $('#tab_10').append($(result).find('#div_sort_inbox').html());
                //$('#tab_10').html($(result).find('#div_sort_inbox').html());
                $('#tab_10 #pageSize').val(pageSize);
                $('#tab_10 #pageSizeSelect').val(pageSize);
                $('#tab_10 #pageSizeSelect').change(function () {
                    changePageSizeStructMov($(this));
                });
                //$('#pagesize').val(pageSize);
                AjaxPaginationStructMov(1);
                removeHLinksStructMov();
                PaginateGridStructMov();
                RemoveLinksStructMov();
                CheckSessionTimeOut();
            },
            error: function (xhr, textStatus, errorThrown) {
                //other stuff
                location.reload();
            },
            complete: function () {
                $('#tab_10 #pageSizeSelect').val(pageSize);
                stopAnimation();
            }
        });
    }


    function gotoRouteLibrary() {
        $("#RoutePart").show();
        $("#div_Route").show();
        $("#ShowDetail").hide();
        $("#RouteMap").html('');
    }
    function ClosePopUp() {
        $("#dialogue").html('');
        $("#dialogue").hide();
        $("#overlay").hide();
        addscroll();
        stopAnimation();
    }

    //----------------------
    function ShowDetail(orgId, orgName) {
        startAnimation();

        $('#showAffectedparties').html("");
        $('#AffectedStructure_xslt').html('');
        var VR1Applciation = $('#VR1Applciation').val();
        var iscandidatert = $('#IsCandVersion').val();
        var revisionId = "";
        var AnalysisId = "";
        var versionId = "";
        if (iscandidatert == 'True') {
            revisionId = $('#revisionId').val();
            AnalysisId = $('#candAnalysisId').val();
        }
        if (status == "MoveVer") {
            AnalysisId = $('#analysis_id').val();
            revisionId = $('#revisionId').val();
        }
        if (VR1Applciation == "True") {
            revisionId = 0;
            versionId = $('#versionId').val();
        }
        $()

        $('#showAffectedparties').load('../RouteAssessment/GetAffectedStructures?analysisID=' + AnalysisId + '&revisionId=' + revisionId + '&versionId=' + versionId + '&IsVR1=' + VR1Applciation + '&FilterByOrgID=' + orgId,
            {},
            function () {
                stopAnimation()
                $('#SearchStructure').show();
                $('#div_StructureGeneralDetails').hide();

                if ($('#routediv1 td').length > 0) {
                    $('#divNoAffected').hide();
                }
                else {
                    $('#divNoAffected').show();
                }
            });
        //GetAffectedStructures
        $('#Organisation_ID').val(orgId);
        $('#Organisation_Name').val(orgName);
        $('#RBDrivingInstruction').hide();
        $('#RBAnnot').hide();
        $('#RBCaution').hide();
        $('#RBConstraints').hide();
        $('#RBRouteDescrp').hide();
        $('#RBAffectedParty').hide();
        $('#generatebutton').hide();
        $('#RBAffectedRoad').hide();
        $('#RBAffectedRoad_StructureWise').show();

        openDetails('RBAffectStruct', 'affectedstructure');
        HideSpan();
        $('#routeAssessmentBckbtn').show();
        $('#showAffectedparties').show();
    }
    //---------------------
    function BackToMovDetails() {
        $("#SelectCurrentMovementsVehicle1").show();
        $("#SelectCurrentMovementsVehicle2").show();
        $("#App_Candidate_Ver").show();
        $("#divCurrentMovement").show();
        $('#ViewRouetDetail').hide();
        $('#divViewRouetDetail').hide();

        CurreMovemenRouteList($('#tem_ReviosionId').val(), $('#temRList_type').val());
    }
    function BackToCandiRoutDetails() {
        $('#RoutePart').show();
        $('#back_btn_Rt').hide();
        $('#route').hide();
        $("#divCandiRouteDeatils").hide();
        BindRouteParts();//loading new routes
    }
    function ShowRouteDetails(RouteId, RouteType) {
        $("#RouteMap1").html('');
        $("#map").html('');
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: '../Routes/GetCandidateRouteDescMap',
            data: { plannedRouteId: RouteId, routeType: RouteType },
            beforeSend: function (xhr) {
            },
            beforeSend: function () {
                startAnimation();
            },
            success: function (result) {
                $("#SelectCurrentMovementsVehicle1").hide();
                $("#SelectCurrentMovementsVehicle2").hide();
                $("#App_Candidate_Ver").hide();
                $("#divCurrentMovement").hide();
                $('#ViewRouetDetail').show();
                $('#divViewRouetDetail').show();
                ClosePopUp();


                var count = -1, strTr, flag = 0, Index = 0;
                if (result.result != null) {
                    count = result.result.routePathList[0].routePointList.length;

                    for (var i = 0 ; i < count; i++) {
                        if (result.result.routePathList[0].routePointList[i].pointGeom != null || (result.result.routePathList[0].routePointList[i].linkId != null && result.result.routePathList[0].routePointList[i].linkId != 0))
                        { flag = 1; break; }
                        if (result.result.routePathList[0].routePointList[i].pointGeom == null)
                        { flag = 0; break; }
                    }
                    $('#Tabvia1').html('');

                    $('#ShowList1').hide();
                    $("#RouteName1").html(result.result.routePartDetails.routeName);
                    $("#temRouteId").html(result.result.routePartDetails.routeID);
                    if (result.result.routePartDetails.routeDescr != null && result.result.routePartDetails.routeDescr != "")
                        $("#RouteDesc1").html(result.result.routePartDetails.routeDescr);
                    for (var i = 0 ; i < count; i++) {
                        if (result.result.routePathList[0].routePointList[i].pointType == 0)
                            $('#Starting1').html(result.result.routePathList[0].routePointList[i].pointDescr);
                        else if (result.result.routePathList[0].routePointList[i].pointType == 1)
                            $('#Ending1').html(result.result.routePathList[0].routePointList[i].pointDescr);

                        else if (result.result.routePathList[0].routePointList[i].pointType >= 2) {
                            Index = Index + 1;
                            strTr = "<tr ><td style='width:40px'>" + Index + "</td><td>" + result.result.routePathList[0].routePointList[i].pointDescr + "</td></tr>"
                            $('#Tabvia1').append(strTr);
                        }
                    }
                    if (typeof result.result.routePathList[0].routeSegmentList != "undefined" && String(typeof result.result.routePathList[0].routeSegmentList) != "[]") {
                        $("#RouteMap1").load('@Url.Action("A2BPlanning", "Routes", new { routeID = 0 })', function () {

                            if (RouteType == "outline" || RouteType == "Outline") {
                                loadmap('DISPLAYONLY');
                                showSketchedRoute(result.result);
                                // loadmap(10, result.result);
                            }
                            else {
                                $("#map").html('');
                                //  loadmap(7, result.result);
                                //loadmap('DISPLAYONLY', result.result);
                                loadmap('DISPLAYONLY', result.result, null, true);
                                //Changed by sali.
                                /*loadmap(10);
                                showSketchedRoute(result.result);*/
                            }
                        });
                    }
                    else {
                        $("#RouteMap1").load('/Routes/A2BPlanning?routeID=0', function () {

                            loadmap('DISPLAYONLY');
                            showSketchedRoute(result.result);
                        });
                    }
                   // $("#RouteName1").html('Route not available.');
                }
                else if (RouteType == 'planned') {
                    displayCandidateRouteDescMap(RouteId, 'outline');
                }

            },
            complete: function () {
                if ($('#Starting1').html() == '') {
                    $('#trRoute1').hide();
                    $('#trStarting1').hide();
                    $('#trVia1').hide();
                    $('#trEnding').hide();
                }
                else {
                    $('#trRoute1').show();
                    $('#trStarting1').show();
                    $('#trVia1').show();
                    $('#trEnding').show();
                }
                if ($("#RouteDesc1").html() != "") {
                    $('#trHeaderDescription1').show();
                    $('#trdesc1').show();
                }
                else {
                    $('#trHeaderDescription1').hide();
                    $('#trdesc1').hide();
                }
                stopAnimation();
            }
        });

        addscroll();
    }

    function ImportRFromView() {
        var routeId = $("#temRouteId").text();
        var routeName = $("#RouteName1").text();
        var routetype = $('#HfRouteType').val();
        if ($('#SortStatus').val() == "CreateSO")
            Importrouteinapp(routeId, routetype);
        else
            ImportRoute(routeId, routeName);
    }

    function ShowAdvancedveh() {

        if ($('#div_advanced_filter').is(':visible')) {
            $('#div_advanced_filter').slideUp('slow');
        }
        else {
            $('#div_advanced_filter').show('fast', function () {
                if ($('#SearchPrevMoveVeh').val() == "No") {
                    $("html,body").animate({ scrollTop: $(document).height() }, 1000);
                }
            });
        }
    }


    function StruRelatedMov_viewDetails(VAnalysisId, VPrj_Status, Vhauliermnemonic, Vesdalref, Vprojectid) {
        $("#StruRelatedMov").hide();
        $("#btnBackToStruRelatedMov").show();
        $("#header-fixed").hide();
        $("#tableheader").hide();

        $("#PrevMove_projectid").val(Vprojectid);
        $("#PrevMove_hauliermnemonic").val(Vhauliermnemonic);
        $("#PrevMove_esdalref").val(Vesdalref);
        $("#IsPrevMoveOpion").val(true);

        $('#RoutePart').hide();
        $('#SelectCurrentMovementsVehicle').show();
        $('#divCurrentMovement').html('');
        WarningCancelBtn();
        startAnimation();
        var wstatus = 'ggg';
        $('#StruRelatedMov_viewDetails').load('../SORTApplication/SORTAppCandidateRouteVersion?AnalysisId=' + VAnalysisId + '&Prj_Status=' + VPrj_Status + '&hauliermnemonic=' + Vhauliermnemonic + '&esdalref=' + Vesdalref + '&projectid=' + Vprojectid + '&IsCandPermision=' + 'true' + '&IsCurrentMovenet=true', {},
            function () {

                //ClosePopUp();
                $("#dialogue").html('');
                $("#dialogue").hide();
                $("#overlay").hide();
                addscroll();
                stopAnimation();
            });
    }
    function BackToStruRelatedMov() {

        $("#StruRelatedMov").show();
        $("#btnBackToStruRelatedMov").hide();

        $("#StruRelatedMov_viewDetails").html('');
        $("#SelectCurrentMovementsVehicle2").html('');
        $("#SelectCurrentMovementsVehicle1").html('');
    }
    $('body').on('click', '#img-EditComponents', function (e) {
        debugger;
        e.preventDefault();
        var VehicleId = $(this).data('VehicleId');
        EditComponents(VehicleId);
    });
    function EditComponents(VehicleId) {
        CloseSuccessModalPopup();
        $.ajax({
            url: '../VehicleConfig/EditConfiguration',
            type: 'GET',
            cache: false,
            async: false,
            data: { vehicleId: VehicleId, isApplication: true, isEditVehicleInSoProcessing: true, isCandidate: true },
            beforeSend: function () {
                startAnimation();
            },
            success: function (response) {
                //$('#vehicle_edit_section').hide();
                //$('#vehicle_Component_edit_section').hide();
                //$('#vehicle_Create_section').hide();
                CandidateBackFlag = 1;
                SubStepFlag = 2.5;
                //$('#vehicle_Component_edit_section').show();
                $('#tab_2').html('<div id="vehicle_Component_edit_section"></div>');
                $('#tab_2 #vehicle_Component_edit_section').html(response);

                $('.new-vehicle').unwrap('#banner-container');

            },
            complete: function () {
                stopAnimation();
            }
        });
    }
    $('body').on('click', '#img-DeleteSelectedNotifComponent', function (e) {
        e.preventDefault();
        var VehicleId = $(this).data('VehicleId');
        var VehicleName = $(this).data('VehicleName');
        DeleteSelectedNotifComponent(this,VehicleId, VehicleName);
    });
    $('body').on('click', '#img1-DeleteSelectedNotifComponent', function (e) {
        e.preventDefault();
        var VehicleId = $(this).data('VehicleId');
        var VehicleName = $(this).data('VehicleName');
        DeleteSelectedNotifComponent(this, VehicleId, VehicleName);
    });
    function DeleteSelectedNotifComponent(_this, id, name) {
        delvehicleName = name;
        vehID = id;
        var Msg = "Do you want to delete '" + "" + "'" + delvehicleName + "'" + "" + "' ?";
        ShowDialogWarningPop(Msg, 'NO', 'YES', 'WarningCancelBtn', 'DeleteDNComponent', 1, 'warning');           //reference to popup

    }
    //function to delete component
    function DeleteDNComponent() {
       var  isVR1 = true;
        var isNotif = false;
        var notifid = $("#NotificatinId").val() ? $('#NotificatinId').val() : 0;
        if (notifid != 0) {
            isNotif = true;
        }
        var contentNum = $('#CRNo').val();
        var routePartID = $('#RoutePartId').val();
        var movever = $('#MoveVersionStatus').val();
        var socode = $('#SOVehicleCode').val();
        var agrd = 0;
        $.ajax({
            url: "../Application/DeleteSelectedVehicleComponent",
            type: 'POST',
            cache: false,
            async: false,
            data: { vehicleId: vehID, isVR1: true, isNotif: isNotif, NotificationId: notifid },
            beforeSend: function () {
                startAnimation();
                //WarningCancelBtn();
            },
            success: function (result) {

                if (result.Success) {
                    var msg1 = "Configuration  '" + "" + "'" + delvehicleName + "'" + "" + "'  deleted successfully";
                    if (status != "CandidateRT") {
                        showWarningPopDialog(msg1, 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
                        if ((movever == 305004 || movever == 305005 || movever == 305006) && socode == 241002) {
                            agrd = 1;
                        }

                        $('#tab_2').load('../Application/ListImportedVehicleConfiguration?routepartId=' + routePartID + '&ContentRefNo=' + contentNum + '&IsNotif=' + true + '&AgreedSO=' + agrd, function () {
                            $('#div_notification_vehicle').find('input:radio').attr('checked', false);
                        });
                    }
                    else {
                        showWarningPopDialog(msg1, 'Ok', '', 'CandidateVehicleDeleteSuccess', '', 1, 'info');
                    }
                }
                else {
                    var msg1 = "Configuration  '" + "" + "'" + delvehicleName + "'" + "" + "' is not deleted successfully, please try again";

                    showWarningPopDialog(msg1, 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                //other stuff
                location.reload();


            },
            complete: function () {
                $('.loading').hide();
                $("#overlay").hide();
                //cntTr.remove();
            }

        });

        /*if ((movever == 305004 || movever == 305005 || movever == 305006) && socode == 241002) {
            agrd = 1;
        }

        $('#tab_2').load('../Application/ListImportedVehicleConfiguration?routepartId=' + routePartID + '&ContentRefNo=' + contentNum + '&IsNotif=' + true + '&AgreedSO=' + agrd, function () {
            $('#div_notification_vehicle').find('input:radio').attr('checked', false);
        });*/
    }

    function CandidateVehicleDeleteSuccess() {
        WarningCancelBtn();
        Show_CandidateRTVehicles();
    }

    function ViewVehicleDetails(VehicleId) {
        var ComponentCntrId = "viewcomponentdetails_" + VehicleId;
        if (document.getElementById(ComponentCntrId).style.display !== "none") {
            document.getElementById(ComponentCntrId).style.display = "none"
            document.getElementById('chevlon-up-icon_' + VehicleId).style.display = "none"
            document.getElementById('chevlon-down-icon_' + VehicleId).style.display = "block"
        }
        else {
            document.getElementById(ComponentCntrId).style.display = "block"
            document.getElementById('chevlon-up-icon_' + VehicleId).style.display = "block"
            document.getElementById('chevlon-down-icon_' + VehicleId).style.display = "none"
        }
    }

    //function changePageSizeStructMov(_this) { }
    function ViewAppGeneralDetails() {
        if (document.getElementById('viewappgeneral').style.display !== "none") {
            document.getElementById('viewappgeneral').style.display = "none"
            document.getElementById('chevlon-up-icon1').style.display = "none"
            document.getElementById('chevlon-down-icon1').style.display = "block"
        }
        else {
            document.getElementById('viewappgeneral').style.display = "block"
            document.getElementById('chevlon-up-icon1').style.display = "block"
            document.getElementById('chevlon-down-icon1').style.display = "none"
        }
    }
    $('body').on('click', '#view-routeandvehicles', function (e) {
        e.preventDefault();
        var var1 = $(this).data('var1');
        ViewRouteAndVehicles(var1);
    });
    function ViewRouteAndVehicles(ComponentCntrId) {

        if (document.getElementById(ComponentCntrId).style.display !== "none") {
            document.getElementById(ComponentCntrId).style.display = "none"
            document.getElementById('chevlon-up-icon3').style.display = "none"
            document.getElementById('chevlon-down-icon3').style.display = "block"
        }
        else {
            document.getElementById(ComponentCntrId).style.display = "block"
            document.getElementById('chevlon-up-icon3').style.display = "block"
            document.getElementById('chevlon-down-icon3').style.display = "none"
        }
    }
    function copyroute() {
        $('#btnfinishcopy').show();
        // fillroutes();
        $('.editnmode').show();
        $('.hiddenmode').hide();
        $('#copy').hide();

        $('.hiddenmode').each(function () {
            var name = $.trim($(this).html());

            $(this).parent().find('.drp_route_parts option').filter(function () {
                return ($.trim($(this).text()) == name);
            }).prop('selected', true);
        });

    }

    function CandidateVehicleBackButton() {

        switch (CandidateBackFlag) {
            case 0://vehicle create
                if (CandidateBackSubFlag == 0.1 || CandidateBackSubFlag == 0.2) {
                    CandidateBackSubFlag = 0;
                    var guid = $('#GUID').val();
                    FillComponentDetailsForConfig(guid,'');
                }
                else {
                    Show_CandidateRTVehicles();
                }
                break;
            case 1: //vehicle edit
                if (CandidateBackSubFlag == 1.1) {
                    CandidateBackSubFlag = 0;
                    $('#tab_2').show();
                    $('#vehicle_Component_edit_section').show();
                    $('#tab_7').html('');
                    $('#tab_7').hide();
                    $('#divbtn_candidateVehicleNext').remove();
                    $('#tab_2').append('<div id="divbtn_candidateVehicleEdit" class="row mt-4" style="float: right;"><button class="btn outline-btn-primary SOAButtonHelper mr-2 mb-2" id="show-candidate" >BACK</button></div>');

                }
                else {
                    Show_CandidateRTVehicles();
                }
                break;
        }
    }
