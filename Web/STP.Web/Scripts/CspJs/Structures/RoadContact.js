    {
        $(document).ready(function () {
            $("#span-close").on('click', closeSpan);
            $("#closingcontact").on('click', CloseContact);
            $('#span-close').click(function () {
                $('#overlay').hide();
                addscroll();
            });
            Resize_PopUp(650);
            addscroll();
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
