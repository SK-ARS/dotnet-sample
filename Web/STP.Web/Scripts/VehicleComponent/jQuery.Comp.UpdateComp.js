
    $(function () {
        $(".btn_back").live("click", function () {
            
        });

        $(".page_help").attr('url', '../../Content/ESDAL2_help/axle.html');
        $(".page_help").click(function () {
            
            window.open($(this).attr("url"), "popupWindow", "width=920,height=620,scrollbars=yes");
        });
        $(".btn_Next").live("click", function () {

            var _this = $(this);
            var _form = _this.closest("form");

            var validator = $("form").validate(); // obtain validator
            var anyError = false;
            _form.find("input").each(function () {
                if (!validator.element(this)) { // validate every input element inside this step
                    anyError = true;
                }
            
            });

            if (anyError)
                return false; // exit if any error found    


            $.ajax({
                type: 'POST',
                url: _form.attr("action"),
                data: _form.serialize(),
                success: function (data) {
                    if (data.Success) {
                        $('#selection').hide();
                        $('#div_general').hide();
                        $('#div_register').show();
                    }
                },
                async: false
            });

            //$("#tabs").tabs("option", "active", 1);
            return false;

        });

        $(".btn_Prev").click(function () {
            var selected = $("#tabs").tabs("option", "active");
            $("#tabs").tabs("disable", selected);
            $("#tabs").tabs("enable", selected - 1);
            $("#tabs").tabs("option", "active", selected - 1);
        });

        $('.btn_add').live('click', function () {
            $(this).closest('tr').clone().find("input:text").each(function () {
                $(this).val('').attr('id', function (_, id) { return id });
            }).end().appendTo("#id_Register tbody");
        });
    });
