var selectedDiv = 0;
var lastDiv;
var clearnavigation = true;
var vehicleCompList; //= [{}];
var vehicleCompNum;
var compSubTypeList = new Array();
var compId;
var compAppId;
var compVR1Id;
var componentName;
var AppComponentName;
var VR1ComponentName;
var flag;
var compTypeTemp;
var vehCompList;
isCompEdit = false;
$(function () {
	//GetComponentList method returns the list of vehicleComponent
	//vehicleCompList = GetComponentList();   
	//$.when(GetComponentList()).then(SetDataAndImages());

	$('body').on('click', '.btn-navigate-to-spec-component', function (e) {
		var indexVal = $(this).data('indexval')
		NavigateToSpecComponent(indexVal);
	});
	$('body').on('click', '.btn-delete-comp-from-config', function (e) {
		DeleteCompFromConfig();
	});
	$('body').on('click', '.btn-edit-component-summ', function (e) {
		var compId = $(this).data('compid')
		EditComponent(compId);
	});
	$('body').on('click', '.btn-add-to-fleet', function (e) {
		var compId = $(this).data('compid');
		var compname = $(this).data('compname');
		AddToFleet(compId, compname);
	});
});
function SetDataAndImages() {
   
	if (typeof (GetDropDownNumSecond) == 'function') {
		vehicleCompNum = GetDropDownNumSecond();
	}
	else {
		vehicleCompNum = GetCompNum();
	}
	
	if (vehicleCompNum == undefined || vehicleCompNum == null || vehicleCompNum == '') {
		vehicleCompNum = $('#ComponentNum').val();
	}

	var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
	var isVR1 = $('#vr1appln').val();
	var isNotify = $('#ISNotif').val();
	if (isNotify == 'True' || isNotify == 'true') {
		isVR1 = 'True';
	}
   
	if (ApplicationRevId == 0 && !isVR1) {
		AddImagesAtTop();
	}
	else if (isVR1 == 'True') {
		AddVR1ImagesAtTop();
	}
	else {
		AddAppImagesAtTop();
	}

	AlignSideBySide();
}
//function to navigate back to registration page
function NavigateBackRegPage() {
	if (selectedDiv == 0) {
		$('#div_Registration').show();
		$("#div_Summary").html('');
	}
	else {
		ConfigureBackComponent();
	}
}
function ConfigureAndProceedBtn() {

	$('.pop-message').html('');
	$('.box_warningBtn1').html('');
	$('.box_warningBtn2').html('');
	$('#pop-warning').hide();

	$('.box_warningBtn1').unbind();
	$('.box_warningBtn2').unbind();

	ConfigureBackComponent();
}
//function to navigate to next component
function ConfigureNextComponent() {
    var movementId = $('#MovementClassConfig').val();
    var isAxleReqrd = $('#IsAxleConfigRqrd').val();
	if (selectedDiv != lastDiv) {
		var nextDiv = selectedDiv + 1;
		var axleSpacing = $('#DistanceToNxtAxle').val();
		$('#div_Summary').find('#div_' + selectedDiv + '').removeClass('vehiclecomp_active');
		
		if (!CheckSideBySideLastComp() && movementId != 270001 && (isAxleReqrd == 'undefined' || isAxleReqrd == 'True')) {
			if (axleSpacing == null || axleSpacing == 0 || axleSpacing == 'undefined' || axleSpacing == '') {
				showWarningPopDialog('Enter \'Axle spacing to following \' by editing component details before proceeding to next component', 'Ok', '', 'ConfigureAndProceedBtn', '', 1, 'info');
			}
		}

		$('#div_Summary').find('#div_' + nextDiv + '').addClass('vehiclecomp_active');
		selectedDiv++;
	}

	else {
		//location.reload();
	}
	ButtonFinishShow();
	var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
	var isVR1 = $('#vr1appln').val();
	var isNotify = $('#ISNotif').val();
	if (isNotify == 'True') {
		AjaxGetComponentDescFromVR1();  //mirza1233
	}
	else if (ApplicationRevId == 0 && !isVR1) {
		AjaxGetComponentDesc();
	}
	else if (isVR1 == 'True') {
		AjaxGetComponentDescFromVR1();
	}
	else {
		AjaxGetComponentDescFromApplication();

	}
}
//function to navigate back to last component
function ConfigureBackComponent() {
	
	var lastDiv = selectedDiv - 1;
	$('#div_Summary').find('#div_' + selectedDiv + '').removeClass('vehiclecomp_active');
	$('#div_Summary').find('#div_' + lastDiv + '').addClass('vehiclecomp_active');
	selectedDiv--;
	ButtonFinishShow();
	//LoadSelectedGrid();
	var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
	var isVR1 = $('#vr1appln').val();
	var isNotify = $('#ISNotif').val();
	if (isNotify == 'True') {
		AjaxGetComponentDescFromVR1();
	}
	else if (ApplicationRevId == 0 && !isVR1) {
		AjaxGetComponentDesc();
	}
	else if (isVR1 == 'True') {
		AjaxGetComponentDescFromVR1();
	}
	else {
		AjaxGetComponentDescFromApplication();
	}
}
//function to find total div number of div class div_img_line
function FindLastDiv() {
	lastDiv = $('.div_img_line').length;
	//set finish button if it is last component of configuration
	ButtonFinishShow();
	//var componentId = $('#div_Summary').find('#div_' + selectedDiv + '').attr('compId');
	//LoadGrid(componentId);
}
//function to show next and finish button
function ButtonFinishShow() {    
	if (selectedDiv == (lastDiv-1)) {
		$('#btn_summ_finish').show();
		$('#btn_summ_Next').hide();
	}
	else {
		$('#btn_summ_finish').hide();
		$('#btn_summ_Next').show();
	}
}
//function to fire finish button event
function ButtonFinish() {
	//$('#AxleDistanceSum').val('');
	var continueProcess = true;
	var sumofaxlspace = true;
	var vehicleName = GetVehicleName();
	var vehicleConfigId = GetConfigurationId();
	var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
	var isVR1 = $('#vr1appln').val() ? $('#vr1appln').val() : 0;
	var isNotify = $('#ISNotif').val() ? $('#ISNotif').val() : 0;
	var notifid = $('#NotificatinId').val();;
	var isCandVhcl = $('#IsCandVehicle').val() ? $('#IsCandVehicle').val() : 0;
	var isEdit = $('#IsEditVehicle').val();
	if (isCandVhcl != 'True') {
		if (isNotify == 'True' || isNotify == 'true') {
			continueProcess = IsValidNewVehicle(vehicleConfigId);
		}
	}
	if (isCandVhcl ==0 && isVR1 == 0 && isNotify == 0 && ApplicationRevId ==  0) {
		// to obtain the sum of distance to next axle
		sumofaxlspace = CheckwheelbaseOvrLen(vehicleConfigId, ApplicationRevId, isNotify, isVR1);
	}
	if (continueProcess == true) {
	    if (sumofaxlspace == true) {
	      
			$.ajax({
				url: '../VehicleConfig/UpdateVehicleOnFinish',
				type: 'POST',
				cache: false,
				async: false,
				data: { configId: vehicleConfigId, applnRev: ApplicationRevId, isNotif: isNotify, isVR1: isVR1, NotificationId: notifid, isEdit: isEdit },
				success: function (result) {
				    finishPopUp();
				    $('#IsEdit').val(false);
				},
				error: function (xhr, textStatus, errorThrown) {
					//other stuff
				},
				complete: function () {
					$('#span-close').show();
				}
			});
		} else {
			$('#overlay').show();
			$('#dialogue').show();
			$('#span-close').hide();
			showWarningPopDialog('Sum of axle spacing should be less than vehicle length', 'Ok', '', 'WarningCancelBtn', '', 1, 'error');
		}
	}
	else {
		$('#overlay').show();
		$('#dialogue').show();
		showWarningPopDialog('Components missing.Create or import component to complete the process', 'Ok', '', 'WarningCancelBtn', '', 1, 'error');
	}
	
}
function finishPopUp() {
	var vehicleName = GetVehicleName();
	var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
	var isVR1 = $('#vr1appln').val();
	var isNotify = $('#ISNotif').val();

	if (isNotify == 'True') {

		var sortstaus = $('#SortStatus').val();
		if (sortstaus == "CandidateRT") {
			
			showWarningPopDialog('Configuration  "' + vehicleName + '"  saved successfully', 'Ok', '', 'Show_CandidateRTVehicles', '', 1, 'info');
		}
		else {
   
			showWarningPopDialog('Configuration  "' + vehicleName + '"  saved successfully', 'Ok', '', 'LoadNotificationVehicleList', '', 1, 'info');
		}
	}
	else if (ApplicationRevId == 0 && !isVR1) {
	   
		showWarningPopDialog('Configuration  "' + vehicleName + '"  saved successfully', 'Ok', '', 'ReloadLocation', '', 1, 'info');
	}
	else if (isVR1 == 'True') {


		//showPopUpDialog('Configuration  "' + vehicleName + '"  saved successfully', 'Ok', '', CloseVehicle, '', 1, 'info');
		showWarningPopDialog('Configuration  "' + vehicleName + '"  saved successfully', 'Ok', '', 'CloseVehicle','', 1, 'info');
	}
	else {

		showWarningPopDialog('Configuration  "' + vehicleName + '"  saved successfully', 'Ok', '', 'CloseVehicle', '', 1, 'info');
	}
}
// to obtain the sum of distance to next axle with overall length
function CheckwheelbaseOvrLen(vehicleId, appl_id, is_notif, is_vrl1) {
	var resume = true;
	//retrieve vehicle details
	$.ajax({

		url: '../VehicleConfig/Checkwheelwithsumaxel',
		type: 'POST',
		async: false,
		data: { VehicleID: vehicleId, applnRev: appl_id, isNotif: is_notif, isVR1: is_vrl1 },
		beforeSend: function () {
			startAnimation();
		},
		success: function (result) {
			
				resume = result.result;
		},
		error: function (xhr, textStatus, errorThrown) {
			//other stuff
			location.reload();
		},
		complete: function () {
			stopAnimation();
		}
	});
	return resume;
}
function CloseVehicle() {
	WarningCancelBtn();
	CloseModelPop();
	$("#overlay").hide();
	addscroll();
	//startAnimation();
	var vehid = $('#VehicleID').val();
	var editveh1 = $('#editveh').val();
	var notifid = $('#NotificatinId').val();
	var vehcode = $('#VehicleClass').val();
	var vehname = $('#VehicleName').val();

	//if (editveh1 == 1) {
	//    $.ajax({
	//        type: "GET",
	//        url: "../Notification/SaveGeneralDet_SimpleNotif",
	//        data: { VehicleClassCode: vehcode, VehicleName: vehname, IsUpdate: 1, NotificationId: notifid, IsClone: 1, VehicleId: vehid },
	//        beforeSend: function () {
	//            startAnimation();
	//        },
	//        success: function (page) {
	//            $('#simplenote').html(page);
	//        },
	//        complete: function () {
	//            stopAnimation();
	//        }
	//    });

		
	//    $('#tab_3').hide();
	//    $("#leftpanel").hide();
	//}
	if (editveh1 != 1) {
		LoadSelectedFleetConfiguration(function () {
			//stopAnimation();
		});
	}
	$('#dialogue').html('');
}
//function to add number of images
function AddImagesAtTop() {
	var latPosition;
	var longPosition;
	var totlength = vehicleCompList[vehicleCompNum].length;
	var title = '';
	for (var i = 0; i < totlength; i++) {
		title = vehicleCompList[vehicleCompNum][i].ComponentName;
		if (GetLatLongPos().length>0) {
			
			latPosition = GetLatLongPos()[i].LatPos;
			longPosition = GetLatLongPos()[i].LongPos;

		}
		else {
			
			latPosition = i + 1;
			longPosition = 1;
		}
		
		//exceptional cases
		var movementId = $('#MovementClassConfig').val();
		if (movementId == 270005) {
			if (i == 0) {
				vehicleCompList[vehicleCompNum][0].ImageName = "recoveryvehicle";
				title = 'recovery vehicle';
			}
			else {
				vehicleCompList[vehicleCompNum][i].ImageName = "recoveredvehicle";
				title = 'recovered vehicle';
			}
		}
		else if (movementId == 270003)
		{
			title = 'Mobile Crane';
		}

		//-----------------------------------------
		
		if (i == 0) {
			$('#div_img_wrap').append('<div class="centre"  title="' + vehicleCompList[vehicleCompNum][i].ComponentName + '" ><div class="div_img_line vehiclecomp_active" id="div_' + i + '" compId="' + vehicleCompList[vehicleCompNum][i].ComponentTypeId + '" compTypeName="' + vehicleCompList[vehicleCompNum][i].ComponentName + '" number="' + i + '" latPos="' + latPosition + '" longPos="' + longPosition + '"><div class="vehiclecomp_div cursor" ><div data-indexval='+i+' class="btn-navigate-to-spec-component vehiclecomponent ' + vehicleCompList[vehicleCompNum][i].ImageName + '"  title="' + title + '"></div></div></div></div>');
		}
		else {
			$('#div_img_wrap').append('<div class="centre"  title="' + vehicleCompList[vehicleCompNum][i].ComponentName + '" ><div class="div_img_line" id="div_' + i + '" compId="' + vehicleCompList[vehicleCompNum][i].ComponentTypeId + '" compTypeName="' + vehicleCompList[vehicleCompNum][i].ComponentName + '" number="' + i + '" latPos="' + latPosition + '" longPos="' + longPosition + '"><div class="vehiclecomp_div cursor" ><div data-indexval=' + i +' class="btn-navigate-to-spec-component vehiclecomponent ' + vehicleCompList[vehicleCompNum][i].ImageName + '"  title="' + title + '" ></div></div></div></div>');
		}
	}
	//AlignSideBySide();

	FindLastDiv();
	AjaxGetComponentDesc();
}
//function to add number of images
function AddAppImagesAtTop() {
	var latPosition;
	var longPosition;
	var totlength = vehicleCompList[vehicleCompNum].length;
	var title = '';
	for (var i = 0; i < totlength; i++) {
		title = vehicleCompList[vehicleCompNum][i].ComponentName;
		if (GetLatLongPos().length > 0) {            
			latPosition = GetLatLongPos()[i].LatPos;
			longPosition = GetLatLongPos()[i].LongPos;
			
		}
		else {
			
			latPosition = i + 1;
			longPosition = 1;
		}


		//exceptional cases
		var movementId = $('#MovementClassConfig').val();
		if (movementId == 270005) {
			if (i == 0) {
				vehicleCompList[vehicleCompNum][0].ImageName = "recoveryvehicle";
				title = 'recovery vehicle';
			}
			else {
				vehicleCompList[vehicleCompNum][i].ImageName = "recoveredvehicle";
				title = 'recovered vehicle';
			}
		}
		else if (movementId == 270003) {
			title = 'Mobile Crane';
		}

		//-----------------------------------------



		if (i == 0) {
			$('#div_img_wrap').append('<div class="centre"  title="' + vehicleCompList[vehicleCompNum][i].ComponentName + '" ><div class="div_img_line vehiclecomp_active" id="div_' + i + '" compId="' + vehicleCompList[vehicleCompNum][i].ComponentTypeId + '" compTypeName="' + vehicleCompList[vehicleCompNum][i].ComponentName + '" number="' + i + '" latPos="' + latPosition + '" longPos="' + longPosition + '"><div class="vehiclecomp_div cursor" ><div data-indexval=' + i +' class="btn-navigate-to-spec-component vehiclecomponent ' + vehicleCompList[vehicleCompNum][i].ImageName + '" title="' + title + '"></div></div></div></div>');
		}
		else {
			$('#div_img_wrap').append('<div class="centre"  title="' + vehicleCompList[vehicleCompNum][i].ComponentName + '" ><div class="div_img_line" id="div_' + i + '" compId="' + vehicleCompList[vehicleCompNum][i].ComponentTypeId + '" compTypeName="' + vehicleCompList[vehicleCompNum][i].ComponentName + '" number="' + i + '" latPos="' + latPosition + '" longPos="' + longPosition + '"><div class="vehiclecomp_div cursor" ><div data-indexval=' + i +' class="btn-navigate-to-spec-component vehiclecomponent ' + vehicleCompList[vehicleCompNum][i].ImageName + '" title="' + title + '" ></div></div></div></div>');
		}
	}
	//AlignSideBySide();

	FindLastDiv();
	AjaxGetComponentDescFromApplication();

	
}
//function to add number of images
function AddVR1ImagesAtTop() {
	//var s = document.getElementById(vehicleCompList[vehicleCompNum]);	
	var latPosition;
	var longPosition;
	var title = '';

	var movementId = $('#MovementClassConfig').val();
	var isNotify = $('#ISNotif').val();

	if (movementId == 270003 && (isNotify == 'False' || isNotify == 'false')) {           
			vehicleCompNum = 6;//to select mobile crane
		}
	
		var sLength = vehicleCompList.length;
	if (sLength == 0)
	{
	var totlength = 0;
	}
	else{
		var vehicleVal = vehicleCompList[vehicleCompNum];
		var totlength = vehicleVal.length;
		}
	
	for (var i = 0; i < totlength; i++) {
		title = vehicleCompList[vehicleCompNum][i].ComponentName;
		if (GetLatLongPos().length > 0) {
			
			latPosition = GetLatLongPos()[i].LatPos;
			longPosition = GetLatLongPos()[i].LongPos;

		}
		else {
			latPosition =  i+1; 
			longPosition = 1;
			
		}


		//exceptional cases
		if (movementId == 270005) {
			if (i == 0) {
				vehicleCompList[vehicleCompNum][0].ImageName = "recoveryvehicle";
				title = 'recovery vehicle';
			}
			else {
				vehicleCompList[vehicleCompNum][i].ImageName = "recoveredvehicle";
				title = 'recovered vehicle';
			}
		}
		else if (movementId == 270003) {
			title = 'Mobile Crane';
		}
		//-----------------------------------------



		if (i == 0) {
			$('#div_img_wrap').append('<div class="centre"  title="' + vehicleCompList[vehicleCompNum][i].ComponentName + '" ><div class="div_img_line vehiclecomp_active" id="div_' + i + '" compId="' + vehicleCompList[vehicleCompNum][i].ComponentTypeId + '" compTypeName="' + vehicleCompList[vehicleCompNum][i].ComponentName + '" number="' + i + '" latPos="' + latPosition + '" longPos="' + longPosition + '"><div class="vehiclecomp_div cursor" ><div data-indexval=' + i +' class="btn-navigate-to-spec-component vehiclecomponent ' + vehicleCompList[vehicleCompNum][i].ImageName + '" title="' + title + '"></div></div></div></div>');
		}
		else {
			$('#div_img_wrap').append('<div class="centre"  title="' + vehicleCompList[vehicleCompNum][i].ComponentName + '" ><div class="div_img_line" id="div_' + i + '" compId="' + vehicleCompList[vehicleCompNum][i].ComponentTypeId + '" compTypeName="' + vehicleCompList[vehicleCompNum][i].ComponentName + '" number="' + i + '" latPos="' + latPosition + '" longPos="' + longPosition + '"><div class="vehiclecomp_div cursor" ><div data-indexval=' + i +' class="btn-navigate-to-spec-component vehiclecomponent ' + vehicleCompList[vehicleCompNum][i].ImageName + '" title="' + title + '" ></div></div></div></div>');
		}
	}
	//AlignSideBySide();

	FindLastDiv();
	AjaxGetComponentDescFromVR1();

   
}
//function Load Grid
function LoadGrid(compType) {
	
	//@Html.Hidden("isVR1", false, new { id="hidden_isVR1"})
	
	var isVR1 = $('#vr1appln').val();
	compTypeTemp = compType;

	$('#hidden_compType_grid').remove();
	$('#form_fleet_grid').append('<input type="hidden" name="compType" id="hidden_compType_grid" value="' + compType + '"><input type="hidden" name="isVR1" id="hidden_isVR1" value="' + isVR1 + '">');
	$('#form_fleet_grid').submit();
	//return false;
	//$.ajax({
	//    url: "../VehicleConfig/FleetComponentList",
	//    data: '{ "compType":' + JSON.stringify(compType) + '}',
	//    type: 'POST',
	//    async: false,
	//    contentType: 'application/json; charset=utf-8',
	//    success: function (data) {
			
	//        $('#div_tbl_grid').html(data);
	//        //compTypeName = compType;
			
	//        $('#hidden_compType').remove();
	//        $('#form_fleet_pagination').append('<input type="hidden" name="compType" id="hidden_compType" value="' + compType + '">');
			
	//        compTypeTemp = compType;
	//        //btn_create_component
	//        $('#btn_create_component').show();
	//        $('.summaryinfo').show();
	//    },
	//    error: function () {
	//        location.reload();
	//    }
	//});
}
//function Get ComponentList of single ComponentId
function GetCompListbyNum() {
  
	//vehicleCompList[vehicleCompNum]
}
//function for create component
function CreateComponent() {
	//isCompEdit = true;
	$('.loading').show();
	var islast = false;
	if (selectedDiv == (lastDiv - 1)) {
		islast = true;
	}
	if (CheckSideBySideLastComp()) {
		islast = true;
	}
	var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
	$('#IsEdit').val(false);
   // if (ApplicationRevId==0)
   // {
		$('#div_create_component').load('../Vehicle/CreateComponent?isLastComponent=' + islast + '', function () {
			$('#div_Summary').hide();
			$('.head_component').remove();
			$('.form_component').find('.body').removeClass('body');

			fillDropCreateComponent();
			$('.loading').hide();

		
		});
	//}
	//else {

	//    $('#div_create_component').load('../Vehicle/InsertAppVehicleComponent?isLastComponent=' + islast + '', function () {
	//        $('#div_Summary').hide();
	//        $('.head_component').remove();
	//        $('.form_component').find('.body').removeClass('body');

	//        fillDropCreateComponent();
	//        $('.loading').hide();


	//    });
	//}
}
//function to select dropdown values of create component
function fillDropCreateComponent() {
	var movementId = $('#MovementClassConfig').val();
	var vehicleTypeId = $('#div_Summary').find('#div_' + selectedDiv + '').attr('compId'); 

	//$('#Movement').val(movementId).change();
	//$('#Movement').attr('disabled', 'disabled');
	////SECOND DROPDOWN IS FILLED WITH A DELAY TO COMPLETE LOADING THE DATA AFTER FIRST DROPDOWN CHANGE IS TRIGGERED
	//setTimeout(TriggerVehicleType, 200);

	$('#Movement').val(movementId);
	$('#VehicleSubType option').remove();
	$('#componentDetails').hide("Blind");
	$('#componentDetails').html("");
	if (movementId == "") {
		$('#VehicleType option').not('option:eq(0)').remove();
		$('#VehicleType').attr("disabled", "disabled");
		$('#VehicleSubType').attr("disabled", "disabled");
	}
	else {
		$('#Movement').attr('disabled', 'disabled');
		$('#VehicleType option:not(:first-child)').remove();
		var url = '../Vehicle/FillVehicleType';
		$.post(url, { movementId: movementId }, function (data) {
			var datalength = data.type.length;
			for (var i = 0; i < datalength; i++) {
				$('#VehicleType').append('<option value="' + data.type[i].ComponentTypeId + '">' + data.type[i].ComponentName + '</option>');
				$('#VehicleType').attr("disabled", false);
			}
			if (datalength > 0) {
				TriggerVehicleType();
			}
		});
	}
}
//function for Hiding Finish Button in Axle Page
function HideFinishButton() {
	$('#btn_axle_finish').hide();
	$('#btn_axle_Next').show();

	$('#btn_reg_finish').hide();
	$('#btn_reg_Next_Config').show();
	//btn_reg_finish
	//btn_reg_Next
	//btn_reg_Next_Config
}
//function to show back button from create component page
function ShowBackBtnComponent() {
	$('#btn_back_to_Config').show();
}
//function Navigate to summary page from axle
function NavigateToSummary() {
	var componentId = $('#Component_Id').val();
	//have to implement the save method with alert
	var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
	var isVR1 = $('#vr1appln').val();

	if (ApplicationRevId == 0 && !isVR1) {
		SaveComponentOnImport(componentId);
	}
	else if (isVR1 == 'True' || isVR1 == 'true') {
		SaveVR1ComponentOnImport(componentId);
	}
	else {
		SaveAppComponentOnImport(componentId);//added by ajit
	}
		
	BackToConfig();
}
//function navigate to summary from Registration
function NavigateToSummaryReg() {
	var componentId = $('#Component_Id').val();
	var isAxleRqrd = $('#IsConfigAxle').val();
	$('#IsAxleConfigRqrd').val(isAxleRqrd);

	//have to implement the save method with alert
	var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
	var isVR1 = $('#vr1appln').val();
	if (ApplicationRevId == 0) {
		SaveComponentOnImport(componentId);
		
	}
	else if (isVR1 == 'True' || isVR1 == 'true') {
		SaveVR1ComponentOnImport(componentId);
		
	}
	else {
		SaveAppComponentOnImport(componentId);//added by ajit
		
	}
	BackToConfig();
}
//function to navigate back to configuration from create component
function BackToConfig() {
	$('#div_create_component').html('');
	$('#div_Summary').show();
	$.ajax({
		url: "../Vehicle/ClearSession",
		type: 'POST',
		cache: false,
		async: false,
		success: function (result) {
		}
	});
}
//function trigger vehicle type
function TriggerVehicleType() {
	var vehicleTypeId = $('#div_Summary').find('#div_' + selectedDiv + '').attr('compId');
	$('#VehicleType').val(vehicleTypeId).trigger('change');
	$('#VehicleType').attr('disabled', 'disabled');
	 
	//$('#SetShowComponent').val(0);
}
//function to load Grid Based on component Selected
function LoadSelectedGrid() {
	
	var componentType = new Array();
	componentType = GetSubComponentTypes();

	LoadGrid(componentType);
}
//function to retrieve componentType
function GetComponentType() {
	//var componentType = $('#div_Summary').find('#div_' + selectedDiv + '').attr('comptypename');
	//var componentType = new Array();
	//componentType = GetSubComponentTypes();
	return compSubTypeList;
}
//function to save vehicle component on import
function SaveComponentOnImport(componentId) {
	;
	var vehicleConfigId = GetConfigurationId();
	var vehicleTypeId = $('#div_Summary').find('#div_' + selectedDiv + '').attr('compId');
	//var latpostion = parseInt(selectedDiv) + 1;
	//var longposition = 1;
	var latpostion = $('#div_Summary').find('#div_' + selectedDiv + '').attr('latPos');
	var longposition = $('#div_Summary').find('#div_' + selectedDiv + '').attr('longPos');
	var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
	var isVR1 = $('#vr1appln').val();
	var isNotify = $('#ISNotif').val();

	if (isNotify == 'True' || isNotify == 'true') {
		AjaxImportVR1VehComponent(vehicleConfigId, componentId, vehicleTypeId, latpostion, longposition);
	}
	else if (ApplicationRevId == 0 && isVR1 != 'True') {
		AjaxImportComponent(vehicleConfigId, componentId, vehicleTypeId, latpostion, longposition);
	}
	else if (isVR1 == 'True' || isVR1 == 'true') {
		AjaxImportVR1VehComponent(vehicleConfigId, componentId, vehicleTypeId, latpostion, longposition);
	}
	else {
		AjaxImportApplicationVehComponent(vehicleConfigId, componentId, vehicleTypeId, latpostion, longposition);
	}
}
//function ajax to import vehicle component
function AjaxImportComponent(vehicleConfigId, componentId, vhclTypeId, latPosition, longPosition) {
	if (vehicleConfigId == null) { vehicleConfigId = 0; }
	if (vhclTypeId == null) { vhclTypeId = 0; }
	if (latPosition == null) { latPosition = 0; }
	if (longPosition == null) { longPosition = 0; }
	;
	$.ajax({
		async: false,
		type: "POST",
		url: '../VehicleConfig/ImportComponent',
		dataType: "json",
		//contentType: "application/json; charset=utf-8",
		data: JSON.stringify({ vehicleConfigId: vehicleConfigId, componentId: componentId, vehicleTypeId: vhclTypeId, latitudePos: latPosition, longitudePos: longPosition }),
		processdata: true,
		success: function (result) {
			;
			//var url = "/VehicleConfig/CreateConfiguration";
			//window.location.href = url;
			FillComponentDetailsForConfig(componentId);
		},
		error: function (result) {
		}
	});

}
//function ajax Get Component List
var selComp = 0;
//function to display selected Component
function DisplaySelectedComponent(desc, compId, compName) {
    selComp = compId;
	$('#div_tbl_grid').html('<table class="tbl_registration"><tr><th class="headgrad">Description</th><th class="headgrad"></th></tr><tr><td>' + desc + '</td><td><button class="tdbutton tdright btngrad btnrds btnbdr btnautowidth btn-delete-comp-from-config" aria-hidden="true" data-icon="&#xe08e;" type="button">Replace</button><button class="tdbutton tdright btngrad btnrds btnbdr btn-edit-component-summ" aria-hidden="true" data-icon="&#xe000;" type="button" data-compid=' + compId + '>Edit</button><button class="tdbuttonauto tdright btngrad btnrds btnbdr btn-add-to-fleet" aria-hidden="true" type="button"  data-compid=' + compId + '  data-compname=' + compName+'>Add to Fleet</button></td></tr></table>');
}
//function for saving component in application component table code added by ajit
function SaveAppComponentOnImport(componentId) {
	var vehicleConfigId = GetConfigurationId();
	var vehicleTypeId = $('#div_Summary').find('#div_' + selectedDiv + '').attr('compId');
	//var latpostion = parseInt(selectedDiv) + 1;
	//var longposition = 1;
	var latpostion = $('#div_Summary').find('#div_' + selectedDiv + '').attr('latPos');
	var longposition = $('#div_Summary').find('#div_' + selectedDiv + '').attr('longPos');
	if (isCompEdit) {
		AjaxGetComponentDescFromApplication();
		isCompEdit = false;
	}
	else {
	AjaxImportApplicationVehComponent(vehicleConfigId, componentId, vehicleTypeId, latpostion, longposition);
	}
}
//function for saving component in route component table for VR1 appln
function SaveVR1ComponentOnImport(componentId) {
	
	var vehicleConfigId = GetConfigurationId();
	var vehicleTypeId = $('#div_Summary').find('#div_' + selectedDiv + '').attr('compId');
	//var latpostion = parseInt(selectedDiv) + 1;
	//var longposition = 1;
	var latpostion = $('#div_Summary').find('#div_' + selectedDiv + '').attr('latPos');
	var longposition = $('#div_Summary').find('#div_' + selectedDiv + '').attr('longPos');
	
	if (isCompEdit) {
	   
		AjaxGetComponentDescFromVR1();
		isCompEdit = false;
	}
	else {
		
		AjaxImportVR1VehComponent(vehicleConfigId, componentId, vehicleTypeId, latpostion, longposition);
	}
}
//function ajax to import application vehicle component
function AjaxImportApplicationVehComponent(vehicleConfigId, compId, vhclTypeId, latPosition, longPosition) {
	
	var isImportFromFleet = $('#IsSOCompFromFleet').val();
	var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
	$.ajax({
		url: "../VehicleConfig/ImportApplicationComponent",
		type: 'POST',
		cache: false,
		async: false,
		data: { vehicleConfigId: vehicleConfigId, componentId: compId, vehicleTypeId: vhclTypeId, latitudePos: latPosition, longitudePos: longPosition, ApplicationRevId: ApplicationRevId, isImportFromFleet: isImportFromFleet },
		success: function (result) {
			//console.log(result);
			if (result.Success) {
				//method
				AjaxGetComponentDescFromApplication();
			}
			else {
			   
			}
		}
	});
}
//function ajax to import VR1 vehicle component
function AjaxImportVR1VehComponent(vehicleConfigId, compId, vhclTypeId, latPosition, longPosition) {
    var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
    var notifid = $("#NotificatinId").val() ? $('#NotificatinId').val() : 0;
	$.ajax({
		url: "../VehicleConfig/ImportVR1Component",
		type: 'POST',
		cache: false,
		async: false,
		data: { vehicleConfigId: vehicleConfigId, componentId: compId, vehicleTypeId: vhclTypeId, latitudePos: latPosition, longitudePos: longPosition, ApplicationRevId: ApplicationRevId, NotificationID: notifid },
		success: function (result) {
			
			if (result.Success) {
				//method
				AjaxGetComponentDescFromVR1();
			}
			else {
			   
			}
		}
	});
}
//function ajax Get Component List from Application Component table
function AjaxGetComponentDescFromApplication() {
	console.log('AjaxGetComponentDescFromApplication');
	var configId = GetConfigurationId();
	//var latitudePos = parseInt(selectedDiv) + 1;
	//var longtitudePos = 1;
	var latitudePos = $('#div_Summary').find('#div_' + selectedDiv + '').attr('latPos');
	var longtitudePos = $('#div_Summary').find('#div_' + selectedDiv + '').attr('longPos');
	var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
	console.log('configId : ' || configId || ' ,latitudePos : ' || latitudePos || ' ,longtitudePos : ' || longtitudePos||' ,ApplicationRevId : '||ApplicationRevId);
	$.ajax({
		url: "../VehicleConfig/GetVehicleDescription",
		type: 'POST',
		cache: false,
		data: { vehicleId: configId, latitudePos: latitudePos, longtitudePos: longtitudePos, ApplicationRevId: ApplicationRevId, isApplication: true },
		success: function (result) {

			if (result.desc == null) {
				//method
				LoadSelectedGrid();
			}
			else {

				if (result.desc.AxleSpacing != null || result.desc.AxleSpacing != 0) {
					document.getElementById("DistanceToNxtAxle").setAttribute('value', result.desc.AxleSpacing);
				}

				DisplaySelectedComponent(result.desc.ComponentDescription, result.compId, result.desc.ComponentName);
				$('#btn_create_component').hide();
				$('.summaryinfo').hide();
			}
		}
	});
}
//function ajax Get Component List from Application Component table
function AjaxGetComponentDescFromVR1() {
	var configId = GetConfigurationId();
	//var latitudePos = parseInt(selectedDiv) + 1;
	//var longtitudePos = 1;
	var latitudePos = $('#div_Summary').find('#div_' + selectedDiv + '').attr('latPos');
	var longtitudePos = $('#div_Summary').find('#div_' + selectedDiv + '').attr('longPos');

	$.ajax({
		url: "../VehicleConfig/GetVehicleDescription",
		type: 'POST',
		cache: false,
		data: { vehicleId: configId, latitudePos: latitudePos, longtitudePos: longtitudePos, isApplication: false, isVR1: true},
		success: function (result) {

			if (result.desc == null) {
				//method
				LoadSelectedGrid();
			}
			else {

				
				if (result.desc.AxleSpacing != null || result.desc.AxleSpacing != 0) {
					document.getElementById("DistanceToNxtAxle").setAttribute('value', result.desc.AxleSpacing);
				}

				DisplaySelectedComponent(result.desc.ComponentDescription, result.compId, result.desc.ComponentName);
				$('#btn_create_component').hide();
				$('.summaryinfo').hide();
				
			}
		}
	});
}
//function delete component Config with alert
function DeleteCompFromConfig() {
   
	// $('#pop-warning').hide();
	var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
	var isVR1 = $('#vr1appln').val();
	var isNotify = $('#ISNotif').val();
	if (isNotify == 'True' || isNotify == 'true') {
		isVR1 = 'True';
	}

	if (ApplicationRevId == 0 && !isVR1) {
		showWarningPopDialog('Do you want to replace?', 'No', 'Yes', 'CloseModelPop', 'DeleteComponent', 1, 'warning');
	}
	else if (isVR1 == 'True') {
		//showPopUpDialog('Do you want to replace?', 'No', 'Yes', CloseModelPop, DeleteVR1Component, 1, 'warning');
		DeleteVR1Component();
	}
	else {
		//showWarningDialog('Do you want to replace?', 'No', 'Yes', WarningCancelBtn, DeleteAppComponent, 1, 'warning');
		DeleteAppComponent();
	}
}
//function to delete component
function DeleteComponent() {
	var configId = GetConfigurationId();
	//var latitudePos = parseInt(selectedDiv) + 1;
	//var longtitudePos = 1;

	var latitudePos = $('#div_Summary').find('#div_' + selectedDiv + '').attr('latPos');
	var longtitudePos = $('#div_Summary').find('#div_' + selectedDiv + '').attr('longPos');
	$.ajax({
		url: "../VehicleConfig/DeleteVehicleConfigPos",
		type: 'POST',
		cache: false,
		data: { vehicleConfigId: configId, latitudePos: latitudePos, longitudePos: longtitudePos, ComponentId: selComp },
		success: function (result) {
			if (result.Success) {
				//method
				AjaxGetComponentDesc();
				CloseModelPop();
			}
			else {
				
			}
		}
	});
}
//function to delete component from Application Component
function DeleteAppComponent() {
	var configId = GetConfigurationId();
	var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
	var latitudePos = $('#div_Summary').find('#div_' + selectedDiv + '').attr('latPos');
	var longtitudePos = $('#div_Summary').find('#div_' + selectedDiv + '').attr('longPos');
	var CompID = selComp;
	$.ajax({
		url: "../VehicleConfig/DeleteVehicleConfigPos",
		type: 'POST',
		cache: false,
		data: { vehicleConfigId: configId, latitudePos: latitudePos, longitudePos: longtitudePos, isAppRevId: ApplicationRevId, ComponentId: CompID, isApplication: true },
		success: function (result) {
			if (result.Success) {
				//method
			   
				AjaxGetComponentDescFromApplication();
				CloseModelPop();
			}
			else {
			   
			}
		}
	});
}
//function to delete component from Application Component
function DeleteVR1Component() {

	var configId = GetConfigurationId();
	var CompID = selComp;
	var latitudePos = $('#div_Summary').find('#div_' + selectedDiv + '').attr('latPos');
	var longtitudePos = $('#div_Summary').find('#div_' + selectedDiv + '').attr('longPos');
	var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
	var notifid = $("#NotificatinId").val() ? $('#NotificatinId').val() : 0;
	$.ajax({
		url: "../VehicleConfig/DeleteVehicleConfigPos",
		type: 'POST',
		cache: false,
		data: { vehicleConfigId: configId, latitudePos: latitudePos, longitudePos: longtitudePos, isAppRevId: ApplicationRevId, ComponentId: CompID, isApplication: true, isVR1: true, NotificationID: notifid },
		success: function (result) {
			if (result.Success) {
				//method
				
				AjaxGetComponentDescFromVR1();
				CloseModelPop();
			}
			else {
				
			}
		}
	});
}
//function to Add to Fleet()
function AddToFleet(componentId) {
	
	var compName = $('#' + componentId).find('#Internal_Name').val();
	CheckFormalNameExists(componentId, compName);

	//var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
	//var isVR1 = $('#vr1appln').val();
	//var NotifId = $('#Notificationid').val();
	   
	//if(NotifId != null && NotifId != undefined && NotifId != 0){
	//	CheckVR1FormalNameExists(componentId, compName);
	//}
	//else if (ApplicationRevId == 0) {
	//	CheckFormalNameExists(componentId, compName);
	//}
	//else if (isVR1 == 'True') {
	//	CheckVR1FormalNameExists(componentId, compName);
	//}
	//else {
	//	CheckAppFormalNameExists(componentId, compName);
	//}
}
//function to edit component
function EditComponent(componentId) {
   
	isCompEdit = true;
	var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
	var isVR1 = $('#vr1appln').val();
	var isNotify = $('#ISNotif').val();
	if (isNotify == 'True' || isNotify == 'true') {
		isVR1 = 'True';
	}
	if (ApplicationRevId == 0 && !isVR1) {
		EditComponentForVehicle(componentId);
	}
	else if (isVR1 == 'True') {
		EditComponentForVR1(componentId);
	}
	else {
		EditComponentForApplication(componentId);
	}
}
function EditComponentForVehicle(componentId) {	
	var islast = false;
	var isNotify = $('#ISNotif').val();
	var configId = GetConfigurationId();
	if (selectedDiv == (lastDiv - 1)) {
		islast = true;
	}
	if (CheckSideBySideLastComp()) {
		islast = true;
	}

	$.ajax({
		url: '../Vehicle/GeneralComponent',
		type: 'GET',
		cache: false,
		async: false,
		data: { vehicleSubTypeId: 0, vehicleTypeId: 0, movementId: 0, componentId: componentId, isLastComponent: islast, vehicleConfigId: configId, isNotify: isNotify },
		beforeSend: function () {
			$('.loading').show();
		},
		success: function (result) {
			$('#div_create_component').html(result);
			
		},
		error: function (xhr, textStatus, errorThrown) {
			//other stuff
			location.reload();
		},
		complete: function () {
			$('.loading').hide();
			$('#div_Summary').hide();
			$('#Component_Id').val(componentId);
			$('#btn_back_to_Config_main').show();
			$("#IsEdit").val(true);
			//$('#div_general').find('#Speed').hide();
		}
	});
}
//function for Application Vehicle added by ajit
function EditComponentForApplication(componentId) {
   
	var islast = false;
	if (selectedDiv == (lastDiv - 1)) {
		islast = true;
	}
	if (CheckSideBySideLastComp()) {
		islast = true;
	}

	$.ajax({
		url: '../Vehicle/GeneralComponentAppl',
		type: 'GET',
		cache: false,
		async: false,
		data: { vehicleSubTypeId: 0, vehicleTypeId: 0, movementId: 0, componentId: componentId, isLastComponent: islast },
		beforeSend: function () {
			$('.loading').show();
		},
		success: function (result) {
			$('#div_create_component').html(result);
		},
		error: function (xhr, textStatus, errorThrown) {
			//other stuff
			location.reload();
		},
		complete: function () {
			$('.loading').hide();
			$('#div_Summary').hide();
			$('#Component_Id').val(componentId);
			$('#btn_back_to_Config_main').show();
			$("#IsEdit").val(true);
			//$('#div_general').find('#Speed').hide();
		}
	});
}
//function for Application Vehicle added by ajit
function EditComponentForVR1(componentId) {
	var islast = false;
	if (selectedDiv == (lastDiv - 1)) {
		islast = true;
	}
	if (CheckSideBySideLastComp()) {
		islast = true;
	}

	$.ajax({
		url: '../Vehicle/GeneralComponentVR1',
		type: 'GET',
		cache: false,
		async: false,
		data: { vehicleSubTypeId: 0, vehicleTypeId: 0, movementId: 0, componentId: componentId, isLastComponent: islast },
		beforeSend: function () {
			$('.loading').show();
		},
		success: function (result) {
			$('#div_create_component').html(result);
		},
		error: function (xhr, textStatus, errorThrown) {
			//other stuff
			location.reload();
		},
		complete: function () {
			$('.loading').hide();
			$('#div_Summary').hide();
			$('#Component_Id').val(componentId);
			$('#btn_back_to_Config_main').show();
			$("#IsEdit").val(true);
			//$('#div_general').find('#Speed').hide();
		}
	});
}
//function to purticular component
function NavigateToSpecComponent(id) {
   
	$('.div_img_line').removeClass('vehiclecomp_active');
	$('#div_Summary').find('#div_' + id + '').addClass('vehiclecomp_active');
	selectedDiv = id;

	ButtonFinishShow();
	var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
	var isVR1 = $('#vr1appln').val();
	var isNotify = $('#ISNotif').val();
	if (isNotify == 'True') {
		AjaxGetComponentDescFromVR1();
	}
	else if (ApplicationRevId == 0 && !isVR1) {
		AjaxGetComponentDesc();
	}
	else if (isVR1 == 'True') {
		AjaxGetComponentDescFromVR1();
	}
	else {
		AjaxGetComponentDescFromApplication();
	}
}
//function to check show newly created component
function IsConfigCreate() {
	return 0;
}
//function to align side by side and inline
function AlignSideBySide() {
	var IsSidebySide = false;

	var _this=$('.div_img_line');
	var imgLength = $('.div_img_line').length;
	for (var i = 0; i < imgLength; i++) {
		var latPrev = _this.eq(i).attr('latpos');
		for (var j = i + 1; j < imgLength; j++) {
			var latCurrent = _this.eq(j).attr('latpos');
			if (latPrev == latCurrent) {
				//$('[latpos=' + latPrev + ']').eq(0).parent().addClass('sidebysidetop');
				//$('[latpos=' + latPrev + ']').eq(1).parent().addClass('sidebysidebottom');
				//console.log(latCurrent);
				_this.eq(i).parent().addClass('sidebysidetop');
				_this.eq(j).parent().addClass('sidebysidebottom');

				//$('[latpos=' + latPrev + ']').parent().removeClass('centre');
				_this.eq(i).parent().removeClass('centre');

				IsSidebySide = true;
			}
		}
	}

	if (!IsSidebySide) {
		$('#div_img_wrap .centre').removeClass('centre');
	}
}
//function get sub component types based on compId
function GetSubComponentTypes() {
	var compTypeList = new Array();

	var movementId = $('#MovementClassConfig').val();
	var vehicleTypeId = $('#div_Summary').find('#div_' + selectedDiv + '').attr('compId');
	//var url = '../Vehicle/FillVehicleSubType';
	$.ajax({
		url: '../Vehicle/FillVehicleSubType',
		type: 'POST',
		cache: false,
		async:false,
		data:{ movementId: movementId, vehicleTypeId: vehicleTypeId },
		success: function (data) {
			var datalength = data.type.length;
			for (var i = 0; i < datalength; i++) {
				var subcompName = data.type[i].SubCompName.toLowerCase().toString();                
				compTypeList.push(subcompName);
			}
		}
	});
	compSubTypeList = compTypeList;
   
	return compTypeList;

}
//function to check the formal name exists during add to fleet
function CheckFormalNameExists(componentId, compName) {
	
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
		url: '../VehicleConfig/CheckFormalName',
		type: 'POST',
		cache: false,
		async: false,
		data: { componentid: componentId, vehicleConfigId: configId, isMovement: planMvmt, organisationId: orgId },
		beforeSend: function () {
			startAnimation();
		},
		success: function (data) {
			
			compId = componentId;
			if (data.success > 0) {
				var warningOverwrite = "Component already exists. Do you want to overwrite? <br /><br /> Note : If you want to save it as a new component, edit the internal name and then add to fleet";
				ShowWarningPopup(warningOverwrite, 'AddComponentToFleet');
			}
			else {
				AddComponentToFleet();
			}
		},
		complete: function () {
			stopAnimation();
		}
	});
}
//function to add to fleet
function AddComponentToFleet() {
    //var notifid = $("#NotificatinId").val() ? $('#NotificatinId').val() : 0;
	//var configId = GetConfigurationId();
	var planMvmt =false ;
	if ($('#IsMovement').val() == "True" || $('#IsMovement').val() == "true" || $('#isMovement').val() == "true") {
		planMvmt = true;
	}
	var configId = $('#vehicleConfigId').val();
	var orgId = 0;
	 
	if ($('#Organisation_ID').val() != undefined && $('#Organisation_ID').val() != '') {
		orgId = $('#Organisation_ID').val();
	}
	$.ajax({
		url: '../VehicleConfig/AddComponentToFleet',
		type: 'POST',
		cache: false,
		async: false,
		data: { componentid: compId, vehicleConfigId: configId, isMovement: planMvmt, organisationId:orgId },
		beforeSend: function () {
			CloseWarningPopup();
			startAnimation();
		},
		success: function (result) {
			if (result.success > 0) {
				ShowSuccessModalPopup('Component added to fleet', 'CloseSuccessModalPopup()');
				//showWarningPopDialog('Component added to fleet', 'Ok', '', 'CloseModelPop', '', 1, 'info');
			}
			else {
				ShowSuccessModalPopup('Not saved', 'CloseSuccessModalPopup()');
				
				//showWarningPopDialog('Not saved', 'Ok', '', 'CloseModelPop', '', 1, 'error');
			}
		},
		complete: function () {
			stopAnimation();
		}
	});
}
function closepopup() {
	CloseSuccessModalPopup();
}
//function for Application component check formal name
function CheckAppFormalNameExists(componentId, compName) {

	AppComponentName = compName;
	$.ajax({
		url: '../VehicleConfig/CheckFormalName',
		type: 'POST',
		cache: false,
		async: false,
		data: { componentid: componentId, isApplication:true },
		success: function (data) {
			compAppId = componentId;

			if (data.success > 0) {
				showWarningPopDialog('Component already exists. Do you want to over write?', 'No', 'Yes', 'WarningCancelBtn', 'AddAppComponentToFleet', 1, 'warning');
			  
				if (data.success > 1)
					flag = 1;
			}
			else {
				flag = data.success;
				AddAppComponentToFleet();
			}
		}
	});
}
//function for VR1 vehicle component check formal name
function CheckVR1FormalNameExists(componentId, compName) {
	VR1ComponentName = compName;
	$.ajax({
		url: '../VehicleConfig/CheckFormalName',
		type: 'POST',
		cache: false,
		async: false,
		data: { componentid: componentId, isApplication: true ,isVR1: true},
		success: function (data) {
			compVR1Id = componentId;

			if (data.success > 0) {
				showWarningPopDialog('Component already exists. Do you want to over write?', 'No', 'Yes', 'CloseModelPop', 'AddVR1ComponentToFleet', 1, 'warning');
				
				if (data.success > 1)
					flag = 1;
			}
			else {
				flag = data.success;
				AddVR1ComponentToFleet();                
			}
		}
	});
}
//function for Application component add to fleet
function AddAppComponentToFleet() {
	var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
	var configId = GetConfigurationId();
	var orgId = 0;
	 
	if ($('#Organisation_ID').val() != undefined && $('#Organisation_ID').val() != '') {
		orgId = $('#Organisation_ID').val();
	}
	$.ajax({
		url: '../VehicleConfig/AddComponentToFleet',
		type: 'POST',
		cache: false,
		async: false,
		data: { componentid: compAppId, vehicleConfigId: configId, isAppRevId: ApplicationRevId, isApplication: true, flag: flag, isVR1: false, organisationId: orgId },
		beforeSend: function () {
			$('#pop-warning').hide();
		},
		success: function (result) {
			if (result.success > 0) {
				showWarningPopDialog('Component added to fleet', 'Ok', '', 'CloseModelPop', '', 1, 'info');
			}
			else {
				showWarningPopDialog('Not saved', 'Ok', '', 'CloseModelPop', '', 1, 'error');
			}
		}
	});
}
//function for Application component add to fleet
function AddVR1ComponentToFleet() {
    var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
	var configId = GetConfigurationId();
	var orgId = 0;
	 
	if ($('#Organisation_ID').val() != undefined && $('#Organisation_ID').val() != '') {
		orgId = $('#Organisation_ID').val();
	}
	$.ajax({
		url: '../VehicleConfig/AddComponentToFleet',
		type: 'POST',
		cache: false,
		async: false,
		data: { componentid: compVR1Id, vehicleConfigId: configId, isApplication: true, isAppRevId: ApplicationRevId, flag: flag, isVR1: true, organisationId: orgId },
		beforeSend: function () {
			$('#pop-warning').hide();
		},
		success: function (result) {
			if (result.success > 0) {
				showWarningPopDialog('Component added to fleet', 'Ok', '', 'CloseModelPop', '', 1, 'info');
			}
			else {
				showWarningPopDialog('Not saved', 'Ok', '', 'CloseModelPop', '', 1, 'error');
			}
		}
	});
}
function showPopUpDialog(message, btn1_txt, btn2_txt, btn1Action, btn2Action, autofocus, type) {
	ResetDialog();
	$('.pop-message').html(message);
	if (btn1_txt == '') { $('.box_Cancel_btn').hide(); } else { $('.box_Cancel_btn').html(btn1_txt); }
	if (btn2_txt == '') { $('.box_Ok_btn').hide(); } else { $('.box_Ok_btn').html(btn2_txt); }

	switch (autofocus) {
		case 1: $('.box_Cancel_btn').attr("autofocus", 'autofocus'); break;
		case 2: $('.box_Ok_btn').attr("autofocus", 'autofocus'); break;
		default: break;
	}

	switch (type) {
		case 'error': $('.message1').addClass("errror"); $('.popup1').css({ "background": '#fcd1d1' }); break;
		case 'info': $('.message1').addClass("info"); $('.popup1').css({ "background": '#cdecfe' }); break;
		case 'warning': $('.message1').addClass("warning"); $('.popup1').css({ "background": '#ffffd0' }); break;
		default: break;

	}

	$('#pop_dialog').show();

	$('.box_Cancel_btn').click(function () {
		btn1Action(function () {
			$('.box_Cancel_btn').unbind();
		}); });
	$('.box_Ok_btn').click(function () {
		btn2Action(function () {
			$('.box_Ok_btn').unbind();
		}); });
}
//function for reseting the popup
function ResetDialog() {
	$('.message1').removeClass("errror");
	$('.message1').removeClass("info");
	$('.message1').removeClass("warning");
	$('.box_Cancel_btn').show();
	$('.box_Ok_btn').show();
}
function CloseModelPop() {
	$('.pop-message').html('');
	$('.box_Cancel_btn').html('');
	$('.box_Ok_btn').html('');
	$('#pop-warning').hide();
}
function AjaxGetComponentDescFromNotification() {
	var configId = GetConfigurationId();
	var latitudePos = $('#div_Summary').find('#div_' + selectedDiv + '').attr('latPos');
	var longtitudePos = $('#div_Summary').find('#div_' + selectedDiv + '').attr('longPos');

	$('#form_listImportVehicle').find('#hidden_vehicleId').val(configId);
	$('#form_listImportVehicle').find('#hidden_latitudePos').val(latitudePos);
	$('#form_listImportVehicle').find('#hidden_longtitudePos').val(longtitudePos);

	$('#form_listImportVehicle').submit();
}
function GetComponentField() {
	return compTypeTemp;
}
function FillSelectGrid(result) {

	var htm = '<div>' + result + '</div>'
	var gridhtm = $(htm).find('#div_fleet_component').html();
	$('#div_tbl_grid').html(result);

		$('#hidden_compType').remove();
		$('#form_fleet_pagination').append('<input type="hidden" name="compType" id="hidden_compType" value="' + compTypeTemp + '">');

		//btn_create_component
		$('#btn_create_component').show();
		$('.summaryinfo').show();

		PaginateComponent();

}
function PaginateComponent() {
	$('body').on('click', '#dialogue #div_tbl_grid .pagination li a', function () {
		var className = $(this).parent().attr('class');
		
		switch (className) {
			case 'active':
				break;
			case 'PagedList-skipToLast':
				var pageCount = $('#div_tbl_grid').find('#TotalPages').val();
				AjaxVehiclePaginationComp(pageCount);
				break;
			case 'PagedList-skipToNext':
				var thisPage = $('#div_tbl_grid').find('.active').find('a').html();
				var nextPage = parseInt(thisPage) + 1;
				AjaxVehiclePaginationComp(nextPage);
				break;
			case 'PagedList-skipToFirst':
				AjaxVehiclePaginationComp(1);
				break;
			case 'PagedList-skipToPrevious':
				var thisPage = $('#div_tbl_grid').find('.active').find('a').html();
				var prevPage = parseInt(thisPage) - 1;
				AjaxVehiclePaginationComp(prevPage);
				break;
			default:
				var pageNum = $(this).html();
				AjaxVehiclePaginationComp(pageNum);
				break;
		}

	});
}
//function Ajax call fro pagination
function AjaxVehiclePaginationComp(pageNum) {
	var keyword = $('#txt_search').val();

	$('#hidden_comp_page').val(pageNum);
	$('#hidden_comp_keyword').val(keyword);


	$('#form_fleet_pagination').submit();
}
// function to check whether the component is the last component of a side by side config
function CheckSideBySideLastComp() {

	var isSideBySideConfig = $('#VehicleTypeConfig').val();
	var latitudePos = $('#div_Summary').find('#div_' + selectedDiv + '').attr('latPos');
	var latPosition;
	var maxLatPosn = 0;
	var result = false;
  
	if (isSideBySideConfig == 244007) {
		var totlength = vehicleCompList[vehicleCompNum].length;
		for (var i = 0; i < totlength; i++) {
			if (GetLatLongPos().length > 0) {

				latPosition = GetLatLongPos()[i].LatPos;
				if (latPosition > maxLatPosn) {
					maxLatPosn = latPosition;
				}
			}
		}

		if (latitudePos == maxLatPosn) {
			result = true;
		}
	}

	return result;
}
//function to check whether the route vehicle configuration 
//has missing components while creating
function IsValidNewVehicle(vehicleId) {
	var resume = true;
	//retrieve vehicle details
	$.ajax({

		url: '../VehicleConfig/CheckNotifVehicleConfigOnFinish',
		type: 'POST',
		async: false,
		data: { VehicleID: vehicleId },
		beforeSend: function () {
			startAnimation();
		},
		success: function (result) {

			var datacollection = result;

			//warning while importing a config with missing components
			if (datacollection.result.ConfigurationId == 0) {
				resume = false;
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
	return resume;
}