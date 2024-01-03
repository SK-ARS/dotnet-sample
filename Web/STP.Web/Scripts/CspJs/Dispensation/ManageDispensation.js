    var dispId;
    var DRefNo;
    $(document).ready(function () {
        $("#create-dis").on('click', CreateDispensations);
        $("#closefilter").on('click', closeFilters);
        $("#clearsearch").on('click', clearSearchListDispenstaion);
        $("#searchdis").on('click', SearchDispensationList);
        if ((@userTypeID == 696001)||(@userTypeID == 696002)) {
            SelectMenu(3);
        }
        if (@userTypeID == 696007) {
            SelectMenu(4);
        }
        //Load list on initial load
        //SearchDispensationList();

        var createAlertMsg = $('#CreateAlert').val();
        var DRefNo  = $('#hf_drnnumber').val(); 
        if (createAlertMsg == "True")
        {
if($('#hf_mode').val() ==  'Edit') {
                ShowSuccessModalPopup('Dispensation reference number "' + DRefNo + '" updated successfully', "CloseSuccessModalPopup");
            }
            else {
                ShowSuccessModalPopup('New dispensation reference number "' + DRefNo + '" created successfully', "CloseSuccessModalPopup");
            }
        }

    });
