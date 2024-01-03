var getValue = 0;
$(document).ready(function () {
    $('.dyntitleConfig').html('Update Candidate Route Name');
    $('body').on('click', '#btnCloseGeneralPopup,#spanCloseGeneralPopup', function (e) {
        e.preventDefault();
        CloseGeneralPopup();
    });
    
    $('body').on('click', '#btnUpdateName', function (e) {
        e.preventDefault();
        var CnRouteID = $(this).data('cnrouteid');
        UpdateName(CnRouteID);
    });

    Resize_PopUp(440);

    $('body').on('change', '#ddlroutes', function (e) {
        getValue = $(this).val();

        if (getValue != 0) {
            $('#trvalidcandrt').hide();
        }
    });
    $('body').on('click', '#txtRoutename', function (e) {
        $('#trvalidcandname').css("display", "none");
    });
});
function ClosePopUp() {
    $("#dialogue").html('');
    $("#dialogue").hide();
    $("#overlay").hide();
    addscroll();
}
function UpdateName(CnRouteID) {
  
    var candidatroutename = $('#txtRoutename').val();
    if (candidatroutename == "") {
        $('#trvalidcandname').css("display", "block");
        return;
    }
    else {
        $('#trvalidcandname').css("display", "none");
        startAnimation();
        $.ajax({
            url: "../SORTApplication/UpdateCandidateRoute",
            type: 'post',
            async: true,
            data: { name: candidatroutename, RouteId: CnRouteID },
            beforeSend: function () {
                startAnimation();
            },
            success: function (data) {
                stopAnimation();
                if (data != null) {
                    ///var result = JSON.parse(data);
                    CloseGeneralPopup();
                    ShowSuccessModalPopup('Candidate route updated successfully', 'DisplayProjView');
                    //showWarningPopDialog('Candidate route updated successfully', 'Ok', '', 'DisplayProjView', '', 1, 'info');
                }
                else {
                    showWarningPopDialog('The candidate route is not updated successfully', '', 'OK', '', 'WarningCancelBtn', 1, 'error');
                }
            },
            error: function () {
                stopAnimation();
                showWarningPopDialog('The candidate route is not saved successfully', '', 'OK', '', 'WarningCancelBtn', 1, 'error');
            }
        });
    }
}
