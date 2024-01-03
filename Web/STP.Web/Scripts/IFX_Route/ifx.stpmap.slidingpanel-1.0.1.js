var affectedStructTimeFlag = 0, allStructTimeFlag = 0, affectedConstTimeFlag = 0, allConstTimeFlag = 0;
var affectedStructArr = null;
var allStructArr = null;
var structPanTimeFlag = 0;
var showAnimation = true;
var unsuitableStructTimeFlag = 0, unsuitableConstTimeFlag = 0;
var unsuitableStructArr = null;
//function for loading the slidingpanel dynamically.
function load_Structureslidingpanel() {
    var html = "<div class='slidingpanelstructures_content slidingpanelstructuresclose'>" +
                "<form>" +
                    "<fieldset style='border: 1px solid'>" +
                        "<legend></legend>" +
                        "<table class='table' style='margin-left: 7px;'>" +
                            "<tbody>" +
                                "<tr>" +
                                    "<td id='AffectedLabel' style='opacity: 0.5;'>Affected <input id='Affected' name='Affected' type='checkbox' value='true' disabled='true'><input name='Affected' type='hidden' value='false'></td>" +
                                "</tr>" +
                                "<tr>" +
                                    "<td>All <input id='All' name='All' type='checkbox' value='true'><input name='All' type='hidden' value='false'></td>" +
                                "</tr>" +
                            "</tbody>" +
                        "</table>" +
                    "</fieldset>" +
                "</form>" +

                "<form>" +
                    "<fieldset style='border: 1px solid;'>" +
                        "<legend></legend>" +
                        "<table class='table' style='margin-left: 7px;'>" +
                            "<tbody>" +
                                "<tr>" +
                                    "<td>Structures <input id='Structs' name='Structs' type='checkbox' value='true'><input name='Structs' type='hidden' value='false'></td>" +
                                "</tr>" +
                                "<tr>" +
                                    "<td>" +
                                        "<table class='table' style='margin-left: 20px; margin-top: -5px; margin-bottom: -5px'>" +
                                            "<tbody><tr>" +
                                                "<td>Underbridge <input id='Underbridge' name='Underbridge' type='checkbox' value='true'><input name='Underbridge' type='hidden' value='false'></td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>Overbridge <input id='Overbridge' name='Overbridge' type='checkbox' value='true'><input name='Overbridge' type='hidden' value='false'></td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>Under and over bridge <input id='UnderAndOverbridge' name='UnderAndOverbridge' type='checkbox' value='true'><input name='UnderAndOverbridge' type='hidden' value='false'></td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>Level crossing <input id='LevelCrossing' name='LevelCrossing' type='checkbox' value='true'><input name='LevelCrossing' type='hidden' value='false'></td>" +
                                            "</tr></tbody></table>" +
                                     "</td>" +
                                "</tr>" +
                                "<tr>" +
                                    "<td>Constraints <input id='Constraints' name='Constraints' type='checkbox' value='true'><input name='Constraints' type='hidden' value='false'></td>" +
                                "</tr></tbody></table></fieldset></form></div>" +
    "<span class='slidingpanelnav' style=''>Show/hide structures<span data-icon='&#xe111;' style='top: 20px !important; float: right; color: #ffffff;' class='intelli_accordian_show'></span></span></div>";
    $("#slidingpanelstructures").html(html);
}
//function for initializing structuresliding panel functionalities
function init_structureslidingpanel() {
    $(".slidingpanelnav").toggle(function () {
        $(this).parent().find('.slidingpanelstructures_content').removeClass("slidingpanelstructuresclose").addClass("slidingpanelstructuresopen");
        $(this).find("span").removeClass("intelli_accordian_show").addClass("intelli_accordian_hide");
        $(this).css({ "margin-top": 7 });
    }, function () {
        $(this).parent().find('.slidingpanelstructures_content').removeClass("slidingpanelstructuresopen").addClass("slidingpanelstructuresclose");
        $(this).find("span").removeClass("intelli_accordian_hide").addClass("intelli_accordian_show");
        $(this).css({ "margin-top": 0 });
    });

    if ($(".slidingpanelstructures_content").html().length > 1) {
        if ($("#btnCreate").length > 0) {
            if ($(".quick_tbl").html().length > 0) {
                $("#btnCreate").css({ "margin-top": 21, "margin-bottom": 6 });
            }
        }
        else {
            $(".quick_tbl").css({ "margin-top": 21 });
        }

        if ($(".slidingpanelnav").find(".slidingpanelstructuresclose").length >= 1) { $(".slidingpanelnav").css({ "margin-top": 0 }); }
    }

    $('#Affected').change(function () {
        showAnimation = true;
        showStructBoundsA2B();
    });

    $('#All').change(function () {
        if (this.checked) {
            var boundsAndZoom = getCurrentBoundsAndZoom();
            if (boundsAndZoom.zoom < 7 && (document.getElementById('Structs').checked == true || document.getElementById('Constraints').checked == true)) {
                showNotification("Zoom-in to view structures/constraints");
                //$('#overlay').show();
                return;
            }
        }
        else {
            if (document.getElementById('Affected').checked == false) {
                document.getElementById('Structs').checked = false;
                document.getElementById('Underbridge').checked = false;
                document.getElementById('Overbridge').checked = false;
                document.getElementById('UnderAndOverbridge').checked = false;
                document.getElementById('LevelCrossing').checked = false;
                document.getElementById('Constraints').checked = false;
            }
        }
        showAnimation = true;
        showStructBoundsA2B();
    });

    $('#Structs').change(function () {
        if (this.checked) {
            document.getElementById('Underbridge').checked = true;
            document.getElementById('Overbridge').checked = true;
            document.getElementById('UnderAndOverbridge').checked = true;
            document.getElementById('LevelCrossing').checked = true;
            if (document.getElementById('Affected').checked == false) {
                if (document.getElementById('All').checked == undefined) {
                    document.getElementById('All').checked = true;
                }
                var boundsAndZoom = getCurrentBoundsAndZoom();
                if (boundsAndZoom.zoom < 7 && (document.getElementById('Affected').checked == true || document.getElementById('All').checked == true)) {
                    showNotification("Zoom-in to view structures/constraints");
                    //$('#overlay').show();
                    return;
                }
            }
        }
        else {
            document.getElementById('Underbridge').checked = false;
            document.getElementById('Overbridge').checked = false;
            document.getElementById('UnderAndOverbridge').checked = false;
            document.getElementById('LevelCrossing').checked = false;
        }
        showAnimation = true;
        showStructBoundsA2B();
        //$('#overlay').show();
    });

    $('#Constraints').change(function () {
        if (this.checked && document.getElementById('Affected').checked == false) {
            var boundsAndZoom = getCurrentBoundsAndZoom();
            if (boundsAndZoom.zoom < 7 && (document.getElementById('Affected').checked == true || document.getElementById('All').checked == true)) {
                showNotification("Zoom-in to view structures/constraints");
                return;
            }
        }
        showAnimation = true;
        showStructBoundsA2B();
    });

    $('#Underbridge').change(function () {
        if (this.checked) {
            var boundsAndZoom = getCurrentBoundsAndZoom();
            if (boundsAndZoom.zoom < 7 && ( document.getElementById('All').checked == true)) {
                showNotification("Zoom-in to view structures/constraints");
                return;
            }
            document.getElementById('Structs').checked = true;
        }
        else {
            if (document.getElementById('Overbridge').checked == false &&
            document.getElementById('UnderAndOverbridge').checked == false &&
            document.getElementById('LevelCrossing').checked == false){
        document.getElementById('Structs').checked = false;
    }
        }
        showAnimation = true;
        showStructBoundsA2B();
    });

    $('#Overbridge').change(function () {
        if (this.checked) {
            var boundsAndZoom = getCurrentBoundsAndZoom();
            if (boundsAndZoom.zoom < 7 && (document.getElementById('All').checked == true)) {
                showNotification("Zoom-in to view structures/constraints");
                return;
            }
            document.getElementById('Structs').checked = true;
        }
        else {
            if (document.getElementById('Underbridge').checked == false &&
                document.getElementById('UnderAndOverbridge').checked == false &&
                document.getElementById('LevelCrossing').checked == false) {
                document.getElementById('Structs').checked = false;
            }
        }
        showAnimation = true;
        showStructBoundsA2B();
    });

    $('#UnderAndOverbridge').change(function () {
        if (this.checked) {
            var boundsAndZoom = getCurrentBoundsAndZoom();
            if (boundsAndZoom.zoom < 7 && ( document.getElementById('All').checked == true)) {
                showNotification("Zoom-in to view structures/constraints");
                return;
            }
            document.getElementById('Structs').checked = true;
        }
        else {
            if (document.getElementById('Underbridge').checked == false &&
                document.getElementById('Overbridge').checked == false &&
                document.getElementById('LevelCrossing').checked == false) {
                document.getElementById('Structs').checked = false;
            }
        }
        showAnimation = true;
        showStructBoundsA2B();
    });

    $('#LevelCrossing').change(function () {
        if (this.checked) {
            var boundsAndZoom = getCurrentBoundsAndZoom();
            if (boundsAndZoom.zoom < 7 && ( document.getElementById('All').checked == true)) {
                showNotification("Zoom-in to view structures/constraints");
                return;
            }
            document.getElementById('Structs').checked = true;
        }
        else {
            if (document.getElementById('Underbridge').checked == false &&
                document.getElementById('Overbridge').checked == false &&
                document.getElementById('UnderAndOverbridge').checked == false) {
                document.getElementById('Structs').checked = false;
            }
        }
        showAnimation = true;
        showStructBoundsA2B();
    });
}
//function for show structures slidingpanel
function structureslidingpanel_show() {
    $(".slidingpanelstructures").removeClass("hide").addClass("show");
}
//function for hide structures slidingpanel
function structureslidingpanel_hide() {
    $(".slidingpanelstructures").removeClass("show").addClass("hide");
}
function structPanChangedA2B() {
    structPanTimeFlag = 1;
}

function emptyStructureArray() {
     affectedStructArr = null;
     allStructArr = null;
}

function clearStructFlags() {
    affectedStructTimeFlag = 0;
    affectedConstTimeFlag = 0;
    unsuitableStructTimeFlag = 0;
    unsuitableConstTimeFlag = 0;
}
function showStructBoundsA2B() {
    if ($('#IsMovListAvalabl').val() != "Yes") {
        if (structPanTimeFlag == 1) {
            structPanTimeFlag = 0;

            allStructTimeFlag = 0;
            allConstTimeFlag = 0;

            if ($('#hf_StructureCode').val() != undefined && $('#hf_StructureCode').val() != '0') {
                clearStructuresExceptone(1, $('#hf_StructureCode').val());
            }
            else {
                clearStructures(1);
            }
            clearConstraints(1);

            removePopups();
            showAnimation = false;
        }
        var boundsAndZoom = getCurrentBoundsAndZoom();

        var animationText = "";
        if (document.getElementById('Structs').checked == true) {
            if (document.getElementById('All').checked == true && allStructTimeFlag == 0) {
                animationText = "Loading structures";
            }
        }
        if (document.getElementById('Constraints').checked == true) {
            if (document.getElementById('All').checked == true && allConstTimeFlag == 0) {
                if (animationText == "")
                    animationText = "Loading constraints";
                else
                    animationText += " and constraints";
            }
        }
        if (animationText != "")
            animationText += ", please wait...";

        if (showAnimation == false) {
            animationText = null;
        }
        if (document.getElementById('Structs').checked == true) {
            if (document.getElementById('Affected').checked == true) {
                btnAffectedStructures.activate();
                if (affectedStructTimeFlag == 0) {
                    affectedStructTimeFlag = 1;
                    var routeID = $('#hf_RouteID').val();
                    
                    showAffectedStructures(routeID, function (result) {
                        affectedStructArr = result;
                        setBounds(boundsAndZoom.bounds);
                    });
                }
                else {
                    if (affectedStructArr != undefined) {
                        if (document.getElementById('Underbridge').checked == true) {
                            filterStructures(affectedStructArr.underBridge, true, false, boundsAndZoom.zoom);
                        }
                        else {
                            filterStructures(affectedStructArr.underBridge, false, false, boundsAndZoom.zoom);
                        }
                        if (document.getElementById('Overbridge').checked == true) {
                            filterStructures(affectedStructArr.overBridge, true, false, boundsAndZoom.zoom);
                        }
                        else {
                            filterStructures(affectedStructArr.overBridge, false, false, boundsAndZoom.zoom);
                        }
                        if (document.getElementById('UnderAndOverbridge').checked == true) {
                            filterStructures(affectedStructArr.underAndOverBridge, true, false, boundsAndZoom.zoom);
                        }
                        else {
                            filterStructures(affectedStructArr.underAndOverBridge, false, false, boundsAndZoom.zoom);
                        }
                        if (document.getElementById('LevelCrossing').checked == true) {
                            filterStructures(affectedStructArr.levelCrossing, true, false, boundsAndZoom.zoom);
                        }
                        else {
                            filterStructures(affectedStructArr.levelCrossing, false, false, boundsAndZoom.zoom);
                        }
                    }
                }
            }
            else {
                btnAffectedStructures.deactivate();
                if (affectedStructTimeFlag == 1 && affectedStructArr != undefined) {
                    if (document.getElementById('Underbridge').checked == true) {
                        filterStructures(affectedStructArr.underBridge, false, false, boundsAndZoom.zoom);
                    }
                    if (document.getElementById('Overbridge').checked == true) {
                        filterStructures(affectedStructArr.overBridge, false, false, boundsAndZoom.zoom);
                    }
                    if (document.getElementById('UnderAndOverbridge').checked == true) {
                        filterStructures(affectedStructArr.underAndOverBridge, false, false, boundsAndZoom.zoom);
                    }
                    if (document.getElementById('LevelCrossing').checked == true) {
                        filterStructures(affectedStructArr.levelCrossing, false, false, boundsAndZoom.zoom);
                    }
                }
            }
            if (document.getElementById('All').checked == true) {
                if (boundsAndZoom.zoom > 6) {
                    if (allStructTimeFlag == 0) {
                        allStructTimeFlag = 1;
                        showAllStructures(animationText, function (result) {
                            allStructArr = result;
                            setBounds(boundsAndZoom.bounds);
                        });
                    }
                    else {
                        if (allStructArr != undefined) {
                            if (document.getElementById('Underbridge').checked == true) {
                                filterStructures(allStructArr.underBridge, true, true, boundsAndZoom.zoom);
                            }
                            else {
                                filterStructures(allStructArr.underBridge, false, true, boundsAndZoom.zoom);
                            }
                            if (document.getElementById('Overbridge').checked == true) {
                                filterStructures(allStructArr.overBridge, true, true, boundsAndZoom.zoom);
                            }
                            else {
                                filterStructures(allStructArr.overBridge, false, true, boundsAndZoom.zoom);
                            }
                            if (document.getElementById('UnderAndOverbridge').checked == true) {
                                filterStructures(allStructArr.underAndOverBridge, true, true, boundsAndZoom.zoom);
                            }
                            else {
                                filterStructures(allStructArr.underAndOverBridge, false, true, boundsAndZoom.zoom);
                            }
                            if (document.getElementById('LevelCrossing').checked == true) {
                                filterStructures(allStructArr.levelCrossing, true, true, boundsAndZoom.zoom);
                            }
                            else {
                                filterStructures(allStructArr.levelCrossing, false, true, boundsAndZoom.zoom);
                            }
                        }
                    }
                }
            }
            else {
                if (boundsAndZoom.zoom > 6) {
                    if (allStructTimeFlag == 1 && allStructArr != undefined) {
                        if (document.getElementById('Underbridge').checked == true) {
                            filterStructures(allStructArr.underBridge, false, true, boundsAndZoom.zoom);
                        }
                        if (document.getElementById('Overbridge').checked == true) {
                            filterStructures(allStructArr.overBridge, false, true, boundsAndZoom.zoom);
                        }
                        if (document.getElementById('UnderAndOverbridge').checked == true) {
                            filterStructures(allStructArr.underAndOverBridge, false, true, boundsAndZoom.zoom);
                        }
                        if (document.getElementById('LevelCrossing').checked == true) {
                            filterStructures(allStructArr.levelCrossing, false, true, boundsAndZoom.zoom);
                        }
                    }
                }
            }
        }
        else {
            btnAffectedStructures.deactivate();
            if (affectedStructArr != undefined) {
                filterStructures(affectedStructArr.underBridge, false, false, boundsAndZoom.zoom);
                filterStructures(affectedStructArr.overBridge, false, false, boundsAndZoom.zoom);
                filterStructures(affectedStructArr.underAndOverBridge, false, false, boundsAndZoom.zoom);
                filterStructures(affectedStructArr.levelCrossing, false, false, boundsAndZoom.zoom);
            }
            if (allStructArr != undefined) {
                filterStructures(allStructArr.underBridge, false, true, boundsAndZoom.zoom);
                filterStructures(allStructArr.overBridge, false, true, boundsAndZoom.zoom);
                filterStructures(allStructArr.underAndOverBridge, false, true, boundsAndZoom.zoom);
                filterStructures(allStructArr.levelCrossing, false, true, boundsAndZoom.zoom);
            }
        }
        if (document.getElementById('Constraints').checked == true) {
            if (document.getElementById('Affected').checked == true) {
                btnAffectedConstraints.activate();
                if (affectedConstTimeFlag == 0) {
                    affectedConstTimeFlag = 1;
                    var routeID = $('#hf_RouteID').val();
                    showAffectedConstraints(routeID);
                    setBounds(boundsAndZoom.bounds);
                }
                else {
                    filterConstraints(true, false);
                }
            }
            else {
                btnAffectedConstraints.deactivate();
                if (affectedConstTimeFlag == 1)
                    filterConstraints(false, false);
            }

            if (document.getElementById('All').checked == true) {
                if (boundsAndZoom.zoom > 6) {
                    if (allConstTimeFlag == 0) {
                        allConstTimeFlag = 1;
                        showAllConstraints(animationText);
                        setBounds(boundsAndZoom.bounds);
                    }
                    else
                        filterConstraints(true, true);
                }
            }
            else {
                if (boundsAndZoom.zoom > 6) {
                    if (allConstTimeFlag == 1)
                        filterConstraints(false, true);
                }
            }
        }
        else {
            btnAffectedConstraints.deactivate();
            if (affectedConstTimeFlag == 1)
                filterConstraints(false, false);
            if (allConstTimeFlag == 1)
                filterConstraints(false, true);
        }
        if (typeof $("#callFromViewMap").val() != "undefined" && $("#callFromViewMap").val() == "true") {//check added notification map
            isViewfromNotif = true;
        }
        if (isViewfromNotif) {//if it is a map from deatailed/simplified view map click
            var routeID = $('#hf_RouteID').val();
            if (affectedStructTimeFlag == 0) {
                if (document.getElementById('Structs').checked == false && document.getElementById('Affected').checked == false) {//existing functionality retained
                    if (unsuitableStructTimeFlag == 0) {
                        unsuitableStructTimeFlag = 1;
                        showUnsuitableStructures(routeID, function (result) {
                            unsuitableStructArr = result;
                            setBounds(boundsAndZoom.bounds);
                        });
                    }
                    else {
                        if (unsuitableStructArr != undefined) {
                            filterStructures(unsuitableStructArr.underBridge, true, false, boundsAndZoom.zoom);
                            filterStructures(unsuitableStructArr.overBridge, true, false, boundsAndZoom.zoom);
                            filterStructures(unsuitableStructArr.underAndOverBridge, true, false, boundsAndZoom.zoom);
                            filterStructures(unsuitableStructArr.levelCrossing, true, false, boundsAndZoom.zoom);
                        }
                    }
                }
            }
            else if (document.getElementById('Structs').checked == false && document.getElementById('Affected').checked == false) {
                filterStructures(unsuitableStructArr.underBridge, false, false, boundsAndZoom.zoom);
                filterStructures(unsuitableStructArr.overBridge, false, false, boundsAndZoom.zoom);
                filterStructures(unsuitableStructArr.underAndOverBridge, false, false, boundsAndZoom.zoom);
                filterStructures(unsuitableStructArr.levelCrossing, false, false, boundsAndZoom.zoom);
            }
            if (affectedConstTimeFlag == 0) {
                if (document.getElementById('Constraints').checked == false && document.getElementById('Affected').checked == false) {//existing functionality retained
                    if (unsuitableConstTimeFlag == 0) {
                        unsuitableConstTimeFlag = 1;
                        showUnsuitableConstraints(routeID);
                        setBounds(boundsAndZoom.bounds);
                    }
                    else {
                        ShowHideUnsuitableconstraints(true, false);
                    }
                }
            }
            else if (document.getElementById('Constraints').checked == false && document.getElementById('Affected').checked == false) {
                ShowHideUnsuitableconstraints(false, false);
            }
            if (typeof $("#callFromViewMap").val() != "undefined" && $("#callFromViewMap").val() == "false") {//check added notification map
                if (unsuitableStructArr != undefined) {
                    filterStructures(unsuitableStructArr.underBridge, false, false, boundsAndZoom.zoom);
                    filterStructures(unsuitableStructArr.overBridge, false, false, boundsAndZoom.zoom);
                    filterStructures(unsuitableStructArr.underAndOverBridge, false, false, boundsAndZoom.zoom);
                    filterStructures(unsuitableStructArr.levelCrossing, false, false, boundsAndZoom.zoom);
                }
                ShowHideUnsuitableconstraints(false, false);
            }
        }
    }
}
function getTimeFlagA2B() {
    return { affectedStruct: affectedStructTimeFlag, allStruct: allStructTimeFlag, affectedConst: affectedConstTimeFlag, allConst: allConstTimeFlag, unSuitStruct: unsuitableStructTimeFlag, unSuitConst: unsuitableConstTimeFlag };
}
function ShowAgreedApplStructureOnMap() {
    $('#agreedRouteMap').html('');
    $('#RouteMap').html('');
    var V_StruCode = $("#HFStruCode").val();
    loadmap('DISPLAYONLY');
    document.getElementById('AffectedLabel').style.opacity = "1";
    document.getElementById('Affected').disabled = false;
    $('#Overbridge').prop('checked', false);
    $('#Underbridge').prop('checked', false);
    $('#UnderAndOverbridge').prop('checked', false);
    $('#LevelCrossing').prop('checked', false);
    if (resultArr.length > 0) {
        if (resultArr[0].structureClass == "overbridge")
            $('#Overbridge').prop('checked', true);
        else if (resultArr[0].structureClass == "underbridge")
            $('#Underbridge').prop('checked', true);
        else if (resultArr[0].structureClass == "under and over bridge")
            $('#UnderAndOverbridge').prop('checked', true);
        else if (resultArr[0].structureClass == "level crossing")
            $('#LevelCrossing').prop('checked', true);
        else if (resultArr[0].structureClass == "constraints")
            $('#Constraints').prop('checked', true);
    }
    $('#Structs').prop('checked', true);
    $('#Affected').prop('checked', true);
        
    setZoomTo($('#Xcordi').val(), $('#Ycordi').val(), 12);
    myStructureArray = showStructures(resultArr, false, 'AFFECTED');
    var title = $('#UserTitle').html();
    if (title != "SOA Portal") {
        $('#idOwnership').hide();
        $('#OpenLayers_Control_Panel_169').hide();
    }
    $("#AddBack").html('<button type="button"  class="btn_reg_back next btngrad btnrds btnbdr ifx-slide-panel-back-to-prev" aria-hidden="true" data-icon="&#xe119;">Back</button>');
    stopAnimation();
}
$('body').on('click', '.ifx-slide-panel-back-to-prev', function (e) {
    BackToPreviousPage(this);
});
function clearAllInPanel() {
    if (document.getElementById('All').checked == false) {
        document.getElementById('Structs').checked = false;
        document.getElementById('Underbridge').checked = false;
        document.getElementById('Overbridge').checked = false;
        document.getElementById('UnderAndOverbridge').checked = false;
        document.getElementById('LevelCrossing').checked = false;
        document.getElementById('Constraints').checked = false;
        document.getElementById('AffectedLabel').style.opacity = "0.5";
        document.getElementById('Affected').checked = false;
        document.getElementById('Affected').disabled = true;
        if ($(".slidingpanelstructuresopen").length >= 1)
            $(".slidingpanelnav").trigger("click");
    }
    if (typeof $("#callFromViewMap").val() != "undefined" && $("#callFromViewMap").val() == "true") {//check added notification map
        $('#callFromViewMap').val(false);//clearing unsitable structures/constraints from map
    }
}
$('#PoliceBoundaries').change(function () {
    startAnimation();
    //$('.bs-canvas-overlay').remove();
    //closeRouteFilters();
    if (this.checked) {
        PoliceBoundaries(true);
    }
    else {
        PoliceBoundaries(false);
    }
    stopAnimation();
    $('#overlay').show();
    $('.bs-canvas-overlay').remove();
});
$('#LABoundaries').change(function () {
    startAnimation();
    /*$('.bs-canvas-overlay').remove();*/
    if (this.checked) {
        LABoundaries(true);
    }
    else {
        LABoundaries(false);
    }
    stopAnimation();
});
$('#NHBoundaries').change(function () {
    startAnimation();
    //$('.bs-canvas-overlay').remove();
    //closeRouteFilters();
    if (this.checked) {
        NHBoundaries(true);
    }
    else {
        NHBoundaries(false);
    }
    stopAnimation();
});
$('#TfLRoads').change(function () {
    startAnimation();
    //$('.bs-canvas-overlay').remove();
    //closeRouteFilters();
    if (this.checked) {
        TfLRoads(true);
    }
    else {
        TfLRoads(false);
    }
    stopAnimation();
});
$('#WelshTrunkRoads').change(function () {
    startAnimation();
    //$('.bs-canvas-overlay').remove();
    //closeRouteFilters();
    if (this.checked) {
        WelshTrunkRoads(true);
    }
    else {
        WelshTrunkRoads(false);
    }
    stopAnimation();
});
$('#ScottishTrunkRoads').change(function () {
    startAnimation();
    //$('.bs-canvas-overlay').remove();
    //closeRouteFilters();
    if (this.checked) {
        ScottishTrunkRoads(true);
    }
    else {
        ScottishTrunkRoads(false);
    }
    stopAnimation();
});