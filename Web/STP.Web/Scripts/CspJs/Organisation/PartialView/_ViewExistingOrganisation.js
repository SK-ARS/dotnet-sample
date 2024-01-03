    $(document).ready(function () {
        StepFlag = 0;
        SubStepFlag = 0.2;
        CurrentStep = "Haulier Details";
        $('#plan_movement_hdng').text("PLAN MOVEMENT");
        $('#current_step').text(CurrentStep);
        $('#save_btn').hide();
        $('#confirm_btn').show();
    });
    function retFaxSort(event) {
        $('#RtnHdnOrgFax').val($('#FaxSORT').val());
    }
    function retEmailIdSort(event) {
        $('#RtnHdnOrgEmailId').val($('#EmailIdSORT').val());
    }

    function ValidateHaulierDetails() {
        if ($("#spnOrgName").text() != "") {
            if (($("#ddlHaulierContactName option:selected").index() != 0 && $("#ddlHaulierContactName option:selected").index() != -1) || $("#HaulierContactNameExist").val().length > 0) {

                $("#err_ddl_ContactName").html("");
                $("#err_select_existing_haulier").html("");

                if ($("#EmailIdSORT").val().length > 0 || $("#FaxSORT").val().length > 0) {
                    $("#emailFaxValid").html("");

                    if ($("#ddlHaulierContactName option:selected").index() > 0) {
                        $("#HdnHaulierContactName").val($("#ddlHaulierContactName").val());
                    }
                    else {
                        $("#HdnHaulierContactName").val($("#HaulierContactNameExist").val());
                    }

                    $("#HdnOrgFax").val($("#FaxSORT").val());
                    $("#HdnOrgEmailId").val($("#EmailIdSORT").val());
                    $("#HdnOrgIDSORT").val($("#Organisation_ID").val());
                    $("#HdnOrgNameSORT").val($("#Organisation_Name").val());
                    $("#HdnHaulierContactName").val($("#ddlHaulierContactName").val());
                    $("#hdnCONTACTID").val($("#SOHndContactID").val());

                    LoadContentForAjaxCalls("POST", '../Movements/SelectVehicle', { contactId: $("#SOHndContactID").val() }, '#select_vehicle_section');
                }
                else {
                    $("#emailFaxValid").html("Email/Fax is required");
                }
            }
            else {
                $("#err_ddl_ContactName").html("Haulier contact name is required");
            }
        }
        else {
            $("#err_select_existing_haulier").html("Please select the organisation");
        }
    }

    $("#ddlHaulierContactName").change(function () {
        var selIndex = $("#ddlHaulierContactName option:selected").index();
        if (selIndex != 0) {
            $("#err_ddl_ContactName").html("");
            $("#HaulierContactNameExist").val("");
            var arrFax = $("#hdnArrFax").val().split(",");
            var arrEmail = $("#hdnArrEmail").val().split(",");
            var arrAddress1 = $("#hdnAddress1").val().split(",");
            var arrAddress2 = $("#hdnAddress2").val().split(",");
            var arrAddress3 = $("#hdnAddress3").val().split(",");
            var arrAddress4 = $("#hdnAddress4").val().split(",");
            var arrAddress5 = $("#hdnAddress5").val().split(",");
            var arrPostCode = $("#hdnpostcode").val().split(",");
            var arrCountryId = $("#hdncountryid").val().split(",");
            var arrPhone = $("#hdnphonenumber").val().split(",");
            //var arrContactID = $("hdnHULContactID").val().split(",");
            var arrContactID = $("#hdncontactId").val().split
            var arrContactID = $("#hdncontactId").val().split(",");
            var arrCountryID = $("#hdnCountryID").val().split(",");

            $("#EmailIdSORT").val(arrEmail[selIndex - 1]);
            $("#FaxSORT").val(arrFax[selIndex - 1]);
            $('#spnAddressLine_1').text(arrAddress1[selIndex - 1]);
            $('#spnAddressLine_2').text(arrAddress2[selIndex - 1]);
            $('#spnAddressLine_3').text(arrAddress3[selIndex - 1]);
            $('#spnAddressLine_4').text(arrAddress4[selIndex - 1]);
            $('#spnAddressLine_5').text(arrAddress5[selIndex - 1]);
            $("#spnPostCode").text(arrPostCode[selIndex - 1]);
            $("#hdncountryid").text(arrCountryId[selIndex - 1]);
            $('#spnPhone').text(arrPhone[selIndex - 1]);
            //$("#hdnHULContactID").val(arrContactID[selIndex - 1]);
            $('#hdnCONTACTID').val(arrContactID[selIndex - 1]);
            $("#HdnHaulierContactName").val($("#ddlHaulierContactName").val());
            $("#SOHndContactID").val(arrContactID[selIndex - 1]);
            $("#HdnOrgFax").val($("#FaxSORT").val());
            $("#HdnOrgEmailId").val($("#EmailIdSORT").val());
            $("#hdnCountryID").text(arrCountryID[selIndex - 1]);
            var country = $("#CountryID option[value=" + arrCountryID[selIndex - 1] + "]").text();
            $("#spnCountryID").html(country);
        }
        else {
            $("#EmailIdSORT").val('');
            $("#FaxSORT").val('');
        }
    })

    $("#HaulierContactNameExist").keypress(function () {
        $("#ddlHaulierContactName").val("0");
        $("#EmailIdSORT").val("");
        $("#FaxSORT").val("");
        $("#err_ddl_ContactName").html("");
    })


