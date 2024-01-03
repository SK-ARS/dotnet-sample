    $("#btnExport").click(function () {
        var link = "../Report/CommunicationExportToCSV" + EncodedQueryString("startMonth=" + $("#hdnStartMonth").val() + "&startYear=" + $("#hdnStartYear").val());
        window.location.href = link;
    }); 
