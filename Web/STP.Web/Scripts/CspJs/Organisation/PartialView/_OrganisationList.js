            $(document).ready(function () {
                
                $("#btnclose").on('click', closeFilters);
                $("#btnopen").on('click', openFilters);
                $("#filterimage").on('click', clearOrganisationSearch);
                $("#btn-clearsearch").on('click', clearOrganisationSearch);
                $("#btnsearchorg").on('click', SearchOrganisation);
                
                var searchString = $('#SearchString').val();
                var searchOrganisation = $('#SearchOrganisation').val();
                StepFlag = 0;
                SubStepFlag = 0.3;
                CurrentStep = "Haulier Details";
                $('#plan_movement_hdng').text("PLAN MOVEMENT");
                $('#current_step').text(CurrentStep);
                $('#save_btn').hide();

                var createAlertMsg = $('#CreateAlert').val();
                if (createAlertMsg == "true") {

                    ShowSuccessModalPopup('@ViewBag.SaveMsg','ReloadLocation');
                }

                $("#SearchString").autocomplete({
                    appendTo: $("#SearchString").parent(),
                source: function (request, response) {
                    ;
                        $.ajax({
                            url: '@Url.Action("OrganisationSummary", "Dispensation")',
                            dataType: "json",
                            data: {
                                SearchString: request.term, page: 0, pageSize:0
                            },
                            success: function (data) {
                                ;
                                response($.map(data, function (item) {
                                    return { label: item.OrganisationName  };
                                }));
                            },
                            error: function (jqXHR, exception, errorThrown) {
                                console.log(errorThrown);
                            }
                        });
                    },
                    minLength: 2,
                    select: function (event, ui) {
                        $('#SearchString').val(ui.item.label);
                        return false;
                    },
                    focus: function (event, ui) {
                        $("#SearchString").val(ui.item.label);
                        return false;
                    }
                });


            });
            $('body').on('click', '.view', function (e) {

                e.preventDefault();
                var orgid = $(this).data('orgid');

                viewDetails(orgid);
            });

            $('body').on('click', '.import', function (e) {

                e.preventDefault();
                var orgid = $(this).data('orgid');

                Import(orgid);
            });
            $('body').on('click', '.edit', function (e) {

                e.preventDefault();
                var orgid = $(this).data('orgid');

                Edit(orgid);
            });

            $('body').on('click', '#btnclosedetails', function (e) {

                e.preventDefault();
                var orgid = $(this).data('orgid');

                closeDetailsDiv(orgid);
            });
            $('#organisationpaginator').on('click', 'a', function (e) {
                e.preventDefault();
                var page = getUrlParameterByName("page", this.href);
                $('#pageNum').val(page);
                SearchOrganisation(true);//using sorting as true to avoid page reset
            });
            function clearOrganisationSearch() {
                $(':input', '#filters')
                    .not(':button, :submit, :reset, :hidden')
                    .val('')
                    .removeAttr('selected');
                SearchOrganisation();
            }
            function SearchOrganisation(isSort=false) {
                var searchString = $('#SearchString').val();
                var searchOrganisation = $('#SearchOrganisation').val();
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

if($('#hf_SORT').val() ==  'SORTSO') {
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
                        }
                        else {
                            $("#manage-user").html(response);
                            var filters = $('#manage-user').find('div#filters');
                            $('#organisationheader').hide();
                            $(filters).insertAfter('section#banner');
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

                var link = '@Html.Raw(Url.Action("CreateOrganisation", "Organisation", new { mode = "Edit", organisationId = "_ID" }))';
                link = link.replace("_ID", id);

                $("#" + id).load(link);
                $("#orgDetails-"+id).show();
            }
            function closeDetailsDiv(id) {
                $("#" + id).empty();
                $("#orgDetails-" + id).hide();
            }
            function viewDetails(id) {
                startAnimation();
                var link = '@Html.Raw(Url.Action("CreateOrganisation", "Organisation", new { mode = "View", organisationId = "_ID" }))';
                link = link.replace("_ID", id);

                $("#" + id).load(link);
                $("#orgDetails-" + id).show();
                $("#@ViewBag.orgID").find("#desc-entry").hide();
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
            
