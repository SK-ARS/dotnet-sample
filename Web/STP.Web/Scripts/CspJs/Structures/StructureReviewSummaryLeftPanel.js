    oldId = 'li1';
    oldIdDiv = 'generalSettingsId';
    document.getElementById(this.oldIdDiv).style.display = 'block';

    $(document).ready(function () {
        $(".divOpenStructure").on('click', openStructure);
        $(".liViewGeneralDetail").on('click', ViewGeneralDetailsFn);
        $(".liShowHistory").on('click', ShowStructureHistory);
        $(".liInboxItem").on('click', relatedInboxItems); 
        $(".liManageGeneralDetail").on('click', ManageGeneralDetailsFn);
        $(".liMostOnerousVehicle").on('click', MostOnerousVehicleFn);
        $(".liShowStructureOnMap").on('click', ShowStructureOnMapFn);
        $(".liViewCausion").on('click', ViewCausionFn);
        $(".divOpenManage").on('click', openManage);
        $(".liManageConstruction").on('click', ManageDimensionConstructionFn);
        $(".liManageConstraints").on('click', ManageImposedConstraintsFn);
        $(".liManageSVData").on('click', ManageSVDataFn);
        $(".liManageICA").on('click', ManageICAFn);
        $(".liConfigure").on('click', ConfigureFn);
        $(".liCheckSuitability").on('click', CheckSuitabilityFn);
        
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
        window.location.href = "../Structures/MyStructures" + EncodedQueryString("x=" + OSGREast + "&y=" + OSGRNorth+"&structId="+'@ViewBag.StructureId');
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
    
    function ShowStructureHistory(id,idDiv,pageNum=1) {
        
        var id = 'li2';
        var idDiv = 'caution';
            //window.location.href = "../Structures/StructureHistory?structId=" + '@ViewBag.StructureId' + "&structName=" + '@ViewBag.structureName' + "&ESRN=" + '@ViewBag.ESRN';
      
        startAnimation();
         $.ajax({
             url: '../Structures/StructureHistory' + EncodedQueryString('page=' + pageNum+'&structId=@ViewBag.StructureId&structName=@HttpUtility.UrlEncode(ViewBag.structureName)&ESRN=@ViewBag.ESRN'),
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
    $('#backButtonId').hide();
    function ViewGeneralDetails(id, idDiv) {
        startAnimation();
        window.location.href = "../Structures/ReviewSummary" + EncodedQueryString("structureId=" + '@ViewBag.StructureId');
        openDetails(id, idDiv);
    };
    function ViewCausion(id, idDiv) {
        $('#backButtonId').hide();
        if (this.oldId !== id && this.oldIdDiv !== idDiv) {
            startAnimation();
            $("#generalSettingsId").empty();
            $('#bottomSection').empty();
        var randomNumber = Math.random();

        $("#generalSettingsId").load('../Structures/ReviewCautionsList?structureId=' + '@ViewBag.StructureId' + "&sectionId=" + @ViewBag.sectionId + '&random=' + randomNumber,
            function () {
                openDetails(id, idDiv);
                stopAnimation();
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
            $("#generalSettingsId").load('../Structures/StructureMostOnerousVehicleList?PageStatus=true&structureId=' + '@ViewBag.EncryptStructureId' + "&structureName=" + '@ViewBag.encryptESRN',
            function () {
                openDetails(id, idDiv);
                $('#mostOnerousId').show();
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
if($('#hf_sectionType').val() ==  "underbridge")
            {
                strucType = 1;
            }
if($('#hf_sectionType').val() ==  "overbridge")
            {
                strucType = 2;
            }
            else
            {
                strucType = 3;
            }
            $("#generalSettingsId").load('../Structures/GeneralStructure?structureId=' + '@ViewBag.StructureId' + "&sectionId=" +  @ViewBag.sectionId + "&structureNm=" + '@ViewBag.ESRN' + "&ESRN=" + '@ViewBag.ESRN',
            function () {
                openDetails(id, idDiv);
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
if($('#hf_sectionType').val() ==  "underbridge")
            {
                strucType = 1;
            }
if($('#hf_sectionType').val() ==  "overbridge")
            {
                strucType = 2;
            }
            else
            {
                strucType = 3;
            }
            $("#generalSettingsId").load('../Structures/StructureDimensions?structureId=' + '@ViewBag.StructureId' + "&sectionId=" +  @ViewBag.sectionId + "&structureNm=" + '@ViewBag.ESRN' + "&ESRN=" + '@ViewBag.ESRN' + "&strucType=" + strucType,
            function () {
                openDetails(id, idDiv);
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
if($('#hf_sectionType').val() ==  "underbridge")
            {
                strucType = 1;
            }
if($('#hf_sectionType').val() ==  "overbridge")
            {
                strucType = 2;
            }
            else
            {
                strucType = 3;
            }
            $("#generalSettingsId").load('../Structures/StructureImposedContraint?structureId=' + '@ViewBag.StructureId' + "&sectionId=" +  @ViewBag.sectionId + "&structureNm=" + '@ViewBag.ESRN' + "&ESRN=" + '@ViewBag.ESRN' + "&strucType=" + strucType,
            function () {
                openDetails(id, idDiv);
                stopAnimation();
            }
            );
        }
    };

    function ManageSVData(id, idDiv) {
        $('#backButtonId').hide();
        if (this.oldId !== id && this.oldIdDiv !== idDiv) {
            startAnimation();
            $("#generalSettingsId").empty();
            $('#bottomSection').empty();
            var strucType;
if($('#hf_sectionType').val() ==  "underbridge")
            {
                strucType = 1;
            }
if($('#hf_sectionType').val() ==  "overbridge")
            {
                strucType = 2;
            }
            else
            {
                strucType = 3;
            }
            $("#generalSettingsId").load('../Structures/SVData?StructId=' + '@ViewBag.StructureId' + "&sectionId=" +  @ViewBag.sectionId + "&structName=" + '@ViewBag.ESRN' + "&ESRN=" + '@ViewBag.ESRN',
            function () {
                openDetails(id, idDiv);
                stopAnimation();
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
            $("#generalSettingsId").load('../Structures/ManageICAUsage?structureID=' + '@ViewBag.StructureId' + "&sectionID=" + '@ViewBag.sectionId' + "&structName=" + '@ViewBag.ESRN' + "&ESRN=" + '@ViewBag.ESRN',
               function () {
                openDetails(id, idDiv);
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
            $("#generalSettingsId").load('../Structures/ConfigureBandings?structureId=' + '@ViewBag.StructureId' + "&sectionId=" + '@ViewBag.sectionId' + "&structureName=" + '@ViewBag.ESRN' + "&ESRN=" + '@ViewBag.ESRN',
               function () {
                openDetails(id, idDiv);
                stopAnimation();

               }
            );
            }
    };
    function relatedInboxItems() {
        startAnimation();
        var SectionID = $("#hdnSectionID").val();
        window.location.href = "../Movements/MovementInboxList" + EncodedQueryString("structureID=" + '@ViewBag.StructureId' + "&sectionID=" + '@ViewBag.sectionId' + "&structureNm=" + '@ViewBag.structureName' + "&ESRN=" + '@ViewBag.ESRN');

    };

    function checkSuitability(id, idDiv) {
        $('#backButtonId').hide();
        startAnimation();
        if (this.oldId !== id && this.oldIdDiv !== idDiv) {
                $("#generalSettingsId").empty();
                $('#bottomSection').empty();
            $("#generalSettingsId").load('../Structures/ICAVehicle?structName=' + '@ViewBag.ESRN' + "&ESRN=" + '@ViewBag.ESRN' + "&structureId=" + '@ViewBag.StructureId' + "&sectionId=" + '@ViewBag.sectionId',
                    function () {
                        openDetails(id, idDiv);
                        stopAnimation();

                }
            );
            }
      };

    function openDetails(id, idDiv) {
        
        if (this.oldId !== id && this.oldIdDiv !== idDiv) {
            const x1 = document.getElementById(idDiv);
            x1.style.display = 'block';

            const x = document.getElementById(id);
            x.classList.toggle('active1');

            this.closeActiveText(this.oldId);
            this.oldIdDiv = idDiv
            this.oldId = id;
        }
    }

    function openDetailsforStructHist(id, idDiv, pageNum) {
      
        if (this.oldId !== id && this.oldIdDiv !== idDiv) {
            
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
    
