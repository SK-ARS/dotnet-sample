/*==========================================
Plugin    : AutocompleteControl
Developer : NIDHIN V V
Date      : 14-06-2014
Doc Type  : Js
==========================================*/

$(function () {
    $('body').on('keyup','.AutocompleteElement', function (e)
    {
      
        $(".disableClearButton").attr("disabled", true);//disable clear button

        var classname = $(this).parent().attr("class");
        if (classname)
            classname = classname.substring(0, 15);
        else
            classname = "";
        if (classname != "searchtableleft") {
            var offsettop = $(this).offset().top;
            var Element_height = $(this).css("height");
            offsettop = (parseInt(offsettop) + parseInt(Element_height) + parseInt(72));
            var offsetleft = null;
        }
        else {
            var offsettop = $(this).offset().top;
            console.log("leftpanel");
            var Element_height = $(this).css("height");
            offsettop = (parseInt(offsettop) + parseInt(Element_height) + parseInt(72));
            var offsetleft = $(this).offset().left;
        }
        //var wd = parseInt($(this).css("width")) + 10;
        var wd = 350;
        var height = parseInt(Element_height) ;
        var checkpopupornot = $('body').find('#outline_tbl_route_details').length;
        if (e.keyCode != 38 && e.keyCode != 40) {
            $(this).parent().find('#route_search_popup').hide();
            $(document).find("#Map_View").css("z-index", 0);
            $(document).find("#map").css("z-index", 0);
            $(document).find("#wraper_leftpanel_content").css("overflow", 'auto');
            }
        if (e.keyCode == 13) {
            var text = $.trim($(this).val());
            if (text[0] == '$') {
                debugSearch(text);
            }
            else {
                if (checkpopupornot == 1) {
                    var offsettop = $(this).offset().top;
                    offsettop = offsettop + parseInt(height) + 76;
                }
                var self = $(this);
                $('#route_search_popup').remove();     
                CreateAutocompleteElementNotification($(this), offsetleft);
                CreateAutocompleteElement($(this), function () {
                    self.parent().find('#route_search_popup').css({ "top": offsettop - 61, "left": offsetleft, "width": wd });
                    self.parent().find('#route_search_popup').show();
                    if ($(document).find("#Map_View").length > 0) {
                        $(document).find("#Map_View").css("z-index", -1);
                        $(document).find("#map").css("z-index", -1);
                        $(document).find("#wraper_leftpanel_content").css("overflow", 'inherit');
                    }
                    self.parent().find('#newList').css({ "top": offsettop - 65});
                });
            }
        }
    });

    //for selecting values using arrow keys 
    var chosen = "";
    scrolled = 0;
    $(document).keydown(function (e) {
        //$('.route_search_popup').show();// 38-up, 40-down
        if (e.keyCode == 40) {
            if (chosen === "") {
                chosen = 0;
            } else if ((chosen + 1) < $('.route_search_popup').find('#newList').find('li').length) {
                chosen++;
            }
            //else { chosen = 0; }
            $('.route_search_popup').find('#newList').find('li').removeClass('Auto_selected');
            $('.route_search_popup').find('#newList').find('li:eq(' + chosen + ')').addClass('Auto_selected');
           // if (chosen>=2 && chosen % 2 == 0) {
                scrolled = scrolled + 65;
                $(".route_search_popup").animate({
                    scrollTop: scrolled
                });

           // }
            return false;
        }
        if (e.keyCode == 38) {
            if (chosen === "") {
                chosen = 0;
            } else if (chosen > 0) {
                chosen--;
            }
            //else { chosen = $('.route_search_popup').find('#newList').find('li').length - 1; }
            $('.route_search_popup').find('#newList').find('li').removeClass('Auto_selected');
            $('.route_search_popup').find('#newList').find('li:eq(' + chosen + ')').addClass('Auto_selected');
           // if (chosen >= 2 && chosen % 2 == 0) {
                scrolled = scrolled - 65;
                $(".route_search_popup").animate({
                    scrollTop: scrolled
                });

            //}
            return false;
        }
        //if (e.keyCode == 13) {
        //    if ($(document).find('#route_search_popup').find('#newList').length > 1) {
        //      
        //    }
        //}
        if (e.keyCode == 27) {
            $(".disableClearButton").removeAttr('disabled');

            $(this).parent().find('#route_search_popup').hide();
            $(document).find("#Map_View").css("z-index", 0);
            $(document).find("#map").css("z-index", 0);
            $(document).find("#wraper_leftpanel_content").css("overflow", 'auto');
        }
        $('#wraper_leftpanel_content').mousedown(function (event) {
            if (event.which) {
              
                $(".disableClearButton").removeAttr('disabled');
            }

        });
    });
    $('body').on('click', '.ifx-auto-complete-click', function (e) {
        AutoClickEvent(this);
    });

});
var origin
function getPoint(moniker, callback) {
    var point = {};
    $.ajax({
        url: '../QAS/GetAddress',
        type: 'POST',
        async: true,
        data: { moniker: moniker },
        beforeSend: function () {
            // startAnimation();
            if (origin == "outline")
                $('#div_saving').show();
            else
                $('#search_anim').show();
        },
        success: function (result) {

            if (callback && typeof (callback) === "function") {
                callback(result);
            }
        },
        complete: function () {
            if (origin == "outline")
                $('#div_saving').hide();
            else
                $('#search_anim').hide();

        }
    });
}

function formatString(first) {
    var strlen = first.length;
    var display_string = " ";
    for (var i = 0; i < strlen - 1; i++) {
        if (i < 3) {
            var temp = i;
            display_string = display_string + first[i] + ',' ;
        }
        display_string =  '<br>' + display_string + first[i] + '.' ; 
    }

    return display_string;

}
var picklist;

function addList(x, result) {
    $(x).parent().append("<div id='route_search_popup' class='route_search_popup'></div>");
    $('#route_search_popup').append("<ul class='qas-autofill-ul' id='newList'></ul>");
    for (var cnt = 0; cnt < result.length; cnt++) {
        var String = result[cnt].AddressLine;         
        $("#newList").append("<li class='pl-4 pr-2 pt-2 pb-2 ifx-auto-complete-click'><div><span class='text-normal'>" + String + "</span></div></li>");
    }
}
function ValidXY(x, Easting, Northing) {
    if (Easting == null || Easting == 0 || Northing == null || Northing == 0) {
        Error_showNotification(x, "Address not found ");
        return false;
    }
    return true;
}
function onCompleteSearch(result, x, autocompletecallback) {
    picklist = result;
    picklist.details = x;
    if (result.length > 0) {
              
        if (result.length == 1) {
            point = getPoint(picklist[0].Moniker, function (result) {
                var pointNo = $(picklist.details).attr('pointNo');
                var pointType = $(picklist.details).attr('pointType');
                var str = $(picklist.details).attr('id');
                if (pointNo == undefined) {
                    pointNo = Number(str.substr(str.length - 1)) + 1;
                }
                else
                    pointNo = Number(pointNo);
                var origin = $(picklist.details).attr('origin');
                if (origin == 'a2bLeft') {
                    if(ValidXY(x,result.Easting, result.Northing)==true)
                    setRoutePoint(picklist[0].AddressLine, Number(pointNo), result.Easting, result.Northing, pointType);
                }
                else if (origin == 'simpleNotif') {
                    if (ValidXY(x, result.Easting, result.Northing) == true)
                        setRoutePointNotif(picklist[0].AddressLine, Number(pointNo), result.Easting, result.Northing);
                    var input_id = '#' + str;
                    $(input_id).val(picklist[0].AddressLine);
                }
                else if (origin == 'outline') {
                    var e = document.getElementById("Pointtype");
                    var index = e.selectedIndex;
                    $("#L_validation").html('');
                    if (result.Northing == null || result.Northing == 0 || result.Easting == null || result.Easting == 0) {
                        if (index == 0)
                            $("#L_validation").html("Enter valid start location.");
                        else
                            $("#L_validation").html("Enter valid end location.");
                    }
                    else
                        setRoutePointSoApp(picklist[0].AddressLine, index, result.Northing, result.Easting);

                }
                else {
                    var input_id = '#' + str;
                    $(input_id).val(picklist[0].AddressLine);
                }
                if ($("#hIs_NEN").val() == "true") {
                    $('#' + str).css({ 'border': ''});
                }
                if (autocompletecallback && typeof (autocompletecallback) === "function") {
                    autocompletecallback();
                }
            });
        }
        else {
            addList(x, result);
            if (autocompletecallback && typeof (autocompletecallback) === "function") {
                autocompletecallback();
            }
        }

    }
    else {
        Error_showNotification(x, "Address not found or too many results");
    }
}
//method for creating Autocomplete element
function CreateAutocompleteElement(x, autocompletecallback) {
   
    var point = {};
    var url = $(x).attr('url');
    var text = $.trim($(x).val());
     origin = $(x).attr('origin');
     $(x).parent().append("<div id='search_anim' class='search_anim'></div>");
    $.ajax({
        url: url,
        type: 'POST',
        async:true,
        data: { searchKeyword: text },
        beforeSend: function () {
           
            var url = $(x).parent().parent().attr('class');
            if (origin == "outline")
                $('#div_saving').show();
            else
                $('#search_anim').show();
        },
        success: function (result) {
            onCompleteSearch(result, x, autocompletecallback);
        },      
        error: function (xhr, textStatus, errorThrown) {
            Error_showNotification(x, errorThrown);
        },
        complete: function () {
            if (origin == "outline")
                $('#div_saving').hide();
            else
                $('#search_anim').remove();
                
    }
    });

    $(document).find("#Map_View").css("z-index", 0);
    $(document).find("#map").css("z-index", 0);
        $(document).find("#wraper_leftpanel_content").css("overflow", 'auto');

    //});
}

//for adding error span
function CreateAutocompleteElementNotification(x, offsetleft) {
    $(x).parent().find('.error').remove();
    var left = offsetleft;
    if (left > 5) { left = 0;}
    $(x).parent().append("<span class='error' style='margin: 0px 0px 4px 0px; display: block;'></span>");
    $(x).parent().find('.error').css("margin-left",left);
}

function AutoClickEvent(_this) {
    var s = $(_this).index();
    var a = $(_this).parent().parent().parent().find("input[type=text]").attr('id');
    var x = $(_this).parent().parent().parent();
    var input_id = '#' + a;
    $(input_id).val(jQuery.trim($(_this).text()));
    var moniker = picklist[s].Moniker;
    var str = $(picklist.details).attr('id');
    getPoint(moniker, function (point) {
        var pointType = 0;
        var origin = $(picklist.details).attr('origin');
        var pointNo = $(picklist.details).attr('pointNo');
        pointType = $(picklist.details).attr('pointType');
        pointNo = Number(pointNo);
        if (picklist[s].AddressLine == "Viridor Waste Management, Lean Quarry, Horningtops, LISKEARD, Cornwall, PL14 3QD") {
            point.Easting = 226877;
            point.Northing = 61300;
        }
        if (origin == 'a2bLeft') {
            if (ValidXY(x, point.Easting, point.Northing) == true)
                setRoutePoint(picklist[s].AddressLine, Number(pointNo), point.Easting, point.Northing, pointType);
        }
        else if (origin == 'simpleNotif') {
            if (ValidXY(x, point.Easting, point.Northing) == true)
            setRoutePointNotif(picklist[s].AddressLine, Number(pointNo), point.Easting, point.Northing);
        }
        else if (origin == 'outline') {
            var e = document.getElementById("Pointtype");
            var index = e.selectedIndex;
            $("#L_validation").html('');
            if (point.Northing == null || point.Northing ==0||point.Easting == null || point.Easting == 0) {
                if (index == 0)
                    $("#L_validation").html("Enter valid start location.");
                else
                    $("#L_validation").html("Enter valid end location.");
            }
            else
                setRoutePointSoApp(picklist[s].AddressLine, index, point.Northing, point.Easting);

        }

        else {
            var input_id = '#' + str;
            $(input_id).val(picklist[s].AddressLine);
           
        }
        if ($("#hIs_NEN").val() == "true") {
            $('#' + str).css({ 'border': '', 'background-color': '', 'color': 'white'});

            
            //if (VObj_err != null) {
            //    var errorCount, VElement = 'Waypoint', wayPindex = 1;
            //    if (str == 'To_location')
            //        VElement = 'endPoint';
            //    else if (str == 'From_location')
            //        VElement = 'endPoint';
            //    else{
            //        wayPindex = parseInt( str.substr(8));
            //    }
            //    errorCount = VObj_err.length;
            //    for (var i = 0 ; i < errorCount; i++) {
            //        if(VObj_err[i].point == VElement && VObj_err[i].point != 'wayPoint'  )
            //        {
            //            VObj_err[i].isSetCorrect = 1;
            //        }
            //        else if (VObj_err[i].add_index ==  wayPindex )
            //            VObj_err[i].isSetCorrect = 1;
            //        }
            //    }
            }
        
        $(".disableClearButton").removeAttr('disabled');
    });

    
}


