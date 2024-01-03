// var antiForgeryToken = '@Html.AntiForgeryToken()';
//var antiForgeryToken = $('#AntiForgeryToken').val();
//AntiForgeryTokenInclusionRequest(antiForgeryToken);

$(document).ready(function () {

    //$('#datepicker1').datepicker();
    //highlightHolidays();
    var holidaystring = $('#holidaystring').val();
    loadCalendar(holidaystring);
    $('body').on('click', '#holidayBtn', function (e) { e.preventDefault(); openHolidayCard(); });
    $('body').on('click', '#spalertclose', function (e) { e.preventDefault(); alertClose(); });
    $('body').on('click', '#btn_createholiday', function (e) { e.preventDefault(); Create(); });
    $('body').on('click', '#btn_createholiday', function (e) { e.preventDefault(); holidayupdateButton(); });
    /* $("body").on('click', '#holidayBtn', openHolidayCard)*/
    $('body').on('click', '.Holiday-Edit', function (e) {
        e.preventDefault();
        holidayedit(this);
    });
    $('body').on('click', '#btnalertclose', function (e) {
        e.preventDefault();
        alertClose(this);
    });
    $('body').on('click', '#filterimage', function (e) {
        e.preventDefault();
        ClearHoliday(this);
    });
    $("#btnsearch").on('mouseover', mouseoverSearch);
    $("#btnsearch").on('mouseout', mouseoutSearch);
    
    $("#Cancel").on('mouseover', mouseoutSearch);
    $("#Cancel").on('mouseout', mouseout);
    $('body').on('click', '#Cancel', function (e) {
        e.preventDefault();
        ClearHoliday(this);
    });
    /*$("body").on('click', '.Holiday-Edit', holidayedit);*/


});
$('body').on('click', '#holidayedit', function (e) {

    e.preventDefault();
    var holidayId = $(this).data('holidayid');
    var holidayDate = $(this).data('holidaydate');
    var countryId = $(this).data('countryid');
    var description = $(this).data('description');
    holidayeditButton(holidayId, holidayDate, countryId, description);
});

$('body').on('click', '#holidayDelete', function (e) {

    e.preventDefault();
    var holidayId = $(this).data('holidayid');
    var holidayDate = $(this).data('holidaydate');

    DeleteHoliday(holidayId, holidayDate);
});

function loadCalendar(holidaydatestring, onchange) {
    $.ajax({
        url: '../Holidays/showCalendar',
        type: 'POST',
        cache: false,
        async: false,
        data: { holidaystring: holidaydatestring, onchange: "" },
        beforeSend: function (data) {
        },
        success: function (data) {
            $("#toaddCalendarPartialView").html(data);
            showCalendarInit();
        },
        error: function (xhr, textStatus, errorThrown) {

        },
        complete: function () {

        }
    });
}


var holiday_id = 0;
var countryId = 0;
var holiday_date = null;
var selectedDate = null;
var holiday_description = null;
var count = 0;



document.getElementById('editholiday').style.display = 'none';
document.getElementById('holidayCardId').style.display = 'none';

$(document).ready(function () {
    SelectMenu(7);
    $('#btn_createholiday').css('padding', 'none');
    document.getElementById('editholiday').style.display = 'none';
    document.getElementById('holidayCardId').style.display = 'none';
    $('#SelectDateId').datepicker({
        inline: true,
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd/mm/yy',
        beforeShowDay: function (date) {
            var theday = (date.getDate() + 1) + '/' + date.getMonth() + '/' + date.getFullYear();
            return [true, $.inArray(theday) >= 0 ? "specialDate" : ''];
        }
    });
    $('#SelectDateIdEdit').datepicker({
        inline: true,
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd/mm/yy',
        beforeShowDay: function (date) {
            var theday = date.getDate() + '/' + (date.getMonth() + 1) + '/' + date.getFullYear();
            return [true, $.inArray(theday) >= 0 ? "specialDate" : ''];
        }
    });
});




// showing user-setting inside vertical menu
function showuserinfo() {
    if (document.getElementById('user-info').style.display !== "none") {
        document.getElementById('user-info').style.display = "none"
    }
    else {
        document.getElementById('user-info').style.display = "block";
        document.getElementsById('userdetails').style.overFlow = "scroll";
    }
}
// showing user-setting-info-filter
// showing filter-settings

// opening the card

function openHolidayCard() {

    document.getElementById('editholiday').style.display = 'none';
    document.getElementById('calenderId').style.display = 'none';
    document.getElementById('holidayCardId').style.display = 'block';
}

function closeHolidayCard() {
    document.getElementById('calenderId').style.display = 'block';
    document.getElementById('holidayCardId').style.display = 'none';
}

function DeleteHoliday(holi_id, holi_date) {
    holiday_id = holi_id;
    holiday_date = holi_date;
    if (count == 0) {
        var Msg = 'Do you want to delete holiday on date' + holiday_date + ' ?';
        $('#deletePopup').modal('hide');
        ShowWarningPopup(Msg, "DeleteHolidayConfirm", '');
    }
    else {
        showWarningPopDialog('Pending update, please finish and continue...', 'Ok', '', 'WarningCancelBtn', 'LoadList', 1, 'warning');
    }
}

function DeleteHolidayConfirm() {
    var Message = 'Holiday on date ' + holiday_date + ' deleted successfully';
    $.ajax({
        url: '../Holidays/DeleteHolidayDetails',
        type: 'POST',
        cache: false,
        async: false,
        data: { holidayId: holiday_id, holidayDate: holiday_date },
        beforeSend: function () {

        },
        success: function (result) {

            if (result.Success == true) {
                $('#WarningPopup').modal('hide');
                ShowSuccessModalPopup(Message, "ToReload");

                //$('#msgDeletePopup').modal('show');

            }
            else if (result.Success == false) {
                alert("Deletion failed");
            }
        },
        error: function (result) {
            alert("Deletion failed");
        }
    });
}
function load() {
    location.reload();
}
function alertClose() {
    ShowWarningPopup("Do you want to discard the changes?", 'load')

}
function LoadList() {
    if (flagcheck == 1) {
        var monthYear = $('#holidaystring').val();
        var flag = 1;
    }
    else {
        var monthYear = null;
        var flag = 0;
    }
    $.ajax({
        url: '../Holidays/HolidayCalender',
        type: 'GET',
        data: {
            flag: flag, MonthYear: monthYear, page: 1, pageSize: $('#pageSizeVal').val(),
            sortType: $('#filters #SortTypeValue').val(), sortOrder: $('#filters #SortOrderValue').val()
        },
        beforeSend: function () {

            startAnimation();
        },
        success: function (result) {
            fix_tableheader();
            $('#list_calender_tbl').html($(result).find('#list_calender_tbl').html());

        },
        error: function (xhr, textStatus, errorThrown) {
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function LoadHoliday(isClear = false) {
    var monthYear = null;
    var flag = 2;
    var searchType = 0;

    $.ajax({
        url: '../Holidays/HolidayCalender',
        type: 'GET',
        data: {
            flag: flag, MonthYear: monthYear, searchType: searchType, page: 1, pageSize: $('#pageSizeVal').val(),
            sortType: $('#filters #SortTypeValue').val(), sortOrder: $('#filters #SortOrderValue').val(), isClear: isClear
        },
        beforeSend: function () {

            startAnimation();
        },
        success: function (result) {
            //fix_tableheader();
            //$('#list_calender_tbl').html($(result).find('#list_calender_tbl').html());
            $('#holiday-calendar').html($(result).find('#holiday-calendar').html());

        },
        error: function (xhr, textStatus, errorThrown) {
        },
        complete: function () {
            stopAnimation();
        }
    });
}
function holidayCancelButton() {
    showWarningPopDialog('Do you want to discard changes?', 'No', 'Yes', 'WarningCancelBtn', 'DiscardChange', 1, 'warning');

}
function DiscardChange() {
    location.reload();
}

function ToReload() {
    //$('#MonthYear').val($('#MonthYear').val());
    location.reload();
}
function Create() {
    var selectedDate = $("#SelectDateId").val();
    var Description = $("#DescriptionId").val();

    $('#btn_createholiday').keypress(function (evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode;
        if (charCode == 32) {
            ShowSuccessModalPopup("Holiday on this date already exists", '')

            return true;
        }
    });

    if (selectedDate != '' && selectedDate != null && ValidateTextInput(Description, true)) {
        $("#errorCreate").hide();
        createHoliday();
    }
    else {
        $("#errorCreate").show();
    }
}


function createHoliday() {
    $('#editholiday').hide();
    var selectedDate = $("#SelectDateId").val();
    var Description = $("#DescriptionId").val();
    var CountryName = $('#CountrySelectId').val();
    $.ajax({
        url: '../Holidays/InsertHolidayDetails',
        type: 'POST',
        cache: false,
        async: false,
        data: { HolidayDate: selectedDate, DESCRIPTION: Description, COUNTRY_ID: CountryName },
        beforeSend: function () {
        },
        success: function (data) {
            if (data.result.check == 0) {
                ShowSuccessModalPopup('Holiday already added edit the holiday instead of adding new holiday', "ToReload")
            }
            if (data.result.check == 1) {

                ShowSuccessModalPopup('Holiday on date  ' + selectedDate + ' added succesfully', "ToReload");
                $("#overlay").hide();
            }
            if (data.result.check == 2 && selectedDate != null && selectedDate != " ") {
                $('#alreadyAdded').modal('hide');

                ShowSuccessModalPopup('Holiday already added edit the holiday instead of adding new holiday', "ToReload");
                $("#dialogue").show();
                $("#overlay").show();

                $("#overlay").show();
            }
        },
        error: function (xhr, textStatus, errorThrown) {
        },
        complete: function () {
            $('#overlay').hide();
            addscroll();
            resetdialogue();


        }
    });
}

function holidayeditButton(holidayId, holidayDate, countryId, description) {
    document.getElementById('calenderId').style.display = 'none';
    document.getElementById('holidayCardId').style.display = 'none';
    document.getElementById('editholiday').style.display = 'block';
    $("#SelectDateIdEdit").val(holidayDate);
    $("#DescriptionIdEdit").val(description);
    $("#CountrySelectIdEdit").val(countryId);
    $("#holidayId").val(holidayId);
}
function mouseoverSearch() {
    document.getElementById("btnSearch").style.color = "white";
    document.getElementById("btnSearch").style.backgroundColor = "#275795";
}

function mouseoutSearch() {
    document.getElementById("btnSearch").style.color = "#275795";
    document.getElementById("btnSearch").style.backgroundColor = "white";
}
function holidayupdateButton() {
    startAnimation();
    holidayId = $("#holidayId").val();
    var holidayDate = $("#SelectDateIdEdit").val();
    var description = $("#DescriptionIdEdit").val();
    var countryId = $("#CountrySelectIdEdit").val();

    if (holidayDate == "") {
        holidayDate = $("#SelectDateIdEdit").val();
    }

    if (ValidateTextInput(description, true)) {
        $('#errorEdit').hide();
        var Msg = 'Do you want to update holiday on date ' + holidayDate + ' ?';
        ShowWarningPopup(Msg, "updateholiday");
        stopAnimation();
    }
    else {
        $('#errorEdit').show();
        stopAnimation();
    }
}

function updateholiday() {
    startAnimation();
    holidayDate = $("#SelectDateIdEdit").val();
    description = $("#DescriptionIdEdit").val();
    countryId = $("#CountrySelectIdEdit").val();
    count = 0;
    var Msg = 'Holiday on date ' + holidayDate + ' updated successfully .';

    $.ajax({
        url: '../Holidays/EditHolidayDetails',
        type: 'POST',
        cache: false,
        async: false,
        data: { holidayId: holidayId, holidayDate: holidayDate, description: description, countryId: countryId },
        beforeSend: function () {

        },
        success: function (data) {
            stopAnimation();
            if (data.result.check == 0) {
                // ShowWarningPopup('Select date from calendar.', 'Ok', '', 'ToReload', 'LoadList', 1, 'info');
                //ShowWarningPopup('Holiday on this date ' + holidayDate + ' already present in list.', 'ToReload', 'ToReload');
                $('#WarningPopup').modal('hide');
                ShowSuccessModalPopup('Holiday on this date ' + holidayDate + ' already present in list', "ToReload");


            }
            if (data.result.check == 1) {

                $('#msgUpdatePopup').modal('hide');
                $('#WarningPopup').modal('hide');
                ShowSuccessModalPopup(Msg, "ToReload");
            }
            if (data.result.check == 2) {
                $('#' + holiday_id).show();
                $('#msgUpdatePopup').modal('hide');

                $('#WarningPopup').modal('hide');
                ShowSuccessModalPopup('Holiday on this date ' + holidayDate + ' already present in list', "ToReload");
            }
        },
        error: function (xhr, textStatus, errorThrown) {
        },
        complete: function () {
            //location.reload();
        }
    });
}


function loadListChange(nextMonth, nextYear) {
    var MonthYear = nextMonth + "/" + nextYear;
    $.ajax({

        url: '../Holidays/HolidayCalender',
        type: 'POST',
        cache: false,
        async: false,
        data: { flag: 1, MonthYear: MonthYear },
        success: function (result) {
        },
        error: function (xhr, textStatus, errorThrown) {
            location.reload();
        },
        complete: function () {

        }
    });

}

function getAdjacentMonth(curr_month, curr_year, direction) {
    var theNextMonth;
    var theNextYear;
    if (direction == "next") {
        theNextMonth = (curr_month + 1) % 12;
        theNextYear = (curr_month == 11) ? curr_year + 1 : curr_year;
    } else {
        theNextMonth = (curr_month == 0) ? 11 : curr_month - 1;
        theNextYear = (curr_month == 0) ? curr_year - 1 : curr_year;
    }
    return [theNextMonth, theNextYear];
}




function changeCalendar(direction) {
    var monthYear = $('#MonthYear').val();
    var splitString = monthYear.split("/");
    var adjacentMonthYearArray = getAdjacentMonth(parseInt(splitString[0]) - 1, parseInt(splitString[1]), direction);
    var adjacentMonthYear = (adjacentMonthYearArray[0] + 1) + "/" + adjacentMonthYearArray[1];
    $('#MonthYear').val(adjacentMonthYear);
    $.ajax({

        url: '../Holidays/HolidayCalender',
        type: 'GET',
        data: {
            flag: 1, MonthYear: adjacentMonthYear, searchType: $('#SearchString').val(), page: 1, pageSize: $('#pageSizeVal').val(),
            sortType: $('#filters #SortTypeValue').val(), sortOrder: $('#filters #SortOrderValue').val()
        },
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
            $('#holiday-calendar').html($(result).find('#holiday-calendar').html());
            var holidaysList = [];
            $("#holiday-calendar tr").each(function () {
                var valueOfCell = $(this).find('.holidaydate').text();
                var bool = valueOfCell != '';
                if (bool) {
                    var date = valueOfCell.split('-')
                    holidaysList.push(date[0]);
                }
            });
            var holidays = holidaysList;
            highlightDate(holidays);
        },
        error: function (xhr, textStatus, errorThrown) {
        },
        complete: function () {
            stopAnimation();
        }
    });
}

function getMonthYear() {
    var monthYear = $('#MonthYear').val();
    var splitString = monthYear.split("/");
    return [splitString[0] - 1, splitString[1]];
}

/*function getMonth() {
    var monthYear = $('#MonthYear').val();
var splitString = monthYear.split("/");
return parseInt(splitString[0] - 1);
}

function getYear() {
    var monthYear = $('#MonthYear').val();
var splitString = monthYear.split("/");
return parseInt(splitString[1]);
}*/

function highlightHolidays() {
    var holidaysDateList = $('#HolidayDateArray').val();
    var holidays = holidaysDateList.split(',');
    if (!$.isEmptyObject(holidays)) {
        var dateElements1 = datesBody1.find('div');

        for (var d in holidays) {
            dateElements1.each(function (index) {
                if (parseInt($(this).text()) == holidays[d]) {
                    $(this).addClass('selected');
                }
            });
        }
    }
}

function highlightDate(holidays) {
    if (!$.isEmptyObject(holidays)) {
        var dateElements1 = datesBody1.find('div');

        for (var d in holidays) {
            dateElements1.each(function (index) {
                if (parseInt($(this).text()) == holidays[d]) {
                    $(this).addClass('selected');
                }
            });
        }
    }
}
function ClearHoliday() {
    $('#DDDsearchHoliday').prop('selectedIndex', 0);
    //document.getElementById("DDDsearchHoliday").selectedIndex = 0;
    $('#SearchString').val('');
    LoadHoliday(true);
}
function mouseover() {
    document.getElementById("Cancel").style.color = "white";
    document.getElementById("Cancel").style.backgroundColor = "#275795";
}

function mouseout() {
    document.getElementById("Cancel").style.color = "#275795";
    document.getElementById("Cancel").style.backgroundColor = "white";
}
$('body').on('change', '.PageSize-HolidayCalender #pageSizeSelect', function () {
    var page = getUrlParameterByName("page", this.href);
    $('#pageNum').val(page);
    var pageSize = $(this).val();
    $('#pageSizeVal').val(pageSize);
    GetHolidayTableItems(isSorting = true);
});
var sortTypeGlobal = 1;//0-asc
var sortOrderGlobal = 2;//date
function CalendarSort(event, param) {
    sortOrderGlobal = param;
    sortTypeGlobal = event.classList.contains('sorting_desc') ? 0 : 1;
    $('#SortTypeValue').val(sortTypeGlobal);
    $('#SortOrderValue').val(sortOrderGlobal);
    //getMonthYear();
    GetHolidayTableItems(isSorting = true);
}

function GetHolidayTableItems(isSorting = false) {
    var date = $("#datepicker").datepicker("getDate");
    var MonthYear = $.datepicker.formatDate("mm/yy", date);
    var searchType = $('#SearchString').val();
    flagcheck = 1;
    $.ajax({

        url: '../Holidays/HolidayCalender',
        type: 'GET',
        data: {
            flag: 1, MonthYear: MonthYear, searchType: searchType, page: (isSorting ? $('#PageNum').val() : 1), pageSize: $('#pageSizeVal').val(),
            sortType: $('#filters #SortTypeValue').val(), sortOrder: $('#filters #SortOrderValue').val()
        },
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
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
        },
        error: function (xhr, textStatus, errorThrown) {
        },
        complete: function () {
            stopAnimation();
        }
    });

}

$('body').on('click', '.table-pagination a', function (e) {
    e.preventDefault();
    e.stopPropagation();
    var page = getUrlParameterByName("page", this.href);
    $('#PageNum').val(page);
    GetHolidayTableItems(true);//using sorting as true to avoid page reset
});
