    /**********************************************
     Developer   : Anlet
     Added on    : 16 Feb 2017
     Modified on : -
     Purpose     : For RM #6037
   **********************************************/
    $(document).ready(function () {
        var movClass = $('#MovementClassification').val();
        var movementTypeId = $('#movementTypeId').val();

        if (movClass == 241002 || movementTypeId == 270006) {
            $('#div_tyre_Spce_msg').show();
        }
        else {
            $('#div_tyre_Spce_msg').hide();
        }

        $(".btn_CopyAll").on('click', AxleCopyAll);
        $(".btn_CopyFromPrev").on('click', AxleCopyPrevious);
    });
    /**********************************************/


    function AxleCopyAll(e) {
        AxleCopyToAll(e);
    }
    function AxleCopyPrevious(e) {
        var arg1 = e.currentTarget.dataset.arg1;
        AxleCopyFromPrev(e, arg1);
    }
