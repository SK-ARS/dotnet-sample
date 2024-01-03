        $(document).ready(function () {
            $("#closeViewCollab").on('click', closeViewCollab);        
        });


        $('#reviewCausionPaginator').on('click', 'a', function (e) {
            if (this.href == '') {
                return false;
            }
            else {
                $.ajax({
                    url: this.href,
                    type: 'GET',
                    cache: false,
                    success: function (result) {
                        $('#General').html(result);
                    }
                });
                return false;
            }
        });

