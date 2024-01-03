    $(document).ready(function () {        
        StepFlag = 4;
        SubStepFlag = 4.1;
        CurrentStep = "Route Details";
        SelectMenu(2);
        $('#confirm_btn').hide();
        $('#back_btn').show();
        $('#IsVehicle').val(false);
        $('#ImportFrom').val('@ViewBag.ImportFrm');
        $('#IsFavouriteRoute').val('@ViewBag.IsFavourite');

if($('#hf_ImportFrm').val() ==  'library') {
            $('#list_heading').text("Select route from library");
            SelectRouteFromLibrary();
        }
if($('#hf_ImportFrm').val() ==  'prevMov') {
            $('#IsVehicle').val(false);
            $('#list_heading').text("Select route from previous movements");
            UseMovement();
        }
    });

    function ImportRouteInAppLibrary(routeID, routetype) {
        var routename = $('#btnrouteimport_' + routeID).data('name');
        var routeType = "PLANNED";
        var AppRevId = $('#AppRevisionId').val() ? $('#AppRevisionId').val() : 0;
        $('#IsRouteModify').val(1);
        $('#IsReturnRoute').val(0);
        $('#IsReturnRouteAvailable_Flag').val(false);
        //added by poonam (13.8.14)
        if ($('#CRNo').val() == undefined) {
            var vr1contrefno = $('#VR1ContentRefNo').val();
        }
        else {
            var vr1contrefno = $('#CRNo').val();// $('#VR1ContentRefNo').val();
        }
        var vr1versionid = $('#AppVersionId').val();

        $.ajax({
            url: '../Routes/ImportRouteFromLibrary',
            type: 'POST',
            async: true,
            cache: false,
            data: { routepartId: routeID, routetype: routetype, AppRevId: AppRevId, CONTENT_REF: vr1contrefno, VersionId: vr1versionid },
            beforeSend: function () {
                startAnimation();
            },
            success: function (result) {               
                LoadContentForAjaxCalls("POST", '../Routes/MovementRoute', { workflowProcess: 'HaulierApplication', apprevisionId: AppRevId, versionId: vr1versionid, isNotif: $('#IsNotif').val(), IsReturnAvailable: $('#IsReturnRoute').val(), IsRouteModify: $('#IsRouteModify').val() }, '#select_route_section');
            },
            error: function (xhr, textStatus, errorThrown) {                
            },
            complete: function () {
                stopAnimation();
            }
        });
    }
    $('body').on('click', '#importroutein', function (e) {
        e.preventDefault();
        var routeid = $(this).data('routeid');
        var routetype = $(this).data('routetype');
        ImportRouteInAppParts(routeid, routetype);
    });
    function ImportRouteInAppParts(RouteId, RouteType) {
		if (RouteType != 'planned') {
            ShowErrorPopup('An outline route cannot be imported to the movement.');
        }
        else {
            $('#IsRouteModify').val(1);
            $('#IsReturnRoute').val(0);
            $('#IsReturnRouteAvailable_Flag').val(false);
            var AppRevId = $('#AppRevisionId').val() ? $('#AppRevisionId').val() : 0;
            var SOVersionID = $('#RevisionID').val() ? $('#RevisionID').val() : 0; //Previous Movement version id
            var PrevMovESDALRefNum = $('#PrevMovESDALRefNum').val();
            var ShowPrevMoveSortRoute = $("#ShowPrevMoveSortRT").val();
            //added by poonam (14.8.14)
            var vr1contrefno = $('#VR1ContentRefNo').val() ? $('#VR1ContentRefNo').val() : null;
            var vr1versionid = $('#AppVersionId').val();
            //-----------

            $.ajax({
                url: '../Routes/ImportRouteFromPrevious',
                type: 'POST',
                async: false,
                cache: false,
                data: { routepartId: RouteId, routeType: RouteType, AppRevId: AppRevId, versionid: vr1versionid, contentref: vr1contrefno, SOVersionId: SOVersionID, PrevMovEsdalRefNum: PrevMovESDALRefNum, ShowPrevMoveSortRoute: ShowPrevMoveSortRoute },
                beforeSend: function () {
                    startAnimation();
                },
                success: function (result) {                   
                    $('#back_btn').show();
                    $('#back_btn_Rt_prv').hide();
                    LoadContentForAjaxCalls("POST", '../Routes/MovementRoute', { workflowProcess: 'HaulierApplication', apprevisionId: AppRevId, versionId: vr1versionid, isNotif: $('#IsNotif').val(), IsReturnAvailable: $('#IsReturnRoute').val(), IsRouteModify: $('#IsRouteModify').val() }, '#select_route_section');
                },
                error: function (xhr, textStatus, errorThrown) {                   
                },
                complete: function () {
                    stopAnimation();
                }
            });
        }
    }   

