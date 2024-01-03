

    $(document).ready(function () {
        //CreateLink();
        SelectMenu(4);
    });

function CreateLink() {
    let hf_contentId = $('#hf_contentId').val()
    let hf_mode = $('#hf_mode').val()
        $.ajax({
            url: '../Information/NewsDetails',
            data: { mode: hf_mode, ContentId: hf_contentId },
            type: 'GET',
            beforeSend: function () {
                startAnimation();
            },
            success: function (result) {
                $("#banner-section-2").html(result);
            },
            error: function (xhr, textStatus, errorThrown) {
                //location.reload();
            },
            complete: function () {
                stopAnimation();
            }
        });
    }
