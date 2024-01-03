$(document).ready(function () {
$('body').on('click','.component-list', function(e) { 
  e.preventDefault();
  ComponentDetail(this);
}); 
$('body').on('click','.newComponent', function() { window['GoToCreateComponentPage'](); }); 
$('body').on('click','.btn_back_to_Config', function() { window['Backbutton'](); }); 
$('body').on('click','.closeCompFilter', function() { window['clearSearch'](); }); 
$('body').on('click','.useComponent', function(e) { 
  e.preventDefault();
  ImportFleetComponent(this);
}); 
$('body').on('click','.deleteComponent', function(e) { 
    e.preventDefault();
    var name = $(this).attr("name");
    var componentId = $(this).attr('id');
    DeleteConfirmation(componentId, name);
    //DeleteComponentConfirmation(componentId,name);
}); 
$('body').on('click','.manageFav', function(e) { 
  e.preventDefault();
  ManageComponentFavourites(this);
}); 
$('body').on('click','.btnClearFleet', function() { window['clearSearch'](); }); 
$('body').on('click','.btnFleetSearch', function() { window['SearchVehicleComponent'](); }); 
    
});

function ComponentDetail(e) {
    var componentId = $(e).attr("data-arg1");
    var mode = $(e).attr("data-arg2");
    ViewComponentDetail(componentId, mode);
}

function ImportFleetComponent(e) {
    var componentId =$(e).attr("componentid");
    ImportComponentToConfig(componentId);
}
function DeleteComponentConfirmation(e) {
    var componentDesc =$(e).attr("componentdescription");
    DeleteConfirmation(componentId, compName);
}
function ManageComponentFavourites(e) {
    var componentId =$(e).attr("componentid");
    var arg1 =$(e).attr("arg1");
    var arg2 =$(e).attr("arg2");
    ManageFavourites(componentId, arg1, arg2);
}
