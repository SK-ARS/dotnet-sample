/*
==============================
Developer : NIDHIN VINCENT
Doc Type  : JAVASCRIPT
Date 	  : 28-1-2014
==============================
*/
var title = '';
var title1 = '';
var tab_id = 0;
var li_pos = 0;
var txtchange = false;
var tabname = "";
$(document).ready(function () {
    //title = $('#div_layout_body').find('h3').eq(0).text();
    title1 =  $('.sorthead').text();
    if ($('#tab_wrapper').find('.t').length > 0) { set_tab(); }
    if ($("#2MoveVer").hasClass("active-card")) {
        $('.sorthead').text(title1 + ' - Movement Summary');
    }
    if ($("#3general").hasClass("active-card")) {
        $('.sorthead').text(title1 + ' - General');
    }    

    //working of tabs goes here
    $(".t").bind("click", function () {
        
        $('.slidingpanelstructures_content').remove(); 
        $('.slidingpanelnav').remove();
        //code for checking text change
        var vr1flag = $("#vr1appln").val();
        if (vr1flag == "true" || vr1flag == "True") {
                txtchange = $("#TextChangeFlagVR1").val();
        }
        else {
            txtchange = $("#TextChangeFlagSO").val();
        }
        if (txtchange == "true" || txtchange == "True") {
            tab_id = $(this).attr("id");
            li_pos = $(this).index();
            tabname = $(this).find('.tab_centre').text();
            showWarningPopDialog('You have changes to be saved, do you want to save the changes?', 'No', 'Yes', 'switchtab', 'hideleftpanel', 1, 'warning');
        }
        else {
            //$(".t").each(function () {
            //    $(this).addClass("nonactive");

            //});
            $(".tab_content1").each(function () {
                $(this).hide();
            });
            id = $(this).attr("id");
            $("#tab_" + id).show();
            //$(this).removeClass("nonactive");

            $('.sorthead').text(title1 + ' - ' + $(this).find('.tab_centre').text());
        }
    });

});

//function set_title() {
//    var prjheader = "";
//    if (VR1Applciation == "True")
//        if (hauliermnemonic != "" && esdalref != "") {
//            prjheader = hauliermnemonic + "/" + esdalref + " - VR1 - " + prjstatus;
//        } else {
//            prjheader = "Apply for VR-1";
//        }
//    else
//        if (hauliermnemonic != "" && esdalref != "") {
//            prjheader = hauliermnemonic + "/" + esdalref + " - SO - " + prjstatus;
//        } else {
//            prjheader = "Apply for SO";
//        }
//    $('.mainhead').html('');
//    $('.sorthead').html('');
//    $('.sorthead').append(prjheader);
//}

// function for setting tab
function set_tab() {
    hidetabs();
    $('#tab_1').show();
    //$('#tab_wrapper ul li').eq(0).removeClass('nonactive');
    var test = title + '-' + $('#tab_wrapper ul li').eq(0).find('.tab_centre').text();
    $('#pageheader').find('h3').text(test);
    
}

//functin for hiding all tabs
function hidetabs() {
   

    $(".tab_content1").each(function () {
        $(this).hide();
    });
    //$('.t').addClass('nonactive');
    
    
}

function switchtab() {
    WarningCancelBtn();
    $("#TextChangeFlagSO").val(false);
    $("#TextChangeFlagVR1").val(false);
    //$(".t").each(function () {
    //    $(this).addClass("nonactive");

    //});
    $(".tab_content1").each(function () {
        $(this).hide();
    });
    $("#tab_" + tab_id).show();
    //$(".t").eq(li_pos).removeClass("nonactive");

    $('.sorthead').text(title + ' - ' + tabname);
}

function hideleftpanel() {
    WarningCancelBtn();
    $("#leftpanel").html('');
    $("#tab_" + tab_id).hide();
}