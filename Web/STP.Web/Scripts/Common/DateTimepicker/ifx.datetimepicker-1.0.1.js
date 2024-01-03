//datetimepicker
$(function () {

    $('#ToDateTime').datepicker({dateFormat: 'dd/mm/yy',

        //timeFormat: "H:mm",

        //hourMin: 0,
    
        //hourMax: 23,
      
        numberOfMonths: 1,
       
        changeMonth: true, changeYear: true,
        beforeShow: function (input, inst) {
            setTimeout(function () {
                inst.dpDiv.css({
                    top: $("#ToDateTime").offset().top + 35,
                    left: $("#ToDateTime").offset().left
                });
            }, 0);
        }
    });

    $('#FromDateTime').datepicker({
        dateFormat: 'dd/mm/yy',

        //timeFormat: "H:mm",

        //hourMin: 0,
      
        //hourMax: 23,
     
        numberOfMonths: 1,
        
        changeMonth: true, changeYear: true
    });
});

