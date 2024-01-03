    $(document).ready(function () {
       
        $(".btnrouteing").on('click', Importrouteinnotification);
        $("#displaypopup").on('click', displayPopupList);
        $("#btn_cancel").on('click', UsePreviousMovement);
        $(".getting").on('click', Gettingdetails);
        $(".getsoapp").on('click', GetSOapplication);
        $("#btnmovsaveannotation").on('click', btn_save_annotation);
        $(".getsortroute").on('click', GetSORTVehRoutes );
        $(".getvr1routes").on('click', GetVrOneRoutes);
        $(".getvr1route").on('change', GetVrOneRoutes);
        $(".displaysoroute").on('click', displaySoRouteDescMapforNotify);
        //----------------$(document).ready(function () { external script load function start here }-----------
      //  RouteDetailsReady();
        //----------------$(document).ready(function () { external script load function end here }-------------
        $('#Suggestedroute').hide();
        $('#Suggestedroute1').hide();

       var as = $('#hf_soapp').val(); 

if($('#hf_soap').val() == 'true')
        {
            $("#singelradiobtn").click();
        }
if($('#hf_VrOneRout').val() == 'true' && '@ViewBag.soapp'=='true')
        {
            $("#singelradiobtn").click();
        }

        IfOneRecord();

        $('#check_1').click();

    });
    function Gettingdetails(e) {
        var arg1 = e.currentTarget.dataset.arg1;
        var arg2 = e.currentTarget.dataset.arg2;
        Get(arg1, arg2);
    }
    function GetSOapplication(e) {
        var arg1 = e.currentTarget.dataset.arg1;
        var arg2 = e.currentTarget.dataset.arg2;
        GetSOapp(arg1,arg2);
    }

    function Importrouteinnotification(e) {
        var arg1 = e.currentTarget.dataset.arg1;
        var arg2 = e.currentTarget.dataset.arg2;
        Importrouteinnotif(arg1, arg2);
    }


    function GetVrOneRoutes(e) {
        var arg1 = e.currentTarget.dataset.arg1;
        GetVrOneRoute(arg1);
    }

    function GetSORTVehRoutes(e) {
        var arg1 = e.currentTarget.dataset.arg1;
        var arg2 = e.currentTarget.dataset.arg2;
        GetSORTVehRoute(arg1,arg2);
    }

    function displaySoRouteDescMapforNotify(e) {
        var arg1 = e.currentTarget.dataset.arg1;
        var arg2 = e.currentTarget.dataset.arg2;
        displaySoRouteDescMapforNotification(arg1, arg2);
    }
    function IfOneRecord() {
        if ('@Model.Count' == 1){
            var Vpartid = 0;
            var VRouteType = "";
            Vpartid = $('#radio_chk').val();
            V_RouteType = $('#radio_chk').data('type');



            if ('@Model.Count' == 1){




if($('#hf_soapp').val() ==  'True')
                {
                    var rt_type= '@Model[0].RouteType';
                    GetSOapp(Vpartid,rt_type);
                }
if($('#hf_approute').val() ==  'True')
                {

                    displaySoRouteDescMapforNotification(Vpartid,V_RouteType);

                }
if($('#hf_VrOneRout').val() ==  'Null' && '@ViewBag.VrOneRoute' == 'True' )
                {
                    GetVrOneRoute(Vpartid) ;
                }
                else
                {
if($('#hf_SORTVehRoute').val() ==  'True')
                    {
                        GetSORTVehRoute(Vpartid,V_RouteType) ;

                    }
                    else{
                        Get(Vpartid,V_RouteType);

                    }
                }


    }
        }
    }
    $('#back-link').click(function () {
        
        history.go(-1);
    });
    function GetSOapp(partid,rt_type)
    {
        
        var FlagAppVeh=0;
        var isVR1 = $('#vr1appln').val();
        if (isVR1 == 'True')
        {
            FlagAppVeh=1;//for VR1 application
        }
        else
        {
            FlagAppVeh=2; //for SO application
        }
        //to check this is call from SO application to fetch vehicles list
        $.ajax
        ({
            url:'../Application/appvehconfigImport',
            type:'POST',
            datatype:'json',
            async:false,
            data:{partid:partid,FlagSOAppVeh:FlagAppVeh, RouteType:rt_type},
            beforeSend:function(){
                startAnimation();
            },
            success: function(page)
            {

                $('#routelist').html('');
                $('#routelist').show();

                $('#routelist').html($(page).find('#divappvehcongi1').html(), function ()
                {
                });
            },
            error: function(){
                location.reload();
            },
            complete: function(){
                stopAnimation();
            }
        });
    }
    function Get(PartId, routeType) {
        
        //$("#leftpanel").load('../Application/SoRoute', { pageflag: 2 }, function () {
        //    $('#leftpanel_quickmenu').html('');
        //    $("#leftpanel").show();
        //    $('#TabOption').hide();
        //});
if($('#hf_NotifShowVe').val() == 'True'){
            $.ajax({
                url:'../Application/ApplicationVehicle',
                type:'POST',
                datatype:'json',
                async:true,
                data:{PartId:PartId,IsVRVeh:true},
                success: function(page){
                    startAnimation();
                    //$('#ApplicationVehicle').show();
                    //$('#ApplicationVehicle').html(page);
                    $('#ApplicationParts').show();
                    $('#ApplicationParts').html(page);
                },
                error: function(){
                    location.reload();
                },
                complete: function(){
                    stopAnimation();
                }
            });
        }
        else{
            var revisionId=$('#RevisionID').val();
            var version_id=$('#VersionID').val();
            $.ajax({
                url:'../Application/ApplicationPartDetails',
                type:'POST',
                datatype:'json',
                async:true,
                data:{partid:PartId,routeType:routeType,RevisionID:revisionId,Version_ID:version_id},
                success: function(page){
                    startAnimation();
                    $('#ApplicationVehicle').show();
                    $('#ApplicationVehicle').html(page);
                },
                error: function(){
                    location.reload();
                },
                complete: function(){
                    stopAnimation();
                }
            });
          //  GetRoutePartIDForHuilierTab(PartId);
            NewGet(PartId,routeType);
            $('#divMap1').hide();
            $('#ShowDetail').hide();
            $('#PartDetail').hide();
            $('#Suggestedroute').hide();


        }
    }
    function GetRouteForHuilierTab(PartId) {
        
        $('#Suggestedroute').show();
        var revisionId = $('#RevisionID').val();
        var version_id=$('#VersionID').val();
        $('#tab_4').show();
        // $('#tab_4').show();
        $('#PartDetail').load('../Application/ApplicationPartDetails?partid=' + PartId + '&RevisionID=' + revisionId+ '&Version_ID=' + version_id);
        GetRoutePartIDForHuilierTab(PartId);
        //  Get(PartId);
    }

    function GetRoutePartIDForHuilierTab(PartId)
    {
        
        $("#overlay").show();
        $("#dialogue").hide();
        $('.loading').show();
        $.ajax({
            type: 'POST',
            async: true,
            dataType: 'json',
            url: '../Routes/GetAgreedRoute',
            data: { routePartId: PartId },
            beforeSend: function (xhr) {
                startAnimation();
            },
            success: function (result) {
                if(result != null){


                    var count = result.routePathList[0].routePointList.length, strTr, flag = 0, Index = 0;

                    for (var i = 0 ; i < count; i++) {
                        if (result.routePathList[0].routePointList[i].pointGeom != null || result.routePathList[0].routePointList[i].linkId != null)
                            flag = 1;
                    }
                    if (flag == 1 || '@Session["RouteFlag"]' == 1 || '@Session["RouteFlag"]' == 3) {
                    $('#Tabvia').html('');
                    $("#ShowDetail").show();
                    $("#div_Route").hide();

                    $("#RouteName").html(result.routePartDetails.routeName);
                    if (result.routePartDetails.routeDescr != null && result.routePartDetails.routeDescr != "")
                        $("#RouteDesc").html(result.routePartDetails.routeDescr);
                    for (var i = 0 ; i < count; i++) {
                        if (result.routePathList[0].routePointList[i].pointType == 0)
                            $('#Starting').html(result.routePathList[0].routePointList[0].pointDescr);
                        else if (result.routePathList[0].routePointList[i].pointType == 1)
                            $('#Ending').html(result.routePathList[0].routePointList[1].pointDescr);

                        else if (result.routePathList[0].routePointList[i].pointType >= 2) {
                            Index = Index + 1;
                            strTr = "<tr ><td style='width:40px'>" + Index + "</td><td>" + result.routePathList[0].routePointList[i].pointDescr + "</td></tr>"
                            $('#Tabvia').append(strTr);
                        }
                    }
                    $("#map").html('');
                    $("#tab_3 #map").load('@Url.Action("A2BPlanning", "Routes", new { routeID = 0 })', function () {
                    // $("#map").addClass("context-wrap olMap");
                    var listCountSeg = 0;
                    //if (result.routePathList[0].routeSegmentList != null)
                    //    listCountSeg = 1;
                    for (var i = 0; i < result.routePathList.Count; i++) {
                        listCountSeg = result.routePathList[i].routeSegmentList.Count;
                        if (listCountSeg > 0)
                            break;
                    }

                    if (listCountSeg == 0) {
                        if (result.routePathList[0].routeSegmentList != null)
                            listCountSeg = 1;
                    }

                    if (listCountSeg == 0)// if ('@Session["RouteFlag"]' == 2)
                    {
                        //loadmap(2);
                        //showSketchedRoute(result);
                        loadmap('DISPLAYONLY', result.result);
                    }
                    else {
                        loadmap('DISPLAYONLY', result);
                    }
                })
            }
            else {

                $("#RouteMap").html('');
                $("#ShowDetail").show();
                $("#div_Route").hide();
                $("#RouteName").html(result.routePartDetails.routeName);
                $('#Tabvia').html('');
                if (result.routePartDetails.routeDescr != null && result.routePartDetails.routeDescr != "")
                    $("#RouteDesc").html(result.routePartDetails.routeDescr);
                for (var i = 0 ; i < count; i++) {

                    if (result.routePathList[0].routePointList[i].pointType == 0)
                        $('#Starting').html(result.routePathList[0].routePointList[0].pointDescr);
                    else if (result.routePathList[0].routePointList[i].pointType == 1)
                        $('#Ending').html(result.routePathList[0].routePointList[1].pointDescr);

                    else if (result.routePathList[0].routePointList[i].pointType >= 2) {
                        Index = Index + 1;
                        strTr = "<tr ><td style='width:40px'>" + Index + "</td><td>" + result.routePathList[0].routePointList[i].pointDescr + "</td></tr>"
                        $('#Tabvia').append(strTr);
                    }
                }
                $("#RouteMap").html('No visual representation of route available.');
            }
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
                stopAnimation();
            }
        });
    }
    function GetRoute(PartId, RouteType) {
        
            $('#Suggestedroute1').show();
            var revisionId = $('#RevisionID').val();
            $('#tab_4').show();
            // $('#tab_4').show();
            $('#PartDetail1').load('../Application/ApplicationPartDetails?partid=' + PartId + '&routeType='+RouteType+'&RevisionID=' + revisionId+ '&HideVehDet=' + true);
            GetRoutePartID(PartId,RouteType);
            //  Get(PartId);
        }
    function GetRoutePartID(PartId,RouteType) {
        
            $("#overlay").show();
            $("#dialogue").hide();
            $('.loading').show();
            $.ajax({
                type: 'POST',
                dataType: 'json',
               // url: '../Routes/GetAgreedRoute',
                url: '../Routes/GetSoRouteDescMap',
                data: { plannedRouteId: PartId, routeType: RouteType},
                beforeSend: function (xhr) {
                    startAnimation();
                },
                success: function (result) {
                    $("#tab_3 #map").load('@Url.Action("A2BPlanning", "Routes", new { routeID = 0 })', function () {
                        $("#map").addClass("context-wrap olMap");
                        if(RouteType == "planned")
                            loadmap('A2BPLANNING',result.result);
                        else{
                            loadmap('A2BPLANNING',result.result);
                          //  showOutlineRoute(result.result);
                        }
                    })
                    stopAnimation();
                }
            });
        }
    function GetVrOneRoute(partId) {
        
        $('#part_Id').val(partId);
        $('#form_vrone_route_parts').submit();
    }
    function LoadOtherMovements(result)
    {
        
        $('#routelist').html('');
        $('#routelist').show();
        $('#AffectedStructures').find('#btn_cancel').remove();
        $('#routelist').html(result);
    }

    //SORT Mov Version route & vehicle shown here

    function GetSORTVehRoute(PartId,routeType) {
        
        var version_id=$('#VersionID').val();
        $.ajax({
            url:'../Application/ApplicationPartDetails',
            type:'POST',
            datatype:'json',
            async:true,
            data:{partid:PartId,routeType:routeType,Version_ID:version_id},
            success: function(page){
                startAnimation();
                $('#ApplicationVehicle').show();
                $('#ApplicationVehicle').html(page);
            },
            error: function(){
                //location.reload();
            },
            complete: function(){
                stopAnimation();
            }
        });
        //  GetRoutePartIDForHuilierTab(PartId);
        NewGet(PartId,routeType);
        $('#divMap1').hide();
        $('#ShowDetail').hide();
        $('#PartDetail').hide();
        $('#Suggestedroute').hide();
        $.ajax({
            url: '../Application/ApplicationVehicle',
            type: 'POST',
            datatype: 'json',
            async: false,
            data: { PartId: PartId, SORTMOV: true },
            success: function (page) {
                        $('#Vehicle_sort').show();
                        $('#Vehicle_sort').html(page);

                    },
                    error: function () {
                        //location.reload();
                    },
                    complete: function () {

                    }

        });

                $('#TRbtnBackToRouteList1').hide();
    }

    function btn_save_annotation() {
        var analysis_id = $('#analysis_id').val();
        var revisionId = $('#revisionId').val();
        var versionId = $('#versionId').val();
        startAnimation();
        $('#overlay_load').addClass("ZINdexMax");
        var rdetails = getRouteDetails();
        $.ajax({
            url: '/SORTApplication/SaveMovAnnotation',
            type: 'POST',
            // async: false,
            //contentType: 'application/json; charset=utf-8',
            processData: false,
            data: JSON.stringify({ plannedRoutePath1: rdetails, revisionId: revisionId, analysisId: analysis_id, versionId: versionId}),
            beforeSend: function () {
                startAnimation();
            },
            success: function (val) {
                $('#btnmovsaveannotation').hide();
                if (val != 0) {
                    ShowInfoPopup("Annotation(s) saved successfully");
                }
                else {
                    ShowInfoPopup("Annotation(s) saved successfully");
                }
            },
            error: function (err) {
                ShowErrorPopup("Annotation(s) are not saved.");
            },
            complete: function () {
                stopAnimation();
                $('#overlay_load').removeClass("ZINdexMax");
            }
        });
    }
    function NewGet(RouteId, RouteType) {
        $("#ShowDetail1").show();
        $('#Vehicle').show();
        $("#map").html('');
        var str = '';
        if (RouteType != "")
            str = RouteType;
        var HfRouteType = $('#HfRouteType1').val();
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
                    var PathListCount = 0;
                    PathListCount = result.result.routePathList.length - 1;

                    var count = result.result.routePathList[0].routePointList.length, strTr, flag = 0, Index = 0;
                    for (var i = 0 ; i < count; i++) {
                        if (result.result.routePathList[0].routePointList[i].pointGeom != null )//|| result.result.routePathList[0].routePointList[i].linkId != null
                            flag = 1;
                    }
                    $("#RouteDesc1").html('');
                    $("#RouteName1").html('');
                    $('#Starting1').html('');
                    $('#Ending1').html('');
                    $('#Tabvia1').html('');
                    if (flag == 1 || '@Session["RouteFlag"]' == 1 || '@Session["RouteFlag"]' == 3){
                            $('#Tabvia1').html('');
                            $("#ShowDetail1").show();
                            $('#Vehicle').show();
                            $("#div_Route1").hide();

                            $("#RouteName1").html(result.result.routePartDetails.routeName);
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
                            $("#map").html('');
                            $("#tab_3 #map").load('@Url.Action("A2BPlanning", "Routes", new { routeID = 0 })', function () {
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
                                if (RouteType == 'outline')// if ('@Session["RouteFlag"]' == 2)
                                {
                                    loadmap('DISPLAYONLY');
                                    showSketchedRoute(result.result);
                                   // loadmap(10,result.result);
                                }
                                else {
                                   var chk_status = $('#CheckerStatus').val() ? $('#CheckerStatus').val() : 0;
                                    var sort_user_id = $('#SortUserId').val() ? $('#SortUserId').val() : 0;
                                    var checker_id = $('#CheckerId').val() ? $('#CheckerId').val() : 0;
                                    var MovLatestVer = $('#MovLatestVer').val();
                                    var MovVersion = $('#versionno').val();
                                    var proj_status=$('#AppStatusCode').val();
if($('#hf_SORTVehRout').val() == 'False' ){
                                        loadmap('DISPLAYONLY', result.result,null,false);
                                    }
                                    else if(status!="MoveVer")
                                    {
                                        loadmap('DISPLAYONLY', result.result);
                                    }
                                    else if(status =="MoveVer" && MovLatestVer == MovVersion) {
                                        if((chk_status == 301002 || chk_status == 301008 || chk_status == 301005) && (sort_user_id != checker_id))
                                            loadmap('DISPLAYONLY', result.result);
                                        else if(chk_status == 301006)
                                            loadmap('DISPLAYONLY', result.result);
                                        else if(proj_status == 307012 || proj_status == 307011)
                                            loadmap('DISPLAYONLY', result.result);
                                        else
                                            loadmap('DISPLAYONLY_EDITANNOTATION', result.result);
                                    }
                                    else if(status =="MoveVer") {
                                        loadmap('DISPLAYONLY', result.result);
                                    }
                                    else
                                    {
                                        loadmap('DISPLAYONLY_EDITANNOTATION', result.result);
                                    }
                                }
                            })
                        }
                        else {

                            $("#map").html('');
                            $("#ShowDetail1").show();
                            $('#Vehicle').show();

                            $("#RouteName1").html(result.result.routePartDetails.routeName);
                            $('#Tabvia1').html('');
                            if (result.result.routePartDetails.routeDescr != null && result.result.routePartDetails.routeDescr != "")
                                $("#RouteDesc1").html(result.result.routePartDetails.routeDescr);
                            for (var i = 0 ; i < count; i++) {

                                if (result.result.routePathList[0].routePointList[i].pointType == 0)
                                    $('#Starting1').html(result.result.routePathList[0].routePointList[0].pointDescr);
                                else if (result.result.routePathList[0].routePointList[i].pointType == 1)
                                    $('#Ending1').html(result.result.routePathList[0].routePointList[1].pointDescr);

                                else if (result.result.routePathList[0].routePointList[i].pointType >= 2) {
                                    Index = Index + 1;
                                    strTr = "<tr ><td style='width:40px'>" + Index + "</td><td>" + result.result.routePathList[0].routePointList[i].pointDescr + "</td></tr>"
                                    $('#Tabvia1').append(strTr);
                                }
                            }
                            $("#map").html('No visual representation of route available.');
                        }
                    }
                    else
                    $("#RouteName1").html('Route not available.');
                    if ($('#Starting1').html() == '') {
                        $('#trRoute1').hide();
                        $('#trStarting1').hide();
                        $('#trVia1').hide();
                        $('#trEnding1').hide();
                    }
                    else {
                        $('#trRoute1').show();
                        $('#trStarting1').show();
                        $('#trVia1').show();
                        $('#trEnding1').show();
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

            $("#tdBtn1").html('');

        $("#tdBtn1").html('<button class="btn_reg_back next btngrad btnrds btnbdr" id=displaypopup  aria-hidden="true" data-icon="" type="button" onclick="displayPopupList()">Back</button>  <button onclick="Importrouteinnotif(' + RouteId + ',' + $("#RouteName").html() + '"")" arg1="' + RouteId + '" arg2="' + $("#RouteName").html() + '" class="tdbutton next btngrad btnrds btnbdr btnroutein"  aria-hidden="true" data-icon="&#xe0f4;" >Import</button>');


        }
