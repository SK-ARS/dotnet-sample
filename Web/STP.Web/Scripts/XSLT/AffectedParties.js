function AffectedPartiesInit() {
    var ProjectStatus = $("#ProjectStatus").val();
    var SORTType = $("#SORTType").val();
    if (SORTType == undefined) {
        if (ProjectStatus == 307014) {
            $('#addcontact').hide();
            $('.dispensation_btn').hide();
            $('.delete-manually-affected').hide();
        }
    }
}

$(document).ready(function () {
    $('body').on('click', '.delete-affect-contact', function (e) {
        e.preventDefault();
        var orgname = $(this).attr("orgname");
        var fullname = $(this).attr("fullname");
        DeleteAffectedContact(orgname, fullname);
    });
    $('body').on('click', '.add-contact-popup', function (e) {
        
        e.preventDefault();
        AddContactPopUp();
    });
    $('body').on('click', '.display-contact', function (e) {
        e.preventDefault();
        var contactId = $(this).attr("contactid");
        DisplayContact(contactId);
    });
    $('body').on('click', '.exclude', function (e) {
        
        e.preventDefault();
        var contactid = $(this).attr("contactid");
        var orgid = $(this).attr("orgid");
        var orgname = $(this).attr("orgname");
        Exclude(contactid, orgid, orgname);
    });
    $('body').on('click', '.include', function (e) {
        
        e.preventDefault();
        var contactid = $(this).attr("contactid");
        var orgid = $(this).attr("orgid");
        var orgname = $(this).attr("orgname");
        Include(contactid, orgid, orgname);
    });
    $('body').on('click', '.view-disp-aff-party', function (e) {
        e.preventDefault();
        var orgname = $(this).attr("orgname");
        var orgid = $(this).attr("orgid");
        ViewDispensationAffParties(orgid, orgname);
    });
    $('body').on('click', '.affect-party-show-detail', function (e) {
        e.preventDefault();
        var orgname = $(this).attr("orgname");
        var orgid = $(this).attr("orgid");
        ShowDetail(orgid, orgname);
    });
});