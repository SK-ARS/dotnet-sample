    var getValue = 0;
    var date = 0;
    var dropSort = "";
    $(document).ready(function () {       
        $("#dropSort").change(function () {
            getValue = $(this).val();
            if (getValue != "") {
                $('#sortnameerror').text('');
            }
            $("#btnvalidAllocate").on('click', validAllocate);
            $("#btnremoveAllocateButton").on('click', removeAllocateButton);
        });
if($('#hf_planner_user_id').val() ==  '') {
            $("#dropSort").val('@ViewBag.planner_user_id');
            getValue = $('#dropSort').val();
        }
if($('#hf_date').val() ==  '') {
            $('#WorkDueDate').val('@ViewBag.date');
        }
        function setDatepickerPos(input, inst) {
            var rect = input.getBoundingClientRect();
            setTimeout(function () {
                var scrollTop = $("body").scrollTop();
                inst.dpDiv.css({ top: rect.top + input.offsetHeight + scrollTop });
            }, 0);
        }
        $('#WorkDueDate').datepicker({
            dateFormat: "dd/mm/yy",
            changeMonth: true,
            changeYear: true,
            inline: true,
            beforeShow: function (input, inst) {
                setDatepickerPos(input, inst);
            },
        });
    });

    $('#WorkDueDate').change(function () {
        $('#sortdateerror').text('');
    });



    function validAllocate() {
        if (validate()) {
            saveAllocateUser();
        }

    }

    function validate() {

        dropSort = $("#dropSort :selected").text();
        date = $('#WorkDueDate').val();
        var flag=0;

        if (dropSort == "--Select user--") {
            $('#sortnameerror').text('User is required');
            flag=1;
        }
        if (date == "") {
            $('#sortdateerror').text('Due date is required');
            flag = 1;
        }

        if (flag == 1) {
            return false;
        }
        else {
            $('#sortnameerror').text('');
            $('#sortdateerror').text('');
            return true;
        }

    }

        function saveAllocateUser() {

        var revisionNO  = $('#hf_revisionNo').val(); 
        var hauliermnemonic = $('#hauliermnemonic').val();
        var esdalref = $('#esdalref').val();
            var esdalRef = hauliermnemonic + "/" + esdalref;
            
            var vr1Application = false;
            if ($('#VR1Applciation').val() === "True") {
                vr1Application = true;
            }
        var projectowner = $('#Owner').val();
            var _appstatus = $('#AppStatusCode').val();
            var dataModelPassed = { projectId: projectid, pln_user_id: getValue, due_date: date, revisionNo: revisionNO, DropSort: dropSort, EsdalRef: esdalRef, ProjectOwner: projectowner, firstAllocate: true, isVr1Application: vr1Application, isWorkflow:true };
            
            var result = SORTSORouting(1, dataModelPassed);
            if (result == null || result != "NOROUTE") {
                $.ajax({
                    url: result.route, /*Expected route : ../SORTApplication/AllocateSORTUser */
                    type: 'POST',
                    data: {
                        allocateSORTUserCntrlModel: result.dataJson
                    },
                    dataType:"json",
                    beforeSend: function () {
                startAnimation();
            },
            success: function (result) {
                stopAnimation();
                if (result == "1") {
                    $('#PlannrUserId').val(getValue);
                    $('#Owner').val(dropSort);
                    if (_appstatus == 307001) {
                        $('#AppStatusCode').val(307002);
                        $('#Proj_Status').val("Work In Progress");
                    }
                    ClosePopUp();
                    ShowInfoPopup('Sort user "' + dropSort + '" is allocated to this application.', 'Redirect_ProjectOverview');
                }
                else
                    ShowInfoPopup('Sort user "' + dropSort + '" is not allocated to this application.','WarningCancelBtn');
            },
            error: function (jqXHR, textStatus, errorThrown) {
                stopAnimation();
                showWarningPopDialog('Sort user "' + dropSort + '" is not allocated to this application.', 'Ok', '', 'WarningCancelBtn', '', 1, 'error');
            }
        });
            }

    }

    function ClosePopUp() {
        $("#dialogue").html('');
        $("#dialogue").hide();
        $("#overlay").hide();
        resetdialogue();
        addscroll();
    }
    function RedirectToMovement() {
        var hauliermnemonic = $('#hauliermnemonic').val();
        var esdalref = $('#esdalref').val();
        var revisionno = $('#revisionno').val();
        var versionno = $('#versionno').val();
        var Checker = $('#Checker').val();
        var ApprevId = $('#ApprevId').val();
        var VR1Applciation = $('#VR1Applciation').val();
        WarningCancelBtn();
        $("#leftpanel").hide();
        $('#leftpanel_div').hide();
        $('#leftpanel').hide()
        Owner = dropSort.replace(/ /g, '%20');
        Checker = Checker.replace(/ /g, '%20');
        startAnimation()
        $('#HaulOrg').load('../SORTApplication/SORTProjectOverview?hauliermnemonic=' + hauliermnemonic + '&esdalref=' + esdalref + '&revisionno=' + revisionno + '&versionno=' + versionno + '&Rev_ID=' + ApprevId + '&Checker=' + Checker + '&OwnerName=' + Owner, {},
           function () {
               if (VR1Applciation == "True") {
                   $('#HaulOrg').show();
                   $('#HaulOrg').find('#Candidate_route_version').hide();
                   $('#HaulOrg').find('#Special_order').hide();
               } else {
                   $('#HaulOrg').show();
               }
               stopAnimation()
           });
        addscroll();
    }
