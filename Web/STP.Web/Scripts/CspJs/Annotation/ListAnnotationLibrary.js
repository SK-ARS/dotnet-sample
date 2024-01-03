
    $(document).ready(function () {
if($('#hf_FromAnnotation').val() ==  'True') {
                    $('#contactDivPopup').css('display', 'block');
                    $('#contactDivPage').css('display', 'none');
                }
                else {
                    $('#contactDivPopup').css('display', 'none');
                    $('#contactDivPage').css('display', 'block');
        }

        $(".esdal-table > thead .sorting").each(function () {
             var sort_Order  = $('#hf_sortOrder').val(); 
      var sort_Type  = $('#hf_sortType').val(); 
     var item = $(this);

     if ((sort_Type == 1) && item.find('span').attr('param') == sort_Order) {
         item.addClass('sorting_desc');
     }
     else if ((sort_Type == 0) && item.find('span').attr('param') == sort_Order) {
         item.addClass('sorting_asc');
     }


        });


        $(".AnnotationName").on('click', ViewAnnotationContactDetail);
        $("#thOrganisation").on('click', AnnotationContactSort); 
        $("#filterAddressBook").on('click', openFiltersAddressBook);
        $("#filterContatact").on('click', openFilterContatactView);
        $("#filterimage").on('click', clearAddressBookFilter);
        $("#ImportNotifContact").on('click', ImportNotifContact);
        $("#backToAffectedParties").on('click', BacktoAffectedParties);
        $("#closeAnnotation").on('click', CloseAnnotationPopUp);
        $("#openAnnotationFilter").on('click', openAnnotationFilter);
        
    });

    $('#addressBookPaginator').on('click', 'a', function (e) {
        debugger
                if (this.href == '') {
                    return false;
                }
                else {
                    startAnimation();
                    var FromAnnotation = $("#FromAnnotation").val();
                    var searchColumn = '';
                    var searchValue = '';
                    var search = '';
                    var searchresult = '';
                    if (FromAnnotation == 'True') {
                        searchColumn = $('#popupFilter').find("#DDsearchCriteria").val();
                        searchValue = $('#popupFilter').find("#SearchValue");
                        searchresult = $('#popupFilter').find("#SearchValue").val();
                    }
                    else {
                        searchColumn = $("#DDsearchCriteria").val();
                        searchValue = $("#SearchValue");
                        searchresult = $("#SearchValue").val();
                    }
                    if (searchresult != '' && searchresult != undefined) {
                        search = '&SearchColumn=' + searchColumn + '&' + searchValue.serialize();
                    }
                    $.ajax({
                        url: this.href + search,
                        type: 'GET',
                        cache: false,
                        success: function (result) {

                            if ($('#Isconview').val() == 'False') {

                                $('#notif_contact_list').html(result);
                            }
                            else {

                                $('#portal-table').html(result);
                                $('html, body').animate({ scrollTop: 0 });

                            }
                            stopAnimation();
                        },
                        error: function (xhr, status, error) {
                            stopAnimation();
                        },
                        complete: function () {
                            stopAnimation();

                        }
                    });
                    return false;
                }
            });
            function BacktoAffectedParties() {

                startAnimation();
                $('#leftpanel').show();
                $('#route-assessment').show();
                $("#General").hide();
                $("#backbutton").show();
                stopAnimation();
            }


            function ViewContactDetail(id) {

                startAnimation();
                $('#exampleModalCenter22').hide();
                $("#dialogue1").load('../Application/ViewContactDetails?ContactId=' + id + "", function () {
                    removescroll();
                    $('#contactDetails').modal('hide');
                    $('#contactDetailsForMap').modal('show');
                    $("#dialogue1").show();
                    $("#overlay").show();
                    //startAnimation();
                });
            }
            function closeContactPopup() {
                $('#contactDetailsForMap').modal('hide')
                $("#dialogue1").hide();
                $('#exampleModalCenter22').show();
            }
    function CloseAnnotationPopUp() {
        $("#overlay").css('display', 'block');
                $('#dialogue1').hide();
                $('#dialogue').show();

    }
    function CloseAnnotationErrorPopUp() {
        $("#overlay").css('display', 'block');
        $('#ErrorPopupWithAction').modal('hide');
    }

    $('#addressBookPopupPaginator').on('click', 'a', function (e) {
                if (this.href == '') {
                    return false;
                }
                else {
                    $.ajax({
                        url: this.href,
                        type: 'GET',
                        cache: false,
                        success: function (result) {
                            $('#dialogue1').html(result);
                            $('#exampleModalCenterCnt').modal('show');
                        },

                        complete: function () {
                            //stopAnimation();

                        }
                    });
                    return false;
                }
    });
        function ContactSort(event, param) {
        debugger

        var sort_Order = param;
        var sort_Type = 1;
        if (event.classList.contains('sorting_asc')) {
            sort_Type = 1;
        }
        else if (event.classList.contains('sorting_desc')) {
            sort_Type = 0;
        }
        else if (!event.classList.contains('sorting_asc') && !event.classList.contains('sorting_desc')) {
            sort_Type = 1;
        }
        var url = '../Contact/ContactList';
        $.ajax({
            url: url,
            type: 'POST',
            cache: false,
            data: { sortType: sort_Type, sortOrder: sort_Order, page: '@ViewBag.page',pageSize:'@ViewBag.pageSize' },
            beforeSend: function () {
                startAnimation();
            },
            success: function (response) {
                debugger
                //$('#caution').html();

               // $('#contactDivPage').html($(response).find('#contactDivPage').html());
                //$('#caution').find('#contactDiv').html($(response).find('#contactDiv').html());
                $('#portal-table').html(response);

                //$('#contactDivPage').html(response);

                //$('#contactDivPopup').css('display', 'block');
                //$('#caution').find('#contactPagination').html($(response).find('#contactPagination').html());

                //event.attributes.class.value = 'sorting';
                $(".esdal-table > thead .sorting").removeClass('sorting_asc sorting_desc');

                $(".esdal-table > thead .sorting").each(function () {

                    var item = $(this);
                    if ((sort_Type == 1) && item.find('span').attr('param') == sort_Order) {
                        item.addClass('sorting_desc');
                    }
                    else if ((sort_Type == 0) && item.find('span').attr('param') == sort_Order) {
                        item.addClass('sorting_asc');
                    }

                });
                //$(".sort").css('display', 'none');//Clear all filter value
                //$(".sort[order='1']").not(".sort[param ='" + param + "']").css('display', 'block');//set all filter pipeline
                //$(".sort[order='" + new_sort + "'][param='" + param + "']").css('display', 'block');// display current sort element
            },
            error: function (result) {
                //location.readload();
            },
            complete: function () {
                stopAnimation();
            }
        });
        //event.attributes.class.value = 'sorting';
    }

    function ViewAnnotationContactDetail(e) {
        var contactId = e.currentTarget.dataset.ContactId;
        ViewContactDetail(contactId);
    }
    function AnnotationContactSort(e) {
        ContactSort(e,1);
    }
