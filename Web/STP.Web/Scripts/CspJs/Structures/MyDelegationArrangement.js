    $(document).ready(function () {
        $(".editdelegation").on('click', EditDelegationtDetails);
        $(".linkstructurecodeclick").on('click', linkStructureCodeClicking);
        $("#closefilterstruct").on('click', closeFilters );
        $(".deleteconfirmation").on('click', DeletingConfirmation);
    });

    function EditDelegationtDetails(e) {
        var param1 = e.currentTarget.dataset.arg1;
        var param2 = e.currentTarget.dataset.arg2;
        var param3 = e.currentTarget.dataset.arg3;
        var param4 = e.currentTarget.dataset.arg4;
        EditDelegation(param1,param2,param3,param4);
    }
    function linkStructureCodeClicking(e) {
        var param1 = e.currentTarget.dataset.arg1;
        var param2 = e.currentTarget.dataset.arg2;
        var param3 = e.currentTarget.dataset.arg3;
        linkStructureCodeClick(param1, param2, param3);
    }
    function DeletingConfirmation(e) {
        var param1 = e.currentTarget.dataset.arg1;
        var param2 = e.currentTarget.dataset.arg2;
        DeleteConfirmation(e,param1, param2);
    }
