	$(document).ready(function () {
		if (mapaddresssearch != undefined && mapaddresssearch != "") {
			$("#txtAddressSearch").val(mapaddresssearch);
		}
        $("#table-head").on('click', viewMapFilter);
	});
