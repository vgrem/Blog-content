function _registerMap() {
    Type.registerNamespace('SP.Maps');
    SP.Maps.IMapProvider = function() {
    };
    SP.Maps.IMapProvider.registerInterface('SP.Maps.IMapProvider');
    SP.Maps.MapOptions = function SP_Maps_MapOptions() {
        this.center = new SP.Maps.Geolocation(0, 0);
        this.disableKeyboardInput = false;
        this.disableUserInput = false;
        this.enableClickableLogo = false;
        this.enableSearchLogo = false;
        this.showCopyright = true;
        this.showDashboard = false;
        this.showLogo = true;
        this.showScalebar = true;
        this.zoom = 1;
    };
    SP.Maps.MapOptions.prototype = {
        center: null,
        credentials: null,
        disableKeyboardInput: false,
        disableUserInput: false,
        enableClickableLogo: false,
        enableSearchLogo: false,
        height: 0,
        showCopyright: false,
        showDashboard: false,
        showLogo: false,
        showScalebar: false,
        width: 0,
        zoom: 0
    };
    SP.Maps.PushpinOptions = function SP_Maps_PushpinOptions() {
        this.icon = null;
        this.text = null;
        this.typeName = null;
        this.visible = true;
        this.draggable = false;
    };
    SP.Maps.PushpinOptions.prototype = {
        icon: null,
        text: null,
        typeName: null,
        visible: false,
        draggable: false
    };
    SP.Maps.ViewOptions = function SP_Maps_ViewOptions() {
    };
    SP.Maps.ViewOptions.prototype = {
        bounds: null,
        center: null,
        zoom: 0
    };
    SP.Maps.MouseEventArgs = function SP_Maps_MouseEventArgs() {
    };
    SP.Maps.MouseEventArgs.prototype = {
        eventName: null,
        pageX: 0,
        pageY: 0,
        target: null,
        targetType: null,
        getX: function SP_Maps_MouseEventArgs$getX() {
            return 0;
        },
        getY: function SP_Maps_MouseEventArgs$getY() {
            return 0;
        }
    };
    SP.Maps.SPPushpin = function SP_Maps_SPPushpin() {
    };
    SP.Maps.SPPushpin.prototype = {
        latitude: 0,
        longitude: 0,
        isDraggable: false,
        showHover: false,
        label: null,
        pushpinID: null,
        icon: null
    };
    SP.Maps.SPMapOption = function SP_Maps_SPMapOption() {
    };
    SP.Maps.SPMapOption.prototype = {
        mapDiv: null,
        center: null,
        zoom: 0,
        height: 0,
        width: 0,
        apiKey: null
    };
    SP.Maps.Geolocation = function SP_Maps_Geolocation(latitude, longitude) {
        this.latitude = latitude;
        this.longitude = longitude;
    };
    SP.Maps.Geolocation.prototype = {
        latitude: 0,
        longitude: 0
    };
    SP.Maps.BingMaps = function SP_Maps_BingMaps() {
    };
    SP.Maps.BingMaps.isLoaded = function SP_Maps_BingMaps$isLoaded() {
        if (typeof window.Microsoft === 'undefined' || typeof window.Microsoft.Maps === 'undefined' || typeof window.Microsoft.Maps.Location === 'undefined') {
            return false;
        }
        return true;
    };
    SP.Maps.BingMaps.isBlocked = function SP_Maps_BingMaps$isBlocked() {
        return SP.Maps.BingMaps.isBingMapsBlocked;
    };
    SP.Maps.BingMaps.blockedStatus = function SP_Maps_BingMaps$blockedStatus(isBlocked) {
        SP.Maps.BingMaps.isBingMapsBlocked = isBlocked;
    };
    SP.Maps.BingMapProvider = function SP_Maps_BingMapProvider() {
        this.$$d_removePushpinOnHover = Function.createDelegate(this, this.removePushpinOnHover);
        this.$$d_showPushpinOnHover = Function.createDelegate(this, this.showPushpinOnHover);
        this.$2_0 = new SP.Maps.MapOptions();
        this.$3_0 = null;
        if (typeof window.Microsoft !== 'undefined' && typeof window.Microsoft.Maps !== 'undefined' && typeof window.Microsoft.Maps.EntityCollection !== 'undefined') {
            this.$3_0 = new Microsoft.Maps.EntityCollection();
            this.$4_0 = new Microsoft.Maps.EntityCollection();
        }
        this.$1_0 = false;
    };
    SP.Maps.BingMapProvider.prototype = {
        $0_0: null,
        $2_0: null,
        $3_0: null,
        $4_0: null,
        $1_0: false,
        normalizeLocation: function SP_Maps_BingMapProvider$normalizeLocation(latitude, longitude) {
            if (!SP.Maps.BingMaps.isLoaded()) {
                return null;
            }
            return new Microsoft.Maps.Location(latitude, Microsoft.Maps.Location.normalizeLongitude(longitude));
        },
        loadMap: function SP_Maps_BingMapProvider$loadMap(userMapOption) {
            if (!SP.ScriptUtility.isNullOrUndefined(userMapOption) && !SP.ScriptUtility.isNullOrUndefined(userMapOption.mapDiv)) {
                if (userMapOption.width > 0) {
                    userMapOption.mapDiv.style.width = userMapOption.width + 'px';
                }
                if (userMapOption.height > 0) {
                    userMapOption.mapDiv.style.height = userMapOption.height + 'px';
                }
                if (SP.Maps.BingMaps.isBlocked()) {
                    userMapOption.mapDiv.innerHTML = '<div style=\"position:absolute;width:100%;bottom:50%;text-align:center;\"><span>' + Strings.STS.L_BingMap_Blocked + '</span></div>';
                    return;
                }
                if (!SP.Maps.BingMaps.isLoaded()) {
                    userMapOption.mapDiv.innerHTML = '<div style=\"position:absolute;width:100%;bottom:50%;text-align:center;\"><span>' + Strings.STS.L_BingMap_NoInternetAccess + '</span></div>';
                    return;
                }
                if (!this.isNullOrUndefinedOrEmpty(userMapOption.apiKey)) {
                    this.$2_0.credentials = userMapOption.apiKey;
                }
                else {
                    this.$2_0.credentials = '';
                }
                if (!SP.ScriptUtility.isNullOrUndefined(userMapOption.center)) {
                    var $v_0 = this.normalizeLocation(userMapOption.center.latitude, userMapOption.center.longitude);

                    this.$2_0.center = new SP.Maps.Geolocation($v_0.latitude, $v_0.longitude);
                    if (userMapOption.zoom > 0) {
                        this.$2_0.zoom = userMapOption.zoom;
                    }
                    else {
                        this.$2_0.zoom = 15;
                    }
                }
                this.$0_0 = new Microsoft.Maps.Map(userMapOption.mapDiv, this.$2_0);
                this.$1_0 = true;
            }
        },
        isNullOrUndefinedOrEmpty: function SP_Maps_BingMapProvider$isNullOrUndefinedOrEmpty(str) {
            var $v_0 = null;
            var $v_1 = str;

            return $v_1 === $v_0 || typeof $v_1 === 'undefined' || !str.length;
        },
        clearMap: function SP_Maps_BingMapProvider$clearMap() {
            if (!this.$1_0) {
                return;
            }
            this.$0_0.entities.clear();
            this.$3_0.clear();
        },
        disposeMap: function SP_Maps_BingMapProvider$disposeMap() {
            if (!this.$1_0) {
                return;
            }
            this.$0_0.dispose();
        },
        disableKeyboardInputOnMap: function SP_Maps_BingMapProvider$disableKeyboardInputOnMap(disableKeyboardInput) {
            if (!this.$1_0) {
                return;
            }
            if (disableKeyboardInput) {
                this.$2_0.disableKeyboardInput = true;
            }
            else {
                this.$2_0.disableKeyboardInput = false;
            }
            this.$0_0.setOptions(this.$2_0);
        },
        addPushpins: function SP_Maps_BingMapProvider$addPushpins(pushpins) {
            if (!this.$1_0) {
                return;
            }
            if (!pushpins || !pushpins.length) {
                return;
            }
            var $v_0;
            var $v_1;
            var $v_2 = new SP.Maps.ViewOptions();
            var $v_3;
            var $v_4 = pushpins.length;
            var $v_5 = 0, $v_6 = 0;

            for ($v_6 = 0; $v_6 < $v_4; $v_6++) {
                if (pushpins[$v_6]) {
                    $v_0 = this.normalizeLocation(pushpins[$v_6].latitude, pushpins[$v_6].longitude);
                    $v_1 = new SP.Maps.PushpinOptions();
                    $v_1.draggable = pushpins[$v_6].isDraggable;
                    $v_1.text = pushpins[$v_6].label;
                    if (!this.isNullOrUndefinedOrEmpty(pushpins[$v_6].icon)) {
                        $v_1.icon = pushpins[$v_6].icon;
                    }
                    $v_1.typeName = 'ms-pushpin';
                    $v_3 = new Microsoft.Maps.Pushpin($v_0, $v_1);
                    $v_3.pushpinID = pushpins[$v_6].pushpinID;
                    if (pushpins[$v_6].showHover) {
                        var $v_7, $v_8;

                        $v_7 = this.$$d_showPushpinOnHover;
                        $v_8 = this.$$d_removePushpinOnHover;
                        Microsoft.Maps.Events.addHandler($v_3, 'mouseover', $v_7);
                        Microsoft.Maps.Events.addHandler($v_3, 'mouseout', $v_8);
                        Microsoft.Maps.Events.addHandler($v_3, 'mousedown', $v_8);
                    }
                    this.$3_0.push($v_3);
                }
            }
            $v_5 = this.$3_0.getLength();
            if (!$v_5) {
                return;
            }
            if ($v_5 === 1) {
                $v_2.zoom = 15;
                $v_2.center = (this.$3_0.get(0)).getLocation();
            }
            else {
                var $v_9 = new Array($v_5);

                for ($v_6 = 0; $v_6 < $v_5; $v_6++) {
                    $v_9[$v_6] = (this.$3_0.get($v_6)).getLocation();
                }
                $v_2.bounds = Microsoft.Maps.LocationRect.fromLocations.apply(null, $v_9);
            }
            this.$0_0.setView($v_2);
            this.$0_0.entities.clear();
            this.$0_0.entities.push(this.$3_0);
        },
        addHighlightedPushpin: function SP_Maps_BingMapProvider$addHighlightedPushpin(pushpin) {
            if (!this.$1_0 || !pushpin) {
                return;
            }
            var $v_0 = this.normalizeLocation(pushpin.latitude, pushpin.longitude);
            var $v_1 = new SP.Maps.PushpinOptions();

            $v_1.draggable = pushpin.isDraggable;
            $v_1.text = pushpin.label;
            $v_1.icon = SP.Utilities.Utility.get_layoutsLatestVersionUrl() + 'images/MapPushpinHover.25x39x32.png';
            $v_1.typeName = 'ms-pushpin';
            var $v_2 = new Microsoft.Maps.Pushpin($v_0, $v_1);

            this.$4_0.push($v_2);
            if (this.$0_0.entities.indexOf(this.$4_0) === -1) {
                this.$0_0.entities.push(this.$4_0);
            }
        },
        removeHighlightedPushpin: function SP_Maps_BingMapProvider$removeHighlightedPushpin() {
            if (!this.$1_0) {
                return;
            }
            this.$4_0.clear();
        },
        moveMapToSelectedLocation: function SP_Maps_BingMapProvider$moveMapToSelectedLocation(targetLocation) {
            if (!this.$1_0 || !targetLocation) {
                return;
            }
            var $v_0 = this.normalizeLocation(targetLocation.latitude, targetLocation.longitude);
            var $v_1 = this.$0_0.tryLocationToPixel($v_0, 2);

            if (!($v_1.x >= 0 && $v_1.y >= 0 && $v_1.x < this.$0_0.getWidth() && $v_1.y < this.$0_0.getHeight())) {
                var $v_2 = new SP.Maps.ViewOptions();

                $v_2.center = $v_0;
                $v_2.zoom = this.$0_0.getZoom();
                this.$0_0.setView($v_2);
            }
        },
        showPushpinOnHover: function SP_Maps_BingMapProvider$showPushpinOnHover(e) {
            if (!this.$1_0) {
                return;
            }
            if (e.targetType != 'pushpin')
                return;
            var $v_0 = e.target;
            var $v_1 = this.$0_0.tryLocationToPixel(e.target.getLocation(), 2);

            MapViewTemplate.ShowMapPushpinHover($v_1.x, $v_1.y - 20, $v_0);
        },
        removePushpinOnHover: function SP_Maps_BingMapProvider$removePushpinOnHover(e) {
            if (!this.$1_0) {
                return;
            }
            if (e.targetType != 'pushpin')
                return;
            var $v_0 = e.target.pushpinID;

            MapViewTemplate.HideMapPushpinHover($v_0);
        }
    };
    SP.Maps.MapView = function SP_Maps_MapView() {
    };
    SP.Maps.MapView.getCenter = function SP_Maps_MapView$getCenter(points) {
        if (!SP.Maps.BingMaps.isLoaded() || SP.Maps.BingMaps.isBlocked()) {
            return null;
        }
        var $v_0 = null;
        var $v_1;
        var $v_2 = new Array(0);
        var $v_3 = 0;
        var $v_4 = points.length;

        for (var $v_5 = 0; $v_5 < $v_4; $v_5++) {
            if (points[$v_5]) {
                $v_0 = new Microsoft.Maps.Location(points[$v_5].latitude, Microsoft.Maps.Location.normalizeLongitude(points[$v_5].longitude));
                $v_2[$v_3++] = $v_0;
            }
        }
        if ($v_2.length > 1) {
            $v_1 = Microsoft.Maps.LocationRect.fromLocations.apply(null, $v_2);
            return $v_1.center;
        }
        else {
            return $v_0;
        }
    };
    SP.Maps.MapProviderFactory = function SP_Maps_MapProviderFactory() {
    };
    SP.Maps.MapProviderFactory.createMapProvider = function SP_Maps_MapProviderFactory$createMapProvider(name, version) {
        var $v_0 = name + '_' + version;
        var $v_1 = new SP.Maps.Geolocation(0, 0);
        var $v_2;

        switch ($v_0) {
        case 'bing_7.0':
            $v_2 = new SP.Maps.BingMapProvider();
            return $v_2;
        default:
            $v_2 = new SP.Maps.BingMapProvider();
            return $v_2;
        }
    };
    SP.Maps.MapOptions.registerClass('SP.Maps.MapOptions');
    SP.Maps.PushpinOptions.registerClass('SP.Maps.PushpinOptions');
    SP.Maps.ViewOptions.registerClass('SP.Maps.ViewOptions');
    SP.Maps.MouseEventArgs.registerClass('SP.Maps.MouseEventArgs');
    SP.Maps.SPPushpin.registerClass('SP.Maps.SPPushpin');
    SP.Maps.SPMapOption.registerClass('SP.Maps.SPMapOption');
    SP.Maps.Geolocation.registerClass('SP.Maps.Geolocation');
    SP.Maps.BingMaps.registerClass('SP.Maps.BingMaps');
    SP.Maps.BingMapProvider.registerClass('SP.Maps.BingMapProvider', null, SP.Maps.IMapProvider);
    SP.Maps.MapView.registerClass('SP.Maps.MapView');
    SP.Maps.MapProviderFactory.registerClass('SP.Maps.MapProviderFactory');
    function sp_map_initialize() {
        SP.Maps.BingMaps.isBingMapsBlocked = false;
    }
    ;
    sp_map_initialize();
    ///RegisterModuleInit("SP.Map.js", sp_map_initialize);
    NotifyScriptLoadedAndExecuteWaitingJobs("SP.Map.js");
}
ExecuteOrDelayUntilScriptLoaded(_registerMap, "sp.js");
