        $(document).ready(function () {
            $("#table-head").on('click', ViewTermsAndConditions);
            $("#back_btns").on('click', OnBackButtonClick);
            $("#apply_btn1").on('click', onApply);                        
        });


        $('#confirm_btn').removeClass('blur-button');
        function ViewTermsAndConditions() {
            debugger
            if (document.getElementById('terms-and-conditions').style.display !== "none") {
                document.getElementById('terms-and-conditions').style.display = "none"
                document.getElementById('chevlon-up-icon').style.display = "none"
                document.getElementById('chevlon-down-icon').style.display = "block"
            }
            else {
                document.getElementById('terms-and-conditions').style.display = "block"
                document.getElementById('chevlon-up-icon').style.display = "block"
                document.getElementById('chevlon-down-icon').style.display = "none"
            }
        }
        $('#accept-terms-and-condtn').click(function () {
            if ($("#accept-terms-and-condtn").is(":checked")) {
                $('#apply_btn').show();
            }
            else {
                $('#apply_btn').hide();
            }
        });
        function onApply() {
            debugger
            if ($('#IsNotif').val() == 'true') {
                ValidateGeneralDetails();
            }
            else {
                OverViewValidation();
            }
        }

