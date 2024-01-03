$(function () {
});

function ParseDate(date) {
    var regexDate = date.replace(new RegExp("/", "g"), "-");
    //var splitdate = regexDate.split('-');
    
    //var convertedDate = splitdate[0] + "-" + GetMonth(parseInt(splitdate[1])) + "-" + splitdate[2];
    //var newdate = new Date(convertedDate);

    var sample = new Date(regexDate.replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3"));
    var newdate = new Date(sample);
    return newdate;
}

function GetMonth(month) {
    var strMonth;
    switch (month) {
        case 1:
            strMonth = "Jan";
            break;
        case 2:
            strMonth = "Feb";
            break;
        case 3:
            strMonth = "Mar";
            break;
        case 4:
            strMonth = "Apr";
            break;
        case 5:
            strMonth = "May";
            break;
        case 6:
            strMonth = "Jun";
            break;
        case 7:
            strMonth = "Jul";
            break;
        case 8:
            strMonth = "Aug";
            break;
        case 9:
            strMonth = "Sep";
            break;
        case 10:
            strMonth = "Oct";
            break;
        case 11:
            strMonth = "Nov";
            break;
        case 12:
            strMonth = "Dec";
            break;
        default:
            strMonth = "";
            break;
    }
    return strMonth;
}