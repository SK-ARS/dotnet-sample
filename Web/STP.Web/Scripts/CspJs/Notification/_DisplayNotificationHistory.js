 

    $('#NotificationHistory').on('click', 'a', function (e) {
        if (this.href == '') {
            return false;
        }
        else {
            $.ajax({
                url: this.href,
                type: 'GET',
                cache: false,
                success: function (result) {
                    $('#banner-container').find('div#filters').remove();
                    document.getElementById("vehicles").style.filter = "unset";
                    $("#notif_history_details").html(result);
                    var filters = $('#notif_history_details').find('div#filters');
                        $(filters).appendTo('#banner-container');

                       
                     
                }
            });
            return false;
        }
    });


