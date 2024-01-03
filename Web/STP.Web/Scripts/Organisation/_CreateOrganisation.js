var isvalid = true;
var isvalidOrg = true;
var mousedownHappened = false;
var flag = true;
var hf_mode;
var hf_sortApp;
var hf_orgID;
var hf_showHaulCnt;

function CreateOrganisationInit() {
    hf_mode = $('#hf_mode').val();
    hf_sortApp = $('#hf_sortApp').val();
    hf_orgID = $('#hf_orgID').val();
    hf_showHaulCnt = $('#hf_showHaulCnt').val();
    if (hf_mode == "Save") {
        document.getElementById("AuthKeySave").readOnly = true;
    }
   
    if (hf_mode == "View") {
        
        $("#" + hf_orgID).find("#desc-entry").hide();
        $("#" + hf_orgID).find("#desc-all-entr").css("display", "flex");
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: '../Organisation/ViewOrganisationByID',
            data: { orgId: hf_orgID, RevisionId: 0 },


            beforeSend: function (xhr) {
                startAnimation();
            },
            complete: function (result) {
                var dataCollection = result.responseJSON.result;
                if (dataCollection.length > 0) {
                    $("#" + hf_orgID).find("#viewOrgName").text(dataCollection[0].OrgName);
                    var organisationType = $('#' + hf_orgID).find('#ViewOrgType option[value=' + dataCollection[0].OrgType + ']').text();
                    $("#" + hf_orgID).find("#viewOrgType").text(organisationType);
                    //$("#" + hf_orgID).find("#viewOrgType").text(dataCollection[0].OrgType);
                    $("#" + hf_orgID).find("#viewOrgCode").text(dataCollection[0].OrgCode);
                    if (dataCollection[0].IsNENsReceive == true) {
                        $("#" + hf_orgID).find("#viewIsNenValidTrue").show();
                    }
                    else {
                        $("#" + hf_orgID).find("#viewIsNenValidFalse").show();
                    }
                    if (dataCollection[0].OrgType != 237013 && dataCollection[0].OrgType != 237014 && dataCollection[0].OrgType != "" && dataCollection[0].OrgType != 237007) {
                        $("#" + hf_orgID).find("#divAlsatAccess").show();
                        if (dataCollection[0].AccessToALSAT == true) {
                            $("#" + hf_orgID).find("#viewAccessToALSATTrue").show();
                        }
                        else {
                            $("#" + hf_orgID).find("#viewAccessToALSATFalse").show();
                        }
                    }
                    $("#" + hf_orgID).find("#viewLicenseNumber").text(dataCollection[0].LicenseNR);
                    $("#" + hf_orgID).find("#viewAddr1").text(dataCollection[0].AddressLine1);
                    $("#" + hf_orgID).find("#viewAddr2").text(dataCollection[0].AddressLine2);
                    $("#" + hf_orgID).find("#viewAddr3").text(dataCollection[0].AddressLine3);
                    $("#" + hf_orgID).find("#viewAddr4").text(dataCollection[0].AddressLine4);
                    $("#" + hf_orgID).find("#viewAddr5").text(dataCollection[0].AddressLine5);
                    $("#" + hf_orgID).find("#viewPostCode").text(dataCollection[0].PostCode);
                    $("#" + hf_orgID).find("#viewPhone").text(dataCollection[0].Phone);
                    //$("#" + hf_orgID).find('#ViewCountryID').val(dataCollection[0].CountryId);
                    var Country = $('#' + hf_orgID).find('#ViewCountryID option[value=' + dataCollection[0].CountryId + ']').text();
                    $("#" + hf_orgID).find("#viewCountry").text(Country);
                    if ($('#hf_sortApp').val() == "SORTSO") {
                        $("#" + hf_orgID).find("#viewEmail").text(dataCollection[0].EmailId);
                        $("#" + hf_orgID).find("#viewFax").text(dataCollection[0].Fax);

                    }
                    $("#" + hf_orgID).find("#viewWebsite").text(dataCollection[0].Web);
                    stopAnimation();
                }

            }

        });
    }

    if (hf_mode == "Edit") {
        var RevisionId = $('#ApprevId').val();
        var organisationID = $('#hf_orgID').val();
        var selectedOrgType = $('#OrgType option:selected').val();
        
        
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: '../Organisation/ViewOrganisationByID',
            data: { orgId: organisationID, RevisionId: RevisionId },
            beforeSend: function (xhr) {
            }
        }).done(function (Result) {
            stopAnimation();
            var dataCollection = Result;
            if (dataCollection.result.length > 0) {
                var VIsNENsReceive = true;
                var VAccessToALSAT = true;
                if (dataCollection.result[0].IsNENsReceive != VIsNENsReceive)
                    VIsNENsReceive = false;

                if (dataCollection.result[0].AccessToALSAT != VAccessToALSAT)
                    VAccessToALSAT = false;
                $("#hdnOrgName").val(dataCollection.result[0].OrgName);
                $("#hdnOrgType").val(dataCollection.result[0].OrgType)
                $("#hdnOrgCode").val(dataCollection.result[0].OrgCode)
                $("#hdnLICENSE_NR").val(dataCollection.result[0].LicenseNR)
                $("#hdnIsReceiveNEN").val(VIsNENsReceive)
                $("#hdnAccessToALSAT").val(VAccessToALSAT)
                $("#hdnAddressLine_1").val(dataCollection.result[0].AddressLine1)
                $("#hdnAddressLine_2").val(dataCollection.result[0].AddressLine2)
                $("#hdnAddressLine_3").val(dataCollection.result[0].AddressLine3)
                $("#hdnAddressLine_4").val(dataCollection.result[0].AddressLine4)
                $("#hdnAddressLine_5").val(dataCollection.result[0].AddressLine5)
                $("#hdnPostCode").val(dataCollection.result[0].PostCode)
                if ($('#hf_sortApp').val() != "SORTSO") {
                    $("#hdnCountryID").val(dataCollection.result[0].CountryId);
                } else {
                    var country = $("#CountryID option[value=" + dataCollection.result[0].CountryId + "]").text();
                    $("#hdnCountryID").val(dataCollection.result[0].CountryId);
                }
                $("#hdnPhone").val(dataCollection.result[0].Phone)
                $("#hdnWeb").val(dataCollection.result[0].Web)
                $("#hdnHaulierContactName").val(dataCollection.result[0].HA_Contact)
                $("#hdnEmailId").val(dataCollection.result[0].EmailId)
                $("#hdnFax").val(dataCollection.result[0].Fax)
                if ($('#hf_sortApp').val() != "SORTSO") {
                    $("#" + hf_orgID).find("#OrgName").val($("#hdnOrgName").val());
                    $("#" + hf_orgID).find("#OrgType").val($("#hdnOrgType").val());
                    $("#" + hf_orgID).find("#OrgCode").val($("#hdnOrgCode").val());
                    $("#" + hf_orgID).find("#LICENSE_NR").val($("#hdnLICENSE_NR").val());
                    $("#" + hf_orgID).find('#IsReceiveNEN').attr('checked', VIsNENsReceive);
                    $("#" + hf_orgID).find('#AccessToALSAT').attr('checked', VAccessToALSAT);
                    $("#" + hf_orgID).find("#AddressLine_1").val($("#hdnAddressLine_1").val());
                    $("#" + hf_orgID).find("#AddressLine_2").val($("#hdnAddressLine_2").val());
                    $("#" + hf_orgID).find("#AddressLine_3").val($("#hdnAddressLine_3").val());
                    $("#" + hf_orgID).find("#AddressLine_4").val($("#hdnAddressLine_4").val());
                    $("#" + hf_orgID).find("#AddressLine_5").val($("#hdnAddressLine_5").val());
                    $("#" + hf_orgID).find("#PostCode").val($("#hdnPostCode").val());
                    $("#" + hf_orgID).find("#CountryID").val($("#hdnCountryID").val());
                    $("#" + hf_orgID).find("#Phone").val($("#hdnPhone").val());
                    $("#" + hf_orgID).find("#Web").val($("#hdnWeb").val());
                    $("#" + hf_orgID).find("#AuthKeyEdit").val(dataCollection.result[0].AuthenticationKey);
                    $("#" + hf_orgID).find("#AuthKeyEdit").prop("readonly", true);
                    if ($('#hf_sortApp').val() == "SORTSO") {
                        $('#selectionOrg').hide();
                        $('#selectionOrgSORT').show();
                        $("#spnOrgName").html($("#hdnOrgName").val());
                        $("#spnOrgCode").html($("#hdnOrgCode").val());
                        $("#spnLICENSE_NR").html($("#hdnLICENSE_NR").val());
                        $("#spnIsReceiveNEN").html($("#hdnIsReceiveNEN").val());
                        $("#spnAccessToALSAT").html($("#hdnAccessToALSAT").val());
                        $("#spnAddressLine_1").html($("#hdnAddressLine_1").val());
                        $("#spnAddressLine_2").html($("#hdnAddressLine_2").val());
                        $("#spnAddressLine_3").html($("#hdnAddressLine_3").val());
                        $("#spnAddressLine_4").html($("#hdnAddressLine_4").val());
                        $("#spnAddressLine_5").html($("#hdnAddressLine_5").val());
                        $("#spnPostCode").html($("#hdnPostCode").val());
                        $("#spnCountryID").html(country);
                        $("#spnPhone").html($("#hdnPhone").val());
                        $("#leftpanel").hide();
                        $('#leftpanel_div').hide();
                        $('#leftpanel').hide();
                        if ($('#hf_showHaulCnt').val() == 'True') {
                            $('#btn_SelectHaul').hide();
                            $('#btn_SelectHaul').unbind('click').live("click", function () {
                                $('#ddlHaulContFlag').val(1);
                                var pageNum = 1;
                                var pageSize = 10;
                                var showflag
                                $("#dialogue").load('../SORTApplication/HaulOrgListPopup?pageNum=' + pageNum + '&pageSize=' + pageSize, {},
                                    function () {
                                        $('#pageSizeVal').val(pageSize);
                                        $('#Config-body #pageSizeSelect').val(pageSize);
                                        removescroll();
                                        $("#dialogue").show();
                                        $("#overlay").show();
                                    });
                            });
                            $("#ddlHaulierContactName Option").each(function () { $(this).remove(); });
                            $("#ddlHaulierContactName").append("<option value='" + $("#hdnHaulierContactName").val() + "'>" + $("#hdnHaulierContactName").val() + "</option>");
                            if ($('#RtnHdnOrgFax').val() == "")
                                $("#FaxSORT").val($("#hdnFax").val());
                            else
                                $("#FaxSORT").val($("#RtnHdnOrgFax").val());
                            if ($('#RtnHdnOrgEmailId').val() == "")
                                $("#EmailIdSORT").val($("#hdnEmailId").val());
                            else
                                $("#EmailIdSORT").val($("#RtnHdnOrgEmailId").val());
                            $("#ddlHaulierContactName").change(function () {
                                var selIndex = $("#ddlHaulierContactName option:selected").index();
                                if (selIndex != 0) {
                                    $("#err_ddl_ContactName").html("");
                                    $("#HaulierContactNameExist").val("");
                                    var arrFax = $("#hdnArrFax").val().split(",");
                                    var arrEmail = $("#hdnArrEmail").val().split(",");
                                    $("#EmailIdSORT").val(arrEmail[selIndex - 1]);
                                    $("#FaxSORT").val(arrFax[selIndex - 1]);
                                }
                                else {
                                    $("#EmailIdSORT").val('');
                                    $("#FaxSORT").val('');
                                }
                            })
                        } else {
                            $("#spnHaulierContactName").html($("#hdnHaulierContactName").val());
                            $("#spnEmail").html($("#hdnEmailId").val());
                            $("#spnFax").html($("#hdnFax").val());
                            $('#btn_SelectHaul').hide();
                        }
                    }

                    var selectedOrgType = $('#OrgType option:selected').val();
                    if (selectedOrgType == 237013 || selectedOrgType == 237014 || selectedOrgType == "") {
                        $('#spnSymbolMandetoryCode').show();
                        $('#err_OrgType_exists').text('');
                        $('#divReceiveNENs').hide();
                        $('#divAccessToALSAT').hide();
                    }
                    else if (selectedOrgType == 237007) {
                        $('#spnSymbolMandetoryCode').hide();
                        $('#divReceiveNENs').show();
                        $('#divAccessToALSAT').hide();
                    }
                    else {
                        $('#err_OrgType_exists').text('');
                        $('#divReceiveNENs').show();
                        $('#divAccessToALSAT').show();
                    }
                }
            }
        }).fail(function (error, a, b) {
        }).always(function (xhr) {
        });
    }

    StepFlag = 0;
    SubStepFlag = 0.1;
    CurrentStep = "Haulier Details";
    $('#plan_movement_hdng').text("PLAN MOVEMENT");
    $('#current_step').text(CurrentStep);
    //for cross dailog----
    $('#addrchk').text('');
    checkValidate();
    addresschk();
    OrgNameKeypress();
    OrgCodeKeypress();
    CountryKeypress();

    $('#save_btn').show();
    $('#confirm_btn').hide();
    stopAnimation();
}
$(document).ready(function () {
    $('body').on('click', '#authKey', function (e) {
        e.preventDefault();
        GenerateAuthKey(this);
    });

    $('body').on('change', '#OrgType', function (e) {
       
        var selectedOrg = $('#OrgType option:selected').text().toLowerCase();
        //if (selectedOrg != '' || hf_mode == "SORTSO") {
        //    $('#spnSymbolMandetoryCode').show();
        //}
        var selectedOrgType = $('#OrgType option:selected').val();
        if (selectedOrgType == 237013 || selectedOrgType == 237014 || selectedOrgType == "") {
            $('#spnSymbolMandetoryCode').show();
        }
        else if (hf_mode == "SORTSO") {
            $('#spnSymbolMandetoryCode').hide();
            $("#errmsg_OrgCode").text("");
        }
        var selectedOrgType = $('#OrgType option:selected').val();
        if (selectedOrgType == 237013 || selectedOrgType == 237014 ||  selectedOrgType == "") {
            $('#err_OrgType_exists').text('');
            $('#divReceiveNENs').hide();
            $('#divAccessToALSAT').hide();
        }
        else if(selectedOrgType == 237007)
        {
            $('#spnSymbolMandetoryCode').hide();
            $('#divReceiveNENs').show();
            $('#divAccessToALSAT').hide();

        }
        else {
            $('#err_OrgType_exists').text('');
            $('#divReceiveNENs').show();
            $('#divAccessToALSAT').show();
        }
    });

    $('body').on('change', '#EmailIdSORT', function (e) {
        $("#emailFaxValid").html("");
    });

    $('body').on('change', '#FaxSORT', function (e) {
        $("#emailFaxValid").html("");
    });

    $('body').on('keyup', '#OrgName', function (e) {
        $("#ownerValidationMsg").text("");
        $("#ownerValidationMsg").hide();
        if ($("#OrgName").val() == '') {
            $("#ownerValidationMsg").text("Organisation name is required");
            $("#ownerValidationMsg").show();

        } else if ($("#OrgName").val().trim().length < 3 && $("#OrgName").val().trim().length > 0) {
            $("#ownerValidationMsg").text("Please enter a valid name");
            $("#ownerValidationMsg").show();
            $("#err_OrgName_exists").hide();
        }
        else {
            $("#ownerValidationMsg").hide();
        }
    });
    $('body').on('click', '#OrgType', function (e) {

        if (hf_mode != "SORTSO" && hf_sortApp != "SORTSO") {

            if ($('#OrgType option:selected').val() == '') {
                $("#err_OrgType_exists").html("Organisation Type is required");
                $("#err_OrgType_exists").show();
            }
            else {
                $("#err_OrgType_exists").html("");
            }
        }
    });
    $('body').on('keyup', '#OrgName', function (e) {
        $("#ownerValidationMsg").text("");
        $("#ownerValidationMsg").hide();
        if ($("#OrgName").val() == '') {
            $("#ownerValidationMsg").text("Organisation name is required");
            $("#ownerValidationMsg").show();

        } else if ($("#OrgName").val().trim().length < 3 && $("#OrgName").val().trim().length > 0) {
            $("#ownerValidationMsg").text("Please enter a valid name");
            $("#ownerValidationMsg").show();
            $("#err_OrgName_exists").hide();

        }
        else {
            $("#ownerValidationMsg").hide();
        }
    });
    $('body').on('keyup', '#OrgCode', function (e) {
    
        var orgCodePattern = new RegExp(/^[A-Z0-9]+$/);

        $("#err_OrgCode_exists").text("");
        $("#err_OrgCode_exists").hide();
        $('#err_OrgName_exists').text("");
        $('#err_OrgName_exists').hide();
        $('#codeExistsValidation').text("");
        if ($("#OrgCode").val() != '') {

            if (!orgCodePattern.test($("#OrgCode").val())) {

                $("#err_OrgCode_exists").text("Only uppercase and numbers are allowed");
                $("#err_OrgCode_exists").show();
                $("#orgCodeCodeIsRequiredMsg").hide();
            }

            //else if ($("#OrgCode").val().trim().length >4 ) {
            //    $("#err_OrgCode_exists").text("Please enter a valid code");
            //    $("#err_OrgCode_exists").show();
            //    $("#orgCodeCodeIsRequiredMsg").hide();
            //}
        }
       
        //else {
        //    $("#orgCodeCodeIsRequiredMsg").text("Organisation code is required");
        //    $("#orgCodeCodeIsRequiredMsg").show();
        //    $("#err_OrgCode_exists").hide();
        //}
    });

    $('body').on('keyup', '#PostCode', function (e) {
        var postCodePattern = new RegExp(/^[a-zA-Z0-9\s]+$/);
        $("#postCodeIsRequiredMsg").hide();
        $("#errorPostcodevalidation").text("");
        $("#errorPostcodevalidation").hide();
        if ($("#PostCode").val() == '') {
            $("#postCodeIsRequiredMsg").show();
            $("#errorPostcodevalidation").hide();
            postCodeValid = false;
        }
        else {
            $("#postCodeIsRequiredMsg").hide();
        }
        if ($("#PostCode").val() != '') {
            if (!postCodePattern.test($("#PostCode").val())) {
                $("#errorPostcodevalidation").text("Special characters are not allowed");
                $("#errorPostcodevalidation").show();
            }
            else {
                $("#errorPostcodevalidation").text("");
            }
        }
        if ($("#PostCode").val() != '') {
            if ($("#PostCode").val().length < 4) {
                $("#errorPostcodevalidation").text("Please enter a valid postcode");
                $("#errorPostcodevalidation").show();
            }
        }

    }
    );

    $('body').on('click', '#CountryID', function (e) {
        if ($('#CountryID option:selected').val() == '') {
            $("#countryValidationMsg").show();
        }
        else {
            $("#countryValidationMsg").hide();
        }

    });
    $('body').on('keyup', '#Phone', function (e) {
        var phonePattern = new RegExp(/^(\+)?[0-9\ -]+$/);
        $("#phoneIsRequiredMsg").hide();
        $("#phonevalidationMsg").text("");
        if ($("#Phone").val() == '') {
            $("#phoneIsRequiredMsg").show();
            $("#phonevalidationMsg").text("");
        }
        else {
            $("#phoneIsRequiredMsg").hide();
        }
        if (hf_mode != "SORTSO") {
            if ($("#Phone").val() != '') {
                if (!phonePattern.test($("#Phone").val())) {
                    $("#phonevalidationMsg").text("Please enter a valid phone number");
                }
                else {
                    $("#phonevalidationMsg").text("");
                }
            }
        }
    });

    $('body').on('keyup', '#EmailId', function (e) {
        var emailPattern = new RegExp(/^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/);
        var faxPattern = new RegExp(/^\+?[0-9]{6,}$/);
        $("#emailFaxValid1").html("");
        if ($("#EmailId").val() == '' && $("#Fax").val() == '') {
            $("#emailFaxValid1").html("Email/Fax is required");
            flag = false;
        }

        if ($("#EmailId").val() != '') {
            if (!emailPattern.test($("#EmailId").val())) {
                $("#emailFaxValid1").html("Please enter a valid email id");
            }
        }

        if ($("#Fax").val() != '') {
            if (!faxPattern.test($("#Fax").val())) {
                $("#emailFaxValid1").html("Please enter a valid fax");
            }
        }
    });
    $('body').on('keyup', '#Fax', function (e) {
        var emailPattern = new RegExp(/^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/);
        var faxPattern = new RegExp(/^\+?[0-9]{6,}$/);
        $("#emailFaxValid1").html('');
        if ($("#EmailId").val() == '' && $("#Fax").val() == '') {
            $("#emailFaxValid1").html("Email/Fax is required");
        }

        if ($("#EmailId").val() != '') {
            if (!emailPattern.test($("#EmailId").val())) {
                $("#emailFaxValid1").html("Please enter a valid email id");
            }
        }

        if ($("#Fax").val() != '') {
            if (!faxPattern.test($("#Fax").val())) {
                $("#emailFaxValid1").html("Please enter a valid fax");
            }
        }
    });
    $('body').on('change', '#HaulierContactName', function (e) {
        $('#err_Haulier_contact_name').html('');
    });

    $('body').on('change', '#EmailId', function (e) {
        $('#emailFaxValid1').html('');
    });
    $('body').on('change', '#Fax', function (e) {
        $('#emailFaxValid1').text('');
    });
    $('body').on('change', '#OrgName', function (e) {
        $('#ownerValidationMsg').hide();
    });
    $('body').on('change', '#OrgCode', function (e) {
        $('#codeIsRequiredMsg').hide();
        $('#orgCodeCodeIsRequiredMsg').html('');
    });
    $('body').on('change', '#PostCode', function (e) {
        $('#postCodeIsRequiredMsg').hide();
    });
    $('body').on('change', '#Phone', function (e) {
        $('#phoneIsRequiredMsg').hide();
    });
    $('body').on('change', '#AddressLine_1, #AddressLine_2, #AddressLine_3, #AddressLine_4, #AddressLine_5', function (e) {
        $('#addrchk').text('');
    });

    $('body').on('click', '#btn_reset', function (e) {
        if (hf_mode == 'Edit') {
            Edit(hf_orgID);
        } else {
            $("#OrgName").val('');
            $('select#OrgType').prop('selectedIndex', 0);
            $("#OrgCode").val('');
            $("#LICENSE_NR").val('');
            $('#IsReceiveNEN').prop('checked', false);
            $('#AccessToALSAT').prop('checked', false);
            $("#AddressLine_1").val('');
            $("#AddressLine_2").val('');
            $("#AddressLine_3").val('');
            $("#AddressLine_4").val('');
            $("#AddressLine_5").val('');
            $("#PostCode").val('');
            $('select#CountryID').prop('selectedIndex', 0);
            $("#Phone").val('');
            $("#Web").val('');
            $('#addrchk').text('');
            $(".error").hide();
            $("#AuthKeySave").val('');
            $('#spnSymbolMandetoryCode').hide();
        }
        
    });
    if ($('#hf_IsPlanMovmentGlobal').length == 0) {
        $('body').on('click', '#btn_save', function (e) {
           
            if (hf_mode == 'Edit') {
                SaveOrganisationData(hf_orgID);
            } else {
                SaveOrganisationData();
            }
        });
    }
});
function GenerateAuthKey() {

    $.ajax({
        url: '../Organisation/GenerateAuthenticationKey',
        type: 'POST',
        dataType: 'json',
        beforeSend: function () {
            startAnimation();
        },
        success: function (data) {
            if (hf_mode == "Edit") {
                $("#" + hf_orgID).find("#authKeyIsRequiredMsg").hide();
                $("#" + hf_orgID).find("#AuthKeyEdit").val(data.result);
                $("#KeyValidator").val(data.validator);
            }
            else {
                $("#authKeyIsRequiredMsg").hide();
                $("#AuthKeySave").val(data.result);
                $("#KeyValidator").val(data.validator);
            }
            stopAnimation();
        },
        error: function (xhr, textStatus, errorThrown) {
            location.reload();
        },
        complete: function () {
        }
    });

}
function checkValidate() {

    $("form[id='form_Organisation']").submit(function (e) {
        flag = true;
        var orgName = $('#OrgName').val();
        var typeId = 1;
        var ErrMsg = "Organisation Name already exists";
        GetOrganisationExists(orgName, typeId, ErrMsg)

        if (!isvalid) {
            flag = false;
        }
        var orgCode = $('#OrgCode').val();
        var typeId = 2;
        var ErrMsg = "Organisation Code already exists";
        GetOrganisationExists(orgCode, typeId, ErrMsg);

        if (!isvalid) {
            flag = false;
        }

        if ($('#AddressLine_1').val() == "" && $('#AddressLine_2').val() == "" && $('#AddressLine_3').val() == "" && $('#AddressLine_4').val() == "" && $('#AddressLine_5').val() == "") {
            $('#addrchk').text("Atleast one address line is required");
            flag = false;
        }


        var selectedOrg = $('#OrgType option:selected').text().toLowerCase();
        if (selectedOrg == 'haulier' || hf_mode == "SORTSO") {
            if ($("#OrgCode").val() == "" || $("#OrgCode").val() == null) {
                $("#orgCodeCodeIsRequiredMsg").text("Organisation code is required");
                $("#errmsg_OrgCode").show();
                flag = false;
            }
        }
        if (hf_mode != "SORTSO" && hf_sortApp != "SORTSO") {
            if ($('select#OrgType option:selected').val() == '')
                $("#err_OrgType_exists").text("Organisation Type is required");
            $("#err_OrgType_exists").show();
            flag = false;
        }
        if (hf_mode == "SORTSO" || hf_sortApp == "SORTSO") {
            if ($("#HaulierContactName").val() == "" || $("#HaulierContactName").val() == null) {
                $("#err_Haulier_contact_name").text("Haulier contact name is required");
                flag = false;
            }
            if ($("#EmailId").val() == "" && $("#Fax").val() == "") {
                $("#emailFaxValid1").html("Email/Fax is required");
                flag = false;
            }
        }
        if (flag) {
            $('#addrchk').text('');
        }
        else {
            return false;
        }

        //For checking NEN orgcode is not used while creating organisation
        var ErrNENMsg = "";
        var OrgCodeNEN = $("#OrgCode").val();
        if (OrgCodeNEN == "NEN") {
            ErrNENMsg = "NEN keyword is reserved for NE notifications";
            $('#err_OrgCode_exists').text(ErrNENMsg);
            return false;
        }
    })
}
function SaveOrganisationData(orgId) {
    startAnimation();
    var emailPattern = new RegExp(/^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/);
    var phonePattern = new RegExp(/^(\+)?[0-9\ -]+$/);
    var orgCodePattern = new RegExp(/^[A-Z0-9]+$/);
    var postCodePattern = new RegExp(/^[a-zA-Z0-9\s]+$/);
    var faxPattern = new RegExp(/^\+?[0-9]{6,}$/);
    $("#dialogue").show();
    $("#overlay").show();
    var orgNameValid = true;
    var orgCodeValid = true;
    var emailValid = true;
    var faxValid = true;
    var postCodeValid = true;
    var telephoneValid = true;
    var countryValid = true;
    var orgTypeValid = true;
    var licenseValid = true;
    var keyCodeValid = true;
    var flag = true;
    if (hf_mode == 'Edit') {
        startAnimation();
        if ($("#" + orgId).find("#OrgName").val() == '') {
            $("#" + orgId).find("#ownerValidationMsg").show();
            $("#" + orgId).find("#err_OrgName_exists").hide();
            orgNameValid = false;
            flag = false;
        }
        else if ($("#" + orgId).find("#OrgName").val().trim().length < 3 && $("#" + orgId).find("#OrgName").val().trim().length > 0) {
            $("#" + orgId).find("#ownerValidationMsg").text("Please enter a valid name");
            $("#" + orgId).find("#ownerValidationMsg").show();
            $("#" + orgId).find("#err_OrgName_exists").hide();
            orgNameValid = false;
            flag = false;
        }
        else {
            $("#" + orgId).find("#ownerValidationMsg").hide();
            var orgName = $("#" + orgId).find('#OrgName').val();
            var typeId = 1;
            var ErrMsg = "Organisation Name already exists";
            GetOrganisationExists(orgName, typeId, ErrMsg, orgId);
        }
        if (!isvalidOrg)
            flag = false;

        if ($("#" + orgId).find("#OrgCode").val() == '') {

            $("#" + orgId).find("#err_OrgCode_exists").text("");
            $("#" + orgId).find("#codeExistsValidation").text("");
            $("#" + orgId).find("#orgCodeCodeIsRequiredMsg").text("Organisation code is required").show();
            orgCodeValid = true;
            flag = true;
        }
        else {
            $("#" + orgId).find("#codeExistsValidation").text("");
            $("#" + orgId).find("#orgCodeCodeIsRequiredMsg").text("");
            var orgCode = $("#" + orgId).find('#OrgCode').val();
            var typeId = 2;
            var ErrMsg = "Organisation Code already exists";
            GetOrganisationExists(orgCode, typeId, ErrMsg, orgId);
        }

        if (!isvalidOrg)
            flag = false;
        if ($("#" + orgId).find("#OrgCode").val() != '') {
            if (!orgCodePattern.test($("#" + orgId).find("#OrgCode").val())) {
                $("#" + orgId).find("#err_OrgCode_exists").text("Please enter a valid code");
                orgCodeValid = false;
                flag = false;
            }
            else {
                $("#" + orgId).find("#err_OrgCode_exists").text("");
            }
        }

        if ($("#" + orgId).find('select#CountryID option:selected').val() == "") {
            $("#" + orgId).find("#countryValidationMsg").show();
            countryValid = false;
        }
        else {
            $("#" + orgId).find("#countryValidationMsg").hide();
        }
        if ($("#" + orgId).find("#PostCode").val() == '') {
            $("#" + orgId).find("#postCodeIsRequiredMsg").show();
            $("#" + orgId).find("#errorPostcodevalidation").hide();
            postCodeValid = false;
            flag = false;
        }
        else {
            $("#" + orgId).find("#postCodeIsRequiredMsg").hide();
        }
        if ($("#" + orgId).find("#AuthKeyEdit").val() == '') {
            $("#" + orgId).find("#authKeyIsRequiredMsg").show();
            keyCodeValid = false;
            flag = false;
        }
        else {
            $("#" + orgId).find("#authKeyIsRequiredMsg").hide();
        }

        if ($("#" + orgId).find("#PostCode").val() != '') {
            if (!postCodePattern.test($("#" + orgId).find("#PostCode").val())) {
                $("#" + orgId).find("#errorPostcodevalidation").text("Special characters are not allowed");
                $("#" + orgId).find("#errorPostcodevalidation").show();
                flag = false;
                postCodeValid = false;
            }
            else {
                $("#errorPostcodevalidation").text("");

            }

        }
        if ($("#" + orgId).find("#PostCode").val() != '') {
            if ($("#" + orgId).find("#PostCode").val().length < 4) {
                $("#" + orgId).find("#errorPostcodevalidation").text("Please enter a valid postcode");
                $("#" + orgId).find("#errorPostcodevalidation").show();
                postCodeValid = false;
                flag = false;
            }
            else {
                $("#" + orgId).find("#errorPostcodevalidation").text("");
            }
        }
        if ($("#" + orgId).find("#OrgCode").val() != '') {
            if ($("#" + orgId).find("#OrgCode").val().length < 3) {
                $("#" + orgId).find("#err_OrgCode_exists").text("Please enter a valid code");
                orgCodeValid = false;
                flag = false;
            }
            else {
                $("#" + orgId).find("#err_OrgCode_exists").text("");
            }
        }
        if ($("#" + orgId).find("#Phone").val() == '') {

            $("#" + orgId).find("#phoneIsRequiredMsg").show();
            $("#" + orgId).find("#phonevalidationMsg").text("");
            telephoneValid = false;
            flag = false;
        }
        else {
            $("#" + orgId).find("#phoneIsRequiredMsg").hide();
        }

        if ($("#" + orgId).find("#Phone").val() != '') {
            if (!phonePattern.test($("#" + orgId).find("#Phone").val())) {
                $("#" + orgId).find("#phonevalidationMsg").text("Please enter a valid phone number");
                telephoneValid = false;
            }
            else {
                $("#" + orgId).find("#phonevalidationMsg").text("");
                telephoneValid = true;
            }
        }

        if ($("#" + orgId).find('#AddressLine_1').val() == "" && $("#" + orgId).find('#AddressLine_2').val() == "" && $("#" + orgId).find('#AddressLine_3').val() == "" && $("#" + orgId).find('#AddressLine_4').val() == "" && $("#" + orgId).find('#AddressLine_5').val() == "") {
            $("#" + orgId).find('#addrchk').text("Atleast one address line is required");
            $("#" + orgId).find('#addrchk').show();
            flag = false;
        }
        else {
            $("#" + orgId).find('#addrchk').text("");
            $("#" + orgId).find('#addrchk').hide();
        }
        if (hf_mode == "SORTSO" && hf_sortApp != "SORTSO") {
            if ($("#" + orgId).find('select#OrgType option:selected').val() == '') {
                $("#" + orgId).find("#err_OrgType_exists").text("Organisation Type is required");
                $("#" + orgId).find("#err_OrgType_exists").show();
                orgTypeValid = false;
            }
            else {
                $("#" + orgId).find("#err_OrgType_exists").text("");
                $("#" + orgId).find("#err_OrgType_exists").hide();
            }
        }
        //For checking NEN orgcode is not used while creating organisation
        var ErrNENMsg = "";
        var OrgCodeNEN = $("#" + orgId).find("#OrgCode").val();
        if (OrgCodeNEN == "NEN") {
            ErrNENMsg = "NEN keyword is reserved for NE notifications";
            $('#err_OrgCode_exists').text(ErrNENMsg);
            flag = false;
        }

        if (flag) {
            $("#" + orgId).find('#addrchk').text('');
            $("#" + orgId).find('#emailFaxValid1').html('');
            $("#" + orgId).find('#err_Haulier_contact_name').text('');
            $("#" + orgId).find('#err_OrgName_exists').text('');
            $("#" + orgId).find('#err_OrgCode_exists').text('');
            $("#" + orgId).find('#ownerValidationMsg').hide();
            $("#" + orgId).find("#codeIsRequiredMsg").hide();
            $("#" + orgId).find("#countryValidationMsg").hide();
            $("#" + orgId).find("#postCodeIsRequiredMsg").hide();
            $("#" + orgId).find("#phoneIsRequiredMsg").hide();
            $("#" + orgId).find('#orgCodeCodeIsRequiredMsg').text('');
        }
    }
    else {
        startAnimation();
        if ($("#OrgName").val() == '') {
            $("#ownerValidationMsg").show();
            orgNameValid = false;
        } else if ($("#OrgName").val().trim().length < 3 && $("#OrgName").val().trim().length > 0) {
            $("#ownerValidationMsg").text("Please enter a valid name");
            $("#ownerValidationMsg").show();
            $("#err_OrgName_exists").hide();
            orgNameValid = false;
            flag = false;
        }
        else {
            $("#ownerValidationMsg").hide();
            var orgName = $('#OrgName').val();
            var typeId = 1;
            var ErrMsg = "Organisation Name already exists";
            GetOrganisationExists(orgName, typeId, ErrMsg);
        }
        if (!isvalidOrg) {
            flag = false;
        }
        var x = $('#spnSymbolMandetoryCode').is(':visible')
        var orgCode = $('#OrgCode').val();
        if (($("#OrgCode").val() == "") && (x == true)) {
            $("#orgCodeCodeIsRequiredMsg").text("Organisation code is required").show();
        }
        if ($("#OrgCode").val() != '') {
            $("#codeExistsValidation").text("");
            var orgCode = $('#OrgCode').val();
            var typeId = 2;
            var ErrMsg = "Organisation Code already exists";
            GetOrganisationExists(orgCode, typeId, ErrMsg);
        }


        if (!isvalidOrg) {
            flag = false;
        }
        if ($("#OrgCode").val() != '') {
            if (!orgCodePattern.test($("#OrgCode").val())) {

                $("#err_OrgCode_exists").text("Only uppercase and numbers are allowed");
                orgCodeValid = false;
                flag = false;
            }
        }
        //if ($("#OrgCode").val() == '') {

        //    $("#orgCodeCodeIsRequiredMsg").text("Organisation code is required");
        //    flag = false;

        //}
        if ($("#OrgCode").val() != '') {
            if ($("#OrgCode").val().length < 3) {
                $("#err_OrgCode_exists").text("Please enter a valid code");
                $("#orgCodeCodeIsRequiredMsg").text("");
                orgCodeValid = false;
                flag = false;
            }
        }
        //else {
        //    $("#orgCodeCodeIsRequiredMsg").text("Organisation code is required");
        //    flag = false;

        //}
        if (!isvalidOrg) {
            flag = false;
        }
        if ($('select#CountryID option:selected').val() == "") {
            $("#countryValidationMsg").show();
            countryValid = false;
        }
        else {
            $("#countryValidationMsg").hide();
        }
        if ($("#PostCode").val() == '') {
            $("#postCodeIsRequiredMsg").show();
            $("#errorPostcodevalidation").hide();
            postCodeValid = false;
        }
        else {
            $("#postCodeIsRequiredMsg").hide();
        }
        if ($("#AuthKeySave").val() == '') {
            $("#authKeyIsRequiredMsg").show();
            keyCodeValid = false;
            flag = false;
        }
        else {
            $("#authKeyIsRequiredMsg").hide();
        }
        if ($("#PostCode").val() != '') {
            if (!postCodePattern.test($("#PostCode").val())) {
                $("#errorPostcodevalidation").text("Please enter a valid postcode");
                $("#errorPostcodevalidation").show();
                flag = false;
                postCodeValid = false;
            }
            else {
                $("#errorPostcodevalidation").text("");
            }
        }
        if ($("#PostCode").val() != '') {
            if ($("#PostCode").val().length < 4) {
                $("#errorPostcodevalidation").text("Please enter a valid postcode");
                $("#errorPostcodevalidation").show();
                postCodeValid = false;
                flag = false;
            }
        }

        if ($("#Phone").val() == '') {

            $("#phoneIsRequiredMsg").show();
            $("#phonevalidationMsg").text("");
            telephoneValid = false;
            flag = false;
        }
        else {
            $("#phoneIsRequiredMsg").hide();
        }
        if (hf_mode != "SORTSO") {
            if ($("#Phone").val() != '') {
                if (!phonePattern.test($("#Phone").val())) {

                    $("#phonevalidationMsg").text("Please enter a valid phone number");
                    telephoneValid = false;
                }
                else {
                    $("#phonevalidationMsg").text("");
                    telephoneValid = true;
                }
            }
        }

        if (hf_mode == "SORTSO" || hf_sortApp == "SORTSO") {
            if ($("#EmailId").val() == '' && $("#Fax").val() == '') {
                $("#emailFaxValid1").html("Email/Fax is required");
                flag = false;
            }
            else {
                $("#emailFaxValid1").html('');
            }

            if ($("#EmailId").val() != '') {
                if (!emailPattern.test($("#EmailId").val())) {
                    emailValid = false;
                }
            }

            if ($("#Fax").val() != '') {
                if (!faxPattern.test($("#Fax").val())) {
                    faxValid = false;
                }
            }
        }

        if ($('#AddressLine_1').val() == "" && $('#AddressLine_2').val() == "" && $('#AddressLine_3').val() == "" && $('#AddressLine_4').val() == "" && $('#AddressLine_5').val() == "") {
            $('#addrchk').text("Atleast one address line is required");
            $('#addrchk').show();
            flag = false;
        }
        else {
            $('#addrchk').text("");
            $('#addrchk').hide();
        }
        if (hf_mode != "SORTSO" && hf_sortApp != "SORTSO") {
            if ($('select#OrgType option:selected').val() == '') {
                $("#err_OrgType_exists").text("Organisation Type is required");
                $("#err_OrgType_exists").show();
                orgTypeValid = false;
            }
            else {
                $("#err_OrgType_exists").text("");
                $("#err_OrgType_exists").hide();
            }
        }

        if (hf_mode == "SORTSO" || hf_sortApp == "SORTSO") {
            if ($("#HaulierContactName").val() == "" || $("#HaulierContactName").val() == null) {
                $("#err_Haulier_contact_name").text("Haulier contact name is required");
                flag = false;
            }
            else {
                $("#err_Haulier_contact_name").text("");
            }
        }

        //For checking NEN orgcode is not used while creating organisation
        var ErrNENMsg = "";
        var OrgCodeNEN = $("#OrgCode").val();
        if (OrgCodeNEN == "NEN") {
            ErrNENMsg = "NEN keyword is reserved for NE notifications";
            $('#err_OrgCode_exists').text(ErrNENMsg);
            flag = false;
        }

        if (flag) {
            $('#addrchk').text('');
            $('#emailFaxValid1').html('');
            $('#err_Haulier_contact_name').text('');
            $('#orgCodeCodeIsRequiredMsg').text('');
            $('#err_OrgName_exists').text('');
            $('#err_OrgCode_exists').text('');
            $('#ownerValidationMsg').hide();
            $("#codeIsRequiredMsg").hide();
            $("#countryValidationMsg").hide();
            $("#postCodeIsRequiredMsg").hide();
            $("#phoneIsRequiredMsg").hide();
        }
        if (hf_mode == 'Edit') {
            $("#" + orgId).find('#err_Haulier_contact_name').text('');
            $("#" + orgId).find('#err_OrgName_exists').text('');
            $("#" + orgId).find('#err_OrgCode_exists').text('');
            $("#" + orgId).find('#orgCodeCodeIsRequiredMsg').text('');
            $("#" + orgId).find('#ownerValidationMsg').hide();
            $("#" + orgId).find("#codeIsRequiredMsg").hide();
            $("#" + orgId).find("#countryValidationMsg").hide();
            $("#" + orgId).find("#postCodeIsRequiredMsg").hide();
            $("#" + orgId).find("#phoneIsRequiredMsg").hide();
        }
    }
    if (orgNameValid && orgTypeValid && emailValid && faxValid && postCodeValid && telephoneValid && orgCodeValid && countryValid && flag && isvalidOrg) {
        var organisationData = {};
        startAnimation();

        if (hf_mode == 'Edit') {
            organisationData.OrgName = $("#" + orgId).find("#OrgName").val();
            organisationData.HaulierContactName = $("#" + orgId).find("#HaulierContactName").val();
            organisationData.OrgCode = $("#" + orgId).find("#OrgCode").val();
            organisationData.orgType = $("#" + orgId).find("#OrgType").val();
            organisationData.Licence_NR = $("#" + orgId).find("#LICENSE_NR").val();
            organisationData.AddressLine1 = $("#" + orgId).find("#AddressLine_1").val();
            organisationData.AddressLine2 = $("#" + orgId).find("#AddressLine_2").val();
            organisationData.AddressLine3 = $("#" + orgId).find("#AddressLine_3").val();
            organisationData.AddressLine4 = $("#" + orgId).find("#AddressLine_4").val();
            organisationData.AddressLine5 = $("#" + orgId).find("#AddressLine_5").val();
            organisationData.CountryID = $("#" + orgId).find("#CountryID").val();
            organisationData.PostCode = $("#" + orgId).find("#PostCode").val();
            organisationData.Phone = $("#" + orgId).find("#Phone").val();
            organisationData.IsNENsReceive = $("#" + orgId).find('#IsReceiveNEN').is(':checked');
            organisationData.AccessToALSAT = $("#" + orgId).find('#AccessToALSAT').is(':checked');
            organisationData.Web = $("#" + orgId).find("#Web").val();
            organisationData.AuthenticationKey = $("#" + orgId).find("#AuthKeyEdit").val();
            organisationData.KeyValidator = $("#KeyValidator").val();

        }
        else {
            organisationData.OrgName = $("#OrgName").val();
            organisationData.HaulierContactName = $("#HaulierContactName").val();
            organisationData.OrgCode = $("#OrgCode").val();
            organisationData.orgType = $("#OrgType").val();
            organisationData.Licence_NR = $("#LICENSE_NR").val();
            organisationData.AddressLine1 = $("#AddressLine_1").val();
            organisationData.AddressLine2 = $("#AddressLine_2").val();
            organisationData.AddressLine3 = $("#AddressLine_3").val();
            organisationData.AddressLine4 = $("#AddressLine_4").val();
            organisationData.AddressLine5 = $("#AddressLine_5").val();
            organisationData.CountryID = $("#CountryID").val();
            organisationData.PostCode = $("#PostCode").val();
            organisationData.Phone = $("#Phone").val();
            organisationData.IsNENsReceive = $('#IsReceiveNEN').is(':checked');
            organisationData.AccessToALSAT = $('#AccessToALSAT').is(':checked');

            organisationData.EmailId = $("#EmailId").val();
            organisationData.Fax = $("#Fax").val();
            organisationData.Web = $("#Web").val();
            organisationData.AuthenticationKey = $("#AuthKeySave").val();
            organisationData.KeyValidator = $("#KeyValidator").val();
        }

        var authKey = organisationData.AuthenticationKey;
        var keyValidator = organisationData.KeyValidator;
        if (authKey != null && (keyValidator == null || keyValidator == "")) {
            $.ajax({
                url: '../Organisation/GenerateAuthenticationKeyChecksum',
                dataType: 'json',
                type: 'POST',
                data: { authenticationKey: authKey },
                beforeSend: function () {
                },
                success: function (data) {
                    organisationData.KeyValidator = data.validator;
                    if ($('#spnSymbolMandetoryCode').is(':visible')) {
                        var orgCode = $('#OrgCode').val();
                        if ($("#OrgCode").val() == "") {
                            $("#orgCodeCodeIsRequiredMsg").text("Organisation code is required").show();
                        }
                        else {
                            SaveOrganisation(orgId, organisationData);
                        }
                        stopAnimation();
                    }
                    else {
                        SaveOrganisation(orgId, organisationData);
                    stopAnimation();
                }
                },
                error: function (xhr, status) {
                    location.reload();
                },
                complete: function () {
                }
            });
        }
        else {
            if ($('#spnSymbolMandetoryCode').is(':visible')) {
                var orgCode = $('#OrgCode').val();
                if (($("#OrgCode").val() == "") && (x == true)) {
                    $("#orgCodeCodeIsRequiredMsg").text("Organisation code is required").show();
                }
                else {
                    SaveOrganisation(orgId, organisationData);
                }
                stopAnimation();
            }
            else
                SaveOrganisation(orgId, organisationData);
            stopAnimation();
        }
    }
    else {
        stopAnimation();
    }
}
function SaveOrganisation(orgId, organisationData) {
   
    $.ajax({
        url: '../Organisation/SaveOrganisation',
        dataType: 'json',
        type: 'POST',
        data: { orgDet: organisationData, mode: hf_mode, orgID: hf_orgID, sortApp: hf_sortApp, RevisionId: $("#hf_RevisionId").val(), SORTStatus: $("#hf_SortStatus").val() },
        beforeSend: function () {

        },
        success: function (data) {
            if (data.result == true) {
                if ($('#hf_IsPlanMovmentGlobal').length > 0)
                    CreateLocalObject(NavigationEnum.HAULIER_DETAILS);//For local storage 
                $("#HdnHaulierContactName").val($("#HaulierContactName").val());
                $("#HdnOrgEmailId").val($("#EmailId").val());
                $("#HdnOrgFax").val($("#Fax").val());
                $("#HdnOrgIDSORT").val(data.OrgId);
                $("#HdnOrgNameSORT").val($("#OrgName").val());
                $("#SOHndContactID").val(data.OrgId);
                var msg = "";
                var loadFunction = "";
                if (hf_mode == 'Edit') {
                    msg = 'Organisation ' + "\"" + $("#OrgName").val() + '\" edited successfully';
                    if ($('#UserType').val() == 696008) {
                        $("#SOHndContactID").val(0);
                        loadFunction = "NavigateToSelectVehicle";
                    }
                    else
                        loadFunction = "load";
                }
                else {
                    msg = 'Organisation ' + "\"" + $("#OrgName").val() + '\" created successfully';
                    loadFunction = "load";
                }
                ShowSuccessModalPopup(msg, loadFunction);
            }
            else {
                var msg = "Saving failed";
                var loadFunction = "load";
                ShowErrorPopup(msg, loadFunction);
            }
        },
        error: function (xhr, status,error) {
            console.error(error)
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function addresschk() {
    $('body').on('keypress', '#AddressLine_1, #AddressLine_2, #AddressLine_3, #AddressLine_4, #AddressLine_5', function (e) {
        $('#addrchk').text('');
    });
}
function OrgNameKeypress() {
    $('body').on('keypress', '#OrgName', function (e) {
        $('#err_OrgName_exists').text('');
        $('#ownerValidationMsg').hide();
    });
}
function CountryKeypress() {
    $('body').on('change', '#CountryID', function (e) {
        $('#countryValidationMsg').hide();
    });
}
function OrgCodeKeypress() {
    $('body').on('keypress', '#OrgCode', function (e) {
        $('#err_OrgCode_exists').text('');
        $('#codeIsRequiredMsg').text('');
    });
}
function GetOrganisationExists(orgName, typeId, ErrMsg, orgId) {
    var orgUrl = "";
    var data = "";
    if (orgId == undefined) {
        orgId = $('#hf_orgID').val();
    }
    var organisationID = orgId;

    if (hf_mode == 'Edit') {
        data = { OrganisationName: orgName, type: typeId, mode: 'Edit', orgID: organisationID };
    }
    else {
        data = { OrganisationName: orgName, type: typeId };
    }

    $.ajax({
        url: '../Organisation/CheckOrganisationExists',
        type: 'POST',
        dataType: 'json',
        cache: false,
        async: false,

        data: JSON.stringify(data),
        beforeSend: function () {
            startAnimation();
        },
        success: function (data) {
            if (typeId == 1) {
                if (data.result > 0) {
                    if (hf_mode == 'Edit') {
                        $("#" + organisationID).find('#err_OrgName_exists').text(ErrMsg);
                        $("#" + organisationID).find('#err_OrgName_exists').show();
                    }
                    else {
                        $('#err_OrgName_exists').text(ErrMsg);
                        $('#err_OrgName_exists').show();
                    }
                    isvalidOrg = false;
                }
                else {
                    if (hf_mode == 'Edit') {
                        $("#" + organisationID).find('#err_OrgName_exists').hide();
                    }
                    else {
                        $('#err_OrgName_exists').hide();
                    }
                    isvalidOrg = true;
                }
            }
            else if (typeId == 2) {
                if (data.result > 0) {
                    if (hf_mode == 'Edit') {
                        $("#" + organisationID).find('#codeExistsValidation').text(ErrMsg);
                        $("#" + organisationID).find('#err_OrgName_exists').hide();
                    }
                    else {
                        $('#codeExistsValidation').text(ErrMsg);
                        $('#err_OrgName_exists').hide();
                    }
                    isvalidOrg = false;
                }
                else {
                    if (hf_mode == 'Edit') {
                        $("#" + organisationID).find('#codeExistsValidation').text('');
                    }
                    else {
                        $('#codeExistsValidation').text('');
                    }
                    isvalidOrg = true;
                    isvalid = true;
                }
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            location.reload();
        },
        complete: function () {
        }
    });
}
function NavigateToSelectVehicle() {
    CloseSuccessModalPopup();
    LoadContentForAjaxCalls("POST", '../Movements/SelectVehicle', { contactId: $("#SOHndContactID").val() }, '#select_vehicle_section', '', function () {
        MovementSelectVehicleInit();
    });
}
function SuccessSave() {
    if (hf_mode != "Edit") {
        $("#HdnHaulierContactName").val($("#HaulierContactName").val());
        $("#HdnOrgEmailId").val($("#EmailId").val());
        $("#HdnOrgFax").val($("#Fax").val());
        $("#HdnOrgIDSORT").val(data.OrgId);
        $("#HdnOrgNameSORT").val($("#OrgName").val());
        $("#SOHndContactID").val(data.OrgId);
        var Msg = 'Organisation ' + "\"" + $("#OrgName").val() + '\" created successfully';
        ShowSuccessModalPopup(Msg, "NavigateToSelectVehicle");
    }
    stopAnimation();

}
function load() {
    location.reload();
}
