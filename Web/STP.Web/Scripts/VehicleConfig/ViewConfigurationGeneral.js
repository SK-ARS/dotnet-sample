var hf_OverviewDisplayVehicleId;
function ViewConfigurationGeneralInit(VehicleId=0) {
    hf_OverviewDisplayVehicleId = VehicleId > 0 ? VehicleId : $('#hf_OverviewDisplayVehicleId').val();
    if ($('#hf_IsOverviewDisplay').val() == 'True' || $('#hf_IsOverviewDisplay').val() == 'true' || StepFlag==6) {
        VehicleDetailsShowHide(hf_OverviewDisplayVehicleId);
        IterateThroughTextboxOverview(hf_OverviewDisplayVehicleId);
        ShowInFeetOverview(hf_OverviewDisplayVehicleId);
        ShowAxleInFeetOverview();
    }
    GeneralVehicCompInit();
}
$(document).ready(function () {
    $('body').on('click', '.VehicleShowHide', function (e) {
        e.preventDefault();
        VehicleShowHide(this);
    });
    $('body').on('click', '#view-allAxle', function (e) {
        var vehicleId = $(this).data("vehicleid");
        var movementTypeId = $(this).data("movementtypeid");
        var isFleet = $(this).data("isfleet");
        AllAxleDetailPopUp(vehicleId, movementTypeId, isFleet, "#popupDialogue", false);
    });
    $('body').on('click', '.view-allaxle-modal-close', function (e) {
        e.preventDefault();
        CloseAllAxlePopUp();
    });
});
function VehicleDetailsShowHide(displayToggleVehicleId) {
    var targetElem = "applnOverviewDisplayDetails_" + displayToggleVehicleId;
    if ($('#' + targetElem).is(":visible")) {
        $('#' + targetElem).css("display", "none");
        $("#chevlon-up-icon_overviewDisplayDiv_" + displayToggleVehicleId).css("display", "none");
        $("#chevlon-down-icon_overviewDisplayDiv_" + displayToggleVehicleId).css("display", "block");
        $('#applnOverviewDisplay_' + displayToggleVehicleId).text("Show Details");
    }
    else {
        $('#' + targetElem).css("display", "block");
        $("#chevlon-up-icon_overviewDisplayDiv_" + displayToggleVehicleId).css("display", "block");
        $("#chevlon-down-icon_overviewDisplayDiv_" + displayToggleVehicleId).css("display", "none");
        $('#applnOverviewDisplay_' + displayToggleVehicleId).text("Hide Details");
    }
}
function VehicleShowHide(e) {
    var vehicleId = $(e).attr("vehicleid");
    VehicleDetailsShowHide(vehicleId);
}

function AllAxleDetailPopUp(vehicleId, movementTypeId, isFleet, elemId = "#popupDialogue", showAmendButton=true) {
    startAnimation();
    var isMovement = false;
    if ($("#hf_IsPlanMovmentGlobal").val() == "true" || $("#hf_IsPlanMovmentGlobal").val() == "True")
        isMovement = true;

    var isCandidate = false;
    var isNotif = false;
    var flag = "";
    var isSort = false;
    if ($("#hf_IsCandidate").val() == "true" || $("#hf_IsCandidate").val() == "True" || 
        $("#candidate_axle").val() == "true" || $("#candidate_axle").val() == "True") {
        isCandidate = true;
        isNotif = true;
        flag = "CandidateVehicle";
        isSort = true;
    }
    $.ajax({
        url: '../VehicleConfig/AllAxleDetailsPopUp',
        type: 'POST',
        beforeSend: function () {
            startAnimation();
        },
        data: {
            vehicleId: vehicleId, movementTypeId: movementTypeId, isMovement: isMovement, isNotif: isNotif, flag: flag, isSort: isSort, isFleet: isFleet
        },
        success: function (response) {
            stopAnimation();
            if (elemId == "#popupDialogue") {
                $(elemId).html(response);
                var axleCountLoaded = $(elemId).find('#hf_AxleAssignmentCount').val() || 0;
                if (axleCountLoaded && parseInt(axleCountLoaded) > 1) {
                    $('#allAxleDetailsPopUp').modal({ keyboard: false, backdrop: 'static' });
                    $("#popupDialogue").show();
                    $("#overlay").show();
                    $('#allAxleDetailsPopUp').modal('show');
                    ShowHideHeaderTyreSpaceAllAxle();
                    HeaderTyreSpaceCountAllAxle();
                    ShowAxleInFeetAllAxle();
                    //SHOW AXLE AMEND BUTTON ON LIST - If no axle exist, this is not required
                    if (showAmendButton)
                        $('.vehicle-list-' + vehicleId + ' .amend-axle').show();

                } else {//C&U not required axles and while importing it may not exist axles.In this case we dont need to show axle amend popup
                    $(elemId).html('');
                    $('.vehicle-list-' + vehicleId + ' .imgAmendComponentAxle').remove();//Remove amend icon
                    $('.vehicle-list-' + vehicleId + ' .IsBlankTractorAxleElemExist').remove();//Remove hidden field which used to track axle amend check
                    showValidationPopupIfErrorExist();
                }
            } else if ('#axleAmendmentPopup .allAxleDetailsAmendPopUpContainer') {//Show axle details in Amend popup
                $(elemId).html($(response).find('.allAxleDetailsPopUp').html());
                var axleCountLoaded = $(elemId).find('#hf_AxleAssignmentCount').val() || 0;
                if (axleCountLoaded && parseInt(axleCountLoaded) > 1) {
                    $('#axleAmendmentPopup').modal({ keyboard: false, backdrop: 'static' });
                    $('#axleAmendmentPopup').modal('show');
                    ShowHideHeaderTyreSpaceAllAxle();
                    HeaderTyreSpaceCountAllAxle();
                    ShowAxleInFeetAllAxle();
                    //SHOW AXLE AMEND BUTTON ON LIST
                    if (showAmendButton)
                        $('.vehicle-list-' + vehicleId + ' .amend-axle').show();
                } else {//C&U not required axles and while importing it may not exist axles.In this case we dont need to show axle amend popup
                    $(elemId).html('');
                    $('.vehicle-list-' + vehicleId + ' .imgAmendComponentAxle').remove();//Remove amend icon
                    $('.vehicle-list-' + vehicleId + ' .IsBlankTractorAxleElemExist').remove();//Remove hidden field which used to track axle amend check
                    showValidationPopupIfErrorExist();
                }
            }
        },
        complete: function () {
            
        }
    });
}

function CloseAllAxlePopUp() {
    $('#allAxleDetailsPopUp').modal('hide');
    $("#overlay").hide();
}

function HeaderTyreSpaceCountAllAxle() {
    if ($('#tyreEmpty').val().toLowerCase() != "true") {
        var grtValue = 0;
        $('.allAxleDetailsPopUp #vehicle-table table tr').each(function () {
            var _thisVal = parseInt($(this).find('.wheel_space').length);
            if (_thisVal > grtValue) {
                grtValue = _thisVal;
            }
        });
        for (var i = 1; i <= grtValue; i++) {
            $('.allAxleDetailsPopUp .tyreSpaceCnt').append('<th style="width: 10%;">' + i + '</th>');
        }
        $('.allAxleDetailsPopUp .headgrad2').attr('colspan', grtValue);
    }
}

//function to show tyre spacing header
function ShowHideHeaderTyreSpaceAllAxle() {
    if ($('.allAxleDetailsPopUp .wheel_space').length == 0) {
        $('.allAxleDetailsPopUp .headgrad2').hide();
        //$('.sub1').hide();
    }
    else {
        $('.allAxleDetailsPopUp .headgrad2').show();
        $('.allAxleDetailsPopUp .sub1').show();
    }

    if ($('.allAxleDetailsPopUp .tyre_size').length == 0) {
        $('.allAxleDetailsPopUp .headgrad_tyreSize').hide();
    }
    else {
        $('.allAxleDetailsPopUp .headgrad_tyreSize').show();
    }
}

function ShowAxleInFeetAllAxle() {
    var unitvalue = $('#UnitValue').val();

    if (unitvalue == 692002) {
        $('.allAxleDetailsPopUp .tblAxle tbody tr').each(function () {
            var distanceToNxtAxl = $(this).find('.disttonext').text();
            if (distanceToNxtAxl.indexOf('\'') === -1) {
                distanceToNxtAxl = ConvertToFeets(distanceToNxtAxl);
                $(this).find('.disttonext').text(distanceToNxtAxl);
            }
            var tyreSpace = null;
            $(this).find('.cstable').each(function () {
                var _thistxt = $(this).text();
                //if (_thistxt != undefined) {
                //    _thistxt = ConvertToFeets(_thistxt);
                //    $(this).text(_thistxt);
                //}

                if (tyreSpace != null) {
                    tyreSpace = tyreSpace + "," + _thistxt;
                }
                else {
                    tyreSpace = _thistxt;
                }

            });
        });
    }
}


function IterateThroughTextboxOverview(vehicleId) {
    $('#applnOverviewDisplayDetails_' + vehicleId +' input:text').each(function () {
        if (IsLengthFields(this)) {
            ConvertRangeToFeets(this);
        }
    });
}

function ShowInFeetOverview(vehicleId) {

    $('#applnOverviewDisplayDetails_' + vehicleId +' input:text').each(function () {

        if (IsPreference()) {

            if (IsLengthFields(this)) {
                var data = $(this).val();
                data = ConvertToFeets(data);
                if (data != "0\'0\"") {
                    $(this).val(data);
                }
                else {
                    $(this).val(null);
                }

            }
        }
    });

}

function ShowAxleInFeetOverview() {
    var unitvalue = $('#UnitValue').val();

    if (unitvalue == 692002) {
        $('.tblAxle tbody tr').each(function () {
            var distanceToNxtAxl = $(this).find('.disttonext').text();
            if (distanceToNxtAxl != undefined && distanceToNxtAxl.indexOf('\'') === -1) {
                distanceToNxtAxl = ConvertToFeet(distanceToNxtAxl);
                $(this).find('.disttonext').text(distanceToNxtAxl);
            }
            var tyreSpace = null;
            $(this).find('.cstable input:text').each(function () {
                var _thistxt = $(this).val();

                if (tyreSpace != null) {
                    tyreSpace = tyreSpace + "," + _thistxt;
                }
                else {
                    tyreSpace = _thistxt;
                }

            });
        });
    }
}