
$(document).ready(function () {
    //CreateInformation();
    SelectMenu(4);

});

function CreateInformation() {
    $.ajax({
        url: '../Information/NewsDetails',
        data: { mode: $("#hf_mode").val(), ContentId: $("#hf_contentId").val() },
        type: 'GET',
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
            $("#banner-section-2").html(result);
            //NewsDetailsInit();
        },
        error: function (xhr, textStatus, errorThrown) {
            location.reload();
        },
        complete: function () {
            stopAnimation();
        }
    });
}
