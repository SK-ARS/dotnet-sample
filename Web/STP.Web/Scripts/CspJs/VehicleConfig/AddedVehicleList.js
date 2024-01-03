    $(document).ready(function () {
        $(".importVehicle").on('click', ImportVehicleForNotif); 
        $(".hrefDeleteVehicle").on('click', DeleteConfiguration);
    });
    function ImportVehicleForNotif(e) {
        var configurationId = e.currentTarget.dataset.ConfigurationId;
        var vehicleName = e.currentTarget.dataset.VehicleName;        
        ImportForNotif(configurationId, vehicleName);
    }
    function DeleteConfiguration(e) {
        var configurationId = e.currentTarget.dataset.ConfigurationId;
        Delete(e,configurationId);
    }
