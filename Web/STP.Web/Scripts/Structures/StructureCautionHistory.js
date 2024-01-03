var StructureIdVal = $('#hf_StructureId').val();
var sectionIdVal = $('#hf_sectionId').val();
function StructureCautionHistoryInit() {
    StructureIdVal = $('#StructureCautionHistoryContainer #hf_StructureId').val();
    sectionIdVal = $('#StructureCautionHistoryContainer #hf_sectionId').val();
}
$(document).ready(function () {

    $('body').on('click', '#StructureCautionHistoryContainer .addingcausion', function (e) {
        e.preventDefault();
        addingCausions(this);
    });

    $('body').on('click', '#StructureCautionHistoryContainer .backingcausion', function (e) {
        e.preventDefault();
        BackToCausion(this);
    });

    $('body').on('click', '#StructureCautionHistoryContainer #paginator a', function (e) {
        e.preventDefault();
        if (this.href == '') {
            return false;
        }
        else {
            $.ajax({
                url: this.href,
                type: 'GET',
                cache: false,
                success: function (result) {
                    $('#generalSettingsId').html(result);
                },
                error(err) {
                    console.error(err);
                }
            });
            return false;
        }

    });
});
function addingCausions(e) {
    var arg1 = $(e).attr("arg1");
    addCausion(arg1);
}
function BackToCausion() {
    startAnimation();
    var randomNumber = Math.random();
    $("#generalSettingsId").load('../Structures/ReviewCautionsList?structureId=' + StructureIdVal + "&sectionId=" + sectionIdVal + '&random=' + randomNumber,
        function () {
            $('#manageCautionId').css('display','block');
            stopAnimation();
            ReviewCautionsListInit();
        }
    );
};

