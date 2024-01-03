var isSearchStarted = false;
var isApiCallPending = false;
var showAnimation = true;
$(document).ready(function () {
    var pageSize = $(this).val();
    $('#pageSizeVal').val(pageSize);
    SelectMenu(8);
    SetSearchText();
    var createAlertMsg = $('#CreateAlert').val();
    var contactAlertMsg = $('#ContactAlert').val();
    var strActionType = $('#ActionType').val();
    if (createAlertMsg == "True") {
        if (strActionType == "New")
            ShowAddedUsers();
    }
    else if (contactAlertMsg == "True") {
        if (strActionType == "New")
            ShowAddedContacts();
    }

    $("#searchString").autocomplete({
        appendTo: $("#searchString").parent(),
        source: function (request, response) {
            let disabledUsers = false, disabledContacts = false, showContacts = false;
            if ($("#chk_DisabledUsers").is(':checked')) { disabledUsers = true; }
            if ($("#chk_Contact").is(':checked')) { showContacts = true; }
            if ($("#chk_DisabledContacts").is(':checked')) { disabledContacts = true; }

            var usercontactSearchItems = {};
            usercontactSearchItems.DisabledUsers = disabledUsers;
            usercontactSearchItems.DisabledContacts = disabledContacts;
            usercontactSearchItems.ShowContacts = showContacts;
            usercontactSearchItems.Criteria = $("#searchString").val();
            usercontactSearchItems.SearchType = $('#DDsearchCriteria option:selected').val();
            usercontactSearchItems.SearchName = $('#DDsearchCriteria option:selected').text();
            usercontactSearchItems.OrganisationCode = null;

            $.ajax({
                url: '../Account/SearchUserCriteria',
                type: 'POST',
                cache: false,
                async: false,
                data: { objUserSearch: usercontactSearchItems },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item, value: item
                        };
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
            $('#searchString').val(ui.item.label); // display the selected text
            return false;
        },
        focus: function (event, ui) {
            /* $("#searchString").val(ui.item.label);*/
            return false;
        }
    });

    $('body').on('click', '#btnLoginAsUser', function () {
        var userName = $(this).data('username');
        LoginAsOtherUser(userName);
    });
    $('body').on('click', '.btn-delete', function () {
        var UserId = $(this).data('userid');
        var UserName = $(this).data('username');
        Delete(UserId, UserName);
    });

    $('body').on('change', '#chkDisabledUsersOutside', function () {//outside filter toggle change
        if ($(this).prop('checked') == true) 
            $('#chk_DisabledUsers').prop('checked', true);////inside filter
        else 
            $('#chk_DisabledUsers').prop('checked', false);//inside filter
        SearchDetails();
    });

    //Outside textbox keyup - txtUserNameOutside
    $('body').on('keyup', '#txtUserNameOutside', function () {
        if (isSearchStarted) {
            isApiCallPending = true;
            return false;
        }
        isSearchStarted = true;
        showAnimation = false;
        //On outside textbox keyup, we need to change the textbox value and dropdown value inside the filter
        var userNameOutsideVal = $('#txtUserNameOutside').val();
        $("#searchString").val(userNameOutsideVal);
        $("#DDsearchCriteria").val("1").attr("selected", "selected");
        SearchDetails();
    });

    $('body').on('click', '.btn-create-user-info', function () {
        CreateNewUserInfo();
    });

    $('body').on('click', '.img-filter-clear,.btn-clear-search', function () {
        clearSearch();
    });
    $('body').on('click', '.btn-delete-contact', function () {
        var contactId = $(this).data('contactid');
        var firstName = $(this).data('firstname');
        DeleteContact(contactId, firstName);
    });
    $('body').on('click', '.btn-enable-contact', function () {
        var contactId = $(this).data('contactid');
        var contactName = $(this).data('firstname');
        EnableContact(contactId, contactName);
    });

    $('body').on('click', '.btn-enable-user', function () {
        var UserId = $(this).data('userid');
        var UserName = $(this).data('username');
        EnableUser(UserId, UserName);
    });

    $('body').on('click', '#chk_DisabledUsers', function () {
        ShowDisable();
    });

    $('body').on('click', '#chk_Contact', function () {
        ShowContacts();
    });

    $('body').on('click', '#chk_DisabledContacts', function () {
        ShowdisabCont();
    });

    $('body').on('click', '.btn-search-details', function () {
        SearchDetails();
    });

    $('body').on('click', '.btn-edit', function () {
        var userid = $(this).data('userid');
        Edit(userid);
    });

    $('body').on('click', '.btn-edit-contact', function () {
        var contactid = $(this).data('contactid');
        EditContact(contactid);
    });

    $('body').on('change', '#DDsearchCriteria', function () {
        SetSearchText()
    });

    $('body').on('click', '.btn-view-user', function (e) {
        var userid = $(this).data('userid');
        var contactid = $(this).data('contactid');

        ViewUser(userid, contactid);
    });

    $('body').on('change', '.ManageUsers-Pag #pageSizeSelect', function () {
        var page = getUrlParameterByName("page", this.href);
        $('#pageNum').val(page);
        var pageSize = $(this).val();
        $('#pageSizeVal').val(pageSize);
        SearchDetails(isSort = true, pageSize);
    });

    $('body').on('click', '#userpaginator a', function (e) {
        e.preventDefault();
        var page = getUrlParameterByName("page", this.href);
        $('#pageNum').val(page);
        SearchDetails(isSort = true);//using sorting as true to avoid page reset
    });

    $('body').on('click', '.closedet', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        closeDetails(id);
    });


});
function ViewUserData(userid, contactid) {

    hf_mode = $("#hf_mode").val();
    hf_userID = userid || $("#hf_userID").val();
    hf_usertypeId = $("#hf_userTypeID").val();
    hf_IsAdmin = $("#hf_IsAdmin").val();
    hf_contactId = contactid || $("#hf_contactID").val();
    var userID = userid || $("#hf_userID").val();
    var contact_ID = contactid || $('#hf_contactID').val();
    if (hf_usertypeId == 696006) {
        $("#" + hf_userID).attr('colspan', 7);
    }
    else {
        $("#" + hf_userID).attr('colspan', 5);
    }
    openContentLoader("#" + hf_userID);
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: '../Users/ViewUserByID',
        data: { userTypeId: 0, UserId: userID, ContactId: contact_ID },
        beforeSend: function (xhr) {

        }
    }).done(function (Result) {
        var dataCollection = Result;
        if (dataCollection.result.length > 0) {
            if ($("#" + hf_userID).length > 0) {
                $("#" + hf_userID).find('#UserType').text(dataCollection.result[0].UserType);
                if (dataCollection.result[0].IsAdministrator == true) {
                    $("#" + hf_userID).find('#IsAdministrator').prop('checked', true);
                    $("#" + hf_userID).find("#viewIsAdminTrue").show();
                }
                else {
                    $("#" + hf_userID).find("#viewIsAdminFalse").show();
                }
                $("#" + hf_userID).find('#userOrgName').text(dataCollection.result[0].OrgUser)
                $("#" + hf_userID).find('#userType').text(dataCollection.result[0].UserTypeName);
                $("#" + hf_userID).find('#userFirstName').text(dataCollection.result[0].FirstName);
                $("#" + hf_userID).find('#userSurname').text(dataCollection.result[0].SurName);
                $("#" + hf_userID).find('#userName').text(dataCollection.result[0].UserName);
                $("#" + hf_userID).find('#userRole').text(dataCollection.result[0].Title);

                if (dataCollection.result[0].SecurityQuestion != null) {
                    var value = 0;
                    if (dataCollection.result[0].SecurityQuestion.toUpperCase() == "MAIDEN NAME OF MOTHER") {
                        value = 1;
                    } else if (dataCollection.result[0].SecurityQuestion.toUpperCase() == "FIRST SCHOOL ATTENDED") {
                        value = 2;
                    } else if (dataCollection.result[0].SecurityQuestion.toUpperCase() == "PLACE OF BIRTH") {
                        value = 3;
                    }
                    var SecurityQuestion = $("#" + hf_userID).find("#userSecurityId option[value=" + value + "]").text();
                    $("#" + hf_userID).find("#userSecurityQues").text(SecurityQuestion);
                    $("#" + hf_userID).find("#userSecurityAns").text(dataCollection.result[0].SecurityAnswer);
                }
                $("#" + hf_userID).find("#AddressLine1").text(dataCollection.result[0].AddressLine1);
                $("#" + hf_userID).find("#AddressLine2").text(dataCollection.result[0].AddressLine2);
                $("#" + hf_userID).find("#AddressLine3").text(dataCollection.result[0].AddressLine3);
                $("#" + hf_userID).find("#AddressLine4").text(dataCollection.result[0].AddressLine4);
                $("#" + hf_userID).find("#AddressLine5").text(dataCollection.result[0].AddressLine5);
                $("#" + hf_userID).find("#userTelephone").text(dataCollection.result[0].Telephone);
                $("#" + hf_userID).find("#userCountryID option[value=" + dataCollection.result[0].Country + "]").val();
                $("#" + hf_userID).find("#userCountry").text(camelize(dataCollection.result[0].CountryName));
                $("#" + hf_userID).find("#userPostCode").text(dataCollection.result[0].PostCode);
                $("#" + hf_userID).find("#userExtension").text(dataCollection.result[0].Extension);
                $("#" + hf_userID).find("#userMobile").text(dataCollection.result[0].Mobile);
                $("#" + hf_userID).find("#userFax").text(dataCollection.result[0].Fax);
                $("#" + hf_userID).find("#userEmail").text(dataCollection.result[0].Email);
                $("#" + hf_userID).find("#userNotes").text(dataCollection.result[0].Notes);
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
                $("#" + hf_userID).find("#userFormalRole").html(output);
            }
            else {
                $("#" + hf_contactId).find('#userOrgName').text(dataCollection.result[0].OrgUser);
                $("#" + hf_contactId).find('#userFirstName').text(dataCollection.result[0].FirstName);
                $("#" + hf_contactId).find('#userSurname').text(dataCollection.result[0].SurName);
                $("#" + hf_contactId).find('#userName').text(dataCollection.result[0].UserName);
                $("#" + hf_contactId).find("#AddressLine1").text(dataCollection.result[0].AddressLine1);
                $("#" + hf_contactId).find("#AddressLine2").text(dataCollection.result[0].AddressLine2);
                $("#" + hf_contactId).find("#AddressLine3").text(dataCollection.result[0].AddressLine3);
                $("#" + hf_contactId).find("#AddressLine4").text(dataCollection.result[0].AddressLine4);
                $("#" + hf_contactId).find("#AddressLine5").text(dataCollection.result[0].AddressLine5);
                $("#" + hf_contactId).find("#userTelephone").text(dataCollection.result[0].Telephone);
                $("#" + hf_contactId).find("#userCountryID option[value=" + dataCollection.result[0].Country + "]").val();
                $("#" + hf_contactId).find("#userCountry").text(camelize(dataCollection.result[0].CountryName));
                $("#" + hf_contactId).find("#userPostCode").text(dataCollection.result[0].PostCode);
                $("#" + hf_contactId).find("#userExtension").text(dataCollection.result[0].Extension);
                $("#" + hf_contactId).find("#userMobile").text(dataCollection.result[0].Mobile);
                $("#" + hf_contactId).find("#userFax").text(dataCollection.result[0].Fax);
                $("#" + hf_contactId).find("#userEmail").text(dataCollection.result[0].Email);
                $("#" + hf_contactId).find("#userNotes").text(dataCollection.result[0].Notes);
                $("#" + hf_contactId).find('#userTitle').text(dataCollection.result[0].Title);
                if (dataCollection.result[0].ContactPref == "1") {
                    $("#" + hf_contactId).find('#rbEmail').attr('checked', true);
                }
                if (dataCollection.result[0].ContactPref == "2") {
                    $("#" + hf_contactId).find('#rbFax').attr('checked', true);
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
                $("#" + hf_contactId).find("#userFormalRole").html(output);
            }
        }
        closeContentLoader("#" + hf_userID);
    }).fail(function (error, a, b) {

    }).always(function (xhr) {

    });
}

function LoginAsOtherUser(userName) {
    let user_Name = userName;
    $.ajax({
        url: '../Account/LoginAsAnotherUser',
        data: { user: user_Name },
        type: 'POST',
        async: false,
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
            window.location.href = result.redirectToUrl;
        },
        error: function (xhr, textStatus, errorThrown) {
            alert('Login Failed');
        },
        complete: function () {
            stopAnimation();
        }
    });

}

function addresschk() {
    $('#AddressLine1, #AddressLine2, #AddressLine3, #AddressLine4, #AddressLine5').change(function () {
        $('#addrchk').text('');
    });

    $('#Email').keypress(function () {
        $('#error_Email').html('');
    });
}

function UserNameKeypress() {
    $('#UserName').keypress(function () {
        $('#err_UserName_exists').text('');
    });
}
function BackToManageUser() {
    window.location.href = "/Account/ManageUsers";
}
function OrgNameKeypress() {
    $('#OrgUser').keypress(function () {
        $('#err_OrgUser_exists').text('');
    });
}

function EditUserInfo(id) {
    startAnimation();
    window.location.href = "/Users/Register?criteria=&mode=Edit&userID=" + id + "";
}
function CreateNewUserInfo() {
    startAnimation();
    window.location.href = "/Users/Register";
}

function EditContactInfo(cntid) {
    startAnimation();
    window.location.href = "/Users/Register?criteria=&mode=Edit&contactID=" + cntid + "";
}

function DisableUser(UserId, DeleteVal) {
    CloseWarningPopup();
    startAnimation();
    $.ajax({
        url: '../Users/DeleteUser',
        type: 'POST',
        cache: false,
        data: { UserId: UserId, deleteVal: DeleteVal },
        success: function (result) {
            if (result.Success) {
                if (DeleteVal == 1)
                    ShowSuccessModalPopup('Disabled successfully', 'ReloadUserList');
                else
                    ShowSuccessModalPopup('Enabled successfully', 'ReloadUserList');
            }
            else {
                ShowDialogWarningPop('User selected is a notifying contact disabling is not allowed!', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
            }
        }
    });
    stopAnimation();
}

function Delete(UserId, UserName) {
    startAnimation();
    var isAccessible = $('#EditForAdmin').val();
    if (isAccessible == 'False') {
        ShowDialogWarningPop('You are not authorized to disable it.', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
    }
    else {
        var Name = "Do you want to disable '" + "" + "'" + UserName + "'" + "" + "' ?";
        ShowWarningPopup(Name, 'DisableUser', '', UserId, 1)
    }
    stopAnimation();
}

function DeleteContact(contactId, contactName) {
    CloseWarningPopup();
    startAnimation();
    var isAccessible = $('#EditForAdmin').val();
    if (isAccessible == 'False') {
        ShowDialogWarningPop('You are not authorized to disable it.', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
    }
    else {
        var Name = "Do you want to disable '" + "" + "'" + contactName + "'" + "" + "' ?";
        ShowWarningPopup(Name, 'DisableContact', '', contactId, 1)
    }
    stopAnimation();
}

function DisableContact(contactId, DeleteVal) {
    CloseWarningPopup();
    startAnimation();
    $.ajax({
        url: '../Users/DeleteContact',
        type: 'POST',
        cache: false,
        data: { ContactId: contactId, deleteVal: DeleteVal },
        success: function (result) {
            if (result.Success) {
                if (DeleteVal == 1)
                    ShowSuccessModalPopup('Disabled successfully', 'ReloadUserList');
                else
                    ShowSuccessModalPopup('Enabled successfully', 'ReloadUserList');
            }
            else {
                ShowDialogWarningPop('Error on the page', 'Ok', '', 'WarningCancelBtn', '', 1, 'error');
            }
        }
    });
    stopAnimation();
}

function viewotheroptions() {
    if (document.getElementById('viewotheroptions').style.display !== "none") {
        document.getElementById('viewotheroptions').style.display = "none"
        document.getElementById('chevlon-up-icon1').style.display = "none"
        document.getElementById('chevlon-down-icon1').style.display = "block"
    }
    else {
        document.getElementById('viewotheroptions').style.display = "block"
        document.getElementById('chevlon-up-icon1').style.display = "block"
        document.getElementById('chevlon-down-icon1').style.display = "none"
    }
}

function clearSearch() {
    $('#txtsearchOrganisationCode').val('');
    $('#searchString').val('');
    $('#txtUserNameOutside').val('');//clear outside username textbox
    $("#chk_DisabledUsers").prop("checked", false);
    $('#chkDisabledUsersOutside').prop("checked", false);//uncheck outside toggle
    $('#chk_Contact').prop("checked", false);
    $('#chk_DisabledContacts').prop("checked", false);
    BindCriteriaType(1);
    $('#div_chkcntdisble').show();
    //$('#div_chkdisable').show();
    $('#div_chkdisblecont').show();
    SearchDetails(false);
}

function ReloadUserList() {
    CloseSuccessModalPopup();
    window.location.reload();
}

function EnableContact(contactId, contactName) {
    startAnimation();
    var isAccessible = $('#EditForAdmin').val();
    if (isAccessible == 'False') {
        ShowDialogWarningPop('You are not authorized to enable it.', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
    }
    else {
        var Name = "Do you want to enable '" + "" + "'" + contactName + "'" + "" + "' ?";
        ShowWarningPopup(Name, 'DisableContact', '', contactId, 0)

    }
    stopAnimation();
}

function EnableUser(UserId, UserName) {
    startAnimation();
    var isAccessible = $('#EditForAdmin').val();
    if (isAccessible == 'False') {
        ShowDialogWarningPop('You are not authorized to enable it.', 'Ok', '', 'WarningCancelBtn', '', 1, 'info');
    }
    else {
        var Name = "Do you want to enable '" + "" + "'" + UserName + "'" + "" + "' ?";
        ShowWarningPopup(Name, 'DisableUser', '', UserId, 0)

    }
    stopAnimation();
}
function BindCriteriaType(contact) {
    $("#DDsearchCriteria option").remove();

    if (contact == 1) {
        $('#DDsearchCriteria').append('<option value=1>User Name</option>');
        $('#DDsearchCriteria').append('<option value=2>First Name</option>');
        $('#DDsearchCriteria').append('<option value=3>Surname</option>');
        $('#DDsearchCriteria').append('<option value=4>Organisation Name</option>');
        $('#searchString').attr("placeholder", "User Name");
    }
    else if (contact == 0) {
        $('#DDsearchCriteria').append('<option value=2>First Name</option>');
        $('#DDsearchCriteria').append('<option value=1>User Name</option>');
        $('#DDsearchCriteria').append('<option value=3>Surname</option>');
        $('#DDsearchCriteria').append('<option value=4>Organisation Name</option>');
        $('#searchString').attr("placeholder", "First Name");
    }
    else if (contact == 2) {
        $('#DDsearchCriteria').append('<option value=2>First Name</option>');
        $('#DDsearchCriteria').append('<option value=1>User Name</option>');
        $('#DDsearchCriteria').append('<option value=3>Surname</option>');
        $('#DDsearchCriteria').append('<option value=4>Organisation Name</option>');
        $('#searchString').attr("placeholder", "First Name");
    }
    else if (contact == 3) {
        $('#DDsearchCriteria').append('<option value=3>Surname</option>');
        $('#DDsearchCriteria').append('<option value=1>User Name</option>');
        $('#DDsearchCriteria').append('<option value=2>First Name</option>');
        $('#DDsearchCriteria').append('<option value=4>Organisation Name</option>');
        $('#searchString').attr("placeholder", "Surname");
    }
    else if (contact == 4) {
        $('#DDsearchCriteria').append('<option value=4>Organisation Name</option>');
        $('#DDsearchCriteria').append('<option value=2>First Name</option>');
        $('#DDsearchCriteria').append('<option value=3>Surname</option>');

        $('#searchString').attr("placeholder", "Organisation Name");
    }
    else {
        $("#DDsearchCriteria").append('<option value=1>User Name</option>');
        $('#DDsearchCriteria').append('<option value=2>First Name</option>');
        $('#DDsearchCriteria').append('<option value=3>Surname</option>');
        $('#DDsearchCriteria').append('<option value=4>Organisation Name</option>');
        $('#searchString').attr("placeholder", "User Name");
        /*$('#searchString').val('');*/
    }
}

function ShowDisable() {
    $("#chk_Contact").prop("checked", false);
    $("#chk_DisabledContacts").prop("checked", false);
    if ($("#chk_DisabledUsers").is(':checked')) {
        isDisabledUsers = true;
        $('#div_chkcntdisble').hide();
        $('#div_chkdisblecont').hide();

    } else {
        $("#chk_DisabledUsers").prop("checked", false);
        $('#div_chkcntdisble').show();
        $('#div_chkdisblecont').show();
    }
    var searchcriteria = $('#DDsearchCriteria').val();
    BindCriteriaType(searchcriteria);
    SearchDetails(false);
}

function ShowContacts() {
    $("#chk_DisabledContacts").prop("checked", false);
    $("#chk_DisabledUsers").prop("checked", false);
    if ($("#chk_Contact").is(':checked')) {
        isShowContacts = true;
        if ($("#chk_Contact").is(':checked') && ($('#searchString').val() != "")) {
            isShowContacts = true;
            var searchcriteria = $('#DDsearchCriteria').val();
            BindCriteriaType(searchcriteria);
            SearchDetails(false);
        }
        else {
            //$('#div_chkdisable').hide();
            $('#div_chkdisblecont').hide();
            BindCriteriaType(0);
            SearchDetails(false);
        }
    }
    else if ($("#chk_Contact").prop("checked", false) && ($('#searchString').val() != "")) {
        //$('#div_chkdisable').show();
        $('#div_chkdisblecont').show();
        var searchcriteria = $('#DDsearchCriteria').val();
        BindCriteriaType(searchcriteria);
        SearchDetails(false);
    }
    else {
        $("#chk_Contact").prop("checked", false);
        //$('#div_chkdisable').show();
        $('#div_chkdisblecont').show();
        BindCriteriaType(1);
        SearchDetails(false);
    }



}

function ShowdisabCont() {
    $("#chk_DisabledUsers").prop("checked", false);
    $("#chk_Contact").prop("checked", false);
    if ($("#chk_DisabledContacts").is(':checked')) {
        isDisabledContacts = true;
        if ($("#chk_DisabledContacts").is(':checked') && ($('#searchString').val() != "")) {
            isDisabledContacts = true;
            var searchcriteria = $('#DDsearchCriteria').val();
            BindCriteriaType(searchcriteria);
            SearchDetails(false);
        }
        else {
            //$('#div_chkdisable').show();
            $('#div_chkdisblecont').show();
            BindCriteriaType(0);
            SearchDetails(false);
        }
    }
    else if ($("#chk_DisabledContacts").prop("checked", false) && ($('#searchString').val() != "")) {
        //$('#div_chkdisable').show();
        $('#div_chkdisblecont').show();
        /*$('#searchString').val('');*/
        var searchcriteria = $('#DDsearchCriteria').val();
        BindCriteriaType(searchcriteria);
        SearchDetails(false);
    }

    else {
        $("#chk_Contact").prop("checked", false);
        //$('#div_chkdisable').show();
        $('#div_chkdisblecont').show();
        /*$('#searchString').val('');*/
        BindCriteriaType(1);
        SearchDetails(false);
    }
    SearchDetails(false);
}


function SearchDetails(isSort = false) {
    let disabledUsers = false, disabledContacts = false, showContacts = false;
    if ($("#chk_DisabledUsers").is(':checked')) { disabledUsers = true; }
    if ($("#chk_Contact").is(':checked')) { showContacts = true; }
    if ($("#chk_DisabledContacts").is(':checked')) { disabledContacts = true; }

    if (showAnimation) {//on filter search
        var userNameInsideVal = $('#searchString').val();
        var searchCriteriaVal = $("#DDsearchCriteria").val();
        if (searchCriteriaVal=="1")
            $('#txtUserNameOutside').val(userNameInsideVal);
    }

    if (!showAnimation) {//on keyup
        var userNameOutsideVal = $('#txtUserNameOutside').val();
        $("#searchString").val(userNameOutsideVal);
        $("#DDsearchCriteria").val("1").attr("selected", "selected");
    }
    var usercontactSearchItems = {};
    usercontactSearchItems.DisabledUsers = disabledUsers;
    usercontactSearchItems.DisabledContacts = disabledContacts;
    usercontactSearchItems.ShowContacts = showContacts;
    usercontactSearchItems.Criteria = $("#searchString").val();
    usercontactSearchItems.SearchType = $('#DDsearchCriteria option:selected').val();
    usercontactSearchItems.SearchName = $('#DDsearchCriteria option:selected').text();
    usercontactSearchItems.OrganisationCode = $("#txtsearchOrganisationCode").val();
    usercontactSearchItems.SortOrderValue = $("#SortOrderValue").val();
    usercontactSearchItems.SortTypeValue = $("#SortTypeValue").val();
    var index = $('#DDsearchCriteria option:selected').val();

    var pageSize = $('#pageSizeSelect').val();

    closeFilters();
    var params = isSort ? "?page=" + $('#pageNum').val() : "?page=1";
    $.ajax({
        url: '../Account/SetUserSearch' + params,
        type: 'POST',
        cache: false,
        //async: false,
        data: { objUserSearch: usercontactSearchItems, pageSize: pageSize },
        beforeSend: function () {
            if (showAnimation)
                startAnimation();
            else
                openContentLoader('#manage-user');
        },
        success: function (response) {
            var result = $(response).find('#banner #manage-user').html();
            $('#banner #manage-user').html(result);
        },
        error: function (xhr, textStatus, errorThrown) {
            location.reload();
        },
        complete: function () {
            isSearchStarted = false;
            if (isApiCallPending) {
                isApiCallPending = false;
                SearchDetails();
            } else {
                if (showAnimation)
                    stopAnimation();
                else
                    closeContentLoader('#sort-movement-table');
                showAnimation = true;
            }
        }
    });
}

//================================================================================
//=====
//=========================================================================


function ShowAddedUsers() {
    $('#txtsearchOrganisationCode').val('');
    $('#searchString').val('');
    $("#chk_DisabledUsers").prop("checked", false);
    $('#chk_Contact').prop("checked", false);
    $('#chk_DisabledContacts').prop("checked", false);
    BindCriteriaType(0);
    $('#div_chkcntdisble').show();
    //$('#div_chkdisable').show();
    $('#div_chkdisblecont').show();
    SearchDetails(false);
}

function ShowAddedContacts() {
    $('#txtsearchOrganisationCode').val('');
    $('#searchString').val('');
    $("#chk_DisabledContacts").prop("checked", false);
    $("#chk_DisabledUsers").prop("checked", false);
    $("#chk_Contact").prop("checked", true);
    BindCriteriaType(1);
    //$('#div_chkdisable').hide();
    $('#div_chkdisblecont').hide();
    $('#div_chkcntdisble').show();
    SearchDetails(false);
}

function Edit(id) {
    if ($('#isAdmin').val() == '1') {
        EditUserInfo(id);
    }
}

function EditContact(cntid) {
    if ($('#isAdmin').val() == '1') {
        EditContactInfo(cntid);
    }
}

function SetSearchText() {
    if ($('#hfSearchType').val() != '') {
        $('#DDsearchCriteria').val($('#hfSearchType').val());
        $('#hfSearchType').val('');
    }
    else {
        $('#DDsearchCriteria').val();
    }
    var index = $('#DDsearchCriteria option:selected').val();
    if (index == 1) {
        $('#searchString').attr('placeholder', 'User Name');
    }
    if (index == 2) {
        $('#searchString').attr('placeholder', 'First Name');
    }
    if (index == 3) {
        $('#searchString').attr('placeholder', 'Surname');
    }
    if (index == 4) {
        $('#searchString').attr('placeholder', 'Organisation Name');
    }
}

function ViewUser(userid, contactid) {

    if ($('#isAdmin').val() == '1') {
        ViewUserInfo(userid, contactid);
    }

}

function ViewUserInfo(userid, contactid) {

    startAnimation();
    if (userid != "") {

        $('#description-' + userid).load("/Users/Register?criteria=&mode=View&userID=" + userid + "&contactID=" + contactid, function (data) {
            $("#description-" + userid).show();
            stopAnimation();
            ViewUserData(userid, contactid);
        });
    }
    else {
        $('#description-' + contactid).load("/Users/Register?criteria=&mode=View&userID=" + userid + "&contactID=" + contactid, function (data) {
            $("#description-" + contactid).show();
            stopAnimation();
        });
    }
}

function EditContact(cntid) {
    if ($('#isAdmin').val() == '1') {
        EditContactInfo(cntid);
    }
}

var sortTypeGlobal = 0;//0-asc
var sortOrderGlobal = 2;//name
function SortUserList(event, param) {
    sortOrderGlobal = param;
    sortTypeGlobal = event.classList.contains('sorting_asc') ? 1 : 0;
    $('#SortTypeValue').val(sortTypeGlobal);
    $('#SortOrderValue').val(sortOrderGlobal);
    SearchDetails(isSort = true);
}

function closeDetails(id) {
    $("#description-" + id).hide();
}

function camelize(str) {
    str = str.charAt(0).toUpperCase() + str.slice(1);
    return str;
}