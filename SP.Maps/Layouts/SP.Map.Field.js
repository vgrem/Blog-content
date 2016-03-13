(function () {
    if (typeof GeolocationField == "object") {
        return;
    }

    window.GeolocationField = (function () {

        return {
            ParseGeolocationValue: function (fieldValue) {
                var spatialtype = "POINT";
                var space = ' ';
                var openingBracket = '(';
                var closingBracket = ')';
                var point = new Object();

                point.longitude = null;
                point.latitude = null;
                point.altitude = null;
                point.measure = null;
                if (fieldValue == null || fieldValue == '')
                    return null;
                var valueIndex = 0;
                var valueLength = fieldValue.length;
                var subStr;
                var argIndex = 0;
                var index = fieldValue.indexOf(openingBracket, valueIndex);

                if (index <= valueIndex) {
                    return null;
                }
                var headEnd = index;

                if (fieldValue.charCodeAt(index - 1) == space.charCodeAt(0)) {
                    headEnd--;
                }
                subStr = fieldValue.substr(valueIndex, headEnd - valueIndex);
                if (spatialtype.toLowerCase() != subStr.toLowerCase()) {
                    return null;
                }
                valueIndex = index + 1;
                while (valueIndex < valueLength) {
                    index = fieldValue.indexOf(space, valueIndex);
                    if (index <= valueIndex) {
                        index = fieldValue.indexOf(closingBracket, valueIndex);
                    }
                    if (index <= valueIndex) {
                        return null;
                    }
                    subStr = fieldValue.substr(valueIndex, index - valueIndex);
                    if (argIndex == 0) {
                        point.longitude = parseFloat(subStr);
                    } else if (argIndex == 1) {
                        point.latitude = parseFloat(subStr);
                    } else if (argIndex == 2) {
                        point.altitude = parseFloat(subStr);
                    } else if (argIndex == 3) {
                        point.measure = parseFloat(subStr);
                    }
                    argIndex++;
                    valueIndex = index + 1;
                }
                if (argIndex < 2) {
                    return null;
                }
                return point;
            },
            ShowMappyHover: function (fieldName, listItemID, values, apiKey) {
                var mapDiv = document.getElementById("mapContainer_" + listItemID);
                var map = new SP.Maps.MapProviderFactory.createMapProvider("Bing", "7.0");
                var points = [];
                var point = new Object();

                point.latitude = values.latitude;
                point.longitude = values.longitude;
                point.isDraggable = false;
                point.showHover = false;
                point.icon = "/_layouts/images/MapPushpin.25x39x32.png";
                points.push(point);
                var spMapOption = new SP.Maps.SPMapOption();

                spMapOption.mapDiv = mapDiv;
                spMapOption.apiKey = apiKey;
                spMapOption.center = new SP.Maps.Geolocation(point.latitude, point.longitude);
                spMapOption.height = 280;
                //spMapOption.width = 280;
                map.loadMap(spMapOption);
                map.addPushpins(points);
            },
            ShowMapPopup: function (viewonmap) {
                var svalue = $(viewonmap).attr('fldValue');
                var values = GeolocationField.ParseGeolocationValue(svalue);
                var listItemId = $(viewonmap).attr('liid');
                var fieldName = $(viewonmap).attr('fld');
                var fieldTitle = $(viewonmap).attr("fldDisplayName");
                var apiKey = '';


                ExecuteOrDelayUntilEventNotified(function () {
                    GeolocationField.ShowMappyHover(fieldName, listItemId, values, apiKey);
                }, 'bingmapjsloaded');
            },
            InitMapPopup: function (viewonmap) {
                var listItemId = $(viewonmap).attr('liid');
                var popupMap = $('div[id="popupMap_' + listItemId + '"]');


                $(viewonmap).live('click', function () {
                    if ($(this).hasClass("map-selected")) {
                        GeolocationField.ClearMapPopup(viewonmap);
                    } else {
                        $(this).addClass("map-selected");
                        popupMap.slideFadeToggle(function () {
                            GeolocationField.ShowMapPopup(viewonmap);
                        });
                    }
                    return false;
                });

                popupMap.find(".map-closeButton").live('click', function () {
                    GeolocationField.ClearMapPopup(viewonmap);
                    return false;
                });
            },
            ClearMapPopup: function (viewonmap) {
                var listItemId = $(viewonmap).attr('liid');
                var popupMap = $('div[id="popupMap_' + listItemId + '"]');
                popupMap.slideFadeToggle(function () {
                    $(viewonmap).removeClass("map-selected");
                });
            }
        };
    })();

})();


(function ($) {
    $.fn.slideFadeToggle = function (easing, callback) {
        return this.animate({ opacity: 'toggle', height: 'toggle' }, "fast", easing, callback);
    };
})(jQuery);


function mapScriptLoaded() {
    if (typeof NotifyScriptLoadedAndExecuteWaitingJobs == "function") {
        ExecuteOrDelayUntilScriptLoaded(function () {
            NotifyEventAndExecuteWaitingJobs("bingmapjsloaded");
        }, 'SP.Map.js');
    }
}


function LoadBingMapsApi() {
    var bingMapControlApi = 'ecn.dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=7.0&onscriptload=mapScriptLoaded';

    if (typeof Microsoft != "undefined" && typeof Microsoft.Maps != "undefined" && typeof Microsoft.Maps.Location != "undefined") {
        CleanBingMapScripts();
    }
    var currentUICulture = "en";  //STSHtmlEncode(Strings.STS.L_CurrentUICulture_Name);
    var protocol = document.location.protocol;
    var mapScript = document.createElement('script');

    mapScript.type = "text/javascript";
    mapScript.id = "virtualearthscript";
    if ("https:" == protocol)
        mapScript.src = 'https://' + bingMapControlApi + '&s=1&mkt=' + currentUICulture;
    else
        mapScript.src = 'http://' + bingMapControlApi + '&mkt=' + currentUICulture;
    var head = (document.getElementsByTagName("head"))[0];

    head.appendChild(mapScript);
}
function CleanBingMapScripts() {
    var bingApiDomain = 'ecn.dev.virtualearth.net';
    var bingMapScripts = [];
    var head = (document.getElementsByTagName('head'))[0];
    var scriptElements = head.getElementsByTagName('script');
    var scriptlength = scriptElements.length;
    var script = null;

    for (var i = 0; i < scriptlength; i++) {
        var source = scriptElements[i].src;

        if (null != source && source.length > 0) {
            if ((source.toLowerCase()).indexOf(bingApiDomain) > 0) {
                bingMapScripts.push(scriptElements[i]);
            }
        }
    }
    scriptlength = bingMapScripts.length;
    for (i = 0; i < scriptlength; i++) {
        head.removeChild(bingMapScripts[i]);
    }
}