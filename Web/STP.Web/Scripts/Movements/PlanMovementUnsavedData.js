var SaveDataConfig = {
    HAULIER_DETAILS: {
        LocalStorageKey: "HaulierDetails",
        DivId: "#form_Organisation #desc-entry"
    },
    SELECT_VEHICLE: {
        LocalStorageKey: "",
        DivId: ""
    },
    VEHICLE_DETAILS: {
        LocalStorageKey: "",
        DivId: ""
    },
    MOVEMENT_TYPE: {
        LocalStorageKey: "MovementTypeConfirmation",
        DivId:"#movement_type_confirmation",
        DivId1: "#allocate",
    },
    ROUTE_DETAILS: {
        LocalStorageKey: "",
        DivId: ""
    },
    ROUTEASSESSMNT_SUPPLY: {
        LocalStorageKey: "SupplyMentoryInfo",
        DivId:"#supplimentaryinfo"
    },
    OVERVIEW: {
        LocalStorageKey: "MovementOverView",
        DivId: "#overview_info_section .overview"
    },
}

function CreateLocalObject(stepFlag, saveToLocalStorage = true, createAsBlank = false) {
    var obj;
    var obj1;
    var key;
    switch (stepFlag) {
        case NavigationEnum.HAULIER_DETAILS:
            obj = GetDivObjectJson(SaveDataConfig.HAULIER_DETAILS.DivId);
            key = SaveDataConfig.HAULIER_DETAILS.LocalStorageKey;
            break;
        case NavigationEnum.MOVEMENT_TYPE:
            obj = GetDivObjectJson(SaveDataConfig.MOVEMENT_TYPE.DivId);
            key = SaveDataConfig.MOVEMENT_TYPE.LocalStorageKey;

            if (SaveDataConfig.MOVEMENT_TYPE.DivId1) {
                obj1 = GetDivObjectJson(SaveDataConfig.MOVEMENT_TYPE.DivId1);
                if (obj1 != undefined) {
                    for (var keyObj in obj1) {
                        if (obj1.hasOwnProperty(keyObj)) {
                            obj[keyObj] = obj1[keyObj];
                        }
                    }
                }
            }
            break;
        case NavigationEnum.ROUTEASSESSMNT_SUPPLY:
            obj = GetDivObjectJson(SaveDataConfig.ROUTEASSESSMNT_SUPPLY.DivId);
            key = SaveDataConfig.ROUTEASSESSMNT_SUPPLY.LocalStorageKey;
            break;
        case NavigationEnum.OVERVIEW:
            obj = GetDivObjectJson(SaveDataConfig.OVERVIEW.DivId);
            key = SaveDataConfig.OVERVIEW.LocalStorageKey;
            break;
        default:
            break;
    }
    obj = JSON.stringify(obj);
    if (createAsBlank)
        localStorage.setItem(key, '{}');
    else if (saveToLocalStorage)
        localStorage.setItem(key, obj);

    return obj;
}

function AppendObject(obj, containerId) {
    $.each(obj, function (key, valueObj) {
        var inputId = containerId + ' #' + key;
        if ($(inputId).is(':checkbox') || $(inputId).is(':radio')) {
            if (containerId == SaveDataConfig.MOVEMENT_TYPE.DivId) {
                //var value = $(inputId).is(':checked');
                $(inputId).prop("checked", valueObj);
                obj[key] = value;
                localStorage.setItem(SaveDataConfig.MOVEMENT_TYPE.LocalStorageKey, JSON.stringify(obj));
            }
            else {
                $(inputId).prop("checked", valueObj);
            }
        }
        else {
            if (containerId == SaveDataConfig.MOVEMENT_TYPE.DivId) {
                var value = $(inputId).val();
                if ((key == "FromDate" || key == "ToDate") && (value >= valueObj)) {
                    $(inputId).val(value);
                    obj[key] = value;
                    localStorage.setItem(SaveDataConfig.MOVEMENT_TYPE.LocalStorageKey, JSON.stringify(obj));
                }
                else if ($(inputId).attr('type') == 'hidden') {
                    $(inputId).val(value);
                    obj[key] = value;
                    localStorage.setItem(SaveDataConfig.MOVEMENT_TYPE.LocalStorageKey, JSON.stringify(obj));
                }
                else {
                    $(inputId).val(valueObj);
                }
            }
            else {
                $(inputId).val(valueObj);
            }
        }

        if (key == "dropSort") {
            AllocatePOP(this, valueObj);
        }
    });
}
function GetLocalObject(stepFlag,updateControls=true) {
    var objExisting;
    switch (stepFlag) {
        case NavigationEnum.HAULIER_DETAILS:
            objExisting = localStorage.getItem(SaveDataConfig.HAULIER_DETAILS.LocalStorageKey);
            if (objExisting != undefined) {
                var data = JSON.parse(objExisting);
                if (updateControls)
                    AppendObject(data, SaveDataConfig.HAULIER_DETAILS.DivId);
            }
            else {
                CreateLocalObject(stepFlag);
            }
            break;
        case NavigationEnum.MOVEMENT_TYPE:
            objExisting = localStorage.getItem(SaveDataConfig.MOVEMENT_TYPE.LocalStorageKey);
            if (objExisting != undefined) {
                var data = JSON.parse(objExisting);
                if (updateControls)
                    AppendObject(data, SaveDataConfig.MOVEMENT_TYPE.DivId);
            }
            else {
                CreateLocalObject(stepFlag);
            }
            break;
        case NavigationEnum.ROUTEASSESSMNT_SUPPLY:
            objExisting = localStorage.getItem(SaveDataConfig.ROUTEASSESSMNT_SUPPLY.LocalStorageKey);
            if (objExisting != undefined) {
                var data = JSON.parse(objExisting);
                if (updateControls)
                    AppendObject(data, SaveDataConfig.ROUTEASSESSMNT_SUPPLY.DivId);
            }
            else {
                CreateLocalObject(stepFlag);
            }
            break;
        case NavigationEnum.OVERVIEW:
            objExisting = localStorage.getItem(SaveDataConfig.OVERVIEW.LocalStorageKey);
            if (objExisting != undefined) {
                var data = JSON.parse(objExisting);
                if (updateControls)
                    AppendObject(data, SaveDataConfig.OVERVIEW.DivId);
            }
            else {
                CreateLocalObject(stepFlag);
            }
            break;
        default:
            break;
    }
    return objExisting;
}
function CompareLocalObject(stepFlag) {
    var key;
    var result = false;
    switch (stepFlag) {
        case NavigationEnum.HAULIER_DETAILS:
            key = SaveDataConfig.HAULIER_DETAILS.LocalStorageKey;
            break;
        case NavigationEnum.MOVEMENT_TYPE:
            key = SaveDataConfig.MOVEMENT_TYPE.LocalStorageKey;
            break;
        case NavigationEnum.ROUTEASSESSMNT_SUPPLY:
            key = SaveDataConfig.ROUTEASSESSMNT_SUPPLY.LocalStorageKey;
            break;
        case NavigationEnum.OVERVIEW:
            key = SaveDataConfig.OVERVIEW.LocalStorageKey;
            break;
        default:
            break;
    }
    if (key != undefined) {
        var objExisting = localStorage.getItem(key);
        var objNew = CreateLocalObject(stepFlag, false);

        if (objExisting != null && objNew!=null && objExisting != objNew)
            result = true;
    }
    return result;
}
function RemoveLocalObject(stepFlag) {
    switch (stepFlag) {
        case NavigationEnum.HAULIER_DETAILS:
            localStorage.removeItem(SaveDataConfig.HAULIER_DETAILS.LocalStorageKey);
            break;
        case NavigationEnum.MOVEMENT_TYPE:
            localStorage.removeItem(SaveDataConfig.MOVEMENT_TYPE.LocalStorageKey);
            break;
        case NavigationEnum.ROUTEASSESSMNT_SUPPLY:
            localStorage.removeItem(SaveDataConfig.ROUTEASSESSMNT_SUPPLY.LocalStorageKey);
            break;
        case NavigationEnum.OVERVIEW:
            localStorage.removeItem(SaveDataConfig.OVERVIEW.LocalStorageKey);
            break;
        default:
            break;
    }
}
function CheckLocalStorageValExist(stepFlag) {
    var objExisting;
    switch (stepFlag) {
        case NavigationEnum.HAULIER_DETAILS:
            objExisting = localStorage.getItem(SaveDataConfig.HAULIER_DETAILS.LocalStorageKey);
            break;
        case NavigationEnum.MOVEMENT_TYPE:
            objExisting = localStorage.getItem(SaveDataConfig.MOVEMENT_TYPE.LocalStorageKey);
            break;
        case NavigationEnum.ROUTEASSESSMNT_SUPPLY:
            objExisting = localStorage.getItem(SaveDataConfig.ROUTEASSESSMNT_SUPPLY.LocalStorageKey);
            break;
        case NavigationEnum.OVERVIEW:
            objExisting = localStorage.getItem(SaveDataConfig.OVERVIEW.LocalStorageKey);
            break;
        default:
            break;
    }
    return objExisting!=null && objExisting != undefined && objExisting != '';
}