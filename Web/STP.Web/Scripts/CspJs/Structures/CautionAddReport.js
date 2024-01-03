        $(document).ready(function () {
            
            $("#SaveCaution").on('click', SaveCautionsFromReport );
            $(".backbtn").on('click', Back);
            
            $(".Reviewbtn").on('click', ReviewCaution );
        });
    function Back() {
        $('#causionReport').empty();
        $('#generalSettingsId').show();
    }
    function failed() {
        document.getElementById('generalSettingsId').style.display = "block"
        document.getElementById('check-caution').style.display = "none"
    }
    function closecaution() {
        document.getElementById('check-caution').style.display = "none"
        document.getElementById('add-caution').style.display = "block"
    }
    function SaveCautionsFromReport() {
        $.ajax({
            url: '../Structures/SaveCautions',
            dataType: 'json',
            type: 'POST',
            data: $("#CautionInfo").serialize(),
            beforeSend: function () {
                startAnimation();
            },
            success: function (result) {
                if (result) {
if($('#hf_mode').val() ==  "add") {
                        stopAnimation();
                        ShowSuccessModalPopup("Caution added successfully", "ReviewCaution")
                    }
                    else {
                        stopAnimation();
                        ShowSuccessModalPopup("Caution updated successfully","ReviewCaution")
                    }
                }
                else {
                    redirectpage();
                }
            },
            error: function (xhr, status) {
            }
        });
    }
        function ReviewCaution() {
            CloseSuccessModalPopup();
        $('#cautionPopup').modal('hide');
        startAnimation();
        var randomNumber = Math.random();
        $("#generalSettingsId").load('../Structures/ReviewCautionsList?structureId=' + '@ViewBag.StructureID' + "&sectionId=" + @ViewBag.SectionID + '&random=' + randomNumber,
            function () {
                stopAnimation();
                $('#causionReport').empty();
                $('#generalSettingsId').show();
                $('#manageCautionId').show();

            }
        );
    };
