
    $(document).ready(function () {
            CheckOnlyRoutePart();


            $("#RoutePartsDescription").hide();

            if('@Model.Count' == "1")
            {

if($('#hf_IsNotifRouteVehicle').val() ==  "True"){


                    $('#rbtn1').attr('checked',true);
                }
if($('#hf_IsNotifRoute').val() ==  "True"){
                    GetNotifRoutePartID('@Rtpartid');
                    $('#rbtn').attr('checked',true);
                }


            }


        });

    function GetRoutePartID(RoutePartId) {


            $("#RoutePartsDescription").show();
            $("#overlay").show();
            $("#dialogue").hide();
            $('.loading').show();


            $.ajax({
                type: 'POST',
                dataType: 'json',
                url: '../Routes/GetAgreedRoute',
                data: { routePartId: RoutePartId },
                beforeSend: function (xhr) {
                },
                success: function (result) {
                    $("#map").html('');
                    $("#idstrDesc").html('');
                    $("#idEndDesc").html('');
                    $("#Tabvia").html('');

                    $("#map").show();
                    if (result.routePartDetails.routeDescr!=null){
                        $('#trHeaderDescription1').show();
                        $('#trdesc1').show();
                        $('#RouteDesc1').html(result.routePartDetails.routeDescr);
                    }
                    $('#RoutePartsDescription').show();
                    $("#map").load('@Url.Action("A2BPlanning", "Routes", new { routeID = 0 })', function () {
                        $("#map").addClass("context-wrap olMap");
                        loadmap('DISPLAYONLY', result);
                        var count = -1, strTr,Index = 0;
                        if(result != null)
                            count =  result.routePathList[0].routePointList.length;
                        for (var i = 0 ; i < count; i++) {
                            if (result.routePathList[0].routePointList[i].pointType == 0)
                                $('#idstrDesc').html(result.routePathList[0].routePointList[0].pointDescr);
                            else if (result.routePathList[0].routePointList[i].pointType == 1)
                                $('#idEndDesc').html(result.routePathList[0].routePointList[1].pointDescr);
                            else if (result.routePathList[0].routePointList[i].pointType >= 2) {
                                Index = Index + 1;
                                strTr = "<tr ><td style='width:40px'>" + Index + "</td><td>" + result.routePathList[0].routePointList[i].pointDescr + "</td></tr>"
                                $('#Tabvia').append(strTr);
                            }
                        }
                    })
                    stopAnimation();
                }
            });
    }
    function GetDetailNotifRoutePartID(RoutePartId) {

        $("#RoutePartsDescription").show();

        $('#RoutePart').hide();
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: '../Routes/GetAgreedRoute',
            data: { routePartId: RoutePartId },
            beforeSend: function (xhr) {
            },
            success: function (result) {
                $("#map").html('');
                $("#Starting").html('');
                $("#Ending").html('');
                $("#Tabvia").html('');

                $("#map").show();
                $('#RoutePartsDescription').show();
                $("#map").load('@Url.Action("A2BPlanning", "Routes", new { routeID = 0 })', function () {
                        $("#map").addClass("context-wrap olMap");
                        loadmap('DISPLAYONLY', result);
                        var count = -1, strTr,Index = 0;
                        if(result != null)
                            count =  result.routePathList[0].routePointList.length;
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
                    })
                    stopAnimation();
                }
            });
        }
    function GetNotifRoutePartID(Routepartid){

if($('#hf_ViewRout').val() == 'True' ){
            $('#TDaddButtons').html('<button class="btn_reg_back next btngrad btnrds btnbdr" id="btnBackToMovementList"  type="button" data-icon="" aria-hidden="true">Back</button>');
            $('#TDaddButtons').append('<button class="btn_reg_back next btngrad btnrds btnbdr" id="btnImpRouteInDetailNotif" data-id='+Routepartid+'  type="button" data-icon="&#xe0f4;" aria-hidden="true">Import</button>');
        }
       GetRoutePartID(Routepartid);
    }


    function CheckOnlyRoutePart(){
        var len=$('#rbn_routepart').length;
        if(len==1){
            $('#rbn_routepart').prop('checked','checked');
            $('#rbn_routepart').click();
        }
    }

        $(document).ready(function () {
            $('body').on('click', '#rbtn', function (e) {
                e.preventDefault();
                var RoutePartID = $(this).data('RoutePartID');
                GetNotifRoutePartID(RoutePartID);
            });
            $('body').on('click', '#rbtn1', function (e) {
                e.preventDefault();
                var RoutePartID = $(this).data('RoutePartID');
                GetNotifVehicleRoutePartID(RoutePartID);
            });
            $('body').on('click', '#rbn_routepart', function (e) {
                e.preventDefault();
                var RoutePartID = $(this).data('RoutePartID');
                GetRoutePartID(RoutePartID);
            });
            $("#btnBackToMovementList").on('click', BackToMovementList);

            $('body').on('click', '#btnImpRouteInDetailNotif', function (e) {
                e.preventDefault();
                var id = $(this).data('id');
                ImpRouteInDetailNotif(id);
            });
        });
