  $(document).ready(function () {
    selectedmenu('Reports');
  });
    $('body').on('click', '.showsession', function (e) {

        e.preventDefault();
        var usertype = $(this).data('usertype');

        ShowSessionLengthDetails(usertype);
    });
  $("#btnExport").click(function () {

      var link = "../Report/SessionLengthTypeWiseExportToCSV" + EncodedQueryString("month=" + $("#hdnMonth").val() + "&year=" + $("#hdnYear").val());
      window.location.href = link;
  });

  function ShowSessionLengthDetails(userType) {

    $.ajax({
      url: '../Report/SessionLengthHistory',
      type: 'POST',
      data: { userType: userType, month: $("#hdnMonth").val(), year: $("#hdnYear").val() },
      beforeSend: function () {
        startAnimation();
      },
      success: function (html) {
        $('#SessionLengthReport').html(html);
      },
      error: function () {
        location.reload();
      },
      complete: function () {
        stopAnimation();
      }
    });
  }
