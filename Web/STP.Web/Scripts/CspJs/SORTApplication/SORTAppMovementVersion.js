    var Enter_BY_SORT = $('#EnterBySort').val();
    $(document).ready(function () {
        
        $(".currentmovement").on('click', CurreMovemenRouteListing);
        $(".currentmovementvehicle").on('click', CurreMovemenVehicleListListing);
        $(".showversion").on('click', ShowingVersions);
if($('#hf_LatVer').val() ==  0 && '@ViewBag.LatVer' != null) {
            $('#MovLatestVer').val(@ViewBag.LatVer);
        }
    });
    function CurreMovemenRouteListing(e) {
        var param1 = e.currentTarget.dataset.arg1;
        var param2 = e.currentTarget.dataset.arg2;
        CurreMovemenRouteList(param1, param2);
    }
    function CurreMovemenVehicleListListing(e) {
        var param1 = e.currentTarget.dataset.arg1;
        var param2 = e.currentTarget.dataset.arg2;
        CurreMovemenVehicleList(param1, param2);
    }
    function ShowingVersions(e) {
        var param1 = e.currentTarget.dataset.arg1;
        var param2 = e.currentTarget.dataset.arg2;
        var param2 = e.currentTarget.dataset.arg3;
        var param2 = e.currentTarget.dataset.arg4;
        var param2 = e.currentTarget.dataset.arg5;
        ShowVersions(param1, param2);
    }



    function ShowVersions(vern_vo, vern_id, rev_no, rev_Id, esdal_history) {
        var LatestVer  = $('#hf_LatVer').val(); 
        var Owner  = $('#hf_OwnerName').val(); 
        var Checker  = $('#hf_Checker').val(); 
        var Work_status = $('#hdnWork_Status').val();
        var Org_Id = $('#mov_ver_' + vern_id).data('id');

        startAnimation();
        window.location.href = '../SORTApplication/SORTListMovemnets' + EncodedQueryString('SORTStatus=MoveVer&projecid=' + ProjectID + '&VR1Applciation=' + VR1Applciation + '&reduceddetailed=' + reduceddetailed + '&hauliermnemonic=' + hauliermnemonic + '&esdalref=' + esdalref + '&revisionId=' + rev_Id + '&movementId=' + movementId + '&apprevid=' + rev_Id + '&revisionno=' + rev_no + '&OrganisationId=' + Org_Id + '&versionno=' + vern_vo + '&versionId=' + vern_id + '&VR1Applciation=' + VR1Applciation + '&reduceddetailed=' + reduceddetailed + '&pageflag=' + Pageflag + '&esdal_history=' + esdal_history + '&LatestVer=' + LatestVer + '&WorkStatus=' + Work_status + '&EnterBySORT=' + Enter_BY_SORT + '&Checker=' + Checker + '&Owner=' + Owner.replace(/ /g, '%20'));
        stopAnimation();

    }

