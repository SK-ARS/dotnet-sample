$('#hdnGranter_ID').val($('#GranterID').val());
var hfGrantorName;
var Grant_ID ;
var analysisId ;
var notifid ;
var showRestriction;
var Granter_IDVal = $('#hf_Granter_ID').val();
var GranterNameVal = $('#hf_GranterName').val();
function NotificationDispensationPopUpInit(Grantor_id, Grantor_name) {
    hfGrantorName = $('#hf_grantor_name').val();
    Grant_ID = Grantor_id;
    analysisId = $('#analysisId').val() ? $('#analysisId').val() : 0;
    notifid = $('#notifid').val() ? $('#notifid').val() : 0;
    showRestriction = $('#hf_showRestriction').val();
    $('#confirm_btn').hide();
    if (showRestriction == 'False') {
        //if ($('#hf_hideLayout').val() == 'True') {
        //    CreateDis(0);
        //} else {
        //    $('#dispensation-body').load('../Dispensation/PartialView/_ListDispensation?Org_Id=' + Grant_ID + '&hideLayout=' + true + '&analysisId=' + analysisId);
        //    $('.dyntitle').html('List dispensation');
        //}

        $.ajax(
            {
                url: '../Dispensation/ListDispensation',
                type: 'POST',
                //async: false,
                data: { Org_Id: Grant_ID, hideLayout: true, analysisId: analysisId },
                beforeSend: function () {
                    startAnimation();
                },
                success: function (data) {
                    var result = $(data).find('table');
                    $("#dispensationBody").html(data);
                    $('<div id="btncreatedispensation" class="button mr-0"><button class= "btn-outline-primary SOAButtonHelper mb-3 btn-border-remove CreateDis" role = "button" aria - pressed="true" style="background-color: white;" >Create Dispensation</button></div>').insertBefore("#dispensationBody");
                    
                },
                error: function (xhr, textStatus, errorThrown) {
                    stopAnimation();
                },
                complete: function () {
                    stopAnimation();
                }
            });
    } else {
        $('#editRest').show();
        //$('.dyntitle').html('Edit restrictions');
        //$('#popUp').hide();
        //Resize_PopUp(416);
    }
}

$(document).ready(function () {

   
    $('body').on('click', '.CreateDis', function (e) {
      
        CreateDis();
    });

    //$('#span-close').click(function () {

    //    var callFromAfftd = $('#CallFromAfftdDisp').val();

    //    if (callFromAfftd == 'True') {
    //        var Grant_ID = $('#hdnGranter_ID').val();
    //        var grantorName = $('#hdnGrantor_Org').val();
    //        $('#CallFromAfftdDisp').val('False');   //setting as true to default status
    //        ViewDispensationAffParties(Grant_ID, grantorName);
    //    }
    //    else {
    //        $("#dialogue").hide();
    //        $("#overlay").hide();
    //        resetdialogue();
    //        addscroll();
    //    }
    //});
});

function CreateDis(f_name) {
   
    //$('#back_btn').hide();
    if (f_name == "" || f_name == null) {
        f_name = 0;
    }

    $('#CallFromAfftdDisp').val('True');

    Grant_ID = $('#GranterID').val();
    var Grantor_name = $('#hdnGrantor_Org').val() ? $('#hdnGrantor_Org').val() : hfGrantorName.replace(/ /g, '%20');
    $('#dispensationBody').hide();
    $.ajax(
        {
            url: '../Dispensation/CreateDispensation',
            type: 'GET',
            //async: false,
            data: $("#DispensationInfo").serialize() + "&mode=create" + "&fromAffectedParties=" +true + "&Grantor_name=" + Grantor_name + "&NotifID=" + notifid + "&Granter_ID=" + Grant_ID + "&hideLayout="+true ,
            
            beforeSend: function () {
                startAnimation();
            },
            success: function (data) {

                $('#movement_type_confirmation').html('');
                $('#route-assessment').hide();
                var result = $(data).find('#divCreateDispensation');
                $("#General").html(result);
                //var result = $(data).find('#div_dispensation');
                //$("#General").html(result);
                $("#General").show();
                $('#divCreateDispensation').unwrap('#banner-container');
                $("#btncreatedispensation").hide();
                $("#btn_cancel").hide();
                $("#btn_cancel").show();
                $("#back_btn").hide();
                $("#refno").val($("#hdnDRN").val());
                $("#summary").val($("#hdnsummary").val());
                $("#descp").val($("#hdndescp").val());
                if ($("#FromDate").text() == '') {
                    $("#FromDate").text($("#hdnfrmdate").val());
                }
                if ($("#ToDate").val() == '') {
                    $("#ToDate").val($("#hdnToDate").val());
                }
                if ($("#GrantedBy").text() == '') {
                    $("#GrantedBy").val($('#hdnGrantor_Org').val());
                    $("#GrantedBy").attr('readonly', true);
                }
                SelectMenu(2);

                $('#gross_spn').val($("#hdngross").val());
                $('#axle_spn').val($("#hdnaxle").val());
                $('#width_spn').val($("#hdnlength").val());
                $('#length_spn').val($("#hdnwidth").val());
                $('#height_spn').val($("#hdnheight").val());

                if ($('#gross_text').val() == "" && $('#axle_text').val() == "" && $('#length_text').val() == "" && $('#width_text').val() == "" && $('#height_text').val() == "") {
                    //console.log('if');
                    $('#clear').show();
                    $('#gross_clr').hide();
                    $('#axle_clr').hide();
                    $('#length_clr').hide();
                    $('#width_clr').hide();
                    $('#height_clr').hide();
                } else {

                    $('#gross_clr').hide();
                    $('#axle_clr').hide();
                    $('#length_clr').hide();
                    $('#width_clr').hide();
                    $('#height_clr').hide();
                    if (f_name != 1) {
                        if ($('#gross_text').val() != "") {
                            $('#gross_spn').html($("#hdngross").val());
                            $('#hdngross').val($("#gross_spn").text());
                            $('#gross_clr').show();
                        }
                        if ($('#axle_text').val() != "") {
                            $('#axle_spn').html($("#hdnaxle").val());
                            $('#hdnaxle').html($("#axle_spn").text());
                            $('#axle_clr').show();
                        }
                        if ($('#length_text').val() != "") {
                            $('#length_spn').html($("#hdnlength").val());
                            $('#hdnlength').html($("#length_spn").text());
                            $('#length_clr').show();
                        }
                        if ($('#width_text').val() != "") {
                            $('#width_spn').html($("#hdnwidth").val());
                            $('#hdnwidth').html($("#width_spn").text());
                            $('#width_clr').show();
                        }
                        if ($('#height_text').val() != "") {
                            $('#height_spn').html($("#hdnheight").val());
                            $('#hdnheight').html($("#height_spn").text());
                            $('#height_clr').show();
                        }
                        $('#clear').hide();
                    }
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                stopAnimation();
            },
            complete: function () {
                stopAnimation();
            }
        });
    //$('#dispensationBody').load('../Dispensation/CreateDispensation?hideContent=' + true + '&Granter_id=' + Grant_ID + '&Grantor_name=' + Grantor_name + '&NotifID=' + notifid, function () {


    //});
}


function ViewDispensationAffParties(Grantor_id, Grantor_name) {
    
    $('#hdnGrantor_Org').val(Grantor_name);
    $("#hdnDRN").val('');
    $("#hdnsummary").val('');
    $("#hdndescp").val('');
    $("#hdnfrmdate").val('');
    $("#hdnToDate").val('');
    $("#hdngrantby").val('');

    $("#hdngross").val('');
    $("#hdnaxle").val('');
    $("#hdnlength").val('');
    $("#hdnwidth").val('');
    $("#hdnheight").val('');

    var analysisId = $('#AnalId').val() ? $('#AnalId').val() : 0;
    var notifid = $('#Notificationid').val() ? $('#Notificationid').val() : 0;

    if (notifid == 0) {
        notifid = $('#hf_NotificationID').val();
    }

    $.ajax(
        {
            url: '../Notification/NotificationDispensationPopUp',
            type: 'POST',
            //async: false,
            data: { organisationId: Grantor_id, analysisId: analysisId, grantorName: Grantor_name.replace(/ /g, '%20'), notifid: notifid },
            beforeSend: function () {
                startAnimation();
            },
            success: function (data) {
                SubStepFlag = 5.1;
                $('#div_dispensation').html(data);
                $('#div_address_parties').hide();
                $('#div_dispensation').find('div#filterDivDispensation').remove();
                var filters = $(data).find('div#filterDivDispensation');
                $(filters).insertAfter('#banner');
                NotificationDispensationPopUpInit(Grantor_id, Grantor_name);
            },
            error: function (xhr, textStatus, errorThrown) {
                stopAnimation();
            },
            complete: function () {
                stopAnimation();
            }
        });

    //$("#overlay").show();
    //$('.loading').show();
    //$("#dialogue").html('');


    //$("#dialogue").load("../Notification/NotificationDispensationPopUp?Org_Id=" + Grantor_id + "&hideLayout=" + true + "&analysisId=" + analysisId + "&Grantor_name=" + Grantor_name.replace(/ /g, '%20') + '&notifid=' + notifid, function (responseText, textStatus, req) {//+'&page='+1+'&pageSize='+10

    //    $('.loading').hide();
    //    $('#span-help').hide();
    //    if (textStatus == "error") {
    //        location.reload();
    //    }
    //});
    //removescroll();
    //resetdialogue();
    //$("#overlay").show();
    //$("#dialogue").show();
}
$('body').on('click', '#select', function (e) {
    e.preventDefault();
    var dispId = $(this).data('disid');
    var dispRefNo = $(this).data('disref');
    var grantorName = $(this).data('grantedby');
    var summary = $(this).data('summary');
    Select(dispId, dispRefNo, grantorName, summary);
});

function Select(dispId, dispRefNo, grantorName, summary) {
    var dispId = dispId;
    var grantorId = $('#GranterID').val();
    if (notifid == 0) { notifid = $('#Notificationid').val(); }
    if (analysisId == 0) { analysisId = $('#NotifAnalysisId').val(); }
    $.ajax({

        url: '../Notification/AddDispensationDetail',
        type: 'POST',
        async: false,
        data: { dispensationId: dispId, analysisId: analysisId, notificationId: notifid, grantorId: grantorId, grantorName: grantorName, dispensationRefNo: dispRefNo, dispSummary: summary },
        beforeSend: function () {
            startAnimation();
        },
        success: function (data) {
            if (data.result == 1) {
                BacktoAffectedParties();
                //loadAffectedParty(data.notifId, data.analysisId);
            }
            else if (data.result == 2) {
                ShowErrorPopup("Dispensation already in use", "CloseDispensationErrorPopUp");
            }
            else {
                ShowErrorPopup("Dispensation not added", "CloseDispensationErrorPopUp");
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            //other stuff
            location.reload();
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function CloseDispensationErrorPopUp() {
    $("#overlay").css('display', 'block');
    $('#ErrorPopupWithAction').modal('hide');
    BacktoAffectedParties();
    //loadAffectedParty(notifid, analysisId);
}

//#region
function loadAffectedParty(notifid, analysisId) {

    if (notifid == 0 || analysisId == 0) {
        var analysisId = $('#AnalId').val();
        var notifId = $('#Notificationid').val();
    }
    $.ajax({
        url: '../Notification/ManualAddedParties',
        type: 'POST',
        async: false,
        data: { NotifID: notifId, analysisId: analysisId },
        beforeSend: function () {
        },

        success: function (data) {
            $('#btncreatedispensation').hide();
            $('#dispensationBody').html('');
            $('#div_address_parties').show();
            $('#ManuallyAddedContact').html('');
            $('#notif_section').html('');
            $('#ManuallyAddedContact').html(data);
            addscroll();
        },
        error: function (xhr, textStatus, errorThrown) {
            location.reload();
        },
        complete: function () {
            stopAnimation();
        }

    });
}
//#endregion
function BacktoAffectedParties() {
    startAnimation();
    $('#leftpanel').show();
    $("#General").hide();
    ListAffectedParty('RBAffectedParty', 'affectedParties');
    stopAnimation();
}

