            integrity="sha384-b5kHyXgcpbZJO/tY9Ul7kGkf1S0CWuKcCD38l8YkeH8z8QjE0GmW1gYU5S9FOnJ0"

        $(document).ready(function () {
            SelectMenu(3);
            $("#btnBack").on('click', BackButtonClick);
        });
        $('.dropdown-toggle').dropdown();

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

        // Attach listener function on state changes

        function BackClick(structId) {

            window.location.href = "../Structures/ReviewSummary?structureId=" + structId + "";
        };

        function BackButtonClick(e) {
            var structureId = e.currentTarget.dataset.StructureId;
            BackClick(structureId);
        }

