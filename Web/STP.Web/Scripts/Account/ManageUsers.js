var VariableObject = $('#VariableObject').val();
var ObjectValue = JSON.parse(VariableObject);
$(document).ready(function () {
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
                /*url: '@Url.Action("SearchUserCriteria", "Account")',*/
                url: "../ Account/SearchUserCriteria",
                type: 'POST',
                cache: false,
                async: false,
                data: { objUserSearch: usercontactSearchItems },
                success: function (data) {
                    ;
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
            $("#searchString").val(ui.item.label);
            return false;
        }
    });
    //$( "#automplete-6" ).autocomplete("option", "position",
    //          {my : "right-10 top+10", at: "right top" })
    //    });
});

function ShowAddedUsers() {
    $('#txtsearchOrganisationCode').val('');
    $('#searchString').val('');
    $("#chk_DisabledUsers").prop("checked", false);
    $('#chk_Contact').prop("checked", false);
    $('#chk_DisabledContacts').prop("checked", false);
    BindCriteriaType(0);
    $('#div_chkcntdisble').show();
    $('#div_chkdisable').show();
    $('#div_chkdisblecont').show();
    SearchUserContactDetails(false, false, false);
}

function ShowAddedContacts() {
    $('#txtsearchOrganisationCode').val('');
    $('#searchString').val('');
    $("#chk_DisabledContacts").prop("checked", false);
    $("#chk_DisabledUsers").prop("checked", false);
    $("#chk_Contact").prop("checked", true);
    BindCriteriaType(1);
    $('#div_chkdisable').hide();
    $('#div_chkdisblecont').hide();
    $('#div_chkcntdisble').show();
    SearchUserContactDetails(false, false, true);
}

function Edit(id) {
    if (ObjectValue.isAdmin == 1) {
        EditUserInfo(id);
    }
}

function EditContact(cntid) {
    if (ObjectValue.isAdmin == 1) {
        EditContactInfo(cntid);
    }
}

function SetSearchText() {
    if (objUserSearch.SearchType != '') {
        $('#DDsearchCriteria').val(objUserSearch.SearchType);
    }
    else {
        $('#DDsearchCriteria').val(1);
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

    if (ObjectValue.isAdmin == 1) {
        ViewUserInfo(userid, contactid);
    }

}

function ViewUserInfo(userid, contactid) {
    startAnimation();
    if (userid != "") {
        $('#description-' + userid).load("/Users/Register/?criteria=&mode=View&userID=" + userid + "&contactID=" + contactid, function (data) {
            $("#description-" + userid).show();
            stopAnimation();
        });
    }
    else {
        $('#description-' + contactid).load("/Users/Register/?criteria=&mode=View&userID=" + userid + "&contactID=" + contactid, function (data) {
            $("#description-" + contactid).show();
            stopAnimation();
        });
    }
}

function EditContact(cntid) {
    if (ObjectValue.isAdmin == 1) {
        EditContactInfo(cntid);
    }
}



