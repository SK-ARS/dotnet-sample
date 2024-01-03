    $("#btnExport").click(function () { 
        var monthValue = $('#DDStartMonth').val();
        var yearValue = $('#DDStartYear').val();

        var link = "../Report/IndustryLiaisonExportToCSV" + EncodedQueryString("startMonth=" + monthValue + "&startYear=" + yearValue);
        window.location.href = link;
    }); 
