    $(document).ready(function () {
        $(".docsearch").on('change', DocumentSerach);
        $(".documntsearch").on('change', DocumentSerach);
        if ($('#haulierportal').val() == "True") {
            SelectMenu(7);
        }
        else {
            SelectMenu(5);
        }
    });

    function DocumentSerach(thisVal) {
        var documentTypeId = $(thisVal).val();
        var documentType = $("#DDsearchCriteria option:selected").text();
        var IMF = {
            SearchColumn: documentType,
            SearchValue: documentTypeId
        };

        $.ajax({
            async: false,
            type: "POST",
            url: '../Information/ViewDocumentList',
            data: { IMF: IMF  },
            beforeSend: function () {
                startAnimation();
            },
            success: function (response) {
                var result = $(response).find('section#banner').html();
                $('section#banner').html(result);

            },
            error: function (result) {
            },
            complete: function () {
                stopAnimation();
            }
        });
    }
