    var closeFlag = 0;
    $(document).ready(function () {
        $("#btn_update").on('click', Updateval);
if($('#hf_notifFla').val() == 'True'){
            selectedmenu('Dispensations');
        }

        //back to view dispensation
        $('#btn_bck').click(function (e) {
            location.href = '@Url.Content("~/Dispensation/CreateDispensation/")';
        });



        $("#gross_text,#axle_text,#length_text,#width_text,#height_text").live("keypress", function (event) {
            if (event.which == 13) {
                Updateval();
            }
        });

    });

    function CloseDialog() {
if($('#hf_notifFlag').val() ==  'True') {
            showWarningPopDialog('Closing the window may end up in losing data. Continue?', 'Cancel', 'Ok', 'WarningCancelBtn', 'Close', 1, 'warning');
        } else {
            Resize_PopUp(900);
            $('.dyntitle').html('Create dispensation');

            closeFlag = 1;
            CreateDis(closeFlag);
            $('#clear').show();
            $('#gross_clr').hide();
            $('#axle_clr').hide();
            $('#length_clr').hide();
            $('#width_clr').hide();
            $('#height_clr').hide();
            $('#editRest').hide();
            $('#popUp').show();
        }
    }

    function Close() {
        $('#overlay').hide();
        addscroll();
        WarningCancelBtn();
    }

    function closeModal() {

        var pageSize = $('#pageSizeVal').val();
        var pageNum = $('#pageNum').val();

        if (!view) {
            if (confirm("Closing the window may end up in losing data. Continue?", 'Confirm')) {
                $('#dialogue').html('');
                $("#dialogue").hide();
                $("#overlay").hide();
                location.reload();
            }
        }
        else {
            $('#dialogue').html('');
            $("#dialogue").hide();
            $("#overlay").hide();
            location.reload();
        }
    }


