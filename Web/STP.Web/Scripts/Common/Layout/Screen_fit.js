/*
=========================================
Code for : PERFEFECT SCREEN FIT
Developer : NIDHIN V V
Date : 31-12-2013
=========================================

* The script will change the all the css properties used in this web portal

* All changes are based on the #wrapper_center width

*/

var window_height = $(window).height();
var min_height = parseInt(window_height - (100));
function setsize(wrapper_width)
{
    
		var window_height = $(window).height();
		var min_height = parseInt(window_height - (100));
		var headerheight = $("#headersection").height();
		min_height = parseInt(window_height - (headerheight + 47));
		var contentwidth = $("#content").width();
		var calenderTablewidth = parseInt($("#rightpanel").width()) - parseInt($("#calender").width());
		calenderTablewidth = calenderTablewidth - 40;

		$("#calenderTable").css({ "width": calenderTablewidth });

       	$('#map').css({ "width": '99.8%', "height": min_height});
		$('.tab_content').find('#map').css({ "width": '99.8%', "height": min_height - 185 });
		$('#RoutePart').find('#divmap').css({ "position": 'relative', "top": 5 });
		$('#RoutePart').find('#divmap').find('#map').css({ "width": '99.8%', "height": min_height});
		$('#div_layout_body').find('#map').css({ "width": '99.8%', "height": min_height-2, "padding": 0 });
		$("#Map_View").css({ "top": headerheight+5 });
		if ($("#quick_links").length >= 1) {
		    var quick_height = $("#quick_links").height();
		    var height = window_height - (quick_height + 142);
		    $("#wraper_leftpanel_content").css({ "height": height, "padding-bottom": 0 });
		}
		else {
		    var height = window_height - 142;
		    $("#wraper_leftpanel_content").css({ "height": height, "padding-bottom": 0 });
		}
		$('#tab_4 > #map').css({ "height": window_height - 281 });
		if ($("#pageheader > h3").text().length < 2) {
		    $("#pageheader").hide();
		    //$('#CreateRoute').find("#Map_View").css({ "top": 140 });
		    //$('#div_layout_body').find('#map').css({ "width": '99.8%', "height": min_height - 84, "padding": 0 });
		   
		}
		holidaycalendertextbox();
		//else {
		//    $("#Map_View").css({ "top": 98 });
		    
		//    $("#pageheader").hide();
		//}
		
		//if ($('#newsbaroutter').is(':visible')) { 
		//    $('#Map_View').css({ "top": 133 });
		//    $('#div_layout_body').find('#map').css({ "width": '99.8%', "height": min_height - 78, "padding": 0 });
		//    if ($("#pageheader > h3").text().length > 2) {
		//        $("#Map_View").css({ "top": 176 });
		//        $('#div_layout_body').find('#map').css({ "width": '99.8%', "height": min_height - 121, "padding": 0 });

		//    }
		//}


		
		if ($('#leftpanel').html() == '') {
		    $('#leftpanel').css({ "margin-bottom": 0 });
		}
		else {
		    $('#leftpanel').css({ "margin-bottom": 50 });
		}
		
		//var x = fix_tableheader();
		//if (x == 1) {
		//    $('#tableheader').css("margin-top", headerheight);
		//    $('#tableheader').show();
		//}
		return false;
	}
function setvalue(value)
	{
		var a = value;
		
		if( a > 1024 ){return  a;}
		else{ a = 1024; return  a; }
    	
		
	}
function applysize()
{
    
        var wrapper_width = $('#wrapper').width();			
        if (wrapper_width = setvalue(wrapper_width)) {
            setsize(wrapper_width);
        }
       
	    return false;
}


function Map_size_fit()
{
    var window_height = $(window).height();
    $('#Map_View').css({ "position": 'inherit',"margin-top":5 });
    $('#tab_4').find('#map').css({ "height": window_height - 281 });
    console.log(window_height - 231);
}



// code related to news bar
function shownewsbar() {
    $('#newsbaroutter').show();
    $("#Map_View").css({ "top": 133 });
    $('#div_layout_body').find('#map').css({ "width": '99.8%', "height": min_height - 126, "padding": 0 });
    //$('#pageheader').css({ "top": 127 });
    //$('#content').css({ "margin-top": 33 });
    $('#content').css({ "margin-top": 27 });
    $('#tab_4 > #map').css({ "height": window_height - 312 });
   }

// code related to news bar
function hidenewsbar() {
    $('#newsbaroutter').hide();
    //$('#pageheader').css({ "top": 97 });
    $('#content').css({ "margin-top": '' });
    }

//function for heding
function SetHeading(x)
{
    //if (x.length > 90) {var x = x.substring(0, 85) + '...';}
    //return x;
    
}

//function for setting dimentions for holiday calendar text boxes 
function holidaycalendertextbox() {
    var width = $("#calenderTable").width();
    $("#holidayList_table").css({ "width": width});
    width = width - 460;
    
    $("#holidayList_table").find('th').eq(0).css({ "width": width - 40 });
    $("#holidayList_table").find('th').eq(3).css({ "width": 240 });
    $(".holidaydescrption").css({ "width": width});
    $(".holidaydateText").css({ "width": 98 });
    $(".holidaycontryText").css({ "width": 98 });
    
}

//function for  resizing popup
function Resize_PopUp(x) {
    var width = parseInt(x);
    $("#dialogue").css("width", width);
    $("#dialogue").find('.head').css("width", width-10);
    $("#dialogue").find('.body').css("width", width-20);
}