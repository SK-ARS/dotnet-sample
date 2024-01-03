    $(document).ready(function () {
        
        $("#btnfiltersuccessdata").on('click', FilterSuccessdata);
        $(".clrsortdata").on('click', closeFilters);
        $("#aclosefilter").on('click',closeFilters);
        $("#Mapfilterdiv").on('click', openFilters);
        $(".sortwithdrawn").on('click', SortWithdrawn1);
        $(".withdrawsoapplcn").on('click', WithdrawSoApplications);
        $("#showcandidatevehicle").on('click', Show_CandidateRTVehicles);
        $("#openFilter").on('click', openFilters);
        $("#filterimage").on('click', ClearSORTData);
        $(".selectmovement").on('click', SelectPrevitMovementsVehicles);
        $(".slctcrntmvmt").on('click', SelectCurrentMovementsRoutes);
        $(".selectprevmovementvr1").on('click', SelectPrevitMovementsVehiclevr1);
        $(".selectcurrentmovementvr1").on('click', SelectCurrentMovementsRoutevr1);
    });

    function ClearSORTDatas(e) {
        
        ClearSORTData(0);
    }
    function SortWithdrawn1(e) {
        var param1 = e.currentTarget.dataset.arg1;
        var param2 = e.currentTarget.dataset.arg2;
        var param3 = e.currentTarget.dataset.arg3;
        SortWithdraw1(param1, param2, param3);
    }

    function WithdrawSoApplications(e) {
        var param1 = e.currentTarget.dataset.arg1;
        var param2 = e.currentTarget.dataset.arg2;
        var param3 = e.currentTarget.dataset.arg3;
        var param4 = e.currentTarget.dataset.arg4;
        var param5 = e.currentTarget.dataset.arg5;
        WithdrawSoApplication(param1, param2, param3, param4, param5,2);
    }

    function SelectPrevitMovementsVehicles(e) {
        var param1 = e.currentTarget.dataset.arg1;
        var param2 = e.currentTarget.dataset.arg2;
        var param3 = e.currentTarget.dataset.arg3;
        var param4 = e.currentTarget.dataset.arg4;
        var param5 = e.currentTarget.dataset.arg5;
        SelectPrevitMovementsVehicle(0,param1, param2,param3, param4, param5);
    }
    function SelectCurrentMovementsRoutes(e) {
        var param1 = e.currentTarget.dataset.arg1;
        var param2 = e.currentTarget.dataset.arg2;
        var param3 = e.currentTarget.dataset.arg3;
        var param4 = e.currentTarget.dataset.arg4;
        var param5 = e.currentTarget.dataset.arg5;
        SelectCurrentMovementsRoute(0, param1, param2, param3, param4, param5);
    }
    function SelectPrevitMovementsVehiclevr1(e) {
        var param1 = e.currentTarget.dataset.arg1;
        var param2 = e.currentTarget.dataset.arg2;
        var param3 = e.currentTarget.dataset.arg3;
        var param4 = e.currentTarget.dataset.arg4;
        var param5 = e.currentTarget.dataset.arg5;
        SelectPrevitMovementsVehicle(0, param1, param2, param3, param4, param5);
    }
    function SelectCurrentMovementsRoutevr1(e) {
        var param1 = e.currentTarget.dataset.arg1;
        var param2 = e.currentTarget.dataset.arg2;
        var param3 = e.currentTarget.dataset.arg3;
        var param4 = e.currentTarget.dataset.arg4;
        var param5 = e.currentTarget.dataset.arg5;
        SelectCurrentMovementsRoute(0, param1, param2, param3, param4, param5);
    }
