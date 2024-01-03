integrity = "sha384-b5kHyXgcpbZJO/tY9Ul7kGkf1S0CWuKcCD38l8YkeH8z8QjE0GmW1gYU5S9FOnJ0"

$(document).ready(function () {
    SelectMenu(3);
$('body').on('click','#btnBack', function(e) { 
  e.preventDefault();
  BackButtonClick(this);
});
$('.dropdown-toggle').dropdown();
});

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

function BackClick(structId) {
    window.location.href = "../Structures/ReviewSummary?structureId=" + structId + "";
};

function BackButtonClick(e) {
    var structureId =$(e).attr("structureid");
    BackClick(structureId);
}

