$(function () {  
    
    $(".datetimepicker").datetimepicker({
        dateFormat: 'dd/mm/yy',

        timeFormat: "H:mm",

        hourMin: 0,

        hourMax: 23,

        numberOfMonths: 1,

        minDate: 0

    });
    $('input[control-type="datetimepicker"]').datetimepicker({
        dateFormat: 'dd/mm/yy',

        timeFormat: "H:mm",

        hourMin: 0,

        hourMax: 23,

        numberOfMonths: 1,

        minDate: 0

    });

    $('input[control-type="datetimepicker_min"]').datetimepicker({
        dateFormat: 'dd/mm/yy',

        timeFormat: "H:mm",

        hourMin: 0,

        hourMax: 23,

        numberOfMonths: 1,

        minDate: new Date()
        
    });
});