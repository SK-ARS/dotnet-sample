    {
       
        $(document).ready(function () {
            $('#span-close').click(function () {
                $('#overlay').hide();
                addscroll();
            });
            Resize_PopUp(650);
            addscroll();

            $("#span-close").on('click', closeSpan);
            $("#IDclose").on('click', CloseContact);
        });

        function closeSpan() {
            $('#overlay').hide();
            addscroll();
            resetdialogue();
        }
        function CloseContact() {
            $('#exampleModalCenter22').hide();
            $('#overlay').hide();
            addscroll();
            resetdialogue();
        }
    }
