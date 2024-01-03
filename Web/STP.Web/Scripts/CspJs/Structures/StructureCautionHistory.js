    $(document).ready(function () {

        $(".addingcausion").on('click', addingCausions);
       
        $(".backingcausion").on('click', BackToCausion);
    });
    function addingCausions(e) {
        var arg1 = e.currentTarget.dataset.arg1;

        addCausion(arg1);
    }
    function BackToCausion() {
        startAnimation();
        var randomNumber = Math.random();
        $("#generalSettingsId").load('../Structures/ReviewCautionsList?structureId=' + '@ViewBag.StructureId' + "&sectionId=" + @ViewBag.sectionId + '&random=' + randomNumber,
            function () {
                document.getElementById('manageCautionId').style.display = 'block';
                stopAnimation();
            }
        );
    };
    $('#paginator').on('click','a', function (e) {
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
            }
        });
        return false;
        }

    });
