    var isvalid = true;
    var mousedownHappened = false;
    var flag = true;
    $(document).ready(function () {

        $("#SaveNeUser").on('click', SaveNeUser);
if($('#hf_mode').val() ==  "SORTSO") {
            selectedmenu('Admin');
        }
if($('#hf_AuthKey').val() ==  '') {
            document.getElementById("IsValidNe").checked = true;

        }
        document.getElementById("AuthKey").readOnly = true;

        $('#addrchk').text('');

        $('#btn_reset').click(function () {
            $("#HaulName").val('');
            $("#AuthKey").val('');
            $("#OrgNam").val('');
            $('#IsValidNe').attr('checked', false);
            $('#IsValidNe').prop('checked', false);
            $("#NeLimit").val('');
            $(".error").html("");

        });
        $("#HaulName").change(function () {
            var NehaulName = $("#HaulName").val();
            var Encrypt = btoa(NehaulName);
            Encrypt = RandomEncry(Encrypt);
            Encrypt = Encrypt.substring(0, 20);
            $("#AuthKey").val(Encrypt);
            if ($("#AuthKey").val() != '') {
                document.getElementById("IsValidNe").checked = true;

            }


        });

    });
    function EncryDecry(e) {
        $('#err_required_haulname').text('');
        var NehaulName = $("#HaulName").val();
        var Encrypt = btoa(NehaulName);
        Encrypt = RandomEncry(Encrypt);
        Encrypt = Encrypt.substring(0, 20);
        $("#AuthKey").val(Encrypt);
        if ($("#AuthKey").val() != '') {
            document.getElementById("IsValidNe").checked = true;

        }
        var code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13) { //Enter keycode
            SaveNeUser();
        }

    }
    function SaveNeUser() {
        startAnimation();
         var NehaulName = $("#HaulName").val();
         var AuthKey = $("#AuthKey").val();
         var OrgName = $("#OrgNam").val();
         var NenLimit = $("#NeLimit").val();
        var AuthKeyId  = $('#hf_AuthKeyId').val(); 
        if (document.getElementById("IsValidNe").checked != true) {
            AuthKey = "";
        }
         if (AuthKeyId == 0) {
             var msg = "Non esdal haulier saved successfully";
         }
         else if (AuthKey == "") {
             var msg = "Non esdal haulier updated successfully";
         }
         else {
             var msg = "Non esdal haulier updated successfully and new authentication key is generated";
         }

         if (checkValidate(AuthKeyId)) {
             $.ajax({
                 url: '@Url.Action("SaveNeUser", "NENNotification")',
                 type: 'GET',
                 cache: false,
                 async: false,
                 data: { Haulname: NehaulName, authKey: AuthKey, OrgName: OrgName, NeLimit: NenLimit, KeyId: AuthKeyId },
                 beforeSend: function () {
                     startAnimation();
                 },
                 success: function (result) {
                     if (result == 1) {
                         ShowSuccessModalPopup(msg, 'ReloadNeUsers');
                     }
                     else {
                         ShowWarningPopup('Saving Failed','ReloadNeUsers');

                     }

                 },
                 error: function (xhr, textStatus, errorThrown) {
                     //other stuff
                     location.reload();
                 },
                 complete: function () {
                     stopAnimation();

                 }
             });
         }

    }
    function checkValidate(IsCreate) {
        //IsCreate flag is 0 then we have to check duplication of haulier
        var NehaulName = $("#HaulName").val();
        var OrgName = $("#OrgNam").val();
        var NenLimit = $("#NeLimit").val();
        if (NehaulName == '') {
            $('#err_required_haulname').text('Haulier Name is required');
            $('#err_required_haulname').show();
            if (OrgName == '') {
                $('#err_required_orgname').text('Organisation Name is required');
                $('#err_required_orgname').show();
            }
            if (NenLimit != "") {
                if (isNaN(NenLimit)) {
                    $('#err_number_nelimit').text('NEN limit must be a number');
                    $('#err_number_nelimit').show();
                }

            }
            stopAnimation();
            return false;

        }
        if (OrgName == '') {
            $('#err_required_orgname').text('Organisation Name is required');
            $('#err_required_orgname').show();
            if (NenLimit != "") {
                if (isNaN(NenLimit)) {
                    $('#err_number_nelimit').text('NEN limit must be a number');
                    $('#err_number_nelimit').show();
                }

            }
            stopAnimation();
            return false;

        }
        if (NenLimit != "") {
            if (isNaN(NenLimit)) {
                $('#err_number_nelimit').text('NEN limit must be a number');
                $('#err_number_nelimit').show();
                stopAnimation();
                return false;

            }

        }
        if (IsCreate == 0) {
            var IsValHaul = HaulierValidation();
            if (IsValHaul > 0) {
                $('#err_required_haulname').text('Haulier Name already exists');
                $('#err_required_haulname').show();
                stopAnimation();
                return false;

            }

        }
        return true;
    }
    function HaulierValidation() {
        var NehaulName = $("#HaulName").val();
        var OrgName = $("#OrgNam").val();
        var IsVal = 0;
        $.ajax({
            url: '@Url.Action("HaulierValid", "NENNotification")',
            type: 'GET',
            cache: false,
            async: false,
            data: { haulname: NehaulName, orgname: OrgName },
            success: function (result) {
                IsVal=result;
            },

        });
        return IsVal;
    }
    function OrgKeypress() {
        $('#err_required_orgname').text('');
        var code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13) { //Enter keycode
            SaveNeUser();
        }

    }
        function RandomEncry(possible) {   // function for making a random string to appent
            var text = "";

            for (var i = 0; i < 20; i++) {
                text += possible.charAt(Math.floor(Math.random() * possible.length));
            }
            text = text.substring(0, 20);
            return text;
        }
