    
    var month
    var year
    MonthYear = $("#MonthYear").val();
    var arry1 = MonthYear.split("/");
    var mm = arry1[0];
    var yy = arry1[1];
    $(document).ready(function ()
    {
        var holidaydatestring = $('#holidaystring').val();
        //var holidaydatestring = $('#holidaydatestring').val();
        addthisClasstoAll(holidaydatestring);
        $('#datepicker').datepicker("setDate", '01/' + mm + '/' + yy);
        //$('#datepicker').datepicker("setDate", + mm + '01/' + yy);
    });
    function addthisClasstoAll(holidaystring) {
        
        //$('#datepicker').datepicker('destroy');
        var datesArray = holidaystring.split(',');
        var month;
        var year;
        var Datestringarray = [];
        var Datestringarray1 = [];
        for (var i = 0; i < datesArray.length; i++) {
            if (datesArray[i] != '') {
                Datestringarray1.push(datesArray[i].replace(/\b0(?=\d)/g, ''));
                }
            }
        function loadHolidayList(changeYear, changeMonth) {
            
            var searchType = $('#SearchString').val();
            var v = parseInt(changeMonth);
            changeMonth = v < 10 ? "0" + changeMonth : changeMonth;
            var MonthYear = changeMonth + "/" + changeYear;
            $('#holidaystring').val(MonthYear);
            flagcheck = 1;
            //window.location.href ='../Holidays/HolidayCalender?flag=1&MonthYear=' + MonthYear + '&searchType=' + searchType;
            $.ajax({
                url: '../Holidays/HolidayCalender',
                type: 'POST',
                cache: false,
                async: false,
                data: {
                    flag: 1, MonthYear: MonthYear, searchType: searchType, page: 1, pageSize: $('#pageSizeVal').val(),
                    sortType: $('#filters #SortTypeValue').val(), sortOrder: $('#filters #SortOrderValue').val() },
                beforeSend: function () {


                },
                success: function (result) {
                    
                    //fix_tableheader();
                    $('#holiday-calendar').html($(result).find('#holiday-calendar').html());
                    Datestringarray = [];
                    Datestringarray1 = [];
                    var holidaystring = $(result).find('#holidaystring').val();
                    var datearray = holidaystring.split(',')
                    for (var i = 0; i < datearray.length; i++) {
                        if (datearray[i] != '') {
                            Datestringarray.push(datearray[i].replace(/\b0(?=\d)/g, ''));
                        }
                    }
                    //$('#datepicker').datepicker('refresh');
                },
                error: function (xhr, textStatus, errorThrown) {
                    location.reload();
                },
                complete: function () {

                }
            });

            // var url = '/Holidays/HolidayCalender?flag=1&MonthYear=' + MonthYear ; 
            //window.location.href = url;
        }

        $('#datepicker').datepicker({
            inline: true,
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy',

            onChangeMonthYear: function (changeYear, changeMonth, widget) {
                
                //if (count == 0) {
                    month = changeMonth;
                    year = changeYear;
                    loadHolidayList(changeYear, changeMonth);
                $(this).datepicker("setDate", '01/' + changeMonth + '/' + changeYear);
                //$(this).datepicker("setDate",  changeMonth + '/01/' + changeYear);
                //}
                //else {
                   // showWarningPopDialog('Click on update button to update your Holiday.', 'Ok', '', 'WarningCancelBtn', 'LoadList', 1, 'warning');
                //}
            },
            beforeShowDay: function (date) {
                
                var theday = date.getDate() + '/' + (date.getMonth() + 1) + '/' + date.getFullYear();
                 //var theday = date.getDate() + '-' + (date.getMonth() + 1) + '-' + date.getFullYear(); 
                if (Datestringarray1.length > 0) {

                    return [true, $.inArray(theday, Datestringarray1) >= 0 ? "specialDate" : ''];
                }
                return [true, $.inArray(theday, Datestringarray) >= 0 ? "specialDate" : ''];
                // $(this).datepicker('refresh');
            }
        });
        $('.specialDate').find('a').addClass('test');
    }

