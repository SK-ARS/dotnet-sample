ViewContactDetails_Obj = {};
function ViewContactDetailsInit() {
    $("#dialogue").show();
    $("#popupDialogue").show();
    stopAnimation();
    $("#overlay").show();
}
$(document).ready(function () {
    
    $('body').on('click', '#spnclosemap', function (e) { e.preventDefault(); closeMp(); });
    $('body').on('click', '#close_contact_popup', function (e) {
        e.preventDefault();
        closeContactPopupViewContact();
    });

});

function closeContactPopupViewContact() {
    $('#contactDetailsForMap').modal("hide");
    $('#contactDetailsForMap').hide();
    $('#exampleModalCenter22').show();
    $("#dialogue1").hide();
    $("#overlay").hide();
    if (ViewContactDetails_Obj.ContactParent != null) {
        window[ViewContactDetails_Obj.ContactParent.Method](ViewContactDetails_Obj.ContactParent.StructureId);
        ViewContactDetails_Obj.ContactParent = null;
    }
}

function closeMp() {
    $('#contactDetails').modal('hide');
    $("#overlay").hide();
}
