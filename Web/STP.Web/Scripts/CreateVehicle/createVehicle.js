var AplnMovemntId = 0;
var ApplnMovementId = 0;
var vehicleConfigurationId = 0;
let FleetStepFlag = 0;
var axleweightMax = 0;
var compHtml;
var axleweightsum = 0;
var configWheelbase = 0;
var importComponentFromFleet = 0;
var vehicleResetOnEdit = false;
var configFieldUpdatedOnSave = false;
var updatedConfigFieldName = false;
var saveVehicleButtonClicked = false;
var compFieldUpdatedOnSave = false;
var updatedCompFieldName = false;

$(function () {
    suppressKey();
});

$(document).ready(function () {
    $('body').on('click', '.deleteVehicleComp', function (e) {
        e.preventDefault();
        DeleteVehicleComponentFn(this);
    });
    $('body').on('change', '.drpComponentType', function (e) {
        e.preventDefault();
        VehicleTypeChangeFn(this);
    });
    $('body').on('change', '.drpComponentSubType', function (e) {
        e.preventDefault();
        VehicleSubTypeChangeFn(this);
    });
    $('body').on('click', '.comp-shad-sub-add-fleet', function (e) {
        e.preventDefault();
        var componentId = $(this).data('compshadsubid');
        ComponentAddToFleet(componentId);
    });
    $('body').on('click', '.create-vehicle-import-comp-config', function (e) {
        e.preventDefault();
        var componentId = $(this).data('compid');
        ImportComponentToConfig(componentId);
    });

    //Component List pagination
    $('body').off('click', ".fleet-comp-pagination .pagination li a");
    $('body').on('click', ".fleet-comp-pagination .pagination li a", function (e) {
        e.preventDefault();
        var pageNum = getUrlParameterByName("page", this.href);
        $('#pageNum').val(pageNum);
        AjaxPaginationForComponentList(pageNum);
    });
    $('body').on('change', '.FleetComponent-Pag #pageSizeSelect', function () {
        var pageSize = $(this).val();
        var pageNum = $('#pageNum').val();
        $('#pageSizeVal').val(pageSize);
        AjaxPaginationForComponentList(pageNum);
    });
});

function LoadSelectVehicleComponentV1() {
    var IsCandidate = false;
    if ($('#IsCandVersion').val() == "True") {
        IsCandidate = true;
    }
    openContentLoader("#component_selection_section");
    LoadVehicleContentForAjaxCalls("POST", '../VehicleConfig/SelectVehicleComponent', { isCandidate: IsCandidate }, '#component_selection_section', -1, function () {
        ComponentSelectionInit();
        closeContentLoader("#component_selection_section");
    },showAnimation=false);

}
function LoadVehicleContentForAjaxCalls(Type, Url, Params, ResLoadContnr, flag, callBackFn,showAnimation=true) {
    CloseSuccessModalPopup();
    if (showAnimation)
        openContentLoader('body');
    $.ajax({
        type: Type,
        url: Url,
        data: Params,
        async: false,
        beforeSend: function () {
            
        },
        success: function (response) {
            $('#component_selection_section').hide();
            $('#component_create_section').hide();
            $('#component_list_section').hide();
            $('#config_assessment_section').hide();
            $('#config_registration_section').hide();
            $("#vehicle_config_assessment_btn").hide();
            $('#config_create_section').hide();
            $("#movement_assessment_section").hide();
            $("#component_view_section").hide();
            if (flag == 1 || flag == 0) {
                $('#widgetsContent').find('.comp_').remove();
                $('#hf_compCount').remove();
                $('#hf_compType1').remove();
                $('#hf_compType2').remove();
                $(response).insertBefore('#dropdown_div');
            }
            else {
                $(ResLoadContnr).show();
                $(ResLoadContnr).html(response);
            }
            if (typeof callBackFn != 'undefined') {
                callBackFn();
            }
        },
        error: function (result) {
            if (showAnimation)
                closeContentLoader('body');
        },
        complete: function () {
            if (showAnimation)
                closeContentLoader('body');
        }
    });
}
function FillComponentFav() {
    var orgId = $('#OrgID').val();
    $.ajax({
        async: false,
        type: "POST",
        url: '../VehicleConfig/GetComponentFavourites',
        dataType: "json",
        data: { OrganisationId: orgId},
        processdata: true,
        beforeSend: function () {
            openContentLoader("#componentFav");
        },
        success: function (result) {

            var li = "";
            var favList = result;
            $.each(favList, function (key, value) {
                li += '<li id="table-head" class="pr-2 pl-2 create-vehicle-import-comp-config" data-compid=' + value.ComponentId + '><span><a class="dropdown-item edit-normal" href="#">' + value.ComponentName + '</a></span><span><img src="../Content/assets/images/star-enabled.svg" width="20" alt="vehicle-1"></span></li><hr class="pl-0">';
            });

            $("ul #componentFav").prepend(li);

        },
        error: function (result) {

        },
        complete: function () {
            closeContentLoader("#componentFav");
        }
    });
}
function CreateComponentForVehicle(flag, isAddCompBtnClick=false,compTypeId=0,compSubTypeId=0) {
    var componentCount = $('#ComponentCount').val();
    if (componentCount > 8) {
        ShowErrorPopup("No.of Component count exeeded the limit")
    }
    else {
        var configId = $('#vehicleConfigId').val() == "" ? 0 : $('#vehicleConfigId').val();
        if (configId == undefined || configId == "undefined") {
            configId = 0;
        }
        var IsApplication = false;
        if ($('#IsApplication').val() == "true" || $('#IsApplication').val() == "True") {
            IsApplication = true;
        }
        var isCandidate = false;
        if ($('#IsCandVersion').val() == "true" || $('#IsCandVersion').val() == "True") {
            isCandidate = true;
        }
        var guid = $('#GUID').val();
        var previousComponentTypeId = $('.CreateComponent:last').find('#ComponentType').val()||0;
        var previousComponentSubTypeId = $('.CreateComponent:last').find('#VehicleSubType').val() || 0;
        LoadVehicleContentForAjaxCalls("GET", '../VehicleConfig/CreateComponent', {
            vehicleConfigId: 0, isMovement: IsApplication, guid: guid, isCandidate: isCandidate, flag: flag,
            componentCount: componentCount, previousComponentTypeId: previousComponentTypeId, previousComponentSubTypeId: previousComponentSubTypeId,
            isAddCompBtnClick: isAddCompBtnClick, isDeleteButtonClicked: isDeleteButtonClicked, isAddCompBtnClick: isAddCompBtnClick, isImportComponent: importComponentFromFleet,
            firstComponetTypeId: $('#component_selection_section .drpComponentType:first').val()
        }, '#component_create_section', 1, function () {
            CreateComponentInit();
            isDeleteButtonClicked = false;
            $('#dropdown_div').hide();
            VehicleConfigurationAssessmentInit('config_assessment_section');

            $('#component_selection_section').show();


            if (FleetStepFlag == 0.1) {
                $('#vehicle_back_btn').hide();
                FleetStepFlag = 0;
            }
            if (isCandidate) {
                CandidateBackSubFlag = 0;
            }
            if (guid != "") {
                $('#ComponentList #vehicle-shadow').attr("style", "display:block");
                $('#ComponentList #ComponentDelete').attr("style", "display:block ;padding-left: 12rem;margin-top: -5rem;");

                AssessConfigType();
                if ($('#IsMovement').val() == "True" || $('#IsMovement').val() == "true") {
                    $('#back_btn_inVehiclePage').show();
                    $('#btnbacktochoose').hide();
                }
                $('#backbutton').hide();
                if (isCandidate) {
                    $('#divbtn_candidateVehicleCreate').hide();
                    $('#back_btn_inVehiclePage').show();
                    $('#btnbacktochoose').hide();
                }
            }
            importComponentFromFleet = 0;

            if (typeof isPlanMovementEditStarted !='undefined' && isPlanMovementEditStarted) {
                isVehicleHasChanged = false;
            }
            if (typeof isPlanMovementEditStarted != 'undefined' && isPlanMovementEditStarted == false && typeof BackAndForthNavMethods != 'undefined' &&  BackAndForthNavMethods.IsWorkFlowStarted() == true && isWorkFlowCompleted == false && $('#hf_IsPlanMovmentGlobal').length>0 && (StepFlag == NavigationEnum.VEHICLE_DETAILS)) {
                isVehicleHasChanged = true;
            }            
        });
        
    }
}
function VehTypeChange(thisVal) {

    var vehicleTypeId = $(thisVal).val();
    var divId = $(thisVal).closest('.CreateComponent').attr('id');
    $('#' + divId).find('#VehicleSubType option').remove();
    if (vehicleTypeId == "") {
        $('#' + divId).find('#VehicleSubType').attr("disabled", "disabled");
    }
    else {
        $('#' + divId).find('#VehicleSubType').attr("disabled", "");
        FillVehicleSubTypeDrp(vehicleTypeId, divId);
    }
    vehicleResetOnEdit = false;
    //if ($('#IsVehicleConfigEdit').val() == "1") {
    //    vehicleResetOnEdit = true;
    //}
    //else {
    //    vehicleResetOnEdit = false;
    //}
}
var vehicleTypeIdGlobal = 0;
function FillVehicleSubTypeDrp(vehicleTypeId, divId) {
    $('#' + divId).find('#VehicleSubType option:not(:first-child)').remove();

    $.ajax({
        async: false,
        type: "POST",
        url: '../VehicleConfig/FillVehicleSubType',
        dataType: "json",
        data: { vehicleTypeId: vehicleTypeId },
        beforeSend: function () {
            startAnimation();
        },
        success: function (data) {
            var datalength = data.type.length;
            var element = $('#' + divId);
            if (element.length <= 0) 
                element = $('.CreateComponent');

            for (var i = 0; i < datalength; i++) {
                element.find('#VehicleSubType').append('<option value="' + data.type[i].SubCompType + '">' + data.type[i].SubCompName + '</option>');
                element.find('#VehicleSubType').attr("disabled", false);
            }
            //Hide Subtypes based on Type
            element.find('#VehicleSubType').closest('.row').show();
            if (vehicleTypeId == TypeConfiguration.CONVENTIONAL_TRACTOR || vehicleTypeId == TypeConfiguration.RIGID_VEHICLE
                || vehicleTypeId == TypeConfiguration.SPMT || vehicleTypeId == TypeConfiguration.TRACKED_VEHICLE || vehicleTypeId == TypeConfiguration.MOBILE_CRANE
                || vehicleTypeId == TypeConfiguration.RECOVERY_VEHICLE
                || vehicleTypeId == TypeConfiguration.ENGINEERING_PLANT_SEMI_TRAILER
                || vehicleTypeId == TypeConfiguration.ENGINEERING_PLANT_DRAWBAR_TRAILER
                || vehicleTypeId == TypeConfiguration.GIRDER_SET
                || vehicleTypeId == TypeConfiguration.MOBILE_CRANE
            ) {
                element.find('#VehicleSubType').closest('.row').hide();
            }

            vehicleTypeIdGlobal = parseInt(vehicleTypeId);

            element.find('#VehicleSubType').val(data.defaultType);
            VehSubTypeChange(element.find('#VehicleSubType'),0);
        },
        error: function (result) {
            $('#' + divId).find('#VehicleSubType').html(result.responseText);
            stopAnimation();
        },
        complete: function () {
            stopAnimation();
        }
    });
}

function VehSubTypeChange(thisVal,isSubTypeClick=0) {

    var divId = $(thisVal).closest('.CreateComponent').attr('id');
    var vehicleSubTypeId = $(thisVal).val();
    var componentTypeId = $('#' + divId).find('#ComponentType').val();
    $.ajax({
        async: false,
        type: "POST",
        url: '../VehicleConfig/GetComponentImage',
        dataType: "json",
        data: { componentTypeId: componentTypeId, componentSubTypeId: vehicleSubTypeId },
        beforeSend: function () {
            startAnimation();
        },
        success: function (data) {

            var imgurl = "../Content/images/Common/MasterPage/componet_icons/" + data.result + ".png";
            $('#' + divId).find('#singleComponentImg').attr("src", imgurl);
            $('#div_vehicleImg').show();
            $('#div_vehicleImg').append('<img alt="vehicle-2" id="vimg" src="' + imgurl + '">');
            $('#' + divId).find('#vehicle-shadow').attr("style", "display:block");
            $('#CreateNewComponent #ComponentDelete').attr("style", "display:block; padding-left: 12rem;margin-top: -5rem;");
            $('#vehicle_next_btn').show();
            $('#backbutton').hide();
            AddComponent(divId, isSubTypeClick);
            VehicleConfigurationAssessmentInit('config_assessment_section');
        },
        error: function (result) {
            stopAnimation();
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function PreviousPage() {
    startAnimation();
    CloseWarningPopupRef();
    isVehicleHasChanged = false;
    var componentCount = $('#ComponentCount').val();
    if (FleetStepFlag == 0.1) {
        $('#component_create_section').show();
        $('#component_selection_section').show();
        $('#component_list_section').hide();
        $('#vehicle_back_btn').hide();
        $('#backbutton').hide();
        if ($('#IsMovement').val() == "True" || $('#IsMovement').val() == "true") {
            $('#back_btn_inVehiclePage').show();
            $('#btnbacktochoose').hide();
        }
        if (componentCount > 0) {
            $('#vehicle_next_btn').show();
        }
        if ($('#IsCandVersion').val() == "True" || $('#IsCandVersion').val() == "true") {
            $('#divbtn_candidateVehicleCreate').show();
        }
        FleetStepFlag = 0;
    }
    else if (FleetStepFlag == 0.2) {
        $('#component_list_section').hide();
        $('#component_create_section').show();
        $('#component_selection_section').show();
        $("#config_assessment_section").show();
        $('#vehicle_back_btn').hide();
        if (componentCount > 0) {
            $("#vehicle_config_assessment_btn").show();
        }
        if ($('#IsMovement').val() == "True" || $('#IsMovement').val() == "true") {
            $('#back_btn_inVehiclePage').show();
            $('#btnbacktochoose').hide();
        }
        FleetStepFlag = 1;
    }
    else if (FleetStepFlag == 0.3) {
        $('#component_view_section').hide();
        $('#component_list_section').show();
        FleetStepFlag = 0.2;
    }
    else if (FleetStepFlag == 1) {
        $('#component_create_section').show();
        $('#vehicle_config_assessment_btn').hide();
        $('#vehicle_back_btn').hide();
        $('#vehicle_next_btn').show();
        $('#backbutton').hide();
        $("#config_assessment_section").html('');
        if ($('#IsMovement').val() == "True" || $('#IsMovement').val() == "true") {
            $('#back_btn_inVehiclePage').show();
            $('#btnbacktochoose').hide();
        }
        FleetStepFlag = 0;
    }
    else if (FleetStepFlag == 2) {
        $('#component_selection_section').show();
        if (componentCount > 3) {
            if ($('#vehicle_Create_section').length > 0)
                $('#vehicle_Create_section #scroll-btns').show();
            else
                $('#scroll-btns').show();
        }
        $('#component_create_section').show();
        $("#config_assessment_section").show();
        $('#vehicle_config_assessment_btn').show();
        $('#vehicle_movement_confirm_btn').hide();
        $('#config_create_section').hide();
        $('#config_registration_section').hide();
        $("#movement_assessment_section").hide();
        if ($('#IsMovement').val() == "True" || $('#IsMovement').val() == "true" || $('#IsCandVersion').val() == "true" || $('#IsCandVersion').val() == "True") {
            $('#back_btn_inVehiclePage').show();
            $('#btnbacktochoose').hide();
        }
        $('#vehicle_back_btn').hide();
        $("#vehicle_save_btn").hide();
        FleetStepFlag = 0;
    }
    else if (FleetStepFlag == 3) {
        $('#vehicle_movement_confirm_btn').show();
        VehicleConfigPage();
        //$('#config_create_section').show();
        //$('#config_registration_section').show();
        $('#movement_assessment_section').show();
        $('#component_detail_section').hide();
        $('#config_review_section').hide();
        $("#vehicle_save_btn").hide();
        FleetStepFlag = 2;
    }

    stopAnimation();
}
var vehicleSubTypeIdGlobal = 0;
function AddComponent(divId, isSubTypeClick=0) {
    isVehicleEditOnPlanMovement = false;
    var orgId = 0;
    if ($('#Organisation_ID').val() != undefined && $('#Organisation_ID').val() != '') {
        orgId = $('#Organisation_ID').val();
    }
    var configId = $('#vehicleConfigId').val() == "" ? 0 : $('#vehicleConfigId').val();
    if (configId == undefined || configId == "undefined") {
        configId = 0;
    }

    var IsApplication = false;
    if ($('#IsApplication').val() == "true" || $('#IsApplication').val() == "True") {
        IsApplication = true;
    }
    var guid = $('#GUID').val();

    var componentIdExisting = 0;
    if (divId != "CreateComponent") {
        componentIdExisting = divId.split("CreateComponent_")[1];
    }
    var componentTypeId = $('#' + divId).find('#ComponentType').val();
    var vehicleSubTypeId = $('#' + divId).find('#VehicleSubType').val();

    vehicleSubTypeIdGlobal = parseInt(vehicleSubTypeId);
    var totalCompCount = $('.Comp_Div').length;

    if (isSubTypeClick == 0 || (isSubTypeClick == 1 && componentTypeId == TypeConfiguration.ENGINEERING_PLANT)) {
	    var compDivParent = $('#' + divId).closest('.Comp_Div');
	    var compDivCount = compDivParent.nextAll('.Comp_Div').length || 0;
        if ((isSubTypeClick == 0 && compDivCount > 0) || (compDivCount > 0 && isSubTypeClick == 1
            && componentTypeId == TypeConfiguration.ENGINEERING_PLANT)) {
	        var nextComps = compDivParent.nextAll('.Comp_Div');
            if (isSubTypeClick == 0) {
                nextComps.push(compDivParent);
            }
            $(nextComps).each(function () {                
	            var CreateComponentDiv = $(this).find('.CreateComponent');
                var compDiv = CreateComponentDiv.attr('id');
                if (compDiv != "CreateComponent") {
                    var compId = compDiv.split("CreateComponent_");
                    DeleteVehicleComponent(compId[1], null, 1);

                    var delDiv = $('#' + compDiv).closest('.Comp_Div').attr('id');
                    $('#' + delDiv).remove();
                    deleteFlag = 2;
                }
	        });
        } else if (isSubTypeClick == 0) {
	        var CreateComponentDiv = compDivParent.find('.CreateComponent');
	        var compDiv = CreateComponentDiv.attr('id');
	        if (compDiv != 'CreateComponent') {
	            var compId = compDiv.split("CreateComponent_");
	            DeleteVehicleComponent(compId[1], null, 1);

	            var delDiv = $('#' + compDiv).closest('.Comp_Div').attr('id');
	            $('#' + delDiv).remove();
	            //deleteFlag = 2;
	        }
	    }
	}

    $.ajax({
        async: false,
        type: "POST",
        url: '../VehicleConfig/AddComponent',
        dataType: "json",
        data: { vehicleConfigId: configId, componentTypeId: componentTypeId, componentSubTypeId: vehicleSubTypeId, isMovement: IsApplication, organisationId: orgId, guid: guid, isSubTypeClick: isSubTypeClick, componentIdExisting: componentIdExisting },
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
            $('#' + divId).find(".CreateComponent").attr('id', 'CreateComponent_' + result.componentId);
            $("body").off('click', '#' + divId + ' #ComponentDelete');
            $("body").on('click', '#' + divId + ' #ComponentDelete', function () {
                window['DeleteVehicleComponent'](result.componentId, divId);
                
            });

            $('#GUID').val(result.guid);
            var flag = 0;
            if (totalCompCount == 0 ) {
                flag = 0;
            }
            else if (totalCompCount != 0) {
                flag = 1;
            }
            else {
                flag = 3;
            }
            var ConfigTypeId = $('#ConfigTypeId').val();//
            //SubTypeCLick=1 - enggplant=type
            if (ConfigTypeId != VEHICLE_CONFIGURATION_TYPE_CONFIG.RECOVER_VEHICLE && isSubTypeClick == 1 && componentTypeId == TypeConfiguration.ENGINEERING_PLANT
                && (vehicleSubTypeId == SubTypeConfiguration.ENGPLANT_CONVENTIONAL_TRACTOR
                    || vehicleSubTypeId == SubTypeConfiguration.ENGPLANT_BALLAST_TRACTOR)) {
                AddDefaultComponent(componentTypeId, vehicleSubTypeId, flag);
            } else if (isSubTypeClick == 0) {
                if (componentTypeId == TypeConfiguration.DRAWBAR_TRAILER || componentTypeId == TypeConfiguration.SEMI_TRAILER ||
                    componentTypeId == TypeConfiguration.ENGINEERING_PLANT_DRAWBAR_TRAILER || componentTypeId == TypeConfiguration.ENGINEERING_PLANT_SEMI_TRAILER) {
                    CreateComponentForVehicle(flag);
                } else {
                    AddDefaultComponent(componentTypeId, vehicleSubTypeId, flag);
                }
            } else if (isSubTypeClick == 1) {
                AssessConfigType();
            }

        },
        error: function (result) {
            stopAnimation();
        },
        complete: function () {
            //totalCompCount = $('.Comp_Div').length;
            //$('#ComponentCount').val(totalCompCount);
            stopAnimation();
        }
    });
}
function DeleteVehicleComponentConfirmation(componentId, _this) {
    isDeleteButtonClicked = true;
    var msg = "Do you want to delete the component?";
    ShowWarningPopup(msg, 'DeleteVehicleComponent', 'CloseWarningPopupRefDeleteComp', componentId, _this,0);
   
}
function CloseWarningPopupRefDeleteComp() {
    isDeleteButtonClicked = false;
    CloseWarningPopupRef();
}
var isDeleteButtonClicked = false;
function DeleteVehicleComponent(componentId, _this, flag) {
    isVehicleEditOnPlanMovement = false;
    var divId = $(_this).closest('.Comp_Div').attr('id');
    var configId = $('#vehicleConfigId').val() == "" ? 0 : $('#vehicleConfigId').val();
    if (configId == undefined || configId == "undefined") {
        configId = 0;
    }
    var IsApplication = false;
    if ($('#IsApplication').val() == "true" || $('#IsApplication').val() == "True") {
        IsApplication = true;
    }
    $.ajax({
        async: false,
        type: "POST",
        url: '../VehicleConfig/DeleteComponentConfiguration',
        dataType: "json",
        data: { componentId: componentId, vehicleId: configId, isMovement: IsApplication },
        beforeSend: function () {
            if (isDeleteButtonClicked)
                startAnimation();
        },
        success: function (result) {
            CloseWarningPopup();
            if (flag == 0) {
                if (result.result > 0) {
                    showToastMessage({
                        message: "Component deleted successfully",
                        type: "success"
                    });
                    DeleteCompletion(divId);
                    if ($('.Comp_Div').length <= 0)//if all components deleted, we don't need to keep the values
                        localStorage.removeItem('ComponentTempData');
                }
            }
            else {
                var totalCompCount = $('#ComponentCount').val();
                $('#ComponentCount').val(parseInt(totalCompCount) - 1);
            }
        },
        error: function (result) {
            stopAnimation();
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function DeleteCompletion(divId) {
    $('#' + divId).remove();
    CreateComponentForVehicle(3);
    vehicleTypeIdGlobal = 0;
    VehicleConfigurationAssessmentInit('config_assessment_section');

}
function SelectVehiclecomponentFromFleet() {
    var componentCount = $('#ComponentCount').val();
    if (componentCount > 8) {
        ShowErrorPopup("No.of Component count exeeded the limit")
    }
    else {
        $.ajax({
            type: "POST",
            url: '../Vehicle/FleetComponent',
            data: { isFromConfig: 1 },
            beforeSend: function () {
                startAnimation();
            },
            success: function (response) {
                if (FleetStepFlag == 0) {
                    FleetStepFlag = 0.1;
                }
                $('#component_selection_section').hide();
                $('#component_create_section').hide();
                $('#component_list_section').html($(response).find('#vehicle-components-list'), function () {
                    event.preventDefault();
                });
                $('#banner-container').find('div#filters').remove();
                document.getElementById("vehicles").style.filter = "unset";
                if (!$('#vehicle_Create_section')[0]) {
                    $("#component_list_section").prepend('<div class="row"><div class="col-lg-9 col-md-9 col-sm-9"><span id="list_heading" class="title">Select component from fleet</span></div><div class="col-lg-2 col-md-2 col-sm-2"><div class="button main-button mr-0"></div></div></div>');


                }
                var filters = $(response).find('div#filters');

                var movementId = $('#movementTypeId').val();
                if (movementId != undefined && movementId != "") {
                    var mvmnt = filters.find('#Indend :selected').text();
                    if (mvmnt == "Intended Use" || mvmnt != $('#movementClassificationName').val()) {
                        mvmnt = $('#movementClassificationName').val();
                    }
                    filters.find('#Indend option').remove();
                    filters.find('#Indend').append($('<option>', {
                        text: mvmnt
                    }, '</option>'));
                }
                $(filters).appendTo('#banner-container');
                var pagination = $(response).find('div#comp-pagination');
                $(pagination).appendTo('#component_list_section');
                $('#component_list_section').show();
                $('#vehicle_back_btn').show();
                $('#btnbacktochoose').hide();
                $('#vehicle_next_btn').hide();
                $('#back_btn_inVehiclePage').hide();
                $('#divbtn_candidateVehicleCreate').hide();
                $("#ImportFrom").val('fleet');
                if (FleetStepFlag == 1) {
                    $("#config_assessment_section").hide();
                    $('#vehicle_next_btn').hide();
                    $("#vehicle_config_assessment_btn").hide();
                    FleetStepFlag = 0.2;
                }
                $('#backbutton').hide();
                //removeCompHrefLinks();
                //PaginateComponentList();
            },
            error: function (result) {
            },
            complete: function () {
                stopAnimation();
            }
        });
    }

}
function removeCompHrefLinks() {
    $('#component_list_section').find('.pagination').find('li a').removeAttr('href').css("cursor", "pointer");
}
function PaginateComponentList() {
    $('#component_list_section').find('.pagination').find('li').not('.active, .PagedList-skipToLast, .PagedList-skipToNext, .PagedList-skipToFirst, .PagedList-skipToPrevious').find('a').click(function () {
        var pageNum = $(this).html();
        AjaxPaginationForComponentList(pageNum);
    });
    PaginateComponentToLastPage();
    PaginateComponentToFirstPage();
    PaginateComponentToNextPage();
    PaginateComponentToPrevPage();
}
function AjaxPaginationForComponentList(pageNum) {
    var selectedVal = $('#pageSizeVal').val();
    var pageSize = selectedVal;
    var searchString = $('#searchText').val();
    var vehicleIntend = $('#Indend').val();
    var vehicleType = $('#VehType').val();
    var searchFavourites = $('#FilterFavourites').is(":checked");

    $.ajax({
        url: '../Vehicle/FleetComponent',
        data: { page: pageNum, pageSize: pageSize, searchString: searchString, searchVhclType: vehicleType, searchVhclIntend: vehicleIntend, isFromConfig: 1, filterFavourites: searchFavourites },
        type: 'GET',
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {

            $('#component_list_section').html($(response).find('#vehicle-components-list'), function () {
                event.preventDefault();
            });
            if (!$('#vehicle_Create_section')[0]) {
                $("#component_list_section").prepend('<div class="row"><div class="col-lg-9 col-md-9 col-sm-9"><span id="list_heading" class="title">Select component from fleet</span></div></div>');
            }
            $('#divAllComponent').html('');
            $('#banner-container').find('div#filters').remove();
            document.getElementById("vehicles").style.filter = "unset";
            var filters = $(response).find('div#filters');

            var movementId = $('#movementTypeId').val();
            if (movementId != undefined && movementId != "") {
                var mvmnt = filters.find('#Indend :selected').text();
                filters.find('#Indend option').remove();
                filters.find('#Indend').append($('<option>', {
                    text: mvmnt
                }, '</option>'));
            }
            $(filters).appendTo('#banner-container');
            var pagination = $(response).find('div#comp-pagination');
            $(pagination).appendTo('#component_list_section');
            //removeCompHrefLinks();
            //PaginateComponentList();
        },
        error: function (xhr, textStatus, errorThrown) {
        },
        complete: function () {
            stopAnimation();
        }
    });
}
//method to paginate to last page
function PaginateComponentToLastPage() {
    $('#component_list_section').find('.PagedList-skipToLast').click(function () {
        var pageCount = $('#TotalPages').val();
        AjaxPaginationForComponentList(pageCount);

    });
}
//method to paginate to first page
function PaginateComponentToFirstPage() {
    $('#component_list_section').find('.PagedList-skipToFirst').click(function () {
        AjaxPaginationForComponentList(1);
    });
}
//method to paginate to Next page
function PaginateComponentToNextPage() {
    $('#component_list_section').find('.PagedList-skipToNext').click(function () {
        var thisPage = $('#component_list_section').find('.active').find('a').html();
        var nextPage = parseInt(thisPage) + 1;
        AjaxPaginationForComponentList(nextPage);

    });
}
//method to paginate to Previous page
function PaginateComponentToPrevPage() {
    $('#component_list_section').find('.PagedList-skipToPrevious').click(function () {
        var thisPage = $('#component_list_section').find('.active').find('a').html();
        var prevPage = parseInt(thisPage) - 1;
        AjaxPaginationForComponentList(prevPage);
    });
}
/// import component (clone component)
function ImportComponentToConfig(componentId,isFromFleet) {
    var componentCount = $('#ComponentCount').val();
    if (componentCount > 8) {
        ShowErrorPopup("No.of Component count exeeded the limit")
    }
    else {
        var IsApplication = false;
        var vehicleConfigId = 0;
        if ($('#IsApplication').val() == "true" || $('#IsApplication').val() == "True") {
            IsApplication = true;
        }
        if ($('#vehicleConfigId').val() != undefined) {
            vehicleConfigId = $('#vehicleConfigId').val();
        }
        var isCandidateVehicle = false;
        if ($('#IsCandVersion').val() == "True" || $('#IsCandVersion').val() == "true") {
            isCandidateVehicle = true;
            if (isFromFleet)
                isCandidateVehicle = false;
        }
        var guid = $('#GUID').val();
        $.ajax({
            async: false,
            type: "POST",
            url: '../VehicleConfig/ImportComponent',
            dataType: "json",
            //contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ componentId: componentId, isMovement: IsApplication, vehicleId: vehicleConfigId, guId: guid, isCandidate: isCandidateVehicle }),
            processdata: true,
            beforeSend: function () {
                startAnimation();
            },
            success: function (result) {
                importComponentFromFleet = 1;
                $('#GUID').val(result.Guid);
                CreateComponentForVehicle(1);                
            },
            error: function (result) {
            },
            complete: function () {
                VehicleConfigurationAssessmentInit('config_assessment_section');
                stopAnimation();
            }
        });
    }
}
function openFilterInConfig() {
    var filterWindowWidth = "";
    if ($('#ImportFrom').val() == 'createcomponent' && $('#ImportFrom').val() != 'fleet') {
        filterWindowWidth = "660px";
    }
    else {
        filterWindowWidth = "350px";
    }
    document.getElementById("filters").style.width = filterWindowWidth;
    //document.getElementById("vehicles").style.filter = "brightness(0.5)";
    document.getElementById("vehicles").style.background = "white";

    function myFunction(x) {
        if (x.matches) { // If media query matches
            document.getElementById("filters").style.width = "200px";
        }
    }
    var x = window.matchMedia("(max-width: 770px)")
    myFunction(x) // Call listener function at run time
    x.addListener(myFunction)
}
function closeFilters() {
    document.getElementById("filters").style.width = "0";
    document.getElementById("vehicles").style.filter = "unset";
}
function SearchVehicleComponentInConfig() {
    var searchString = $('#searchText').val();
    var vehicleIntend = $('#Indend').val();
    var vehicleType = $('#VehType').val();
    var IsFromConfig = $('#IsFromConfig').val();
    var searchFavourites = $('#FilterFavourites').is(":checked");
    var filterFavourites = 0;
    if (searchFavourites) {
        filterFavourites = 1;
    }
    else {
        filterFavourites = 0;
    }
    closeFilters();
    $.ajax({
        url: '../Vehicle/SaveSearchData',
        type: 'POST',
        cache: false,
        async: false,
        data: { searchString: searchString, vehicleIntend: vehicleIntend, vehicleType: vehicleType, filterFavourites:filterFavourites, isFromConfig: IsFromConfig },
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {

            stopAnimation();
            $('#component_list_section').html($(response).find('#vehicle-components-list'), function () {
            });
            if (!$('#vehicle_Create_section')[0]) {
                $("#component_list_section").prepend('<div class="row"><div class="col-lg-9 col-md-9 col-sm-9"><span id="list_heading" class="title">Select component from fleet</span></div><div class="col-lg-2 col-md-2 col-sm-2"><div class="button main-button mr-0"></div></div></div>');

            }
            var filters = $(response).find('div#filters');

            var movementId = $('#movementTypeId').val();
            if (movementId != undefined && movementId != "") {
                //var IndentDrpdwn = $(response).find('div#filters #Indend');
                var mvmnt = filters.find('#Indend :selected').text();
                filters.find('#Indend option').remove();
                filters.find('#Indend').append($('<option>', {
                    text: mvmnt
                }, '</option>'));


            }
            $(filters).appendTo('#banner-container');
            var pagination = $(response).find('div#comp-pagination');
            $(pagination).appendTo('#component_list_section');
            $('#component_list_section').show();
            $('#component_list_section').append('<button id="backFromFleet" class="btn btn-outline-primary SOAButtonHelper ml2 mb-2 create-vehilce-btn-back-from-fleet" role="button" aria-pressed="true" style="background-color: white; float:right;">BACK</button>');
            $('#vehicle_back_btn').show();
            $('#btnbacktochoose').hide();
            $('#backFromFleet').hide();
            $("#ImportFrom").val('fleet');
            //removeCompHrefLinks();
            //PaginateComponentList();

        }
    });
}
$('body').on('click', '.create-vehilce-btn-back-from-fleet', function (e) {
    OnBackBtnClick();
});
function BackButtonVehiclePagefn() {
    IsVehicleComponentTypeChanged = false;
    isVehicleHasChanged = false;
    CloseWarningPopupRef();
    var isCandidateVehicle = false;
    if ($('#IsCandVersion').val() == "True" || $('#IsCandVersion').val() == "true") {
        isCandidateVehicle = true;
    }
    if (!isCandidateVehicle) {
        OnBackButtonClick();
        $('#backbutton').show();
    }
    else {
        CandidateBackSubFlag = 0;
        CandidateVehicleBackButton();
        //$('#divbtn_candidateVehicleCreate').show();
    }
    localStorage.removeItem('ComponentTempData');
}
function clearComponentSearch() {
    $(':input', '#filters')
        .not(':button, :submit, :reset, :hidden')
        .val('')
        .removeAttr('selected');
    $("#FilterFavourites").prop("checked", false);
    SearchVehicleComponentInConfig();
}
function AssessConfigType() {
    var guid = $('#GUID').val();
    var BoatMastFlag = $('#BoatMastException').is(':checked');
    $.ajax({
        url: "../VehicleConfig/AssessConfigurationType",
        type: 'GET',
        //contentType: 'application/json; charset=utf-8',
        async: false,
        data: { guId: guid, boatMastFlag: BoatMastFlag },
        success: function (data) {
            $('#ConfigTypeId').val($(data).find("#hf_configId").val());
            if (isVehicleEditOnPlanMovement) {
                localStorage.setItem('ConfigTypeIdTemp', $('#ConfigTypeId').val());
            }
            var isMovement = $('#IsMovement').val().toLowerCase() == "true" ? true : false;
            var isCandidateVehicle = false;
            if ($('#IsCandVersion').val() != undefined) {
                isCandidateVehicle = $('#IsCandVersion').val().toLowerCase() == "true" ? true : false;
            }
            $("#config_assessment_section").show();
            $("#config_assessment_section").html(data);
            $('#div_VehicleTypeConfig').show();
            if ($(data).find("#VehicleTypeConfig").html().trim().length == 0) {
                $('#spnconfigmessage').html("The system cannot identify a configuration type for the component(s) selected. Kindly review.");
                $("#vehicle_next_btn").hide();
                //$("#vehicle_back_btn").show();
                $('#back_btn_inVehiclePage').hide();
                FleetStepFlag = 1;
                if (!isMovement && !isCandidateVehicle) {
                    $('#scoll-left-btn').css("margin-top", "-39rem");
                    $('#scoll-right-btn').css("margin-top", "-39rem");
                }
            }
            else {
                var msg = "Based on the details you've filled in, the Configuration type is  ";
                $('#spnconfigmessage').html(msg);
                $("#vehicle_config_assessment_btn").show();
                $("#vehicle_next_btn").hide();
                if ($('#IsMovement').val() == "True" || $('#IsMovement').val() == "true") {
                    $('#back_btn_inVehiclePage').show();
                    $('#btnbacktochoose').hide();
                }
                else {
                    $('#back_btn_inVehiclePage').hide();
                    $('#btnbacktochoose').show();
                }
                FleetStepFlag = 1;
                $('#scoll-left-btn').css("margin-top", "-50rem");
                $('#scoll-right-btn').css("margin-top", "-50rem");
            }

        },
        error: function (data) {

        }
    });
}
function VehicleConfigPage() {
    var vehicleData = localStorage.getItem('ComponentTempData');
    var configTypeIdTemp = localStorage.getItem('ConfigTypeIdTemp');
    var newConfigTypeId = $('#ConfigTypeId').val();
    var oldConfigTypeId;
    if (vehicleData) {
        var vehicleDataObj = JSON.parse(vehicleData);
        oldConfigTypeId = vehicleDataObj.configTypeId;
    } else if (configTypeIdTemp) {
        oldConfigTypeId = configTypeIdTemp;
    }

    if (oldConfigTypeId != undefined && oldConfigTypeId != newConfigTypeId) {
        if (
            (oldConfigTypeId == VEHICLE_CONFIGURATION_TYPE_CONFIG.SEMI_TRAILER && newConfigTypeId == VEHICLE_CONFIGURATION_TYPE_CONFIG.SEMI_TRAILER_3_TO_8) ||
            (oldConfigTypeId == VEHICLE_CONFIGURATION_TYPE_CONFIG.SEMI_TRAILER_3_TO_8 && newConfigTypeId == VEHICLE_CONFIGURATION_TYPE_CONFIG.SEMI_TRAILER) ||
            (oldConfigTypeId == VEHICLE_CONFIGURATION_TYPE_CONFIG.DRAWBAR_TRAILER && newConfigTypeId == VEHICLE_CONFIGURATION_TYPE_CONFIG.DRAWBAR_TRAILER_3_TO_8) ||
            (oldConfigTypeId == VEHICLE_CONFIGURATION_TYPE_CONFIG.DRAWBAR_TRAILER_3_TO_8 && newConfigTypeId == VEHICLE_CONFIGURATION_TYPE_CONFIG.DRAWBAR_TRAILER)
        ) {
            console.log('vehicle config type not changed');
            vehicleResetOnEdit = false;
            VehicleDetailsPage();
        } else {
            var msg = "Type is changed also the previously entered information will be reset. Do you want to continue?";
            vehicleResetOnEdit = true;
            ShowWarningPopup(msg, 'VehicleDetailsPage', 'CloseWarningPopup');
        }
    } else {
        VehicleDetailsPage();
    }
}
function VehicleDetailsPage() {
    CloseWarningPopup();
    $('html, body').animate({
        scrollTop: $("#config_create_section").offset().top
    }, 2000);
    localStorage.removeItem('ConfigTypeIdTemp');
    var vehicleId = $("#vehicleId").val();
    var configId = $('#ConfigTypeId').val();
    var mvmntType = $('#MovementTypeId').val();
    if (mvmntType == "0")
        mvmntType = $('#movement_Id').val() != undefined ? $('#movement_Id').val() : $('#MovementTypeId').val();
    var guid = $('#GUID').val();
    var isMovement = false;
    if ($('#IsMovement').val() == "True" || $('#IsMovement').val() == "true")
        isMovement = true;

    var isCandidateVehicle = false;
    if ($('#IsCandVersion').val() == "true" || $('#IsCandVersion').val() == "True") {
        isCandidateVehicle = true;
    }
    var VSONotificationVehicle = false;
    if ($('#NotiVSO').val() != '') {
        VSONotificationVehicle = true;
    }
    openContentLoader('body');
    LoadVehicleContentForAjaxCalls("POST", '../VehicleConfig/VehicleConfiguration', { movementId: mvmntType, vehicleConfigId: configId, vehicleId: vehicleId, planMovement: isMovement, isEditVehicleInSoProcessing: isCandidateVehicle, guId: guid, VSONotificationVehicle: VSONotificationVehicle, vehicleResetOnEdit: vehicleResetOnEdit}, '#config_create_section', -1, function () {
        VehicleConfigurationInit();
        VehicleConfig_VehicleConfigurationInit();
        var componentCount = $('#ComponentCount').val();
        IterateThroughComponent()
        ShowFeetComponent();
        ConfigAutoFillOnEdit();
        AssessmentForImportedComponent();
        if ($("#IsVehicleConfigEdit").val() != 1) {
            if ($('#MovementTypeId').val() == "270101" || $('#MovementTypeId').val() == "270155")
                $("#movement_assessment_section").hide();
        }
        compHtml = GenerateComponentModel(1);
        LoadComponentSection(true);
        Comp_GeneralInit();
        //Comp_AxleInit(0);
        AxleComponentInit();
        Comp_RegisterInit();
        Comp_CreateInit();

        vehicleConfigAssessmentBtnCLicked = false;
        VehicleRegistartionPage(FillOldDataSavedOnBackAction);
        
    });

    
    $("#vehicle_movement_confirm_btn").hide();
    $('#vehicle_save_btn').show();
    $('#vehicle_back_btn').show();
    $('#btnbacktochoose').hide();
    $('#back_btn_inVehiclePage').hide();
    if ($('#vehicle_Create_section').length > 0)
        $('#vehicle_Create_section #scroll-btns').hide();
    else
        $('#scroll-btns').hide();
    CloseWarningPopup();
}
var vehicleDataLocalStorageExist = false;
function FillOldDataSavedOnBackAction() {
    var vehicleData = localStorage.getItem('ComponentTempData');
    localStorage.removeItem('ComponentTempData');
    if (vehicleData) {
        vehicleDataLocalStorageExist = true;
        var vehicleDataObj = JSON.parse(vehicleData);
        var newConfigTypeId = $('#ConfigTypeId').val();
        var oldConfigTypeId = vehicleDataObj.configTypeId;
        if (oldConfigTypeId != newConfigTypeId) {
            if (
                (oldConfigTypeId == VEHICLE_CONFIGURATION_TYPE_CONFIG.SEMI_TRAILER && newConfigTypeId == VEHICLE_CONFIGURATION_TYPE_CONFIG.SEMI_TRAILER_3_TO_8) ||
                (oldConfigTypeId == VEHICLE_CONFIGURATION_TYPE_CONFIG.SEMI_TRAILER_3_TO_8 && newConfigTypeId == VEHICLE_CONFIGURATION_TYPE_CONFIG.SEMI_TRAILER) ||
                (oldConfigTypeId == VEHICLE_CONFIGURATION_TYPE_CONFIG.DRAWBAR_TRAILER && newConfigTypeId == VEHICLE_CONFIGURATION_TYPE_CONFIG.DRAWBAR_TRAILER_3_TO_8) ||
                (oldConfigTypeId == VEHICLE_CONFIGURATION_TYPE_CONFIG.DRAWBAR_TRAILER_3_TO_8 && newConfigTypeId == VEHICLE_CONFIGURATION_TYPE_CONFIG.DRAWBAR_TRAILER)
            ) {
                console.log('vehicle config type not changed');
            } else {
                vehicleData = {};
                console.log('vehicle config type changed');
                return false;
            }
        }

        var isNeedToUpdateOldValues = false;//if there is no matching component ids, we don't need to update fields 
        if (vehicleDataObj.componentIds) {
            $(vehicleDataObj.componentIds).each(function (_key, _value) {
                var compId = _value;
                if ($('#' + compId).length > 0) {
                    isNeedToUpdateOldValues = true;
                }
            }).promise().done(function () {
                if (isNeedToUpdateOldValues) {
                    $.each(vehicleDataObj.configDetails, function (_key, _value) {
                        var $elem;
                        if (_key.indexOf('Number_of_Axles##') !== -1) {
                            var keyArr = _key.split('##');
                            $elem = $('#div_config_general #' + keyArr[0] + '.axledropVehicle_' + keyArr[1] + '');
                            if ($elem.length > 0) {
                                $elem.val(_value);
                            } else if ($('#div_config_general #Number_of_Axles').length <= 1){
                                $elem = $('#div_config_general #' + keyArr[0] + '.axledropVehicle');
                                if ($elem.length > 0)
                                    $elem.val(_value);
                            }
                        } else {
                            $elem = $('#div_config_general #' + _key);
                            if ($elem.length > 0)
                                $elem.val(_value);
                        }

                    });
                    AxleTableMethods.AxlesArr = vehicleDataObj.AxlesArr;

                    //Update config reg details
                    if (vehicleDataObj.registrationDetails && vehicleDataObj.registrationDetails.length > 0) {
                        $.each(vehicleDataObj.registrationDetails, function (_key1, _value1) {
                            var table_len = $("#div_reg_config_vehicle #vehicle-reg-table tbody tr").length;
                            $("#div_reg_config_vehicle #vehicle-reg-table").find('tbody tr:last').before("<tr id='row" + table_len + "' class='tr_config_Registration'><td id='registerId" + table_len + "' class='cls_regId txt_register_config tblregcomponent'>" + _value1.RegNo + "</td><td id='fleetId" + table_len + "' class='cls_fleetId txt_fleet_config tblregcomponent'>" + _value1.FleetNo + "</td><td><a href='#' RowId='" + table_len + "' class='delete btngrad btnrds btnbdr tdbutton deleteConfigRow'></a ></td></tr>");
                        });
                    }

                    if (vehicleDataObj.components) {
                        $(vehicleDataObj.components).each(function (_key, _value) {
                            var compId = _value.compId;
                            var componentData = _value.componentData;
                            if (componentData) {
                                $.each(componentData, function (_key1, _value1) {
                                    var $elem;
                                    if (_key1.indexOf('Number_of_Axles##') !== -1) {//Number_of_Axles
                                        var keyArr = _key1.split('##');
                                        $elem = $('#' + compId + ' #' + keyArr[0] + '[axledropcount=' + keyArr[1] + ']');
                                        if ($elem.length > 0) {
                                            $elem.val(_value1);
                                        }
                                    } else {
                                        $elem = $('#' + compId + ' #' + _key1);
                                        if ($elem.length > 0)
                                            $elem.val(_value1);
                                    }
                                });
                            }
                            //Update comp reg details
                            $('#' + compId + " #vehicle-table #txt_register").val('');
                            $('#' + compId + " #vehicle-table #txt_fleet").val('');
                            if (_value.registrationDetails && _value.registrationDetails.length > 0) {
                                $('#' + compId + " .RegistrationComponent").find('tbody tr:not(:last)').remove();//To append localstorage values
                                $.each(_value.registrationDetails, function (_key1, _value1) {
                                    var table_len = $('#' + compId + " .RegistrationComponent tbody tr").length;
                                    $('#' + compId + " .RegistrationComponent").find('tbody tr:last').before("<tr id='row" + table_len + "' class='tr_Registration'><td id='registerId" + table_len + "' class='cls_regId txt_register tblregcomponent'>" + _value1.RegNo + "</td><td id='fleetId" + table_len + "' class='cls_fleetId txt_fleet tblregcomponent'>" + _value1.FleetNo + "</td><td><a href='javascript:void(0)' RowId=" + table_len + " class='delete btngrad btnrds btnbdr tdbutton deleteCompRegNew'></a ></td></tr>");
                                });
                            }

                        }).promise().done(function () {
                            MovementAssessment(false, false, function () {
                                $('.component-item-axle-section #Number_of_Axles').each(function (index) {
                                    $(this).trigger('blur');
                                });

                                $('.component-item-axle-section:last #Weight').trigger('keyup');//Reset Gross weight 
                                $('.component-item-axle-section:last #Weight').trigger('blur');//Reset Gross weight
                                $('#div_config_general #Internal_Name').trigger('blur');//add name for newly added component

                            });

                        });;
                    }
                } else {
                    vehicleDataObj = undefined;

                }
            });
        }

      
    }
}

function VehicleRegistartionPage(callbackfn) {
    var vehicleConfigId = 0;
    if ($('#vehicleConfigId').val() != undefined) {
        vehicleConfigId = $('#vehicleConfigId').val();
    }
    else if ($('#vehicleId').val() != undefined && $('#vehicleId').val() != "") {
        vehicleConfigId = $('#vehicleId').val();
    }
    var isMovement = false;
    if ($('#IsMovement').val() == "True" || $('#IsMovement').val() == "true")
        isMovement = true;
    var isCandidateVehicle = false;
    if ($('#IsCandVersion').val() == "true" || $('#IsCandVersion').val() == "True") {
        isCandidateVehicle = true;
    }
    $.ajax({
        url: '../VehicleConfig/ConfigurationRegistration',
        data: '{"vehicleId":' + vehicleConfigId + ',"RegBtn":' + true + ',"isVR1":' + false + ',"planMovement":' + isMovement + ',"isEditVehicleInSoProcessing":' + isCandidateVehicle + ',"vehicleResetOnEdit":' + vehicleResetOnEdit +'}',
        type: 'POST',
        //contentType: 'application/json; charset=utf-8',
        async: false,
        success: function (data) {
            FleetStepFlag = 2;
            if (data) {
                $('#config_registration_section').html(data);
                $('#config_registration_section').show();
            }
            if (callbackfn != undefined && callbackfn != null && callbackfn != '') {
                callbackfn();
            }
        },
        complete: function (data) {
            var countrw = 0;
            if (vehicleDataLocalStorageExist==false) {
                $(".comp").each(function () {
                    var compId = $(this).attr('id');
                    var Tractor = $("#" + compId).find('#Tractor').val();
                    if (Tractor.toLowerCase() != "false") {
                        var configTable = $("#vehicle-reg-table");
                        var configRegCount = $('#div_reg_config_vehicle .tr_config_Registration').not(':last').length;
                        if (configRegCount > 0) {
                            var compTable = $("#" + compId + " .div_reg_component_vehicle");
                            var compRegs = $("#" + compId + " .div_reg_component_vehicle .tr_Registration").not(':last');
                            var configRegs = $("#vehicle-reg-table .tr_config_Registration").not(':last');
                            if (compRegs.length == 0) {
                                $(configRegs).each(function () {
                                    countrw++;
                                    var regId = $(this).find('.txt_register_config').text();
                                    var fleetId = $(this).find('.txt_fleet_config').text();
                                    var HTMLTr = "<tr id='row" + countrw + "' class='tr_Registration'><td id='registerId" + countrw + "' class='cls_regId txt_register  tblregcomponent'>" + regId + "</td><td id='fleetId" + countrw + "' class='cls_fleetId txt_fleet tblregcomponent'>" + fleetId + "</td><td><a href='javascript:void(0)' RowId=" + countrw + " class='delete btngrad btnrds btnbdr tdbutton deleteCompRegNew'></a ></td></tr>";
                                    compTable.find('tr:last').before(HTMLTr)
                                });
                            }
                        }
                        //if (configRegCount == 0) {
                        //    var compRegs = $("#" + compId + " .div_reg_component_vehicle .tr_Registration").not(':last');
                        //    $(compRegs).each(function () {
                        //        countrw++;
                        //        var regId = $(this).find('.txt_register').text();
                        //        var fleetId = $(this).find('.txt_fleet').text();
                        //        var HTMLTr = "<tr id='row" + countrw + "' class='tr_config_Registration'><td id='regId" + countrw + "' class='cls_regId txt_register_config  tblregcomponent'>" + regId + "</td><td id='fleetId" + countrw + "' class='cls_fleetId txt_fleet_config tblregcomponent'>" + fleetId + "</td><td><a href='javascript:void(0)' RowId=" + countrw + " class='delete btngrad btnrds btnbdr tdbutton deleteConfigRow'></a ></td></tr>";
                        //        configTable.find('tr:last').before(HTMLTr)
                        //    });
                        //}
                    }
                });
            }
            vehicleDataLocalStorageExist = false;
        },
        error: function (data) {
            alert(data);
        }
    });
}
function SaveVehicleConfigurationV1(_this) {
    var isCandidate = false;
    var isFleet = false;
    if ($('#IsCandVersion').val() == "true" || $('#IsCandVersion').val() == "True") {
        isCandidate = true;
    }
    var planMovement = false;
    if ($('#IsMovement').val() == "True" || $('#IsMovement').val() == "true" || $('#isMovement').val() == "true") {
        planMovement = true;
    }
    if (!isCandidate && !planMovement) {
        isFleet = true;
    }
    if (validationConfigV1()) {
        var isComponentValid = ValidateVehicleComponents();
        if (isComponentValid) {
            if ($('#MovementTypeId').val() == "270154" || $('#MovementTypeId').val() == "270101" && !isFleet) {
                if ($("#hf_IsSortApp").val() == "True" || ($('#IsCandVersion').val() == "true" || $('#IsCandVersion').val() == "True") ) {
                    msg = "Based on the information entered, your vehicle is not categorised as a SO.";
                }
                else {
                    msg = "Based on the information entered your vehicle does not meet the requirements for a notification. However, further details may be required for a more accurate assessment. If you are unsure, please complete the remaining details on the form or contact the helpdesk on 0300 470 3733";
                }
                ShowErrorPopup(msg, 'CloseErrorPopup');
            }
            else {
                var Isvalid = true;
                ComponentsAutoFillOnEdit(true);
                if (!compFieldUpdatedOnSave)
                    Isvalid = ConfigAutoFillOnEdit(true);
                var isAxleWeightValid = ValidateMaxAxleWeightOnSave();
                if (Isvalid && isAxleWeightValid) {
                    if (configFieldUpdatedOnSave) {
                        var popupMsg = "Configuration "+updatedConfigFieldName + " has been modified by your last change. Please confirm you are happy with this change before saving.";
                        ShowDialogWarningPop(popupMsg, 'CANCEL', 'SAVE', 'WarningCancelBtn', "validateAxleWeightWithConfigWeight", 1, 'info', "SaveConfigMovementConfirmation", _this);

                        configFieldUpdatedOnSave = false;
                        updatedConfigFieldName = "";
                    }
                    else if (compFieldUpdatedOnSave) {
                        var popupMsg = updatedCompFieldName + " has been modified by your last change. Please confirm you are happy with this change before saving.";
                        ShowDialogWarningPop(popupMsg, 'CANCEL', 'SAVE', 'WarningCancelBtn', "validateAxleWeightWithConfigWeight", 1, 'info', "SaveConfigMovementConfirmation", _this);

                        compFieldUpdatedOnSave = false;
                        updatedCompFieldName = "";
                    }
                    else {
                        validateAxleWeightWithConfigWeight("SaveConfigMovementConfirmation", _this);
                    }
                }
            }
        }
    }
}
function validateAxleWeightWithConfigWeight(callbackFn, params) {
    //WarningCancelBtn();
    CloseWarningPopupDialog();
    var totalAxleWeightComponent = 0;
    if ($('#VehicleConfigInfo #Weight').length > 0 && $(".comp").length >= 1) {
        var weightElem = $('#VehicleConfigInfo #Weight');
        if ($('#VehicleConfigInfo #TrainWeight').length > 0) {
            weightElem = $('#VehicleConfigInfo #TrainWeight');
        }
        $(".comp").each(function (index) {
            var compId = $(this).attr('id');
            $('#' + compId + ' .axle').find('.axleweight').each(function () {
                var axleWyt = $(this).val() != "" ? $(this).val() : 0;
                totalAxleWeightComponent += parseFloat(axleWyt);
            });
        }).promise().done(function () {
            var configWeight = parseFloat(weightElem.val() || 0);
            if (configWeight < totalAxleWeightComponent) {
                //Invalid
                showToastMessage({
                    message: "Configuration weight is less than total axle weight!",
                    type: "error"
                });
            } else {
                window[callbackFn](params);//Valid - call save fn
            }
        });
    } else {
        window[callbackFn](params);//Valid - call save fn
    }
}
function SaveConfigMovementConfirmation(_this, flag = false) {
    if (flag) {
        CloseWarningPopup();
        CloseWarningPopupDialog();
    }
    
    var vehicleConfigurationId = 0;
    if ($("#vehicleId").val() != undefined) {
        vehicleConfigurationId = $("#vehicleId").val();
    }
    if (vehicleConfigurationId == 0) {
        if ($('#MovementTypeId').val() != 1) {
            SaveConfigurationV1(_this);
        }
        else {
            ShowErrorPopup("Movement assessment is not completed", 'CloseErrorPopup');
        }
    }
    else {
        //method to update
        EditConfigurationV1(_this);
    }
}
//function to validate
function validationConfigOverallLength() {
    var isvalid = true;
    var configTypeId = $('#ConfigTypeId').val();
    $('#Length').closest('div').find('.error').text('');
    var length = $('#Length').val();
    var overallLength = $('#OverallLength').val();
    var vehicleSubType = $('#VehicleSubType').val();

    if (configTypeId == 244003 && vehicleSubType == 224013) {
        if (length != overallLength) {
            $('#Length').closest('div').find('.error').text('Rigid Length should be equal to Length');
            isvalid = false;
        }
    }
    return isvalid;
}
//function to validate data against the save
var $invalidFieldForFocus;
function validationConfigV1() {
    $invalidFieldForFocus = undefined;
    var isvalid = true;
    var isRegvalid = true;
    var movementId = $('#MovementClassConfig').val();

    $('#div_config_general input:text,textarea').each(function () {
        if ($(this).is(':visible')) {
            $(this).closest('div').find('.error').text('');

            var required = $(this).attr('isrequired');//is required field
            var type = $(this).attr('datatype');//datatype of the field
            var range = $(this).attr('range');//min and max range
            var field = $(this).attr('validationmsg');//textbox id/name field
            var displayName = $(this).attr('displayName');
            var text = $(this).val();//textbox value
            text = $.trim(text);

            var splitRange = ",";

            if (range != undefined) {
                splitRange = range.split(',');
            }
            var minval = splitRange[0];
            var maxval = splitRange[1];



            var tyreSpace = $('#div_config_general').find('#Tyre_Spacing').val();
            var overWidth = $('#div_config_general').find('#Width').val();


            var movementClass = $('#MovementClassification').val(); // to get the vehicle class 

            if ((required == 1 && (text == '' || text == '0')) || (field == "Number of Axles" && required == 1 && text == '0')) {
                $(this).closest('div').find('.error').text(displayName + ' is required');
                isvalid = false;
            }
            if (field != undefined) {
                if ((parseFloat(text) < parseFloat(minval) || parseFloat(text) > parseFloat(maxval)) && parseFloat(text) > 0) {
                    $(this).closest('div').find('.error').text(displayName + ' must be in a range between ' + minval + ' and ' + maxval);
                    isvalid = false;
                }
            }
            if (isvalid)
                isvalid = ComponentKeyUpValidation(this);

            if (!isvalid && $invalidFieldForFocus==undefined) {
                $invalidFieldForFocus = $(this);
            }
        }//-END check is visble
    });

    isRegvalid = validateVehicleRegTable();
    if (isvalid && !isRegvalid)
        isvalid = false;

    var isvalidLength = ValidateLengthConfigOnKeyUp();

    if (isvalid && !isvalidLength)
        isvalid = false;
    if (!isvalidLength && $invalidFieldForFocus == undefined) {
        $invalidFieldForFocus = $('#div_config_general input#Length');
    }

    if ($invalidFieldForFocus) {
        $('html, body').animate({
            scrollTop: ($invalidFieldForFocus.position().top)
        }, 1000);
    }
    return isvalid;
}

function ValidateLengthConfigOnKeyUp(isRigidValidationRequired=true) {
    var isValid = true;
    var OverallLengthElem = $('#div_config_general input#OverallLength');
    var RigidLengthElem = $('#div_config_general input#Length');
    if (OverallLengthElem.length > 0 && RigidLengthElem.length > 0) {
        var overallLength = $('#div_config_general input#OverallLength').val() || 0;
        var rigidLength = $('#div_config_general input#Length').val() || 0;
        if (parseFloat(overallLength) != 0 && parseFloat(overallLength) < parseFloat(rigidLength)) {
            $('#div_config_general input#Length').closest('div').find('.error').text('');
            isValid = isRigidValidationRequired ? false : isValid;
            $('#div_config_general input#Length').closest('div').find('.error').text('Rigid Length should be less than or equal to Overall Length');
        }
        else if (parseFloat(overallLength) != 0 && parseFloat(rigidLength) != 0){
            $('#div_config_general input#Length').closest('div').find('.error').text('');
        }
    }
    return isValid;
}

//validate the fleetid and registrationid
function validateConfigDataV1(_this) {

    var isvalid = true;
    var this_regId = $('#txt_register_config').val();
    var this_fleetId = $('#txt_fleet_config').val();

    var messageValidation = "";
    $('#div_reg_config_vehicle').find('.cls_regId,.cls_regId_config').each(function () {
        var prv_reg_txt = $(this).text();
        if (prv_reg_txt == this_regId && this_regId != '') {
            messageValidation += "Vehicle Registration already in use";
            isvalid = false;
        }
    });

    if (this_fleetId != '') {
        $('#div_reg_config_vehicle').find('.cls_fleetId,.cls_fleetId_config').each(function () {
            var prv_fleet_txt = $(this).text();
            if (prv_fleet_txt == this_fleetId) {
                if (messageValidation != '')
                    messageValidation+=', '
                messageValidation += "Fleet ID already in use";
                isvalid = false;
            }
        });
    }

    if (!isvalid) //If invalid, show error message
        $('#div_reg_config_vehicle').find('#error_msg').text(messageValidation);
    
    return isvalid;
}
//function to Save configuration
function SaveConfigurationV1(_this) {

    var movementId = $('#MovementTypeId').val() == undefined ? 0 : $('#MovementTypeId').val();
    var configTypeId = $('#ConfigTypeId').val() == undefined ? 0 : $('#ConfigTypeId').val();
    var speedUnit = $('#SpeedUnits').val();
    var guid = $('#GUID').val();

    if (speedUnit == undefined || speedUnit == '') {
        speedUnit = null;
    }
    var isMovement = false;
    if ($('#IsMovement').val() != undefined) {
        var isMovement = $('#IsMovement').val() == "False" ? false : true;
        if (isMovement == true) {
            ApplnMovementId = $('#MovementId').val();
            if (ApplnMovementId == undefined) {
                ApplnMovementId = $('#movementId').val();
            }
        }
    }
    var isCandidateVehicle = false;
    if ($('#IsCandVersion').val() == "true" || $('#IsCandVersion').val() == "True") {
        isCandidateVehicle = true;
    }
    var candrevisionid = 0;
    if ($('#LastCandRevisionId').val() != undefined) {
        candrevisionid = $('#LastCandRevisionId').val();
    }
    var configList = {
        moveClassification: { ClassificationId: movementId },
        VehicleConfigParamList: []
    };

    var axleCount = 0;
    var overAllLength = 0;
    var Length = 0;
    
    $('#VehicleConfigInfo input,#VehicleConfigInfo textarea').not(':hidden,:checkbox').each(function () {
        var name;
        if ($(this)[0].localName == "span") {
            name = $(this)[0].innerText;
        }
        else {
            name = $(this).val();
        }
        var model = $(this).attr("id");
        var datatype = $(this).attr("datatype");
        if ($('#UnitValue').val() == 692002) {
            if (IsLengthFields(this)) {
                name = ConverteFeetToMetre(name);
            }
        }
        if ((isMovement || isCandidateVehicle) && model == "Internal_Name") {
            configList.VehicleConfigParamList.push({ ParamModel: "Formal_Name", ParamValue: name, ParamType: datatype });
            configList.VehicleConfigParamList.push({ ParamModel: model, ParamValue: name, ParamType: datatype });
        }
        else if (model == "Number_of_Axles") {
            axleCount += parseInt(name != "" ? name : 0);
        }
        else {
            if (model == "OverallLength") {
                overAllLength = name != "" ? name : 0;
            }
            if (model == "Length") {
                Length = name != "" ? name : 0;
            }
            configList.VehicleConfigParamList.push({ ParamModel: model, ParamValue: name, ParamType: datatype });
        }
    });

    if (configTypeId == VEHICLE_CONFIGURATION_TYPE_CONFIG.BOAT_MAST_EXCEPTION) {
        var trailerWeight = GetTrailerWeghtForBoatMast();
        configList.VehicleConfigParamList.push({ ParamModel: "TrailerWeight", ParamValue: trailerWeight, ParamType: "float" });
    }
    var wb = $('#VehicleConfigInfo #Wheelbase').val();
    if ($('#VehicleConfigInfo #Wheelbase').length == 0)
        wb = configWheelbase;
    configList.VehicleConfigParamList.push({ ParamModel: "Number of Axles", ParamValue: axleCount, ParamType: "int" });
    configList.VehicleConfigParamList.push({ ParamModel: "Wheelbase", ParamValue: wb, ParamType: "float" });
    if (configTypeId == VEHICLE_CONFIGURATION_TYPE_CONFIG.SPMT || configTypeId == VEHICLE_CONFIGURATION_TYPE_CONFIG.CRANE) {
        configList.VehicleConfigParamList.push({ ParamModel: "Length", ParamValue: overAllLength, ParamType: "float" });
    }
    if (configTypeId == VEHICLE_CONFIGURATION_TYPE_CONFIG.RIGID) {
        configList.VehicleConfigParamList.push({ ParamModel: "OverallLength", ParamValue: Length, ParamType: "float" });
    }
    var registrationParams = [];

    $('.tbl_registration tbody .tr_config_Registration').each(function () {
        var regVal = $(this).find('.txt_register_config').val();
        if (regVal == "") { regVal = $(this).find('.txt_register_config').html(); }
        var fleetId = $(this).find('.txt_fleet_config').val();
        if (fleetId == "") { fleetId = $(this).find('.txt_fleet_config').html(); }
        if (fleetId != "" || regVal != "") {
            registrationParams.push({ RegistrationValue: regVal, FleetId: fleetId });
        }

    });

    var componentCount = $('#ComponentCount').val();
    if (componentCount == 1 || componentCount == "1") {
        var unitvalue = $('#UnitValue').val();
        var wb = 0;
        $('#tbl_Axle tbody tr').each(function () {
            var distanceToNxtAxl = $(this).find('.disttonext').val();

            //to check if distance tonext axle value is definedor not
            if (typeof distanceToNxtAxl !== "undefined" && distanceToNxtAxl != "") {
                if (unitvalue == 692002) {
                    distanceToNxtAxl = ConverteFeetToMetre(distanceToNxtAxl) != null ? ConverteFeetToMetre(distanceToNxtAxl) : 0;
                }
                wb = parseFloat(wb) + parseFloat(distanceToNxtAxl);
            }
        });
        for (var i = 0; i < configList.VehicleConfigParamList.length; i++) {
            if (configList.VehicleConfigParamList[i].ParamModel == "Wheelbase")
                configList.VehicleConfigParamList[i].ParamValue = wb == 0 ? configList.VehicleConfigParamList[i].ParamValue : wb;
        }
    }

    $.ajax({
        url: "../VehicleConfig/SaveConfiguration",
        //data: '{"vehicleConfigObj":' + JSON.stringify(configList) + ',"configTypeId":' + configTypeId + ',"movementId":' + movementId + ',"speedUnit":' + speedUnit + ',"registrationParams":' + JSON.stringify(registrationParams) + ',"isMovement":' + isMovement + ',"AplnMovemntId":' + ApplnMovementId + ',"isCandidate":' + isCandidateVehicle + ',"CandRevisionId":' + candrevisionid + ',"guId":' + JSON.stringify(guid) + '}',
        data: {vehicleConfigObj:configList,configTypeId: configTypeId,movementId:movementId ,speedUnit: speedUnit ,registrationParams:registrationParams,isMovement:isMovement ,AplnMovemntId: ApplnMovementId ,isCandidate: isCandidateVehicle ,CandRevisionId: candrevisionid ,guId:guid },
        type: 'POST',
        //contentType: 'application/json; charset=utf-8',
        async: false,
        beforeSend: function () {
            startAnimation();
        },
        success: function (data) {
            if (data.configId != 0) {
                FleetStepFlag = 3;
                vehicleConfigurationId = data.configId;
                ApplnMovementId = data.movementId;
                $('#vehicleId').val(vehicleConfigurationId);
                //VehicleOverviewPage(data.configId);
                SaveVehicle_Components();
            }
            else {
                if (isCandidateVehicle && data.candidateResult == 1) {
                    ShowErrorPopup("The vehicle cannot be added, as it is not of special order", 'CloseErrorPopup');
                }
                else {
                    $('.err_formalName').text('Name already exists');
                }
            }
        },
        error: function (data) {
        },
        complete: function () {
            stopAnimation();
        }
    });

}
function VehicleOverviewPage(vehicleId) {
    FleetStepFlag = 3;
    var guid = $('#GUID').val();
    var isMovement = false;
    if ($('#IsMovement').val() != undefined) {
        isMovement = $('#IsMovement').val() == "False" ? false : true;
    }
    var isCandidate = false;
    if ($('#IsCandVersion').val() == "true" || $('#IsCandVersion').val() == "True") {
        isCandidate = true;
    }
    $('#vehicleId').val(vehicleId);
    vehicleId = vehicleId == 0 ? $('#vehicleId').val() : vehicleId;
    var movementTypeId = $('#MovementTypeId').val();
    var vehicleConfigTypeId = $('#ConfigTypeId').val();
    var isEditMovement = false;
    if ($('#IsEditMovement').val() == "true" || $('#IsEditMovement').val() == "True") {
        isEditMovement = true;
    }
    var isEdit = $("#IsVehicleConfigEdit").val();
    LoadVehicleContentForAjaxCalls("GET", '../VehicleConfig/VehicleOverview', { vehicleId: vehicleId, Guid: guid, isMovement: isMovement, vehicleConfigId: vehicleConfigTypeId, isCandidate: isCandidate, movementTypeId: movementTypeId, isEditMovement: isEditMovement, isEdit: isEdit }, '#config_review_section');
    $('#vehicle_save_btn').show();
    $('#vehicle_movement_confirm_btn').hide();
    $('#movement_assessment_section').show();
}
function ValidateVehicleComponents() {
    configWheelbase = 0;
    var isNotify = $('#ISNotif').val();
    if (isNotify == 'True' || isNotify == 'true') {
        isVR1 = 'True';
    }
    $('#trailer_axlecount_error_msg').text('');
    $('#tractor_axlecount_error_msg').text('');
    
    var AxleCountValidate = true;
    axleweightMax = 0;
    var IsComponentValid = false;
    var IsAxleValid = false;
    var validateCount = 0;
    var isCompDivOpened = false;
    var cmpwyt = 0;
    var cmpwidth = 0;
    var drwabarCmpId = 0;
    var configTypeId = $('#ConfigTypeId').val();
    //$.each(compList, function (key, value) {
    $(".comp").each(function (index) {
        var compId = $(this).attr('id');
        if (configTypeId == VEHICLE_CONFIGURATION_TYPE_CONFIG.DRAWBAR_TRAILER || configTypeId == VEHICLE_CONFIGURATION_TYPE_CONFIG.DRAWBAR_TRAILER_3_TO_8) {
            var weight = $('#' + compId + ' #Weight').val();
            var width = $('#' + compId + ' #Width').val();
            if (parseFloat(weight) > parseFloat(cmpwyt)) {
                cmpwyt = weight;
                if (width >= cmpwidth) {
                    cmpwidth = width;
                    drwabarCmpId = compId;
                    calculateWheelBase(drwabarCmpId);
                }
            }
            else {
                if (width > cmpwidth) {
                    cmpwidth = width;
                    drwabarCmpId = compId;
                    calculateWheelBase(drwabarCmpId);
                }
            }
        }
        else {
            calculateWheelBase(compId);
        }
        IsComponentValid = validateComponentV1(compId);
        IsAxleValid = ValidateAxlesV1(compId);
        if (!IsComponentValid || !IsAxleValid) {
            stopAnimation();
            if ($('#chevlon-up-icon_' + compId).length > 0) {
                $('#' + compId).css('display', "block");
                $('#chevlon-up-icon_' + compId).css('display', "block");
                $('#chevlon-down-icon_' + compId).css('display', "none");
                $('#spnDetailStatus_' + compId).text("Hide Details");
                var container = $('div');
                var scrollTo = $(".error:not(:contains(' '))");

                $('html, body').animate({
                    scrollTop: scrollTo.offset().top - container.offset().top + container.scrollTop()
                });
            }

            if ($('#compHeader-' + compId).length > 0 && !isCompDivOpened) {
                $('.divComponentDataItem').removeClass('active');
                $('.component-item').hide();
                $('#compHeader-' + compId).addClass('active');
                $('#divComponentDataMain-' + compId).fadeIn();
                isCompDivOpened = true;
                var container = $('#compHeader-' + compId);
                var scrollTo = $(".error:not(:contains(' '))");

                $('html, body').animate({
                    scrollTop: (container.position().top + container.scrollTop())
                });
            }
            HeaderHeight(compId);
            validateCount++;
            //return false;
        }
    });
    var maxAxleValidate = true;
    var v_AxleWeight = $('#div_config_general').find("input[name='AxleWeight']").val();
    //if (parseFloat(v_AxleWeight) != 0 && parseFloat(axleweightMax) > parseFloat(v_AxleWeight)) {
    //    $('#axle_error_msg').text('Max.axle Weight should be less than or equal to ' + v_AxleWeight);
    //    maxAxleValidate = false;
    //}
    var isValid = false;
    if (IsComponentValid && IsAxleValid && maxAxleValidate && validateCount == 0 && AxleCountValidate) {
        isValid = true;
    }
    else {
        isValid = false;
    }

    if (isValid && typeof validatDataVehicleRegNoComponents != 'undefined' &&$('#ComponentCount').val() != "1")
        isValid = validatDataVehicleRegNoComponents();

    return isValid;
}

function validatDataVehicleRegNoComponents(_this) {
    var isvalid = true;
    $(".comp").each(function (index) {
        var compId = $(this).attr('id');
        var this_regId = $(this).find('#txt_register').val();
        var this_fleetId = $(this).find('#txt_fleet').val();

        var messageValidation = "";
        //if (this_regId == '' && this_fleetId == '') {
        //    var regId = $(this).find('.txt_register').html();
        //    var fleetId = $(this).find('.txt_fleet').html();
        //    if (regId == '' && fleetId == '') {
        //        messageValidation = "Please enter at least one registration plate or fleet id";
        //        isvalid = false;
        //    }
        //}
        $(this).find('.div_reg_component_vehicle').find('.cls_regId').each(function () {

            var prv_reg_txt = $(this).text();
            if (prv_reg_txt == this_regId && this_regId != '') {
                messageValidation += "Vehicle Registration already in use";
                isvalid = false;
            }
        });

        if (this_fleetId != '') {
            $(this).find('.div_reg_component_vehicle').find('.cls_fleetId').each(function () {
                var prv_fleet_txt = $(this).text();
                if (prv_fleet_txt == this_fleetId) {
                    if (messageValidation != '')
                        messageValidation += ', '
                    messageValidation += "Fleet ID already in use";
                    isvalid = false;
                }


            });
        }
        if (!isvalid) //If invalid, show error message
            $(this).find('.div_reg_component_vehicle').find('#error_msg').text(messageValidation);
    });
    return isvalid;
}

function SaveVehicle_Components(isEdit) {
    startAnimation();
    //var isValid = ValidateVehicleComponents();
    //if (isValid) {
        GenerateVehicleConfiguration(isEdit);
    //}
    //else {
        stopAnimation();
    //}
}
function UpdateComponents() {
    CloseWarningPopup();
    CloseSuccessModalPopup();
    var guid = $('#GUID').val();
    var validateData = false;
    var isFromConfig = $('#IsFromConfig').val() == "" ? 0 : $('#IsFromConfig').val();

    var configId = $('#vehicleConfigId').val() == "" ? 0 : $('#vehicleConfigId').val();
    var vhclType = $('#vehicleTypeValue').val();
    var vhclIntend = $('#movementTypeId').val();
    var vehicleId = $('#vehicleId').val();
    if (vehicleId == undefined) {
        vehicleId = null;
    }

    var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
    var isVR1 = $('#vr1appln').val();
    var isNotify = $('#ISNotif').val();


    var isNotify = $('#ISNotif').val();
    var vehicleClassCode = $('#MovementClassification').val();


    var VehicleComponentDetail = GenerateComponentModel(2);
    //--------------------------------------
    var vehicleEdit = false;
    var planMovement = false;
    var isCandidate = false;
    if ($('#IsMovement').val() == "True" || $('#IsMovement').val() == "true" || $('#isMovement').val() == "true") {
        planMovement = true;
    }
    if ($('#IsCandidate').val() == "True" || $('#IsCandidate').val() == "true") {
        isCandidate = true;
    }
    if ($('#IsCandVersion').val() == "true" || $('#IsCandVersion').val() == "True") {
        isCandidate = true;
    }
    //WarningCancelBtn();
    var isEdit = $("#IsVehicleConfigEdit").val();
    $.ajax({
        url: '../VehicleConfig/SaveVehicleComponents',
        type: 'POST',
        //contentType: 'application/json; charset=utf-8',
        //async: false,
        data: { vehicleComponentDetail: VehicleComponentDetail, isMovement: planMovement, isCandidate: isCandidate, isEdit: isEdit },
        beforeSend: function () {
            startAnimation();
        },
        success: function (data) {
            if (data.result == 1) {
                var isMovement = false;
                if ($('#IsMovement').val() == "True" || $('#IsMovement').val() == "true") {
                    isMovement = true;
                }
                else if ($('#vehicle_Component_edit_section #IsMovement').val() == "True") {
                    isMovement = true;
                }
                if (isMovement&&isVehicleHasChanged)
                    localStorage.setItem("isVehicleHasChanges", true);//To check the vehcile edited during plan movement - top navigation back and forth
                var msg = "Vehicle saved successfully";
                if (isCandidate) {
                    ShowSuccessModalPopup(msg, "Show_CandidateRTVehicles");
                }
                else if (isMovement) {
                    var MovementId = $('#MovementId').val();
                    if (MovementId == "" || MovementId == "0") {
                        MovementId = ApplnMovementId;
                    }
                    if (isEdit == "1") {
                        ShowSuccessModalPopup(msg, "ReloadVehicleList", 2);
                    }
                    else {
                        var params1 = MovementId;
                        var params2 = vehicleId;
                        var cvVehicleClass = $('#cv_vehicleClass').val();
                        var cvMovementType = $('#cv_movementType').val();
                        var pmVehicleClass = $('#pm_vehicleClass').val();
                        var pmMovementType = $('#pm_movementType').val();
                        /*Handle HE-6862 changes*/
                        if ((pmVehicleClass != "" && pmMovementType != "")&&(cvVehicleClass != pmVehicleClass || cvMovementType != pmMovementType)) {
                            var vehicleClassDesc = GetVehicleClassErroDescription(pmVehicleClass, pmMovementType);
                            LoadAppVehicle(params1, params2, vehicleClassDesc);
                        }
                        else {
                            ShowDialogWarningPop(msg, 'Accept Vehicle(s) and Continue', 'Review and/or Add More Vehicles', "GoToMovementPage", "LoadAppVehicle", 1, 'info', params1, params2);
                        }
                    }
                }
                else {
                    ShowSuccessModalPopup(msg, "LoadVehicleConfigList");
                }
                saveVehicleButtonClicked = false;
                $('#chars_left').hide();
                //$('#HiddenFormalName').val(componentName);
            }
            else if (data.result == 2) {
                ShowErrorPopup("The vehicle cannot be added, as it is not of special order", 'CloseErrorPopup');
            }
            else {

                $('#chars_left').show();
                var IN = document.getElementById("Internal_Name");
                IN.focus();
                //$('.err_formalName').text('Name already exists');
            }
        },
        error: function (data) {
        },
        complete: function () {
            //$('#backbutton').show();
            //stopAnimation();
        }
    });
}
function GenerateComponentModel(flag=0) {// flag=1 no need to convert feet to metre
    var guid = $('#GUID').val();
    var vehicleId = $('#vehicleId').val();
    var movemnetTypeId = $('#MovementTypeId').val();
    var ConfigTypeId = $('#ConfigTypeId').val();
    var ComponentDetailList = [];
    var isCandidateVehicle = false;
    if ($('#IsCandidate').val() != undefined && $('#IsCandidate').val() != "false" && $('#IsCandidate').val() != "False") {
        isCandidateVehicle = true;
    }
    if ($('#IsCandVersion').val() == "true" || $('#IsCandVersion').val() == "True") {
        isCandidateVehicle = true;
    }

    var planMovement = false;
    if ($('#PlanMovement').val() == "True" || $('#isMovement').val() == "true" || $('#IsMovement').val() == "True" || $('#IsMovement').val() == "true") {
        planMovement = true;
    }
    var VehicleComponentDetail = {
        ConfigurationId: vehicleId,
        MovemnetTypeId: movemnetTypeId,
        ComponentDetailList: ComponentDetailList
    };
    $('.comp').each(function () {
        var VehicleParamList = [];
        var registrationParams = [];
        var axlesList = []; //Axle list
        var componentId = $(this).attr("id");
        var componentType = $('#' + componentId + ' #ComponentType_Id').val();
        var componentSubType = $('#' + componentId + ' #ComponentSubType_Id').val();

        $('#' + componentId).find('.dynamic input,textarea:not(hidden,checkbox)').each(function () {
            var model = $(this).attr("id");
            var name = $(this).val();
            var datatype = $(this).attr("datatype");
            var fieldName = $(this).attr("name");
            if (flag!=1&& $('#UnitValue').val() == 692002) {
                if (IsLengthField(this)) {
                    name = ConverteFeetToMetre(name);
                }
            }
            if ($('#ComponentCount').val() == "1") {
                if (fieldName == "Left Overhang" || fieldName == "Right Overhang" || fieldName == "Front Overhang" || fieldName == "Rear Overhang") {
                    if (name == "") {
                        name = $("#VehicleConfigInfo input[name='"+ fieldName + "']").val();
                    }
                }
            }
            if ((planMovement || isCandidateVehicle) && model == "Internal_Name") {
                VehicleParamList.push({ ParamModel: "Formal_Name", ParamValue: name, ParamType: datatype });
                VehicleParamList.push({ ParamModel: model, ParamValue: name, ParamType: datatype });
            }
            else if (model != "Number_of_Axles") {
                VehicleParamList.push({ ParamModel: model, ParamValue: name, ParamType: datatype });
            }
        });

        var numberOfAxles = $('#' + componentId + ' .axledrop').val();
        var configurableAxles = $('#' + componentId + ' .AxleConfig').val();
        var axleModel = $('#' + componentId + ' .axledrop').attr("id");
        var axleDatatype = "int";
        VehicleParamList.push({ ParamModel: axleModel, ParamValue: numberOfAxles, ParamType: axleDatatype });
        var couplingType = $('#Coupling').html();
        var couplingDatatype = "string";
        VehicleParamList.push({ ParamModel: 'Coupling', ParamValue: couplingType, ParamType: couplingDatatype });

        $('#' + componentId + ' .dynamic input:checkbox').each(function () {

            var name = $(this).val();
            var model = $(this).attr("id");
            var value = "false";

            if ($(this).is(':checked')) {
                value = "true";
            }
            var datatype = "bool";

            VehicleParamList.push({ ParamModel: model, ParamValue: value, ParamType: datatype });
        });


        //var comp_registerId = $(this).find("#txt_register").val();
        //var comp_fleetId = $(this).find("#txt_fleet").val();
        //if (comp_registerId != '' || comp_fleetId != '') {
        //    var elemid = $(this).find("table.RegistrationComponent").data('elemid');
        //    add_row_component(this, elemid);
        //}

        var componentCount = $('#ComponentCount').val();
        if (componentCount == 1 || componentCount == "1") {
            $('.tbl_registration tbody .tr_config_Registration').each(function () {
                var regVal = $(this).find('.txt_register_config').val();
                if (regVal == "") { regVal = $(this).find('.txt_register_config').html(); }
                var fleetId = $(this).find('.txt_fleet_config').val();
                if (fleetId == "") { fleetId = $(this).find('.txt_fleet_config').html(); }
                if (fleetId != "" || regVal != "") {
                    registrationParams.push({ RegistrationValue: regVal, FleetId: fleetId });
                }

            });
        }
        else {
            $('#' + componentId).find('.tbl_registration tbody .tr_Registration').each(function () {
                var regVal = $(this).find('.txt_register').val();
                if (regVal == "") { regVal = $(this).find('.txt_register').html(); }
                var fleetId = $(this).find('.txt_fleet').val();
                if (fleetId == "") { fleetId = $(this).find('.txt_fleet').html(); }
                if (fleetId != "" || regVal != "") {
                    registrationParams.push({ RegistrationValue: regVal, FleetId: fleetId });
                }
            });
        }
        //--------------- Axle Details -----------------
        var isAxleValid = ValidateAxlesV1(componentId);
        if (flag == 1)
            isAxleValid = true;
        if (isAxleValid){
            var i = 1;
            var unitvalue = $('#UnitValue').val();
            var wb = 0;
            var axle_spacing_to_following = 0;
            $('#' + componentId).find('#tbl_Axle tbody tr').each(function () {
                var axleNum = i;
                var noOfWheels = $(this).find('.nowheels').val();
                var axleWeight = $(this).find('.axleweight').val();
                var distanceToNxtAxl = $(this).find('.disttonext').val();

                //to check if distance tonext axle value is definedor not
                if (typeof distanceToNxtAxl !== "undefined" && distanceToNxtAxl != "") {
                    if (unitvalue == 692002) {
                        distanceToNxtAxl = ConverteFeetToMetre(distanceToNxtAxl);
                    }
                    wb = parseFloat(wb) + parseFloat(distanceToNxtAxl);
                }

                var tyreSize = $(this).find('.tyresize').val();

                var tyreSpace = null;
                $(this).find('.cstable input:text').each(function () {
                    var _thistxt = $(this).val();
                    //if (_thistxt != undefined) {
                    //    if (unitvalue == 692002) {
                    //        _thistxt = ConverteFeetToMetre(_thistxt);
                    //    }
                    //}
                    if (tyreSpace != null) {
                        tyreSpace = tyreSpace + "," + _thistxt;
                    }
                    else {
                        tyreSpace = _thistxt;
                    }
                });
                if (noOfWheels != "" || flag == 1) {
                    axlesList.push({ AxleNumId: axleNum, NoOfWheels: noOfWheels, AxleWeight: axleWeight, DistanceToNextAxle: distanceToNxtAxl, TyreSize: tyreSize, TyreCenters: tyreSpace });
                }
                if (i == numberOfAxles)
                    axle_spacing_to_following = distanceToNxtAxl;
                i++;
            });
            var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
            var isVR1 = $('#vr1appln').val();
            var isNotify = $('#ISNotif').val();
            var validwb = true;
            var axlecount = $('#' + componentId + ' .axledrop').val();  //added for crane workflow changes
            if (isNotify == 'True' || isNotify == 'true') {
                var vc = $('#' + componentId + ' #VehicleClass').val();
                var catAwb = 1.5 * axlecount + 0.5 * (axlecount % 2);
                if (vc == 241006) {
                    if (parseFloat(wb) < parseFloat(catAwb)) {
                        $('#' + componentId + ' .error').text('Wheelbase should be greater than or equal to ' + catAwb);
                        validwb = false;
                    }
                }
                isVR1 = 'True';
            }

            if (componentCount == 1 || componentCount == "1") {
                for (var i = 0; i < VehicleParamList.length; i++) {
                    if (VehicleParamList[i].ParamModel == "Wheelbase")
                        VehicleParamList[i].ParamValue = wb == 0 ? VehicleParamList[i].ParamValue : wb;
                }
            }
            if (axle_spacing_to_following != undefined)
                VehicleParamList.push({ ParamModel: "SPACE_TO_FOLLOWING", ParamValue: axle_spacing_to_following, ParamType: "float"});

            ComponentDetailList.push({ ComponentId: componentId,ComponentTypeId: componentType, ComponentSubTypeId: componentSubType,  Guid: guid, vehicleComponent: { VehicleParamList: VehicleParamList }, ComponentAxleList: axlesList, ComponentRegistrationList: registrationParams });

        }
        else {
            ComponentDetailList.push({ ComponentId: componentId,ComponentTypeId: componentType, ComponentSubTypeId: componentSubType,  Guid: guid, vehicleComponent: { VehicleParamList: VehicleParamList }, ComponentAxleList: axlesList, ComponentRegistrationList: registrationParams });

            var container = $('div');
            var scrollTo = $(".error:not(:contains(' '))");

            $('html, body').animate({
                scrollTop: scrollTo.offset().top - container.offset().top + container.scrollTop()
            });
            
        }

    });

    

    if (flag != 2) {
        var componentCount = $('#ComponentCount').val();

        if (componentCount == 1 || componentCount == "1") {
            $('.comp').each(function () {
                var componentId = $(this).attr("id");
                var axlesList = [];
                //ComponentDetailList = [];
                var i = 1;
                var unitvalue = $('#UnitValue').val();
                var wb = 0;
                $('#' + componentId).find('#tbl_Axle tbody tr').each(function () {
                    var axleNum = i;
                    var noOfWheels = $(this).find('.nowheels').val();
                    var axleWeight = $(this).find('.axleweight').val();
                    var distanceToNxtAxl = $(this).find('.disttonext').val();

                    //to check if distance tonext axle value is definedor not
                    if (typeof distanceToNxtAxl !== "undefined" && distanceToNxtAxl != "") {
                        if (unitvalue == 692002) {
                            distanceToNxtAxl = ConverteFeetToMetre(distanceToNxtAxl);
                        }
                        wb = parseFloat(wb) + parseFloat(distanceToNxtAxl);
                    }

                    var tyreSize = $(this).find('.tyresize').val();

                    var tyreSpace = null;
                    $(this).find('.cstable input:text').each(function () {
                        var _thistxt = $(this).val();
                        //if (_thistxt != undefined) {
                        //    if (unitvalue == 692002) {
                        //        _thistxt = ConverteFeetToMetre(_thistxt);
                        //    }
                        //}
                        if (tyreSpace != null) {
                            tyreSpace = tyreSpace + "," + _thistxt;
                        }
                        else {
                            tyreSpace = _thistxt;
                        }
                    });

                    axlesList.push({ AxleNumId: axleNum, NoOfWheels: noOfWheels, AxleWeight: axleWeight, DistanceToNextAxle: distanceToNxtAxl, TyreSize: tyreSize, TyreCenters: tyreSpace });
                    i++;
                });
                ComponentDetailList[0].ComponentAxleList = axlesList;
                //ComponentDetailList.push({ ComponentId: componentId, Guid: guid, vehicleComponent: { VehicleParamList: [] }, ComponentAxleList: axlesList, ComponentRegistrationList: [] });
            });
        }
    }
    return VehicleComponentDetail;
}
//function to validate component data against the save
function validateComponentV1(componentId) {
    var componentCount = $('#ComponentCount').val();
    var isvalid = true;
    var movementId = $('#movementTypeId').val();
    var vc = $("#VehicleClass").val(); //added for crane workflow changes
    var isDetNotif = $('#DetailNotif').val();
    var cmp = "";
    var configTypeId = $('#ConfigTypeId').val();
    if (componentId != undefined) {
        cmp = "#" + componentId;
    }
    var vhclType = $(cmp).find('#vehicleTypeValue').val();
    var axlecount = $(cmp + ' .axledrop').val();
    $('' + cmp + ' .dynamic input:text,textarea').each(function () {
        //$(this).closest('div').find('.error').text('');

        var id = $(this).attr('id');
        var required = $(this).attr('isrequired');//is required field
        var type = $(this).attr('datatype');//datatype of the field
        var range = $(this).attr('range');//min and max range
        var field = $(this).attr('validationmsg');//textbox id/name fieldvar 
        var displayName = $(this).attr('displayName');
        if (field == "Rear Overhang") {
            field = "Projection Rear";
        }
        var text = $(this).val();//textbox value (str.name.value.trim() 
        text = $.trim(text);
        var text_inch_var = $(this).val();

        var unitValue = $('#UnitValue').val();
        var onlyInch = 0;

        //if (unitValue == 692002 && (field != 'Weight' && field != 'TravelSpeed')) {
        //    if (IsLengthFields(this)) {
        //        text = ConverteFeetToMetre(text);
        //    }
        //}

        // var splitRange = range.split(',');
        var splitRange = ",";

        if (range != undefined && range != "") {
            splitRange = range.split(',');
        }
        var minval = splitRange[0];
        var maxval = splitRange[1];


        if (required == 1 && text == '') {
            $(this).closest('div').find('.error').text(displayName + ' is required');
            isvalid = false;
        }
        else if (required == 1 && text < 0.5 && field == 'Axle Spacing To Following' && maxval != undefined) {
            $(this).closest('div').find('.error').text(field + ' must be in a range between ' + minval + ' and ' + maxval);
            isvalid = false;
        }
        if (movementId == 270006 && (parseFloat(text) == 0) && (field == 'Maximum_Height' || field == 'Length' || field == 'Width' || field == 'Reducable_Height' || field == 'Weight')) {
            $(this).closest('div').find('.error').text(field + ' must be greater than 0');
            isvalid = false;
        }
        if (componentCount == 1 && type != "string") {
            if (field == 'Length') {
                field = "OverallLength";
                var vehcielfield = $("#VehicleConfigInfo input[name='" + field + "']").val();
                if (text != "" && text != undefined && vehcielfield != "" && vehcielfield != undefined) {
                    if (parseFloat(text) != parseFloat(vehcielfield)) {
                        $(this).closest('div').find('.error').text(field + ' should be equal to ' + vehcielfield);
                        isvalid = false;
                    }
                }
            }
            else if (field == 'Weight') {
                if (text != "") {
                    var vehicleGW = $("#VehicleConfigInfo input[name='Weight']").val();
                    if (parseFloat(text) > parseFloat(vehicleGW)) {
                        $(this).closest('div').find('.error').text(field + ' should be less than or equal to ' + vehicleGW);
                        isvalid = false;
                    }
                    if (configTypeId == "244001" && vhclType == "234006") {
                        var heaviestGW = $("#VehicleConfigInfo input[name='HeaviestComponentWeight']").val();
                        if (parseFloat(text) != parseFloat(heaviestGW)) {
                            $(this).closest('div').find('.error').text(field + ' should be equal to ' + heaviestGW);
                            isvalid = false;
                        }
                    }
                }
            }
            else if (field == 'Length') {
                var vehiclelLength = $("#VehicleConfigInfo input[name='OverallLength']").val();
                if (parseFloat(text) > parseFloat(vehiclelLength)) {
                    $(this).closest('div').find('.error').text(field + ' should be less than or equal to ' + vehiclelLength);
                    isvalid = false;
                }
            }
            else if (field == 'Width') {
                var vehiclelWidth = $("#VehicleConfigInfo input[name='Width']").val();
                if (vehiclelWidth != "0") {
                    if (parseFloat(text) > parseFloat(vehiclelWidth)) {
                        $(this).closest('div').find('.error').text(field + ' should be less than or equal to ' + vehiclelWidth);
                        isvalid = false;
                    }
                }
            }
            else if (field == 'Maximum Height') {
                var vehiclelHeight = $("#VehicleConfigInfo input[name='Maximum Height']").val();
                if (parseFloat(text) > parseFloat(vehiclelHeight)) {
                    $(this).closest('div').find('.error').text(field + ' should be less than or equal to ' + vehiclelHeight);
                    isvalid = false;
                }
            }
            if (field != 'Wheelbase') {
                if (movementId != 270006 && (parseFloat(text) < parseFloat(minval) || parseFloat(text) > parseFloat(maxval)) && parseFloat(text) > 0) {
                    $(this).closest('div').find('.error').text(field + ' must be in a range between ' + minval + ' and ' + maxval);
                    $('.div_' + id).find('.error').text(field + ' must be in a range between ' + minval + ' and ' + maxval);
                    isvalid = false;
                }
            }
        }
        else if ((parseFloat(text) < parseFloat(minval) || parseFloat(text) > parseFloat(maxval)) && parseFloat(text) > 0 && field != 'Wheelbase') {
            $(this).closest('div').find('.error').text(field + ' must be in a range between ' + minval + ' and ' + maxval);
            isvalid = false;
        }
        if (isvalid) {
            isvalid = ComponentKeyUpValidation(this);
        }
        
        //if (movementId == 270003 && field == 'Weight') {

        //    //added for crane workflow changes
        //    var catAwt = ((9 * axlecount) + (4 - axlecount) + (2 * (axlecount % 2))) * 1000;
        //    var catBwt = 12.5 * axlecount * 1000;
        //    var catAwb = 1.5 * axlecount + 0.5 * (axlecount % 2);
        //    if (isDetNotif == "1") {
        //        minval = 12000;
        //        if (vc == 241006) {

        //            if ((parseFloat(text) > parseFloat(catAwt)) || (parseFloat(text) <= parseFloat(minval))) {
        //                $(this).closest('div').find('.error').text(field + ' must be in a range between ' + 12000 + ' and ' + catAwt);
        //                isvalid = false;
        //            }

        //        }
        //        else if (vc == 241007) {

        //            if ((parseFloat(text) > parseFloat(catBwt)) || (parseFloat(text) <= parseFloat(minval)) || (parseFloat(text) > 150000)) {
        //                if (axlecount > 12) {
        //                    $(this).closest('div').find('.error').text(field + ' must be in a range between ' + 12000 + ' and ' + 150000);
        //                    isvalid = false;
        //                }
        //                else {
        //                    $(this).closest('div').find('.error').text(field + ' must be in a range between ' + 12000 + ' and ' + catBwt);
        //                    isvalid = false;
        //                }
        //            }
        //        }
        //        else if (vc == 241008) {
        //            if ((parseFloat(text) > 150000) || (parseFloat(text) <= parseFloat(minval))) {
        //                $(this).closest('div').find('.error').text(field + ' must be in a range between ' + 12000 + ' and ' + 150000);
        //                isvalid = false;
        //            }
        //        }
        //    }
        //}

        //if (movementId == 270001 && (field == 'Length' || field == 'Width')) {
        //    if (field == 'Length' && unitValue == 692001) {
        //        maxval = 27.4;
        //    }
        //    if (field == 'Length' && unitValue == 692002) {
        //        maxval = 89 + "'" + 9 + '"';
        //    }
        //    if (parseFloat(text) < parseFloat(minval) || parseFloat(text) > parseFloat(maxval)) {
        //        $(this).closest('div').find('.error').text(field + ' must be in a range between ' + minval + ' and ' + maxval);
        //        isvalid = false;
        //    }
        //    if (field == 'Width' && unitValue == 692002) {//#5950 Too many inches
        //        //if (text_inch_var.search('\'') != -1) {

        //        if (isDetNotif == "1") {
        //            var vhclWidth = $(this).val();
        //            vhclWidth = vhclWidth.replace("\'", '.');
        //            vhclWidth = vhclWidth.replace("\"", '');
        //            var vhclMaxWidth = maxval;
        //            vhclMaxWidth = vhclMaxWidth.replace("\'", '.');
        //            vhclMaxWidth = vhclMaxWidth.replace("\"", '');
        //            var arr_width = [];
        //            arr_width = vhclWidth.split('.');
        //            if (arr_width[1] > 11) {
        //                $(this).closest('div').find('.error').text(field + ' inches should be less than or equal to ' + '11' + '\"');
        //                isvalid = false;
        //            }
        //            if (vhclWidth > vhclMaxWidth) {
        //                $(this).closest('div').find('.error').text(field + ' must be less than ' + maxval);
        //                isvalid = false;
        //            }
        //        }
        //    }
        //}
        //else if (movementId == 270006 && (parseFloat(text) == 0) && (field == 'Maximum_Height' || field == 'Length' || field == 'Width' || field == 'Reducable_Height' || field == 'Weight')) {
        //    $(this).closest('div').find('.error').text(field + ' must be greater than 0');
        //    isvalid = false;
        //}
        //else if (movementId == 270002 && field == 'Weight') {
        //    if (vc == 241003 || vc == 241006) {
        //        maxval = 50000;
        //        if (parseFloat(text) < parseFloat(minval) || parseFloat(text) > parseFloat(maxval)) {
        //            $(this).closest('div').find('.error').text(field + ' must be in a range between ' + minval + ' and ' + maxval);
        //            isvalid = false;
        //        }
        //    }
        //    else if (vc == 241004 || vc == 241007) {
        //        maxval = 80000;
        //        if (parseFloat(text) < parseFloat(minval) || parseFloat(text) > parseFloat(maxval)) {
        //            $(this).closest('div').find('.error').text(field + ' must be in a range between ' + minval + ' and ' + maxval);
        //            isvalid = false;
        //        }
        //    }
        //    else {
        //        if (parseFloat(text) < parseFloat(minval) || parseFloat(text) > parseFloat(maxval)) {
        //            $(this).closest('div').find('.error').text(field + ' must be in a range between ' + minval + ' and ' + maxval);
        //            isvalid = false;
        //        }
        //    }
        //}
        //else if (movementId != 270006 && (parseFloat(text) < parseFloat(minval) || parseFloat(text) > parseFloat(maxval)) && parseFloat(text) > 0) {
        //    $(this).closest('div').find('.error').text(field + ' must be in a range between ' + minval + ' and ' + maxval);
        //    isvalid = false;
        //}



    });

    var wheelbaseval = $('#VehicleConfigInfo').find('#Wheelbase').val();
    var loadLength = $('#VehicleConfigInfo').find('#OverallLength').val();
    if (loadLength == undefined) {
        loadLength = $('#VehicleConfigInfo').find('#Length').val();
    }
    //Wheelbase should be less than length   mirza

    if (parseFloat(loadLength) < parseFloat(wheelbaseval)) {
        $('#wheelbase_error_msg').text('Wheelbase should be less than the length');
        isvalid = false;
    }


    var catAwb = 1.5 * axlecount + 0.5 * (axlecount % 2);
    if (isDetNotif == "1") {
        if (vc == 241006) {
            if (parseFloat(wheelbaseval) < parseFloat(catAwb)) {
                $(_this).closest('div').find('.error').text('Wheelbase should be greater than or equal to ' + catAwb);
                isvalid = false;
            }
            if (axlecount >= 5) {
                $('.axledrop').closest('div').find('.error').text('Axle count must be less than 5');
                isvalid = false;
            }
        }

    }

    if ($('' + cmp + ' #IsConfigAxle').val() == 'True') {
        if ($(cmp + ' .axledrop').val() == "Select") {
            $(cmp + ' .axledrop').closest('div').find('.error').text('Please select the number of axles');
            isvalid = false;
        }
    }
    return isvalid;
}
function ValidateAxlesV1(componentId) {
    var isvalid = true;
    var unitvalue = $('#UnitValue').val();
    var movementId = $('#movementTypeId').val();
    var wb = 0;
    $('#' + componentId + ' #tbl_Axle tbody').find('input:text').each(function () {
        var this_txt = $(this).val();
        var type = $(this).attr('datatype');//datatype of the field
        var range = $(this).attr('range');//min and max range
        var field = $(this).attr('validationmsg');//textbox id/name field
        var name = $(this).attr('name');//name

        var minval = '';
        var maxval = '';
        var minvalfeet = '';
        var maxvalfeet = '';
        var valuemetres;

        if (range != undefined) {
            var splitRange = range.split(',');
            if (splitRange.length > 1) {
                minval = splitRange[0];
                maxval = splitRange[1];
            }

        }
        var configurableAxles = $('#' + componentId).find('#div_component_general_page').find('.AxleConfig').val();
        var isTyreSizeRequired = $('#' + componentId).find('#IsTyreSizeRequired').val();
        var isTyreCentreSpacingRequired = $('#' + componentId).find('#IsTyreCentreSpacingRequired').val();
        if (configurableAxles == 'True') {
            if (parseFloat(this_txt) < parseFloat(minval) || parseFloat(this_txt) > parseFloat(maxval)) {
                $('#' + componentId + ' .axlewrapper').find('.error').text(field + ' must be in a range between ' + minval + ' and ' + maxval);
                isvalid = false;
                return false;
            }
            if ((this_txt == '' || parseFloat(this_txt) == 0) && (name.charAt(0) != 'q' && name != 'tyresize')
                && isTyreSizeRequired != "True" && isTyreSizeRequired != "true"
                && isTyreCentreSpacingRequired != "True" && isTyreCentreSpacingRequired != "true") {
                isvalid = false;
                $('#' + componentId + ' .axlewrapper').find('.error').text("All fields except tyre size and centre spacing are required");
                return false;
            }
            else if ((this_txt == '' || parseFloat(this_txt) == 0) && (isTyreSizeRequired == "True" || isTyreSizeRequired == "true")
                && (isTyreCentreSpacingRequired == "True" || isTyreCentreSpacingRequired == "true")) {
                isvalid = false;
                $('#' + componentId + ' .axlewrapper').find('.error').text("All fields are required");
                return false;
            }
        }
        if (name == "distancetoaxle" && this_txt != undefined) {
            var distanceToNxtAxl = this_txt;
            if (unitvalue == 692002) {
                distanceToNxtAxl = ConverteFeetToMetre(distanceToNxtAxl);
            }
            wb = parseFloat(wb) + parseFloat(distanceToNxtAxl);
        }
    });
    var componentCount = $('#ComponentCount').val();
    //if (componentCount == 1 || componentCount == "1") {
    //    var configWb = $('#VehicleConfigInfo').find('#Wheelbase').val() != "" ? parseFloat($('#VehicleConfigInfo').find('#Wheelbase').val()) : 0;
    //    if (wb != 0 && parseFloat(configWb.toFixed(2)) > parseFloat(wb.toFixed(2))) {
    //        $('#VehicleConfigInfo #Wheelbase').closest('div').find('.error').text("Wheelbase should not be greater than the sum of the distances to next axle");
    //        isvalid = false;
    //    }
    //}

    if (isvalid) {
        AxleValidationCalculationV1($('#' + componentId).find('#Weight').val(), componentId);
        var axleweightsum = 0;
        var axleWeightArray = [];
        var axleweightMaxval = 0;
        $('#' + componentId + ' #Config-body').find('.axleweight').each(function () {
            var _thisVal = $(this).val();
            axleweightsum = axleweightsum + parseFloat(_thisVal);
        });

        if (axleweightsum == 0) {
            $('#' + componentId + ' #tbl_Axle').find('.axleweight').each(function () {
                var _thisVal = $(this).val();
                axleweightsum = axleweightsum + parseFloat(_thisVal);
                axleWeightArray.push(parseFloat(_thisVal));
            });
        }

        axleweightMaxval = Math.max.apply(Math, axleWeightArray);
        if (parseFloat(axleweightMax) < parseFloat(axleweightMaxval)) {
            axleweightMax = axleweightMaxval;
        }

        isvalid = checkaxle_weightV1(axleweightsum, componentId);

    }
    return isvalid;
}
function checkaxle_weightV1(x, compId = 0) {
    var result = true;
    if (x != 0) {
        var range;
        if (compId != 0)
            range = $('#' + compId).find('#axleweight_range').val();
        else
            range = $('#axleweightRange').val();
        var splitRange = range.split(',');
        var minval = splitRange[0];
        var maxval = splitRange[1];
        if (parseFloat(x) < parseFloat(minval) || parseFloat(x) > parseFloat(maxval)) {
            var msg = 'Total axle weight must be in a tolerance range between ' + minval + ' and ' + maxval;
            if (compId != 0)
                $('#' + compId).find('.axlewrapper').find('.error').text(msg);
            else
                $('.axlewrapper').find('.error').text(msg);
            result = false;
        }
    }
    return result;
}
function AxleValidationCalculationV1(weight, compId) {

    if (weight == "") {
        weight = null;
    }
    $.ajax({
        url: '../Vehicle/AxleValidationCalculation',
        type: 'POST',
        async: false,
        data: { "weight": weight },
        success: function (response) {
            $('#' + compId).find('#axleweight_range').val(response);
        },
        error: function (response) {
        }
    });
}
function EditConfigurationV1(_this) {
    //var internalName = $('#Internal_Name').val();
    //$('#Formal_Name').val(internalName);
    var movementId = $('#MovementTypeId').val() == undefined ? 0 : $('#MovementTypeId').val();
    var configTypeId = parseInt($('#ConfigTypeId').val());
    var vehicleId = parseInt($('#vehicleId').val());
    var speedUnit = $('#SpeedUnits').val();

    if (speedUnit == undefined || speedUnit == '') {
        speedUnit = null;
    }
    var isCandidateVehicle = false;
    if ($('#IsCandidate').val() != undefined && $('#IsCandidate').val() != "false" && $('#IsCandidate').val() != "False") {
        isCandidateVehicle = true;
    }
    if ($('#IsCandVersion').val() == "true" || $('#IsCandVersion').val() == "True") {
        isCandidateVehicle = true;
    }

    var planMovement = false;
    if ($('#PlanMovement').val() == "True" || $('#isMovement').val() == "true" || $('#IsMovement').val() == "True" || $('#IsMovement').val() == "true") {
        planMovement = true;
    }

    var configList = {
        moveClassification: { ClassificationId: movementId },
        //vehicleConfigType: { ConfigurationTypeId: configTypeId },
        VehicleConfigParamList: []
    };

    var axleCount = 0;
    var overAllLength = 0;
    var Length = 0;
    // $('.div_config_general input:text,textarea').each(function () {
    $('#div_config_general input,#div_config_general textarea,#div_config_general span.lblParam').not(':checkbox').each(function () {
        var name;
        if ($(this)[0].localName == "span") {
            name = $(this)[0].innerText;
        }
        else {
            name = $(this).val();
        }
        var model = $(this).attr("id");
        var datatype = $(this).attr("datatype");
        if ($('#UnitValue').val() == 692002) {
            if (IsLengthFields(this)) {
                name = ConverteFeetToMetre(name);
            }
        }
        if ((planMovement || isCandidateVehicle) && model == "Internal_Name") {
            configList.VehicleConfigParamList.push({ ParamModel: "Formal_Name", ParamValue: name, ParamType: datatype });
            configList.VehicleConfigParamList.push({ ParamModel: model, ParamValue: name, ParamType: datatype });
        }
        else if (model == "Number_of_Axles") {
            axleCount += parseInt(name != "" ? name : 0);
        }
        else {
            if (model == "OverallLength") {
                overAllLength = name != "" ? name : 0;
            }
            if (model == "Length") {
                Length = name != "" ? name : 0;
            }
            configList.VehicleConfigParamList.push({ ParamModel: model, ParamValue: name, ParamType: datatype });
        }
    });
    if (configTypeId == VEHICLE_CONFIGURATION_TYPE_CONFIG.SPMT || configTypeId == VEHICLE_CONFIGURATION_TYPE_CONFIG.CRANE) {
        configList.VehicleConfigParamList.push({ ParamModel: "Length", ParamValue: overAllLength, ParamType: "float" });
    }
    if (configTypeId == VEHICLE_CONFIGURATION_TYPE_CONFIG.RIGID) {
        configList.VehicleConfigParamList.push({ ParamModel: "OverallLength", ParamValue: Length, ParamType: "float" });
    }

    var wb = $('#VehicleConfigInfo #Wheelbase').val();
    configList.VehicleConfigParamList.push({ ParamModel: "Wheelbase", ParamValue: wb, ParamType: "float" });

    var registrationParams = [];
    $('.tbl_registration tbody .tr_config_Registration').each(function () {

        var regVal = $(this).find('.txt_register_config').val();
        if (regVal == "") { regVal = $(this).find('.txt_register_config').html(); }
        var fleetId = $(this).find('.txt_fleet_config').val();
        if (fleetId == "") { fleetId = $(this).find('.txt_fleet_config').html(); }
        if (fleetId != "" || regVal != "") {
            registrationParams.push({ RegistrationValue: regVal, FleetId: fleetId });
        }

    });

    var componentCount = $('#ComponentCount').val();
    if (componentCount == 1 || componentCount == "1") {
        var unitvalue = $('#UnitValue').val();
        var wb = 0;
        $('#tbl_Axle tbody tr').each(function () {
            var distanceToNxtAxl = $(this).find('.disttonext').val();

            //to check if distance tonext axle value is definedor not
            if (typeof distanceToNxtAxl !== "undefined" && distanceToNxtAxl != "") {
                if (unitvalue == 692002) {
                    distanceToNxtAxl = ConverteFeetToMetre(distanceToNxtAxl);
                }
                wb = parseFloat(wb) + parseFloat(distanceToNxtAxl);
            }
        });
        for (var i = 0; i < configList.VehicleConfigParamList.length; i++) {
            if (configList.VehicleConfigParamList[i].ParamModel == "Wheelbase")
                configList.VehicleConfigParamList[i].ParamValue = wb == 0 ? configList.VehicleConfigParamList[i].ParamValue : wb;
        }
    }

    $.ajax({
        url: "../VehicleConfig/UpdateVehicleConfiguration",
        //data: '{"vehicleConfigObj":' + JSON.stringify(configList) + ',"configTypeId":' + configTypeId + ',"vehicleId":' + vehicleId + ',"speedUnit":' + speedUnit + ',"registrationParams":' + JSON.stringify(registrationParams) + ',"planMovement":' + planMovement + ',"isCandidate":' + isCandidateVehicle + '}',
        data: { vehicleConfigObj:  configList, configTypeId: configTypeId , vehicleId:  vehicleId, speedUnit:  speedUnit ,registrationParams: registrationParams , planMovement: planMovement , isCandidate:  isCandidateVehicle },
        type: 'POST',
        //contentType: 'application/json; charset=utf-8',
        async: false,
        beforeSend: function () {
            startAnimation();
        },
        success: function (data) {

            if (data.Success) {
                //VehicleOverviewPage(vehicleId);  
                $('#vehicleId').val(vehicleId);
                SaveVehicle_Components(2);
            }
            else {

                $('.err_formalName').text('Name already exists');
                //formal name
                ShowErrorPopup("Vehicle Updation failed");
            }
        },
        error: function (data) {
            //location.reload();
        },
        complete: function () {
            stopAnimation();
            $('.popoverlay').hide();
            $('.config_body').show();
        }
    });
}
function LoadVehicleConfigList() {
    window.location = "/VehicleConfig/VehicleConfigList";
}
//function to prevent entering non-numeric characters
function suppressKey() {
    $('.dynamic input:text').each(function () {
        $(this).keypress(function (evt) {
            $(this).closest('div').find('.error').text('');
            var type = $(this).attr('datatype');//datatype of the field
            var text = $(this).val();//textbox value

            if (!IsPreference()) {

                if (type == 'int') {
                    //this.value = this.value.replace(/[^0-9\.]/g, '');
                    var charCode = (evt.which) ? evt.which : event.keyCode;
                    return !(charCode > 31 && (charCode < 48 || charCode > 57));
                }
                else if (type == 'float') {
                    //this.value = this.value.replace(/[^0-9\.]/g, '');
                    var charCode = (evt.which) ? evt.which : event.keyCode;
                    return !(charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57));
                }
            }
            else {
                if (type != 'string') {
                    var charCode = (evt.which) ? evt.which : event.keyCode;
                    return !((charCode != 39 && charCode != 34) && charCode > 31 && (charCode < 48 || charCode > 57));
                }
            }
        });
        $(this).on("paste", function (e) {
            $(this).closest('div').find('.error').text('');
            var type = $(this).attr('datatype');//datatype of the field
            var text = $(this).val();//textbox value

            if (!IsPreference()) {

                if (type == 'int') {
                    if (e.originalEvent.clipboardData.getData('Text').match(/[^\d]/)) {
                        event.preventDefault();
                    }
                }
                else if (type == 'float') {
                    if (e.originalEvent.clipboardData.getData('Text').match(/[^\d]/)) {
                        event.preventDefault();
                    }
                }
            }
        });
        var original = '';
        $(this).on('input', function () {
            if ($(this).attr("id") != "Internal_Name") {
                if ($(this).val().replace(/[^.]/g, "").length > 1) {
                    $(this).val(original);
                } else {
                    original = $(this).val();
                }
            }
        });
    });
}

var isAssessmentApiCalled = false;
function MovementAssessment(needConfirm = false, importedComponent = false, callbackfn=undefined) {
    $('#error_msg').text('');
    if (isAssessmentApiCalled) {
        return false;
    }
    var assessmentResult = 0;
    var searchFlag = false;
    var isCandidate = false;
    var isFleet = false;
    var noAxle = 0;
    if ($('#IsCandVersion').val() == "true" || $('#IsCandVersion').val() == "True") {
        isCandidate = true;
    }
    var planMovement = false;
    if ($('#IsMovement').val() == "True" || $('#IsMovement').val() == "true" || $('#isMovement').val() == "true") {
        planMovement = true;
    }
    if (!isCandidate && !planMovement) {
        isFleet = true;
    }
    $("input[type=text]").each(function () {
        if ($(this).val()) {
            searchFlag = true;
        }
    });
    var VSONotificationVehicle = false;
    if ($('#NotiVSO').val()!='') {
        VSONotificationVehicle = true;
    }
    var vehicleConfigurationId = 0;
    if ($("#vehicleId").val() != undefined && $("#vehicleId").val() != '') {
        vehicleConfigurationId = $("#vehicleId").val();
    }
    var configId = $('#ConfigTypeId').val();
    var configList = {
        VehicleConfigParamList: []
    };
    if (searchFlag) {
        var axleCount = 0;
        var overAllLength = 0;
        $('#VehicleConfigInfo input').not(':hidden,:checkbox').each(function () {
            var name;
            if ($(this)[0].localName == "span") {
                name = $(this)[0].innerText;
            }
            else {
                name = $(this).val();
            }
            var model = $(this).attr("id");
            var datatype = $(this).attr("datatype");
            if ($('#UnitValue').val() == 692002) {
                if (IsLengthField(this)) {
                    name = ConverteFeetToMetre(name);
                }
            }
            if (model == "Number_of_Axles") {
                if (name == "")
                    noAxle = 1;

                if (configId == VEHICLE_CONFIGURATION_TYPE_CONFIG.DRAWBAR_TRAILER || configId == VEHICLE_CONFIGURATION_TYPE_CONFIG.DRAWBAR_TRAILER_3_TO_8) {
                    var cmpwyt = 0;
                    var drwabarCmpId = 0;
                    $(".comp").each(function (index) {
                        var compId = $(this).attr('id');
                        var weight = $('#' + compId + ' #Weight').val();
                        if (parseFloat(weight) > parseFloat(cmpwyt)) {
                            cmpwyt = weight;
                            drwabarCmpId = compId;
                        }
                        if (weight == "")
                            drwabarCmpId = 0;
                    });
                    if (drwabarCmpId != 0) 
                        axleCount = $('#' + drwabarCmpId + ' #Number_of_Axles').val();
                    else
                        axleCount = 0;
                }
                else {
                    if (noAxle == 0)
                        axleCount += parseInt(name != "" ? name : 0);
                    else
                        axleCount = 0;
                }
            }
            else {
                if (model == "OverallLength") {
                    overAllLength = name != "" ? name : 0;
                }
                configList.VehicleConfigParamList.push({ ParamModel: model, ParamValue: name, ParamType: datatype });
            }
        });
        configList.VehicleConfigParamList.push({ ParamModel: "Number of Axles", ParamValue: axleCount, ParamType: "int" });
        var wb = $('#VehicleConfigInfo #Wheelbase').val();
        configList.VehicleConfigParamList.push({ ParamModel: "Wheelbase", ParamValue: wb, ParamType: "float" });

        if (configId == VEHICLE_CONFIGURATION_TYPE_CONFIG.SPMT || configId == VEHICLE_CONFIGURATION_TYPE_CONFIG.CRANE) {
            configList.VehicleConfigParamList.push({ ParamModel: "Length", ParamValue: overAllLength, ParamType: "float" });
        }
        if (configId == VEHICLE_CONFIGURATION_TYPE_CONFIG.BOAT_MAST_EXCEPTION) {
            var trailerWeight = GetTrailerWeghtForBoatMast();
            configList.VehicleConfigParamList.push({ ParamModel: "TrailerWeight", ParamValue: trailerWeight, ParamType: "float" });
        }
        if (configList.VehicleConfigParamList.length > 0) {
            var leadingComponentType = parseInt($('#div_Individual_component_0 #VehicleSubType').val());
            $.ajax({
                url: '../VehicleConfig/GetMovementAssessment',
                type: 'POST',
                cache: false,
                async: false,
                data: { vehicleConfigObj: configList, VehicleId: vehicleConfigurationId, configTypeId: configId, isFleet: isFleet, isNotifyVSO: VSONotificationVehicle, leadingComponentType: leadingComponentType },
                beforeSend: function () {
                    //startAnimation();
                },
                success: function (response) {
                    $("#movement_assessment_section").show();
                    $("#movement_assessment_section").html(response);
                    VehicleConfigurationAssessmentInit("movement_assessment_section");
                    $('#cv_movementType').val($('#movementTypeForPlanMovement').val());
                    $('#cv_vehicleClass').val($('#vehicleClassForPlanMovement').val());
                    if ($(response).find("#MovementType").html() && $(response).find("#MovementType").html().trim().length == 0) {
                        $('#div_MovementType').show();
                        $('#spnMovementmessage').html("Based on the details you've filled in, the system cannot identify a Movement type.");
                    }
                    else {
                        var msg = "Based on the details you've filled in, the Movement type is  ";
                        if ($('#MovementTypeId').val() == "270154" || $('#MovementTypeId').val() == "270101" && !isFleet) {
                            if ($("#hf_IsSortApp").val() == "True" || ($('#IsCandVersion').val() == "true" || $('#IsCandVersion').val() == "True") ) {
                                msg = "Based on the information entered, your vehicle is not categorised as a SO ";
                            }
                            else {
                                msg = "Based on the information entered your vehicle does not meet the requirements for a notification. However, further details may be required for a more accurate assessment. If you are unsure, please complete the remaining details on the form or contact the helpdesk on 0300 470 3733";
                            }
                            $('#div_MovementType').show();
                            $('#div_MovementType').find('#MovementType').hide();
                        }
                        else {
                            $('#div_MovementType').show();
                        }
                        $('#spnMovementmessage').html(msg);
                        $("#vehicle_back_btn").show();
                        $('#btnbacktochoose').hide();
                    }
                    if (needConfirm) {

                        if ($('#PreviousMovementTypeId').val() != $(response).find('#ChangedMovementTypeId').val()) {
                            var isCandidate = false;
                            if ($('#IsCandVersion').val() == "true" || $('#IsCandVersion').val() == "True") {
                                isCandidate = true;
                            }
                            if ($('#MovementTypeId').val() == "270101") {
                                ShowErrorPopup("Based on the information entered, your vehicle is not categorised as a SO or VR1.", 'CloseErrorPopup');
                            }
                            else if (isCandidate) {
                                if ($('#MovementTypeId').val() != "207006") {
                                    ShowErrorPopup("The vehicle cannot be added, as it is not of special order", 'CloseErrorPopup');
                                }
                            }
                            else {
                                assessmentResult = 1;
                            }
                        }
                        else {
                            if ($('#MovementTypeId').val() == "270101") {
                                ShowErrorPopup("Based on the information entered, your vehicle is not categorised as a SO or VR1.", 'CloseErrorPopup');
                            }
                            else {
                                assessmentResult = 2;
                            }
                        }
                    }
                    else if (!importedComponent) {
                        if ($('#PreviousMovementTypeId').val() != $(response).find('#ChangedMovementTypeId').val()) {
                            compHtml = GenerateComponentModel(1);
                            LoadComponentSection(true);
                        }
                    }

                    if (callbackfn != undefined && callbackfn != null && callbackfn != '') {
                        callbackfn();
                    }
                },
                error: function () {
                    //alert('error 1830');
                    isAssessmentApiCalled = false;
                },
                complete: function () {
                    //stopAnimation();
                    isAssessmentApiCalled = false;

                }
            });
        }
    }
    return assessmentResult;
}
function GenerateVehicleConfiguration(isEdit) {
    var VehicleComponentDetail = GenerateComponentModel();

    var vehicleId = $('#vehicleId').val() || 0;
    var isCandidate = false;
    var isFleet = false;
    if ($('#IsCandVersion').val() == "true" || $('#IsCandVersion').val() == "True") {
        isCandidate = true;
    }
    var planMovement = false;
    if ($('#IsMovement').val() == "True" || $('#IsMovement').val() == "true" || $('#isMovement').val() == "true") {
        planMovement = true;
    }
    if (!isCandidate && !planMovement) {
        isFleet = true;
    }
    var VSONotificationVehicle = false;
    if ($('#NotiVSO').val() != '') {
        VSONotificationVehicle = true;
    }
    var configId = $('#ConfigTypeId').val();
    $.ajax({
        url: '../VehicleConfig/GenerateVehicleConfiguration',
        type: 'POST',
        cache: false,
        //contentType: 'application/json; charset=utf-8',
        async: false,
        data: '{"vehicleID":' + vehicleId + ',"vehicleComponentDetail":' + JSON.stringify(VehicleComponentDetail) + ',"isMovement":' + planMovement + ',"isCandidate":' + isCandidate + '}',
        beforeSend: function () {
            if ($("#IsVehicleConfigEdit").val() != 1 || isEdit == 2)
                startAnimation();
        },
        success: function (data) {
            var leadingComponentType = parseInt(VehicleComponentDetail.ComponentDetailList[0].ComponentSubTypeId);
            $.ajax({
                url: '../VehicleConfig/GetMovementAssessment',
                type: 'POST',
                cache: false,
                async: false,
                data: { vehicleConfigObj: null, VehicleId: data.VehicleId, configuration: data.configuration, configTypeId: configId, isFleet: isFleet, isNotifyVSO: VSONotificationVehicle, leadingComponentType: leadingComponentType },
                beforeSend: function () {
                    if ($("#IsVehicleConfigEdit").val() != 1 || isEdit == 2)
                        startAnimation();
                },
                success: function (response) {
                    $("#movement_assessment_section").show();
                    $("#movement_assessment_section").html(response);
                    VehicleConfigurationAssessmentInit("movement_assessment_section");
                    $('#cv_movementType').val($('#movementTypeForPlanMovement').val());
                    $('#cv_vehicleClass').val($('#vehicleClassForPlanMovement').val());
                    if ($(response).find("#MovementType").html() && $(response).find("#MovementType").html().trim().length == 0) {
                        $('#div_MovementType').show();
                        $('#spnMovementmessage').html("Based on the details you've filled in, the system cannot identify a Movement type.");
                    }
                    else {
                        var msg = "Based on the details you've filled in, the Movement type is  ";
                        if ($('#MovementTypeId').val() == "270154" || $('#MovementTypeId').val() == "270101"  && !isFleet) {
                            if ($("#hf_IsSortApp").val() == "True" || ($('#IsCandVersion').val() == "true" || $('#IsCandVersion').val() == "True") ) {
                                msg = "Based on the information entered, your vehicle is not categorised as a SO.";
                            }
                            else {
                                msg = "Based on the information entered your vehicle does not meet the requirements for a notification. However, further details may be required for a more accurate assessment. If you are unsure, please complete the remaining details on the form or contact the helpdesk on 0300 470 3733";
                            }
                            $('#div_MovementType').show();
                            $('#div_MovementType').find('#MovementType').hide();
                        }
                        else {
                            $('#div_MovementType').show();
                        }
                        $('#spnMovementmessage').html(msg);
                    }
                    if (($("#IsVehicleConfigEdit").val() != 1 && isEdit!=1) || isEdit == 2) {
                        if ($('#PreviousMovementTypeId').val() != $(response).find('#ChangedMovementTypeId').val()) {
                            if ($('#MovementTypeId').val() == "270101") {
                                ShowErrorPopup("Based on the information entered, your vehicle is not categorised as a SO or VR1.", 'CloseErrorPopup');
                            }
                            else if (isCandidate) {
                                if ($('#MovementTypeId').val() != "207006") {
                                    ShowErrorPopup("The vehicle cannot be added, as it is not of special order", 'CloseErrorPopup');
                                }
                            }
                            else {
                                var Msg = "Based on the details you've filled in, now the Movement type is " + $('#MovementType').html() + ". Do you want to save the vehicle?";
                                if ($('#MovementTypeId').val() == "270154" || $('#MovementTypeId').val() == "270101"  && !isFleet) {
                                    if ($("#hf_IsSortApp").val() == "True" || ($('#IsCandVersion').val() == "true" || $('#IsCandVersion').val() == "True") ) {
                                        msg = "Based on the information entered, your vehicle is not categorised as a SO.";
                                    }
                                    else {
                                        msg = "Based on the information entered your vehicle does not meet the requirements for a notification. However, further details may be required for a more accurate assessment. If you are unsure, please complete the remaining details on the form or contact the helpdesk on 0300 470 3733";
                                    }
                                    ShowErrorPopup(msg, 'CloseErrorPopup');
                                }
                                else {
                                    ShowWarningPopup(Msg, 'UpdateComponents', 'CloseWarningPopup');
                                }
                            }
                        }
                        else {
                            if ($('#MovementTypeId').val() == "270101") {
                                ShowErrorPopup("Based on the information entered, your vehicle is not categorised as a SO or VR1.", 'CloseErrorPopup');
                            }
                            else {
                                UpdateComponents();
                            }
                        }
                    }
                },
                complete: function () {
                    stopAnimation();
                }
            });
        },
        complete: function () {
            stopAnimation();
        },
        error: function (response) {
            stopAnimation();
        }
    });
}
function LoadAppVehicle(MovementId, vehicleId, vehicleClassDesc = null) {
    if (!SelectedVehicles.includes(vehicleConfigurationId)) {
        SelectedVehicles.push(vehicleConfigurationId);
    }
    LoadContentForAjaxCalls("POST", '../VehicleConfig/InsertMovementVehicle', { movementId: MovementId, vehicleId: vehicleId, flag: 5 }, '#vehicle_details_section', '', function () {
        MovementSelectedVehiclesInit();
        VehicleDetailsInit();
        ViewConfigurationGeneralInit();
        MovementAssessDetailsInit();
        if (vehicleClassDesc != null) {
            showToastMessage({
                message: "The vehicle(s) highlighted is not " + vehicleClassDesc + ". Please edit or remove the vehicle to continue.",
                type: "error"
            });
        }
        else if ($('#hf_IsAddVehicleErrorMsg').val()==''){
            CloseWarningPopupDialog();
        }
    });
}
function ViewComponentDetail(componentId, mode) {
    $.ajax({
        url: '../Vehicle/GeneralComponent',
        data: { componentId: componentId, mode: mode },
        type: 'GET',
        async: false,
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {
            SubStepFlag = 2.1;
            var compView = $(response).find('.divcomponentView');
            $(compView).appendTo('#component_view_section');
            $(this).scrollTop();
            $('#component_view_section').show();
            $('#component_list_section').hide();
            $('#btn_back_to_fleet').hide();
            FleetStepFlag = 0.3;
        },
        error: function (xhr, textStatus, errorThrown) {
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function ClearValidationMessage(_this) {
    $(_this).closest('div').find('.error').text('');
}
function validateVehicleRegTable(_this, vd) {
    var isTableValid = true;
    var movementTypeId = $('#movementTypeId').val();
        var form = $("#div_reg_config_vehicle");
        if (form.length > 0) {
            var tablevehicletable = form.find('#vehicle-reg-table.RegistrationComponent');
            if (tablevehicletable.length > 0) {
                console.log('vehicle-reg-table', true);
                var allTrs = $("#vehicle-reg-table.RegistrationComponent > tbody > tr");
                var registrationDetails = [];
                allTrs.each(function (index, value) {//loop thorugh all tbody>tr items and insert all itemsinto an array
                    var regIdComp = $(this).find('td .txt_register_config').length > 0 ? $(this).find('td .txt_register_config') : $(this).find('.txt_register_config');
                    var fleetIdComp = $(this).find('td .txt_fleet_config').length > 0 ? $(this).find('td .txt_fleet_config') : $(this).find('.txt_fleet_config');
                    var regId = regIdComp.val() != "" ? regIdComp.val() : regIdComp.text();
                    var fleetId = fleetIdComp.val() != "" ? fleetIdComp.val() : fleetIdComp.text();
                    var id = $(this).find('td .hdId').length > 0 ? $(this).find('td .hdId').val() : 0;
                    if (regId || fleetId) {
                        var obj = { RegId: regId, FleetId: fleetId, Id: id };
                        registrationDetails.push(obj);
                    }
                }).promise().done(function () { // after loop through all tbody tr items, we need to check the table has atleast one entry
                    console.log('registrationDetails', registrationDetails);
                    if (registrationDetails.length == 0) {
                        if (movementTypeId != 270101 && movementTypeId != 270110 && movementTypeId != 270111
                            && movementTypeId != 270112 && movementTypeId != 270006 && movementTypeId != 270156) {
                            form.find('#error_msg').text('Please enter at least one registration plate or fleet id.');
                            //setTimeout(function () {
                            //    form.find('#error_msg').text('');
                            //}, 5000);
                            isTableValid = false;
                        }
                    } else {
                        let len1 = registrationDetails.length; //get the length of your array
                        let len2 = $.unique(registrationDetails).length; //the length of array removing the duplicates

                        if (len1 > len2) {
                            console.log('Found duplicate');
                            isTableValid = false;
                        } else {
                            console.log('Did not find duplicate');
                        }

                        isTableValid = validateConfigDataV1();
                    }
                });
        }
    }
    return isTableValid;
}
function DeleteVehicleComponentFn(e) {
    var componentId = $(e).attr("componentid");
    DeleteVehicleComponentConfirmation(componentId, e);
    //DeleteVehicleComponent(componentId, e);
}
function VehicleTypeChangeFn(e) {
    VehTypeChange(e);
}
function VehicleSubTypeChangeFn(e) {
    VehSubTypeChange(e,1);
    
}
$('body').on('click', '.divComponentDataItem', function () {
    var itemToDIsplay = $(this).data('elemtodisplay');
    $('.divComponentDataItem').removeClass('active');
    $('.component-item').hide();
    $(this).addClass('active');
    $('#' + itemToDIsplay).fadeIn();
    SetHeaderHeight();
});
$('body').on('keyup', '#VehicleConfigInfo input,#VehicleConfigInfo textarea,#config_registration_section input', function () {
    var componentCount = $('#ComponentCount').val();
    var id = $(this).attr('id');
    var weight = 0;
    if (componentCount == 1 || componentCount == "1") {
        if (id == 'OverallLength')
            id = "Length";//id is different in component section

        var isLpConfig = false;
        if (id == 'txt_register_config' || id == 'txt_fleet_config') {
            id = id.replace("_config", "");//id is different in component section
            isLpConfig = true;
        }
        var val = $(this).val();
        var compElem = !isLpConfig ? $('#componentConfig #' + id) : $('.divComponentData #regDetailsConfig #' + id);
        var noChangeFlag = true;
        if (id == "Internal_Name" && compElem.val() == val) {//compElem.val() != ""
            noChangeFlag = false;
        }
        if (id == 'Number_of_Axles' && compElem.val() == val) {
            noChangeFlag = false;
        }
        if (compElem.length > 0 && noChangeFlag) {
            compElem.val(val);
            //if (id == 'Number_of_Axles')
                //compElem.trigger('change');
        }
        if (id == "Weight") {
            weight = val;
            IsGroundClearenceRequired(weight);
        }
    }

});

$('body').on('keypress', '#VehicleConfigInfo #Internal_Name', function () {
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode == '13') {
        $(this).trigger('change');
    }
});

function AddDefaultComponent(componentTypeId, subComponentTypeId=0,flag=1) {
    var orgId = 0;
    if ($('#Organisation_ID').val() != undefined && $('#Organisation_ID').val() != '') {
        orgId = $('#Organisation_ID').val();
    }
    var configId = $('#vehicleConfigId').val() == "" ? 0 : $('#vehicleConfigId').val();
    if (configId == undefined || configId == "undefined") {
        configId = 0;
    }
    var ConfigTypeId = $('#ConfigTypeId').val();//ConfigTypeId!=VEHICLE_CONFIGURATION_TYPE_CONFIG.RECOVER_VEHICLE
    var IsApplication = false;
    if ($('#IsApplication').val() == "true" || $('#IsApplication').val() == "True") {
        IsApplication = true;
    }
    var componentCount = $('#ComponentCount').val();
    var guid = $('#GUID').val();
    $.ajax({
        async: false,
        type: "POST",
        url: '../VehicleConfig/GetNextDefaultComponent',
        data: { componentType: componentTypeId, subComponentType: subComponentTypeId, vehicleConfigId: configId, isMovement: IsApplication, organisationId: orgId, guid: guid, componentCount: componentCount, ConfigTypeId: ConfigTypeId },
        beforeSend: function () {
            //startAnimation();
        },
        success: function (result) {
            //var flag = 1;
            if (result != '' && result != undefined && result != null) {
                flag = 2;
                $('.Comp_Div').remove();
            } 

            CreateComponentForVehicle(flag);
            
        },
        error: function (result) {
            //stopAnimation();
        },
        complete: function () {
            //stopAnimation();
        }
    });
}
function GoToMovementPage(MovementId, vehicleId) {
    if (!SelectedVehicles.includes(vehicleConfigurationId)) {
        SelectedVehicles.push(vehicleConfigurationId);
    }
    var goToMovementPage = true;
    var gtIsRenotify = $('#IsRenotify').val();
    var gtIsRevise = $('#hf_IsRevise').val();
    var gtIsNotif = $('#hf_IsNotif').val();
    var gtIsAppClone = $('#hf_IsAppClone').val();
    var gtIsNotifClone = $('#hf_IsNotifClone').val();
    if (gtIsRenotify.toLowerCase() == "true" || gtIsRevise.toLowerCase() == "true" || gtIsNotif.toLowerCase() == "true"
        || gtIsAppClone.toLowerCase() == "true" || gtIsNotifClone.toLowerCase() == "true" || BackAndForthNavMethods.IsWorkFlowStarted()) {
        goToMovementPage = false;
    }
    $.ajax({
        url: '../VehicleConfig/InsertMovementVehicle',
        data: { movementId: MovementId, vehicleId: vehicleId, flag: 5, goToMovementPage: goToMovementPage },
        type: 'POST',
        async: false,
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {
            $('#vehicle_details_section').html(response);
            CloseWarningPopupDialog();
            var isBckCall = false;
            var appRevId = $('#AppRevisionId').val();
            var notifId = $('#NotificationId').val()
            if (appRevId > 0 || notifId > 0) {
                isBckCall = true;
            }
            MovementSelectedVehiclesInit();
            VehicleDetailsInit();
            ViewConfigurationGeneralInit();
            GeneralVehicCompInit();
            MovementAssessDetailsInit();

            var VehicleClass = $("#hf_VehicleClassNew").val();
            var VSOType = $("#hf_VSOTypeNew").val();
            var isVSO = false;
            if (VehicleClass == '241001' && (VSOType == '' || VSOType == '0' || VSOType == undefined)) {//If vehicle class is Vehicle Special Order and VSO type is not updated, then popup will display
                isVSO = true;
            }

            LoadContentForAjaxCalls("POST", '../Movements/GetMovementTypeConfirmation', { isBackCall: isBckCall }, '#movement_type_confirmation', isVSO, function () {
                MovementTypeConfirmationInit();
            });
            $('.hiding').hide();
        },
        error: function (xhr, textStatus, errorThrown) {
        },
        complete: function () {
            stopAnimation();
        }
    });

}
//function to Add to Fleet()
function ComponentAddToFleet(componentId) {
    var vd = false;
    var axleVd = false;
    vd = validateComponentV1(componentId);
    axleVd = ValidateAxlesV1(componentId);
    if (vd && axleVd && $('#MovementTypeId').val() != 0) {
        var compName = $('#' + componentId).find('#Internal_Name').val();
        CheckCompFormalNameExists(componentId, compName);
    }
}
//function to check the formal name exists during add to fleet
function CheckCompFormalNameExists(componentId, compName) {

    componentName = compName;
    var configId = $('#vehicleConfigId').val();
    var planMvmt = false;
    if ($('#IsMovement').val() == "True" || $('#IsMovement').val() == "true" || $('#isMovement').val() == "true") {
        planMvmt = true;
    }
    var orgId = 0;
    if ($('#Organisation_ID').val() != undefined && $('#Organisation_ID').val() != '') {
        orgId = $('#Organisation_ID').val();
    }
    $.ajax({
        url: '../VehicleConfig/CheckComponentName',
        type: 'POST',
        cache: false,
        async: false,
        data: { componentName: compName, organisationId: orgId },
        beforeSend: function () {
            startAnimation();
        },
        success: function (data) {

            if (data.success > 0) {
                var warningOverwrite = "Component already exists. Do you want to overwrite? <br /><br /> Note : If you want to save it as a new component, edit the internal name and then add to fleet";
                ShowWarningPopup(warningOverwrite, 'AddCompToFleet', '', componentId);
            }
            else {
                AddCompToFleet(componentId);
            }
        },
        complete: function () {
            stopAnimation();
        }
    });
}
//function to add to fleet
function AddCompToFleet(componentId) {
    var compdetail = GetComponentDetails(componentId);
    var movemnetTypeId = $('#MovementTypeId').val();
    $.ajax({
        url: '../VehicleConfig/AddComponentToFleetLibrary',
        type: 'POST',
        async: false,
        data: { componentDetail: compdetail, movementTypeId: movemnetTypeId },
        beforeSend: function () {
            CloseWarningPopup();
            startAnimation();
        },
        success: function (result) {
            if (result.result > 0) {
                ShowSuccessModalPopup('Component added to fleet', 'CloseSuccessModalPopup');
            }
            else {
                ShowSuccessModalPopup('Not saved', 'CloseSuccessModalPopup');
            }
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function GetComponentDetails(componentId) {
    var guid = $('#GUID').val();
    var isCandidateVehicle = false;
    if ($('#IsCandidate').val() != undefined && $('#IsCandidate').val() != "false" && $('#IsCandidate').val() != "False") {
        isCandidateVehicle = true;
    }
    if ($('#IsCandVersion').val() == "true" || $('#IsCandVersion').val() == "True") {
        isCandidateVehicle = true;
    }

    var planMovement = false;
    if ($('#PlanMovement').val() == "True" || $('#isMovement').val() == "true" || $('#IsMovement').val() == "True" || $('#IsMovement').val() == "true") {
        planMovement = true;
    }
    var VehicleParamList = [];
    var registrationParams = [];
    var axlesList = [];
    var vehicleComponent = {
        VehicleParamList: VehicleParamList
    };
    var VehicleComponentDetail = {
        ComponentId: componentId,
        GUID: guid,
        vehicleComponent: vehicleComponent,
        ComponentAxleList: axlesList,
        ComponentRegistrationList: registrationParams
    };

    $('#' + componentId).find('.dynamic input,textarea:not(hidden,checkbox)').each(function () {
        var name = $(this).val();
        var model = $(this).attr("id");
        var datatype = $(this).attr("datatype");
        var fieldName = $(this).attr("name");
        if ($('#UnitValue').val() == 692002) {
            if (IsLengthField(this)) {
                name = ConverteFeetToMetre(name);
            }
        }
        if ($('#ComponentCount').val() == "1") {
            if (fieldName == "Left Overhang" || fieldName == "Right Overhang" || fieldName == "Front Overhang" || fieldName == "Rear Overhang") {
                if (name == "") {
                    name = $("input[name='v_" + fieldName + "']").val();
                }
            }
        }
        if ((planMovement || isCandidateVehicle) && model == "Internal_Name") {
            VehicleParamList.push({ ParamModel: "Formal_Name", ParamValue: name, ParamType: datatype });
        }
        VehicleParamList.push({ ParamModel: model, ParamValue: name, ParamType: datatype });
    });

    var numberOfAxles = $('#' + componentId + ' .axledrop').val();
    var configurableAxles = $('#' + componentId + ' .AxleConfig').val();
    var axleModel = $('#' + componentId + ' .axledrop').attr("id");
    var axleDatatype = "int";
    VehicleParamList.push({ ParamModel: axleModel, ParamValue: numberOfAxles, ParamType: axleDatatype });
    var couplingType = $('#Coupling').html();
    var couplingDatatype = "string";
    VehicleParamList.push({ ParamModel: 'Coupling', ParamValue: couplingType, ParamType: couplingDatatype });
    $('#' + componentId + ' .dynamic input:checkbox').each(function () {

        var name = $(this).val();
        var model = $(this).attr("id");
        var value = 0;

        if ($(this).is(':checked')) {
            value = 1;
        }
        var datatype = "bool";

        VehicleParamList.push({ ParamModel: model, ParamValue: value, ParamType: datatype });
    });


    //var comp_registerId = $(this).find("#txt_register").val();
    //var comp_fleetId = $(this).find("#txt_fleet").val();
    //if (comp_registerId != '' || comp_fleetId != '') {
    //    var elemid = $(this).find("table.RegistrationComponent").data('elemid');
    //    add_row_component(this, elemid);
    //}
    $('#' + componentId).find('.tbl_registration tbody .tr_Registration').each(function () {
        var regVal = $(this).find('.txt_register').val();
        if (regVal == "") { regVal = $(this).find('.txt_register').html(); }
        var fleetId = $(this).find('.txt_fleet').val();
        if (fleetId == "") { fleetId = $(this).find('.txt_fleet').html(); }
        if (fleetId != "" || regVal != "") {
            registrationParams.push({ RegistrationValue: regVal, FleetId: fleetId });
        }

    });

    //--------------- Axle Details -----------------

    var i = 1;
    var unitvalue = $('#UnitValue').val();
    var wb = 0;

    $('#' + componentId).find('#tbl_Axle tbody tr').each(function () {
        var axleNum = i;
        var noOfWheels = $(this).find('.nowheels').val();
        var axleWeight = $(this).find('.axleweight').val();
        var distanceToNxtAxl = $(this).find('.disttonext').val();

        //to check if distance tonext axle value is definedor not
        if (typeof distanceToNxtAxl !== "undefined" && distanceToNxtAxl != "") {
            if (unitvalue == 692002) {
                distanceToNxtAxl = ConverteFeetToMetre(distanceToNxtAxl);
            }
            wb = parseFloat(wb) + parseFloat(distanceToNxtAxl);
        }

        var tyreSize = $(this).find('.tyresize').val();

        var tyreSpace = null;
        $(this).find('.cstable input:text').each(function () {
            var _thistxt = $(this).val();
            //if (_thistxt != undefined) {
            //    if (unitvalue == 692002) {
            //        _thistxt = ConverteFeetToMetre(_thistxt);
            //    }
            //}
            if (tyreSpace != null) {
                tyreSpace = tyreSpace + "," + _thistxt;
            }
            else {
                tyreSpace = _thistxt;
            }
        });

        axlesList.push({ AxleNumId: axleNum, NoOfWheels: noOfWheels, AxleWeight: axleWeight, DistanceToNextAxle: distanceToNxtAxl, TyreSize: tyreSize, TyreCenters: tyreSpace });
        i++;
    });
    var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
    var isVR1 = $('#vr1appln').val();
    var isNotify = $('#ISNotif').val();
    var validwb = true;
    var axlecount = $('#' + componentId + ' .axledrop').val();  //added for crane workflow changes
    if (isNotify == 'True' || isNotify == 'true') {
        var vc = $('#' + componentId + ' #VehicleClass').val();
        var catAwb = 1.5 * axlecount + 0.5 * (axlecount % 2);
        if (vc == 241006) {
            if (parseFloat(wb) < parseFloat(catAwb)) {
                $('#' + componentId + ' .error').text('Wheelbase should be greater than or equal to ' + catAwb);
                validwb = false;
            }
        }
        isVR1 = 'True';
    }


    return VehicleComponentDetail;
}
function GetFilteredVehicles() {    

    var isCandidate = false;
    if ($('#IsCandVersion').val() == "true" || $('#IsCandVersion').val() == "True") {
        isCandidate = true;
    }
    var movementId = $('#movementTypeId').val() == undefined ? 0 : $('#movementTypeId').val();
    if (isCandidate) {
        movementId = 270006;
    }

    var configList = {
        VehicleConfigParamList: []
    };
    var configTypeId = $('#ConfigTypeId').val() == undefined ? 0 : parseInt($('#ConfigTypeId').val());

    $('#div_config_general input,textarea,span.lblParam').not(':checkbox,.axledropVehicle').each(function () {
        var name;
        if ($(this)[0].localName == "span") {
            name = $(this)[0].innerText;
        }
        else {
            name = $(this).val();
        }
        var model = $(this).attr("id");
        var datatype = $(this).attr("datatype");
        if ($('#UnitValue').val() == 692002) {
            if (IsLengthFields(this)) {
                name = ConverteFeetToMetre(name);
            }
        }
        if (model == "Internal_Name") {
            configList.VehicleConfigParamList.push({ ParamModel: "Formal_Name", ParamValue: name, ParamType: datatype });
        }
        configList.VehicleConfigParamList.push({ ParamModel: model, ParamValue: name, ParamType: datatype });
    });

    var numberOfAxles = $('#Number_of_Axles').val();
    var configurableAxles = $('.AxleConfig').val();
    var axleModel = $('#Number_of_Axles').attr("name");
    var axleDatatype = "int";
    configList.VehicleConfigParamList.push({ ParamModel: axleModel, ParamValue: numberOfAxles, ParamType: axleDatatype });
    var numberOfAxlesTrailer = $('#Number_of_Axles_for_Trailer').val();
    var axleModelTrailer = $('#Number_of_Axles_for_Trailer').attr("name");
    configList.VehicleConfigParamList.push({ ParamModel: axleModelTrailer, ParamValue: numberOfAxlesTrailer, ParamType: axleDatatype });

    $.ajax({
        url: '../VehicleConfig/GetVehicleFilteredCombinations',
        type: 'POST',
        cache: false,
        async: false,
        data: { vehicleConfigObj: configList, configTypeId: configTypeId, movementTypeId: movementId },
        beforeSend: function () {
            //startAnimation();
        },
        success: function (response) {
            $('#filtered_vehicle_list').html(response);
            $("#div_config_general").attr('class', 'col-lg-8 col-md-12 col-sm-12');
        },
        error: function (x) {
                
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function ImportFilteredVehicle(vehicleId, vehicleName, flag) {//flag => 1 - applicationvehicle, 2 - fleet, 3 - route vehicle
    var count = CheckVehicleIsValid(vehicleId, flag);
    if (count == 0) {
        var planMovement = false;
        if ($('#PlanMovement').val() == "True" || $('#isMovement').val() == "true" || $('#IsMovement').val() == "True" || $('#IsMovement').val() == "true") {
            planMovement = true;
        }
        var isCandidate = false;
        if ($('#IsCandVersion').val() == "true" || $('#IsCandVersion').val() == "True") {
            isCandidate = true;
        }
        if (planMovement) {
            LoadContentForAjaxCalls("POST", '../VehicleConfig/InsertMovementVehicle', { movementId: $('#MovementId').val() || 0, vehicleId: vehicleId, flag: flag }, '#vehicle_details_section', '', function () {
                MovementSelectedVehiclesInit();
                VehicleDetailsInit();
                ViewConfigurationGeneralInit();
                GeneralVehicCompInit();
                MovementAssessDetailsInit();
                $('#backbutton').show();
            });
        }
        else if (isCandidate) {
            var revId = $('#revisionId').val();
            $.ajax({
                url: '../VehicleConfig/ImportFleetVehicleToRoute',
                type: 'POST',
                async: false,
                data: { vehicleId: vehicleId, ApplnRevId: revId },
                beforeSend: function (result) {
                    startAnimation();
                },
                success: function (result) {
                    if (result != 0) {
                        ShowSuccessModalPopup("'" + vehicleName + "' vehicle imported successfully", "Show_CandidateRTVehicles");
                    }
                    else {
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
    }
    else {
        ShowErrorPopup("Components missing. Choose a valid vehicle.");
    }
}

function LoadComponentSection(isAxleLoad = true) {
    var activeCompTabHeaderId = $('.divComponentDataItem.active').attr('id');
    var activeCompTabContentId = $('.divComponentDataItem.active').data('elemtodisplay');

    var guid = $('#GUID').val();
    var planMvmt = false;
    if ($('#IsMovement').val() == "True" || $('#IsMovement').val() == "true" || $('#isMovement').val() == "true") {
        planMvmt = true;
    }
    var isCandidate = false;
    if ($('#IsCandVersion').val() == "true" || $('#IsCandVersion').val() == "True") {
        isCandidate = true;
    }
    var vehicleId = $("#vehicleId").val();
    var mvmntType = $('#MovementTypeId').val();
    var isEditMovement = false;
    if ($('#IsEditMovement').val() == "true" || $('#IsEditMovement').val() == "True") {
        isEditMovement = true;
    }
    var configTypeId=$('#ConfigTypeId').val();
    $.ajax({
        url: '../VehicleConfig/ComponentDetailSub',
        type: 'POST',
        cache: false,
        async: false,
        data: { Guid: guid, isMovement: planMvmt, vehicleConfigId: vehicleId, isCandidate: isCandidate, movementTypeId: mvmntType, isEditMovement: isEditMovement, ConfigTypeId:configTypeId },
        beforeSend: function () {
            //startAnimation();
        },
        success: function (response) {
            $("#div_ComponentDetailSub").html(response);

            //Stay in same component tab. Earlier after html content reset, the active tab was first component
            if (activeCompTabContentId && $('#' + activeCompTabContentId).length > 0) {
                $('.component-item-axle-section').hide();
                $('.divComponentDataItem').removeClass('active');
                $('#' + activeCompTabHeaderId).addClass('active');
                $('#' + activeCompTabContentId).show();
            }
            SetRangeForSpacing();
            IterateThroughText();
            LoadPreviousComponentValue(isAxleLoad);

            $('.component-item-axle-section').each(function (index) {
                var noOfAxles = $(this).find('#Number_of_Axles').val();
                var axleTableCount = $(this).find('.axle').length;
                if (noOfAxles > 0 && axleTableCount == 0) {
                    $(this).find('#Number_of_Axles').trigger('blur');
                }
            });
        },
        complete: function () {
            //stopAnimation();
        },
        error: function (response) {
            stopAnimation();
        }
    });
}

function LoadPreviousComponentValue(isAxleLoad = true) {
    var mvmntTypeId = $('#MovementTypeId').val();
    var labeldivAxle = $('#VehicleConfigInfo #AxleWeight').parent().parent().find(".labelDiv");
    var labelTextAxle = $(labeldivAxle).find("label").text().trim();
    if (mvmntTypeId != "270001" && mvmntTypeId != "270102" && mvmntTypeId != "270101") {
        $('#VehicleConfigInfo #AxleWeight').attr("isrequired", 1);
        if (labelTextAxle.indexOf("*") < 0) {
            $(labeldivAxle).html('');
            $(labeldivAxle).append('<label class="text-normal"><b>' + labelTextAxle + '<span> *</span></b></label>');
        }
    }
    else {
        $('#VehicleConfigInfo #AxleWeight').attr("isrequired", 0);
        if (labelTextAxle.indexOf("*") > 0) {
            labelTextAxle = labelTextAxle.replace('*', '');
            $(labeldivAxle).html('');
            $(labeldivAxle).append('<label class="text-normal">' + labelTextAxle + '</label>');
        }
    }
    for (var i = 0; i < compHtml.ComponentDetailList.length; i++) {
        var paramLength = compHtml.ComponentDetailList[i].vehicleComponent.VehicleParamList.length;
        var params = compHtml.ComponentDetailList[i].vehicleComponent.VehicleParamList;
        var compId = compHtml.ComponentDetailList[i].ComponentId;
        var mvmntTypeId = compHtml.MovemnetTypeId;
        var configTypeId = $('#ConfigTypeId').val();
        var weight = 0;
        for (var j = 0; j < paramLength; j++) {
            var param = params[j].ParamModel;
            if (param == 'OverallLength')
                param = "Length";//id is different in component section
            
            var val = params[j].ParamValue;
            var compElem = $('#' + compId).find('#' + param);
            var isRequired = $(compElem).attr('isRequired');
            var range = $(compElem).attr('range');
            if (param == "Weight") {
                weight = val;
            }
            if (param =="Ground_Clearance") {
                if (mvmntTypeId == 270006 && weight > 150000 ) {
                    $(compElem).attr("isrequired", 1);
                    var labelcmpdiv = $(compElem).parent(".input-field").parent(".dynamic").find(".labelCompDiv");
                    var labelcmpText = $(labelcmpdiv).text().trim();
                    $(labelcmpdiv).html('');
                    $(labelcmpdiv).append('<label class="text-normal"><b>' + labelcmpText + '<span> *</span></b></label>');
                }
            }
            $(compElem).attr("previousValue", val);
            var noChangeFlag = true;
            if (param == "Internal_Name" && compElem.val() == val) {//compElem.val() != ""
                noChangeFlag = false;
            }
            if (param == 'Number_of_Axles' && compElem.val() == val) {
                noChangeFlag = false;
                var comp = i + 1;
                if (val != "")
                    $('#VehicleConfigInfo .axledropVehicle_' + comp).val(val);

                var maxlength = $(compElem).attr('maxlength');
                
                if (range != undefined) {
                    $('#VehicleConfigInfo .axledropVehicle_' + comp).attr("range", range);
                }
                else {
                    $('#VehicleConfigInfo .axledropVehicle_' + comp).attr("range", '');
                }
                if (maxlength != undefined) {
                    $('#VehicleConfigInfo .axledropVehicle_' + comp).attr("maxlength", maxlength);
                }
            }
            if (compElem.length > 0 && noChangeFlag) {
                compElem.val(val);
                if (param == 'Number_of_Axles')
                    compElem.trigger('change');
            }
        }
        
        if (compHtml.ComponentDetailList[i].ComponentRegistrationList.length > 0) {
            LoadComponentRegistration(compHtml.ComponentDetailList[i]);
        }
    }
    for (var i = 0; i < compHtml.ComponentDetailList.length; i++) {
        if (compHtml.ComponentDetailList[i].ComponentAxleList.length > 0) {
            LoadAxleSection(compHtml.ComponentDetailList[i], isAxleLoad);
        }
    }
    if ($('#MovementTypeId').val() != "0") {
        var componentCount = $('#ComponentCount').val();
        if (componentCount == 1 || componentCount == "1") {
            $("#VehicleConfigInfo input").each(function () {
                var id = $(this).attr('id');
                if (id == 'OverallLength')
                    id = "Length";
                var compElem = $('#componentConfig #' + id);
                var required = $(compElem).attr('isrequired'); 
                var range = $(compElem).attr('range');
                var compElemId = $(compElem).attr('id');
                $(this).attr("isrequired", required);
                var labeldiv = $('#' + id).closest("div").parent().find(".labelDiv");
                var labelText = $(labeldiv).find("label").text().trim();
                if (id != "Number_of_Axles" && required != undefined) {
                    if (required == "1") {
                        if (labelText.indexOf("*") < 0) {
                            $(labeldiv).html('');
                            $(labeldiv).append('<label class="text-normal"><b>' + labelText + '<span> *</span></b></label>');
                        }
                    }
                    else {
                        if (labelText.indexOf("*") > 0) {
                            labelText = labelText.replace('*', '');
                            $(labeldiv).html('');
                            $(labeldiv).append('<label class="text-normal">' + labelText + '</label>');
                        }
                    }
                }
                if (range != undefined) {
                    $(this).attr("range", range);
                }
                else {
                    $(this).attr("range", '');
                }
                if (id != "AxleWeight" && id != "Speed") {
                    if (id == compElemId) {
                        $('.div_' + id).show();
                        if ($(this).val() != "" && $(compElem).val() == "") {
                            $(compElem).val($(this).val());
                        }
                    }
                    else {
                        $(this).attr("isrequired", "0");
                        $('.div_' + id).hide();
                    }
                }
                if ($("#IsVehicleConfigEdit").val() == 1) {
                    if (id == compElemId && $(this).val() == "") {
                        var val = $(compElem).val();
                        $(this).val(val);
                    }
                    else if (id == compElemId && $(compElem).val() != $(this).val()) {
                        $(compElem).val($(this).val());
                    }
                }
            });

            
        }

        if ($("#IsVehicleConfigEdit").val() == 1) {
            AxleTableMethods.ShowHideViewAllAxlePopUpButtonInConfig();
        }
    }

    MaxAxleWeightUpdate();
}

function CheckVehicleIsValid(vehicleId, flag) {
    var v_count = 0;
    $.ajax({
        url: '../VehicleConfig/ChekcVehicleIsValid',
        type: 'POST',
        async: false,
        data: { vehicleId: vehicleId, flag: flag },
        beforeSend: function (result) {
            startAnimation();
        },
        success: function (result) {
            v_count = result.Count;
        },
        complete: function () {
            stopAnimation();
        }
    });
    return v_count;
}

function AssessmentForImportedComponent() {
    var componentCount = $('#ComponentCount').val();
    $("#VehicleConfigInfo input").each(function () {
        var id = $(this).attr('id');
        if (id == 'OverallLength')
            id = "Length";
        var compElem = $('#componentConfig #' + id);
        var compElemId = $(compElem).attr('id');
        if (id == compElemId && $(this).val() == "") {
            var val = $(compElem).val();
            if ((componentCount == 1 || componentCount == "1"))
                $(this).val(val);
        }
    }).promise().done(function () {
        MovementAssessment(false, true);
    });
}

//function to convert feet and inches to metres
function ConverteFeetToMetre(_this) {

    var text = _this;
    var onlyInch = 0;
    var onlyFeet = 0;
    var feet;
    var inches;

    if ((text == "0\'0\"" || text == "") && text !=0) {
        return null;
    }
    else if (text == 0) {
        return 0;
    }

    //to check whether the value is entered only in feet
    if (text.indexOf('\'') === -1 && text.indexOf('\"') === -1) {
        text = text + '\'' + 0 + '\"';
    }
    //to check whether the value is entered only in inches
    else if (text.indexOf('\'') === -1) {
        onlyInch = 1;
    }

    text = text.replace('\"', '');
    if (onlyInch == 0) {
        var splitValue = text.split('\'');
        feet = splitValue[0];
        inches = splitValue[1];
        if (inches == undefined) {
            inches = 0;
        }
    }
    else {
        feet = 0;
        inches = text;
    }

    var totalInches = feet * 12;
    var Inches = totalInches + (inches * 1);
    //var metres = Inches / 39.370;
    var cm = Inches * 2.54;
    var metres = cm / 100;
    metres = metres.toFixed(4);
    return metres;
}

function ConvertMetreToFeet(_this) {

    var needRoundOff = 0;
    var metres = _this;
    var metreInches = metres * 39.370078740157477;

    //to prevent 9'0" from getting converted to 8'11"
    needRoundOff = metreInches % 1;
    if (needRoundOff >= .99) {
        metreInches = Math.ceil(metreInches);
    }

    var feet = parseInt(metreInches / 12);
    var inches = Math.floor(metreInches % 12);
    var result = feet + '\'' + inches + '\"';
    return result;


}

$('body').on('blur', '#divAllComponent input,#divAllComponent textarea,#regDetailsConfig input', function () {
    if (saveVehicleButtonClicked) {
        saveVehicleButtonClicked = false;
    }
    else {
        ConfigAutoFill(this);
        if ($(this).attr('id') == "Weight") {
            var compId = $(this).closest(".comp").attr('id');
            var weight = $(this).val()
            AxleValidationCalculationV1(weight, compId);
        }
    }
});

function LoadAxleSection(ComponentDetailList,isAxleLoad = true) {
    var componentId = ComponentDetailList.ComponentId;
    var numberOfAxles = $('#' + componentId).find('#Number_of_Axles').val() == "" ? 0 : $('#' + componentId).find('#Number_of_Axles').val();
    var compWeight = $('#' + componentId).find('#Weight').val();
    var isFromConfig = $('#HiddenFromConfig').val();

    var componentTypeId = parseInt($('#' + componentId).find('#vehicleTypeValue').val());
    var compSubId = parseInt($('#' + componentId).find('#vehicleSubTypeValue').val());
    var movementId = $('#movementTypeId').val();

    if (componentId == "") { componentId = 0; }
    if (componentTypeId == null) { componentTypeId = 0; }
    if (compSubId == null) { compSubId = 0; }

    var isEdit = $('#IsEdit').val();
    if (isEdit == '') {
        isEdit = false;
    }
    var isLastComponent = false;
    if ($('#' + componentId + ' #IsLastComponent').val().toLowerCase() == "true") {
        isLastComponent = true;
    }
    var guid = $('#GUID').val();

    var axleList;

    if (typeof AxleTableMethods != 'undefined') {
        axleList = AxleTableMethods.GetAxlesByComponentId(componentId, false);
    }

    var data = {
        axleCount: numberOfAxles, componentId: componentId,
        vehicleSubTypeId: compSubId, vehicleTypeId: componentTypeId, movementId: movementId,
        weight: compWeight, IsEdit: true, isFromConfig: isFromConfig, axles: axleList, isLastComponent: isLastComponent, GUID: guid
    };
    var url = '../VehicleConfig/AxleComponent';
    
    $.ajax({
        url: url,
        data: data,
        type: 'POST',
        async: false,
        success: function (response) {
            $('#' + componentId + ' #axlePage').html(response);
            HeaderHeight(componentId);
            AxleTableMethods.ShowHideViewAllAxlePopUpButtonInConfig();
            Comp_AxleInit(0, componentId);
            if (isAxleLoad) {
                if (axleList!=null && (axleList.length == 0 || axleList[0].TyreCenters == null || axleList[0].TyreCenters == ""))
                    $('#' + componentId+' .individualComponentAxle .axlewrapper input:text').blur();
            }
            $('.dyntitle').text('Edit axle');

            AxleTableMethods.CreateAxlesObj(componentId, false);
            AxleTableMethods.AdjustSingleComponentAxleTableWidth();            
        },
        error: function () {

        },
        complete: function () {
        }
    });
}

$('body').on('change', '.component_axle #tbl_Axle .axleweight', function () {
    MaxAxleWeightUpdate(this);
});

$('body').on('change', '.popUpAxleDetails #tbl_Axle .axleweight', function () {
    var axleWeightArray = [];
    var axleweightMaxval = 0;
    axleweightsum = 0;
    if (axleweightsum == 0) {
        $('.popUpAxleDetails  #tbl_Axle').find('.axleweight').each(function () {
            var _thisVal = $(this).val() == "" ? 0 : $(this).val();
            axleweightsum = axleweightsum + parseFloat(_thisVal);
            axleWeightArray.push(parseFloat(_thisVal));
        });
    }

    axleweightMaxval = Math.max.apply(Math, axleWeightArray);
    if (parseFloat(axleweightMax) < parseFloat(axleweightMaxval)) {
        axleweightMax = axleweightMaxval;
    }
    if (axleweightMaxval != 0) {
        $('#VehicleConfigInfo').find('#AxleWeight').val(axleweightMax);
        //$('#VehicleConfigInfo').find('#AxleWeight').trigger('blur');
    }
});

$('body').on('change', '.component_axle #tbl_Axle .disttonext', function () {
    if ($('#ConfigTypeId').val() == "244012") {
        var disttonextsum = 0;
        if (disttonextsum == 0) {
            $('.component_axle #tbl_Axle').find('.disttonext').each(function () {
                var _thisVal = $(this).val() == "" ? 0 : $(this).val();
                disttonextsum = disttonextsum + parseFloat(_thisVal);
            });
        }

        if (disttonextsum != 0) {
            disttonextsum = disttonextsum.toFixed(2);
            $('#VehicleConfigInfo').find('#Wheelbase').val(disttonextsum);
            $('#VehicleConfigInfo').find('#Wheelbase').trigger('blur');
        }
    }
});

function ShowCurrentHideHeaderTyreSpace(div) {
    if ($(div + ' .wheel_space').length == 0 && $(div +' .nowheels').val() == "") {
        $(div +' .headgrad2').hide();
        $(div +' .sub1').hide();
    }
    else {
        $(div +' .headgrad2').show();
        $(div +' .sub1').show();
    }

    if ($(div +' .tyre_size').length == 0) {
        $(div +' .headgrad_tyreSize').hide();
    }
    else {
        $(div +' .headgrad_tyreSize').show();
    }

    var greatestVal = 0;
    $(div + ' .nowheels').each(function () {
        var _thisWheelVal = $(this).val();
        if (greatestVal < parseInt(_thisWheelVal)) {
            greatestVal = parseInt(_thisWheelVal);
        }
    });
    $(div + ' .headgrad2').attr('colspan', greatestVal - 1);    

    $(div + ' .headgrad1').remove();
    for (var j = 1; j < greatestVal; j++) {
        $(div + ' .sub').append('<th class="headgrad1">' + j + '</th>');
        if (j == greatestVal - 1) {
            $(div + ' .sub').append('<th style="background-color: rgb(213 223 234) !important;" class="headgrad1 5"></th>');
        }
        $(div + ' .headgrad2').show();
    }

    //if ((headLength + 1) < greatestVal) {
    //    $(div + ' .headgrad2').attr('colspan', greatestVal - 1);
    //    $(div + ' .centerspace1').attr('colspan', greatestVal - 1);
    //}
    //else {

    //}

}

function calculateWheelBase(componentId) {
    var configTypeId = $('#ConfigTypeId').val();
    var unitvalue = $('#UnitValue').val();
    var wb = 0;
    var drawbar_wb = 0;
    var count = $('#' + componentId + ' #tbl_Axle tbody tr').length;
    var i = 0;
    $('#' + componentId + ' #tbl_Axle .txt-distancetoaxle').each(function () {
        i++;
        var this_txt = $(this).val();
        if (this_txt != undefined && this_txt != "") {
            var distanceToNxtAxl = this_txt;
            if (unitvalue == 692002) {
                distanceToNxtAxl = ConverteFeetToMetre(distanceToNxtAxl) != null ? ConverteFeetToMetre(distanceToNxtAxl) : 0;
            }
            if (count != i)
                drawbar_wb = parseFloat(wb) + parseFloat(distanceToNxtAxl);

            wb = parseFloat(wb) + parseFloat(distanceToNxtAxl);
        }
    });
    $('#' + componentId + ' #Wheelbase').val(wb);
    if (configTypeId == VEHICLE_CONFIGURATION_TYPE_CONFIG.DRAWBAR_TRAILER || configTypeId == VEHICLE_CONFIGURATION_TYPE_CONFIG.DRAWBAR_TRAILER_3_TO_8) {
        $('#VehicleConfigInfo #Wheelbase').val(drawbar_wb);
    }
    else {
        configWheelbase += wb;
        $('#VehicleConfigInfo #Wheelbase').val(configWheelbase);
    }
}

function GetTrailerWeghtForBoatMast() {
    var trailerWeight = 0;
    $('.comp').each(function () {
        var componentId = $(this).attr("id");
        var IsTractor = false;
        if ($('#' + componentId).find('#Tractor').val().toLowerCase() == "true") {
            IsTractor = true;
        }
        if (IsTractor == false) {
            trailerWeight= $('#' + componentId).find('#Weight').val();
        }
    });
    return trailerWeight;
}

function LoadComponentRegistration(ComponentDetailList) {
    var componentId = ComponentDetailList.ComponentId;
    var countrw = 0;
    var compTable = $("#" + componentId + " .div_reg_component_vehicle");
    for (var i = 0; i < ComponentDetailList.ComponentRegistrationList.length; i++) {
        var oldRegId = "";
        var oldFleetId = "";
        var regHtml = $(compTable).find('.tr_Registration')[countrw];
        if ($(regHtml).length > 0) {
            oldRegId = $(regHtml).find('.txt_register').html();
            oldFleetId = $(regHtml).find('.txt_fleet').html();
        }
        countrw++;
        var regId = ComponentDetailList.ComponentRegistrationList[i].RegistrationValue;
        var fleetId = ComponentDetailList.ComponentRegistrationList[i].FleetId;
        if (oldRegId != regId && oldFleetId != fleetId) {
            var HTMLTr = "<tr id='row" + countrw + "' class='tr_Registration'><td id='registerId" + countrw + "' class='cls_regId txt_register  tblregcomponent'>" + regId + "</td><td id='fleetId" + countrw + "' class='cls_fleetId txt_fleet tblregcomponent'>" + fleetId + "</td><td><a href='javascript:void(0)' RowId=" + countrw + " class='delete btngrad btnrds btnbdr tdbutton deleteCompRegNew'></a ></td></tr>";
            compTable.find('tr:last').before(HTMLTr)
        }
    }
}

function MaxAxleWeightUpdate(_this) {
    var componentCount = $('#ComponentCount').val();
    var axleWeightArray = [];
    var axleweightMaxval = 0;
    axleweightsum = 0;
    var isAxleEmpty = false;
    var componentId = $(_this).closest('.AxleComponent').closest('.comp').attr('id');
    if (componentCount == "1")
        componentId = $('.comp').attr('id');
    AxleTableMethods.CreateAxlesObj(componentId);
   
    $('.component_axle #tbl_Axle').find('.axleweight').each(function () {
        var _thisVal = $(this).val() == "" ? 0 : $(this).val();
        if (_thisVal == 0) {
            isAxleEmpty = true;
            return;
        }
        axleweightsum = axleweightsum + parseFloat(_thisVal);
        axleWeightArray.push(parseFloat(_thisVal));
    });
    
    if ($('.component_axle #tbl_Axle').length != componentCount)
        isAxleEmpty = true;

    var configMaxAxleWeight = $('#VehicleConfigInfo').find('#AxleWeight').val();
    if (axleWeightArray.length > 0) {
        axleweightMaxval = Math.max.apply(Math, axleWeightArray);
        if (parseFloat(axleweightMax) < parseFloat(axleweightMaxval)) {
            axleweightMax = axleweightMaxval;
        }
    }
    if (!isAxleEmpty) {      
        if (axleweightMaxval != 0) {
            $('#VehicleConfigInfo').find('#AxleWeight').val(axleweightMaxval);
        }
        if (_this != undefined && _this != "") {
            if ($('#MovementTypeId').val() == "270006" && ($('#ConfigTypeId').val() == "244001" || $('#ConfigTypeId').val() == "244002")) {
                var compId = $(_this).closest('.AxleComponent').closest('.comp').attr('id')
                var groundClr = $('#' + compId).find("#Ground_Clearance");
                var labelcmpdiv = $(groundClr).parent(".input-field").parent(".dynamic").find(".labelCompDiv");
                if (axleweightsum > 150000 && ($('#' + compId).find('#ComponentType_Id').val() == "234002"
                    || $('#' + compId).find('#ComponentType_Id').val() == "234005")) {
                    $(groundClr).attr("isrequired", 1);
                    $(labelcmpdiv).html('');
                    $(labelcmpdiv).append('<label class="text-normal"><b>Ground Clearance <span> *</span></b></label>');
                }
                else if (axleweightsum <= 150000 && ($('#' + compId).find('#ComponentType_Id').val() == "234002"
                    || $('#' + compId).find('#ComponentType_Id').val() == "234005")) {
                    $(groundClr).attr("isrequired", 0);
                    $(labelcmpdiv).html('');
                    $(labelcmpdiv).append('<label class="text-normal">Ground Clearance</label>');
                }
            }
        }
        
        MovementAssessment(false, false);
    }
    else {
        if (axleweightMaxval != 0 && axleweightMaxval > parseFloat(configMaxAxleWeight)) {
            $('#VehicleConfigInfo').find('#AxleWeight').val(axleweightMaxval);
            MovementAssessment(false, false);
        }
    }
}

function ValidateMaxAxleWeightOnSave() {
    var axleWeightArray = [];
    var axleweightMaxval = 0;
    var isAxleWeightValid = true;
    var configTypeId = $('#ConfigTypeId').val();
    $('.component_axle #tbl_Axle').find('.axleweight').each(function () {
        var _thisVal = $(this).val() == "" ? 0 : $(this).val();
        axleWeightArray.push(parseFloat(_thisVal));
    }).promise().done(function () {
        if (axleWeightArray.length > 0) {
            axleweightMaxval = Math.max.apply(Math, axleWeightArray);
            var configMaxAxleWeight = $('#VehicleConfigInfo').find('#AxleWeight').val();
           if (configMaxAxleWeight != "" && parseFloat(configMaxAxleWeight) != axleweightMaxval) {
                isAxleWeightValid = false;
                showMultiToastMessage({
                    message: "Heaviest Axle Weight doesn’t match with the components max axle weight value!",
                    type: "error"
                });
            }
        }
    });

    return isAxleWeightValid;
}

function IsGroundClearenceRequired(weight, componentId) {
    var mvmntTypeId = $('#MovementTypeId').val();
    if (mvmntTypeId == 270006 && weight > 150000) {
        if (componentId != undefined) {
            var compElem = $('#' + componentId + ' #Ground_Clearance');
            $(compElem).attr("isrequired", 1);
            var labelcmpdiv = $(compElem).parent(".input-field").parent(".dynamic").find(".labelCompDiv");
            var labelcmpText = $(labelcmpdiv).text().trim();
            if (labelcmpText.indexOf("*") < 0) {
                $(labelcmpdiv).html('');
                $(labelcmpdiv).append('<label class="text-normal"><b>' + labelcmpText + '<span> *</span></b></label>');
            }
        }
        else {
            var configElem = $('#VehicleConfigInfo #Ground_Clearance');
            $(configElem).attr("isrequired", 1);
            var labelconfigdiv = $(configElem).parent(".input-field").parent(".dynamic").find(".labelDiv");
            var labelconfigText = $(labelconfigdiv).text().trim();
            if (labelconfigText.indexOf("*") < 0) {
                $(labelconfigdiv).html('');
                $(labelconfigdiv).append('<label class="text-normal"><b>' + labelconfigText + '<span> *</span></b></label>');
            }
        }
    }
    else if (mvmntTypeId == 270006 && weight <= 150000) {
        if (componentId != undefined) {
            var compElem = $('#' + componentId + ' #Ground_Clearance');
            $(compElem).attr("isrequired", 0);
            var labelcmpdiv = $(compElem).parent(".input-field").parent(".dynamic").find(".labelCompDiv");
            var labelcmpText = $(labelcmpdiv).text().trim();
            if (labelcmpText.indexOf("*") > 0) {
                labelcmpText = labelcmpText.replace('*', '');
                $(labelcmpdiv).html('');
                $(labelcmpdiv).append('<label class="text-normal">' + labelcmpText + '</label>');
            }

        }
        else {
            var configElem = $('#VehicleConfigInfo #Ground_Clearance');
            $(configElem).attr("isrequired", 0);
            var labelconfigdiv = $(configElem).parent(".input-field").parent(".dynamic").find(".labelDiv");
            var labelconfigText = $(labelconfigdiv).text().trim();
            if (labelconfigText.indexOf("*") > 0) {
                labelconfigText = labelconfigText.replace('*', '');
                $(labelconfigdiv).html('');
                $(labelconfigdiv).append('<label class="text-normal">' + labelconfigText + '</label>');
            }
        }
    }
}

function IterateThroughComponent() {
    $('.dynamic input:text').each(function () {
        if (IsLengthField(this)) {
            ConvertRangeToFeet(this);
        }
    });
}
function ShowFeetComponent() {
    $('.dynamic input:text').each(function () {

        if (IsPreference()) {

            if (IsLengthField(this)) {
                var data = $(this).val();
                if (data.indexOf('\'') === -1) {
                    data = ConvertToFeet(data);
                    if (data != "0\'0\"" && data != "") {
                        $(this).val(data);
                    }
                    else {
                        $(this).val("");
                    }
                }
            }
        }
    })
}

$('body').on('change', '#divAllComponent input,#divAllComponent textarea,#regDetailsConfig input', function () {
    var currentValue = $(this).val();
    var previousValue = $(this).attr('previousvalue');
    if (previousValue == "0")
        previousValue = "";

    if (componentPreviousval.length == 0) {
        componentPreviousval.push({ fieldId: $(this).attr("id"), fieldVal: previousValue });
    }
    else {
        for (var i = 0; i < componentPreviousval.length; i++) {
            if (componentPreviousval[i].fieldId == $(this).attr("id")) {
                componentPreviousval[i].fieldVal = previousValue;
            }
            else {
                componentPreviousval.push({ fieldId: $(this).attr("id"), fieldVal: previousValue });
            }
        }
    }

    $(this).attr('previousvalue', currentValue);
});