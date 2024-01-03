
$(document).ready(function () {
    SelectMenu(5);
    $(".btn_back_to_fleet").on('click', ViewBackbutton);
});

function ViewBackbutton() {
    window.location = "/Vehicle/FleetComponent";
}
