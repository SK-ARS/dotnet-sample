    var annotationContactList = [];
    var annot_obj;
  $(document).ready(function () {
         Resize_PopUp(580);
        $("#dialogue").draggable({ handle: ".head" });
        $("#dialogue").show();
        $("#overlay").show();
        $("#annotText").focus();
        //////////$('#annot_contact_table').find('tr:eq(2)').find('td:eq(1)').text("ffffj");
if($('#hf_editmode').val() ==  1)
        {

            $("#dynatitle").html("View Annotation");
            annot_obj = getAnnotationObject();
            type = annot_obj.annotType;
            annotationContactList = annot_obj.annotationContactList;
           $('#annotText').html(annot_obj.annotText);
            if (annot_obj.annotType == 250001)
                $('#annotType').val(1);
            else if (annot_obj.annotType == 250002)
                $('#annotType').val(2);
            else
                $('#annotType').val(3);
            setcontactTabledata(annot_obj);

        }

        $("#span-close").on('click', closeAnotation);
        $(".deletecontact").on('click', RemoveContactfn);
        $("#InsertAnnot").on('click', InsertAnnotation);
        $("#btnImportAnnotation").on('click', ImportFromAnnotationLibrary);
        
  });

    function setcontactTabledata(obj) {
        var count = obj.annotationContactList.length;
        for (var i = 1; i <= count; i++) {

            $('#tableSOA').find('tr:eq(' + i + ')').find('td:eq(' + 0 + ')').text(obj.annotationContactList[(i - 1)].orgName);
            $('#tableSOA').find('tr:eq(' + i + ')').find('td:eq(' + 1 + ')').text(obj.annotationContactList[(i - 1)].phoneNumber);
        }
    }

  var count = 0;
    $('#addcontact').click(function (e) {
if($('#hf_editmode').val() ==  1) {
          count = annot_obj.annotationContactList.length;
          annotationContactList = annot_obj.annotationContactList;
      }
        count++;
        if (count > 5) {
            count--;
            return false;
        }
        else {
            Resize_PopUp(900);
            $('#dialogue').hide();
            $("#contactpopup").load('../Notification/PopUpAddressBook?origin=' + "annotation" + '&count=' + count + '&fromAnnotation=' + true);
            $("#contactpopup").show();
        }
      // startAnimation();
      
    });
function onAnnotationWindowClose(annotText, annotType, annotationContactList) {
    debugger
    objifxStpMap.setRoutePointAtXY({ pointType: 'ANNOTATION', pointPos: 0, X: annotPosition.x, Y: annotPosition.y, type: annotPosition.type, searchInBbox: false, Zoomin: false }, function (annotObject) {
        if (annotObject != undefined && annotObject != null) {
            annotObject.annotationContactList = annotationContactList;
            annotObject.annotType = annotType;
            annotObject.annotText = annotText;
            objifxStpMap.addAnnotation(annotObject.otherinfo.pathIndex, annotObject.otherinfo.segmentIndex, -1, annotObject);
            if (objifxStpMap.getCurrentPathState() == 'routedisplayed') {
                objifxStpMap.routeManager.setRoutePathState(objifxStpMap.currentActiveRoutePathIndex, 'routeplanned');
                updateUI();
            }
        }
        else {
            ShowErrorPopup('Failed to create annotation');
        }
        stopAnimation();
    });

}
    function InsertAnnotation() {
      $("#AT_validatn").html("");
      if ($("#annotText").val() == "") {
          $("#AT_validatn").html("Annotation Text is required.");
          $("#AT_validatn").show();
          return;
      }
      var e = document.getElementById("annotType");
      if (e.selectedIndex == 0) {
          var type = e.selectedIndex + 250003;//converting to db type
      }
      else{
          var type = e.selectedIndex + 250000;//converting to db type
      }
if($('#hf_editmode').val() ==  1) {
            annot_obj.annotText = $("#annotText").val();
            annot_obj.annotType = type;
            annot_obj.annotationContactList = annotationContactList;
            $('#btn_updateRoute').show();
        }
        else {
          onAnnotationWindowClose($('#annotText').val(), type, annotationContactList);
        }
        if (objifxStpMap.pageType == 'DISPLAYONLY_EDITANNOTATION')
        $('#btnmovsaveannotation').show();
        $('#dialogue').hide();
        $("#dialogue").html(' ');
        $('#overlay').hide();
        $('body').css("overflow", "scroll");
    }
    function removecontact(a) {
        count--;
        var tr = $(a).closest('tr');
          var row_index = $(tr).index();
        tr.remove();

        var str = "<tr><td class='table-soa soptable-soa stHeading1 text1'><input class='edit-normal sopedit- normal' type='text' name='from'></td><td class='table-soa soptable-soa stHeading1 text1'><input class='edit-normal sopedit- normal' type='text' name='from'></td><td><img src='/Content/assets/images/delete-icon.svg' width='15' id='imgRemoveContact'></td></tr>"
        $('#tableSOA tbody').append(str);
        $("#imgRemoveContact").on('click', RemoveContactfn);
          annotationContactList.splice(row_index,1);
      }


        var options = { "backdrop": "static", keyboard: true };
        var id = 0;
        $.ajax({
            type: "GET",
            url: '/Annotation/GetAnnotationsFromLibrary',
            contentType: "application/json; charset=utf-8",
            data: { "pageNumber": 1, "pageSize": 10, "annotationType": 0, "annotationText": "","structureId":0},
            datatype: "json",
            success: function (data) {
                debugger;
                $("#annotationtextpopup").css('display', 'block');
                $('#annotationtextpopup').html(data);             
                $('#annotationtextpopup').modal(options);         
                $("#annottaiondiv").css('display', 'none');
                $("#annotationTextmodal").css('display', 'block');


            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("Dynamic content load failed." + errorThrown);
            }
        });


    }
    function RemoveContactfn(e) {
        removecontact(e);
    }

        var options = { "backdrop": "static", keyboard: true };
        var id = 0;
        $.ajax({
            type: "GET",
            url: '/Annotation/GetAnnotationsFromLibrary',
            contentType: "application/json; charset=utf-8",
            data: { "pageNumber": 1, "pageSize": 10, "annotationType": 0, "annotationText": "","structureId":0},
            datatype: "json",
            success: function (data) {
                debugger;
                $("#annotationtextpopup").css('display', 'block');
                $('#annotationtextpopup').html(data);             
                $('#annotationtextpopup').modal(options);         
                $("#annottaiondiv").css('display', 'none');
                $("#annotationTextmodal").css('display', 'block');


            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("Dynamic content load failed." + errorThrown);
            }
        });


    }
    function RemoveContactfn(e) {
        removecontact(e);
    }

