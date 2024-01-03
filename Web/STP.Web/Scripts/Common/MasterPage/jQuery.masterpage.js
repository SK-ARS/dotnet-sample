$(document).ready(function () {
   
    //$('.tooltip').tooltip({
    //    track: true
    //});
    var notify = $('#Notification').val();
    var daysRemain = $('#DaysRemaining').val();
    
    if (notify == "True") {
       
    }

    PreventDateEntry();
});


function BackToPreviousPage() {
    window.history.back();
}

function close_alert()
{
    $('#pop-warning').hide();
    $('.POP-dialogue1').hide();
}

function ScrollToTop() {
    $("html").animate({ scrollTop: 0 }, 500);
}


function PreventDateEntry() {
    $('.datepicker').keypress(function () {
        return false;
    });
}