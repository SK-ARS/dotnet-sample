    $(document).ready(function () {
        $("#close_contact_popup").on('click', closeContactPopup);
        $("#dialogue").show();
        $("#popupDialogue").show();
        stopAnimation();
        $("#overlay").show();
        $('body').on('click', '#spnclosemap', closeMp);

    });
    
    function closeMp() {
        $('#contactDetails').modal('hide');
        $("#overlay").hide();
    }    
