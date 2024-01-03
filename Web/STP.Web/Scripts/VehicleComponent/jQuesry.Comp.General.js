var initData = [];
var isFromComponent = false;
var isFromConfigFlag = 0;
$(document).ready(function () {
	$('body').on('click', '#MakeConfig', function (e) {
		var movementId = $('#Movement').val();
		if (movementId == VEHICLE_PURPOSE_CONFIG.SPECIAL_ORDER) {
			//if ($(this).is(':checked')) {
			//	$('#Speed').show();
			//}
			//else {
			//	$('#Speed').hide();
			//}
		}
	});
	$('body').on('click', '.axledrop', function (e) {
		var vehclass = $("#VehicleClass").val();
		var axcount = $(".axledrop option:selected").text();
		var isDetNotif = $('#DetailNotif').val();
		if (axcount >= 5 && vehclass == VEHICLE_CLASSIFICATION_TYPE_CONFIG.STGO_MOBILE_CRANE_CAT_A && isDetNotif == "1") {
			$('.axledrop').closest('td').find('.error').text('Axle count must be less than 5');
		}
		else {
			$('.axledrop').closest('td').find('.error').text('');
		}
	});
});
function Comp_GeneralInit()  {
	//SaveAllComponentDetails();
	suppressKey();
	axleButtons();
	RegistrationComplete();
	RegistrationBack();
	//ButtonSave();
	$(".page_help").attr('url', '../../Home/NotImplemented');
	ReadFieldData();
	clearAllErrorSpan();
	$('#PageNum').val(1);
	//$('#Speed').hide();
	if (typeof (CheckIsComponent) == 'function') {
		isFromComponent = CheckIsComponent();
	}
	SetRangeForSpacing();
	IterateThroughText();
	ShowFeet();
};
function validationSpeed() {
	$('#Speed').find('.error').text('');
	var unitvalue = $('#UnitValue').val();

	var this_txt = $('#TravelSpeed').val();
	var type = $('#TravelSpeed').attr('datatype');//datatype of the field
	var range = $('#TravelSpeed').attr('range');//min and max range
	var field = $('#TravelSpeed').attr('validationmsg');

		var minval = '';
		var maxval = '';
		var valueKph;
		var minvalfeet = '';
		var maxvalfeet = '';

		if (range != undefined) {
			var splitRange = range.split(',');
			if (splitRange.length > 1) {
				minval = splitRange[0];
				maxval = splitRange[1];
			}
		}

		if (unitvalue == 692002) {
			minvalmph = (minval / 1.6093).toFixed(2);
			maxvalmph = (maxval / 1.6093).toFixed(2);
			valueKph = ConvertToKph(this_txt);
			this_txt = valueKph;
		}
		
		if (this_txt == '') {
		}
		if (parseFloat(this_txt) < parseFloat(minval) || parseFloat(this_txt) > parseFloat(maxval)) {
			if (unitvalue == 692002) {
				$('#Speed').find('.error').text(field + ' must be in a range between ' + minvalmph + ' and ' + maxvalmph);
			}
			else {
				$('#Speed').find('.error').text(field + ' must be in a range between ' + minval + ' and ' + maxval);
			}
		}
}
//Function to trigger save button click
function ButtonSave() {
	/*$('.btn_save').click(function () {*/
		var componentId = $('#Component_Id').val();
		var unit = $('#UnitValue').val();
		var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;  
		var isVR1 = $('#vr1appln').val();
		var isNotify = $('#ISNotif').val();
		if (isNotify == 'True' || isNotify == 'true') {
			isVR1 = 'True';
		}
	var vd = validation(componentId);
	if (vd) {
			if (ApplicationRevId!=0 && isVR1 == 'False')
			{
				if (componentId == '') {

					Saveappvehcomponent(this);

				}
				else {
					//method
					Updateappvehcomponent(this);
				  //  UpdateData(this);
				}
				
			}
			else if (isVR1 == 'True' || isVR1 == 'true') {
				if (componentId == '') {

					SaveVR1vehcomponent(this);

				}
				else {
				   
					UpdateVR1vehcomponent(this);
				}
			}
			else {
				
				if (componentId == '') {
					Save(this);

				}
				else {
					UpdateData(this);

				}
			}
		}
		return false;
	/*});*/
}
//function to save the general data
function Saveappvehcomponent(_btn) {
	var isNotif = false;
	if ($("#NotificatinId").val() != 0) {
		isNotif = true;
	}
	var _this = $(_btn);
	var _form = _this.closest("form");
	var _showComp = 1;
	if (typeof IsConfigCreate == 'function') {
		_showComp = IsConfigCreate();
		console.log(_showComp);
	}
	//var formalName = $('#FormalName').val();
	//var informalName = $('#InformalName').val();
	var movementId = $('#Movement').val();
	var componentId = parseInt($('#VehicleType').val());
	var compSubId = parseInt($('#VehicleSubType').val());

	var componentName = $('#Internal_Name').val();

	var myEntries = {
		moveClassification: { ClassificationId: movementId },
		vehicleCompType: { ComponentTypeId: componentId },
		VehicleComponentId: compSubId,
		VehicleParamList: []
	};

	var listArray = new Array();


	$('.dynamic input,textarea').not(':hidden,:checkbox').each(function () {
		var name = $(this).val();
		var model = $(this).attr("id");
		var datatype = $(this).attr("datatype");
		if ($('#UnitValue').val() == 692002) {
			if (IsLengthField(this)) {
				name = ConvertToMetres(name);
			}
		}
		myEntries.VehicleParamList.push({ ParamModel: model, ParamValue: name, ParamType: datatype });
	});


	var numberOfAxles = $('.axledrop').val();

	var configurableAxles = $('.AxleConfig').val();

	var axleModel = $('.axledrop').attr("id");
	var axleDatatype = "int";
	myEntries.VehicleParamList.push({ ParamModel: axleModel, ParamValue: numberOfAxles, ParamType: axleDatatype });

	var couplingType = $('#Coupling').html();
	var couplingDatatype = "string";
	myEntries.VehicleParamList.push({ ParamModel: 'Coupling', ParamValue: couplingType, ParamType: couplingDatatype });

	$('.dynamic input:checkbox').each(function () {
		var name = $(this).val();
		var model = $(this).attr("id");
		var value = 0;
		if ($(this).is(':checked')) {
			value = 1;
		}
		var datatype = "bool";

		myEntries.VehicleParamList.push({ ParamModel: model, ParamValue: value, ParamType: datatype });
	});

	//return validation();
	var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;//added for Log
	var configId = GetConfigurationId();//added for Log
	$.ajax({
		url: '../Vehicle/InsertAppVehicleComponent',
		data: '{"vehicleComponent":' + JSON.stringify(myEntries) + ',"showComponent":' + _showComp + ',"appRevID":' + ApplicationRevId + ',"vehicleId":' + configId + ',"isNotif":' + isNotif + '}',
		type: 'POST',
		//contentType: 'application/json; charset=utf-8',
		async: false,
		success: function (data) {
			if (data.Success != 0) {
			   
				$('#Component_Id').val(data.Success);
				$('#div_general').hide();
				loadRegistration();
				$('#chars_left').hide();
				$('#HiddenFormalName').val(componentName);
				$('#IsSOCompFromFleet').val(0);
			}
			else {

				$('#chars_left').show();
				var FN = document.getElementById("Internal_Name");
				FN.focus();
			}
		}
	});
}
//function to save general data of a vr1 appln vehicle
function SaveVR1vehcomponent(_btn) {

	var _this = $(_btn);
	var _form = _this.closest("form");
	var _showComp = 1;
	if (typeof IsConfigCreate == 'function') {
		_showComp = IsConfigCreate();
	}
	//var formalName = $('#FormalName').val();
	//var informalName = $('#InformalName').val();
	var movementId = $('#Movement').val();
	var componentId = parseInt($('#VehicleType').val());
	var compSubId = parseInt($('#VehicleSubType').val());

	var componentName = $('#Internal_Name').val();

	var myEntries = {
		moveClassification: { ClassificationId: movementId },
		vehicleCompType: { ComponentTypeId: componentId },
		VehicleComponentId: compSubId,
		VehicleParamList: []
	};

	var listArray = new Array();


	$('.dynamic input,textarea').not(':hidden,:checkbox').each(function () {
		var name = $(this).val();
		var model = $(this).attr("id");
		var datatype = $(this).attr("datatype");
		if ($('#UnitValue').val() == 692002) {
			if (IsLengthField(this)) {
				name = ConvertToMetres(name);
			}
		}
		myEntries.VehicleParamList.push({ ParamModel: model, ParamValue: name, ParamType: datatype });
	});


	var numberOfAxles = $('.axledrop').val();

	var configurableAxles = $('.AxleConfig').val();

	var axleModel = $('.axledrop').attr("id");
	var axleDatatype = "int";
	myEntries.VehicleParamList.push({ ParamModel: axleModel, ParamValue: numberOfAxles, ParamType: axleDatatype });

	$('.dynamic input:checkbox').each(function () {
		var name = $(this).val();
		var model = $(this).attr("id");
		var value = 0;
		if ($(this).is(':checked')) {
			value = 1;
		}
		var datatype = "bool";

		myEntries.VehicleParamList.push({ ParamModel: model, ParamValue: value, ParamType: datatype });
	});

	//return validation();
	var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
	var configId = GetConfigurationId();  
	var notifid = $("#NotificatinId").val() ? $('#NotificatinId').val() : 0;
	$.ajax({
		url: '../Vehicle/InsertVR1VehicleComponent',
		data: '{"vehicleComponent":' + JSON.stringify(myEntries) + ',"showComponent":' + _showComp + ',"appRevID":' + ApplicationRevId + ',"vehicleId":' + configId + ',"NotificationID":' + notifid + '}',
		type: 'POST',
		//contentType: 'application/json; charset=utf-8',
		async: false,
		success: function (data) {
			if (data.Success != 0) {
				
				$('#Component_Id').val(data.Success);
				$('#div_general').hide();
				loadRegistration();
				$('#chars_left').hide();
				$('#HiddenFormalName').val(componentName);
			}
			else {

				$('#chars_left').show();
				var FN = document.getElementById("Internal_Name");
				FN.focus();
			}
		}
	});
}
function saveAxle() {
   
	var componentId = $('#Component_Id').val();
	var axlesList = []; //Axle list
	var axlePos = 0;
	var axleweightsum = 0;
	$('#tbl_Axle tr').each(function () {
	  
		var count = 0;
		var wheels;
		var axleWeight;
		var disToNxtAxle;
		var tyreSize;
		var tyreSpaceList = "";
		var readDistToNxtAxle = "false";
		var readTyreSize = "false";

		$(this).find('input').each(function () {
	   
			
			//To determine whether we have read the fixed items wheels, axleweight and distance to next axle 
			if ($(this).attr("name") == 'wheels') {
				wheels = $(this).attr('value');
			   
				++count;
			} else if ($(this).attr("name") == 'axleweight') {
				axleWeight = $(this).attr('value');
				axleweightsum = parseFloat(axleweightsum) + parseFloat(axleWeight)
				;
			   
				++count;
			} else if ($(this).attr("name") == 'distancetoaxle') {
				disToNxtAxle = $(this).attr('value');
				
				readDistToNxtAxle = "true";
				++count;
			} else if ($(this).attr("name") == 'tyresize') {
				tyreSize = $(this).attr('value');
				
				readTyreSize = "true";
				++count;
			} else if (readTyreSize == "true") {//We have reached the text boxes of tyre spacing add it to tyre space list
				if (tyreSpaceList.length > 0) {
					tyreSpaceList = tyreSpaceList + ',' + $(this).attr('value');
					
				} else {
					tyreSpaceList = $(this).attr('value');
				   
				}
			}
		});
		

		if (readDistToNxtAxle == "true") {
			
			++axlePos;
			axlesList.push({
				AxleNumId: axlePos,
				NoOfWheels: wheels,
				AxleWeight: axleWeight,
				DistToNextAxle: disToNxtAxle,
				TyreSize: tyreSize,
				TyreCenters: tyreSpaceList
			});
		}
	});
	var result = checkaxle_weight(axleweightsum);
	var textValid = ValidateAxles();
	if (result && textValid) {
		$.ajax({
			url: '../Vehicle/SaveAxles',
			data: '{"axleList":' + JSON.stringify(axlesList) + ',"componentId":' + componentId + '}',
			type: 'POST',
			//contentType: 'application/json; charset=utf-8',
			async: false,
			success: function (data) {
				if (data.success != 0) {
					//$('#dialog').hide();
					//$('#overlay').hide();
					
					showWarningPopDialog('Component saved successfully', 'Yes', '', 'WarningCancelBtn', 'DeleteData', 1, 'info');
					location.reload();
				} else {
				   
					showWarningPopDialog('Error in saving axle details. Please retry', 'Yes', '', 'WarningCancelBtn', 'DeleteData', 1, 'error');
				}
			}
		});
	}
	
}
function checkaxle_weight1(x) {
	
	var result = true;
	var range = $('#axleweight_range').val();
	var splitRange = range.split(',');
	var minval = splitRange[0];
	var maxval = splitRange[1];
   
	if (parseFloat(x) < parseFloat(minval) || parseFloat(x) > parseFloat(maxval)) {
		$('.axlewrapper').find('.error').text('Total axle weight must be in a tolerance range between ' + minval + ' and ' + maxval);
		result = false;
	}
	return result;
}
//function to validate data against the save
function validation(componentId) {
	
	var isvalid = true;
	var movementId = $('#movementTypeId').val();
	var vc = $("#VehicleClass").val(); //added for crane workflow changes
	var isDetNotif = $('#DetailNotif').val();
	var cmp = "";
	if (componentId != undefined) {
		cmp = "#" + componentId;
	}
	var axlecount = $(cmp+'.axledrop').val();
	$('' + cmp + ' .dynamic input:text,textarea').each(function () {
		$(this).closest('div').find('.error').text('');

		var required = $(this).attr('isrequired');//is required field
		var type = $(this).attr('datatype');//datatype of the field
		var range = $(this).attr('range');//min and max range
		var field = $(this).attr('validationmsg');//textbox id/name field
		if (field == "Rear Overhang") {
		    field = "Projection Rear";
		}
		var text = $(this).val();//textbox value (str.name.value.trim() 
		text = $.trim(text);
		var text_inch_var = $(this).val();

		var unitValue = $('#UnitValue').val();
		var onlyInch = 0;
		
		if (unitValue == 692002 && (field != 'Weight' && field != 'Speed' && field != 'Number of Axles')) {
			if (text.indexOf('\'') === -1 && text != '') {
					onlyInch = 1;
					if (onlyInch == 1) {
						text = '0\'' + text;
					}

			}
			isvalid = ComponentKeyUpValidation(this);
			if (!isvalid)
				return isvalid;
		}

	   // var splitRange = range.split(',');
		var splitRange = ",";

		if (range != undefined) {
			splitRange = range.split(',');
		}
		var minval = splitRange[0];
		var maxval = splitRange[1];

		var wheelbaseval = $('#div_general').find('#Wheelbase').val();
		var loadLength = $('#div_general').find('#Length').val();
		//Wheelbase should be less than length   mirza
		
		if (required == 1 && text == '') {
			//if (required == 1 ) {
			if (field == "Internal Name")
				field = "Name";
			$(this).closest('div').find('.error').text(field + ' is required');
			$(this).closest('div').find('.error').show();
			isvalid = false;
		}
		//else if (required == 1 && text < 0.5)
		else if (required == 1 && text < 0.5 && field == 'Axle Spacing To Following')
		{
			$(this).closest('div').find('.error').text(field + ' must be in a range between ' + minval + ' and ' + maxval);
			isvalid = false;
		}
	
		if (movementId == VEHICLE_PURPOSE_CONFIG.WHEELED_CONSTRUCTION_AND_USE && (field == 'Length' || field == 'Width')) {
		    if (field == 'Length' && unitValue == 692001) {
		        maxval = 27.4;
		    }
		    if (field == 'Length' && unitValue == 692002) {
		        maxval = 89 + "'" + 9 + '"';
		    }
		    if (parseFloat(text) < parseFloat(minval) || parseFloat(text) > parseFloat(maxval)) {
				$(this).closest('div').find('.error').text(field + ' must be in a range between ' + minval + ' and ' + maxval);
		        isvalid = false;
		    }
			if (field == 'Width' && unitValue == 692002) {//#5950 Too many inches
				//if (text_inch_var.search('\'') != -1) {
				
				if (isDetNotif == "1") {
					var vhclWidth = $(this).val();
					vhclWidth = vhclWidth.replace("\'", '.');
					vhclWidth = vhclWidth.replace("\"", '');
					var vhclMaxWidth = maxval;
					vhclMaxWidth = vhclMaxWidth.replace("\'", '.');
					vhclMaxWidth = vhclMaxWidth.replace("\"", '');
					var arr_width = [];
					arr_width = vhclWidth.split('.');
					if (arr_width[1] > 11) {
						$(this).closest('div').find('.error').text(field + ' inches should be less than or equal to ' + '11' + '\"');
						isvalid = false;
					}
					if (vhclWidth > vhclMaxWidth) {
						$(this).closest('div').find('.error').text(field + ' must be less than ' + maxval);
						isvalid = false;
					}
				}
				//} else {
				//    $(this).closest('td').find('.error').text(field + ' must be imperial value');
				//    isvalid = false;
				//}
			}
		}
		else if (movementId == VEHICLE_PURPOSE_CONFIG.SPECIAL_ORDER && (parseFloat(text) == 0 && unitValue != 692002) && (field == 'Maximum Height' || field == 'Length' || field == 'Width' || field == 'Reducable_Height' || field == 'Weight'))  {
			$(this).closest('div').find('.error').text(field + ' must be greater than 0');
			isvalid = false;
		}
		else if (movementId == VEHICLE_PURPOSE_CONFIG.STGO_AIL && field == 'Weight') {
			if (vc == VEHICLE_CLASSIFICATION_TYPE_CONFIG.STGO_AIL_CAT1 || vc == VEHICLE_CLASSIFICATION_TYPE_CONFIG.STGO_MOBILE_CRANE_CAT_A) {
		        maxval = 50000;
		        if (parseFloat(text) < parseFloat(minval) || parseFloat(text) > parseFloat(maxval)) {
					$(this).closest('div').find('.error').text(field + ' must be in a range between ' + minval + ' and ' + maxval);
		            isvalid = false;
		        }
		    }
			else if (vc == VEHICLE_CLASSIFICATION_TYPE_CONFIG.STGO_AIL_CAT2 || vc == VEHICLE_CLASSIFICATION_TYPE_CONFIG.STGO_MOBILE_CRANE_CAT_B) {
		        maxval = 80000;
		        if (parseFloat(text) < parseFloat(minval) || parseFloat(text) > parseFloat(maxval)) {
					$(this).closest('div').find('.error').text(field + ' must be in a range between ' + minval + ' and ' + maxval);
		            isvalid = false;
		        }
		    }
		    else {
		        if (parseFloat(text) < parseFloat(minval) || parseFloat(text) > parseFloat(maxval)) {
					$(this).closest('div').find('.error').text(field + ' must be in a range between ' + minval + ' and ' + maxval);
		            isvalid = false;
		        }
		    }
		}
		else if (movementId == VEHICLE_PURPOSE_CONFIG.STGO_MOBILE_CRANE && field == 'Weight') {
		  
		    //added for crane workflow changes
		    var catAwt = ((9 * axlecount) + (4 - axlecount) + (2 * (axlecount % 2))) * 1000;
		    var catBwt = 12.5 * axlecount * 1000;
		    var catAwb = 1.5 * axlecount + 0.5 * (axlecount % 2);
		    if (isDetNotif == "1") {
		        minval = 12000;
		        if (vc == VEHICLE_CLASSIFICATION_TYPE_CONFIG.STGO_MOBILE_CRANE_CAT_A) {

		            if ((parseFloat(text) > parseFloat(catAwt)) || (parseFloat(text) <= parseFloat(minval))) {
						$(this).closest('div').find('.error').text(field + ' must be in a range between ' + 12000 + ' and ' + catAwt);
		                isvalid = false;
		            }
                  
		        }
		        else if (vc == VEHICLE_CLASSIFICATION_TYPE_CONFIG.STGO_MOBILE_CRANE_CAT_B) {
		           
		            if ((parseFloat(text) > parseFloat(catBwt)) || (parseFloat(text) <= parseFloat(minval)) || (parseFloat(text) > 150000)) {
		                if (axlecount > 12)
		                {
							$(this).closest('div').find('.error').text(field + ' must be in a range between ' + 12000 + ' and ' + 150000);
		                    isvalid = false;
		                }
		                else {
							$(this).closest('div').find('.error').text(field + ' must be in a range between ' + 12000 + ' and ' + catBwt);
		                    isvalid = false;
		                }
		            }
		        }
		        else if (vc == VEHICLE_CLASSIFICATION_TYPE_CONFIG.STGO_MOBILE_CRANE_CAT_C) {
		            if ((parseFloat(text) > 150000) || (parseFloat(text) <= parseFloat(minval))) {
						$(this).closest('div').find('.error').text(field + ' must be in a range between ' + 12000 + ' and ' + 150000);
		                isvalid = false;
		            }
		        }
		    }
		}
		else if (movementId != VEHICLE_PURPOSE_CONFIG.SPECIAL_ORDER && (parseFloat(text) < parseFloat(minval) || parseFloat(text) > parseFloat(maxval)) ) {
			$(this).closest('div').find('.error').text(field + ' must be in a range between ' + minval + ' and ' + maxval);
			isvalid = false;
		}
		
		else if (parseFloat(loadLength) <= parseFloat(wheelbaseval)) {
			$('#div_general').find('#Wheelbase').closest('div').find('.error').text('Wheelbase should be less than the length');
			isvalid = false;
		}

		
	});
	if (isvalid) {
		$('#tbl_Axle tbody tr .disttonext').each(function () {
			isvalid = ComponentKeyUpValidation(this);
			if (!isvalid)
				return isvalid;
		});
	}
	//if ($('#IsConfigAxle').val() == 'True') {
	//    if ($('.axledrop').val() == '') {
	//		$('.axledrop').closest('div').find('.error').text('Please select the number of axles');
	//        isvalid = false;
	//    }
	//}
	return isvalid;
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
					this.value = this.value.replace(/[^0-9\.]/g, '');
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
//function to validate data on keyup
function keyUpValidation(_this) {

    var movementId = $('#movementTypeId').val();
	var configTypeId = $('#ConfigTypeId').val();
    var vc = $("#VehicleClass").val();
	$(_this).closest('div').find('.error').text('');
	var range = $(_this).attr('range');//min and max range
	var field = $(_this).attr('validationmsg');//textbox id/name field
	var datatype = $(_this).attr('datatype');
	if (field == "Rear Overhang") {
	    field = "Projection Rear";
	}
	var unitValue = $('#UnitValue').val();
	var text = $(_this).val();//textbox value
	var text_inch_var = $(_this).val();
	var onlyInch = 0;
	var isDetNotif = $('#DetailNotif').val();
	var splitRange = range.split(',');
	var minval = splitRange[0];
	var maxval = splitRange[1];
	SetRangeForSpacing();
	var axlecount = $('.axledrop').val();  //added for crane workflow changes
	if (unitValue == 692002 && (field != 'Weight' && field != 'Speed' && field != 'Number of Axles')) {
		if (text.indexOf('\'') === -1) {
			onlyInch = 1;
			if (onlyInch == 1) {
				text = '0\'' + text;
			}
			
		}
	}
	if (datatype != "string") {
		if (movementId != VEHICLE_PURPOSE_CONFIG.SPECIAL_ORDER) {
			if (text == '') {

				//$(_this).closest('td').find('span').text(field + ' is required');
				//isvalid = false;
			}
			else if (movementId == VEHICLE_PURPOSE_CONFIG.WHEELED_CONSTRUCTION_AND_USE && (field == 'Length' || field == 'Width')) {
				if (field == 'Length' && unitValue == 692001) {
					maxval = 27.4;
				}
				if (field == 'Length' && unitValue == 692002) {
					maxval = 89 + "'" + 9 + '"';
				}
				if (isNaN(parseFloat(text)) || (parseFloat(text) < parseFloat(minval) || parseFloat(text) > parseFloat(maxval))) {
					$(_this).closest('div').find('.error').text(field + ' must be in a range between ' + minval + ' and ' + maxval);
					//isvalid = false;
				}
				if (field == 'Width' && unitValue == 692002) {//#5950 Too many inches
					//if (text_inch_var.search('\'') != -1) {

					if (isDetNotif == "1") {
						var vhclWidth = text;
						vhclWidth = vhclWidth.replace("\'", '.');
						vhclWidth = vhclWidth.replace("\"", '');
						var vhclMaxWidth = maxval;
						vhclMaxWidth = vhclMaxWidth.replace("\'", '.');
						vhclMaxWidth = vhclMaxWidth.replace("\"", '');
						var arr_width = [];
						arr_width = vhclWidth.split('.');
						if (arr_width[1] > 11) {
							$(_this).closest('div').find('.error').text(field + ' inches should be less than or equal to ' + '11' + '\"');
						}
						if (vhclWidth > vhclMaxWidth) {
							$(_this).closest('div').find('.error').text(field + ' must be less than ' + maxval);
						}
					}
					//} else {
					//    $(_this).closest('td').find('.error').text(field + ' must be imperial value');
					//}
				}

			}
			else if (movementId == VEHICLE_PURPOSE_CONFIG.STGO_AIL && field == 'Weight') {
				if (vc == VEHICLE_CLASSIFICATION_TYPE_CONFIG.STGO_AIL_CAT1 || vc == VEHICLE_CLASSIFICATION_TYPE_CONFIG.STGO_AIL_CAT1.STGO_MOBILE_CRANE_CAT_A) {
					maxval = 50000;
					if (parseFloat(text) < parseFloat(minval) || parseFloat(text) > parseFloat(maxval)) {
						$(_this).closest('div').find('.error').text(field + ' must be in a range between ' + minval + ' and ' + maxval);
					}
				}
				else if (vc == VEHICLE_CLASSIFICATION_TYPE_CONFIG.STGO_AIL_CAT2 || vc == VEHICLE_CLASSIFICATION_TYPE_CONFIG.STGO_MOBILE_CRANE_CAT_B) {
					maxval = 80000;
					if (parseFloat(text) < parseFloat(minval) || parseFloat(text) > parseFloat(maxval)) {
						$(_this).closest('div').find('.error').text(field + ' must be in a range between ' + minval + ' and ' + maxval);
					}
				}
				else {
					if (parseFloat(text) < parseFloat(minval) || parseFloat(text) > parseFloat(maxval)) {
						$(_this).closest('div').find('.error').text(field + ' must be in a range between ' + minval + ' and ' + maxval);
					}
				}
			}
			else if (movementId == VEHICLE_PURPOSE_CONFIG.STGO_MOBILE_CRANE && field == 'Weight') {
				//added for crane workflow changes
				var catAwt = ((9 * axlecount) + (4 - axlecount) + (2 * (axlecount % 2))) * 1000;
				var catBwt = 12.5 * axlecount * 1000;
				if (isDetNotif == "1") {
					minval = 12000;
					if (vc == VEHICLE_CLASSIFICATION_TYPE_CONFIG.STGO_MOBILE_CRANE_CAT_A) {
						if ((parseFloat(text) > parseFloat(catAwt)) || (parseFloat(text) <= parseFloat(minval))) {
							$(_this).closest('div').find('.error').text(field + ' must be in a range between ' + 12000 + ' and ' + catAwt);
						}
					}
					else if (vc == VEHICLE_CLASSIFICATION_TYPE_CONFIG.STGO_MOBILE_CRANE_CAT_B) {
						if ((parseFloat(text) > parseFloat(catBwt)) || (parseFloat(text) <= parseFloat(minval)) || (parseFloat(text) > 150000)) {
							if (axlecount > 12) {
								$(_this).closest('div').find('.error').text(field + ' must be in a range between ' + 12000 + ' and ' + 150000);
							}
							else {
								$(_this).closest('div').find('.error').text(field + ' must be in a range between ' + 12000 + ' and ' + catBwt);
							}
						}
					}
					else if (vc == VEHICLE_CLASSIFICATION_TYPE_CONFIG.STGO_MOBILE_CRANE_CAT_C) {
						if ((parseFloat(text) > 150000) || (parseFloat(text) <= parseFloat(minval))) {
							$(_this).closest('div').find('.error').text(field + ' must be in a range between ' + 12000 + ' and ' + 150000);
							isvalid = false;
						}
					}
				}
			} else if (configTypeId == VEHICLE_CONFIGURATION_TYPE_CONFIG.RECOVER_VEHICLE) {
				//No need display validation
			}
			else if (isNaN(parseFloat(text)) || (parseFloat(text) < parseFloat(minval) || parseFloat(text) > parseFloat(maxval)) && field != "Width" && field != "Length") {
				$(_this).closest('div').find('.error').text(field + ' must be in a range between ' + minval + ' and ' + maxval);
				//isvalid = false;
			}
		}
		else {
			if (text == '') {

				//$(_this).closest('td').find('span').text(field + ' is required');
				//isvalid = false;
			}
			else if (isNaN(parseFloat(text)) || (parseFloat(text) == 0 && unitValue != 692002) && (field == 'Maximum_Height' || field == 'Length' || field == 'Width' || field == 'Reducable_Height' || field == 'Weight')) {
				$(_this).closest('div').find('.error').text(field + ' must be greater than 0');

			}
			//else if (parseFloat(text) < parseFloat(minval) || parseFloat(text) > parseFloat(maxval)) {
			//	$(_this).closest('div').find('.error').text(field + ' must be in a range between ' + minval + ' and ' + maxval);
			//	//isvalid = false;
			//}
		}
	}
}
//funtion to load axles page
function loadAxles(axleCount) {
	axleCount = axleCount == "" ? 0 : axleCount;
	var compWeight = $('#div_general').find('#Weight').val();
	var componentId = $('#Component_Id').val();
	var isFromConfig = $('#HiddenFromConfig').val();
	
	var componentTypeId = parseInt($('#vehicleTypeValue').val());
	var compSubId = parseInt($('#vehicleSubTypeValue').val());
	var movementId = $('#movementTypeId').val();

	if (componentId == "") { componentId = 0; }
	if (componentTypeId == null) { componentTypeId = 0; }
	if (compSubId == null) { compSubId = 0; }

	var isEdit = $('#IsEdit').val();
	if (isEdit == '') {
		isEdit = false;
	}
	var data = {
		axleCount: axleCount, componentId: componentId,
		vehicleSubTypeId: compSubId, vehicleTypeId: componentTypeId, movementId: movementId,
		weight: compWeight, IsEdit: isEdit, IsFromConfig: isFromConfig
	};

	$.ajax({
		url: '../Vehicle/AxleComponent',
		data: data,
		type: 'GET',
		//contentType: 'application/json; charset=utf-8',
		async: false,
		success: function (response) {
			$('#axlePage').html(response);
			$('#axlePage').show();
			HeaderHeight(componentId);
			Comp_AxleInit(0, componentId);
			$('.dyntitle').text('Edit axle');
			if (typeof AxleTableMethods != 'undefined')
				AxleTableMethods.AdjustSingleComponentAxleTableWidth();
			$('.axle-table-contents-container').css('width', '90%');
			$('.btnOpenAxlePopUp').remove();
		},
		error: function () {

		},
		complete: function () {

		}
	});
}
//funtion to load app veh axles page
function loadAppvehAxles(axleCount) {

	var url = '../Vehicle/AxleComponent';
	var compWeight = $('#div_general').find('#Weight').val();
	var componentId = $('#Component_Id').val();
	var movementId = $('#movementTypeId').val();

	var componentTypeId = parseInt($('#vehicleTypeValue').val());
	var compSubId = parseInt($('#vehicleSubTypeValue').val());
	var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;//added for log
	var configId = GetConfigurationId();//added for log
	var isEdit = $('#IsEdit').val();
	if (isEdit == '') {
		isEdit = false;
	}
	var data = {
		axleCount: axleCount, componentId: componentId,
		vehicleSubTypeId: compSubId, vehicleTypeId: componentTypeId,
		movementId: movementId, weight: compWeight, IsEdit: isEdit, isApplication: true, appRevID: ApplicationRevId, vehConfigID: configId
	};
	$.post(url, data, function (data) {
		$('#axlePage').html(data);
		$('#axlePage').show();
		HeaderHeight(componentId);
		//$('.axle').find('.sub').hide();------------------->>> to hide the axle center spacing
		$('.dyntitle').text('Edit axle');

		$('#RegisterPage').hide();
		if (typeof AxleTableMethods!='undefined')
			AxleTableMethods.AdjustSingleComponentAxleTableWidth();
	});
}
//funtion to load vr1 appln veh axles page
function loadVR1vehAxles(axleCount) {
	var url = '../Vehicle/AxleComponent';
	var compWeight = $('#div_general').find('#Weight').val();
	var componentId = $('#Component_Id').val();
	var movementId = $('#movementTypeId').val();

	var componentTypeId = parseInt($('#vehicleTypeValue').val());
	var compSubId = parseInt($('#vehicleSubTypeValue').val());
	var isEdit = $('#IsEdit').val();
	if (isEdit == '') {
		isEdit = false;
	}
	var data = {
		axleCount: axleCount, componentId: componentId,
		vehicleSubTypeId: compSubId, vehicleTypeId: componentTypeId,
		movementId: movementId, weight: compWeight, IsEdit: isEdit, isApplication: true,isVR1:true
	};
	$.post(url, data, function (data) {
		$('#axlePage').html(data);
		$('#axlePage').show();
		HeaderHeight(componentId);
		//$('.axle').find('.sub').hide();------------------->>> to hide the axle center spacing
		$('.dyntitle').text('Edit axle');

		$('#RegisterPage').hide();
		if (typeof AxleTableMethods != 'undefined')
			AxleTableMethods.AdjustSingleComponentAxleTableWidth();
	});
}
//function to load registration page
function loadRegistration() {
   
	var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
	var isVR1 = $('#vr1appln').val();
	var isNotify = $('#ISNotif').val();
	if (isNotify == 'True' || isNotify == 'true') {
		isVR1 = 'True';
	}
	var isApplication = false;
	if (ApplicationRevId != 0) {
		isApplication = true;
	}


	var istractor = $('#Tractor').val();
	var vehicleTypeId = $('#vehicleTypeValue').val(); //Added to remove registration Id field for Rigid vehicle component type
	var compId = $('#Component_Id').val();
	var url = '../Vehicle/RegistrationComponent';
	var data = { compId: compId, isTractor: istractor, vehicleTypeId: vehicleTypeId, isApplication: isApplication ,isVR1: isVR1};
	$.post(url, data, function (data) {
		$('#RegisterPage').html(data);
		// checking whether Edit component or Create component is selected by the user and displaying the Component Registration page accordingly
		var isEdit = $('#IsEdit').val();
		if (isEdit == '') {
			isEdit = false;
		}
		if (isEdit) {
			$('.dyntitle').text('Edit component registration');
		}
		else {
			$('.dyntitle').text('Create component registration');
		}


		$('#RegisterPage').show();
		$('#selection').hide();
		var elmnt = document.getElementById("RegisterPage");
		elmnt.scrollIntoView();
	});
}
//function to trigger events based on the axle page button clicks
function axleButtons() {
	
	/*$('#btn_axle_back').live('click', function ()*/
    $('#axle_btns').on('click', '#btn_axle_back', function (){
		
		$('#btn_reg_Next_Config').hide();
		var hasChange = IsChangedData();
		var vehicleTypeId = $('#vehicleTypeValue').val();
		if (hasChange) {
			if (isFromConfigFlag == 1 && vehicleTypeId == 234003) {
				showWarningPopDialog('You have unsaved changes. Do you want to go back?', 'Cancel', 'Ok', 'WarningCancelBtn', 'SkipToGeneralPageFromAxles', 1, 'warning');
			}
			else {
				showWarningPopDialog('You have unsaved changes. Do you want to go back?', 'Cancel', 'Ok', 'WarningCancelBtn', 'ShowRegisterPage', 1, 'warning');
			}
		}
		else {
			if (isFromConfigFlag == 1 && (vehicleTypeId == 234003 || vehicleTypeId == 234007)) {
				SkipToGeneralPageFromAxles();
			}
			else{
				ShowRegisterPage();
				ClearSpan();
			}
		}
		
		$(".page_help").attr('url', '../../Home/NotImplemented');
	});
	/*$('#btn_axle_finish').live('click', function ()*/
    $('#axle_btns').on('click', '#btn_axle_finish', function () {
		//location.reload();
		//saveAxle();

		SaveAxleData();
	});
}
//function to trigger action on registration complete
function RegistrationComplete() {
	var pageSize = $('#pageSizeVal').val();
	var pageNum = $('#pageNum').val();
	
	$('#reg_btns').on('click', '#btn_reg_finish', function (){
		var componentName = $('#Internal_Name').val();
	   
		showWarningPopDialog('Component  "' + componentName + '"  saved successfully', 'Ok', '', 'ReloadLocation', '', 1, 'info');
		//$('body').load('../Vehicle/FleetComponent?page=' + pageNum + '&pageSize=' + pageSize + '');

	});


	//$('#reg_btns').on('click', '#btn_reg_Next', function () {
	//	;
	//	var numberOfAxles = $('#div_component_general_page').find('.axledrop').val();
	//	var configurableAxles = $('#div_component_general_page').find('.AxleConfig').val();
	//	if (configurableAxles == 'True') {
	//		var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
	//		var isVR1 = $('#vr1appln').val();
	//		var isNotify = $('#ISNotif').val();
			
	//		if (isNotify == 'True' || isNotify == 'true') {
	//			loadVR1vehAxles(numberOfAxles);
	//		}
	//		else if (ApplicationRevId == 0 && !isVR1) {
	//			loadAxles(numberOfAxles);
	//		}
	//		else if (isVR1 == 'True') {
	//			loadVR1vehAxles(numberOfAxles);
	//		}
	//		else {
	//			loadAppvehAxles(numberOfAxles);
	//		}
	//		//location.reload();
	//	}
	//	else {
	//		//$('body').load('../Vehicle/FleetComponent?page=' + pageNum + '&pageSize=' + pageSize + '');
	//		location.reload();
	//	}
	//});
}
//function to trigger Registration page back button click event
function RegistrationBack() {
	$('#reg_btns').on('click', '#btn_reg_back', function (){
		$('#RegisterPage').hide();
		$('#div_general').show();
		$('.dyntitle').text('Edit Component');
		$('#selection').show();
		$('.error').text('');

		$('#PageNum').val(1);
		ReadFieldData();

		$(".page_help").attr('url', '../../Home/NotImplemented');
	});
}
//Updateappvehcomponent
//function to update application vehicle component
function Updateappvehcomponent(_btn) {
	var _this = $(_btn);
	var _form = _this.closest("form");

	var componentId = $('#Component_Id').val();

	var componentName = $('#Internal_Name').val();
	

	var myEntries = {
		VehicleParamList: []
	};

	var listArray = new Array();

	$('.dynamic input,textarea').not(':hidden,:checkbox').each(function () {
		var name = $(this).val();
		var model = $(this).attr("id");
		var datatype = $(this).attr("datatype");
		if ($('#UnitValue').val() == 692002) {
			if (IsLengthField(this)) {
				name = ConvertToMetres(name);
			}
		}
		myEntries.VehicleParamList.push({ ParamModel: model, ParamValue: name, ParamType: datatype });
	});

	var numberOfAxles = $('#div_general .axledrop').val();
	var configurableAxles = $('#div_general .AxleConfig').val();
	var axleModel = $('#div_general .axledrop').attr("id");
	var axleDatatype = "int";
	myEntries.VehicleParamList.push({ ParamModel: axleModel, ParamValue: numberOfAxles, ParamType: axleDatatype });

	$('.dynamic input:checkbox').each(function () {
		var name = $(this).val();
		var model = $(this).attr("id");
		var value = 0;
		if ($(this).is(':checked')) {
			value = 1;
		}
		var datatype = "bool";

		myEntries.VehicleParamList.push({ ParamModel: model, ParamValue: value, ParamType: datatype });
	});
	var ApplicationRevId = $('#ApplicationrevId').val()
	var configId = GetConfigurationId();
	//return validation();

	$.ajax({ 
		url: '../Vehicle/UpdateComponent',
		data: '{"vehicleComponent":' + JSON.stringify(myEntries) + ',"componentId":' + componentId + ',"isApplication":' + true + ',"appRevID":' + ApplicationRevId + ',"vehicleConfigId":' + configId + '}',
		type: 'POST',
		//contentType: 'application/json; charset=utf-8',
		async: false,
		success: function (data) {
			if (data.result) {
				$('#div_general').hide();
				loadRegistration();
				$('#chars_left').hide();
				$('#HiddenFormalName').val(componentName);
				
			}
			else {

				$('#chars_left').show();
				var IN = document.getElementById("Internal_Name");
				IN.focus();
				$('.err_formalName').text('Name already exists');
			}
		},
		error: function () {
		   
			showWarningPopDialog('Component  "' + componentName + '" is not saved successfully', 'Ok', '', 'WarningCancelButton', '', 1, 'error');
			location.reload();
		},
		complete: function () {

		}
	});
}
//function to update VR1 application vehicle component
function UpdateVR1vehcomponent(_btn) {
	
	var _this = $(_btn);
	var _form = _this.closest("form");

	var componentId = $('#Component_Id').val();

	var componentName = $('#Internal_Name').val();
   

	var myEntries = {
		VehicleParamList: []
	};

	var listArray = new Array();

	$('.dynamic input,textarea').not(':hidden,:checkbox').each(function () {
		var name = $(this).val();
		var model = $(this).attr("id");
		var datatype = $(this).attr("datatype");
		if ($('#UnitValue').val() == 692002) {
			if (IsLengthField(this)) {
				name = ConvertToMetres(name);
			}
		}
		myEntries.VehicleParamList.push({ ParamModel: model, ParamValue: name, ParamType: datatype });
	});

	var numberOfAxles = $('#div_general .axledrop').val();
	var configurableAxles = $('#div_general .AxleConfig').val();
	var axleModel = $('#div_general .axledrop').attr("id");
	var axleDatatype = "int";
	myEntries.VehicleParamList.push({ ParamModel: axleModel, ParamValue: numberOfAxles, ParamType: axleDatatype });

	//console.log(numberOfAxles); console.log(axleModel); return false;

	$('.dynamic input:checkbox').each(function () {
		var name = $(this).val();
		var model = $(this).attr("id");
		var value = 0;
		if ($(this).is(':checked')) {
			value = 1;
		}
		var datatype = "bool";

		myEntries.VehicleParamList.push({ ParamModel: model, ParamValue: value, ParamType: datatype });
	});

	//return validation();
	var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
	var configId = GetConfigurationId();
	var notifid = $("#NotificatinId").val() ? $('#NotificatinId').val() : 0;
	
	$.ajax({
		url: '../Vehicle/UpdateComponent',
		data: '{"vehicleComponent":' + JSON.stringify(myEntries) + ',"componentId":' + componentId + ',"isApplication":' + true + ',"isVR1":' + true + ',"vehicleConfigId":' + configId + ',"appRevID":' + ApplicationRevId + ',"NotificationID":' + notifid + '}',
		type: 'POST',
		//contentType: 'application/json; charset=utf-8',
		async: false,
		success: function (data) {
			if (data.result) {
				$('#div_general').hide();
				loadRegistration();
				$('#chars_left').hide();
				$('#HiddenFormalName').val(componentName);

			}
			else {

				$('#chars_left').show();
				var IN = document.getElementById("Internal_Name");
				IN.focus();
				$('.err_formalName').text('Name already exists');
			}
		},
		error: function () {
		   
			showWarningPopDialog('Component  "' + componentName + '" is not saved successfully', 'Ok', '', 'WarningCancelButton', '', 1, 'error');
			location.reload();
		},
		complete: function () {

		}
	});
}
//function to clear error span message
function clearAllErrorSpan() {
	$('body').on('focus', 'input,textarea', function()
	{
		$(this).closest('div').find('.error').text('');
	});
}
function ShowRegisterPage() {
	$('#RegisterPage').show();
	$('.dyntitle').text('Edit component registration');
	$('#axlePage').hide();
	WarningCancelBtn();
	$('#PageNum').val(2);
	$('.error').text('');
}
function ReadFieldData() {
	$('#div_general').find('input:text,select,textarea').each(function () {
		var _txt = $(this).val();
		initData.push(_txt);
	});
}
// function to check whether any changes have been made in the Edit Component page
function CompareField() {
	var hasChange = false;
	var i = 0;
	$('#div_general').find('input:text,select,textarea').each(function () {
		var _txt = $(this).val();
		if (_txt != initData[i]) {
			hasChange = true;
			return false;
		}
		i++;
	});
	return hasChange;
}
function SetRangeForSpacing() {
	var movementId = $('#movementTypeId').val();
	var componentId = parseInt($('#vehicleTypeValue').val());
	var vehicleTypeId = $('#VehicleTypeConfig').val();
	var unit = $('#UnitValue').val();

	if (movementId != VEHICLE_PURPOSE_CONFIG.WHEELED_CONSTRUCTION_AND_USE && movementId != VEHICLE_PURPOSE_CONFIG.TRACKED && movementId != VEHICLE_PURPOSE_CONFIG.STGO_MOBILE_CRANE) {

			if (vehicleTypeId == 244002 && componentId == 234002) {
				$('#Axle_Spacing_To_Following').attr('range', '2.5,100');
				if (unit == 692002) {
					ConvertRangeToFeet($('#Axle_Spacing_To_Following'));
				}

			}
			else if (vehicleTypeId == 244001 && componentId == 234001) {
				$('#Axle_Spacing_To_Following').attr('range', '1,50');
				if (unit == 692002) {
					ConvertRangeToFeet($('#Axle_Spacing_To_Following'));
				}

			}
			else if ((vehicleTypeId == 244006 || vehicleTypeId == 244007) ) {//&& (componentId == 234001 || componentId == 234002 || componentId == 234003) code commented for Bug #5114 axle spacing to following missing
				$('#Axle_Spacing_To_Following').attr('range', '0.5,100');
				if (unit == 692002) {
					ConvertRangeToFeet($('#Axle_Spacing_To_Following'));
				}

			}
		}

		
	}
function SkipRegistrationForComp() {
	$('#selection').hide();
	var numberOfAxles = $('#div_component_general_page').find('.axledrop').val();
	var configurableAxles = $('#div_component_general_page').find('.AxleConfig').val();

	if (configurableAxles == 'True') {
		$('#axlePage').show();
		var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
		var isVR1 = $('#vr1appln').val();
		var isNotify = $('#ISNotif').val();
		if (isNotify == 'True' || isNotify == 'true') {
			loadVR1vehAxles(numberOfAxles);
		}
		else if (ApplicationRevId == 0 && !isVR1) {
			loadAxles(numberOfAxles);
		}
		else if (isVR1 == 'True') {
			loadVR1vehAxles(numberOfAxles);
		}
		else {
			loadAppvehAxles(numberOfAxles);
		}
		//location.reload();
	}

	else {
		//$('body').load('../Vehicle/FleetComponent?page=' + pageNum + '&pageSize=' + pageSize + '');
		location.reload();
	}
}
function SkipToGeneralPageFromAxles() {
	$('#RegisterPage').hide();
	$('#axlePage').hide();
	$('#div_general').show();
	$('.dyntitle').text('Edit Component');
	$('#selection').show();
	$('.error').text('');

	$('#PageNum').val(1);
	ReadFieldData();

	$(".page_help").attr('url', '../../Home/NotImplemented');
}
//function to save the general data
function SaveComponent(_btn) {	
	startAnimation();
	var IsApplication = false; //Declared here as in vehicle component create IsApplication is throwing error.
	var _this = $(_btn);
	var _form = _this.closest("form");
	var _showComp = parseInt($('#ShowComponent').val());
	var configId = $('#vehicleConfigId').val() == "" ? 0 : $('#vehicleConfigId').val();
	if (configId == undefined || configId == "undefined") {
		configId = 0;
    }
	var isFromConfig = $('#HiddenFromConfig').val();
	var IsApplication = false;
	if ($('#IsApplication').val() == "true" || $('#IsApplication').val() == "True") {
		IsApplication = true;
	}
	var movementId = $('#movementTypeId').val();
	var componentId = parseInt($('#ComponentType').val());
	var compSubId = parseInt($('#VehicleSubType').val());

	var componentName = $('#Internal_Name').val();
	var travelSpeed = $('#TravelSpeed').val();
	var speedUnit = $('#SpeedUnits').val();
	if (speedUnit == undefined || speedUnit == '') {
		speedUnit = null;
	}

	var myEntries = {
		moveClassification: { ClassificationId: movementId },
		vehicleCompType: { ComponentTypeId: componentId },
		VehicleComponentId: compSubId,
		VehicleParamList: [],
		TravellingSpeed: travelSpeed,
		TravellingSpeedUnit: speedUnit
	};
	var listArray = new Array();

	$('.dynamic input,textarea').not(':hidden,:checkbox').each(function () {
		var name = $(this).val();
		var model = $(this).attr("id");
		var datatype = $(this).attr("datatype");
		if ($('#UnitValue').val() == 692002) {
			if (IsLengthField(this)) {
				name = ConvertToMetres(name);
			}
		}
		myEntries.VehicleParamList.push({ ParamModel: model, ParamValue: name, ParamType: datatype });
	});


	var numberOfAxles = $('.axledrop').val();

	var configurableAxles = $('.AxleConfig').val();

	var axleModel = $('.axledrop').attr("id");
	var axleDatatype = "int";
	myEntries.VehicleParamList.push({ ParamModel: axleModel, ParamValue: numberOfAxles, ParamType: axleDatatype });
	var couplingType = $('#Coupling').html();
	var couplingDatatype = "string";
	myEntries.VehicleParamList.push({ ParamModel: 'Coupling', ParamValue: couplingType, ParamType: couplingDatatype });
	$('.dynamic input:checkbox').each(function () {
		var name = $(this).val();
		var model = $(this).attr("id");
		var value = 0;

		if ($(this).is(':checked')) {
			value = 1;
		}
		var datatype = "bool";

		myEntries.VehicleParamList.push({ ParamModel: model, ParamValue: value, ParamType: datatype });
	});

	//-------- Registration details-------
	$(_this).attr('disabled', 'disabled');
	if (!validatData(_this)) {
		$(_this).attr('disabled', false);
		return false;
	}
	if ($(_this).closest('tr').next('tr').length != 0) {
		$(_this).attr('disabled', false);
		return false;
	}
	var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
	var isVR1 = $('#vr1appln').val();
	var isNotify = $('#ISNotif').val();


	var registrationParams = [];
	$('.tbl_registration tbody .tr_Registration').each(function () {
		var regVal = $(this).find('.txt_register').val();
		if (regVal == "") { regVal = $(this).find('.txt_register').html(); }
		var fleetId = $(this).find('.txt_fleet').val();
		if (fleetId == "") { fleetId = $(this).find('.txt_fleet').html(); }
		if (fleetId != "" || regVal != "") {
			registrationParams.push({ RegistrationValue: regVal, FleetId: fleetId });
		}

	});

	//--------------- Axle Details -----------------
	var axleweightsum = 0;
	$('#Config-body').find('.axleweight').each(function () {
		var _thisVal = $(this).val();
		axleweightsum = axleweightsum + parseFloat(_thisVal);
	});

	if (axleweightsum == 0) {
		$('#tbl_Axle').find('.axleweight').each(function () {
			var _thisVal = $(this).val();
			axleweightsum = axleweightsum + parseFloat(_thisVal);
		});
	}
	var result = checkaxle_weight(axleweightsum);

	var isNotify = $('#ISNotif').val();
	var vehicleClassCode = $('#MovementClassification').val();
	var excessAxleWgt = false;
	var axlesList = []; //Axle list
	//ValidateAxles();
	if (result && !excessAxleWgt) {
		var i = 1;
		var unitvalue = $('#UnitValue').val();
		var wb = 0;
		axlesList = [];
		$('#tbl_Axle tbody tr').each(function () {
			var axleNum = i;
			var noOfWheels = $(this).find('.nowheels').val();
			var axleWeight = $(this).find('.axleweight').val();
			var distanceToNxtAxl = $(this).find('.disttonext').val();

			//to check if distance tonext axle value is definedor not
			if (typeof distanceToNxtAxl !== "undefined") {
				if (unitvalue == 692002) {
					distanceToNxtAxl = ConvertToMetres(distanceToNxtAxl);
				}
				wb = parseFloat(wb) + parseFloat(distanceToNxtAxl);
			}

			var tyreSize = $(this).find('.tyresize').val();

			var tyreSpace = null;
			$(this).find('.cstable input:text').each(function () {
				var _thistxt = $(this).val();
				//if (_thistxt != undefined) {
				//	if (unitvalue == 692002) {
				//		_thistxt = ConvertToMetres(_thistxt);
				//	}
				//}
				if (tyreSpace != null) {
					tyreSpace = tyreSpace + "," + _thistxt;
				}
				else {
					tyreSpace = _thistxt;
				}
			});
			if (noOfWheels != "") {
				axlesList.push({ AxleNumId: axleNum, NoOfWheels: noOfWheels, AxleWeight: axleWeight, DistanceToNextAxle: distanceToNxtAxl, TyreSize: tyreSize, TyreCenters: tyreSpace });
			}
			i++;
		});
		var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
		var isVR1 = $('#vr1appln').val();
		var isNotify = $('#ISNotif').val();
		var validwb = true;
		var axlecount = $('.axledrop').val();  //added for crane workflow changes
		if (isNotify == 'True' || isNotify == 'true') {
			var vc = $("#VehicleClass").val();
			var catAwb = 1.5 * axlecount + 0.5 * (axlecount % 2);
			if (vc == VEHICLE_CLASSIFICATION_TYPE_CONFIG.STGO_MOBILE_CRANE_CAT_A) {
				if (parseFloat(wb) < parseFloat(catAwb)) {
					$('.error').text('Wheelbase should be greater than or equal to ' + catAwb);
					validwb = false;
				}
			}
			isVR1 = 'True';
		}
		var orgId = 0;
		if ($('#Organisation_ID').val() != undefined && $('#Organisation_ID').val() !='') {
			orgId = $('#Organisation_ID').val();
		}
		//--------------------------------------			
		$.ajax({
			url: _form.attr("action"),
			data: '{"vehicleComponent":' + JSON.stringify(myEntries) + ',"showComponent":' + _showComp + ',"registrationParams":' + JSON.stringify(registrationParams) + ',"axleList":' + JSON.stringify(axlesList) + ',"vehicleConfigId":' + configId + ',"isFromConfig":' + isFromConfig + ',"isMovement":' + IsApplication + ',"organisationId":' + orgId + '}',
			type: 'POST',
			//contentType: 'application/json; charset=utf-8',
			async: false,
			beforeSend: function () {
				startAnimation();
			},
			success: function (data) {
				
				if (data.configId != '') {
					vehicleId = data.configId; // check from here
				}
				if (data.Success > 0) {
					
					$('#Component_Id').val(data.Success);

					$('#chars_left').hide();
					$('#HiddenFormalName').val(componentName);
					var isMovement = $('#IsMovement').val();
					if ($('#IsFromConfig').val() == '1' || isMovement=="True") {
						var param1 = data.Guid;
						var param2 = configId;
						ShowSuccessModalPopup('Component  "' + componentName + '"  saved successfully', "FillComponentDetailsForConfig", param1, param2);
					}
					else {
						ShowSuccessModalPopup('Component  "' + componentName + '"  saved successfully', "LoadVehicleComponentList");
					}

					//SaveComponentRegistrationDetails(_form);
				}
				else if (data.Success == 0) {
					$("#btn_comp_finish").removeAttr("disabled");
					$("#chars_left").html("Internal Name already exist");
					$('#vehicles')[0].scrollIntoView();
					//var FN = document.getElementById("Internal_Name");
					//FN.focus();
				}
				else {
					$("#btn_comp_finish").removeAttr("disabled");
					$("#chars_left").html("Work flow failed");
				}
			},
			error: function (data) {
				;
				stopAnimation();
				$("#btn_comp_finish").removeAttr("disabled");
				//alert("error");
				ShowErrorPopup("error");
			},
			complete: function () {
				stopAnimation();
			}
		});

	}
	else {
		stopAnimation();
		$("#btn_comp_finish").removeAttr("disabled");
    }
}
function LoadVehicleComponentList() {
	window.location = "/Vehicle/FleetComponent";
}
//  save component registration details
function SaveComponentRegistrationDetails(_this) {

	$(_this).attr('disabled', 'disabled');
	if (!validatData(_this)) {
		$(_this).attr('disabled', false);
		return false;
	}
	if ($(_this).closest('tr').next('tr').length != 0) {
		$(_this).attr('disabled', false);
		return false;
	}
	var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;
	var isVR1 = $('#vr1appln').val();
	var isNotify = $('#ISNotif').val();

	if (isNotify == 'True' || isNotify == 'true') {

		AddVR1CompRegistration(_this);
	}
	else if (ApplicationRevId == 0 && !isVR1) {
		SaveRegistrationDetails(_this);
	}
	else if (isVR1 == 'True' || isVR1 == 'true') {
		AddVR1CompRegistration(_this);
	}
	else {
		AddAppCompRegistration(_this);
	}

	//});

}
function SaveRegistrationDetails(_this) {

	var registrationParams = [];
	var makeconfig = false;
	var vehicleConfigId = 0;
	if ($('#MakeConfig').is(':checked')) {
		makeconfig = true;
		vehicleConfigId = vehicleId;
	}
	var result = false;
	//vehicleId
	var componentId = $('#Component_Id').val();
	var isFromConfig = $('#IsFromConfig').val();
	if (isFromConfig == "") { isFromConfig = 0; }
	registrationParams = [];
	$('.tbl_registration tbody .tr_Registration').each(function () {

		var regVal = $(this).find('.txt_register').val();
		if (regVal == "") { regVal = $(this).find('.txt_register').html(); }
		var fleetId = $(this).find('.txt_fleet').val();
		if (fleetId == "") { fleetId = $(this).find('.txt_fleet').html(); }
		if (fleetId != "" && regVal != "") {
			registrationParams.push({ RegistrationValue: regVal, FleetId: fleetId });
		}

	});


	if (registrationParams.length > 0) {

		$.ajax({
			url: '../Vehicle/SaveRegistrationID',
			data: '{"compId":' + componentId + ',"registrationParams":' + JSON.stringify(registrationParams) + ',"vehicleConfigId":' + vehicleConfigId + ',"isFromConfig":' + isFromConfig + '}',
			type: 'POST',
			//contentType: 'application/json; charset=utf-8',
			async: false,
			success: function (data) {
				if (data.Success != 0) {
					SaveComponentAxleDetails(componentId);
				}
			},
			error: function (data) {
				;
			}
		});
	}
	else {
		SaveComponentAxleDetails(componentId);
	}
}
function SaveComponentAxleDetails(componentId) {

	var axleweightsum = 0;
	var configFlag = $("#hiddenConfigFlag").val();
	$('#Config-body').find('.axleweight').each(function () {
		var _thisVal = $(this).val();
		axleweightsum = axleweightsum + parseFloat(_thisVal);
	});

	if (axleweightsum == 0) {
		$('#div_component_general_page').find('.axleweight').each(function () {
			var _thisVal = $(this).val();
			axleweightsum = axleweightsum + parseFloat(_thisVal);
		});
	}
	var result = checkaxle_weight(axleweightsum);

	var isNotify = $('#ISNotif').val();
	var vehicleClassCode = $('#MovementClassification').val();
	var excessAxleWgt = false;
	if (result && isNotify) {
		$('#div_component_general_page').find('.axleweight').each(function () {
			var currentAxleWgt = $(this).val();

			// for STGO AIL Cat 1 notifcation && for stgo mobile crane cat a
			if ((vehicleClassCode == VEHICLE_CLASSIFICATION_TYPE_CONFIG.STGO_AIL_CAT1 || vehicleClassCode == VEHICLE_CLASSIFICATION_TYPE_CONFIG.STGO_MOBILE_CRANE_CAT_A) && parseFloat(currentAxleWgt) > 11500) {
				excessAxleWgt = true;
				$('.error').text('Max axle weight should be less than or equal to 11500 kg');
			}
			// for STGO AIL Cat 2 notifcation && for stgo mobile crane cat b
			else if ((vehicleClassCode == VEHICLE_CLASSIFICATION_TYPE_CONFIG.STGO_AIL_CAT2 || vehicleClassCode == VEHICLE_CLASSIFICATION_TYPE_CONFIG.STGO_MOBILE_CRANE_CAT_B) && parseFloat(currentAxleWgt) > 12500) {
				excessAxleWgt = true;
				$('.error').text('Max axle weight should be less than or equal to 12500 kg');
			}
			// for STGO AIL Cat 3 notifcation && for stgo mobile crane cat c
			else if ((vehicleClassCode == VEHICLE_CLASSIFICATION_TYPE_CONFIG.STGO_AIL_CAT3 || vehicleClassCode == VEHICLE_CLASSIFICATION_TYPE_CONFIG.STGO_MOBILE_CRANE_CAT_C) && parseFloat(currentAxleWgt) > 16500) {
				excessAxleWgt = true;
				$('.error').text('Max axle weight should be less than or equal to 16500 kg');
			}
		});
	}
	if (ValidateAxles() && result && !excessAxleWgt) {
		GetAxleDataToSave();
	}
	else {
		var componentId = $('#Component_Id').val();
		var componentName = $('#Internal_Name').val();
		//alert('Component  "' + componentName + '"  saved successfully');
		ShowModalPopup('Component  "' + componentName + '"  saved successfully');
		if (configFlag == 1) {
			ImportComponent(componentId);
		}
		else {
			location.reload();
		}
	}
}
function GetAxleDataToSave() {
	
	var axlesList = []; //Axle list
	var i = 1;
	var unitvalue = $('#UnitValue').val();
	var wb = 0;
	var componentId = $('#Component_Id').val();
	axlesList = [];
	$('#tbl_Axle tbody tr').each(function () {
		var axleNum = i;
		var noOfWheels = $(this).find('.nowheels').val();
		var axleWeight = $(this).find('.axleweight').val();
		var distanceToNxtAxl = $(this).find('.disttonext').val();

		//to check if distance tonext axle value is definedor not
		if (typeof distanceToNxtAxl !== "undefined") {
			if (unitvalue == 692002) {
				distanceToNxtAxl = ConvertToMetres(distanceToNxtAxl);
			}
			wb = parseFloat(wb) + parseFloat(distanceToNxtAxl);
		}

		var tyreSize = $(this).find('.tyresize').val();

		var tyreSpace = null;
		$(this).find('.cstable input:text').each(function () {
			var _thistxt = $(this).val();
			//if (_thistxt != undefined) {
			//	if (unitvalue == 692002) {
			//		_thistxt = ConvertToMetres(_thistxt);
			//	}
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
	var axlecount = $('.axledrop').val();  //added for crane workflow changes
	if (isNotify == 'True' || isNotify == 'true') {
		var vc = $("#VehicleClass").val();
		var catAwb = 1.5 * axlecount + 0.5 * (axlecount % 2);
		if (vc == VEHICLE_CLASSIFICATION_TYPE_CONFIG.STGO_MOBILE_CRANE_CAT_A) {
			if (parseFloat(wb) < parseFloat(catAwb)) {
				$('.error').text('Wheelbase should be greater than or equal to ' + catAwb);
				validwb = false;
			}
		}
		isVR1 = 'True';
	}
	if (validwb) {
		if (ApplicationRevId == 0 && !isVR1) {
			AjaxSaveAxle(axlesList);
		}
		else if (isVR1 == 'True') {
			AjaxSaveVR1VehAxle(axlesList);
		}
		else {
			AjaxSaveAppVehAxle(axlesList);
		}
	}
}

$('body').on('blur', '.componentKeyUpValidation', function (e) {
	ComponentKeyUpValidation(this);
});
function ComponentKeyUpValidation(_this) {
	var isFeetValid = true;
	var unitValue = $('#UnitValue').val();
	var name = $(_this).attr("name");
	var dataType = $(_this).attr("datatype");
	if (name == "distancetoaxle")
		dataType = "float";
	var field = $(_this).attr('validationmsg');//textbox id/name field
	if (unitValue == 692002 && dataType == "float" && (field != 'Weight' && field != 'Speed' && field != 'Number of Axles' && field != "AxleWeight" && field !="TrainWeight")) {
		var compVal = $(_this).val();
		var name = $(_this).attr("name");
		if (compVal != "") {
			//if (compVal.indexOf('\'') >= 0) {
			//	compVal=compVal.replace('"\"', '');
			//}
			var testEmail = /^([0-9]|1[012])+'([0-9])+(''|")$/i;
			if (testEmail.test(compVal)) {
				if (name == "distancetoaxle")
					$('.axlewrapper').find('.error').text('');
				else
					$(_this).closest('div').find('.error').text('');
			}
			else {
				if (name == "distancetoaxle")
					$('.axlewrapper').find('.error').text('Enter valid Distance to Next Axle');
				else
					$(_this).closest('div').find('.error').text('Enter valid ' + name);
				isFeetValid = false;
			}
		}
	}
	return isFeetValid;
}