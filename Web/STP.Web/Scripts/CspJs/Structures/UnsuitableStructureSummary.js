                        var StructureId  = $('#hf_StructureId').val(); 
                                var sectionType1 = '@sectionType';

    $(document).ready(function () {
        if ('@section_Id' != 0) {
            $("input:radio[id='@section_Id']").trigger("click");
        }
        else{
            @{
            if (objListStructureSection != null && objListStructureSection.Count > 0)
            {
                foreach (var item in objListStructureSection)
                {
                    if (item.AffectFlag != 0)
                    {
                        section_Id = item.SectionId;
                        break;
                    }
                }
            }
        }
            $("input:radio[id='@section_Id']").trigger("click");
        }
        var ownerCount = '@StructureOwnChain.Count';
        $('#span-close').click(function () {
            $('#overlay').hide();
            addscroll();
        });
        Resize_PopUp(650);
        addscroll();

        $(".CloseStructure").on('click', CloseStructureDetails);
        $(".DisplayContact").on('click', DisplayContact);
        $(".rd_showSectionDetail").on('click', ShowSectionDetailsFn);

    });
    function ShowContactDetails(contactId) {
        //$("#overlay").show();
        // $('.loading').show();
        var ownerCount = '@StructureOwnChain.Count';
        removescroll();
        $("#StructureOwnerContact").load('../Application/ViewContactDetails?ContactId=' + contactId + "&structOwner=" + 'otherOrg' + "&ownerCnt=" + ownerCount, function () {
            $("#overlay").show();
            $('#structdetail').hide();
            $('#structdetailHead').hide();
            $('#div_review_summary').hide();
            $("#StructureOwnerContact").show();
            $('.loading').hide();
        });

    }
    function ClosePopupRSummary() {
        $('#overlay').hide();
        addscroll();
        resetdialogue();
    }
    function showSectionDetails(StructureId, sectionId, flag, vehicleName) {
        $('#affctdMessage_tr').hide();
        $('#affctdMessage_td').hide();
        if (flag != 0)
        {
            var affect_msg = getAffectedMessage(flag, vehicleName);
            $('#affctdMessage_tr').show();
            $('#affctdMessage_td').show();
            $('#affctdMessage_td').html(affect_msg);
        }
        $("#hdnSectionID").val(sectionId);
        var sectionType = $('#' + sectionId).parent().find('input:hidden:first').val();
        $('#divStructureSectionDetails').load('@Url.Action("ReviewStructureSectionImposedConstraints", "Structures")',
            { structureId: StructureId, sectionId: sectionId, sectionType: sectionType },
            function () {
                if ('@objListStructureGeneralDetails' != null) {
                    $('#divStructureSectionDetails').show();
                }
            });
    }
    function getAffectedMessage(affectFlag, vehicleName) {
        var msg = "";
        if (affectFlag == 1) {
            msg = "Vehicle ( " + vehicleName + " ) gross weight exceeds structure's gross weight, so this structure is unsuitable";
        }
        else if (affectFlag == 2) {
            msg = "Vehicle ( " + vehicleName + " ) gross weight exceeds structure's signed gross weight, so this structure is unsuitable";
        }
        else if (affectFlag == 3) {
            msg = "Vehicle ( " + vehicleName + " ) maximum axle weight exceeds structure's axle weight, so this structure is unsuitable";
        }
        else if (affectFlag == 4) {
            msg = "Vehicle ( " + vehicleName + " ) maximum axle weight exceeds structure's signed axle weight, so this structure is unsuitable";
        }
        else if (affectFlag == 5) {
            msg = "Vehicle ( " + vehicleName + " ) height exceeds structure's height, so this structure is unsuitable";
        }
        else if (affectFlag == 6) {
            msg = "Vehicle ( " + vehicleName + " ) height exceeds structure's signed height, so this structure is unsuitable";
        }
        else if (affectFlag == 7) {
            msg = "Vehicle ( " + vehicleName + " ) width exceeds structure's width, so this structure is unsuitable";
        }
        else if (affectFlag == 8) {
            msg = "Vehicle ( " + vehicleName + " ) width exceeds structure's signed width, so this structure is unsuitable";
        }
        else if (affectFlag == 9) {
            msg = "Vehicle (" + vehicleName + " ) length exceeds structure's length, so this structure is unsuitable";
        }
        else if (affectFlag == 10) {
            msg = "Vehicle ( " + vehicleName + " ) length exceeds structure's signed length, so this structure is unsuitable";
        }
        else if (affectFlag == 11) {
            msg = "Vehicle ( " + vehicleName + " ) axle weight exceeds structure’s maximum weight over distance, so this structure is unsuitable";
        }
        else if (affectFlag == 12) {
            msg = "Vehicle ( " + vehicleName + " ) gross weight doesn't satisfy SV screening, so this structure is unsuitable";
        }
        return msg;
    }
    function CloseStructureDetails() {
        $('#exampleModalCenter22').hide();
        $('#overlay').hide();
        addscroll();
        resetdialogue();
        stopAnimation();
    }
    function DisplayContactDetails(ContactId) {
        startAnimation();
        $('#exampleModalCenter22').hide();
        $("#dialogue1").load('../Application/ViewContactDetails?ContactId=' + ContactId + "", function () {
            removescroll();
            $('#contactDetails').modal('hide');
            $('#contactDetailsForMap').show();
            $("#dialogue1").show();
            $("#overlay").show();
            stopAnimation();
        });

    }
    function closeContactPopup() {
        $('#contactDetailsForMap').hide();
        $("#dialogue1").hide();
        $('#exampleModalCenter22').show();
    }
    function DisplayContact(e) {
        var chainContactId = e.currentTarget.dataset.ChainContactId;
        DisplayContactDetails(chainContactId);
    }
    function ShowSectionDetailsFn(e) {
        var structureId = e.currentTarget.dataset.StructureId;
        var id = e.currentTarget.dataset.id;
        var affectFlag = e.currentTarget.dataset.AffectFlag;
        var vehicleName = e.currentTarget.dataset.VehicleName;
        showSectionDetails(structureId, id, affectFlag, vehicleName);
    }
