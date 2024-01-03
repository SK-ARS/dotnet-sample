    var getValue = 0;
    $(document).ready(function () {

        $('.dyntitleConfig').html('Update Candidate Route Name');
        $("#btnCloseGeneralPopup").on('click', CloseGeneralPopup);
        $("#spanCloseGeneralPopup").on('click', CloseGeneralPopup);

        $('body').on('click', '#btnUpdateName', function (e) {
            e.preventDefault();
            var CnRouteID = $(this).data('CnRouteID');
            UpdateName(CnRouteId);
        });

        Resize_PopUp(440);

        $("#ddlroutes").change(function () {
            getValue = $(this).val();

            if (getValue != 0) {
                $('#trvalidcandrt').hide();
            }
        });
        $('#txtRoutename').click(function () {
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
                        ShowInfoPopup('Candidate route updated successfully', 'DisplayProjView');
                        //showWarningPopDialog('Candidate route updated successfully', 'Ok', '', 'DisplayProjView', '', 1, 'info');
                    }
                    else {
                        showWarningPopDialog('The candidate route is not updated successfully.', '', 'OK', '', 'WarningCancelBtn', 1, 'error');
                    }
                },
                error: function () {
                    stopAnimation();
                    showWarningPopDialog('The candidate route is not saved successfully.', '', 'OK', '', 'WarningCancelBtn', 1, 'error');
                }
            });
        }
    }
