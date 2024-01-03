
        $(document).ready(function () {
            selectedmenu('Structures');
            SelectMenu(3);
            // fillPageSizeSelect();
            var OrgFromId = $('#OrgFromId').val();
            var arrId = $('#arrangId').val();

            var OrgFromId = $('#OrgFromId').val();

            $('#review_deleg').load('../Structures/ReviewDelegation?arrangId=' + arrId + '&OrgFromId=' + OrgFromId);

            $('#BackDeleg').click(function () {
                window.location.href = '../Structures/MyDelegationArrangement';
            });

            $(".StructureCodeClick").on('click', StructureCodeClick);
        });

        function linkStructureCodeClick(structId) {
            var a = EncodedQueryString("structureId=" + structId);
            window.location.href = "../Structures/ReviewSummary" + EncodedQueryString("structureId=" + structId);
        }

        function StructureCodeClick(e) {
            var structureId = e.currentTarget.dataset.StructureId;
            linkStructureCodeClick(structureId);
        }

        function StructureCodeClick(e) {
            var structureId = e.currentTarget.dataset.StructureId;
            linkStructureCodeClick(structureId);
        }
