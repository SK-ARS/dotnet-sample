
        $(document).ready(function () {

            if (($('#PortalType').val() == 696008) || ($('#PortalType').val() == 696002) || ($('#PortalType').val() == 696007)) {
                SelectMenu(5);
            }
            else if ($('#PortalType').val() == 696001) {
                SelectMenu(7);
            }
            else {
                SelectMenu(9);
            }

        $('#Description').val('');
        $('#COMPLAINT').attr('checked', true);
        $('#Description').keyup(function () {
            var FeedBack = $("#Description").val().trim();
            if ((FeedBack != null || FeedBack != '')&& (FeedBack.length < 15 || FeedBack.length > 4000))
            $("#DescriptionValidationMsg").removeClass("showValidationMsg");
            $("#DescriptionValidationMsg").addClass("hideValidationMsg");
            
        });
            $("#btn-sndfeedbk").click(SendFeedback);
      });
    function SendFeedback() {
        var FeedBackType = "";
        var FeedBack = $("#Description").val().trim();
        var FeedBackName = '';

       
        if (FeedBack == null || FeedBack == '') {
            $("#DescriptionValidationMsg").removeClass("hideValidationMsg");
            $("#DescriptionValidationMsg").addClass("showValidationMsg");
            $("#DescriptionValidationMsg").text("Description is required!");
            $("#DescriptionValidationMsg").css("color","red");
        }
        else if (FeedBack.length < 15 || FeedBack.length > 4000) {
            $("#DescriptionValidationMsg").removeClass("hideValidationMsg");
            $("#DescriptionValidationMsg").addClass("showValidationMsg");
            $("#DescriptionValidationMsg").text("Minimum 15, maximum 4000 characters");
            $("#DescriptionValidationMsg").css("color", "red");
        }
        else {

            if ($('#COMPLAINT').is(':checked')) {
                FeedBackType = $('#COMPLAINT').val();
                FeedBackName = 'Complaint';
            }
            else if ($('#SUGGESTION').is(':checked')) {
                FeedBackType = $('#SUGGESTION').val();
                FeedBackName = 'Suggestion';
            }
            else if ($('#GENERAL_REQUEST').is(':checked')) {
                FeedBackType = $('#GENERAL_REQUEST').val();
                FeedBackName = 'General Request';
            }
            else if ($('#SYSTEM_FAULT').is(':checked')) {
                FeedBackType = $('#SYSTEM_FAULT').val();
                FeedBackName = 'System Fault';
            }

            var currentUser = "@loggedInUser";

            var params = '{"FeedBackType":"' + FeedBackType + '","FeedBack":"' + FeedBack + '","UserId":"' + currentUser + '"}';
            $.ajax({
                async: false,
                type: "POST",
                url: '@Url.Action("InsertFeedbackDetails", "Home")',
                dataType: "json",
                //contentType: "application/json; charset=utf-8",
                data: params,
                beforeSend: function () {
                    startAnimation();
                },
                processdata: true,
                success: function (result) {
                   
                    if (result == true) {

                        FeedBackName = FeedBackName.toLowerCase();
                        FeedBackName = FeedBackName.substring(0, 1).toUpperCase() + FeedBackName.substring(1, FeedBackName.length);
                        var Msg = '"' + FeedBackName + '"  submitted successfully.';
                        ShowSuccessModalPopup(Msg, "ReloadLocation");

                    }
                },
                error: function (result) {

                },
                complete: function () {
                   stopAnimation();
                }
            });
        }
}



