
        $(document).ready(function () {
            //addScroll();
            ;
            if ('@Model.SignedHeightStatus' == "251001") {

                $("#SignedHeightCheck").prop('checked', true);
            }
            else {

                $("#SignedHeightCheck").prop('checked', false);
            }
            if ('@Model.SignedLengthStatus' == "251001") {
                $("#SignedLenCheck").prop('checked', true);
            }
            else {
                $("#SignedLenCheck").prop('checked', false);
            }
            if ('@Model.SignedWidthStatus' == "251001") {
                $("#SignedWidthCheck").prop('checked', true);
            }
            else {
                $("#SignedWidthCheck").prop('checked', false);
            }
            if ('@Model.SignedGrossWeightStatus' == "251001") {
                $("#SignedGrossWeightCheck").prop('checked', true);
            }
            else {
                $("#SignedGrossWeightCheck").prop('checked', false);
            }
            if ('@Model.SignedAxleWeightStatus' == "251001") {
                $("#SignedAxleWeightCheck").prop('checked', true);
            }
            else {
                $("#SignedAxleWeightCheck").prop('checked', false);
            }

            if ('@Model.HALoading' == 1) {
                $("#HALoadingTrue").prop('checked', true);
                $("#HALoadingFalse").prop('checked', false);
            }
            else {
                $("#HALoadingFalse").prop('checked', true);
                $("#HALoadingTrue").prop('checked', false);
            }




            if ('@Model.SignedHeight.HeightMeter' == "" && '@Model.SignedHeight.HeightFeet' == "" && '@Model.SignedHeight.HeightInches' == "") {
                $("#SignedHeightNotKnownSigned").prop('checked', true);
                $("#Signed_Height_Meter, #Signed_Height_Feet, #Signed_Height_Inches").prop('readOnly', true);
                $("#SignedHeightCheck").prop('disabled', true);
            }
            else if ('@Model.SignedHeight.HeightMeter' == 0.0 && '@Model.SignedHeight.HeightFeet' == 0.0 && '@Model.SignedHeight.HeightInches' == 0.0) {
            $("#SignedHeightNoSignedConst").prop('checked', true);
            $("#Signed_Height_Meter, #Signed_Height_Feet, #Signed_Height_Inches").prop('readOnly', true);
            $("#Signed_Height_Meter, #Signed_Height_Feet, #Signed_Height_Inches").val("");
            $("#SignedHeightCheck").prop('disabled', true);
        }
        else {
            $("#SignedHeightMadeByEsdal").prop('checked', true);
            $("#Signed_Height_Meter, #Signed_Height_Feet, #Signed_Height_Inches").prop('readOnly', false);
            $("#SignedHeightCheck").prop('disabled', false);
        }




        @*if (('@Model.Signed_Width.WidthMeter' == "" && '@Model.Signed_Width.WidthFeet' == "") || ('@Model.Signed_Width.WidthMeter' == null && '@Model.Signed_Width.WidthFeet' == null)) {*@
            if ('@Model.SignedWidth.WidthMeter' == "" && '@Model.SignedWidth.WidthFeet' == "" && '@Model.SignedWidth.WidthInches' == "") {
                $("#SignedWidthNotKnownSigned").prop('checked', true);
                $("#Signed_Width_Meter, #Signed_Width_Feet, #Signed_Width_Inches").prop('readOnly', true);
                $("#SignedWidthCheck").prop('disabled', true);
            }
            else if ('@Model.SignedWidth.WidthMeter' == 0.0 && '@Model.SignedWidth.WidthFeet' == 0.0 && '@Model.SignedWidth.WidthInches' == 0.0) {
            $("#SignedWidthNoSignedConst").prop('checked', true);
            $("#Signed_Width_Meter, #Signed_Width_Feet, #Signed_Width_Inches").prop('readOnly', true);
            $("#Signed_Width_Meter, #Signed_Width_Feet, #Signed_Width_Inches").val("");
            $("#SignedWidthCheck").prop('disabled', true);
        }
        else {
            $("#SignedWidthMadeByEsdal").prop('checked', true);
            $("#Signed_Width_Meter, #Signed_Width_Feet, #Signed_Width_Inches").prop('readOnly', false);
            $("#SignedWidthCheck").prop('disabled', false);
        }




            if ('@Model.SignedLength.LengthMeter' == "" && '@Model.SignedLength.LengthFeet' == "" && '@Model.SignedLength.LengthInches' == "") {
                $("#SignedLenNotKnownSigned").prop('checked', true);
                $("#Signed_Length_Meter, #Signed_Length_Feet, #Signed_Length_Inches").prop('readOnly', true);
                $("#SignedLengthCheck").prop('disabled', true);
            }
            else if ('@Model.SignedLength.LengthMeter' == 0.0 && '@Model.SignedLength.LengthFeet' == 0.0 && '@Model.SignedLength.LengthInches' == 0.0) {
            $("#SignedLenNoSignedConst").prop('checked', true);
            $("#Signed_Length_Meter, #Signed_Length_Feet, #Signed_Length_Inches").prop('readOnly', true);
            $("#Signed_Length_Meter, #Signed_Length_Feet, #Signed_Length_Inches").val("");
            $("#SignedLengthCheck").prop('disabled', true);
        }
        else {
            $("#SignedLenMadeByEsdal").prop('checked', true);
            $("#Signed_Length_Meter, #Signed_Length_Feet, #Signed_Length_Inches").prop('readOnly', false);
            $("#SignedLengthCheck").prop('disabled', false);
        }




            if ('@Model.SignedGrossWeightObj.GrossWeight' == "" || '@Model.SignedGrossWeightObj.GrossWeight' == null) {
                $("#SignedGrossWeightNotKnownSigned").prop('checked', true);
                $("#Signed_Gross_Weight").prop('readOnly', true);
                $("#SignedGrossWeightCheck").prop('disabled', true);

            }
            else if ('@Model.SignedGrossWeightObj.GrossWeight' == 0.0) {
            $("#SignedGrossWeightNoSignedConst").prop('checked', true);
            $("#Signed_Gross_Weight").prop('readOnly', true);
            $("#Signed_Gross_Weight").val("");
            $("#SignedGrossWeightCheck").prop('disabled', true);
        }
        else {
            $("#SignedGrossWeightMadeByEsdal").prop('checked', true);
            $("#Signed_Gross_Weight").prop('readOnly', false);
            $("#SignedGrossWeightCheck").prop('disabled', false);
        }



            if ('@Model.SignedAxleWeight.AxleWeight' == "" || '@Model.SignedAxleWeight.AxleWeight' == null) {
                $("#SignedAxleWeightNotKnownSigned").prop('checked', true);
                $("#Signed_Axcel_Weight").prop('readOnly', true);
                $("#SignedAxleWeightCheck").prop('disabled', true);
            }
            else if ('@Model.SignedAxleWeight.AxleWeight' == 0.0) {
            $("#SignedAxleWeightNoSignedConst").prop('checked', true);
            $("#Signed_Axcel_Weight").prop('readOnly', true);
            $("#Signed_Axcel_Weight").val("");
            $("#SignedAxleWeightCheck").prop('disabled', true);
        }
        else {
            $("#SignedAxleWeightMadeByEsdal").prop('checked', true);
            $("#Signed_Axcel_Weight").prop('readOnly', false);
            $("#SignedAxleWeightCheck").prop('disabled', false);
        }



            $("#SignedHeightNotKnownSigned, #SignedHeightNoSignedConst").click(function () {
                $("#Signed_Height_Meter, #Signed_Height_Feet, #Signed_Height_Inches").prop('readOnly', true);
                $("#SignedHeightCheck").prop('disabled', true);
            });
            $("#SignedHeightMadeByEsdal").click(function () {
                $("#Signed_Height_Meter, #Signed_Height_Feet, #Signed_Height_Inches").prop('readOnly', false);
                $("#SignedHeightCheck").prop('disabled', false);
            });

            $("#SignedWidthNotKnownSigned, #SignedWidthNoSignedConst").click(function () {
                $("#Signed_Width_Meter, #Signed_Width_Feet, #Signed_Width_Inches").prop('readOnly', true);
                $("#SignedWidthCheck").prop('disabled', true);
            });
            $("#SignedWidthMadeByEsdal").click(function () {
                $("#Signed_Width_Meter, #Signed_Width_Feet, #Signed_Width_Inches").prop('readOnly', false);
                $("#SignedWidthCheck").prop('disabled', false);
            });

            $("#SignedLenNotKnownSigned, #SignedLenNoSignedConst").click(function () {
                $("#Signed_Length_Meter, #Signed_Length_Feet, #Signed_Length_Inches").prop('readOnly', true);
                $("#SignedLenCheck").prop('disabled', true);
            });
            $("#SignedLenMadeByEsdal").click(function () {
                $("#Signed_Length_Meter, #Signed_Length_Feet, #Signed_Length_Inches, #SignedLenCheck").prop('readOnly', false);
                $("#SignedLenCheck").prop('disabled', false);
            });

            //$("#SignedLenNotKnownSigned, #SignedLenNoSignedConst").click(function () {
            //    $("#Signed_Length_Meter, #Signed_Length_Feet, #Signed_Length_Inches, #SignedLenCheck").prop('disabled', true);
            //});
            //$("#SignedLenMadeByEsdal").click(function () {
            //    $("#Signed_Length_Meter, #Signed_Length_Feet, #Signed_Length_Inches, #SignedLenCheck").prop('disabled', false);
            //});

            $("#SignedGrossWeightNotKnownSigned, #SignedGrossWeightNoSignedConst").click(function () {
                $("#Signed_Gross_Weight").prop('readOnly', true);
                $("#SignedGrossWeightCheck").prop('disabled', true);
            });
            $("#SignedGrossWeightMadeByEsdal").click(function () {
                $("#Signed_Gross_Weight").prop('readOnly', false);
                $("#SignedGrossWeightCheck").prop('disabled', false);
            });

            $("#SignedAxleWeightNotKnownSigned, #SignedAxleWeightNoSignedConst").click(function () {
                $("#Signed_Axcel_Weight").prop('readOnly', true);
                $("#SignedAxleWeightCheck").prop('disabled', true);
            });
            $("#SignedAxleWeightMadeByEsdal").click(function () {
                $("#Signed_Axcel_Weight").prop('readOnly', false);
                $("#SignedAxleWeightCheck").prop('disabled', false);
            });


            $("#btnMaxWeightOvrDstAdd").prop('disabled', true);

            
if($('#hf_strucType' == 1 && ('@Model.MaxWeightOverMinDistanceWeight').val() ==  "" && '@Model.MaxWeightOverMinDistanceWeight' != null)) {
            var MaxWeightOverminDistWeight = '@Model.MaxWeightOverMinDistanceWeight';
            var MaxWeightOverminDistDistance = '@Model.MaxWeightOverMinDistanceDistance';

            var arrMaxWeightOverminDistWeight = MaxWeightOverminDistWeight.split(',');
            var arrMaxWeightOverminDistDistance = MaxWeightOverminDistDistance.split(',');

            for (var i = 0; i < arrMaxWeightOverminDistWeight.length; i++) {
                //var name = document.getElementsByName("spnRemove");
                rowID = i + 1; //(name.length) + 1;
                var lblRow = "HeightEnv" + (rowID);
                var DivRow = "weightover" + (rowID);

                
                $("#tabMaxWeightOvrDst").append("<div class='row mb-3' id='" + DivRow + "'><div class='col-lg-3 col-md-6 col-sm-12 edit-normal weight' style='width: 33%; padding-right: 0px; padding-left: 0px;'>" + arrMaxWeightOverminDistWeight[i] + "</div><div class='col-lg-3 col-md-3 col-sm-12 edit-normal distance' style='width: 33%; padding-right: 0px; padding-left: 0px;'>" + arrMaxWeightOverminDistDistance[i] + "</div><div class='col-lg-4 col-md-3 col-sm-12 edit-normal'><a class='text-normal-hyperlink a_deleteMaxWeight' id='" + lblRow + "' RowId=" + '"' + DivRow + '"' +"  style='text-decoration: underline;line-height: 18px;'>Remove</a></div></div>");
                $("#tabMaxWeightOvrDst").css("display", "block");

                $(".a_deleteMaxWeight").on('click', DeleteMaxWeight);
            }
        }



            // $("#btnSaveStructImposed").click(function () {
            $("#frmEditSTRUCT_IMPOSED").submit(function (e) {
                
if($('#hf_strucType').val() ==  1) {
                    var weight = "", distance = "";

                $('#tabMaxWeightOvrDst tr').each(function () {
                    weight = weight + $(this).find(".weight").html() + ",";
                    distance = distance + $(this).find(".distance").html() + ",";
                });
                if (weight.length > 0 && distance.length > 0) {
                    weight = weight.substring(0, (weight.length - 1));
                    distance = distance.substring(0, (distance.length - 1));
                }
                $('#MaxWeightOverminDist_Weight').val(weight);
                $('#MaxWeightOverminDist_Distance').val(distance);
            }

            return ValidationCheckStructImposed();
            //return false;
            });


            $("#btnMaxWeightOvrDstAdd").on('click', insRowMaxWeight);
            $("#btnsaveSVData1").on('click', saveSVData1);

        });


    function saveSVData1() {
        
        var weight = "", distance = "";

        $('#tabMaxWeightOvrDst').children('div').each(function () {
            //alert(this.value); // "this" is the current element in the loop

            weight = weight + $(this).find(".weight").html() + ",";
            distance = distance + $(this).find(".distance").html() + ",";

        });

        if (weight.length > 0 && distance.length > 0) {
            weight = weight.substring(0, (weight.length - 1));
            distance = distance.substring(0, (distance.length - 1));
        }
        $('#MaxWeightOverminDist_Weight').val(weight);
        $('#MaxWeightOverminDist_Distance').val(distance);
        var test = $("#frmEditSTRUCT_IMPOSED").serialize();

        $.ajax({
            url: '../Structures/StoreEditSTRUCT_IMPOSED',
            dataType: 'json',
            type: 'POST',
            data: $("#frmEditSTRUCT_IMPOSED").serialize(),
            success: function (result) {
                if (result == true) {
                    saveDimension();
                }

            },
            error: function (xhr, status) {
            }
        });
    }

       function saveDimension() {
        startAnimation();
        $.ajax({
            url: '../Structures/EditSTRUCT_IMPOSED',
            dataType: 'json',
            type: 'POST',
            data: {StructId : '@ViewBag.structureId', SectionId : '@ViewBag.sectionId' },
            success: function (result) {
                stopAnimation();
                if (result) {
                    ShowSuccessModalPopup("Structure imposed constraints saved successfully.", "ShowReviewSummary");
                }
                else {
                    ShowErrorPopup("Structure imposed constraints save failed.")
                }

            @*$("#generalSettingsId").load('../Structures/SVData?StructId=' + '@ViewBag.StructureId' + "&sectionId=" +  @ViewBag.sectionId + "&structName=" + '@ViewBag.structureName' + "&ESRN=" + '@ViewBag.ESRN',
                    function () {
                        $('#managedimensiosId').show();
                        stopAnimation();

                        if (result == true) {
                            ShowSuccessModalPopup('Structure general details saved successfully.', "");

                        }
                        else {
                            ShowWarningPopup("SVData saving failed");
                        }
                    }
                );*@

            },
            error: function (xhr, status) {
            }
        });
    }


        function ValidationCheckStructImposed() {

if($('#hf_strucType').val() ==  3) {
                if ($("#Vertical_Alignment_EntryDistance").val().length <= 0 && $("#Vertical_Alignment_EntryHeight").val().length <= 0 &&
                    $("#Vertical_Alignment_MaxHeighDistance").val().length <= 0 && $("#Vertical_Alignment_MaxHeighHeight").val().length <= 0 &&
                    $("#Vertical_Alignment_ExitDistance").val().length <= 0 && $("#Vertical_Alignment_ExitHeight").val().length <= 0) {

                    $("#errVerticalAlignment").text("");
                    return true;
                }
                else if ($("#Vertical_Alignment_EntryDistance").val().length > 0 && $("#Vertical_Alignment_EntryHeight").val().length > 0 &&
                    $("#Vertical_Alignment_MaxHeighDistance").val().length > 0 && $("#Vertical_Alignment_MaxHeighHeight").val().length > 0 &&
                    $("#Vertical_Alignment_ExitDistance").val().length > 0 && $("#Vertical_Alignment_ExitHeight").val().length > 0) {

                    $("#errVerticalAlignment").text("");
                    return true;
                }
                else {
                    $("#errVerticalAlignment").text("There must be an entry for all distance and height");
                    return false;
                }
            }
        }

        function ShowReviewSummary() {
            CloseSuccessModalPopup();
            window.location.href = "../Structures/ReviewSummary" + EncodedQueryString("structureId=" + '@ViewBag.StructureId');
    }

    function insRow() {
        // console.log('hi');
        var RowIndex = 1;

        var name = document.getElementsByName("spnRemove");
        rowID = (name.length) + 1;
        var lblRow = "HeightEnv" + (rowID);

        str = "<tr><td> " + $('#Offset').val() + " </td><td> " + $('#EnvelopeWidth').val() + " </td><td> " + $('#EnvelopeHeight').val() + " </td><td><button id='" + lblRow + "' RowId='" + lblRow + "' class='btnbdr btngrad btnrds btnDeleteRow' aria-hidden='true' data-icon='' type='button'>Remove</button></td></tr>";
        $("#tab").append(str);
        $("#Offset, #EnvelopeWidth, #EnvelopeHeight").val("");

        $(".btnDeleteRow").on('click', DeleteRowFn);
    }
    function deleteRow(rowID) {
        var tr = $("#" + rowID).closest('tr');
        tr.remove();
        //document.getElementById('tab').deleteRow(row);
    }


    $("#MaxWeightOverMinDistance_TonnesOver, #MaxWeightOverMinDistance_Meter").keyup(function () {
        var lenTonnes = $("#MaxWeightOverMinDistance_TonnesOver").val().length;
        var lenMeter = $("#MaxWeightOverMinDistance_Meter").val().length;
        if (lenTonnes > 0 || lenMeter > 0) {
            $("#btnMaxWeightOvrDstAdd").prop('disabled', false);
        }
        else {
            $("#btnMaxWeightOvrDstAdd").prop('disabled', true);
        }
    });

    //tabMaxWeightOvrDst
    function insRowMaxWeight() {

        var weight = "";
        

        $('#tabMaxWeightOvrDst').children('div').each(function () {
            //alert(this.value); // "this" is the current element in the loop
            weight = weight + $(this).find(".weight").html() + ",";

        });
        var words = weight.split(",");

        if (weight.length > 0 ) {
            weight = weight.substring(0, (weight.length - 1));
        }

        var flag = 0;

        var test1 = $("#MaxWeightOverMinDistance_TonnesOver").val().length;
        var test2 = $("#MaxWeightOverMinDistance_Meter").val().length;

        if ($("#MaxWeightOverMinDistance_TonnesOver").val().length < 0 && $("#MaxWeightOverMinDistance_Meter").val().length < 0) {
            flag = 1;
        }
        else if ($("#MaxWeightOverMinDistance_TonnesOver").val().length > 0 && $("#MaxWeightOverMinDistance_Meter").val().length > 0) {
            flag = 1;
        }
        else {
            flag = 0;
        }


        if (flag == 1) {
            var RowIndex = 1;

            var name = document.getElementsByName("spnRemove");
            rowID = (words.length) + 1;
            var lblRow = "HeightEnv" + (rowID);
            var DivRow = "weightover" + (rowID);

            str = "<tr><td class='weight'> " + $('#MaxWeightOverMinDistance_TonnesOver').val() + " </td><td class='distance'> " + $('#MaxWeightOverMinDistance_Meter').val() + " </td><td><button RowId='" + lblRow + "' id='" + lblRow + "' class='btnbdr btngrad btnrds a_deleteMaxWeight' aria-hidden='true' data-icon='' type='button'>Remove</button></td></tr>";

            $("#tabMaxWeightOvrDst").append("<div class='row mb-3' id='" + DivRow + "'><div class='col-lg-3 col-md-6 col-sm-12 edit-normal weight' style='width:33%; padding-right: 0px; padding-left: 0px;'>" + $('#MaxWeightOverMinDistance_TonnesOver').val() + "</div><div class='col-lg-3 col-md-3 col-sm-12 edit-normal distance' style='width:33%; padding-right: 0px; padding-left: 0px;'>" + $('#MaxWeightOverMinDistance_Meter').val() + "</div><div class='col-lg-4 col-md-3 col-sm-12 edit-normal'><a class='text-normal-hyperlink a_deleteMaxWeight' id='" + lblRow + "' RowId=" + '"' + DivRow + '"' +" style='text-decoration: underline;line-height: 18px;'>Remove</a></div></div>");
            $("#tabMaxWeightOvrDst").css("display", "block");

            $(".a_deleteMaxWeight").on('click', DeleteMaxWeight);

            $('#MaxWeightOverMinDistance_TonnesOver').val('');
            $('#MaxWeightOverMinDistance_Meter').val('');
            $("#errMaxWeightOvrDst").text("");
        }
        else {
            $("#errMaxWeightOvrDst").text("There must be an entry for weight and distance");
        }
    }
    function deleteRowMaxWeight(rowID) {
        
        $("#" + rowID).remove();


        //var tr = $("#" + rowID).closest('tr');
        //tr.remove();
        //document.getElementById('tab').deleteRow(row);
    }

    $("#btnCancel").click(function () {

        @*window.location.href = "../Structures/ReviewSummary?structureId=" + '@ViewBag.StructureId';*@
        var form = document.createElement("form");

        form.action = '@Url.Action("ReviewSummary", "Structures")';
        form.method = "post";

        input = document.createElement("input");
        input.name = "structureId";
        input.value  = $('#hf_value = '@ViewBag.StructureId').val(); 

        form.appendChild(input);

        document.body.appendChild(form);
        form.submit();
    });

    function DeleteMaxWeight(e) {
        var rowId = e.currentTarget.dataset.RowId;
        deleteRowMaxWeight(rowId);
    }
    function DeleteRowFn(e) {
        var rowId = e.currentTarget.dataset.RowId;
        deleteRow(rowId);
    }
