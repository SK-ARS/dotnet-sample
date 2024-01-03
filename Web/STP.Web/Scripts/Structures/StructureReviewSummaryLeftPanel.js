oldId = 'li1';
oldIdDiv = 'generalSettingsId';
$('#'+oldIdDiv).css('display','block');
var StructureIdVal = $('#hf_StructureId').val();
var structureNameVal = $('#hf_structureName').val();
var ESRNVal = $('#hf_ESRN').val();
var sectionIdVal = $('#hf_sectionId').val();
var EncryptStructureIdVal = $('#hf_EncryptStructureId').val();
var encryptESRNVal = $('#hf_encryptESRN').val();

function StructureReviewSummaryLeftPanelInit() {
    oldId = 'li1';
    oldIdDiv = 'generalSettingsId';
    $('#' + oldIdDiv).css('display', 'block');
    StructureIdVal = $('#hf_StructureId').val();
    structureNameVal = $('#hf_structureName').val();
    ESRNVal = $('#hf_ESRN').val();
    sectionIdVal = $('#hf_sectionId').val() || 0;
    EncryptStructureIdVal = $('#hf_EncryptStructureId').val();
    encryptESRNVal = $('#hf_encryptESRN').val();
}

$(document).ready(function () {
    $('#backButtonId').hide();
    $('body').on('click', '.divOpenStructure', function (e) {
        e.preventDefault();
        openStructure(this);
    });
    $('body').on('click', '.liViewGeneralDetail', function (e) {
        e.preventDefault();
        ViewGeneralDetailsFn(this);
    });
    $('body').on('click', '.liShowHistory', function (e) {
        
        e.preventDefault();
        ShowStructureHistory(this);
    });
    $('body').on('click', '.liInboxItem', function (e) {
        e.preventDefault();
        relatedInboxItems('li3','InboxItem');
    });
    $('body').on('click', '.liManageGeneralDetail', function (e) {
        e.preventDefault();
        ManageGeneralDetailsFn(this);
    });
    $('body').on('click', '.liMostOnerousVehicle', function (e) {
        e.preventDefault();
        MostOnerousVehicleFn(this);
    });
    $('body').on('click', '.liShowStructureOnMap', function (e) {
        e.preventDefault();
        ShowStructureOnMapFn(this);
    });
    $('body').on('click', '.liViewCausion', function (e) {
        e.preventDefault();
        ViewCausionFn(this);
    });
    $('body').on('click', '.divOpenManage', function (e) {
        e.preventDefault();
        openManage(this);
    });
    $('body').on('click', '.liManageConstruction', function (e) {
        e.preventDefault();
        ManageDimensionConstructionFn(this);
    });
    $('body').on('click', '.liManageConstraints', function (e) {
        e.preventDefault();
        ManageImposedConstraintsFn(this);
    });
    $('body').on('click', '.liManageSVData', function (e) {
        e.preventDefault();
        ManageSVDataFn(this);
    });
    $('body').on('click', '.liManageICA', function (e) {
        e.preventDefault();
        ManageICAFn(this);
    });
    $('body').on('click', '.liConfigure', function (e) {
        e.preventDefault();
        ConfigureFn(this);
    });
    $('body').on('click', '.liCheckSuitability', function (e) {
        e.preventDefault();
        CheckSuitabilityFn(this);
    });
});

function closeActiveText(oldId) {
    const x = document.getElementById(oldId);
    x.classList.remove('active1');
}

function ShowStructureOnMap(id, idDiv) {
    sessionStorage.setItem("AuthorizeGeneralUrl", null);
    var OSGREast, OSGRNorth;
    OSGREast = $("#OSGREast").val();
    OSGRNorth = $("#OSGRNorth").val();
    window.location.href = "../Structures/MyStructures" + EncodedQueryString("x=" + OSGREast + "&y=" + OSGRNorth + "&structId=" + StructureIdVal);
    openDetails(id, idDiv);
}

function displayOpener(v) {
    document.getElementById(v).style.display = 'block';
}
$(document).ready(function () {

    $('#generalSettingsId').on('click', '.structure-pagination a', function (e) {
        e.preventDefault();
        var page = getUrlParameterByName("page", this.href);
        ShowStructureHistory('', '', page);
    });

});

function ShowStructureHistory(id, idDiv, pageNum = 1) {
    
    var id = 'li2';
    var idDiv = 'caution';

    startAnimation();
    $.ajax({
      /*  url: '../Structures/StructureHistory' + EncodedQueryString('page=' + pageNum + '&structId=' + StructureIdVal + '&structName=' + structureNameVal + '&ESRN=' + ESRNVal + ''),*/
        url: "../Structures/StructureHistory?page=" + pageNum + "&structId=" + StructureIdVal + "&structName=" + structureNameVal + "&ESRN=" + ESRNVal + "",
        //dataType: 'json',
        
        type: 'GET',
        cache: false,
        async: false,
        //data: $("#frmFilterMoveInbox").serialize(),
        beforeSend: function () {
            startAnimation();
        },
        success: function (response) {
           
           
            $("#generalSettingsId").html($(response).find('#caution').html());
            $('.btn-normal').hide();
            $('.btn-structhist').show();
            $('.titleSOAStructrue').hide();
            $('#bottomSection').html('');
            openDetailsforStructHist(id, idDiv, pageNum);
            //stopAnimation();
        },


        error: function (xhr, status) {

            location.reload();
        },
        complete: function () {

            stopAnimation();

        }

    });
};
function BackClick(structId) {

    window.location.href = "../Structures/ReviewSummary" + EncodedQueryString("structureId=" + structId);
};

function ViewGeneralDetails(id, idDiv) {
    startAnimation();
    window.location.href = "../Structures/ReviewSummary" + EncodedQueryString("structureId=" + StructureIdVal);
    openDetails(id, idDiv);
};
function ViewCausion(id, idDiv) {
    $('#backButtonId').hide();
    if (this.oldId !== id && this.oldIdDiv !== idDiv) {
        startAnimation();
        $("#generalSettingsId").empty();
        $('#bottomSection').empty();
        var randomNumber = Math.random();

        $("#generalSettingsId").load('../Structures/ReviewCautionsList?structureId=' + StructureIdVal + "&sectionId=" + sectionIdVal + '&random=' + randomNumber,
            function () {
                openDetails(id, idDiv);
                stopAnimation();
                ReviewCautionsListInit();
            }
        );
    }
};

function MostOnerousVehicle(id, idDiv) {

    $('#backButtonId').hide();
    if (this.oldId !== id && this.oldIdDiv !== idDiv) {
        startAnimation();
        $("#generalSettingsId").empty();
        $('#bottomSection').empty();
        //$('#caution').empty();
        $("#generalSettingsId").load('../Structures/StructureMostOnerousVehicleList?PageStatus=true&structureId=' + EncryptStructureIdVal + "&structureName=" + encryptESRNVal,
            function () {
                openDetails(id, idDiv);
                $('#mostOnerousId').show();
                StructureMostOnerousVehicleListInit();
                stopAnimation();
            }
        );
    }
};

function ManageGeneralDetails(id, idDiv) {
    $('#backButtonId').hide();
    if (this.oldId !== id && this.oldIdDiv !== idDiv) {
        startAnimation();
        $("#generalSettingsId").empty();
        $('#bottomSection').empty();
        var strucType;
        if ($('#hf_sectionType').val() == "underbridge") {
            strucType = 1;
        }
        if ($('#hf_sectionType').val() == "overbridge") {
            strucType = 2;
        }
        else {
            strucType = 3;
        }
        $("#generalSettingsId").load('../Structures/GeneralStructure?structureId=' + StructureIdVal + "&sectionId=" + sectionIdVal + "&structureNm=" + ESRNVal + "&ESRN=" + ESRNVal,
            function () {
                openDetails(id, idDiv);
                GeneralStructureInit();
                stopAnimation();
            }
        );
    }
};
function ManageDimensionConstruction(id, idDiv) {
    $('#backButtonId').hide();
    if (this.oldId !== id && this.oldIdDiv !== idDiv) {
        startAnimation();
        $("#generalSettingsId").empty();
        $('#bottomSection').empty();
        var strucType;
        if ($('#hf_sectionType').val() == "underbridge") {
            strucType = 1;
        }
        else if ($('#hf_sectionType').val() == "overbridge") {
            strucType = 2;
        }
        else {
            strucType = 3;
        }
        $("#generalSettingsId").load('../Structures/StructureDimensions?structureId=' + StructureIdVal + "&sectionId=" + sectionIdVal + "&structureNm=" + ESRNVal + "&ESRN=" + ESRNVal + "&strucType=" + strucType,
            function () {
                openDetails(id, idDiv);
                StructureDimensionsInit();
                stopAnimation();
            }
        );
    }
};

function ManageImposedConstraoints(id, idDiv) {
    
    $('#backButtonId').hide();
    if (this.oldId !== id && this.oldIdDiv !== idDiv) {
        startAnimation();
        $("#generalSettingsId").empty();
        $('#bottomSection').empty();
        var strucType;
        if ($('#hf_sectionType').val() == "underbridge") {
            strucType = 1;
        }
        else if ($('#hf_sectionType').val() == "overbridge") {
            strucType = 2;
        }
        else {
            strucType = 3;
        }
        $("#generalSettingsId").load('../Structures/StructureImposedContraint?structureId=' + StructureIdVal + "&sectionId=" + sectionIdVal + "&structureNm=" + ESRNVal + "&ESRN=" + ESRNVal + "&strucType=" + strucType,
            function () {
                openDetails(id, idDiv);
                stopAnimation();
                StructureImposedContraintInit();
            }
        );
    }
};

function ManageSVData(id, idDiv, performbtn) {
    $('#backButtonId').hide();
    if (this.oldId !== id && this.oldIdDiv !== idDiv) {
        startAnimation();
        $("#generalSettingsId").empty();
        $('#bottomSection').empty();
        var strucType;
        if ($('#hf_sectionType').val() == "underbridge") {
            strucType = 1;
        }
        if ($('#hf_sectionType').val() == "overbridge") {
            strucType = 2;
        }
        else {
            strucType = 3;
        }
        $("#generalSettingsId").load('../Structures/SVData?StructId=' + StructureIdVal + "&sectionId=" + sectionIdVal + "&structName=" + ESRNVal + "&ESRN=" + ESRNVal,
            function () {
                openDetails(id, idDiv);
                stopAnimation();
                SVDataInit();
            }
        );
    }
    if (performbtn == 'true' || performbtn == 'True') {
        $("#generalSettingsId").load('../Structures/SVData?StructId=' + StructureIdVal + "&sectionId=" + sectionIdVal + "&structName=" + ESRNVal + "&ESRN=" + ESRNVal,
            function () {
                openDetails(id, idDiv);
                stopAnimation();
                SVDataInit();
            }
        );
    }
};

function manageICA(id, idDiv) {
    $('#backButtonId').hide();
    startAnimation();
    if (this.oldId !== id && this.oldIdDiv !== idDiv) {
        $("#generalSettingsId").empty();
        $('#bottomSection').empty();
        $("#generalSettingsId").load('../Structures/ManageICAUsage?structureID=' + StructureIdVal + "&sectionID=" + sectionIdVal + "&structName=" + ESRNVal + "&ESRN=" + ESRNVal,
            function () {
                openDetails(id, idDiv);
                ManageICAUsageInit();
                stopAnimation();

            }
        );
    }
};
function Configure(id, idDiv) {
    $('#backButtonId').hide();
    startAnimation();
    if (this.oldId !== id && this.oldIdDiv !== idDiv) {
        $("#generalSettingsId").empty();
        $('#bottomSection').empty();
        $("#generalSettingsId").load('../Structures/ConfigureBandings?structureId=' + StructureIdVal + "&sectionId=" + sectionIdVal + "&structureName=" + ESRNVal + "&ESRN=" + ESRNVal,
            function () {
                stopAnimation();
                openDetails(id, idDiv);
                ConfigureBandingsInit();
            }
        );
    }
};
function relatedInboxItems(id, idDiv) {
    startAnimation();
    $('#backButtonId').hide();
    var SectionID = $("#hdnSectionID").val();
    $.ajax({
        url: "../Movements/MovementInboxList?structureID=" + StructureIdVal + "&sectionID=" + sectionIdVal + "&structureNm=" + structureNameVal + "&ESRN=" + ESRNVal + "&isRelatedInboxList=1",
        type: 'GET',
        cache: false,
        beforeSend: function () {
            startAnimation();
        },
        success: function (result) {
            $('.titleSOAStructrue').hide();
            $('#generalSettingsId').find('div#filters').remove();
            $('#generalSettingsId').html($(result).find('#banner-container').html(), function () {
                event.preventDefault();
            });
            var filters = $(result).find('div#filters');
            $(filters).find('form .filters:first').remove();
            $(filters).find('form .filters:first').remove();
            $(filters).insertAfter('#banner');

            //bottomSection
            $('#bottomSection').html('');
            openDetails(id, idDiv);
        },
        complete: function () {
            stopAnimation();
        }
    });
};

function checkSuitability(id, idDiv) {
    $('#backButtonId').hide();
    startAnimation();
    if (this.oldId !== id && this.oldIdDiv !== idDiv) {
        $("#generalSettingsId").empty();
        $('#bottomSection').empty();
        $("#generalSettingsId").load('../Structures/ICAVehicle?structName=' + ESRNVal + "&ESRN=" + ESRNVal + "&structureId=" + StructureIdVal + "&sectionId=" + sectionIdVal,
            function () {
                openDetails(id, idDiv);
                stopAnimation();
                ICAVehicleInit();
            }
        );
    }
};

function openDetails(id, idDiv) {

    if (this.oldId !== id && this.oldIdDiv !== idDiv) {
        $('#' + idDiv).css('display', 'block');
        //$('#' + id).addClass('active1');
        $('#' + id).addClass('active1').siblings().removeClass('active1');
        //this.closeActiveText(this.oldId);
        this.oldIdDiv = idDiv
        this.oldId = id;
    }
}

function openDetailsforStructHist(id, idDiv, pageNum) {
   
    if (this.oldId != id && this.oldIdDiv != idDiv) {

        const x = document.getElementById(id);
        if (pageNum == 1) {
            x.classList.toggle('active1');
        }
        else {

        }

        this.closeActiveText(this.oldId);
        this.oldIdDiv = idDiv
        this.oldId = id;
    }
}

//function ViewGeneralDetailsFn(e) {
//    ViewGeneralDetails('li1', 'generalSettingsId');
//}

function ManageGeneralDetailsFn(e) {
    ManageGeneralDetails('li4', 'manageGeneralDetails');
}
function MostOnerousVehicleFn(e) {
    MostOnerousVehicle('li5', 'mostOnerousId');
}
function ShowStructureOnMapFn(e) {
    ShowStructureOnMap('li7', 'manageCautionId');
}
function ViewCausionFn(e) {
    ViewCausion('li7', 'manageCautionId');
}
function ManageDimensionConstructionFn(e) {
    ManageDimensionConstruction('li8', 'managedimensiosId');
}
function ManageImposedConstraintsFn(e) {
    ManageImposedConstraoints('li9', 'manageImposed');
}

function ManageSVDataFn(e) {
    ManageSVData('li10', 'manageSVId');
}
function ManageICAFn(e) {
    manageICA('li11', 'manageICAId');
}

function ViewGeneralDetailsFn(e) {
    ViewGeneralDetails('li1', 'generalSettingsId');
}
function ManageGeneralDetailsFn(e) {
    ManageGeneralDetails('li4', 'manageGeneralDetails');
}
function MostOnerousVehicleFn(e) {
    MostOnerousVehicle('li5', 'mostOnerousId');
}
function ShowStructureOnMapFn(e) {
    ShowStructureOnMap('li7', 'manageCautionId');
}
function ViewCausionFn(e) {
    ViewCausion('li7', 'manageCautionId');
}
function ManageDimensionConstructionFn(e) {
    ManageDimensionConstruction('li8', 'managedimensiosId');
}
function ManageImposedConstraintsFn(e) {
    ManageImposedConstraoints('li9', 'manageImposed');
}

function ManageSVDataFn(e) {
    ManageSVData('li10', 'manageSVId');
}
function ManageICAFn(e) {
    manageICA('li11', 'manageICAId');
}
function ConfigureFn(e) {
    Configure('li12', 'configureDefaultBandingFactorsId');
}
function CheckSuitabilityFn(e) {
    checkSuitability('li13', 'checkSuitabilityOfVehicleId');
}

