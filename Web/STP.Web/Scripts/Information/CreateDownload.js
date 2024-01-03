$(document).ready(function () {
SelectMenu(4);
    //CreateDownload();
});

function CreateDownload() {
    $.ajax({
        url: '../Information/NewsDetails',
        data: { mode: $("#hf_mode").val(), ContentId: $("#hf_contentId").val() },
        type: 'GET',
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
            $("#banner-section-2").html(result);
        },
        error: function (xhr, textStatus, errorThrown) {
            location.reload();
        },
        complete: function () {
            stopAnimation();
        }
    });
}
