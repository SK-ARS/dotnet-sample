  $("#btnExport").click(function () {
    var monthValue = $('#DDStartMonth').val();
    var yearValue = $('#DDStartYear').val();
    var userType = $("#DDUserType").val();

    var link = "../Report/ReportPerUserExportToCSV" + EncodedQueryString("startMonth=" + monthValue + "&startYear=" + yearValue + "&userType=" + userType);
    window.location.href = link;
  });

