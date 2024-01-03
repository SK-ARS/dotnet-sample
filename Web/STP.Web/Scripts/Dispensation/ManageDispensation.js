var dispId;
var DRefNo;
var userTypeIDVal = $('#hf_userTypeID').val();
$(document).ready(function () {
    $('body').on('click', '#create-dis', function (e) {
        e.preventDefault();
        CreateDispensations(this);
    });
    $('body').on('click', '#closefilter', function (e) {
        e.preventDefault();
        closeFilters(this);
    });
    $('body').on('click', '#clearsearch', function (e) {
         e.preventDefault();
        clearSearchListDispenstaion(this);
    });
    $('body').on('click', '#searchdis', function (e) {
        e.preventDefault();
        SearchDispensationList(this);
    });

    if ($('#hf_IsPlanMovmentGlobal').length == 0) {
        if ((userTypeIDVal == 696001) || (userTypeIDVal == 696002)) {
            SelectMenu(3);
        }
        if (userTypeIDVal == 696007) {
            SelectMenu(4);
        }
    }
    var createAlertMsg = $('#CreateAlert').val();
    var DRefNo = $('#hf_drnnumber').val();
    if (createAlertMsg == "True") {
        //if ($('#hf_mode').val() == 'Edit') {
        //    ShowSuccessModalPopup('Dispensation reference number "' + DRefNo + '" updated successfully', "CloseSuccessModalPopup");
        //}
        //else {
        //    ShowSuccessModalPopup('New dispensation reference number "' + DRefNo + '" created successfully', "CloseSuccessModalPopup");
        //}
    }

});
