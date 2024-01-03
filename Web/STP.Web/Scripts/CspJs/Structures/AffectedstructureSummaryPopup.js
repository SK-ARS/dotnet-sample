                        var StructureId  = $('#hf_StructureId').val(); 
                                var sectionType1 = '@sectionType';
    {
        $(document).ready(function () {
            var value = $("#AuthorizeMovementGeneral").val();
            $('#span-close').click(function () {
                $('#overlay').hide();
                addscroll();
            });
            Resize_PopUp(650);
            addscroll();
            $(".span-CloseStructDetails").on('click', CloseStructureDetails);
            $(".DisplayContact").on('click', ShowingContactDetails);
            $(".sectiondetails").on('click', SectionDetails );
            $("#viewMap").on('click', ViewStructureOnMap);
        });
        function ShowingContactDetails(e) {
            var ContactDetails = e.currentTarget.dataset.DisplayContactDetails;
            
            DisplayContactDetails(ContactDetails);
        }
        function SectionDetails(e) {
            var SectionDetails = e.currentTarget.dataset.SectionDetails;
            var arg2 = e.currentTarget.dataset.arg2;
            showSectionDetails(SectionDetails,arg2);
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
        function ViewStructureOnMap() {
            var OSGREast, OSGRNorth;
            OSGREast = $("#OSGREast").val();
            OSGRNorth = $("#OSGRNorth").val();
            var url = window.location.href;
            sessionStorage.setItem("AuthorizeGeneralUrl", url);
            window.location.href = "../Structures/MyStructures" + EncodedQueryString("x=" + OSGREast + "&y=" + OSGRNorth+"&structId="+'@ViewBag.StructureId');
        }
    }
