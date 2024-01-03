
$(document).ready(function () {
    $('body').on('click', '#btn_ClearFilter', function (e) { ClearFilter(); });
    function ClearFilter() {
        $('#SearchSummaryName').val('');
        $('#AlternateName').val('');
        $('#SearchSummaryId').val('');
        $('#Description').val('');
        $('#Carries').val('');
        $('#Crosses').val('');
        $('#DelegateName').val('');
        $("#StructureType").prop('selectedIndex', 0);
        $("#ICAMethod").prop('selectedIndex', 0);
    }
    $('body').on('click', '#clsbtn', function (e) { closeFilters(); });
    $('body').on('click', '#btnStructSearch', function (e) { SoastructureSearch(); });
});
