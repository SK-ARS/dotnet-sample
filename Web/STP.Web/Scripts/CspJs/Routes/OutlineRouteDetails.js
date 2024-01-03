    var wayCnt = 0;
    var textrouteflag = false;
    rpObj = null;
    var startDesc = 0, startNorthing = 0, startEasting = 0;              //to store starting points for automatic route plannig
    var endDesc = 0, endNorthing = 0, endEasting = 0;                    //to store ending points for automatic route plannig
    var _RouteID = 0, _RouteName = "";

    var modelRoutePart = @Html.Raw(Json.Encode(Model))

    $(document).ready(function () {
        $("#span-close").on('click', ClosePopUp);
        $("#span-help").on('click', help_poup);
        $("#span-Edit_help").on('click', EditHelp_popup);

if($('#hf_IsStartAndEndPointOnly').val() ==  'True')
          loadmap('NOMAPDISPLAY', null);

      if ('@Session["RouteFlag"]' != null) {
            var tittle = $("#PopupTitale").val();
            $("div.dyntitleConfig").html('');
            $("div.dyntitleConfig").html(tittle);
            $("#PopupTitale").val("");
        }
        else
          $("div.dyntitleConfig").html('Start and end point only route details');
        rpObj =  @Html.Raw(Json.Encode(ViewBag.rp))
       $('#outline_tbl_route_details').find('#div_saving').hide();
      $('#outline_tbl_route_details').find('#deleteway').hide();

      prevIndex = 0;
      outLIneRouteCreationMode = false;

      Resize_PopUp(440);
      $("#Pointtype").val('Start');

if($('#hf_IsStartAndEndPointOnly').val() ==  "True" && '@ViewBag.IsTextualRouteType' != "True") {
          modelRoutePart = getOutlineRouteDetails();
          $("#startDesc").val(modelRoutePart.routePathList[0].routePointList[0].pointDescr);
          if (modelRoutePart == undefined) {
              modelRoutePart = rpObj;
          }
              if (modelRoutePart != undefined) {
              textrouteflag = true;
              $("#startDesc").attr("disabled", true);
          }
          else {

              modelRoutePart = { "routePartDetails": { "SaveAsNew": false, "wayPointAddress": false, "wayPointContactDetails": false, "routeName": null, "routeType": null, "routeDescr": null, "routeID": 0, "startDesc": null, "endDesc": null, "startLocationAddress": null, "endLocationAddress": null, "startLocationContact": null, "endLocationContact": null, "routeNo": 0, "partGeometry": null }, "routePathList": [{ "subPartNo": 0, "pathDescr": null, "routeSegmentList": [], "routePointList": [{ "pointType": 0, "pointDescr": null, "routePointNo": 0, "routePointId": 0, "direction": 0, "linkId": 0, "pointPathId": 0, "pointRouteVar": 0, "isAnchorPoint": 0, "pointGeom": null, "routeContactList": null, "pointAnnotation": null }, { "pointType": 0, "pointDescr": null, "routePointNo": 0, "routePointId": 0, "direction": 0, "linkId": 0, "pointPathId": 0, "pointRouteVar": 0, "isAnchorPoint": 0, "pointGeom": null, "routeContactList": null, "pointAnnotation": null }] }], "isComplete": false, "startDescription": null, "endDescription": null, "lastUpdated": "\/Date(-62135596800000)\/", "orgId": 0 }
          }
      }
      else {
          $("#startDesc").attr("disabled", false);
      }



      if (modelRoutePart != null && modelRoutePart.routePartDetails.routeID == 0) {
          outLIneRouteCreationMode = true;
          $('#outline_tbl_route_details').find('#Savenew').hide();
          return;
      }
      if (textrouteflag == true && '@Session["RouteFlag"]' != 2) {
            $('#outline_tbl_route_details').find('#AddWaypoint').hide();
            $('#outline_tbl_route_details').find('#DeleteWaypoint').hide();
            $('#outline_tbl_route_details').find('#Savenew').hide();
        }
        if (modelRoutePart == null) {
            modelRoutePart = getRouteDetails();
            $("#Routename").val(modelRoutePart.routePartDetails.routeName);
            $("#routeDescr").val(modelRoutePart.routePartDetails.routeDescr);
            $('#outline_tbl_route_details').find('#AddWaypoint').hide();
            $('#outline_tbl_route_details').find('#DeleteWaypoint').hide();
        }


        if (modelRoutePart.routePathList[0].routePointList[0].routeContactList.length != null && modelRoutePart.routePathList[0].routePointList[0].routeContactList.length > 0) {
            $("#locationAddress").val(modelRoutePart.routePathList[0].routePointList[0].routeContactList[0].addressLine1);
            $("#locationContact").val(modelRoutePart.routePathList[0].routePointList[0].routeContactList[0].telephone);
        }

        wayCnt = modelRoutePart.routePathList[0].routePointList.length - 2;
        if (modelRoutePart.routePathList[0].routePointList != null && modelRoutePart.routePathList[0].routePointList.length > 2) {
            for (var i = 2; i < modelRoutePart.routePathList[0].routePointList.length ; i++) {
                if (modelRoutePart.routePathList[0].routePointList[i].pointType == 2)
                    $("#Pointtype").append($("<option />").val(i).text("Waypoint" + (i - 1)));
                else
                    $("#Pointtype").append($("<option />").val(i).text("Viapoint" + (i - 1)));

            }
        }

        $("#startDesc").attr("disabled", false);
        
  });


    $('#searchcontactbtn').live("click", function () {
        var randomNumber = Math.random();

        Resize_PopUp(900);
        $('#outlinehead').hide();
        $('#outlinebody').hide();
        $("#contactpopup").load('../Notification/PopUpAddressBook?origin=' + "outlinesave" + '&random=' + randomNumber);
        $("#contactpopup").show();
    });
    var flagdelwaypt = false;
    $('#DeleteWaypoint').click(function () {

        var e = document.getElementById("Pointtype");

        var index = e.selectedIndex;
        flagdelwaypt = true;
        e.remove(wayCnt + 1);

        modelRoutePart.routePathList[0].routePointList.splice(index, 1);

        for (i = index; i < e.length; i++)
            modelRoutePart.routePathList[0].routePointList[i].routePointNo = modelRoutePart.routePathList[0].routePointList[i].routePointNo - 1;


        function setSelectedIndex(s, i) {
           s.options[i].selected = true;
           flagdelwaypt = false;
           prevIndex = 0;
            return;
        }
        setSelectedIndex(document.getElementById("Pointtype"), 0);
        $("#startDesc").val(modelRoutePart.routePathList[0].routePointList[0].pointDescr);
      if (modelRoutePart.routePathList[0].routePointList[0].routeContactList.length != null && modelRoutePart.routePathList[0].routePointList[0].routeContactList.length > 0) {
            $("#locationAddress").val(modelRoutePart.routePathList[0].routePointList[0].routeContactList[0].addressLine1);
            $("#locationContact").val(modelRoutePart.routePathList[0].routePointList[0].routeContactList[0].telephone);
        }
       wayCnt--;

       if (wayCnt == 0) {
           $('#outline_tbl_route_details').find('#deleteway').hide();
       }

       $('#SN_validatn').html('');
       $('#GroupboxDetails').html('Start point details');
    });
    $('#AddWaypoint').click(function () {

        $('#outline_tbl_route_details').find('#deleteway').show();
            //save values of current selected index
            var e = document.getElementById("Pointtype");
            var index = e.selectedIndex;

            var contact = {};
            contact['addressLine1'] = $("#locationAddress").val();
            contact['telephone'] = $("#locationContact").val();
            modelRoutePart.routePathList[0].routePointList[prevIndex].pointDescr = $("#startDesc").val();
            modelRoutePart.routePathList[0].routePointList[prevIndex].routeContactList = [];
            modelRoutePart.routePathList[0].routePointList[prevIndex].routeContactList.push(contact);

            $("#Pointtype").append($("<option />").val(wayCnt + 2).text("Viapoint" + (wayCnt + 1)));
            index = wayCnt + 2;
            prevIndex = wayCnt + 2;
            function setSelectedIndex(s, i) {
                s.options[i].selected = true;
                return;
            }
            setSelectedIndex(document.getElementById("Pointtype"), wayCnt + 2);
            document.getElementById('GroupboxDetails').innerHTML = "Viapoint details";
            $("#startDesc").val("");
            $('#locationAddress').val("");
            $('#locationContact').val("");
            wayCnt++;

            var routePoint = {};

            routePoint["pointType"] = 2;
            routePoint["pointDescr"] = "";
            routePoint["routePointNo"] = wayCnt;

            modelRoutePart.routePathList[0].routePointList.push(routePoint);
    });
    $('#Pointtype').change(function () {
        flagdelwaypt = false;
        if (flagdelwaypt == true) {
            flagdelwaypt = false;
            return;
        }
        var e = document.getElementById("Pointtype");
        var index = e.selectedIndex;
        if (index === 0) {
            $('#outline_tbl_route_details').find('#deleteway').hide();
            document.getElementById('GroupboxDetails').innerHTML = "Start point details";
        }
        else
            if (index === 1) {
                $('#outline_tbl_route_details').find('#deleteway').hide();
                document.getElementById('GroupboxDetails').innerHTML = "End point details";
            }
            else {
                document.getElementById('GroupboxDetails').innerHTML = "Waypoint details";
                if (modelRoutePart.routePathList[0].routePointList[index].pointType == 2)
                    $('#startDesc').prop('readonly', true);
                else
                    $("#startDesc").prop("readonly", false);
            }
        var contact = {};
        contact['addressLine1'] = $("#locationAddress").val();
        contact['telephone'] = $("#locationContact").val();
        modelRoutePart.routePathList[0].routePointList[prevIndex].pointDescr = $("#startDesc").val();
        modelRoutePart.routePathList[0].routePointList[prevIndex].routeContactList = [];
        modelRoutePart.routePathList[0].routePointList[prevIndex].routeContactList.push(contact);

        if (outLIneRouteCreationMode == true) {
            if (index == 0)
                modelRoutePart.routePartDetails.startDesc = $("#startDesc").val();

            else if (index == 1)
                modelRoutePart.routePartDetails.endDesc = $("#startDesc").val();

        }
        if (index <= 1 || modelRoutePart.routePathList[0].routePointList[index].pointDescr != "") {
            $('#locationAddress').val("");
            $('#locationContact').val("");
            $("#startDesc").val(modelRoutePart.routePathList[0].routePointList[index].pointDescr);


            if (modelRoutePart.routePathList[0].routePointList[index].routeContactList != null && modelRoutePart.routePathList[0].routePointList[index].routeContactList.length > 0) {
                $("#locationAddress").val(modelRoutePart.routePathList[0].routePointList[index].routeContactList[0].addressLine1);
                $("#locationContact").val(modelRoutePart.routePathList[0].routePointList[index].routeContactList[0].telephone);
            }
            prevIndex = index;
        }

       
    });

    $('#span-close').click(function () {
        $("#dialogue").html('');
        $('#overlay').hide();
        resetdialogue();
        addscroll();
    });
    $('#outline_tbl_route_details').keypress(function (e) {
    });

    $('#outline_btn_save').click(function () {
        if (rpObj != null) {
            if (modelRoutePart == undefined)
            modelRoutePart = rpObj;
            modelRoutePart.routePartDetails.routeID = '@Session["plannedRouteId"]';
            modelRoutePart.routePartDetails.routeName = $("#Routename").val();

        }
        modelRoutePart.routePartDetails.routeDescr = $("#routeDescr").val();
        $("#RN_validatn").html("");
        if ($("#Routename").val() == "") {
            $("#RN_validatn").html("The route name is required.");
            return;
        }
        var isValid = 0;
        if (modelRoutePart.routePartDetails.routeID === undefined || modelRoutePart.routePartDetails.routeID == 0) {
            if ('@Session["RouteFlag"]' == 2) {
                var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
                $.ajax({
                    url: '/Routes/ValidateApplicationRouteName',
                    type: 'POST',
                    async: false,
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify({ ROUTE_NAME: $("#Routename").val(), REVISION_ID: +ApplicationRevId, ROUTE_FOR: 2 }),
                    success: function (result) {
                        isValid = result;
                    }
                });
            }
            else {
                $.ajax({
                    url: '/Routes/isVerifyRouteName',
                    type: 'POST',
                    async: false,
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify({ ROUTE_NAME: $("#Routename").val() }),
                    success: function (result) {
                        isValid = result;
                    }
                });
            }

            if (isValid != 0) {

                $("#RN_validatn").html("The route name already exist.");
                return;
            }
        }



        modelRoutePart.routePartDetails.routeType = "outline";
        if (modelRoutePart.routePathList[0].routePointList.length > 0) {
            modelRoutePart.routePathList[0].routePointList[0].routePointNo = 1;
            if (outLIneRouteCreationMode == true) {
                modelRoutePart.routePathList[0].routePointList[0].routePointNo = 1;
                modelRoutePart.routePathList[0].routePointList[0].pointType = 0;
                modelRoutePart.routePathList[0].routePointList[1].pointType = 1;
                modelRoutePart.routePathList[0].routePointList[1].routePointNo = 2;
            }
        }

        var id = 0;
        if (!$("#SaveAsNew").is(':checked') && typeof modelRoutePart.routePartDetails.routeID != 'undefined')
            id = '@Session["plannedRouteId"]';
        var e = document.getElementById("Pointtype");
        var index = -1;
if($('#hf_IsTextualRouteType').val() ==  "True") {
            index = e.selectedIndex;
        }
        modelRoutePart.routePathList[0].routeName = $("#Routename").val();
        modelRoutePart.routePartDetails.routeName = $("#Routename").val();
        modelRoutePart.routePathList[0].routeDescr = $("#routeDescr").val();

if($('#hf_IsTextualRouteType').val() ==  "True") {
            var contact = {};
            contact['addressLine1'] = $("#locationAddress").val();
            contact['telephone'] = $("#locationContact").val();
            if (modelRoutePart.routePathList[0].routePointList.length > 0) {
                modelRoutePart.routePathList[0].routePointList[index].pointDescr = $("#startDesc").val();
                modelRoutePart.routePathList[0].routePointList[index].routeContactList = [];
                modelRoutePart.routePathList[0].routePointList[index].routeContactList.push(contact);
            }
            if (modelRoutePart.routePathList[0].routePointList.length > 1 && (modelRoutePart.routePathList[0].routePointList[0].pointDescr == null || modelRoutePart.routePathList[0].routePointList[0].pointDescr == "")) {
                $("#L_validation").html("Start location name is required.");
                return;
            }
            else if (modelRoutePart.routePathList[0].routePointList.length > 1 && (modelRoutePart.routePathList[0].routePointList[1].pointDescr == null || modelRoutePart.routePathList[0].routePointList[1].pointDescr == "")) {
                $("#L_validation").html('');
                $("#L_validation").show();
                $("#L_validation").html("End location name is required.");
                
            return;
            }

                //by shraddha
if($('#hf_IsStartAndEndPointOnly').val() ==  "True") {
                $("#SN_validatn").html('');
                $("#L_validation").html('');
                  var Invalid_Location = WhichIs_InvalidLocation();
                  if (Invalid_Location != "No")
                      return;
                }
            else {
                var i;
                for (i = 2 ; i < wayCnt + 2 ; i++)
                    if (modelRoutePart.routePathList[0].routePointList[i].pointDescr == null || modelRoutePart.routePathList[0].routePointList[i].pointDescr == "") {
                        var str = "Waypoint  " + (parseInt(i) - 1).toString() + " description is required";
                        function setSelectedIndex(s, i) {
                            s.options[i].selected = true;
                            $('#Pointtype').change();
                            $("#SN_validatn").html(str);
                            return;
                        }
                        setSelectedIndex(document.getElementById("Pointtype"), i);
                        $("#startDesc").val("");
                        $('#locationAddress').val("");
                        $('#locationContact').val("");
                        return;
                    }
            }
            if (modelRoutePart.routePathList[0].routePointList.length > 0) {
                if (outLIneRouteCreationMode == true) {
                    modelRoutePart.startDescription = modelRoutePart.routePathList[0].routePointList[0].pointDescr;
                    modelRoutePart.endDescription = modelRoutePart.routePathList[0].routePointList[1].pointDescr;
                    modelRoutePart.routePartDetails.startDesc = modelRoutePart.routePathList[0].routePointList[0].pointDescr;
                    modelRoutePart.routePartDetails.endDesc = modelRoutePart.routePathList[0].routePointList[1].pointDescr;
                }
            }
        }
        $('#outline_tbl_route_details').find('#div_saving').show();

if($('#hf_IsStartAndEndPointOnly').val() ==  "True") {
            $.ajax({
                url: '/Routes/SaveRoute',
                type: 'POST',
                async: false,
                contentType: 'application/json; charset=utf-8',

                data: JSON.stringify({ plannedRoutePath1: modelRoutePart, PlannedRouteId: id, orgID: 1101 }),

                success: function (val) {
                    $('#outline_tbl_route_details').find('#div_saving').hide();
                    var RN = modelRoutePart.routePathList[0].routeName;
                    if (id == 0) {
                        id = val.value;
                        RN = "Route " + "'" + RN + "' " + "saved successfully.";
                        showWarningPopDialog(RN, 'Ok', '', 'ClosePopUp12', '', 1, 'info');
                    }
                    else {
                        if (val != 0) {
                            _RouteID = val.value;
                            _RouteName = RN;

                            RN = "Route " + "'" + RN + "' " + "updated successfully.";
                            showWarningPopDialog(RN, 'Ok', '', 'ClosePopUp12', '', 1, 'info');
                        }
                        else
                            showWarningPopDialog('Update failed.', 'Ok', '', 'ClosePopUp12', '', 1, 'info');
                    }
                },
                async: true
            });
            resetdialogue();
        }
        else
            saveStartAndEndPointRoute();
    });

    function ClosePopUp12() {
         if ('@Session["RouteFlag"]' == 2 || '@Session["RouteFlag"]' == 1) {
            $("#dialogue").hide();
            $("#pop-warning").hide();
            $("#overlay").hide();
            if ($('#UserTitle').html() == "SORT Portal")
                CloneRoutesSort();
             else
            CloneRoutes();
            if ('@Session["RouteFlag"]' == 2)
            { $('#pageheader').html(''); $('#pageheader').html('<h3> Apply for SO  - Route</h3>'); }
            else if ('@Session["RouteFlag"]' == 1 || '@Session["RouteFlag"]' == 3)
             { $('#pageheader').html(''); $('#pageheader').html('<h3> Apply for VR-1 - Route</h3>'); }
         }
         else
            ClosePopUp();
        resetdialogue();
     }
    function ClosePopUp() {
        $("#dialogue").html('');
        $("#dialogue").hide();
        $("#pop-warning").hide();
        $("#overlay").hide();
        addscroll();
if($('#hf_IsStartAndEndPointOnly').val() ==  "True") {
            showSavedRouteDetails(_RouteID, "outline", _RouteName, "U");
    }*@
    }
    function saveStartAndEndPointRoute() {
        var e = document.getElementById("Pointtype");

        var index = e.selectedIndex;
        setRoutePoint(startDesc, 0, startNorthing, startEasting, 0, function (result) {
            if (result == true) {
                setRoutePoint(endDesc, 1, endNorthing, endEasting, 1, function (result) {
                    if (result == true) {

                        var routePart = getOutlineRouteDetails();
                        routePart.routePartDetails.routeType = "outline";
                        var RN = modelRoutePart.routePartDetails.routeName;
                        routePart.routePartDetails.routeName = modelRoutePart.routePartDetails.routeName; //$("#Routename").html();
                        $.ajax({
                            url: '/Routes/SaveRoute',
                            type: 'POST',
                            async: false,
                            contentType: 'application/json; charset=utf-8',
                            data: JSON.stringify({ plannedRoutePath1: routePart, PlannedRouteId: 0, RouteFlag: 3 }),
                            success: function (val) {
                                var rt = val.value;
                                stopAnimation();
                                RN = "Route " + "'" + RN + "' " + "saved successfully.";
                                showWarningPopDialog(RN, 'Ok', '', 'ClosePopUp12', '', 1, 'info');
                            },
                            async: true
                        });
                        resetdialogue();
                    }
               });
            }
        });
    }

    function setRoutePointSoApp(locationDesc, pointNo, easting, northing) {
        var pointType = pointNo > 1 ? 2 : pointNo;
        if (pointType == 0) {
            startDesc = locationDesc;
            startEasting = easting;
            startNorthing = northing;

            //save these values in hidden fields
            $('#StartLocationDescr').val(locationDesc);
            $('#StartEasting').val(easting);
            $('#StartNorthing').val(northing);

        }
        else {
            endDesc = locationDesc;
            endEasting = easting;
            endNorthing = northing;
            //save these values in hidden fields
            $('#EndLocationDescr').val(locationDesc);
            $('#EndEasting').val(easting);
            $('#EndNorthing').val(northing);
        }
    }

    function WhichIs_InvalidLocation() {
        var WhichInvalid = "No";
        if (String(startNorthing) == "undefined" || startNorthing == 0 || String(startEasting) == "undefined" || startEasting == 0) {
                WhichInvalid = "startLoca";
                $("#L_validation").html("Enter valid start location.");
        }
        else if (String(endNorthing) == "undefined" || endNorthing == 0 && String(endEasting) == "undefined" || endEasting == 0) {
            WhichInvalid = "endLoca";
            $("#L_validation").html("Enter valid end location.");
        }
        return WhichInvalid;
    }

