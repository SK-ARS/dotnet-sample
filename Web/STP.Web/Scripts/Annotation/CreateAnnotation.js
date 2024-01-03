////var ViewBagObject = $('#ViewBagObject').val();
////var ViewBagValue = JSON.parse(ViewBagObject);
var annotationContactList = [];
var annot_obj;
function CreateAnnotationInit() {
    Resize_PopUp(580);
    $("#dialogue").draggable({ handle: ".head" });
    $("#dialogue").show();
    $("#overlay").show();
    $("#annotText").focus();
    //////////$('#annot_contact_table').find('tr:eq(2)').find('td:eq(1)').text("ffffj");
    if ($('#hf_editmode').val() == 1) {

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
}


$(document).ready(function () {



    $('body').on('click', '.deletecontact', function (e) {

        RemoveContactfn(this);
    });
    $("#span-close").on('click', closeAnotation);
    $("#InsertAnnot").on('click', InsertAnnotation);
    
    $('body').on('click', '#btnImportAnnotation', function (e) {
        ImportFromAnnotationLibrary();
    });
});

function setcontactTabledata(obj) {
    var count = obj.annotationContactList.length;
    for (var i = 1; i <= count; i++) {

        $('#tableSOA').find('tr:eq(' + i + ')').find('td:eq(' + 0 + ')').text(obj.annotationContactList[(i - 1)].orgName);
        $('#tableSOA').find('tr:eq(' + i + ')').find('td:eq(' + 1 + ')').text(obj.annotationContactList[(i - 1)].phoneNumber);
    }
}

var Annotcontactcount = 0;
$('body').on('click', '#addcontact', function (e) {
    if ($('#hf_editmode').val() == 1) {
        Annotcontactcount = annot_obj.annotationContactList.length;
        annotationContactList = annot_obj.annotationContactList;
    }
    Annotcontactcount++;
    if (Annotcontactcount > 5) {
        Annotcontactcount--;
        return false;
    }
    else {
        Resize_PopUp(900);
        //$('#annottaiondiv').hide();
        $('#dialogue').hide();
        $("#contactpopup").load('../Notification/PopUpAddressBook?origin=' + "annotation" + '&count=' + Annotcontactcount + '&fromAnnotation=' + true, function () {
            PopUpAddressBookinit();
        });
        //$("#contactpopup").show();

    }
});


$('body').on('click', '#closeannotation', function (e) {
    $("#overlay").css('display', 'none');
    //$('#dialogue1').hide();
    $('#dialogue').hide();
});


$('body').on('click', '#InsertAnnot', function (e) {
    InsertAnnotation();
});

function InsertAnnotation() {
    var insertToLibrary = 0;

    var annotationType = $("#annotType").val();
    var annotationText = $("#annotText").val();

    if (annotationType == 3)
        annotationType = 250003;
    else if (annotationType == 2)
        annotationType = 250002;
    else if (annotationType == 1)
        annotationType = 250001;

    if ($('#NoteToLibrary').is(":checked") && annotationText) {
        
        $.ajax({
            type: "GET",
            url: '/Annotation/SaveAnnotationToLibrary',
            contentType: "application/json; charset=utf-8",
            data: { "annotationType": annotationType, "annotationText": annotationText, "structureId": 0 },
            datatype: "json",
            success: function (response) {
                
                if (response.success) {
                    ShowSuccessModalPopup("Annotation Text Saved Successfully in Library.", '');
                }
                else {
                    ShowErrorPopup("Annotation Text Already Exists in Library.");
                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("Dynamic content load failed." + errorThrown);
            }
        });

    }




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
    else {
        var type = e.selectedIndex + 250000;//converting to db type
    }
    if ($('#hf_editmode').val() == 1) {
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
    if (document.getElementById('MoveVerVehicleandRoutes') != null && document.getElementById('MoveVerVehicleandRoutes') != undefined) {
        let rtv = document.getElementById('MoveVerVehicleandRoutes');
        if (rtv.className.includes('active-card')) {
            $('#btnmovsaveannotation').show();
        }
    }
    $('#dialogue').hide();
    $("#dialogue").html(' ');
    $('#overlay').hide();
    $('body').css("overflow", "scroll");
    Annotcontactcount = 0;
    annotationContactList = [];
}


function removecontact(a) {
    Annotcontactcount--;
    var tr = $(a).closest('tr');
    var row_index = $(tr).index();
    tr.remove();

    var str = "<tr><td class='table-soa soptable-soa stHeading1 text1'><input class='edit-normal sopedit- normal' type='text' name='from'></td><td class='table-soa soptable-soa stHeading1 text1'><input class='edit-normal sopedit- normal' type='text' name='from'></td><td><img src='/Content/assets/images/delete-icon.svg' width='15' onclick='removecontact(this)'></td></tr>"
    $('#tableSOA tbody').append(str);
    annotationContactList.splice(row_index, 1);
}

function RemoveContactfn(e) {
    removecontact(e);
}


function ImportFromAnnotationLibrary() {
    $('#filters').remove();
    var options = { "backdrop": "static", keyboard: true };
    var id = 0;
    $.ajax({
        type: "GET",
        url: '/Annotation/GetAnnotationsFromLibrary',
        contentType: "application/json; charset=utf-8",
        data: { "pageNumber": 1, "pageSize": 10, "annotationType": 0, "annotationText": "", "structureId": 0 },
        datatype: "json",
        beforeSend: function () {
            openContentLoader('#annottaiondiv .modal-content');
        },
        success: function (data) {
            $('#filters').remove();
            $("#annotationtextpopup").css('display', 'block');
            $('#annotationtextpopup').html(data);
            $('#annotationtextpopup').modal(options);
            $("#annottaiondiv").css('display', 'none');
            $("#annotationTextmodal").css('display', 'block');
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("Dynamic content load failed." + errorThrown);
        },
        complete: function () {
            closeContentLoader('#annottaiondiv .modal-content');
        }
    });


}
