//datetimepicker
$(function () {

   

    $('#ToDateTime').datetimepicker({dateFormat: 'dd/mm/yy',
  
        hourMin: 0,
    
        hourMax: 23,
      
        numberOfMonths: 1,
       
        changeMonth: true, changeYear: true
    });

    $('#FromDateTime').datetimepicker({dateFormat: 'dd/mm/yy',
   
        hourMin: 0,
      
        hourMax: 23,
     
        numberOfMonths: 1,
        
        changeMonth: true, changeYear: true
    });
});

