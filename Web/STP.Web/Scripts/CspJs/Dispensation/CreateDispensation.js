    var isvalid = true;
    var flag = true;
    $(document).ready(function () {

        if ((@userTypeID == 696001)||(@userTypeID == 696002)) {
            SelectMenu(3);
        }
        if (@userTypeID == 696007) {
            SelectMenu(4);
        }
        if ('@fromAffectedParties' == 'True') {
            $('#confirm_btn').attr("disabled", true);
            $('#back_btn').hide();
            SelectMenu(2);
        }
        //$('#FromDate').val('@DateTime.Now.ToString("dd-MMM-yyyy")');
        //$('#ToDate').val('@DateTime.Now.ToString("dd-MMM-yyyy")');

        //function setDatepickerPos(input, inst) {
        //    var rect = input.getBoundingClientRect();
        //    setTimeout(function () {
        //        var scrollTop = $("body").scrollTop();
        //        inst.dpDiv.css({ top: rect.top + input.offsetHeight + scrollTop });
        //    }, 0);
        //}

        $("#FromDate").datepicker({
            //dateFormat: "dd-M-yy",
            //changeYear: true,
            //changeMonth: true,
            //minDate: new Date(),
            //onSelect: function (selected) {
            //    var frmdt = selected.split("-").reverse().join("/");
            //    var dt = new Date(frmdt);
            //    var endDate = $("#ToDate").val();
            //    var dtnewdate1 = endDate.split("-").reverse().join("/");
            //    dt.setDate(dt.getDate());// + 1
            //    //  $("#ToDate").datepicker("option", "minDate", dt || 1);
            //    if (dt > new Date(dtnewdate1)) {
            //        $("#ToDate").datepicker("setDate", dt);
            //    }
            //},
            //beforeShow: function (textbox, instance) {
            //    var date = $("#ToDate").datepicker('getDate');
            //    if (date) {
            //        date.setDate(date.getDate()); //- 1
            //    }
            //    // $('#FromDate').datepicker('option', 'maxDate', date);
            //    var rect = textbox.getBoundingClientRect();
            //    setTimeout(function () {
            //        var scrollTop = $("body").scrollTop();
            //        instance.dpDiv.css({ top: rect.top + textbox.offsetHeight + scrollTop });
            //    }, 0);
            //}
            dateFormat: "dd-mm-yy",
            changeYear: true,
            changeMonth: true,
            minDate: new Date(),
            onSelect: function (selected) {
                var frmdt = selected.split("-").reverse().join("/");
                var dt = new Date(frmdt);
                var endDate = $("#ToDate").val();
                var dtnewdate1 = endDate.split("-").reverse().join("/");
                dt.setDate(dt.getDate());// + 1
                //  $("#ToDate").datepicker("option", "minDate", dt || 1);
                if (dt > new Date(dtnewdate1)) {
                    $("#ToDate").datepicker("setDate", dt);
                }
            },
            //beforeShow: function (textbox, instance) {
            //    var date = $("#ToDate").datepicker('getDate');
            //    if (date) {
            //        date.setDate(date.getDate()); //- 1
            //    }
            //    // $('#FromDate').datepicker('option', 'maxDate', date);
            //    var rect = textbox.getBoundingClientRect();
            //    setTimeout(function () {
            //        var scrollTop = $("body").scrollTop();
            //        instance.dpDiv.css({ top: rect.top + textbox.offsetHeight + scrollTop });
            //    }, 0);
            //}
            //beforeShow: function (input, inst) {
            //    var rect = input.getBoundingClientRect();
            //    setTimeout(function () {
            //        //var scrollTop = $("body").scrollTop();
            //        inst.dpDiv.css({ top: -(rect.top + textbox.offsetHeight ) });
            //    }, 0);
            //}
            //beforeShow: function (input, inst) {
            //    var $this = $(this);
            //    var cal = inst.dpDiv;
            //    var top = $this.offset().top + $this.outerHeight();
            //    var left = $this.offset().left;

            //    setTimeout(function () {
            //        cal.css({
            //            'top': top,
            //            'left': left
            //        });
            //    }, 10);
            //}
            beforeShow: function (a, b) {
                var cnt = 0;
                var interval = setInterval(function () {
                    cnt++;
                    if (b.dpDiv.is(":visible")) {
                        var parent = b.input.closest("div");
                        b.dpDiv.position({ my: "left top", at: "left bottom", of: parent });
                        clearInterval(interval);
                    } else if (cnt > 50) {
                        clearInterval(interval);
                    }
                }, 10);
            }
        });
        $("#ToDate").datepicker({
            //dateFormat: "dd-M-yy",
            //changeYear: true,
            //changeMonth: true,
            //minDate: new Date(),
            //onSelect: function (selected) {
            //    //selected = selected.replaceAll('-', ' ');
            //    //var dt = new Date(selected);
            //    //dt.setDate(dt.getDate() - 1);
            //    //$("#FromDate").datepicker("option", "maxDate", dt);
            //    var date = $(this).datepicker('getDate');
            //    if (date) {
            //        date.setDate(date.getDate()); //- 1
            //    }
            //    //  $('#FromDate').datepicker('option', 'maxDate', date );
            //},
            //beforeShow: function (textbox, instance) {
            //    var date = $("#FromDate").datepicker('getDate');
            //    if (date) {
            //        date.setDate(date.getDate()); //- 1
            //    }
            //    //  $("#ToDate").datepicker("option", "minDate", date);
            //    var rect = textbox.getBoundingClientRect();
            //    setTimeout(function () {
            //        var scrollTop = $("body").scrollTop();
            //        instance.dpDiv.css({ top: rect.top + textbox.offsetHeight + scrollTop });
            //    }, 0);
            //}
            dateFormat: "dd-mm-yy",
            changeYear: true,
            changeMonth: true,
            minDate: new Date(),
            onSelect: function (selected) {
                //selected = selected.replaceAll('-', ' ');
                //var dt = new Date(selected);
                //dt.setDate(dt.getDate() - 1);
                //$("#FromDate").datepicker("option", "maxDate", dt);
                var date = $(this).datepicker('getDate');
                if (date) {
                    date.setDate(date.getDate()); //- 1
                }
                //  $('#FromDate').datepicker('option', 'maxDate', date );
            },
            //beforeShow: function (textbox, instance) {
            //    var date = $("#FromDate").datepicker('getDate');
            //    if (date) {
            //        date.setDate(date.getDate()); //- 1
            //    }
            //    //  $("#ToDate").datepicker("option", "minDate", date);
            //    var rect = textbox.getBoundingClientRect();
            //    setTimeout(function () {
            //        var scrollTop = $("body").scrollTop();
            //        instance.dpDiv.css({ top: rect.top + textbox.offsetHeight + scrollTop });
            //    }, 0);
            //}
            //beforeShow: function (input, inst) {
            //    var rect = input.getBoundingClientRect();
            //    setTimeout(function () {
            //        var scrollTop = $("body").scrollTop();
            //        inst.dpDiv.css({ top: -(rect.top + textbox.offsetHeight + scrollTop) });
            //    }, 0);
            //}
            beforeShow: function (a, b) {
                var cnt = 0;
                var interval = setInterval(function () {
                    cnt++;
                    if (b.dpDiv.is(":visible")) {
                        var parent = b.input.closest("div");
                        b.dpDiv.position({ my: "left top", at: "left bottom", of: parent });
                        clearInterval(interval);
                    } else if (cnt > 50) {
                        clearInterval(interval);
                    }
                }, 10);
            }
        });

        $("#GrantedBy").autocomplete({
        source: function (request, response) {
            ;
				$.ajax({
					url: '@Url.Action("OrganisationSummary", "Dispensation")',
					dataType: "json",
					data: {
                        SearchString: request.term, page: 0, pageSize: 0
					},
                    success: function (data) {
                        ;
                        response($.map(data, function (item) {
                            return { label: item.OrganisationName, value: item.OrganisationId };
                        }));
   					},
					error: function (jqXHR, exception, errorThrown) {
						console.log(errorThrown);
					}
				});
			},
			minLength: 2,
			select: function (event, ui) {
				// Set selection
                $('#GrantedBy').val(ui.item.label); // display the selected text
                $('#SelectOrganisationId').val(ui.item.value); // save selected id to input
               		return false;
			},
			focus: function (event, ui) {
                $("#GrantedBy").val(ui.item.label);
				return false;
			}
          });

        RemoveError();
        ShowVehicleRestrications();

        $("#Gross").keypress(function (evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            return !(charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57));
        });
        $("#Axle").keypress(function (evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            return !(charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57));
        });
        $("#Width").keypress(function (evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            return !(charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57));
        });
        $("#Height").keypress(function (evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            return !(charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57));
        });
        $("#Length").keypress(function (evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            return !(charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57));
        });

        $("#Gross").change(function (e) {
            var a = $('#Gross').val();
            if (a > 99999999) {
                $('#errorChk').text("Gross weight exceeds the maximum allowed weight 99999999 kg");
                $('#Axle').attr('readonly', true);
                $('#Height').attr('readonly', true);
                $('#Width').attr('readonly', true);
                $('#Length').attr('readonly', true);
                flag = 1;
            }
            else {
                $('#errorChk').text("");
                $('#Axle').attr('readonly', false);
                $('#Height').attr('readonly', false);
                $('#Width').attr('readonly', false);
                $('#Length').attr('readonly', false);
                flag = 0;
            }
        });
        $("#Axle").change(function (e) {
            var a = $('#Axle').val();
            if (a > 99999999) {
                $('#errorChk').text("Axle weight exceeds the maximum allowed weight 99999999 kg");
                $('#Gross').attr('readonly', true);
                $('#Height').attr('readonly', true);
                $('#Width').attr('readonly', true);
                $('#Length').attr('readonly', true);
                flag = 1;
            }
            else {
                $('#errorChk').text("");
                $('#Gross').attr('readonly', false);
                $('#Height').attr('readonly', false);
                $('#Width').attr('readonly', false);
                $('#Length').attr('readonly', false);
                flag = 0;
            }
        });
        $("#Length").change(function (e) {
            var len = $("#Length").val();
            if (len > 999.99) {
                $('#errorChk').text("Length exceeds the maximum allowed length 999.999 m");
                $('#Axle').attr('readonly', true);
                $('#Height').attr('readonly', true);
                $('#Width').attr('readonly', true);
                $('#Gross').attr('readonly', true);
                flag = 1;
            }
            else {
                $('#errorChk').text("");
                $('#Axle').attr('readonly', false);
                $('#Height').attr('readonly', false);
                $('#Width').attr('readonly', false);
                $('#Gross').attr('readonly', false);
                flag = 0;
            }
        });
        $("#Width").change(function (e) {
            var len = $("#Width").val();
            if (len > 999.99) {
                $('#errorChk').text("Width exceeds the maximum allowed width 999.999 m");
                $('#Axle').attr('readonly', true);
                $('#Height').attr('readonly', true);
                $('#Length').attr('readonly', true);
                $('#Gross').attr('readonly', true);
                flag = 1;
            }
            else {
                $('#errorChk').text("");
                $('#Axle').attr('readonly', false);
                $('#Height').attr('readonly', false);
                $('#Length').attr('readonly', false);
                $('#Gross').attr('readonly', false);
                flag = 0;
            }
        });
        $("#Height").change(function (e) {
            var len = $("#Height").val();
            if (len > 999.99) {
                $('#errorChk').text("Height exceeds the maximum allowed Height 999.999 m");
                $('#Axle').attr('readonly', true);
                $('#Width').attr('readonly', true);
                $('#Length').attr('readonly', true);
                $('#Gross').attr('readonly', true);
                flag = 1;
            }
            else {
                $('#errorChk').text("");
                $('#Axle').attr('readonly', false);
                $('#Width').attr('readonly', false);
                $('#Length').attr('readonly', false);
                $('#Gross').attr('readonly', false);
                flag = 0;
            }
        });

    });

    @*AntiForgeryTokenInclusionRequest('@Html.AntiForgeryToken()');*@

    $("#btn_saveDispensation").click(function () {
        //
        var fromAffectedParties = '@fromAffectedParties';
        var mode  = $('#hf_mode').val(); 
        var Granter_ID = $('#hdnGranter_ID').val();
        var hideLayout  = $('#hf_hidelayout').val(); 
        if (DispensationValidation()) {
            $.ajax({
                url: '../Dispensation/CreateDispensation',
                type: 'POST',
                data: $("#DispensationInfo").serialize() + "&mode=" + mode + "&fromAffectedParties=" + fromAffectedParties + "&Granter_ID=" + Granter_ID + "&hideLayout=" + hideLayout,
                success: function (result) {
                        if (document.getElementById("General") !== null) {
                            $('#div_dispensation').hide();
                            $('#div_dispensation').html(result);
                            $("#General").html('');
                            var error = $("#ErrorPage").val();
                            if (error == 'True') {
                                $('#div_dispensation').html('');
                                $("#General").html(result);
                                if ('@fromAffectedParties' == 'True') {
                                    $('#divCreateDispensation').unwrap('#banner-container');
                                }
                            }
                            else {
                                $('#confirm_btn').removeAttr("disabled");
                                $('#back_btn').show();
                                $("#General").hide();
                                $('#route-assessment').show();
                                $("#btncreatedispensation").hide();
                                $('#div_dispensation').show();
                                ViewDispensationAffParties('@Session["Granter_ID"]', '@Session["GranterName"]');
                            }
                        }
                        else {
                            if (!result.Success) {
                                $('body').html(result);
                            }
                            else {
                                var url = "../Dispensation/ManageDispensation";
                                window.location.href = url;
                            }
                        }
                },
                error: function (xhr, status) {
                }
            });
        }
    });

    $('#btn_cancel').click(function () {
        if ('@fromAffectedParties' == 'True') {
             $("#General").hide();
             $('#route-assessment').show();
             $("#btncreatedispensation").hide();
            $('#div_dispensation').show();
            ViewDispensationAffParties('@Session["Granter_ID"]', '@Session["GranterName"]');
            $('#confirm_btn').removeAttr("disabled");
            $('#back_btn').show();
        }
        else {
            var url = "../Dispensation/ManageDispensation";
            window.location.href = url;
        }

    });

    function RemoveError() {
        $('body').find('input:text').click(function () {
            $(this).closest('tr').find('.field-validation-error').text('');
            $(this).closest('tr').find('.field-validation-error span').text('');
        });
    }

    function DispensationValidation() {
        var count = true;
        if (!chkvalid()) {
            count = false;
        }
        return count;
    }

    var isValid1 = true;
    function chkvalid() {
        var isValiddate = ValidateDate();
        return isValiddate;
    }

    function ValidateDate() {

        var isValid = true;

        var dt1 = $('#FromDate').val();
        var dt2 = $('#ToDate').val();
        var dtnewdate1 = dt1.split("-").join("/");
        var dtnewdate2 = dt2.split("-").join("/");

                var d = new Date();
                var month = d.getMonth() + 1;
                var day = d.getDate();
                var hour = d.getHours();
                var minute = d.getMinutes();
                var second = d.getSeconds();
                var output = (('' + day).length < 2 ? '0' : '') + day + '/' +
                    (('' + month).length < 2 ? '0' : '') + month + '/' +
                    d.getFullYear() + ' ' +
                    (('' + hour).length < 2 ? '0' : '') + hour + ':' +
                    (('' + minute).length < 2 ? '0' : '') + minute;


        var ArrayDateTime1 = dtnewdate1.split(' ');
                var ArrayDMY1 = ArrayDateTime1[0].split('/');
                var NewDate1 = new Date(ArrayDMY1[2], ArrayDMY1[1], ArrayDMY1[0], 0, 0, 0, 0);
                var TimeOfDate1 = NewDate1.getTime();

        var ArrayDateTime2 = dtnewdate2.split(' ');
                var ArrayDMY2 = ArrayDateTime2[0].split('/');
                var NewDate2 = new Date(ArrayDMY2[2], ArrayDMY2[1], ArrayDMY2[0], 0, 0, 0, 0);
                var TimeOfDate2 = NewDate2.getTime();

                var ArrayDateTime3 = output.split(' ');
                var ArrayDMY3 = ArrayDateTime3[0].split('/');
                var NewTodayDate = new Date(ArrayDMY3[2], ArrayDMY3[1], ArrayDMY3[0], 0, 0, 0, 0);
                var TimeOfTodayDate = NewTodayDate.getTime();
                if (ArrayDMY1[0] == "31" && ArrayDMY2[0] == "01") {
                    if (ArrayDMY1[1] < ArrayDMY2[1]) {
                        $('#spnFromDate').html('');
                        $('#spnToDate').html('');

                    }
                }
                else {
                    // ensure both evaluate to true
                    if (dt1 && dt2) {
                        if (TimeOfDate1 > TimeOfDate2) {
                            $('#spnToDate').html('To date must be greater than from date.');
                            $('#spnFromDate').html('');
                            isValid = false;
                        }
                        else {
                            //if (TimeOfDate1 < TimeOfTodayDate) {
                            //    $('#spnFromDate').html('Date must be today\'s date or greater than today\'s date.');
                            //    $('#spnToDate').html('');
                            //    isValid = false;
                            //}
                            //else {
                                $('#spnFromDate').html('');
                                $('#spnToDate').html('');

                           // }
                        }
                    }
                }
            //}

            return isValid;
    }


