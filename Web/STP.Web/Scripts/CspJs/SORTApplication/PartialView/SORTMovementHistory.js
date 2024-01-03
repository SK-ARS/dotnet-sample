    $(document).ready(function () {
        $('#leftpanel').hide();
    });

    $('#SORTHistory').on('click', 'a', function (e) {
        if (this.href == '') {
            return false;
        }
        else {
            $.ajax({
                url: this.href,
                type: 'GET',
                cache: false,
                success: function (result) {
                    $('#SupplInfo').html(result);
                }
            });
            return false;
        }
    });
