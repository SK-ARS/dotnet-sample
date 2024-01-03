var initData = [];

var isFromComponent = false;
var isFromConfigFlag = 0;

$(function () {
	debugger;
	suppressKey();
	axleButtons();
	RegistrationComplete();
	RegistrationBack();
	ButtonSave();
	$(".page_help").attr('url', '../../Home/NotImplemented');
	ReadFieldData();
	clearAllErrorSpan();
	$('#PageNum').val(1);
		
	if (typeof (CheckIsComponent) == 'function') {
		isFromComponent = CheckIsComponent();
	}

	SetRangeForSpacing();
	IterateThroughText();
	ShowFeet();
	$('#Speed').hide();

});

$('#MakeConfig').click(function () {
	var movementId = $('#Movement').val();
	if (movementId == 270006) {
		if ($(this).is(':checked')) {
			$('#Speed').show();
		}
		else {
			$('#Speed').hide();
		}
	}
});
	

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
	debugger;
	$('.btn_save').click(function () {
		debugger;
		var componentId = $('#Component_Id').val();
		var unit = $('#UnitValue').val();
		var ApplicationRevId = $('#ApplicationrevId').val() ? $('#ApplicationrevId').val() : 0;  
		var isVR1 = $('#vr1appln').val();
		var isNotify = $('#ISNotif').val();
		if (isNotify == 'True' || isNotify == 'true') {
			isVR1 = 'True';
		}
		var vd = validation();
		
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
	});
}

//Saveappvehcomponent

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
		contentType: 'application/json; charset=utf-8',
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
		contentType: 'application/json; charset=utf-8',
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
//function to save the general data
function Save(_btn) {
	var _this = $(_btn);
	var _form = _this.closest("form");
	var _showComp = 1;
	var configId = 0;

	if (typeof IsConfigCreate == 'function') {
	    _showComp = IsConfigCreate();
	     configId = GetConfigurationId();
	}
	//var formalName = $('#FormalName').val();
	//var informalName = $('#InformalName').val();
	var movementId = $('#Movement').val();
	var componentId = parseInt($('#ComponentType').val());
	var compSubId = parseInt($('#VehicleSubType').val());

	var componentName = $('#Internal_Name').val();
	var travelSpeed = $('#TravelSpeed').val();
	var speedUnit = $('#SpeedUnits').val();
	if (speedUnit == undefined || speedUnit == '') {
		speedUnit = null;
	}
	//if ($('#UnitValue').val() == 692002) {
	//    travelSpeed = (travelSpeed * 1.6093);
	//}

	var myEntries = {
		moveClassification: { ClassificationId: movementId },
		vehicleCompType: { ComponentTypeId: componentId },
		VehicleComponentId: compSubId,
		VehicleParamList: [],
		TravellingSpeed: travelSpeed,
		TravellingSpeedUnit : speedUnit
	};
	var listArray = new Array();

	$('.dynamic input,textarea').not(':hidden,:checkbox').each(function () {
		var name = $(this).val();
		var model = $(this).attr("id");
		var datatype = $(this).attr("datatype");
		if ($('#UnitValue').val() == 692002)
		{
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

	debugger;
	loadRegistration();

	$.ajax({
		url: _form.attr("action"),
		data: '{"vehicleComponent":' + JSON.stringify(myEntries) + ',"showComponent":' + _showComp + ',"vehicleConfigId":' + configId + '}',
		type: 'POST',
		contentType: 'application/json; charset=utf-8',
		async: false,
		success: function (data) {
			if (data.configId != '') {
				vehicleId = data.configId; // check from here
			}
			if (data.Success != 0) {
				$('#Component_Id').val(data.Success);
				/*$('#div_general').hide();*/
			   
				if (_showComp == 0 && (componentId == 234003 || componentId == 234007)) {
				   
					if (movementId != 270001) {
						
						SkipRegistrationForComp();
					}
					else {
					  
						NavigateToSummary();
						//var componentName = $('#Internal_Name').val();
						//showWarningPopDialog('Component  "' + componentName + '"  saved successfully', 'Ok', '', 'NavigateToSummary', '', 1, 'info');
					}
				}
				else {
					loadRegistration();
				}
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
			contentType: 'application/json; charset=utf-8',
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
function validation() {
	var isvalid = true;
	var movementId = $('#movementTypeId').val();
	var vc = $("#VehicleClass").val();
	var axlecount = $('.axledrop').val();  //added for crane workflow changes
	var isDetNotif = $('#DetailNotif').val();

	$('.dynamic input:text,textarea').each(function () {

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
		
		if (unitValue == 692002 && (field != 'Weight' && field != 'TravelSpeed')) {
			if (text.indexOf('\'') === -1 && text != '') {
					onlyInch = 1;
					if (onlyInch == 1) {
						text = '0\'' + text;
					}

				}
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
		   
			$(this).closest('div').find('.error').text(field + ' is required');
			isvalid = false;
		}
		//else if (required == 1 && text < 0.5)
		else if (required == 1 && text < 0.5 && field == 'Axle Spacing To Following')
		{
			$(this).closest('div').find('.error').text(field + ' must be in a range between ' + minval + ' and ' + maxval);
			isvalid = false;
		}
	
		if (movementId == 270001 && (field == 'Length' || field == 'Width')) {
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
		else if (movementId == 270006 && (parseFloat(text) == 0) && (field == 'Maximum_Height' || field == 'Length' || field == 'Width' || field == 'Reducable_Height' || field == 'Weight'))  {
			$(this).closest('div').find('.error').text(field + ' must be greater than 0');
			isvalid = false;
		}
		else if (movementId == 270002 && field == 'Weight') {
		    if (vc == 241003 || vc == 241006) {
		        maxval = 50000;
		        if (parseFloat(text) < parseFloat(minval) || parseFloat(text) > parseFloat(maxval)) {
					$(this).closest('div').find('.error').text(field + ' must be in a range between ' + minval + ' and ' + maxval);
		            isvalid = false;
		        }
		    }
		    else if (vc == 241004 || vc == 241007) {
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
		else if (movementId == 270003 && field == 'Weight') {
		  
		    //added for crane workflow changes
		    var catAwt = ((9 * axlecount) + (4 - axlecount) + (2 * (axlecount % 2))) * 1000;
		    var catBwt = 12.5 * axlecount * 1000;
		    var catAwb = 1.5 * axlecount + 0.5 * (axlecount % 2);
		    if (isDetNotif == "1") {
		        minval = 12000;
		        if (vc == 241006) {

		            if ((parseFloat(text) > parseFloat(catAwt)) || (parseFloat(text) <= parseFloat(minval))) {
						$(this).closest('div').find('.error').text(field + ' must be in a range between ' + 12000 + ' and ' + catAwt);
		                isvalid = false;
		            }
                  
		        }
		        else if (vc == 241007) {
		           
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
		        else if (vc == 241008) {
		            if ((parseFloat(text) > 150000) || (parseFloat(text) <= parseFloat(minval))) {
						$(this).closest('div').find('.error').text(field + ' must be in a range between ' + 12000 + ' and ' + 150000);
		                isvalid = false;
		            }
		        }
		    }
		}
		else if (movementId != 270006 && (parseFloat(text) < parseFloat(minval) || parseFloat(text) > parseFloat(maxval)) && parseFloat(text) > 0 ) {
			$(this).closest('div').find('.error').text(field + ' must be in a range between ' + minval + ' and ' + maxval);
			isvalid = false;
		}
		
		else if (parseFloat(loadLength) <= parseFloat(wheelbaseval)) {
			$('#div_general').find('#Wheelbase').closest('td').find('.error').text('Wheelbase should be less than the length');
			isvalid = false;
		}

		
	});
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

	if ($('#IsConfigAxle').val() == 'True') {
	    if ($('.axledrop').val() == '') {
			$('.axledrop').closest('div').find('.error').text('Please select the number of axles');
	        isvalid = false;
	    }
	}
	return isvalid;
}

//function to prevent entering non-numeric characters
function suppressKey() {
	$('.dynamic input:text').each(function () {
		$(this).keypress(function (evt) {
			$(this).closest('td').find('.error').text('');
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
	});
}

//function to validate data on keyup
function keyUpValidation(_this) {
    var movementId = $('#movementTypeId').val();
    var vc = $("#VehicleClass").val();
	$(_this).closest('td').find('.error').text('');
	var range = $(_this).attr('range');//min and max range
	var field = $(_this).attr('validationmsg');//textbox id/name field
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
	if (unitValue == 692002) {
		if (text.indexOf('\'') === -1) {
			onlyInch = 1;
			if (onlyInch == 1) {
				text = '0\'' + text;
			}
			
		}
	}

	if (movementId != 270006) {
		if (text == '') {
			
			//$(_this).closest('td').find('span').text(field + ' is required');
			//isvalid = false;
		}
		else if (movementId == 270001 && (field == 'Length' || field == 'Width')) {
		    if (field == 'Length' && unitValue == 692001) {
		        maxval = 27.4;
		    }
		    if (field == 'Length' && unitValue == 692002) {
		        maxval = 89 + "'" + 9 + '"';
		    }
		    if (parseFloat(text) < parseFloat(minval) || parseFloat(text) > parseFloat(maxval)) {
		        $(_this).closest('td').find('.error').text(field + ' must be in a range between ' + minval + ' and ' + maxval);
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
						$(_this).closest('td').find('.error').text(field + ' inches should be less than or equal to ' + '11' + '\"');
					}
					if (vhclWidth > vhclMaxWidth) {
						$(_this).closest('td').find('.error').text(field + ' must be less than ' + maxval);
					}
				}
				//} else {
				//    $(_this).closest('td').find('.error').text(field + ' must be imperial value');
				//}
			}
			
		}
		else if (movementId == 270002 && field == 'Weight') {
		    if (vc == 241003 || vc == 241006) {
		        maxval = 50000;
		        if (parseFloat(text) < parseFloat(minval) || parseFloat(text) > parseFloat(maxval)) {
		            $(_this).closest('td').find('.error').text(field + ' must be in a range between ' + minval + ' and ' + maxval);
		        }
		    }
		    else if (vc == 241004 || vc == 241007) {
		        maxval = 80000;
		        if (parseFloat(text) < parseFloat(minval) || parseFloat(text) > parseFloat(maxval)) {
		            $(_this).closest('td').find('.error').text(field + ' must be in a range between ' + minval + ' and ' + maxval);
		        }
		    }
		    else {
		        if (parseFloat(text) < parseFloat(minval) || parseFloat(text) > parseFloat(maxval)) {
		            $(_this).closest('td').find('.error').text(field + ' must be in a range between ' + minval + ' and ' + maxval);
		        }
		    }
		}
		else if (movementId == 270003 && field == 'Weight') {
		    //added for crane workflow changes
		    var catAwt = ((9 * axlecount) + (4 - axlecount) + (2 * (axlecount % 2))) * 1000;
		    var catBwt = 12.5 * axlecount * 1000;
		    if (isDetNotif == "1") {
		        minval = 12000;
		        if (vc == 241006) {
		            if ((parseFloat(text) > parseFloat(catAwt)) || (parseFloat(text) <= parseFloat(minval))) {
		                $(_this).closest('td').find('.error').text(field + ' must be in a range between ' + 12000 + ' and ' + catAwt);
		            }
		        }
		        else if (vc == 241007) {
		            if ((parseFloat(text) > parseFloat(catBwt)) || (parseFloat(text) <= parseFloat(minval)) || (parseFloat(text) > 150000)) {
		                if (axlecount > 12) {
		                    $(_this).closest('td').find('.error').text(field + ' must be in a range between ' + 12000 + ' and ' + 150000);
		                }
		                else {
		                    $(_this).closest('td').find('.error').text(field + ' must be in a range between ' + 12000 + ' and ' + catBwt);
		                }
		            }
		        }
		        else if (vc == 241008) {
		            if ((parseFloat(text) > 150000) || (parseFloat(text) <= parseFloat(minval))) {
		                $(_this).closest('td').find('.error').text(field + ' must be in a range between ' + 12000 + ' and ' + 150000);
		                isvalid = false;
		            }
		        }
		    }
		}
		else if (parseFloat(text) < parseFloat(minval) || parseFloat(text) > parseFloat(maxval)) {
		    $(_this).closest('td').find('.error').text(field + ' must be in a range between ' + minval + ' and ' + maxval);
		    //isvalid = false;
		}
	}
	else {
		if (text == '') {
		   
			//$(_this).closest('td').find('span').text(field + ' is required');
			//isvalid = false;
		}
		else if (parseFloat(text) == 0 && (field == 'Maximum_Height' || field == 'Length' || field == 'Width' || field == 'Reducable_Height' || field == 'Weight')) {
			$(_this).closest('td').find('.error').text(field + ' must be greater than 0');
		   
		}
	}
}

//funtion to load axles page
function loadAxles(axleCount) {
	debugger;
	var url = '../Vehicle/AxleComponent';
	var compWeight = $('#div_general').find('#Weight').val();
	var componentId = $('#Component_Id').val();
	//var movementId = $('#movementTypeId').val();

	var componentTypeId = parseInt($('#vehicleTypeValue').val());
	var compSubId = parseInt($('#vehicleSubTypeValue').val());
	var isEdit = $('#IsEdit').val();
	if (isEdit == '') {
		isEdit = false;
	}
	var data = {
		axleCount: axleCount, componentId: componentId,
		vehicleSubTypeId: compSubId, vehicleTypeId: componentTypeId,
		weight: compWeight, IsEdit: isEdit
	};
	$.post(url, data, function (data) {
		$('#axlePage').html(data);
		$('#axlePage').show();
		
		var elmnt = document.getElementById("axlePage");
		elmnt.scrollIntoView();

		HeaderHeight();
		//$('.axle').find('.sub').hide();------------------->>> to hide the axle center spacing
		$('.dyntitle').text('Edit axle');

		//$('#RegisterPage').hide();
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
		HeaderHeight();
		//$('.axle').find('.sub').hide();------------------->>> to hide the axle center spacing
		$('.dyntitle').text('Edit axle');

		$('#RegisterPage').hide();
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
		HeaderHeight();
		//$('.axle').find('.sub').hide();------------------->>> to hide the axle center spacing
		$('.dyntitle').text('Edit axle');

		$('#RegisterPage').hide();
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
		debugger;
		SaveAxleData();
	});
}


//function to trigger action on registration complete
function RegistrationComplete() {
	debugger;
	var pageSize = $('#pageSizeVal').val();
	var pageNum = $('#pageNum').val();
	
	$('#reg_btns').on('click', '#btn_reg_finish', function (){
		var componentName = $('#Internal_Name').val();
	   
		showWarningPopDialog('Component  "' + componentName + '"  saved successfully', 'Ok', '', 'ReloadLocation', '', 1, 'info');
		//$('body').load('../Vehicle/FleetComponent?page=' + pageNum + '&pageSize=' + pageSize + '');

	});

	$('#reg_btns').on('click', '#btn_reg_Next', function () {
		debugger;
		var numberOfAxles = $('#div_component_general_page').find('.axledrop').val();
		var configurableAxles = $('#div_component_general_page').find('.AxleConfig').val();
		if (configurableAxles == 'True') {
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
	});
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

//function to update vehicle component
function UpdateData(_btn) {
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

	//return validation();
	var vhclType = $('#vehicleTypeValue').val();
	var vhclIntend = $('#movementTypeId').val();
	if (vehicleId == undefined) {
		vehicleId = null;
	}

	debugger;
	$.ajax({
		url: '../Vehicle/UpdateComponent',
		data: '{"vehicleComponent":' + JSON.stringify(myEntries) + ',"componentId":' + componentId + ',"vehicleConfigId":' + vehicleId + ',"vehicleType":' + vhclType + ',"vehicleIntend":' + 0 + '}',
		type: 'POST',
		contentType: 'application/json; charset=utf-8',
		async: false,
		success: function (data) {
			debugger;
			if (data.configId != '') {
				vehicleId = data.configId; // check from here
			}
			if (data.result) {
				//$('#div_general').hide();
				isFromConfigFlag = data.isFromConfigFlag;
				if (isFromConfigFlag == 1 && (vhclType == 234003 || vhclType == 234007)) {
					if (vhclIntend != 270001) {
						SkipRegistrationForComp();
					}
					else {                        
						NavigateToSummary();
					}
				}
				else {
					loadRegistration();
				}
				$('#chars_left').hide();
				$('#HiddenFormalName').val(componentName);
			}
			else {
					   
				$('#chars_left').show();
				var IN = document.getElementById("Internal_Name");
				IN.focus();
				$('.err_formalName').text('Internal name already exists');
			}
		},
		error: function () {
			
			showWarningPopDialog('Component  "' + componentName + '"  not saved successfully', 'Ok', '', 'ReloadLocation', '', 1, 'error');
			//location.reload();
		},
		complete: function () {
				
		}
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
		contentType: 'application/json; charset=utf-8',
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
				$('.err_formalName').text('Internal name already exists');
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
		contentType: 'application/json; charset=utf-8',
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
				$('.err_formalName').text('Internal name already exists');
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
		$(this).closest('td').find('.error').text('');
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

		if (movementId != 270001 && movementId != 270008 && movementId != 270003) {

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

$('.axledrop').change(function () {
   
    var vehclass = $("#VehicleClass").val();
    var axcount = $(".axledrop option:selected").text();
    var isDetNotif = $('#DetailNotif').val();
    if (axcount >= 5 && vehclass == 241006 && isDetNotif == "1") {
        $('.axledrop').closest('td').find('.error').text('Axle count must be less than 5');
    }
    //else if (vehclass == 241007 && isDetNotif == "1" && axcount > 12)
    //{
    //    $('.axledrop').closest('td').find('.error').text('Axle count must be less than 13');
    //}
    else {
        $('.axledrop').closest('td').find('.error').text('');
    }
});