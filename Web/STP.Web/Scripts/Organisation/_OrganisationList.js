var SaveMsg = $('#hf_SaveMsg').val();
var orgID = $('#hf_orgID').val();
var openOrgId = 0;

function OrganisationListInit() {
  
    SaveMsg = $('#hf_SaveMsg').val();
    orgID = $('#hf_orgID').val();
    StepFlag = 0;
    SubStepFlag = 0.3;
    CurrentStep = "Haulier Details";
    $('#plan_movement_hdng').text("PLAN MOVEMENT");
    $('#current_step').text(CurrentStep);

    var createAlertMsg = $('#CreateAlert').val();
    if (createAlertMsg == "true" || createAlertMsg == "True") {
        ShowSuccessModalPopup(SaveMsg, 'ReloadLocation');
    }
}

$(document).ready(function () {
   
    $('body').on('click', '.organisation-table #filterimage', function (e) {
        e.preventDefault();
        clearOrganisationSearch(this);
    });
    $('body').on('click', '#btn-clearsearch', function (e) {
        e.preventDefault();
        clearOrganisationSearch(this);
    });
    $('body').on('click', '#btnsearchorg', function (e) {
        
        e.preventDefault();
        SearchOrganisation();
    });
    $('body').on('change', '.ListOrganisation-Pag #pageSizeSelect', function () {
        var page = getUrlParameterByName("page", this.href);
        $('#pageNum').val(page);
        var pageSize = $(this).val();
        $('#pageSizeVal').val(pageSize);
        SearchOrganisation(isSort = true);
    });
    $('body').on('click', '.view', function (e) {
        e.preventDefault();
        var orgid = $(this).attr('orgid');
        if (openOrgId > 0) {
            closeDetailsDiv(openOrgId);
        }
        viewDetails(orgid);
        openOrgId = orgid;
    });
    $('body').on('click', '.import', function (e) {
        e.preventDefault();
        var orgid = $(this).attr('orgid');
        Import(orgid);
    });
    $('body').on('click', '.edit', function (e) {
        e.preventDefault();
        var orgid = $(this).attr('orgid');
        if (openOrgId > 0) {
            closeDetailsDiv(openOrgId);
        }
        Edit(orgid);
        openOrgId = orgid;
    });
    $('body').on('click', '#btnclosedetails', function (e) {
        e.preventDefault();
        var orgid = $(this).attr('orgid');
        closeDetailsDiv(orgid);
    });
    $('body').on('click', '#organisationpaginator a', function (e) {
        e.preventDefault();
        var page = getUrlParameterByName("page", this.href);
        $('#pageNum').val(page);
        SearchOrganisation(true);//using sorting as true to avoid page reset
    });
});

function clearOrganisationSearch() {
    
    $(':input', '#filters')
        .not(':button, :submit, :reset, :hidden')
        .val('')
        .removeAttr('selected');
   
    SearchOrganisation();
}
function SearchOrganisation(isSort = false) {
   
        var searchString = $("#SearchString").val();
   
    var searchOrganisation = $("#SearchOrganisation").val();
    var sortString = $('#txtSORT').val();
    closeFilters();
    $.ajax({
        url: '../Organisation/SaveOrganisationSearch',
        type: 'POST',
        cache: false,
        async: false,
        data: {
            SearchString: searchString, SORT: sortString, SearchOrganisation: searchOrganisation,
            page: (isSort ? $('#pageNum').val() : 1), pageSize: $('#pageSizeVal').val(),
            sortType: $('#filters #SortTypeValue').val(), sortOrder: $('#filters #SortOrderValue').val()
        },
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {
           
            
            if ($('#hf_SORT').val() == 'SORTSO') {
                $('#banner-container').find('div#filters').remove();
                $('div#filters.organisation-filter').remove();
                document.getElementById("vehicles").style.filter = "unset";
                $("#existingOrganisationList").html(response);
                
                var filters = $('#existingOrganisationList').find('div#filters');
                $(filters).insertAfter('#banner');
               
                $("#createOrganisation").hide();
                $("#Go_To_Organisations").hide();
                $("#existingOrganisationList").show();
                $('#list_heading').text("Select Existing Organisations");
                $("#viewExistingOrganisation").hide();
                $('#save_btn').hide();
                $('#confirm_btn').hide();
                initAutoComplete();
            }
            else {
                $('#banner-container').find('div#filters').remove();
                  $('div#filters.organisation-filter').remove();

                $("#manage-user").html(response);
                $('#list_heading').hide();
                initAutoComplete();
                
                //var filters = $('#manage-user').find('div#filters');
                //$(filters).insertAfter('#banner');
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            location.reload();
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function Edit(id) {
    var isAccessible = $('#EditForAdmin').val();
    if (isAccessible == 'False') {
        ShowErrorPopup('You are not authorized to create');
    }
    else {
        startAnimation();
        EditOrganisation(id);
    }
}
function EditOrganisation(id) {

    var link = '../Organisation/CreateOrganisation?mode=Edit&organisationId='+id;
    $("#" + id).load(link, function () {
        CreateOrganisationInit();
        stopAnimation();
    });
    $("#orgDetails-" + id).show();
}
function closeDetailsDiv(id) {
    $("#" + id).empty();
    $("#orgDetails-"+id).hide();
}
function viewDetails(id) {
    startAnimation();
    var link = '../Organisation/CreateOrganisation?mode=View&organisationId='+id;

    $("#" + id).load(link, function () {
        CreateOrganisationInit();
        stopAnimation();
    });
    $("#orgDetails-" + id).show();
    $("#" + id).find("#desc-entry").hide();
}
var sortTypeGlobal = 0;//0-asc
var sortOrderGlobal = 1;//type
function SortOrganisationList(event, param) {
    sortOrderGlobal = param;
    sortTypeGlobal = event.classList.contains('sorting_asc') ? 1 : 0;
    $('#SortTypeValue').val(sortTypeGlobal);
    $('#SortOrderValue').val(sortOrderGlobal);
    SearchOrganisation(isSort = true);
}

