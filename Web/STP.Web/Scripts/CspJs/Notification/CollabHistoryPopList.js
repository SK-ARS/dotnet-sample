
    $(document).ready(function () {
if($('#hf_SORTSetCollab').val() ==  'False') {
        
        }
        $('#span-close').click(function () {
            $(".modal-backdrop").removeClass("show");
            $(".modal-backdrop").removeClass("modal-backdrop");
        $("#dialogue").hide();
        $("#overlay").hide();
        addscroll();
        resetdialogue();
        });



    });
    function SpanClose() {
        $(".modal-backdrop").removeClass("show");
        $(".modal-backdrop").removeClass("modal-backdrop");
        $('#overlay').hide();
        $('#dialogue').hide();
        addscroll();
        resetdialogue();
        DisplayCollabStatus();
    }




