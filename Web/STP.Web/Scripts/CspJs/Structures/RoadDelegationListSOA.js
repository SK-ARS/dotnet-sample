    {

        $(document).ready(function () {
            $("#roaddeleclsfilter").on('click', closeFilters);
            $(".viewdelegation").on('click', Viewdelegationdetails);
            $("#srchbtn").on('click', SearchDelegation);
        });
        function Viewdelegationdetails(e) {
            var arg1 = e.currentTarget.dataset.arg1;
            Viewdelegation(arg1);
        }
    }
