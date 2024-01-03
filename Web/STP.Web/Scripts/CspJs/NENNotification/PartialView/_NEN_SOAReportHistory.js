  function ExportReport() {
      var monthValue = $('#DDStartMonth').val();
        var yearValue = $('#DDStartYear').val();

      var link = "../NENNotification/NENSOAReportExportToCSV" + EncodedQueryString("startMonth=" + monthValue + "&startYear=" + yearValue);
        window.location.href = link;

  } 
