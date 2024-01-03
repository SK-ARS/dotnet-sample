    SelectMenu(3);
    var listContactName = [];
    $(document).ready(function () {
       
        $("#btnsearchroads").click(searchRoadLinks);
       
        $("#map").mouseenter(mouseoveron);
        $("#map").mouseleave(mouseoveroff);
        $("#mapdata").mouseenter(mouseoveron);
        $("#mapdata").mouseleave(mouseoveroff);
        $("#btnmap").click(EnLargeMap);
        $("#owned").click(ownedRoads);
        $("#managed").click(managedRoads);
        $("#btnback").click(BackRoadDelegationList);
        $("#savedelegdata").click(saveRoadDelegDetails);
		if (@Model.DelegateAll == 0) {
			//document.getElementById("delAll").checked = true;

		}
		$("#txtDelegatorOrganisation").autocomplete({

            source: function (request, response) {
				$.ajax({
					url: '@Url.Action("ListOrganisation", "RoadDelegation")',
					dataType: "json",
					data: {
						SearchString: request.term
					},
                    success: function (data) {

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
				$('#txtDelegatorOrganisation').val(ui.item.label); // display the selected text
                $('#hdnFromOrgId').val(ui.item.value); // save selected id to input
                road_from_map();
                $('#movement-map').show();
                $('#inboxCount').show();


				return false;
			},
			focus: function (event, ui) {
				$("#txtDelegatorOrganisation").val(ui.item.label);
				return false;
			}
		});


		$("#txtDelegateOrganisation").autocomplete({

            source: function (request, response) {

				$.ajax({
					url: '@Url.Action("ListOrganisation", "RoadDelegation")',
					dataType: "json",
					data: {
						SearchString: request.term
					},
                    success: function (data) {
                        if (data.length > 0) {
                            data.forEach((element) => {
                                var innerArray = [];

                                innerArray.push(element.OrganisationId);
                                innerArray.push(element.ContactName);
                                listContactName.push(innerArray);


                            });

                        }
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
				$('#txtDelegateOrganisation').val(ui.item.label); // display the selected text
                $('#hdnToOrgId').val(ui.item.value); // save selected id to input
                listContactName.forEach((element) => {
                    if (ui.item.value == element[0]) {
                        $('#toContactName').val(element[1]);

                    }
                });



				return false;
			},
			focus: function (event, ui) {
				$("#txtDelegateOrganisation").val(ui.item.label);
				return false;
			}
		});

		if (@Model.ArrangementId> 0) {

        }

if($('#hf_mode').val() ==  "Create") {
            $('#movement-map').hide();
            $('#inboxCount').hide();

        }
if($('#hf_mode').val() ==  "View") {
            $('#savedelegdata').hide();
        }
if($('#hf_mode' == "Edit" && (mode').val() ==  null && model.arrangementId != 0)) {
            $('#savedelegdata').show();
            //$("#search_from_org").hide();
        }

        if ($('#DelegationId').val() > 0) {

            if (model.AllowSubdelegation == 1)
                $("#sub_delegation").prop("checked", true);

            else
                $('#sub_delegation').prop('checked', false)
            if (model.RetainNotification == 1)
                $("#reNotify").prop("checked", true);

            else
                $('#reNotify').prop('checked', false)
            if (model.AcceptFailure == 1)
                $('#rdoYes').prop('checked', true)

            else if (model.AcceptFailure == 0)
                ($('#rdoNo').prop('checked', true))

        }

		if (@ViewBag.delegId == 0) {
			$("#map").html('');
			$("#map").load('@Url.Action("A2BPlanning", "Routes", new { routeID = 0 })', function () {
				loadmap('DELEGATION_VIEWANDEDIT');

			});
		}
        else {
			var linkInfo = null;
			@*$.ajax({
				url: '../GetRoadDelegationDetails',
				dataType: 'json',
				data: { arrangementId:@ViewBag.delegId, fetchFlag: 0, areaGeom: null, searchParam:null },
                        	success: function (result) {
								linkInfo = result;
								if (linkInfo.length > 0) {
                                //$('#CreateViewDiv').hide();
                                showDelegationOnMap(linkInfo);
                            }
                            else {
                                location.reload();
                            }
                        },
                        error: function (result) {
                        }
                    });*@

          road_from_map();
		}

	});
    $('body').on('keypress', '#txtLinkSearch', function (e) {
        e.preventDefault();
        handleSearch(e, this);
    });
    $('body').on('click', '#search_toorgContact', function (e) {
        e.preventDefault();
        var id = $(this).attr('id');
        ContactSummary(id);
    });
    $('body').on('click', '#search_fromorgContact', function (e) {
        e.preventDefault();
        var id = $(this).attr('id');
        ContactSummary(id);
    });
    function ownedRoads() {
        if ($("#owned").is(":checked")) {
            showAndHideRoads('owned', true);
        }
        else {
            showAndHideRoads('owned', false);
        }
    }

    //function handleSearch(e, obj) {
    //    if (e.keyCode === 13) {
    //        var text = $.trim($(obj).val());
    //        searchLinks(text);
    //    }
    //}

    function searchRoadLinks() {
        var linkid = $.trim($('#txtLinkSearch').val());
        //if (linkid != "" && linkid != null) {
            searchLinks(linkid);
        //}
    }

    function managedRoads() {
        if ($("#managed").is(":checked")) {
            showAndHideRoads('managed', true);
        }
        else {
            showAndHideRoads('managed', false);
        }
    }

    function road_from_map() {
        //condition to verify whether delegator organisation is selected or not
        if ($('#hdnFromOrgId').val() != 0) {

            if ($('#mapViewFlag').val() == "true" && ($('#Delegatororg').val() == $('#txtDelegatorOrganisation').val())) {
                $("#Map_deleg").show();
                mapResize();
                $('#CreateViewDiv').hide();
                $("#pageheader").hide();
                $('#leftDiv').show();
                $('#searchLink').show();
            }
            else {
                //startAnimation("");

                $('#mapViewFlag').val("true");
                if ($('#mode').val() == "View" || $('#mode').val() == "Edit") {
                    var DelegArrangId = $('#DelegationId').val();
                    var linkInfo = null;
                    $.ajax({
                        async: false,
                        type: "POST",
                        url: '@Url.Action("GetRoadDelegationDetails", "RoadDelegation")',
                        dataType: "json",
                        //contentType: "application/json; charset=utf-8",
                        data: JSON.stringify({ arrangementId: DelegArrangId, fetchFlag: 0 }),
                        processdata: true,
                        success: function (result) {
                            linkInfo = result;
                            if (linkInfo.length > 0) {
                                $('#CreateViewDiv').hide();
                                showDelegationOnMap(linkInfo);
                            }
                            else {
                                location.reload();
                            }
                        },
                        error: function (result) {
                        }
                    });
                }
                else {
                    $('#CreateViewDiv').hide();
                    showDelegationOnMap();
                }
            }
        }
        else {
            showWarningPopDialog('Please select delegator organisation before proceeding for road selection.', 'Ok', '', 'WarningCancelBtn', '', 1, 'warning')
        }
    }



	$("#txtDelegateOrganisation").blur(function () {

		if ($("#hdnTriggerflag").val() == 0) {

			$("#hdnTriggerflag").val(1);
		}
	});

	function openNav() {
		document.getElementById("mySidenav").style.width = "320px";
		document.getElementById("banner").style.filter = "brightness(0.5)";
		document.getElementById("banner").style.background = "white";
		document.getElementById("navbar").style.filter = "brightness(0.5)";
		document.getElementById("navbar").style.background = "white";
		function myFunction(x) {
			if (x.matches) { // If media query matches
				document.getElementById("mySidenav").style.width = "200px";
			}
		}
		var x = window.matchMedia("(max-width: 992px)")
		myFunction(x) // Call listener function at run time
		x.addListener(myFunction)

	}

	function closeNav() {
		document.getElementById("mySidenav").style.width = "0";
		document.getElementById("banner").style.filter = "unset"
		document.getElementById("navbar").style.filter = "unset";
	}

	// showing user-setting inside vertical menu
	function showuserinfo() {
		if (document.getElementById('user-info').style.display !== "none") {
			document.getElementById('user-info').style.display = "none"
		}
		else {
			document.getElementById('user-info').style.display = "block";
			document.getElementsById('userdetails').style.overFlow = "scroll";
		}
	}
	// showing user-setting-info-filter

	// showing filter-settings
	function openFilters() {
		document.getElementById("filters").style.width = "400px";
		document.getElementById("banner").style.filter = "brightness(0.5)";
		document.getElementById("banner").style.background = "white";
		document.getElementById("navbar").style.filter = "brightness(0.5)";
		document.getElementById("navbar").style.background = "white";
		function myFunction(x) {
			if (x.matches) { // If media query matches
				document.getElementById("filters").style.width = "200px";
			}
		}
		var x = window.matchMedia("(max-width: 770px)")
		myFunction(x) // Call listener function at run time
		x.addListener(myFunction)
	}
	//function closeFilters() {
	//	document.getElementById("filters").style.width = "0";
	//	document.getElementById("banner").style.filter = "unset"
	//	document.getElementById("navbar").style.filter = "unset";

	//}
	// showing filter-settings




	// view vehicle configuration summary
	//function vehicleSummary() {
	//	if (document.getElementById('vechicleSummary').style.display !== "none") {
	//		document.getElementById('vechicleSummary').style.display = "none"
	//		document.getElementById('chevlon-up-icon1').style.display = "none"
	//		document.getElementById('chevlon-down-icon1').style.display = "block"
	//	}
	//	else {
	//		document.getElementById('vechicleSummary').style.display = "block"
	//		document.getElementById('chevlon-up-icon1').style.display = "block"
	//		document.getElementById('chevlon-down-icon1').style.display = "none"
	//	}
	//}
	// view vehicle configuration summary

	// show print options
	//function printOptions() {
	//	if (document.getElementById('printOptions').style.display !== "none") {
	//		document.getElementById('printOptions').style.display = "none"
	//	}
	//	else {
	//		document.getElementById('printOptions').style.display = "block"
	//	}
	//}

	function BackRoadDelegationList() {

		if ($('#originpage').val() == 'SOA')
            window.location.href = '@Url.Action("ShowRoadDelegation", "Structures")';
        else
            window.location.href = '@Url.Action("GetRoadDelegationList", "RoadDelegation")';

	};





	function SelectByLinkRD() {
		if (objifxStpMap.eventList['DEACTIVATECONTROL'] != undefined && typeof (objifxStpMap.eventList['DEACTIVATECONTROL']) === "function") {
			objifxStpMap.eventList['DEACTIVATECONTROL'](2, "ROADDELEGATION");
		}
		btnSelectByLink.activate();
		objifxstpmap.createClickControl();
	}




    var model = @Html.Raw(Json.Encode(Model))

        $(document).ready(function () {
         
		//removescroll();
        //$('#validarranName').hide();
        //$('#tovalidorganisation').hide();
        //$('#fromvalidorganisation').hide();

       
    });
  
    function showDelegationOnMap(linkInfo) {
        var organisationId = $('#hdnFromOrgId').val();
		$("#map").html('');
		$("#map").load('@Url.Action("A2BPlanning", "Routes", new { routeID = 0 })', function () {

            if ($('#mode').val() == "View") {
                loadmap('DELEGATION_VIEWONLY');
                if (linkInfo != null && linkInfo != undefined) {
                    var linkIndex;
                    if (linkInfo.length == 1) {
                        linkIndex = 0;
                    }
                    else {
                        linkIndex = Math.round(linkInfo.length / 2);
                    }
					zoomInToDelegRoad(linkInfo[linkIndex], @ViewBag.delegId);
                }
                else {
                    showNotification('No data available.');
                }
            }
            else if ($('#mode').val() == "Edit") {
                loadmap('DELEGATION_VIEWANDEDIT');
                if (linkInfo != null && linkInfo != undefined) {
                    addToDelegatedRoadLinks(linkInfo);
                    var linkIndex;
                    if (linkInfo.length == 1) {
                        linkIndex = 0;
                    }
                    else {
                        linkIndex = Math.round(linkInfo.length / 2);
                    }
                    showOrganisationRoads(organisationId);
					zoomInToDelegRoad(linkInfo[linkIndex],  @ViewBag.delegId, function () {
                       // showAllRoads(false);
                    });

                }
                else {
                    showNotification('No data available.');
                }
			}
            else {
                loadmap('DELEGATION_VIEWANDEDIT');
                showAllRoads(true);
                //$('#leftDiv').show();
                //$('#searchLink').show();
            }
            showOrganisationRoads(organisationId);
            CheckSessionTimeOut();
            Map_size_fit();
            mapResize();
        });
	}
    var pDelObj = "";

    function saveRoadDelegDetails() {
		if ($('#sub_delegation').attr('checked'))
			$('#allowSubdelegation').val(1);
		else
            $('#allowSubdelegation').val(0);

        if ($("#reNotify").is(":checked") == true)
			$('#retainNotification').val(1);
		else
            $('#retainNotification').val(0);

        if ($("#delAll").is(":checked") == true)
			$('#delegateAll').val(1);
		else
			$('#delegateAll').val(0);

		if ($('#rdoYes').attr('checked'))
			$('#AcceptFailure').val(1);
		else if ($('#rdoNo').attr('checked'))
			$('#AcceptFailure').val(0);
        var val = validateFlds();
		if (val == true) {
			startAnimation("");
			getDelegationInformation(function (delObj) {
                var fromId = $('#hdnFromOrgId').val();
				if ($('#delAll').is(':checked')) {
					$.ajax({
						url: '../../RoadDelegation/CheckPartialDelegation',
						type: 'POST',
						datatype: 'json',
						async: true,
						data: { orgId: fromId },
						success: function (result) {
							if (result.delegateAll == 1) {
								saveDelegDetails(delObj);
							}
							else {
                                pDelObj = delObj;
                                var message = getHtmlMessage(result.delegDetails);
                                //saveDelegDetails(pDelObj);
                                if (Popheight == 15) {
                                    saveDelegDetails(pDelObj);
                                }
                                else {
                                    ShowWarningPopup(message, 'saveDelegDetails(pDelObj)');
                                    document.getElementById("div_WarningMessage").innerHTML = message;
                                    $("#WarningContent").css("height", Popheight + "rem");
									stopAnimation();
                                }


                                //showWarningPopDialogBig(message, 'No', 'Yes', 'warnclose', 'saveDelegDetails(pDelObj)', 1, 'info');
                                //showWarningPopDialog(message, 'Ok', '', 'OK', 'error');

								//stopAnimation();
							}
						},

					});
				}
				else {
					saveDelegDetails(delObj);
				}
			});

		}
		else
			return;
    }
    var Popheight = 15;
    function getHtmlMessage(delegDetails) {
        Popheight = 15;
        var message = '<div style="text-align: left;padding-left: 1rem;padding-bottom: 0rem;" class="edit-normal">Active delegations are</div>';
        var tr = '<ul style="text-align: initial; margin-bottom: -3rem;padding-bottom:2rem;">';
        for (var i = 0; i < delegDetails.length; i++) {
            var j = i + 1;
            //delegDetails[i].arrangementName = j + " " + delegDetails[i].arrangementName;
            tr += '<li class="edit-normal" style="padding:0rem;">' + delegDetails[i].ArrangementName + '</li>';
            Popheight = Popheight + 2;
        }
        tr += '</ul>';

        var htmlmessage = "Following partial delegation(s) are already exists for the delegator organisation and these will be deleted. Do you want to proceed?" + message + tr;
        return htmlmessage;

    }

    function validateFlds() {
        //var e = document.getElementById("fromContactType");
        //var selectedText1 = e.options[e.selectedIndex].text;
        //var e = document.getElementById("toContactType");
        //var selectedText2 = e.options[e.selectedIndex].text;
        var flag = true;
        if ($('#arrangName').val() == '') {
            $('#validarranName').show();
            flag = false;
            return;
        }
        else {
            $('#validarranName').hide();
            flag = true;
        }

        if ($('#txtDelegatorOrganisation').val() == '') {
            $('#fromvalidorganisation').show();
            flag = false;
            return;
        }
        else
            flag = true;

        //if (selectedText1 == "Search new contact") {

        //    if ($('#fromContactName').val() == '') {
        //        $('#validfromContacttype').show();
        //        flag = false;
        //        return;
        //    }
        //    else
        //        flag = true;
        //}
        if ($('#txtDelegateOrganisation').val() == '') {
            $('#tovalidorganisation').show();
            flag = false;
            return;
        }
        else
            flag = true;

        //if (selectedText2 == "Search new contact") {
        //    if ($('#toContactName').val() == '') {
        //        $('#validtoContacttype').show();
        //        flag = false;
        //        return;
        //    }
        //    else
        //        flag = true;
        //}

        return flag;
    }

    function getDelegationInformation(callback) {
        var delObj = {
            arrangementId: $('#DelegationId').val(),
            ArrangementName : $('#arrangName').val(),
            fromOrgId: $('#hdnFromOrgId').val(),
            fromOrgName: $('#txtDelegatorOrganisation').val(),
            fromContactId: $('#fromContactId').val(),
            toOrgId: $('#hdnToOrgId').val(),
            toOrgName: $('#txtDelegateOrganisation').val(),
            toContactId: $('#toContactId').val(),
            retainNotification: $('#retainNotification').val(),
            allowSubdelegation: $('#allowSubdelegation').val(),
            acceptFailure: $('#AcceptFailure').val(),
            delegateAll: $('#delegateAll').val(),
            //delegateAll: model.delegateAll,
            linkIdInfo: []
        };

        if ($('#mapViewFlag').val() == "false" && $('#mode').val() == "Edit") {
            getDelegLinkInfo(function (data) {
                delObj.linkIdInfo = data;
                callback(delObj);
            });
        }
        else {

            if ($("#delAll").is(":checked") == false)
                delObj.linkIdInfo = getDelegLinkInfoList();
            callback(delObj);
        }
    }
    $("#delAll").change(function () {
        //showWarningPopDialog('This functionality is not implemented', 'Ok', '', 'warnclose', '', 1, 'info');
        //$('#delAll').attr('checked', false);
        //return;
        ;
        var fromId = $('#hdnFromOrgId').val();
        if ($('#hdnFromOrgId').val() != 0) {
            if ($("#delAll").is(":checked") == true) {
                $.ajax({
                    url: '@Url.Action("IsDelegationAllowed", "RoadDelegation")',
                    type: 'POST',
                    datatype: 'json',
                    async: true,
                    data: { orgId: fromId },
                    success: function (value) {

                        if (value.val == true) {
                            //$('#savedelegdata').show();
                            //$("#road_from_map").hide();
                            //$("#movement-map").css('visibility', 'hidden');
                            $('#movement-map').hide();
                            $('#inboxCount').hide();

                        }
                        else {
                            ShowWarningPopup('Sorry,please select roads from map', "CloseWarningPopup()");

                            $('#delAll').prop('checked', false)
                        }

                    },
                    error: function () {
                        //location.reload();
                    },
                    complete: function () {

                    }
                });
            }
            else {
                //$('#savedelegdata').hide();
                //$("#road_from_map").show();
                //$("#movement-map").css('visibility', 'visible');
                $('#movement-map').show();
                $('#inboxCount').show();

                return;
            }


        }
        else {
            //showWarningPopDialog('Please select delegator organisation before proceeding for road selection.', 'Ok', '', 'WarningCancelBtn', '', 'warnclose', 'warning')
            $('#delAll').prop('checked', false); // Unchecks it

            ShowWarningPopup('Please select delegator organisation before proceeding for road selection.', "CloseWarningPopup()");

        }
    });

    function saveDelegDetails(delObj) {
        
        startAnimation("");
        $('#WarningPopup').modal('hide');
		if ($('#mode').val() == "Edit") {
			var url = '../../RoadDelegation/UpdateRoadDelegation';
			var successMsg = 'Road delegation updated successfully';
			var failureMsg = 'Updating failed. Please retry';
			var errorMsg = 'Updating failed. Please retry';
		}
		else {
			var url = '../../RoadDelegation/SaveRoadDelegation';
			var successMsg = 'Road delegation saved successfully';
			var failureMsg = 'Road delegation creation failed';
			var errorMsg = 'Saving failed. Please retry';
        }
        ;
		var newDelegObj = delObj;
		var linkIdInfo = delObj.linkIdInfo;
		delete newDelegObj.linkIdInfo;
		//condition to save when delegate all is selected
        if ($('#delegateAll').val() == 1) {
            
            $("#WarningContent").css("height", "15rem");

			$.ajax({
				url: url,
				//contentType: 'application/json; charset=utf-8',
				type: 'POST',
				datatype: "json",
				data: JSON.stringify({ postFlag: -2, roadDelegationObject: newDelegObj, roadLinkInfo: null, len: -1 }),
				beforeSend: function () {
					startAnimation("");
				},
                success: function (result) {
                    
					if (result.result) {
                        startAnimation();
                        if (result.value == true) {
							stopAnimation();
                            //showWarningPopDialog(successMsg, 'Ok', '', 'Close', '', 1, 'info');
                            ShowSuccessModalPopup(successMsg, "BackRoadDelegationList()");
                            
                            $("body").on('click', '.close', function () { window['BackRoadDelegationList'](); })
                            $('#SuccessPopupAction').modal({ backdrop: 'static', keyboard: false })

						}
						else {
                            ShowWarningPopup(successMsg, "CloseWarningPopup()");

							//showWarningPopDialog(failureMsg, 'Ok', '', 'OK', 'error');
						}
					}
				},
                error: function (xhr, error, status) {
                    stopAnimation();
				},
				complete: function () {
					//stopAnimation();
				}
			});
		}
        else {
            ;
            
			$.ajax({
				url: url,
				//contentType: 'application/json; charset=utf-8',
				type: 'POST',
				datatype: "json",
				data: JSON.stringify({ postFlag: -1, roadDelegationObject: newDelegObj, roadLinkInfo: null, len: linkIdInfo.length }),
				beforeSend: function () {
					startAnimation("");
				},
				success: function (result) {
					for (var i = 0; i < (Math.floor(linkIdInfo.length / 1000)) + 1; i++) {
						if (i == Math.floor(linkIdInfo.length / 1000))
							var limit = (i * 1000) + (linkIdInfo.length % 1000);
						else
							var limit = (i * 1000) + 1000;

                        var tempList = linkIdInfo.slice(i * 1000, limit);
                        if (tempList.length == 0) {
                            ShowErrorPopup("select road links");
                            return false;

                        }

						$.ajax({
							url: url,
							//contentType: 'application/json; charset=utf-8',
							type: 'POST',
							datatype: "json",
							data: JSON.stringify({ postFlag: i, roadDelegationObject: null, roadLinkInfo: tempList, len: linkIdInfo.length }),
                            beforeSend: function () {
                                startAnimation();
                            },
                            success: function (result) {
								if (result.result) {

									if (result.value == true) {
                                        ShowSuccessModalPopup(successMsg, "BackRoadDelegationList()");
                                        
                                        $("body").on('click', '.close', function () { window['BackRoadDelegationList'](); });
										stopAnimation();
									}
									else {
                                        ShowErrorPopup(failureMsg);
										//showWarningPopDialog(failureMsg, 'Ok', '', 'OK', 'error');
									}
								}
							},
                            error: function (xhr, error, status) {
                                ShowErrorPopup(errorMsg);
								stopAnimation();
							},
							complete: function () {
								  //stopAnimation();
							}
						});

					}

				},
				error: function (xhr, error, status) {
                    //showWarningPopDialog(errorMsg, 'Ok', '', 'OK', 'error');
                    ShowErrorPopup(errorMsg);
					stopAnimation();
				},
				complete: function () {
					 //stopAnimation();
				} 
			});
			//}
		}
    }
 
    function EnLargeMap() {
            if ($("#Minimzeicon").is(":visible")) { //mode changing to full screen
                //fullscreenMode();
                $("#navbar").hide();
                $("#movement-details").hide();
                $("#bottom").hide();
                $("#checkbox").hide();
                $("#soa-portal-map").hide();
                $("#Minimzeicon").hide();
				$("#footerdiv").hide();
                $("#MaxmizeIocn").show();
                //$('body').css('overflow', 'hidden');
                $("#banner-container").addClass("bannercfulls");
                $("#container-sub").addClass("containerfulls");
                $("#helpdeskDelegation").addClass("helpdeskfulls");
                $("#movement-map").addClass("movementfulls");
                $("#mvpmap").addClass("mvpmapfulls");
                $("#map").addClass("mapfulls");
                $("#OpenLayers_Control_Panel_200").addClass("horizontalMapRoad");             
                $("#btnmap").addClass("iconfulls");
                $("#helpdeskDelegation").removeClass("row");
                $("#banner-container").removeClass("container-fluid");
                $("#movement-map").removeClass("col-lg-6 col-md-12 col-sm-12 col-xs-12");
                mapResize();
            
            }
            else {
                //mode changes to minimze
                //closeFullscreen();
                $("#navbar").show();
                $("#Minimzeicon").show();
                $("#MaxmizeIocn").hide();
                $("#movement-details").show();
                $("#bottom").show();
                $("#checkbox").show();
                $("#soa-portal-map").show();
                $("#map").removeClass("mapfulls");
                $("#container-sub").removeClass("containerfulls");
                $("#helpdeskDelegation").removeClass("helpdeskfulls");
                $("#movement-map").removeClass("movementfulls");
                $("#mvpmap").removeClass("mvpmapfulls");
                $("#OpenLayers_Control_Panel_200").removeClass("horizontalMapRoad");
                $("#banner-container").removeClass("bannercfulls");
                $("#helpdeskDelegation").addClass("row");
                $("#banner-container").addClass("container-fluid");
                $("#movement-map").addClass("col-lg-6 col-md-12 col-sm-12 col-xs-12");
                $("#overlay_load").addClass("col-lg-6 col-md-12 col-sm-12 col-xs-12");
                $("#btnmap").removeClass("iconfulls");
				$("#footerdiv").show();
				mapResize();

            }
        }

    /* When the openFullscreen() function is executed, open the map in fullscreen.
    Note that we must include prefixes for different browsers, as they don't support the requestFullscreen property yet */
    function fullscreenMode() {
        /* Get the element you want displayed in fullscreen mode*/
        var elem = document.getElementById("mvpmap");
        if (elem.requestFullscreen) {
            elem.requestFullscreen();
        } else if (elem.webkitRequestFullscreen) { /* Safari */
            elem.webkitRequestFullscreen();
        } else if (elem.msRequestFullscreen) { /* IE11 */
            elem.msRequestFullscreen();
        }
    }
    /* Close fullscreen */
    function closeFullscreen() {
        if (document.exitFullscreen) {
            document.exitFullscreen();
        } else if (document.webkitExitFullscreen) { /* Safari */
            document.webkitExitFullscreen();
        } else if (document.msExitFullscreen) { /* IE11 */
            document.msExitFullscreen();
        }
    }

    function mouseoveron() {
        $('body').css('overflow-y', 'hidden');

    }

    function mouseoveroff() {
        $('body').css('overflow-y', 'auto');

    }
