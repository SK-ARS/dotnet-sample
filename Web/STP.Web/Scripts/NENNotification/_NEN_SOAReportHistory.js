function ExportReport() {
    var monthValue = $('#DDStartMonth').val();
    var yearValue = $('#DDStartYear').val();
    var link = "../NENNotification/NENSOAReportExportToCSV" + EncodedQueryString("startMonth=" + monthValue + "&startYear=" + yearValue);
    window.location.href = link;

}
$('body').on('click', '#div_export_soareport #btnExportSOAReport', function (e) { e.preventDefault(); ExportReport(); });
