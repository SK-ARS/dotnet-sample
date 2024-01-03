    $(document).ready(function () {
        var project_status = $('#Proj_Status').val();
        if (project_status == "Approved") {
            $('#btn_vr1approve').hide();
        }
    });
    function SCreateNew() {
        Create();
        removescroll();
    }

    function Create() {

        $("#dialogue").load('@Url.Action("PartialViewLayout", "VehicleConfig")', function () {
            DynamicTitle('Create Configuration');
            $("#Config-body").load('@Url.Action("CreateConfiguration", "VehicleConfig", new { isApplicationVehicle = true })');
            $("#dialogue").show();
            $("#overlay").show();
            //removescroll();
        });
    }

    function SFromFleet() {


        $('#Vehicle').html('');
        UseFleet();
        $('#leftpanel').html('');
        showleftpanelforconfigsearch();
        $('#leftpanel').hide();
        //showleftpanelforconfigsearch();

    }

    function UseFleet() {
        
        var movclassification = "";
        var isVR1 = $('#vr1appln').val();
        if (isVR1 == 'True') {
            movclassification = "VR1";
        }
        else {
            movclassification = "Special Order"
        }
        $.ajax({
            url: '../VehicleConfig/VehicleConfigList',
            data: { isApplicationVehicle: true, movclassification: movclassification },
            type: 'GET',
            beforeSend: function () {
                startAnimation();
            },
            success: function (page) {
                $('#Vehicle').html('');
                $('#Vehicle').show();
                $('#Vehicle').html($(page).find('#div_fleet').html(), function () {
                });
                removeHLinks();
                PaginateGrid();
                fillPageSizeSelect();
            },
            error: function (xhr, textStatus, errorThrown) {
            },
            complete: function () {
                stopAnimation();
                $('#Vehicle').show();
            }
        });
    }

    //vehicle list search button
    function showleftpanelforconfigsearch() {
        // $('#FleetConfiguration').html('');
        $.ajax({
            url: '../VehicleConfig/VehicleConfigBtn',
            data: { isApplicationVehicle: true },
            type: 'GET',
            beforeSend: function () {
                startAnimation();
            },
            success: function (page) {
                $('#leftpanel').html($(page).find('#vehicleConfigBtn').html(), function () {
                });

                $('#leftpanel').find('.searchtableleft button').click(function () {
                    SearchComponent();
                });
                removeHLinks();
                PaginateGrid();
                fillPageSizeSelect();
            },
            error: function (xhr, textStatus, errorThrown) {
            },
            complete: function () {
                stopAnimation();
                //$('#Vehicle').show();
            }
        });
    }

    function SPrevMovement() {
        startAnimation();
        $('#Vehicle').html('');
        UseMovement();
        $('#ShowSelectedFleetConfiguration').hide();
        //showleftpanelforconfigsearch();
        stopAnimation();
    }

    function UseMovement() {
        // $('#VehicleSOApplication').hide();
        $.ajax({
            url: '../Movements/MovementList',
            data: { MovementListForSO: true },
            type: 'GET',
            beforeSend: function () {
                startAnimation();
            },
            success: function (page) {
                $('#Vehicle').show();
                $('#Vehicle').html($(page).find('#divforsofilter').html(), function () {
                });
                $('#Vehicle').find("form").removeAttr('action', "");
                $('#Vehicle').find("form").submit(function (event) {
                    AdvSearchforSOApplivation();
                    event.preventDefault();
                });
                removeHLinks();
                PaginateGridsomovement();
                fillPageSizeSelect();

            },
            error: function (xhr, textStatus, errorThrown) {
            },
            complete: function () {
                stopAnimation();
            }
        });
    }

    function SOnExpand() {

        { $('#expand').show(); }

    }

    function Snewroute() {

        //function for resize map
        var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;

        var page = '@Session["pageflag"]';
        $('#leftpanel').html('');
        $("#mapTitle").show();
        $('#Route').show();
        $('#RoutePart').show();
        $('#RoutePart').html('');

        $.ajax({
            url: '../Routes/LibraryRoutePartDetails',
            //data: { RouteFlag: page, ApplicationRevId: ApplicationRevId },
            data: { RouteFlag: page, ApplicationRevId: ApplicationRevId },
            type: 'GET',
            success: function (page) {
                CheckSessionTimeOut();
                $('#Route').show();
                $('#RoutePart').html('');

                $('#RoutePart').append($(page).find('#CreateRoute').html());
                Map_size_fit();//, function () {
                $("#mapTitle").html('');
                scroll();
            }
        });
    }

    function Sroutelistfrommovement() {
        $("#mapTitle").html('');
        $('#Route').show();
        $('#leftpanel').html('');
        $('#RoutePart').html('');
        UseMovement();
    }

    function UseMovement() {
        // $('#VehicleSOApplication').hide();
        $.ajax({
            url: '../Movements/MovementList',
            data: { MovementListForSO: true, VR1route: true },
            type: 'GET',
            beforeSend: function () {
                startAnimation();
            },
            success: function (page) {
                $('#Route').show();
                $('#RoutePart').html('');
                $('#RoutePart').html($(page).find('#divforsofilter').html(), function () {
                });
                $('#RoutePart').find('h4').text('Route from previous movement ');
                $('#RoutePart').find("form").removeAttr('action', "");
                $('#RoutePart').find("form").submit(function (event) {
                    AdvSearchforApplivation();
                    event.preventDefault();
                });
                removeroutemovementHLinks();
                PaginateGridvrmovement();
                fillPageSizeSelect();
                $('#btn_img_back_selectList').hide();

                $('#RoutePart').append('<button class="btn_reg_back next btngrad btnrds btnbdr btn-cloneroutes" id="btn_img_back_selectList" aria-hidden="true" style="margin-bottom: 5px; float: right;"  type="button" data-icon="">Cancel </button>');
            },
            error: function (xhr, textStatus, errorThrown) {
            },
            complete: function () {
                stopAnimation();
            }
        });
    }

    function Sroutelistfromlibrary() {

        var id = getQuery('VR1Applciation');
        if (id == true) {
            var routeType = 2;
        }
        else {
            var routeType = 1;
        }

        $('#leftpanel').html('');
        $("#mapTitle").html('');
        var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;

        $('#divMap1').hide();
        $.ajax({
            url: '../Routes/RoutePartLibrary',
            data: { isApplicationroute: true, ApprevisionId: ApplicationRevId, RouteType: routeType },
            type: 'POST',
            async: false,
            beforeSend: function () {
                startAnimation();
            },
            success: function (page) {
                $('#Rote').show();
                $('#RoutePart').html('');
                $('#RoutePart').html($(page).find('#div_Route').html(), function () {

                });
                $('#RoutePart').find('.green').removeAttr('href').css("cursor", "pointer");
                $("#mapTitle").html('<h4 style="color:#414193;">Import list</h4> <br/>');
                //$('#RoutePart').find('.green').bind('click',unhook);
                //removeHLinks();
                //PaginateGridvrRoute();
                removerouteHLinks();
                PaginateGridsoroute();
                fillPageSizeSelect();
                load_leftpanel();
            },
            error: function (xhr, textStatus, errorThrown) {
            },
            complete: function () {
                stopAnimation();
            }
        });
    }


