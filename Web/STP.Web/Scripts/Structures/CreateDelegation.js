$(document).ready(function () {
    SelectMenu(3);
    $("#organName").autocomplete(
        {
            source: function (request, response) {
                $.ajax({
                    url: '../Structures/OrganisationList',
                    dataType: "json",
                    data: {
                        SearchString: request.term
                    },
                    success: function (data) {
                        response($.map(data, function (item) {

                            var OrganisationName = item.OrganisationCode != '' ? item.OrganisationName + " - " + item.OrganisationCode : item.OrganisationName;
                            return { label: OrganisationName, value: item.OrganisationId, data: item.OrganisationName };

                        }));
                    },
                    error: function (jqXHR, exception, errorThrown) {
                        alert("error");
                        console.log(errorThrown);
                    }
                });
            },
            minLength: 0,
            select: function (event, ui) {
                // Set selection
                ui.item.label = ui.item.data;
                $('#organName').val(ui.item.label); // display the selected text
                var p = $('#organName').val;

                $('#OrganisationId').val(ui.item.value); // save selected id to input
                if ($('#organName').val() == '') {

                    $('#validorganisation').show();

                    return false;
                }
                else {

                    $('#validorganisation').hide();
                }
                return false;
            },
            focus: function (event, ui) {
                //$("#organName").val(ui.item.label);
                return false;
            }
        });

    $("#contactName").autocomplete(
        {
            source: function (request, response) {
                $.ajax({
                    url: '../Structures/ContactList?orgID=' + $("#OrganisationId").val(),
                    dataType: "json",
                    data:
                    {
                        SearchString: request.term
                    },
                    success: function (data) {
                        if (request.term == null || request.term == '') {
                            data = [];
                        }
                        response($.map(data, function (item) {
                            return { label: item.ContactName, value: item.ContactId };
                        }));
                    },
                    error: function (jqXHR, exception, errorThrown) {
                        alert(errorThrown);
                        console.log(errorThrown);
                    }
                });
            },
            minLength: 0,
            select: function (event, ui) {
                // Set selection
                $('#contactName').val(ui.item.label); // display the selected text
                var c = $('#organName').val;
                $('#ContactId').val(ui.item.value); // save selected id to input
                return false;
            },
            focus: function (event, ui) {

                //$("#contactName").val(ui.item.label);
                return false;
            }
        });

    $('#BackDeleg').click(function () {

        window.location.href = '../Structures/MyDelegationArrangement';
    });
    $("#arrangName").change(function () {
        var arrangname = $("#arrangName").val().trim();
        if (arrangname == '') {
            $("#arrnameReqValidate").show();
        }
        else {
            $("#arrnameReqValidate").hide();
        }
    });
});

$('#addStructure').click(function () {

    $('.loading').show();
    if ($('#organName').val() == '') {

        $('#validorganisation').show();

        return false;
    }
    else {

        $('#validorganisation').hide();
    }
    var SubDelegation = $('#subdel').is(':checked');
    var CopyNotification = $('#copynotification').is(':checked');
    var paramList = {
        ArrangementName: $('#arrangName').val(),
        OrganisationName: $('#organName').val(),
        OrganisationId: $('#OrganisationId').val(),
        ContactName: $('#contactName').val(),
        ContactId: $('#ContactId').val(),
        SubDelegation: $('#subdel').is(':checked'),
        CopyNotification: $('#copynotification').is(':checked'),
        ContactType: $('#ContactType').val(),
        SelectedTypeName: $("input[name$=SelectedType]:checked").val()
    }
    $.ajax({
        async: false,
        type: "POST",
        url: '../Structures/StoreDelegationData',
        dataType: "json",
        data: JSON.stringify(paramList),
        processdata: true,
        success: function (result) {
            if (result) {
                paramList = null;
                window.location.href = '../Structures/StructureList' + EncodedQueryString('Mode=Edit&orgid=' + $("#OrganisationId").val() + '&arrId=' + $("#ArrangementId").val() + '&EditOrgId=' + $('#EditOrganisationId').val());
            }
            else {
                alert('Error');
            }
        },
        error: function () {
        },
        complete: function () {
            $('.loading').hide();
        }
    });
});

$('#saveDeleg').click(function () {

    var response = Validation();

    if (!response) {
        return false;
    }
    //Contact name
    var SubDelegation = $('#subdel').is(':checked');
    var CopyNotification = $('#copynotification').is(':checked');
    var ArrangementId = $("#ArrangementId").val();

    var paramList = {

        ArrangementId: ArrangementId,
        ArrangementName: $('#arrangName').val(),
        OrganisationName: $('#organName').val(),
        OrganisationId: $('#OrganisationId').val(),
        ContactName: $('#contactName').val(),
        ContactId: $('#contactName').val() != '' ? $('#ContactId').val():0,
        SubDelegation: $('#subdel').is(':checked'),
        CopyNotification: $('#copynotification').is(':checked'),
        ContactType: $('#ContactType').val(),
        SelectedTypeName: $("input[name$=SelectedType]:checked").val()
    };
    $.ajax({
        type: "POST",
        url: '../Structures/SaveDelegation',
        dataType: "json",
        data: JSON.stringify(paramList),
        beforeSend: function () {
            startAnimation();
        },
        processdata: true,
        success: function (result) {

            stopAnimation();
            paramList = null;
            if (result == 0) {
                if (ArrangementId == 0) {
                    ShowSuccessModalPopup("Delegation saved sucessfully", "SucessMyDelegationArrangement")
                }
                else {
                    ShowSuccessModalPopup("Delegation updated sucessfully", "SucessMyDelegationArrangement")
                }
            }

            else if (result == 1) {
                ShowErrorPopup("Delegation saving failed", "FailerMyDelegationArrangement");
            }
            else if (result == 2) {
                ShowErrorPopup("Delegation saving failed", "FailerMyDelegationArrangement");
            }
            else if (result == 3) {
                window.location.href = '../Account/Login';
            }
            else {
                ShowErrorPopup("Delegation saving failed", "FailerMyDelegationArrangement");
            }
        },
        error: function () {
            ShowErrorPopup("Delegation saving failed", "FailerMyDelegationArrangement");
        },
        complete: function () {

        }
    });
});

function Validation() {

    var flgReturn = true;
    if ($('#arrangName').val().length == 0) {
        $("#arrnameReqValidate").show();
        var flgReturn = false;
    }
    else {
        $('#validarranName').hide();
    }

    if ($('#organName').val() == '') {

        $('#validorganisation').show();

        var flgReturn = false;
    }
    else {

        $('#validorganisation').hide();
    }

    if ($('#ContactType').val() != 'default' && ($('#contactName').val() == '' || $('#ContactId').val() == '0')) {
        $('#validContacttype').show();
        flgReturn = false;
    }
    else {
        $('#validContacttype').hide();
    }

    if ($("#AcceptFailure").val() == 'Yes') {
        $("#acceptfailure").val(1);
    }
    else {
        $("#acceptfailure").val(0);
    }
    $("#SelectedTypeName").val($("input[name=SelectedType]:checked").val());
    return flgReturn;
}
function SucessMyDelegationArrangement() {
    window.location.href = '../Structures/MyDelegationArrangement';

}

function FailerMyDelegationArrangement() {
    window.location.href = '../Structures/CreateDelegation' + EncodedQueryString('Mode=Edit');
}

