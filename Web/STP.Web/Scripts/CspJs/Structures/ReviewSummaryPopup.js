                        var StructureId  = $('#hf_StructureId').val(); 
                                var sectionType1 = '@sectionType';
    {
        $(document).ready(function () {
            
            $(".closestructdetail").on('click', CloseStructureDetails);
            
            $(".displaycontact").on('click', DisplayContactDetailing);
           
            $(".showsection").on('click', showSectionDetailssection);
            $('#span-close').click(function () {
                $('#overlay').hide();
                addscroll();
            });
            Resize_PopUp(650);
            addscroll();
        });
        
        function DisplayContactDetailing(e) {
            var arg1 = e.currentTarget.dataset.arg1;
            DisplayContactDetails(arg1);
        }
        
        function showSectionDetailssection(e) {
            var arg1 = e.currentTarget.dataset.arg1;
            showSectionDetails(arg1,e.id);
        }


        function closeSpan() {
            $('#overlay').hide();
            addscroll();
            resetdialogue();
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
        function showSectionDetails(StructureId, sectionId) {
            $("#hdnSectionID").val(sectionId);
            var sectionType = $('#' + sectionId).parent().find('input:hidden:first').val();
            $('#divStructureSectionDetails').load('@Url.Action("ReviewStructureSectionImposedConstraints", "Structures")',
                { structureId: StructureId, sectionId: sectionId, sectionType: sectionType },
                function () {
                    $('#divStructureSectionDetails').show();
                }
            );
        }
    }
