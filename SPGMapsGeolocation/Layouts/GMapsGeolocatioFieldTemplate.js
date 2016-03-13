

function $_global_googlemapscontrol() {
    (function() {
        if (typeof GMapsControlTemplate == "object") {
            return;
        }

        

        window.GMapsControlTemplate = (function() {
            return {
                GMapsControl_Display: function(rCtx) {
                    if (rCtx == null || rCtx.CurrentFieldValue == null || rCtx.CurrentFieldValue == '')
                        return '';
 
                    var _myData = SPClientTemplates.Utility.GetFormContextForCurrentField(rCtx);
                    if (_myData == null || _myData.fieldSchema == null)
                        return '';
                    _myData.registerInitCallback(_myData.fieldName, InitControl);

                    var gMapRedirectControl;
                    var _inputId_gMapRedirectControl = _myData.fieldName + '_' + 
                    _myData.fieldSchema.Id + '_$gMapField';

                    var fldvalue = GMapsControlTemplate.ParseGeolocationValue(rCtx.CurrentFieldValue);
                    var googleStaticMapUrl = GMapsControlTemplate.GetGoogleStaticMapUrl(fldvalue, 400, 300);

                    var result = '<div>';
                    result += GMapsControlTemplate.GetRenderableFieldValue(fldvalue);
                    result += '<BR />';
                    result += '<a id="' + STSHtmlEncode(_inputId_gMapRedirectControl) + '" href="javascript:">';
                    result += '<img src="' + googleStaticMapUrl + '" alt="Google Maps" />'
                    result += '</a>';
                    result += '</div>';

                    return result;

                    function RedirectToGMaps() {
                        var googleMapStaticUrl = document.getElementById(_inputId_gMapRedirectControl).childNodes[0].src;
                        window.open(googleMapStaticUrl);
                    }
                    function InitControl() {
                        gMapRedirectControl = document.getElementById(_inputId_gMapRedirectControl);
                        if (gMapRedirectControl != null)
                            AddEvtHandler(gMapRedirectControl, "onclick", RedirectToGMaps);
                    }
                },
                GetGoogleStaticMapUrl: function(fldvalue, width, height) {
                    var googleStaticMapUrl = 'http://maps.googleapis.com/maps/api/staticmap';
                    googleStaticMapUrl += '?center=' + fldvalue.latitude + ',' + fldvalue.longitude;
                    googleStaticMapUrl += '&zoom=11';
                    googleStaticMapUrl += '&size=' + width + 'x' + height;
                    googleStaticMapUrl += '&markers=color:blue%7Clabel:S%7C' + fldvalue.latitude + ',' + fldvalue.longitude;
                    googleStaticMapUrl += '&sensor=false';
                    return googleStaticMapUrl;
                },
                GetGoogleMapUrl: function (fldvalue) {
                    var googleMapUrl = 'https://maps.google.com/maps';
                    googleMapUrl += '?z=11';
                    googleMapUrl += '&t=m';
                    googleMapUrl += '&q=loc:' + fldvalue.latitude + '+' + fldvalue.longitude;
                    return googleMapUrl;
                },
                RedirectToGMaps: function(googleMapStaticUrl) {
                    var url = googleMapStaticUrl.replace("&nord", "");
                    window.open(url);
                },
                ParseGeolocationValue: function(fieldValue) {
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
                        }
                        else if (argIndex == 1) {
                            point.latitude = parseFloat(subStr);
                        }
                        else if (argIndex == 2) {
                            point.altitude = parseFloat(subStr);
                        }
                        else if (argIndex == 3) {
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
                BuildGeolocationValue: function(latitude, longitude) {
                    var geolocationValue = 'Point (' + longitude + ' ' + latitude + ')';
                    return geolocationValue;
                },
                GetRenderableFieldValue: function(fieldValue) {
                    var fldValue = 'Longitude: ' + fieldValue.longitude + ', Latitude: ' + fieldValue.latitude;
                    return fldValue;
                },
                GMapsControl_Edit: function(rCtx) {
                    if (rCtx == null)
                        return '';
                    
                    var _myData = SPClientTemplates.Utility.GetFormContextForCurrentField(rCtx);
                    if (_myData == null || _myData.fieldSchema == null)
                        return '';


                    downloadJS('http://maps.google.com/maps/api/js?sensor=false&callback=initialize');
                    


                    _myData.registerInitCallback(_myData.fieldName, InitControl);

                    var fldvalue = GMapsControlTemplate.ParseGeolocationValue(rCtx.CurrentFieldValue);
                    var controlIdHeader = _myData.fieldName + '_' + _myData.fieldSchema.Id + '_$';
                    var mapDiv = '';
                    var searchControl;
                    var gMapField;
                    var _inputId_gMapField = controlIdHeader + 'gMapField';
                    var _inputId_locationDisplayControl = controlIdHeader + 'locationDisplayControl';
                    var _inputId_nativeGeolocationValue = controlIdHeader + 'nativeGeolocationValue';
                    var googleStaticMapUrl = '';

                    mapDiv += '<div>';
                    mapDiv += '<H3><label id="' + _inputId_locationDisplayControl + '">';
                    if (fldvalue != null) {
                        mapDiv += GMapsControlTemplate.GetRenderableFieldValue(fldvalue);
                    }
                    mapDiv += '</H3></label>';
                    mapDiv += '<label id="' + _inputId_nativeGeolocationValue 
                               + '" style="visibility: hidden;">';
                    if (fldvalue != null) {
                        mapDiv += 'Point(' + fldvalue.longitude + ' ' + fldvalue.latitude +')';
                    }
                    mapDiv += '</label>';

                    mapDiv += '<a id="' + STSHtmlEncode(_inputId_gMapField) + '" href="javascript:">';
                    mapDiv += '<img alt="Loading..." src="';

                    if (fldvalue != null) {
                        googleStaticMapUrl = GMapsControlTemplate.GetGoogleStaticMapUrl(fldvalue, 400, 300);
                        mapDiv += googleStaticMapUrl;
                    }
                    mapDiv += '" />';
                    mapDiv += '</a>';
                    mapDiv += '</div>';

                    var _inputId_address = controlIdHeader + "address";
                    var _inputId_searchButton = controlIdHeader + "searchButton";

                    var result = '<div id="mainDiv">';
                    result += '<div id="inputControls">';
                    result += '<div id="searchControls">';
                    result += '<table>';
                    result += '<tr>';
                    result += '<td width=100%>';
                    result += '<input type="text" id="' + _inputId_address + '" value="" style="width: 100%;"/>';
                    result += '</td>';
                    result += '<td style="width: 100%; text-align: right;">';
                    result += '<input type="button" id="' + _inputId_searchButton 
                               + '" value="Search" style="width: 70%;" />';
                    result += '</td>';
                    result += '</tr>';
                    result += '</table>';
                    result += '</div>';
                    result += '<div id="mapControls" width=50px />';
                    result += mapDiv;
                    result += '</div>';
                    result += '</div>';

                    function downloadJS(jsFile) {
                        var thescript = document.createElement('script');
                        thescript.setAttribute('type','text/javascript');
                        //thescript.setAttribute('charset','UTF-8');
                        thescript.setAttribute('src',jsFile);
                        document.getElementsByTagName('head')[0].appendChild(thescript);
                    }

                   


                    function RedirectToGMaps() {
                        var googleMapStaticUrl = document.getElementById(_inputId_gMapField).childNodes[0].src;
                        window.open(googleMapStaticUrl);
                    }
                    function InitControl() {
                        gMapField = document.getElementById(_inputId_gMapField);
                        if (gMapField != null) {
                            AddEvtHandler(gMapField, "onclick", RedirectToGMaps);
                            
                            var fldValue = document.getElementById(_inputId_nativeGeolocationValue).textContent;
                            if(typeof fldValue == "undefined" || fldValue == '')
                            {
                                gMapField.setAttribute('style', 'visibility: hidden;');
                            }
                        }
                            
                        searchControl = document.getElementById(_inputId_searchButton);
                        if (searchControl != null)
                            AddEvtHandler(searchControl, "onclick", GeocodeAddress);
                        
                    }
                 

                    function GeocodeAddress() {
                        

                        if (typeof google == "undefined" || typeof google.maps.Geocoder == "undefined")
                        {
                            alert('Google Maps not loaded completelly!');
                            return;
                        }
                        
                        var addressControl = document.getElementById(_inputId_address);
                        if (addressControl == null)
                            return;
                            
                        var address = addressControl.value;

                        geocoder = new google.maps.Geocoder();


                        geocoder.geocode({ 'address': address }, function (results, status) {
                            if (status == google.maps.GeocoderStatus.OK) {
                                /*map.setCenter(results[0].geometry.location);
                                var marker = new google.maps.Marker({
                                    map: map,
                                    position: results[0].geometry.location
                                });*/
                                var point = new Object();
                                point.latitude = results[0].geometry.location.lat();
                                point.longitude = results[0].geometry.location.lng();
                                UpdateGeolocationValue(point.latitude, point.longitude);
                            } else {
                                alert("Geocode was not successful for the following reason: " + status);
                            }
                        });


                       

                        

                    }
                    function UpdateGeolocationValue(latitude, longitude) {
                        // Update native value.
                        document.getElementById(_inputId_nativeGeolocationValue).textContent =
                        GMapsControlTemplate.BuildGeolocationValue(latitude, longitude);
                        
                        // Update display value.
                        var point = new Object();
                        point.latitude = latitude;
                        point.longitude = longitude;
                        document.getElementById(_inputId_locationDisplayControl).textContent =
                           GMapsControlTemplate.GetRenderableFieldValue(point);
                        
                        // Update Map control.
                        var googleStaticMapUrl = GMapsControlTemplate.GetGoogleStaticMapUrl(point, 400, 400);
                        document.getElementById(_inputId_gMapField).childNodes[0].src 
                       = googleStaticMapUrl;
                        gMapField.setAttribute('style', 'visibility: none;');
                    }
                    
                    _myData.registerGetValueCallback(_myData.fieldName, function() {
                        var newValue = document.getElementById(_inputId_nativeGeolocationValue).textContent;
                        if(newValue == '')
                            return '';
                            
                        var newFldValue = GMapsControlTemplate.ParseGeolocationValue(newValue);
                        return "Point(" + String(newFldValue.longitude) + " " + String(newFldValue.latitude) + ")";
                    });
                    
                    return result;
                },
                GMapsControl_View: function(inCtx, field, listItem, listSchema) {
                    if (field.XSLRender == '1') {
                        return listItem[field.Name].toString();
                    }
                    else {
                        var fldvalue = GMapsControlTemplate.ParseGeolocationValue(listItem[field.Name]);
                        var ret = [];

                        if (fldvalue != null) {
                            ret.push("<a class=\"js-locationfield-callout\" href=\"javascript:void(0)\" liid=\"");
                            ret.push(GenerateIID(inCtx));
                            ret.push("\" fld=\"");
                            ret.push(field.Name);
                            ret.push("\" ><img title=\"");
                            ret.push(STSHtmlEncode(Strings.STS.L_Clippy_Tooltip));
                            ret.push("\"border=0 src=\"" + "/_layouts/15/images/callout-target.png");
                            ret.push("\"/></a>");
                        }
                        return ret.join('');
                    }
                },
                SetupMappyHoverHandlers: function(inCtx) {
                    EnsureScriptFunc("callout.js", "Callout", function() {
                        EnsureScriptFunc("core.js", "GetListItemByIID", function() {
                            EnsureScriptFunc("mquery.js", "m$", function() {
                                ((m$('.js-locationfield-callout')).not(".js-locationfield-calloutInitialized")).forEach(function(e) {
                                    var listItemID = e.getAttribute("liid");
                                var fieldName = e.getAttribute("fld");
                                var calloutTitle = '';
                                var calloutContent = [];
                                var listItem = GetListItemByIID(listItemID);
                                var values = GMapsControlTemplate.ParseGeolocationValue(listItem[fieldName]);
                                var width=300;
                                var googleMapStaticUrl = GMapsControlTemplate.GetGoogleStaticMapUrl
                                                        (values, width, 300);

                                calloutContent.push("<div><div class='ms-positionRelative' id='loc_mapcontainer_");
                                    calloutContent.push(listItemID);
                                calloutContent.push("_");
                                calloutContent.push(fieldName);
                                //calloutContent.push("' ></div></div>");
                                calloutContent.push("' ></div>");
                                calloutContent.push("<div>");
                                calloutContent.push('<img src="' + googleMapStaticUrl + '" alt="Google Maps" />');
                                calloutContent.push("</div>");
                                calloutContent.push("</div>");
                                    
                                var callout = CalloutManager.createNew({
                                    launchPoint: e,
                                    openOptions: {
                                        closeCalloutOnBlur: true,
                                        event: "click",
                                        showCloseButton: true
                                    },
                                    ID: listItemID + "_" + fieldName,
                                    title: calloutTitle,
                                    content: calloutContent.join(''),                                        
                                    contentWidth: width+40
                                });
                                    
                                callout.addAction(new CalloutAction({
                                    text: "Browse on Google Maps",
                                    onClickCallback: function () {
                                        var googleMapUrl = GMapsControlTemplate.GetGoogleMapUrl(values);
                                        GMapsControlTemplate.RedirectToGMaps(googleMapUrl);
                                    }
                                }));
                                    
                                (m$(e)).addClass("js-locationfield-calloutInitialized");
                            });
                        });
                    });
                });
        },
            GMapsControl_PreRender: function(inCtx) {
                    
            },
        GMapsControl_PostRender: function(inCtx) {
            if (ctx != null && ctx.BaseViewID != null && inCtx != null && inCtx.BaseViewID != null) {
                if (inCtx.BaseViewID == ctx.BaseViewID) {
                    GMapsControlTemplate.SetupMappyHoverHandlers(inCtx);                            
                }
            }
        }
    };
})();
        
function _registerGMapsControlTemplate() {
    var googleMapsControlContext = {};

    googleMapsControlContext.Templates = {};
    googleMapsControlContext.OnPreRender = GMapsControlTemplate.GMapsControl_PreRender;
    googleMapsControlContext.OnPostRender = GMapsControlTemplate.GMapsControl_PostRender;
    googleMapsControlContext.Templates.Fields = {
        // Provide custom field name while registering .
        'GMapFieldGeolocation': {
            'View': GMapsControlTemplate.GMapsControl_View,
            'DisplayForm': GMapsControlTemplate.GMapsControl_Display,
            'EditForm': GMapsControlTemplate.GMapsControl_Edit,
            'NewForm': GMapsControlTemplate.GMapsControl_Edit
        }
    };
    SPClientTemplates.TemplateManager.RegisterTemplateOverrides(googleMapsControlContext);
}
ExecuteOrDelayUntilScriptLoaded(_registerGMapsControlTemplate, 'clienttemplates.js');
})();
}

window.initialize = function () { }
$_global_googlemapscontrol();

