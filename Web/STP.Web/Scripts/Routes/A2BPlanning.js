$(document).ready(function () {
    $(function () {
        return false;
    });
});

function A2BPlanningInit() {
    var wrapper_width = $('#wrapper_center').width();
    var window_height = window.screen.availHeight;
    var min_height = parseInt(window_height - (184 + 100));
    createContextMenu();
    //stopAnimation();
    closeContentLoader('#appRouteMap');
    //if ($('#hf_IsPlanMovmentGlobal').length > 0) {
    //    ShowUnSuitableStructuresOrConstraintsOnMapFromRouteAssessment();
    //}
}

function ShowUnSuitableStructuresOrConstraintsOnMapFromRouteAssessment() {
    //TO display unsuitable structure or constraints in map from Plan movement route assessment page
    if (ViewOrEditRouteFromRouteAssessment != undefined && ViewOrEditRouteFromRouteAssessment != null && ViewOrEditRouteFromRouteAssessment.RouteId != undefined && ViewOrEditRouteFromRouteAssessment.RouteId > 0) {
        //openContentLoader('#vehicles');
        //setTimeout(function () {
        //    EditRoute(ViewOrEditRouteFromRouteAssessment.RouteId, null, null, function () {
        //        if (ViewOrEditRouteFromRouteAssessment.Type == "STRUCTURE") {
        //            showUnsuitableStructures(ViewOrEditRouteFromRouteAssessment.RouteId);
        //            //closeContentLoader('#vehicles');
        //            ViewOrEditRouteFromRouteAssessment = undefined;
        //        } else if (ViewOrEditRouteFromRouteAssessment.Type == "CONSTRAINT") {
        //            showUnsuitableConstraints(ViewOrEditRouteFromRouteAssessment.RouteId);
        //            //closeContentLoader('#vehicles');
        //            ViewOrEditRouteFromRouteAssessment = undefined;
        //        }
        //    });
        //}, 500);
        showUnsuitableStructures(ViewOrEditRouteFromRouteAssessment.RouteId);
        showUnsuitableConstraints(ViewOrEditRouteFromRouteAssessment.RouteId);
    }
}

const eventListenerOptionsSupported = () => {
    let supported = false;

    try {
        const opts = Object.defineProperty({}, 'passive', {
            get() {
                supported = true;
            }
        });

        window.addEventListener('test', null, opts);
        window.removeEventListener('test', null, opts);
    } catch (e) { }

    return supported;
}

const defaultOptions = {
    passive: false,
    capture: false
};
const supportedPassiveTypes = [
    'scroll', 'wheel',
    'touchstart', 'touchmove', 'touchenter', 'touchend', 'touchleave',
    'mouseout', 'mouseleave', 'mouseup', 'mousedown', 'mousemove', 'mouseenter', 'mousewheel', 'mouseover'
];
const getDefaultPassiveOption = (passive, eventName) => {
    if (passive !== undefined) return passive;

    return supportedPassiveTypes.indexOf(eventName) === -1 ? false : defaultOptions.passive;
};

const getWritableOptions = (options) => {
    const passiveDescriptor = Object.getOwnPropertyDescriptor(options, 'passive');

    return passiveDescriptor && passiveDescriptor.writable !== true && passiveDescriptor.set === undefined
        ? Object.assign({}, options)
        : options;
};

const overwriteAddEvent = (superMethod) => {
    EventTarget.prototype.addEventListener = function (type, listener, options) {
        const usesListenerOptions = typeof options === 'object' && options !== null;
        const useCapture = usesListenerOptions ? options.capture : options;

        options = usesListenerOptions ? getWritableOptions(options) : {};
        options.passive = getDefaultPassiveOption(options.passive, type);
        options.capture = useCapture === undefined ? defaultOptions.capture : useCapture;

        superMethod.call(this, type, listener, options);
    };

    EventTarget.prototype.addEventListener._original = superMethod;
};

const supportsPassive = eventListenerOptionsSupported();

if (supportsPassive) {
    const addEvent = EventTarget.prototype.addEventListener;
    overwriteAddEvent(addEvent);
}