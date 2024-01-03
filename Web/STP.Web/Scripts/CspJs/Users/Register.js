    var isvalid = true;
    var IsCheck = true;
    var flag = true;
    var pageNum = 1;
    var pageSize = 10;
        var orgid = 0;
        var isSystemCheck = true;

        $(document).ready(function () {
$('body').on('click','#btnback', function() { window['BackToManageUser'](); }); 
        SelectMenu(8);
        $("#CountryId").val(223001);
        $("#CountryId option[value=223001]").attr('selected', 'selected');
        $("#SecurityQuestion option[value=0]").attr('selected', 'selected');
        Clear_TextFeilds();
        $('#addrchk').text('');

        DefaultSetUserTypeName();
        // to prepopulate the UserType field

        //fetching organisation details ecept for Helpdesk user
        var userType = $('#userTypeId').val();
        if (userType != 696009) {
            orgid = $('#Organisation_ID').val();
            FetchOrganisationDetOnLoad(orgid);
        }


        checkValidate();
        addresschk();
        UserNameKeypress();
        OrgNameKeypress();



if($('#hf_mode').val() ==  "") {
            $('#Password').val('');
            $('#ConfirmPassword').val('');
              $("#SecurityQuestion option[value=0]").attr('selected', 'selected');
            $('#SecurityAnswer').val('');
        }

if($('#hf_mode').val() ==  "Edit" && '@ViewBag.mode' != "View" && '@userTypeID' != 696001) {
            $('#IsContactOnly').show();
        }
        else {
            $('#IsContactOnly').hide();
        }

        $("#OrgUser").autocomplete({
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
                $('#OrgUser').val(ui.item.label);
                $('#Selectorg_Id').val(ui.item.value);
                $('#OrganisationName').val(ui.item.label);
                FetchOrganisationDet(ui.item.value)
                return false;
			},
			focus: function (event, ui) {
                $("#OrgUser").val(ui.item.label);
				return false;
			}
    });

        function checkValidate() {
            $("form").submit(function (e) {
                flag = true;
                var userName = $('#UserName').val();
                var orgUser = $('#OrgUser').val();
                var typeId = 1;
               /* var radioval = $('input[type="radio"]:checked').val();*/
                var radioval = $('input[name="ContactPref"]:checked').val();
                var Emailval = $('#Email').val();
                $('#err_UserName_exists').text('');
                $('#err_OrgUser_exists').text('');
                if ($('#OnlyContact').is(':checked')) {

                }
                else {
                    if ($('#UserName').val() != '' && $('#UserName').val() != undefined) {
                        GetUserNameExists(userName, typeId);
                    }
                    if (isvalid) {
                        if (@isAdmin == 1 && @userTypeID == 696006) {
                            if ($('#OrgUser').val() != '' && $('#OrgUser').val() != undefined) {
                                GetOrgNameExists(orgUser)
                            }
                        }
                    }
                }

                if (!isvalid) {
                    flag = false;
                }
                if ($('#AddressLine1').val() == "" && $('#AddressLine2').val() == "" && $('#AddressLine3').val() == "" && $('#AddressLine4').val() == "" && $('#AddressLine5').val() == "") {
                    $('#addrchk').text("Atleast one address line is required");
                    flag = false;
                }
                if (flag) {
                    $('#addrchk').text('');
                }
                else {
                    return false;
                }

                if (radioval == 695001) {
                    if ($('#Fax').val() == "") {
                        $('#error_Fax').html('Fax is required');
                        return false;
                    } else {
                        $('#error_Fax').html('');
                        return true;
                    }
                }
                else if (radioval == 695002) {
                    if ($('#Email').val() == "") {
                        $('#error_Email').html('E-mail address is required');
                        return false;
                    } else {
                        $('#error_Email').html('');
                        return true;
                    }
                } else if (radioval == undefined) {
                    if (Emailval == "" || Emailval == null) {
                        $('#error_Email').html('E-mail address is required');
                        return false;
                    } else {
                        $('#error_Email').html('');
                        return true;
                    }
                }
                else {
                    $('#error_Fax').html('');
                    $('#error_Email').html('');
                    return true;
                }

            })
        }

        $('#CountryId').on('change', function () {
            $('#Country').val($('#CountryID :selected').text());
        });
        $('#UserTypeNameDrop').on('change', function () {
            $('#UserType').val($('#UserTypeNameDrop :selected').val());
        });
        ViewEdit();

if($('#hf_mode').val() ==  "Edit" && '@ViewBag.mode' != "View" && '@userTypeID' != 696001) {
            $('#IsContactOnly').show();
        }
        else {
            $('#IsContactOnly').hide();
        }
        });

      function GetUserNameExists(userName, typeId) {
        var UsrId  = $('#hf_userID').val(); 
        $.ajax({
            url: '@Url.Action("CheckUserNameExists", "Users")',
            type: 'POST',
            cache: false,
            async: false,
            data: { UserName: userName, type: typeId, mode: '@ViewBag.mode', UserId: UsrId },
            beforeSend: function () {
                startAnimation();
            },
            success: function (data) {
                if (typeId == 1) {
                    if (data.result > 0) {
                        $('#err_UserName_exists').text('User name already exists.');
                        isvalid = false;
                    }
                    else {
                        $('#err_UserName_exists').text('');
                        isvalid = true;
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

     function FetchOrganisationDet(orgid) {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: '@Url.Action("ViewOrganisationByID", "Organisation")',
            data: { orgId: orgid, RevisionId: 0 },
            beforeSend: function (xhr) {
                startAnimation()
            }
        }).done(function (Result) {
            var dataCollection = Result;
            if (dataCollection.result.length > 0) {
                $("#AddressLine1").val(dataCollection.result[0].AddressLine1);
                $("#AddressLine2").val(dataCollection.result[0].AddressLine2);
                $("#AddressLine3").val(dataCollection.result[0].AddressLine3);
                $("#AddressLine4").val(dataCollection.result[0].AddressLine4);
                $("#AddressLine5").val(dataCollection.result[0].AddressLine5);
                //$("#CountryId").val(dataCollection.result[0].CountryId);
                $("#PostCode").val(dataCollection.result[0].PostCode);
                $("#CountryId option:selected").removeAttr('selected');
                $("#CountryId").val(dataCollection.result[0].CountryId);
                $("#CountryId option[value='" + dataCollection.result[0].CountryId +"']").attr('selected', 'selected');
                let OrgType = dataCollection.result[0].OrgType;
                if (OrgType == 237007) {
                    $('#td_display_user').show();
                    $('#td_drop_user').hide();
                    $('#UserTypeName').val("Police ALO");
                    $('#UserType').val(696002);
                } else if (OrgType == 237013) {
                    $('#td_display_user').show();
                    $('#td_drop_user').hide();
                    $('#UserTypeName').val("Haulier");
                    $('#UserType').val(696001);
                } else if (OrgType == 237016) {
                    $('#td_drop_user').show();
                    if ($('#user_type_name').val() == 696008) {
                        $('#sort_flag').show();
                    } else {
                        $('#UserType').val(696001);
                    }
                    $('#td_display_user').hide();
                } else {
                    $('#td_display_user').show();
                    $('#td_drop_user').hide();
                    $('#UserTypeName').val("SOA");
                    $('#UserType').val(696007);
                }

                //if (OrgType == 237016) {
                //    $('#td_drop_user').show();
                //    $('#td_display_user').hide();
                //}
                //else {
                //    $('#td_drop_user').hide();
                //    $('#td_display_user').show();
                //    if (OrgType == 237007) {
                //        $('#UserTypeName').val("Police ALO");
                //        $('#UserType').val(696002);
                //    } else if (OrgType == 237013) {
                //        $('#UserTypeName').val("Haulier");
                //        $('#UserType').val(696001);
                //    }
                //    //else if (OrgType == 237016) {
                //    //    if ($('#user_type_name').val() == 696008) {
                //    //        $('#sort_flag').show();
                //    //    } else {
                //    //        $('#UserType').val(696001);
                //    //    }
                //    //}
                //    else {
                //        $('#UserTypeName').val("SOA");
                //        $('#UserType').val(696007);
                //    }
                //}

            }
        }).fail(function (error, a, b) {
        }).always(function (xhr) {
            stopAnimation()
        });
        }

         function FetchOrganisationDetOnLoad(orgid) {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: '@Url.Action("ViewOrganisationByID", "Organisation")',
            data: { orgId: orgid, RevisionId: 0 },
            beforeSend: function (xhr) {
                startAnimation()
            }
        }).done(function (Result) {
            var dataCollection = Result;
            if (dataCollection.result.length > 0) {
                $("#AddressLine1").val(dataCollection.result[0].AddressLine1);
                $("#AddressLine2").val(dataCollection.result[0].AddressLine2);
                $("#AddressLine3").val(dataCollection.result[0].AddressLine3);
                $("#AddressLine4").val(dataCollection.result[0].AddressLine4);
                $("#AddressLine5").val(dataCollection.result[0].AddressLine5);
                //$("#CountryId").val(dataCollection.result[0].CountryId);
                $("#PostCode").val(dataCollection.result[0].PostCode);
                console.log('D' + dataCollection.result[0].CountryId);
                $("#CountryId option:selected").removeAttr('selected');
                $("#CountryId").val(dataCollection.result[0].CountryId);
                $("#CountryId option[value='" + dataCollection.result[0].CountryId +"']").attr('selected', 'selected');
                  
            }
        }).fail(function (error, a, b) {
        }).always(function (xhr) {
            stopAnimation()
        });
    }

    function GetOrgNameExists(orgName) {
        $.ajax({
            url: '@Url.Action("CheckOrganisationExists", "Organisation")',
            type: 'POST',
            cache: false,
            async: false,
            data: { OrganisationName: orgName, type: 1, mode: 'add', orgID: '' },
            beforeSend: function () {
                startAnimation();
            },
            success: function (data) {
                    if (data.result == 0) {
                        $('#err_OrgUser_exists').text('Organisation not exists.');
                        isvalid = false;
                        $('#Selectorg_Id').val(0);
                        $('#OrganisationName').val('');
                        $("#AddressLine1").val('');
                        $("#AddressLine2").val('');
                        $("#AddressLine3").val('');
                        $("#AddressLine4").val('');
                        $("#AddressLine5").val('');
                        $("#CountryId option:selected").removeAttr('selected');
                        $("#CountryId").val(223001);
                        $("#CountryId option[value=223001]").attr('selected', 'selected');
                        $("#PostCode").val('');

                        DefaultSetUserTypeName();
                    }
                    else {
                        $('#err_OrgUser_exists').text('');
                        isvalid = true;
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

        function DefaultSetUserTypeName() {
            var userType = $('#userTypeId').val()
            if (userType == 696001) {
                $('#UserTypeName').val('Haulier');
            }
            else if (userType == 696002) {
                $('#UserTypeName').val('Police ALO');
            }
            else if (userType == 696003) {
                $('#UserTypeName').val('OPS portal');
            }
            else if (userType == 696004) {
                $('#UserTypeName').val('MIS portal');
            }
            else if (userType == 696005) {
                $('#UserTypeName').val('Public portal');
            }
            else if (userType == 696006) {
                $('#UserTypeName').val('Cm administrator');
            }
            else if (userType == 696007) {
                $('#UserTypeName').val('SOA');
            }
            else if (userType == 696008) {
                $('#UserTypeName').val('SO');
            }
        }

    $('#OnlyContact').click(function () {
        if ($('#OnlyContact').is(':checked')) {
            ShowErrorPopup('You are going to create a contact without ESDAL4 login credentials.');
            $("#RB_Email").attr('checked', true);
            $('#error_Email').html('');
            $('#div_usertype').hide();
            $('#div_isadmin').hide();
            $('#Password').val('notneeded!1N');
            $('#ConfirmPassword').val('notneeded!1N');
            $('#UserName').val('notneeded');
            $('#div_username').hide();
            $('#div_password').hide();
            $('#div_confirmpwd').hide();
            $('#IsAdministrator').attr('checked', false);
            $("#UserType option[value='696001']").attr("selected", "selected");
            $('#div_contact_pref1').show();
            $('#div_contact_pref2').show();
            $('#span_role').html('Title');
            $('#div_systempwd').hide();
            $('#div_securityquest').hide();
            $('#div_securityans').hide();
        } else {
            $('#div_usertype').show();
            $('#div_isadmin').show();
            $('#div_username').show();
            $('#div_resetpwd').show();
            $('#div_password').show();
            $('#div_confirmpwd').show();
            $('#div_securityquest').show();
            $('#div_securityans').show();
            $('#UserName').val('');
            $('#Password').val('');
            $('#ConfirmPassword').val('');
            $('#IsAdministrator').attr('checked', false);
            $("#UserType option[value='']").attr("selected", "selected");
            $('#div_contact_pref1').hide();
            $('#div_contact_pref2').hide();
            $('#span_role').html('Role');
            $('#span_Email').html('Email*');
            $("#RB_Email").attr('checked', true);
            $('#error_Email').html('');
            $('#span_Fax').html('Fax');
            $('#error_Fax').html('');
            $('#div_systempwd').show();
        }
    });

    $("#UserType").change(function () {
            $('#div_isadmin').show();
        $('#UserName').val('');
            $('#Password').val('');
            $('#ConfirmPassword').val('');
            $('#span_role').html('Role');
            $('#OnlyContact').attr('checked', false);
            var hasValue = $('#UserType').val();
            if (hasValue == 696001) {
                $('#IsContactOnly').hide();
                //$('#div_role_within_org').hide();
                $('#div_contact_pref1').hide();
                $('#div_contact_pref2').hide();
                $('#OnlyContact').attr('checked', false);

                $('#div_isadmin').show();
                $('#div_username').show();
                // $('#Username').Attr("data-val-required");
                // $('#div_role').show();
                $('#div_resetpwd').show();
                $('#div_password').show();
                $('#div_confirmpwd').show();
                $('#div_securityquest').show();
                $('#div_securityans').show();
                $('#div_systempwd').show();
            }
            else {
                $('#IsContactOnly').show();
                // $('#div_role_within_org').show();
                // $('#div_contact_pref1').show();
                // $('#div_contact_pref2').show();
            }
        });

    $("input:radio[id=RBFax]").click(function () {
        var value = $(this).val();
        if (value == 695001) {
            $('#span_Fax').html('Fax*');
            $('#span_Email').html('Email');
            $('#error_Email').html('');
        }
        else {

            $('#span_Fax').html('Fax');
            $('#span_Email').html('Email*');
        }
    });

        $("input:radio[id=RB_Email]").click(function () {
        var value = $(this).val();
        if (value == 695002) {
            $('#span_Fax').html('Fax');
            $('#span_Email').html('Email*');
            // $('#error_Email').html('');
            $('#error_Fax').html('');
        }
        else {
            $('#span_Fax').html('Fax*');
            $('#span_Email').html('Email');
        }
    });

    $('#IsResetPW').click(function (e) {
        if (IsCheck == true) {
            ShowDialogWarningPop('Do you want to reset password ?', 'No', 'Yes', 'ReloadLocation1', 'ResetPassword_Checked');
        }
        else {
            IsCheck = true;
            $("#Password").val("dummyPW1!");
            $("#ConfirmPassword").val("dummyPW1!");
            $("#Password").attr("readonly", true);
            $("#ConfirmPassword").attr("readonly", true);
            $('#div_systempwd').hide();
            $("#IsSystemPW").prop("checked", false);
            $('#autopassword').html('');
        }

    });

    function ResetPassword_Checked() {
        CloseWarningPopupDialog();
        if ($('#IsResetPW').is(':checked')) {
            IsCheck = false;
            $('#Password,#ConfirmPassword').val('');
            $("#IsResetPW").prop("checked", true);
            $("#Password").attr("readonly", false);
            $("#ConfirmPassword").attr("readonly", false);
            $('#div_systempwd').show();
            $("#IsSystemPW").prop("checked", false);
            $('#autopassword').html('');
        }
        else {
            IsCheck = true;
            $("#Password").val("dummyPW1!");
            $("#ConfirmPassword").val("dummyPW1!");
            $("#Password").attr("readonly", true);
            $("#ConfirmPassword").attr("readonly", true);
            $('#div_systempwd').hide();
            $("#IsSystemPW").prop("checked", false);
            $('#autopassword').html('');

        }
    }
    function ReloadLocation1() {
        CloseWarningPopupDialog();
        $("#IsResetPW").prop("checked", false);
        $("#Password").val("dummyPW1!");
        $("#ConfirmPassword").val("dummyPW1!");
        $("#Password").attr("readonly", true);
        $("#ConfirmPassword").attr("readonly", true);
        }
    function Clear_TextFeilds() {
            $('input').each(function () {
                switch ($(this).attr("id")) {
                    case "UserName": $(this).val(''); break;
                    case "ConfirmPassword": $(this).val(''); break;
                    case "Password": $(this).val(''); break;
                    case "Title": $(this).val(''); break;
                }
            });
            $('input[type="password"]').val('');
        }

     function ViewEdit() {
if($('#hf_mode').val() ==  "Edit") {

            var userID  = $('#hf_userID').val(); 
            var contact_ID  = $('#hf_contactID').val(); 
            $.ajax({
                type: 'POST',
                dataType: 'json',
                url: '@Url.Action("ViewUserByID", "Users")',
                data: { userTypeID: 0, UserId: userID, ContactID: contact_ID },
                beforeSend: function (xhr) {
                    startAnimation();
                }
            }).done(function (Result) {
                var dataCollection = Result;
                if (dataCollection.result.length > 0) {
                    if (contact_ID == "" || contact_ID == null) {
                        $('#UserType').val(dataCollection.result[0].UserType);
                        $('#UserTypeNameDrop>option[value="' + $('#UserType').val() + '"]').prop('selected', true);

                        if (dataCollection.result[0].IsAdministrator == true)
                        {
                            $('#IsAdministrator').prop('checked', true);
                        }
                        $("#UserTypeName").val(dataCollection.result[0].UserTypeName);
                        $("#Selectorg_Id").val(dataCollection.result[0].AdminSelectedOrganisationId);
                        $("#Password").val("dummyPW1!");
                        $("#ConfirmPassword").val("dummyPW1!");

                        $("#Password").attr("readonly", "readonly");
                        $("#ConfirmPassword").attr("readonly", "readonly");

                        if (dataCollection.result[0].SecurityQuestion.toUpperCase() == "MAIDEN NAME OF MOTHER") {
                            $("#SecurityQuestion").val(1);
                        } else if (dataCollection.result[0].SecurityQuestion.toUpperCase() == "FIRST SCHOOL ATTENDED") {
                            $("#SecurityQuestion").val(2);
                        } else if (dataCollection.result[0].SecurityQuestion.toUpperCase() == "PLACE OF BIRTH") {
                            $("#SecurityQuestion").val(3);
                        }
                        else { $("#SecurityQuestion option[value=0]").attr('selected', 'selected');}
                        $("#SecurityAnswer").val(dataCollection.result[0].SecurityAnswer);
                        $("#UserName").val(dataCollection.result[0].UserName);
                        $('#hdnOnlyContact').val(false);

                    }
                    else
                    {
                        $('#div_contact_pref1').show();
                        $('#div_contact_pref2').show();
                        $('#div_usertype').hide();
                        $('#div_isadmin').hide();
                        $('#div_username').hide();
                        $('#div_resetpwd').hide();
                        $('#div_password').hide();
                        $('#div_confirmpwd').hide();
                        $('#div_securityquest').hide();
                        $('#div_securityans').hide();
                        $('#UserName').val('notneeded');
                        $('#Password').val('notneeded!1N');
                        $('#ConfirmPassword').val('notneeded!1N');
                        $('#IsAdministrator').attr('checked', false);
                        $("#UserType option[value='696001']").attr("selected", "selected");
                        $('#span_role').html('Title');
                        $("#Selectorg_Id").val(dataCollection.result[0].Selectorg_Id);
                        if (dataCollection.result[0].ContactPref == "1") {
                            $("#RB_Email").attr('checked', true);
                        }
                        if (dataCollection.result[0].ContactPref == "2") {

                            $("#RBFax").attr('checked', true);
                        }
                        $('#hdnOnlyContact').val(true);
                    }
                    $("#AddressLine1").val(dataCollection.result[0].AddressLine1);
                    $("#AddressLine2").val(dataCollection.result[0].AddressLine2);
                    $("#AddressLine3").val(dataCollection.result[0].AddressLine3);
                    $("#AddressLine4").val(dataCollection.result[0].AddressLine4);
                    $("#AddressLine5").val(dataCollection.result[0].AddressLine5);
                    $("#Telephone").val(dataCollection.result[0].Telephone)

                    var RoleType = dataCollection.result[0].Roletype;
                    var arrayRoleType = RoleType.split(",");
                    var strRoles = "";
                    for (i = 0; i < arrayRoleType.length; i++)
                    {

                        if (arrayRoleType[i] == 226001) {
                            $("#DataHolder").attr("checked", "checked");

                            strRoles = strRoles +"Data holder,"
                        }
                        if (arrayRoleType[i] == 226002) {
                            $("#NotificationContact").attr("checked", "checked");

                            strRoles = strRoles + "Notification contact,"
                        }
                        if (arrayRoleType[i] == 226003) {
                            $("#OfficialContact").attr("checked", "checked");

                            strRoles = strRoles + "Official contact,"
                        }
                        if (arrayRoleType[i] == 226006) {
                            $("#ItContact").attr("checked", "checked");

                            strRoles = strRoles + "It contact,"
                        }
                        if (arrayRoleType[i] == 226008) {
                            $("#DataOwner").attr("checked", "checked");

                            strRoles = strRoles + "Data owner,"
                        }
                    }

                    var srrayToShow = strRoles.split(",");
                    var output = srrayToShow.join("\n");
                    $("#userFormalRole").val(output);

                    $("#FirstName").val(dataCollection.result[0].FirstName);


                    $("#OrgUser").val(dataCollection.result[0].OrgUser);
                    $("#Title").val(dataCollection.result[0].Title);
                    $("#SurName").val(dataCollection.result[0].SurName);
                    $("#CountryId").val(dataCollection.result[0].Country);
                    $("#PostCode").val(dataCollection.result[0].PostCode);
                    $("#Mobile").val(dataCollection.result[0].Mobile);
                    $("#Email").val(dataCollection.result[0].Email);

                    $("#Notes").html(dataCollection.result[0].Notes);
                    $("#Extension").val(dataCollection.result[0].Extension);
                    $('#Fax').val(dataCollection.result[0].Fax);
                    $("#HContactID").val(dataCollection.result[0].ContactId);
                    $("#OrgList").val(dataCollection.result[0].AdminSelectedOrganisationId);
                    $('#div_systempwd').hide();

                    //if (dataCollection.result[0].UserType == 696008) {
                    //    $('.sort_flag').show();

                    //    $('#hdnSORTCreateJob').val(dataCollection.result[0].SORTCreateJob);
                    //    $('#hdnSORTAllocateJob').val(dataCollection.result[0].SORTAllocateJob);
                    //    $('#hdnSORTCanApproveSignVR1').val(dataCollection.result[0].SORTCanApproveSignVR1);
                    //    $('#hdnSORTCanAgreeUpto150').val(dataCollection.result[0].SORTCanAgreeUpto150);
                    //    $('#hdnSORTCanAgreeAllSO').val(dataCollection.result[0].SORTCanAgreeAllSO);

                    //}

                    stopAnimation();
                }
            }).fail(function (error, a, b) {

            }).always(function (xhr) {

            });
        }
        }

        function LoadUserList(data) {
            if (data.result) {
                if (data.successMsg == 'Edit') {
                    var Msg = 'User ' + "\"" + data.name + "\" updated successfully.";
                    ShowSuccessModalPopup(Msg, 'ShowAddedUsers');
                }
                if (data.successMsg == 'EditContact') {
                    var Msg = 'Contact user ' + "\"" + data.name + "\" updated successfully.";
                    ShowSuccessModalPopup(Msg, 'ShowAddedUsers');
                }
                if (data.successMsg == 'ContactUser') {
                    var Msg = 'New contact user ' + "\"" + data.name + "\" created successfully.";
                    ShowSuccessModalPopup(Msg, 'ShowAddedUsers');
                }
                if (data.successMsg == 'Create') {
                    var Msg = 'New user ' + "\"" + data.name + "\" created successfully.";
                    ShowSuccessModalPopup(Msg, 'ShowAddedUsers');
                }
            }
        }

        function ShowAddedUsers() {
            CloseSuccessModalPopup();
            window.location.href = '/Account/ManageUsers';
        }
        $('#IsSystemPW').click(function (e) {
            if (isSystemCheck == true) {
                ShowDialogWarningPop('Do you want to generate auto password ?', 'No', 'Yes', 'ReloadLocationSystemPassword', 'SystemPassword_Checked');
            }
            else {
                isSystemCheck = true;
                $('#Password,#ConfirmPassword').val('');
                $('#autopassword').html('');
                $("#Password").attr("readonly", false);
                $("#ConfirmPassword").attr("readonly", false);
            }

        });
        function SystemPassword_Checked() {
            CloseWarningPopupDialog();
            if ($('#IsSystemPW').is(':checked')) {
                isSystemCheck = false;
                $('#Password,#ConfirmPassword').val('');
                $("#IsSystemPW").prop("checked", true);
                $("#Password").attr("readonly", false);
                $("#ConfirmPassword").attr("readonly", false);
                $('#autopassword').html('');
                $.ajax({
                    type: "POST",
                    url: "../Users/SystemAutoGeneratePassword",
                    data: {},
                    success: function (data) {
                        $('#autopassword').html(data.result);
                        $('#Password').val(data.result);
                        $('#ConfirmPassword').val(data.result);
                        $("#Password").attr("readonly", true);
                        $("#ConfirmPassword").attr("readonly", true);
                    },
                    error: function (result) {
                        $('#autopassword').html('');
                        $('#Password,#ConfirmPassword').val('');
                        $("#Password").attr("readonly", false);
                        $("#ConfirmPassword").attr("readonly", false);
                    }
                });
            }
            else {
                isSystemCheck = true;
                $('#Password,#ConfirmPassword').val('');
                $("#Password").attr("readonly", false);
                $("#ConfirmPassword").attr("readonly", false);
                $('#autopassword').html('');
            }
        }
        function ReloadLocationSystemPassword() {
            CloseWarningPopupDialog();
            $("#IsSystemPW").prop("checked", false);
            $("#Password").val('');
            $("#ConfirmPassword").val('');
            $("#Password").attr("readonly", false);
            $("#ConfirmPassword").attr("readonly", false);
            $('#autopassword').html('');
        }
        $(document).ready(function () {
            var userID  = $('#hf_userID').val(); 
            var contact_ID  = $('#hf_contactID').val(); 
            $.ajax({
                type: 'POST',
                dataType: 'json',
                url: '@Url.Action("ViewUserByID", "Users")',
                data: { userTypeId: 0, UserId: userID, ContactId: contact_ID },


                beforeSend: function (xhr) {

                }
            }).done(function (Result) {
                var dataCollection = Result;
                if (dataCollection.result.length > 0) {
                    if (contact_ID == "" || contact_ID == null) {

                        $("#@ViewBag.userID").find('#UserType').text(dataCollection.result[0].UserType);

                        if (dataCollection.result[0].IsAdministrator == true) {
                            $("#@ViewBag.userID").find('#IsAdministrator').prop('checked', true);
                            $("#@ViewBag.userID").find("#viewIsAdminTrue").show();
                        }
                        else {
                            $("#@ViewBag.userID").find("#viewIsAdminFalse").show();
                        }
                        $("#@ViewBag.userID").find('#userOrgName').text(dataCollection.result[0].OrgUser)
                        $("#@ViewBag.userID").find('#userType').text(dataCollection.result[0].UserTypeName);
                        $("#@ViewBag.userID").find('#userFirstName').text(dataCollection.result[0].FirstName);
                        $("#@ViewBag.userID").find('#userSurname').text(dataCollection.result[0].SurName);
                        $("#@ViewBag.userID").find('#userName').text(dataCollection.result[0].UserName);
                        $("#@ViewBag.userID").find('#userRole').text(dataCollection.result[0].Title);

                        if (dataCollection.result[0].SecurityQuestion.toUpperCase() == "MAIDEN NAME OF MOTHER") {
                            var SecurityQuestion  = $('#hf_userID").find("#userSecurityId option[value=" + 1 + "]').val(); 
                            $("#@ViewBag.userID").find("#userSecurityQues").text(SecurityQuestion);
                        } else if (dataCollection.result[0].SecurityQuestion.toUpperCase() == "FIRST SCHOOL ATTENDED") {
                            var SecurityQuestion  = $('#hf_userID").find("#userSecurityId option[value=" + 2 + "]').val(); 
                            $("#@ViewBag.userID").find("#userSecurityQues").text(SecurityQuestion);
                        } else if (dataCollection.result[0].SecurityQuestion.toUpperCase() == "PLACE OF BIRTH") {
                            var SecurityQuestion  = $('#hf_userID").find("#userSecurityId option[value=" + 3 + "]').val(); 
                            $("#@ViewBag.userID").find("#userSecurityQues").text(SecurityQuestion);
                        }

                        $("#@ViewBag.userID").find("#userSecurityAns").text(dataCollection.result[0].SecurityAnswer);
                        $("#@ViewBag.userID").find("#AddressLine1").text(dataCollection.result[0].AddressLine1);
                        $("#@ViewBag.userID").find("#AddressLine2").text(dataCollection.result[0].AddressLine2);
                        $("#@ViewBag.userID").find("#AddressLine3").text(dataCollection.result[0].AddressLine3);
                        $("#@ViewBag.userID").find("#AddressLine4").text(dataCollection.result[0].AddressLine4);
                        $("#@ViewBag.userID").find("#AddressLine5").text(dataCollection.result[0].AddressLine5);
                        $("#@ViewBag.userID").find("#userTelephone").text(dataCollection.result[0].Telephone);
                        var country  = $('#hf_userID").find("#userCountryID option[value=" + dataCollection.result[0].Country + "]').val(); 
                        $("#@ViewBag.userID").find("#userCountry").text(country);
                        $("#@ViewBag.userID").find("#userPostCode").text(dataCollection.result[0].PostCode);
                        $("#@ViewBag.userID").find("#userExtension").text(dataCollection.result[0].Extension);
                        $("#@ViewBag.userID").find("#userMobile").text(dataCollection.result[0].Mobile);
                        $("#@ViewBag.userID").find("#userFax").text(dataCollection.result[0].Fax);
                        $("#@ViewBag.userID").find("#userEmail").text(dataCollection.result[0].Email);
                        $("#@ViewBag.userID").find("#userNotes").text(dataCollection.result[0].Notes);
                        var RoleType = dataCollection.result[0].Roletype;
                        var RoleType = dataCollection.result[0].Roletype;
                        var arrayRoleType = RoleType.split(",");
                        var strRoles = "";
                        for (i = 0; i < arrayRoleType.length; i++) {

                            if (arrayRoleType[i] == 226001) {
                                $("#Dataholder").attr("checked", "checked");

                                strRoles = strRoles + "Data holder,"


                            }
                            if (arrayRoleType[i] == 226002) {
                                $("#Notificationcontact").attr("checked", "checked");

                                strRoles = strRoles + "Notification contact,"

                            }
                            if (arrayRoleType[i] == 226003) {
                                $("#Officialcontact").attr("checked", "checked");

                                strRoles = strRoles + "Official contact,"

                            }
                            if (arrayRoleType[i] == 226006) {
                                $("#Itcontact").attr("checked", "checked");

                                strRoles = strRoles + "It contact,"

                            }
                            if (arrayRoleType[i] == 226008) {
                                $("#Dataowner").attr("checked", "checked");

                                strRoles = strRoles + "Data owner,"

                            }

                        }
                        var srrayToShow = strRoles.split(",");
                        var output = srrayToShow.join("<br>");
                        $("#@ViewBag.userID").find("#userFormalRole").html(output);
                    }
                    else {
                        $("#@ViewBag.contactID").find('#userOrgName').text(dataCollection.result[0].OrgUser)
                        $("#@ViewBag.contactID").find('#userFirstName').text(dataCollection.result[0].FirstName);
                        $("#@ViewBag.contactID").find('#userSurname').text(dataCollection.result[0].SurName);
                        $("#@ViewBag.contactID").find('#userName').text(dataCollection.result[0].UserName);

                        $("#@ViewBag.contactID").find("#AddressLine1").text(dataCollection.result[0].AddressLine1);
                        $("#@ViewBag.contactID").find("#AddressLine2").text(dataCollection.result[0].AddressLine2);
                        $("#@ViewBag.contactID").find("#AddressLine3").text(dataCollection.result[0].AddressLine3);
                        $("#@ViewBag.contactID").find("#AddressLine4").text(dataCollection.result[0].AddressLine4);
                        $("#@ViewBag.contactID").find("#AddressLine5").text(dataCollection.result[0].AddressLine5);
                        $("#@ViewBag.contactID").find("#userTelephone").text(dataCollection.result[0].Telephone);
                        var country  = $('#hf_contactID").find("#userCountryID option[value=" + dataCollection.result[0].Country + "]').val(); 
                        $("#@ViewBag.contactID").find("#userCountry").text(country);
                        $("#@ViewBag.contactID").find("#userPostCode").text(dataCollection.result[0].PostCode);
                        $("#@ViewBag.contactID").find("#userExtension").text(dataCollection.result[0].Extension);
                        $("#@ViewBag.contactID").find("#userMobile").text(dataCollection.result[0].Mobile);
                        $("#@ViewBag.contactID").find("#userFax").text(dataCollection.result[0].Fax);
                        $("#@ViewBag.contactID").find("#userEmail").text(dataCollection.result[0].Email);
                        $("#@ViewBag.contactID").find("#userNotes").text(dataCollection.result[0].Notes);
                        $("#@ViewBag.contactID").find('#userTitle').text(dataCollection.result[0].Title);
                        if (dataCollection.result[0].ContactPref == "1") {
                            $("#@ViewBag.contactID").find('#rbEmail').attr('checked', true);
                        }
                        if (dataCollection.result[0].ContactPref == "2") {

                            $("#@ViewBag.contactID").find('#rbFax').attr('checked', true);
                        }

                        var RoleType = dataCollection.result[0].Roletype;
                        var RoleType = dataCollection.result[0].Roletype;
                        var arrayRoleType = RoleType.split(",");
                        var strRoles = "";
                        for (i = 0; i < arrayRoleType.length; i++) {

                            if (arrayRoleType[i] == 226001) {
                                $("#Dataholder").attr("checked", "checked");

                                strRoles = strRoles + "Data holder,"


                            }
                            if (arrayRoleType[i] == 226002) {
                                $("#Notificationcontact").attr("checked", "checked");

                                strRoles = strRoles + "Notification contact,"

                            }
                            if (arrayRoleType[i] == 226003) {
                                $("#Officialcontact").attr("checked", "checked");

                                strRoles = strRoles + "Official contact,"

                            }
                            if (arrayRoleType[i] == 226006) {
                                $("#Itcontact").attr("checked", "checked");

                                strRoles = strRoles + "It contact,"

                            }
                            if (arrayRoleType[i] == 226008) {
                                $("#Dataowner").attr("checked", "checked");

                                strRoles = strRoles + "Data owner,"

                            }

                        }
                        var srrayToShow = strRoles.split(",");
                        var output = srrayToShow.join("<br>");
                        $("#@ViewBag.contactID").find("#userFormalRole").html(output);
                    }
                }
            }).fail(function (error, a, b) {

            }).always(function (xhr) {

            });
        });
        $('body').on('click', '.closedet', function (e) {
       
        e.preventDefault();
        var id = $(this).data('id');

        closeDetails(id);
        });
        function closeDetails(id) {
            $("#description-" + id).hide();
        }

