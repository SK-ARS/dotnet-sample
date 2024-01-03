    {
        $(document).ready(function () {
            $("#closcontact").on('click', CloseContactDetails );
            $('#overlay').show();
            $("#dialogue").show();
            $('#exampleModalCenter22').modal('show');
            $("#dialogue").css("display","block");
        });
        function CloseContactDetails() {
            $('#exampleModalCenter22').hide();
            $('#overlay').hide();
            $("#dialogue").hide();
            $(".modal-backdrop").removeClass("show");
            $(".modal-backdrop").removeClass("modal-backdrop");
            addscroll();
            resetdialogue();
            stopAnimation();
        }

    }
