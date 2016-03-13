(function () {
    if (typeof MapView == "object") {
        return;
    }

    window.MapView = (function () {

        return {
            CreateMap: function (geolocationPoints, apiKey) {
                
                
                var mapdiv = document.getElementById("mapContainer");
                var spMapOption = new SP.Maps.SPMapOption();

                spMapOption.mapDiv = mapdiv;
                spMapOption.apiKey = apiKey;
                var points = geolocationPoints;
                var center = SP.Maps.MapView.getCenter(points);

                if (center != null) {
                    spMapOption.center = center;
                    spMapOption.zoom = 10;
                }
                if (points.length == 1)
                    spMapOption.zoom = 15;
                var _activeElement = document.activeElement;
               
                var mapControl = new SP.Maps.MapProviderFactory.createMapProvider("Bing", "7.0");
                mapControl.loadMap(spMapOption);
                mapControl.addPushpins(points);
                mapControl.disableKeyboardInputOnMap(true);
                var _mapAnchor = document.getElementById("mapAnchor");

                if (_mapAnchor != null) {
                    AddEvtHandler(_mapAnchor, "onfocus", function () {
                        mapControl.disableKeyboardInputOnMap(false);
                    });
                    AddEvtHandler(_mapAnchor, "onblur", function () {
                        mapControl.disableKeyboardInputOnMap(true);
                    });
                    AddEvtHandler(mapdiv, "onmouseover", function () {
                        mapControl.disableKeyboardInputOnMap(false);
                    });
                    AddEvtHandler(mapdiv, "onmouseout", function () {
                        mapControl.disableKeyboardInputOnMap(true);
                    });
                }
                if (mapdiv != null) {
                    AddEvtHandler(mapdiv, "onclick", function () {
                        var _mapAnchor2 = document.getElementById("mapAnchor");

                        if (_mapAnchor2 != null) {
                            _mapAnchor2.focus();
                        }
                    });
                }

                
                if (_activeElement != null)
                    _activeElement.focus();
               
            }
        };
    })();

})();







