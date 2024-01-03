    SelectMenu(4);
    $(document).ready(function () {
        CreateLink();
    });

    function CreateLink() {
        $.ajax({
            url: '../Information/NewsDetails',
            data: { mode: '@mode', ContentId: @contentId },
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
