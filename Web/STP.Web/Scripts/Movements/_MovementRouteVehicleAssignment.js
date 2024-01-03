var hasCollision = false
var offset = [0, 0]
var dragClass;
var VehicleAlreadyMapped = false;
var routeIdsVal;
var vehicleIdsVal;
var alreadyMappedVehicleIds;
var IsNotif;
var IsSupplimentarySaved;
var routeIdArr;
var vehicleIdArr;
var assignedList = [];
var vehicleShortageFlag;    //disable confirm button if there is shortage of vehicle
var isVehicleAssignmentHasChanges = false;
function MovementRouteVehAssignInit() {
    vehicleShortageFlag = false;
    isVehicleAssignmentHasChanges = false;
    stopAnimation();
    routeIdArr = [];
    vehicleIdArr = [];
    assignedList = [];
    routeIdsVal = $('#hf_RouteIdArray').val();
    vehicleIdsVal = $('#hf_VehicleIdArr').val();
    alreadyMappedVehicleIds = $('#hf_AlreadyMappedVehicleIds').val();
    IsNotif = $('#hf_IsNotif').val();
    IsSupplimentarySaved = $('#hf_IsSupplimentarySaved').val();
    // start of drag and drop dunctionalities
    counter = 1;
    elements = document.querySelectorAll('.divdrag')
    StepFlag = 4;
    SubStepFlag = 4.3;
    RouteVehicleAssignFlag = false;
    CurrentStep = "Route Details";
    $('#current_step').text(CurrentStep);
    SetWorkflowProgress(4);
    $('#back_btn').show();
    $('#save_btn').hide();
    $('#apply_btn').hide();
    $('#confirm_btn').addClass('blur-button');
    $('#confirm_btn').attr('disabled', true);
    $('#confirm_btn').show();
    $('#backbutton').show();
    if (routeIdsVal != undefined && routeIdsVal != null) {
        routeIdsVal = JSON.parse(routeIdsVal);
        for (var i = 0; i < routeIdsVal.length; i++) {
            var id = routeIdsVal[i];
            routeIdArr.push(id);
        }
    }

    if (vehicleIdsVal != undefined && vehicleIdsVal != null) {
        vehicleIdsVal = JSON.parse(vehicleIdsVal);
        alreadyMappedVehicleIds = JSON.parse(alreadyMappedVehicleIds);
        for (var i = 0; i < vehicleIdsVal.length; i++) {
            var id = vehicleIdsVal[i];
            VehicleAlreadyMapped = alreadyMappedVehicleIds.indexOf(id) != -1 ? true : false;
            vehicleIdArr.push({ "VehicleId": id, "AssignedFlag": VehicleAlreadyMapped });
        }
    }

    if (VehicleAlreadyMapped) {
        setTimeout(function () {
            $('#confirm_btn').removeClass('blur-button');
            $('#confirm_btn').attr('disabled', false);
        }, 1000);
    }
}
function AssignVehiclesToRoute() {
    startAnimation();
    assignedList = [];
    $.each(routeIdArr, function (index, value) {
        var vehicleIds = [];
        var elems = document.getElementById('route_' + value).querySelectorAll('.divdrag1');
        if (elems.length > 0)
            [...elems].forEach(elem => vehicleIds.push(elem.getAttribute('data-id')))

        var assignObj = {};
        assignObj['RoutePartId'] = value;
        assignObj['VehicleIds'] = vehicleIds;

        assignedList.push(assignObj);
    });
    var vehicleAssign = true;
    for (var i = 0; i < assignedList.length; i++) {
        if (assignedList[i].VehicleIds.length == 0) {
            vehicleAssign = false;
        }
    }
    if (!vehicleAssign) {
        stopAnimation();
        ShowErrorPopup('Vehicle has to be assigned to all the routes defined');
    }
    else {
        if (typeof isVehicleAssignmentHasChanges != 'undefined' && isVehicleAssignmentHasChanges) {
            console.log("is Vehicle Assignment Has Changes -- " + isVehicleAssignmentHasChanges);
            //There are changes in vehicle assignment, Need to update db and payload
            $.ajax({
                type: "POST",
                url: "../Movements/AssignMovementVehicle",
                data: {
                    vehicleAssignment: assignedList,
                    revisionId: $('#AppRevisionId').val(),
                    versionId: $('#AppVersionId').val(),
                    contRefNum: $('#CRNo').val()
                },
                beforeSend: function () {
                },
                success: function (response) {
                    if (response) {
                        $('#hf_VehicleAssignedGlobal').val("true");
                        CompleteVehicleAssignmentLoadNextStep();
                    }
                },
                error: function (result) {
                },
                complete: function () {
                }
            });
        } else {
            //No changes in vehicle assignment
            console.log("is Vehicle Assignment Has Changes -- " + (typeof isVehicleAssignmentHasChanges != 'undefined'?isVehicleAssignmentHasChanges:false));
            CompleteVehicleAssignmentLoadNextStep();
        }
      
    }
}

function CompleteVehicleAssignmentLoadNextStep() {
    if (IsNotif == 'True') {
        LoadContentForAjaxCalls("POST", '../Notification/NotificationRouteAssessment', { workflowProcess: 'HaulierApplication' }, '#route_assessment_section', '', function () {
            NotificationRouteAssessmentInit();
        });
    }
    else {
        if (IsSupplimentarySaved == 'True') {
            LoadContentForAjaxCalls("POST", '../Application/ViewSupplementary', { appRevisionId: $('#AppRevisionId').val() }, '#supplimentary_info_section', '', function () {
                ViewSupplementaryInit();
            });
        }
        else {
            LoadContentForAjaxCalls("POST", '../Application/ApplicationSupplimentaryInfo', { appRevisionId: $('#AppRevisionId').val() }, '#supplimentary_info_section', '', function () {
                ApplicationSupplimentaryInfoInit();
            });
        }
    }
}

function allowDrop(ev) {
    ev.preventDefault();
}
function drag(ev) {
    ev.originalEvent.dataTransfer.setData("text", ev.target.id);
    dragClass = ev.target.classList[3];
}
function drop(ev) {
    ev.preventDefault();
    ev.stopImmediatePropagation();
    ev.stopPropagation();
    var data = ev.originalEvent.dataTransfer.getData("text");
    var droppedVehicleCntrId = data.split("_");
    var vehicleId = (droppedVehicleCntrId.length > 1) ? droppedVehicleCntrId[1] : 0;
    var targetRouteCntrId = (ev.currentTarget.id).split("_");
    var routeId = (targetRouteCntrId.length > 1) ? targetRouteCntrId[1] : 0;
    if ($('#' + ev.currentTarget.id).children('#' + routeId + '_' + vehicleId).length > 0) {
        showToastMessage({
            message: "Vehicle already added to the route",
            type: "error"
        })
    }
    else {
        /* If you use DOM manipulation functions, their default behaviour it not to
       copy but to alter and move elements. By appending a ".cloneNode(true)",
       you will not move the original element, but create a copy. */

        var nodeCopy = document.getElementById(data).cloneNode(true);
        //var nodeCopy = document.getElementById(data);
        nodeCopy.id = routeId + "_" + vehicleId; /* We cannot use the same ID */
        nodeCopy.style.height = "auto !important";
        nodeCopy.className += " col-lg-3 col-sm-12 col-md-4";
        nodeCopy.setAttribute("draggable", "false");
        nodeCopy.setAttribute("data-id", vehicleId);
        nodeCopy.style.margin = "10px";
        nodeCopy.style.width = "12rem";
        $(ev.target).append(nodeCopy);
        
        var elem = document.createElement("img");
        elem.setAttribute("id", "clsID" + counter);
        elem.setAttribute("src", "/Content/assets/images/Group 16.svg");
        elem.setAttribute("width", "20");
        elem.setAttribute("alt", "closeIcon");
        elem.setAttribute("draggable", "false");

        var draggedVehicleCntrId = routeId + "_" + vehicleId;
        //elem.style.marginTop = "-66px";
        //elem.style.padding = "4px 5px 5px 0px";
        //elem.style.marginRight = "5px";
        //elem.style.float = "right";
        //elem.style.cursor = "pointer";
        elem.setAttribute("class", "removedraggedvehicle");
        elem.setAttribute("data-vehiclecntr", draggedVehicleCntrId);
        //elem.setAttribute("onclick", "RemoveDraggedVehicle(\'" + draggedVehicleCntrId + "\')");
        counter++;
        nodeCopy.appendChild(elem);
        vehicleIdArr[vehicleIdArr.findIndex((obj => obj.VehicleId == vehicleId))].AssignedFlag = true;
        if (vehicleIdArr.every(AreVehiclesAssigned) && (!vehicleShortageFlag)) {
            $('#confirm_btn').removeClass('blur-button');
            $('#confirm_btn').attr('disabled', false);
        }

        isVehicleAssignmentHasChanges = true;
    }
    return false;
}
//stop drop inside div
function nodeCopyDrop(e) {
    e.stopPropagation();
}
//stop drop inside div
function dragMove(e) {
    hasCollision = Array.prototype.some.call(elements, d => {
        if (d.id !== e.target.id) {
            return isCollide(e, d)
        }
        return false
    })
}
function isCollide(a, b) {
    const aRect = a.target.getBoundingClientRect()
    const bRect = b.getBoundingClientRect()
    return !(
        ((a.clientY + offset[1] + aRect.height) < (bRect.top)) ||
        (a.clientY + offset[1] > (bRect.top + bRect.height)) ||
        ((a.clientX + offset[0] + aRect.width) < bRect.left) ||
        (a.clientX + offset[0] > (bRect.left + bRect.width))
    )
}
function RemoveDraggedVehicle(VehicleCntr) {
    var droppedVehicleCntrId = VehicleCntr.split("_");
    var vehicleId = (droppedVehicleCntrId.length > 1) ? droppedVehicleCntrId[1] : 0;

    var nodeCopy = document.getElementById(VehicleCntr);
    nodeCopy.id = "drag_" + vehicleId;
    nodeCopy.classList.remove("col-lg-3", "col-sm-12", "col-md-4");
    nodeCopy.setAttribute("draggable", "true");    
    nodeCopy.removeAttribute("data-id");
    nodeCopy.removeAttribute("style");
    nodeCopy.querySelector('[id^="clsID"]').remove();
    nodeCopy.remove();
   // document.getElementById(vehicleId).appendChild(nodeCopy);

    var isExist = $('.drop_vehicle .filter_button').filter(function () {
        return $(this).data("id") == vehicleId
    });
    if (isExist && isExist.length <= 0) {
        vehicleIdArr[vehicleIdArr.findIndex((obj => obj.VehicleId == vehicleId))].AssignedFlag = false;
        if (!vehicleIdArr.every(AreVehiclesAssigned)) {
            $('#confirm_btn').addClass('blur-button');
            $('#confirm_btn').attr('disabled', true);
        }
    }

    isVehicleAssignmentHasChanges = true;
    //document.getElementById(VehicleCntr).remove();
}
function AreVehiclesAssigned(element, index, array) {
    return (element.AssignedFlag == true);
}
// end of drag and drop dunctionalities
$(document).ready(function () {

    $('body').on('dragstart', '.drag_vehicle', function (e) {//Left side vehicle item
        drag(e);
    });
    $('body').on('drag', '.drag_vehicle', function (e) {//Left side vehicle item
        dragMove(e);
    });


    $('body').on('drop', '.drop_vehicle', function (e) {     //drop area inside route div   
        $(this).removeClass("drop-vehicle-highlight");
        drop(e);
    });
    $('body').on('dragleave', '.drop_vehicle', function (e) { //drop area inside route div
        $(this).removeClass("drop-vehicle-highlight");
    });
    $('body').on('dragover', '.drop_vehicle', function (e) { //drop area inside route div
        $(this).addClass("drop-vehicle-highlight");
        allowDrop(e);
    });

    //--------------------------------------
    $('body').on('dragstart', '.drag_start', function (e) {
        drag(e);
    });
    $('body').on('drag', '.drag_start', function (e) {
        dragMove(e);
    });
    $('body').on('click', '.removedraggedvehicle', function (e) {
        var VehicleCntr = $(this).data("vehiclecntr");
        RemoveDraggedVehicle(VehicleCntr);
    });
});

function CheckVehiclesAssignedToRoute() {
    var vehicleAssign = true;
    if (assignedList.length > 0) {
        for (var i = 0; i < assignedList.length; i++) {
            if (assignedList[i].VehicleIds.length == 0) {
                vehicleAssign = false;
            }
        }
    }
    else if (routeIdArr.length > 1) {
        vehicleAssign = false;
    }
    return vehicleAssign;
}