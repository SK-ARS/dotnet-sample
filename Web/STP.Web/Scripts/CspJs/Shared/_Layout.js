    function SelectMenu(index) {
        $('.nav-link').removeClass('active');
        $('#menu_container li:nth-child(' + index + ')').find('a.nav-link').addClass('active');
    }

    //--- for HE-2294: for closing filter when click on outside
    window.addEventListener('click', function (e) {
        if (document.getElementById('filters') != null && document.getElementById('filters') != 'undefined') {
            if (!document.getElementById('filters').contains(e.target)) {
                var filterIcon = document.getElementsByClassName('filter-icon');
                if (filterIcon.length == 0) {
                    //filterIcon = document.getElementsByClassName('table-filter-header');
                    //if (filterIcon.length == 0) {
                    //    filterIcon = document.getElementsByClassName('fltFilter');
                    //}
                }
                else if (!filterIcon[0].contains(e.target)) {
                    if (document.getElementById("filters").style.width != '0px') {
                        $("#overlay").hide();
                        $("#overlay").css("z-index", "");
                        document.getElementById("filters").style.width = "0";
                        document.getElementById("banner").style.filter = "unset"
                        document.getElementById("navbar").style.filter = "unset";
                    }
                }
            }
        }
    });

        //$(document).ready(function () {
            AntiForgeryTokenInclusionRequest('@Html.AntiForgeryToken()');
        //});
