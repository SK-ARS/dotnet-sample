$(document).ready(function () {

    //SelectMenu(5); -- need to fix it on Vehicle Component load
    if ($('#hf_importFrm').val() == 'fleet') {

        $('#list_heading').text("Select component from fleet");
        SelectVehiclecomponentFromFleet();
    }
    if ($('#hf_importFrm').val() == 'createcomponent') {
        $('#list_heading').text("Create Component");
        CreateComponentForConfig();
    }

});
function openFilters() {
    var importFrom = $('#hf_importFrm').val();
    var filterWindowWidth = "";
    if (importFrom == 'createcomponent' && importFrom != 'fleet') {
        filterWindowWidth = "660px";
    }
    else {
        filterWindowWidth = "350px";
    }
    document.getElementById("filters").style.width = filterWindowWidth;
    document.getElementById("vehicles").style.filter = "brightness(0.5)";
    document.getElementById("vehicles").style.background = "white";

    function myFunction(x) {
        if (x.matches) { // If media query matches
            document.getElementById("filters").style.width = "200px";
        }
    }
    var x = window.matchMedia("(max-width: 770px)")
    myFunction(x) // Call listener function at run time
    x.addListener(myFunction)
}
function closeFilters() {
    document.getElementById("filters").style.width = "0";
    document.getElementById("vehicles").style.filter = "unset";
}
// showing filter-AdvHaulier in side-nav
function viewAdvHaulier() {
    if (document.getElementById('viewAdvHaulier').style.display !== "none") {
        document.getElementById('viewAdvHaulier').style.display = "none"
        document.getElementById('chevlon-up-icon2').style.display = "none"
        document.getElementById('chevlon-down-icon2').style.display = "block"
    }
    else {
        document.getElementById('viewAdvHaulier').style.display = "block"
        document.getElementById('chevlon-up-icon2').style.display = "block"
        document.getElementById('chevlon-down-icon2').style.display = "none"
    }
}

function SelectVehiclecomponentFromFleet() {

    $.ajax({
        type: "POST",
        url: '../Vehicle/FleetComponent',
        data: { isFromConfig: 1 },
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {
            //var vehiclelist = $(response).find('#vehicle-components-list');
            //$(vehiclelist).appendTo('#importlist_cntr');
            $('#select_vehicle_section').html('');
            if ($('#vehicle_Component_edit_section').html() == undefined) {
                $('#importlist_cntr').html($(response).find('#vehicle-components-list'), function () {
                    event.preventDefault();
                });
            }
            else if ($('#vehicle_Component_edit_section').html().length == 0 || $('#vehicle_Component_edit_section').html().trim().length == 0) {
                $('#vehicle_Create_section #importlist_cntr').html($(response).find('#vehicle-components-list'), function () {
                    event.preventDefault();
                });
            }
            else {
                $('#vehicle_Component_edit_section #importlist_cntr').html($(response).find('#vehicle-components-list'), function () {
                    event.preventDefault();
                    SelectMenu(2);
                });
            }

            $('#viewComponentDetails').html('');

            $('#banner-container').find('div#filters').remove();
            document.getElementById("vehicles").style.filter = "unset";
            if (!$('#vehicle_Create_section')[0]) {
                if ($('#vehicleId').val() != undefined) {
                    $("#importlist_cntr").prepend('<div class="row"><div class="col-lg-9 col-md-9 col-sm-9"><span id="list_heading" class="title">Select component from fleet</span></div><div class="col-lg-2 col-md-2 col-sm-2"><div class="button main-button mr-0"><button class="btn btn-outline-primary ml2 btnBackEdit" role="button" data-icon="&#xe119;" aria-pressed="true" >BACK</button></div></div></div>');
                }
                else {
                    $("#importlist_cntr").prepend('<div class="row"><div class="col-lg-9 col-md-9 col-sm-9"><span id="list_heading" class="title">Select component from fleet</span></div><div class="col-lg-2 col-md-2 col-sm-2"><div class="button main-button mr-0"><button class="btn btn-outline-primary ml2 btnBack" role="button" data-icon="&#xe119;" aria-pressed="true" >BACK</button></div></div></div>');
                }
            }
            else if (($('#vehicle_Component_edit_section').html().length != 0 && $('#vehicle_Component_edit_section').html().trim().length != 0)) {
                SubStepFlag = 2.5;
                SelectMenu(2);
            }
            else {
                $('#vehicle_Create_section').show();
                SubStepFlag = 2.2;
                SelectMenu(2);
            }
            var filters = $(response).find('div#filters');

            var movementId = $('#movementTypeId').val();
            if (movementId != undefined && movementId != "") {
                //var IndentDrpdwn = $(response).find('div#filters #Indend');
                var mvmnt = filters.find('#Indend :selected').text();
                if (mvmnt == "Intended Use" || mvmnt != $('#movementClassificationName').val()) {
                    mvmnt = $('#movementClassificationName').val();
                }
                filters.find('#Indend option').remove();
                filters.find('#Indend').append($('<option>', {
                    text: mvmnt
                }, '</option>'));
            }
            $(filters).appendTo('#banner-container');
            var pagination = $(response).find('div#comp-pagination');
            $(pagination).appendTo('#importlist_cntr');
            $('#importlist_cntr').show();
            $("#ImportFrom").val('fleet');

            $(".btnBackEdit").on('click', OnBackEditBtnClick);
            $(".btnBack").on('click', OnBackBtnClick);

            removeHrefLinks();
            PaginateListMovement();
            //fillPageSizeSelect();
        },
        error: function (result) {
        },
        complete: function () {
            stopAnimation();
        }
    });
}

function CreateComponentForConfig() {

    $.ajax({
        type: "GET",
        url: '../Vehicle/CreateComponent',
        data: { isFromConfig: 1 },
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {

            if ($('#vehicle_Component_edit_section').html() == undefined) {
                $('#importlist_cntr').html($(response).find('#divCreateComponent'), function () {
                    event.preventDefault();
                });
            }
            else if ($('#vehicle_Component_edit_section').html().length == 0 || $('#vehicle_Component_edit_section').html().trim().length == 0) {
                $('#vehicle_Create_section #importlist_cntr').html($(response).find('#divCreateComponent'), function () {
                    event.preventDefault();
                });
            }
            else {
                $('#vehicle_Component_edit_section #importlist_cntr').html($(response).find('#divCreateComponent'), function () {
                    event.preventDefault();
                    SelectMenu(2);
                });
            }

            if (!$('#vehicle_Create_section')[0] && $('#vehicle_Create_section')[0] != undefined) {
                if ($('#vehicleId').val() != undefined) {
                    $("#importlist_cntr").prepend('<div class="row"><div class="col-lg-9 col-md-9 col-sm-9"><span id="list_heading" class="title">Create component</span></div><div class="col-lg-2 col-md-2 col-sm-2"><div class="button main-button mr-0"><button class="btn btn-outline-primary ml2 btnBackEdit" role="button" data-icon="&#xe119;" aria-pressed="true" >BACK</button></div></div></div>');
                }
                else {
                    $("#importlist_cntr").prepend('<div class="row"><div class="col-lg-9 col-md-9 col-sm-9"><span id="list_heading" class="title">Create component</span></div><div class="col-lg-2 col-md-2 col-sm-2"><div class="button main-button mr-0"><button class="btn btn-outline-primary ml2 btnBack" role="button" data-icon="&#xe119;" aria-pressed="true" >BACK</button></div></div></div>');
                }

                $(".btnBackEdit").on('click', OnBackEditBtnClick);
                $(".btnBack").on('click', OnBackBtnClick);
            }
            else if (($('#vehicle_Component_edit_section').html().length != 0 && $('#vehicle_Component_edit_section').html().trim().length != 0)) {
                SubStepFlag = 2.5;
                SelectMenu(2);
            }
            else {
                $('#vehicle_Create_section').show();
                SubStepFlag = 2.2;
                SelectMenu(2);
            }

            $('#divAllComponent').html('');
            if ($('#IsCandVersion').val() == "True") {
                CandidateBackSubFlag = 0.1;
            }
        },
        error: function (result) {

        },
        complete: function () {
            stopAnimation();
        }
    });
}

//function remove href from pagination ul li
function removeHrefLinks() {

    $('#importlist_cntr').find('.pagination').find('li a').removeAttr('href').css("cursor", "pointer");

    //activate first link
    //$('.pagination').find('li:first').addClass('activated');
}

function fillPageSizeSelect() {
    var selectedVal = $('#pageSizeVal').val();
    $('#pageSizeSelect').val(selectedVal);
}

//function Pagination for ------------------------------
//function PaginateList() {
//    if ($("#IsVehicle").val() != 'true') {
//        //method to paginate through page numbers
//        $('#importlist_cntr').find('.pagination').find('li').not('.active, .PagedList-skipToLast, .PagedList-skipToNext, .PagedList-skipToFirst, .PagedList-skipToPrevious').find('a').click(function () {
//            //var pageCount = $('#TotalPages').val();
//            var pageNum = $(this).html();
//            AjaxPaginationForComponentList(pageNum);

//        });
//        PaginateToLastPage();
//        PaginateToFirstPage();
//        PaginateToNextPage();
//        PaginateToPrevPage();
//    }
//}

//method to paginate to last page
function PaginateToLastPage() {
    $('#importlist_cntr').find('.PagedList-skipToLast').click(function () {

        var pageCount = $('#TotalPages').val();
        AjaxPaginationForComponentList(pageCount);

    });
}

//method to paginate to first page
function PaginateToFirstPage() {
    $('#importlist_cntr').find('.PagedList-skipToFirst').click(function () {
        AjaxPaginationForComponentList(1);
    });
}

//method to paginate to Next page
function PaginateToNextPage() {
    $('#importlist_cntr').find('.PagedList-skipToNext').click(function () {
        var thisPage = $('#importlist_cntr').find('.active').find('a').html();
        var nextPage = parseInt(thisPage) + 1;
        AjaxPaginationForComponentList(nextPage);

    });
}
//method to paginate to Previous page
function PaginateToPrevPage() {
    $('#importlist_cntr').find('.PagedList-skipToPrevious').click(function () {
        var thisPage = $('#importlist_cntr').find('.active').find('a').html();
        var prevPage = parseInt(thisPage) - 1;
        AjaxPaginationForComponentList(prevPage);
    });
}

//function Ajax call for pagination
function AjaxPaginationForComponentList(pageNum) {
    var selectedVal = $('#pageSizeVal').val();
    var pageSize = selectedVal;
    var txt_srch = $('.serchlefttxt').val();
    $.ajax({
        url: '../Vehicle/FleetComponent',
        data: { page: pageNum, pageSize: pageSize, importFlag: true, searchString: txt_srch, isFromConfig: 1 },
        type: 'GET',
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {

            $('#importlist_cntr').html($(response).find('#vehicle-components-list'), function () {
            });
            //$("#importlist_cntr").prepend('<span id="list_heading" class="title">Select vehicle from fleet</span>');
            if (!$('#vehicle_Create_section')[0]) {
                $("#importlist_cntr").prepend('<div class="row"><div class="col-lg-9 col-md-9 col-sm-9"><span id="list_heading" class="title">Select component from fleet</span></div><div class="col-lg-2 col-md-2 col-sm-2"><div class="button main-button mr-0"><button class="btn btn-outline-primary ml2 btnBack" role="button" data-icon="&#xe119;" aria-pressed="true" >BACK</button></div></div></div>');

                $(".btnBack").on('click', OnBackBtnClick);
            }

            $('#divAllComponent').html('');
            $('#banner-container').find('div#filters').remove();
            document.getElementById("vehicles").style.filter = "unset";
            var filters = $(response).find('div#filters');

            var movementId = $('#movementTypeId').val();
            if (movementId != undefined && movementId != "") {
                //var IndentDrpdwn = $(response).find('div#filters #Indend');
                var mvmnt = filters.find('#Indend :selected').text();
                filters.find('#Indend option').remove();
                filters.find('#Indend').append($('<option>', {
                    text: mvmnt
                }, '</option>'));


            }
            //$(filters).insertAfter('section#banner');
            $(filters).appendTo('#banner-container');

            var pagination = $(response).find('div#comp-pagination');
            $(pagination).appendTo('#importlist_cntr');
            removeHrefLinks();
            PaginateListMovement();
            //PaginateList();
        },
        error: function (xhr, textStatus, errorThrown) {
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function clearSearch() {

    $(':input', '#filters')
        .not(':button, :submit, :reset, :hidden')
        .val('')
        .removeAttr('selected');
    $("#FilterFavourites").prop("checked", false);
    SearchVehicleComponent();
}

function SearchVehicleComponent() {

    var searchString = $('#searchText').val();
    var vehicleIntend = $('#Indend').val();
    var vehicleType = $('#VehType').val();
    var IsFromConfig = $('#IsFromConfig').val();
    closeFilters();
    $.ajax({
        url: '../Vehicle/SaveSearchData',
        type: 'POST',
        cache: false,
        async: false,
        data: { searchString: searchString, vehicleIntend: vehicleIntend, vehicleType: vehicleType, isFromConfig: IsFromConfig },
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {

            stopAnimation();
            $('#importlist_cntr').html($(response).find('#vehicle-components-list'), function () {
            });
            if (!$('#vehicle_Create_section')[0]) {
                $("#importlist_cntr").prepend('<div class="row"><div class="col-lg-9 col-md-9 col-sm-9"><span id="list_heading" class="title">Select component from fleet</span></div><div class="col-lg-2 col-md-2 col-sm-2"><div class="button main-button mr-0"><button class="btn btn-outline-primary ml2 btnBack" role="button" data-icon="&#xe119;" aria-pressed="true" >BACK</button></div></div></div>');

                $(".btnBack").on('click', OnBackBtnClick);
            }
            var filters = $(response).find('div#filters');

            var movementId = $('#movementTypeId').val();
            if (movementId != undefined && movementId != "") {
                //var IndentDrpdwn = $(response).find('div#filters #Indend');
                var mvmnt = filters.find('#Indend :selected').text();
                filters.find('#Indend option').remove();
                filters.find('#Indend').append($('<option>', {
                    text: mvmnt
                }, '</option>'));


            }
            $(filters).appendTo('#banner-container');

            removeHrefLinks();
            PaginateListMovement();
            fillPageSizeSelect();

        }
    });
}


function ViewComponentDetail(componentId) {

    $.ajax({
        url: '../Vehicle/GeneralComponent',
        data: { componentId: componentId },
        type: 'GET',
        async: false,
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {
            SubStepFlag = 2.4;
            $('#importlist_cntr').hide();
            var compView = $(response).find('.divcomponentView');
            $(compView).appendTo('#viewComponentDetails');
            $(this).scrollTop();
            if ($('#IsMovement').val() == "true" || $('#IsMovement').val() == "True") {
                $('#btn_back_to_fleet').hide();
            }
        },
        error: function (xhr, textStatus, errorThrown) {
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function ViewBackbutton() {
    $('#importlist_cntr').show();
    $('#viewComponentDetails').html('');
    $(this).scrollTop();
}

