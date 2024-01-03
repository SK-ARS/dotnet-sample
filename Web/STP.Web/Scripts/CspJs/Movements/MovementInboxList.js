
    $(document).ready(function () {
        $("#closeFilters").on('click', closeFilters);
        $("#SearchSOAList").on('click', SearchSOAList);
        $("#ClearAdvanced").on('click', ClearAdvancedfn);
        $("#table-head").on('click', viewAdvanceSearch);
        $("#UnopenedByUser").on('change', ToggleUnopenedByUserOutSide);
    }); 
    SelectMenu(2);


        function ClearAdvancedfn() {
            ClearAdvanced(0);
        }
    // showing user-setting-info-filter
    function openNav() {
        document.getElementById("mySidenav").style.width = "320px";
        document.getElementById("banner").style.filter = "brightness(0.5)";
        document.getElementById("banner").style.background = "white";
        document.getElementById("navbar").style.filter = "brightness(0.5)";
        document.getElementById("navbar").style.background = "white";
        function myFunction(x) {
            if (x.matches) { // If media query matches
                document.getElementById("mySidenav").style.width = "200px";
            }
        }
        var x = window.matchMedia("(max-width: 992px)")
        myFunction(x) // Call listener function at run time
        x.addListener(myFunction)

    }
    function closeNav() {
        document.getElementById("mySidenav").style.width = "0";
        document.getElementById("banner").style.filter = "unset"
        document.getElementById("navbar").style.filter = "unset";
    }

    // showing user-setting inside vertical menu
    function showuserinfo() {
        if (document.getElementById('user-info').style.display !== "none") {
            document.getElementById('user-info').style.display = "none"
        }
        else {
            document.getElementById('user-info').style.display = "block";
            document.getElementsById('userdetails').style.overFlow = "scroll";
        }
    }
    // showing user-setting-info-filter
    // show pop-up card in the column
    function showcard() {
        if (document.getElementById('showcard').style.display !== "none") {
            document.getElementById('showcard').style.display = "none"
        }
        else {
            document.getElementById('showcard').style.display = "block"
        }
    }
    // Attach listener function on state changes

        function BackRevStructSummary() {
            window.location.href = "../Structures/ReviewSummary" + EncodedQueryString("structureId=" + '@ViewBag.structureID');
        }

        var isSearchStarted = false;
        var isApiCallPending = false;
        function SearchSOAList(isSearch=false) {

            if (isSearchStarted) {
                isApiCallPending = true;
                return false;
            }
            console.log('isSearchStarted', isSearchStarted);
            isSearchStarted = true;
            //var insideValue = $('#ESDALReference').val();
            var notiTableAlias = "noti.";
            var appTableAlias = "a.";

            var queryString = "";
            var strVehiclebtw = "";
            var strVehicle = "";
            $('#VehicleFilterDiv').find('.VehicleFilter').each(function () {
                if ($(this).find('#OperatorCount').val() == "between") {
                    strVehiclebtw = $(this).find('#VehicleDimensionCount').val() + " " + $(this).find('#OperatorCount').val() + " " + $(this).find('.vehicletextbox').val() + " and " + $(this).find('.vehicletextbox1').val() ;
                    queryString += " (" + notiTableAlias + strVehiclebtw + " or " + appTableAlias + strVehiclebtw + ") " + $(this).find("#operator option:selected").text();
                }
                else if ($(this).find('.vehicletextbox').val() != "") {
                    strVehicle = $(this).find('#VehicleDimensionCount').val() + " " + $(this).find('#OperatorCount').val() + " " + $(this).find('.vehicletextbox').val() ;
                    queryString += " (" + notiTableAlias + strVehicle + " or " + appTableAlias + strVehicle + ") " + $(this).find("#operator option:selected").text();
                }
            });
            var trimedString = queryString.trim();
            var lastIndex = trimedString.lastIndexOf(" ");
            str = trimedString.substring(0, lastIndex);
            if (str != "") {
                str = "(" + str + ")";
            }
            $('#QueryString').val(str);

            //$('#ESDALReference').val(insideValue);
            var params = isSort ? "?page="+$('#pageNum').val() : "?page=1";
            $.ajax({
                url: '/Movements/FilterMoveInbox',
                //dataType: 'json',
                type: 'POST',
                cache: false,
                async: false,
                data: $("#frmFilterMoveInbox").serialize(),
                beforeSend: function () {
                    if (!isSearch) {
                        startAnimation();
                    }
                },
                success: function (response) {

                    $('.movements').html('');
                    $('.movements').html($(response).find('.movements').html());
                    //$('.div_so_movement').find("#filterimage").css("display", "block");
                    closeFilters();
                    isSearchStarted = false;
                    if (isApiCallPending) {
                        isApiCallPending = false;
                        SearchSOAList();
                    }
                },
                error: function (xhr, status) {

                    location.reload();
                },
                complete: function () {
                    if (!isSearch) {
                        stopAnimation();
                    }
                }

            });
        }
