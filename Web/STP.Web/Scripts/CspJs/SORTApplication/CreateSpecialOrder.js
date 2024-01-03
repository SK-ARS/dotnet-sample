    $(document).ready(function () {
        $(".addingmatrix").on('click', addedmatrix);
        $(".removingmatrix").on('click', removedmatrix);
        $(".addingmatrix").mouseover( addmatrixmouseover);
        $(".removingmatrix").mouseover(removedmatrixmouseover);
        $(".span-count").on('click', showpopup);
    });
    var coverageArr = [];
    var removedcoverg = [];
    var veh_id = 0;
    var veh_arr = "";
    var veh_arr_push = [];
    function addedmatrix(e) {
        var param1 = e.currentTarget.dataset.arg1;
        var param2 = e.currentTarget.dataset.arg2;
        var param4 = e.currentTarget.dataset.arg4;
        var param5 = e.currentTarget.dataset.arg5;
        addmatrix(param1, param2,'',param4,param5);
    }
    function removedmatrix(e) {
        var param1 = e.currentTarget.dataset.arg1;
        var param2 = e.currentTarget.dataset.arg2;
        var param4 = e.currentTarget.dataset.arg4;
        removematrix(param1, param2,'', param4);
    }
    function addmatrixmouseover(e) {
        e.style.color = '#212529';
    }
    function removedmatrixmouseover(e) {
        e.style.color = '#212529';
    }


    function addmatrix(routeId, vehicleId, orderId, spanId, spanClassId) {        
        var elems = document.querySelectorAll(".columMapping_" + spanClassId);        
        [].forEach.call(elems, function (el) {            
            el.className = el.className.replace("rectangle-view2", "");
            el.className = el.className.replace("crnt1", "crnt1 rectangle-view1");
            el.className = el.className.replace("crnt2", "crnt2 rectangle-view2");
            el.className = el.className.replace("crnt3", "crnt3 rectangle-view3");
            el.className = el.className.replace("crnt4", "crnt4 rectangle-view4");           
        });
        $("#span_" + spanId).removeClass('rectangle-view4');
        $("#span_" + spanId).removeClass('rectangle-view1');
        $("#span_" + spanId).addClass('rectangle-view2');        
        coverageArr.push(routeId + "," + vehicleId);        
        removedcoverg.push(routeId + "," + vehicleId + "#" + orderId);
        veh_arr_push.push(vehicleId);      
        $("#btnapply").prop("disabled", false);
    }    
    function removematrix(routeId, vehicleId, orderId, spanId) {
        coverageArr.splice($.inArray(routeId + "," + vehicleId, coverageArr), 1);
        removedcoverg.splice($.inArray(routeId + "," + vehicleId + "#" + orderId, removedcoverg), 1);
        var cnt = 0;
        for (var i = 0; i < veh_arr_push.length; i++) {
            if (veh_arr_push[i] == vehicleId) {
                cnt = i;
            }
        }
        veh_arr_push.splice(cnt, 1);
        $("#btnapply").prop("disabled", false);
        if (orderId != "") {
            $("#span_" + spanId).removeClass('rectangle-view4');
            $("#span_" + spanId).removeClass('rectangle-view2');
            $("#span_" + spanId).removeClass('rectangle-view3');
            $("#span_" + spanId).addClass('rectangle-view1');
        }
        else {
            $("#span_" + spanId).removeClass('rectangle-view1');
            $("#span_" + spanId).removeClass('rectangle-view2');
            $("#span_" + spanId).removeClass('rectangle-view3');
            $("#span_" + spanId).addClass('rectangle-view4');
        }
    }


    $(document).ready(function () {

        coverageArr = [];
        removedcoverg = [];
        veh_id = 0;
        veh_arr = "";
        veh_arr_push = [];
        var spnumber = $('#SONumber').val();

        if (spnumber != '') {
            $('#div_matrix').find('.rectangle-view2').each(function () {
                if ($(this).attr('data-vid') != undefined) {
                    veh_arr_push.push($(this).attr('data-vid'));
                }
            });
        }

        $('#btnTemplatePreview').click(function () {
            var templateToShow = "";
            var userSelectedTemplate = $('#ddltemplate option:selected').val()
            if (userSelectedTemplate == 0) {
                templateToShow = $('#Recommended').val();
            }
            else {
                templateToShow = $('#ddltemplate option:selected').text()
            }
            if (templateToShow.length > 0) {
                var url = '\\Content\\Document\\Preview\\SO\\SO' + templateToShow + '.pdf';
                $('#template_frame').attr('src', url)
                $('#templatePreviewModal').modal('show');
            }
            else {
                ShowInfoPopup('No template to preview.');
            }
        });

        $("#btnModeClose").on("click", function (e) {
            e.preventDefault();
            $("#templatePreviewModal").modal("hide");
            $('#templatePreviewModal').data("modal", null);
        });
        var state = $('#State').val();
        BindState(state);

        var template = $('#Template').val();
        BindTemplate(template);

        var sostatus = $('#saveStatus').val();
        if (sostatus == 1) {
            history.go(-2);
        }

        $("#SOCreateDate").attr("disabled", "disabled");
        $("#Recommended").attr("disabled", "disabled");
        $("#btnapply").attr("disabled", "disabled");

        if ($('#Applicability').val() != "") {
            $("#State").attr("disabled", "disabled");
            $("#SOCreateDate").attr("disabled", "disabled");
            var appicabilities = $('#Applicability').val();

            var items = appicabilities.split(',');
            for (var i = 0; i < items.length; i++) {
                if (items[i] != "") {
                    coverageArr.push(items[i] + "," + items[i + 1]);
                }
                i++;
            }
        }
        //Tempalte dll change
        $('#ddltemplate').change(function () {
            if ($('#ddltemplate option:selected').val() != 0)
                $('#Template').val($('#ddltemplate option:selected').val());
            else
                $('#Template').val("");
        });
        //State dll Change
        $('#ddlstate').change(function () {
            if ($('#ddlstate option:selected').val() != 0)
                $('#State').val($('#ddlstate option:selected').val());
            else
                $('#State').val("");
        });
        $("#State").keypress(function (e) {
            if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                $("#errmsg").html("Digits Only").show().fadeOut("slow");
                return false;
            }
        });
        $('#btnback').click(function () {
            if ($('#hiddenmodelstatus').val() == "False")
                history.go(-2);
            else
                history.go(-1);
        });
        $('#btnsave').click(function () {
            $("#overlay").show();
            $('.loading').show();  
            $("#frm_SpecialOrder").submit();
        });
        $('#Signatory').change(function () {
            if ($('#Signatory').val() != 0) {
                $('#signatory_error').text('');
            }
        });
        $('#SignatoryRole').keypress(function () {
            if ($('#SignatoryRole').val() != "" || $('#SignatoryRole').val().length != 0 || $('#SignatoryRole').val() != null) {
                $('#signatoryrole_error').text('');
            }
        });
        
        $('#ddltemplate').change(function () {
            if ($('#ddltemplate').val() != 0) {
                $('#dlltemplate_error').text('');
            }
        });
    });

    $("form").submit(function () {
        return (ValidateFields());
    });


    $('#btnapply').click(function () {
        
        var result = "";
        $.each(coverageArr, function (e, v) {
            result += v + "#";
            veh_id = v.split(',');
        });
        SuggestTemplate(veh_arr_push);
        $('#Applicability').val(result);
        $('#hiddenremdcovrg').val(removedcoverg);
        $("#btnapply").prop("disabled", "disabled");

    });

    function ValidateFields() {
        var value = $('#ddltemplate').val();
        if (value == "0") {
            $('#dlltemplate_error').text('Template is required');
        }
        else {
            $('#dlltemplate_error').text('');
        }
        var signatory_value = $('#Signatory').val();
        if (signatory_value == "" || signatory_value==0) {
            $('#signatory_error').text('Signatory is required');
        }
        else {
            $('#signatory_error').text('');
        }


        var signatoryrole_value = $('#SignatoryRole').val();

        if (signatoryrole_value == "" || signatoryrole_value.length == 0 || signatoryrole_value == null) {
            $('#signatoryrole_error').text('Signatory Role is required');
        }
        else {
            $('#signatoryrole_error').text('');
        }
        var stateval = $('#ddlstate').val();
        if (value == "0" || stateval == "0" || signatory_value == "0" || signatoryrole_value == "") {
            return false;
        } else {

            return true;
        }
    }



    $('#ddltemplate').change(function () {
        var value = $('#ddltemplate').val();
        var temp = $('#Recommended').val();
        if (value == "0") {
            if (veh_id != "") {
                if (temp != "NOTEMP") {
                    $("#div_sugg_template").show();
                    $("#div_sugg_template").html('The ' + temp + ' template is suitable for the selected vehicle or you can choose another template.');
                } else {
                    $("#div_sugg_template").show();
                    $("#div_sugg_template").html('Please select the template from the list below.');
                }
            }
        } else {
            $("#div_sugg_template").hide();
        }
    });
    function SuggestTemplate(veh_arr_push) {
        
		var veh_detail = veh_arr_push;
        var json = JSON.stringify({ Vehicle_IDs: veh_detail });
        if ($('#Iseditflag').val() == '0') {
        $.ajax({
            url: '../SORTApplication/SuggestTemplateSpecialOrder',
            type: 'POST',
            data: json,
            dataType: 'json',
            beforeSend: function () {
                startAnimation();
            },
            success: function (result) {
                if (result.Success) {
                    if (result.Success != "0") {
                        if (result.Success != "NOTEMP") {
                            $("#div_sugg_template").show();
                            $("#div_sugg_template").html('The ' + result.Success + ' template is suitable for the selected vehicle or you can choose another template.');
                            $('#Recommended').val(result.Success);
                        } else {
                            $("#div_sugg_template").show();
                            $("#div_sugg_template").html('Please select the template from the list below.');
                        }
                        ShowSuccessModalPopup('The special order coverage grid applied successfully.', 'CloseSuccessModalPopup()');
                    }
                    else {
                        $("#div_sugg_template").hide();
                        $('#Recommended').val('');
                    }
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                
            },
            complete: function () {
                stopAnimation();

            }
        });
		}
		else {
		
		
		$.ajax({
			url: '../SuggestTemplateSpecialOrder',
			type: 'POST',
			data: json,
			dataType: 'json',
			beforeSend: function () {
				startAnimation();
			},
			success: function (result) {
				if (result.Success) {
					if (result.Success != "0") {
						if (result.Success != "NOTEMP") {
							$("#div_sugg_template").show();
							$("#div_sugg_template").html('The ' + result.Success + ' template is suitable for the selected vehicle or you can choose another template.');
                            $('#Recommended').val(result.Success);
						} else {
							$("#div_sugg_template").show();
							$("#div_sugg_template").html('Please select the template from the list below.');
                        }
                        ShowSuccessModalPopup('The special order coverage grid applied successfully.', 'CloseSuccessModalPopup()');
					}
					else {
						$("#div_sugg_template").hide();
						$('#Recommended').val('');
					}
				}
			},
			error: function (xhr, textStatus, errorThrown) {
				
				location.reload();
			},
			complete: function () {
				stopAnimation();

			}
        });
		}
    }

    function CloseOrgSavePopup() {
        WarningCancelBtn();
        $("#btnNextSORTGeneralSave").show();
        ViewGeneralTabAuto();
    }

    function BindState(stateText) {
        var stateValue = 0;
        if (stateText != null) {
            switch (stateText) {
                case "work in progress":
                    stateValue = 264001;
                    break;
                case "revoked":
                    stateValue = 264002;
                    break;
                case "signed and distributed":
                    stateValue = 264003;
                    break;
                default:
                    stateValue = 0;
                    break;
            }
        }
        $('#ddlstate').val(stateValue);
        $('#State').val(stateValue);
        $('#hiddenstateval').val(stateValue);
    }
    function BindTemplate(templateText) {
        var templatevalue = 0;
        if (templateText != null) {
            switch (templateText) {
                case "2d1":
                    templatevalue = 256001;
                    break;
                case "2d4":
                    templatevalue = 256002;
                    break;
                case "2d5":
                    templatevalue = 256003;
                    break;
                case "2d7":
                    templatevalue = 256004;
                    break;
                case "2d8":
                    templatevalue = 256005;
                    break;
                case "2d9":
                    templatevalue = 256006;
                    break;
                case "2d4a":
                    templatevalue = 256007;
                    break;
                case "2d4b":
                    templatevalue = 256008;
                    break;
                case "2d7a":
                    templatevalue = 256009;
                    break;
            }
        }
        $('#ddltemplate').val(templatevalue);
        $('#Template').val(templatevalue);
    }


     
