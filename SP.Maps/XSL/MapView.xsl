<xsl:stylesheet xmlns:x="http://www.w3.org/2001/XMLSchema"
  xmlns:d="http://schemas.microsoft.com/sharepoint/dsp"
  version="1.0" exclude-result-prefixes="xsl msxsl ddwrt x d asp __designer SharePoint ddwrt2"
  xmlns:ddwrt="http://schemas.microsoft.com/WebParts/v2/DataView/runtime"
  xmlns:asp="http://schemas.microsoft.com/ASPNET/20"
  xmlns:__designer="http://schemas.microsoft.com/WebParts/v2/DataView/designer"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:msxsl="urn:schemas-microsoft-com:xslt"
  xmlns:SharePoint="Microsoft.SharePoint.WebControls"
  xmlns:ddwrt2="urn:frontpage:internal">
  <xsl:import href="/_layouts/xsl/main.xsl"/>
  <xsl:output method="html" indent="no"/>
  <xsl:param name="IsMap"/>
  <xsl:param name="GeolocationFieldName"/>
  <xsl:param name="BingMapsKey"/>
  <xsl:param name="IsBingMapBlockedInCurrentRegion"/>
  <xsl:template name="emit_MapViewInitialization">
    <script type="text/javascript">
      var GeolocationPoints = [];
      <xsl:for-each select="$AllRows">
        <xsl:variable name="thisNode" select="."/>
        <xsl:variable name="geolocationvalue" select="normalize-space($thisNode/@*[name()=$GeolocationFieldName])" />
        

        var value = GeolocationField.ParseGeolocationValue('<xsl:value-of select="$geolocationvalue" disable-output-escaping="yes"/>');

        if (value != null) {
           var point = new Object();

           point.latitude = value.latitude;
           point.longitude = value.longitude;
           point.isDraggable = false;
           point.showHover = false;
           point.label = "<xsl:value-of select='$thisNode/@ID'/>";
           point.pushpinID = "<xsl:value-of select="$ViewCounter"/>,<xsl:value-of select="$thisNode/@ID"/>,<xsl:value-of select="$thisNode/@FSObjType"/>";
           point.icon = "/_layouts/images/MapPushpin.25x39x32.png";
           GeolocationPoints.push(point);
        }
      </xsl:for-each>

      ExecuteOrDelayUntilEventNotified(function() {
         var apiKey = '<xsl:value-of select='$BingMapsKey'/>';
         MapView.CreateMap(GeolocationPoints, apiKey);
      }, 'bingmapjsloaded');

    </script>
  </xsl:template>
  <xsl:template name="GeoMapViewOverride" mode="RootTemplate" match="View" ddwrt:dvt_mode="root">
    <xsl:choose>
      <xsl:when test="$IsMap='1'">
        <link rel="stylesheet" type="text/css" href="/_layouts/{$LCID}/styles/mapview.css"/>
        <script src="/_layouts/jquery-1.8.2.min.js"></script>
        <script src="/_layouts/sp.map.js"></script>
        <script src="/_layouts/sp.map.field.js"></script>
        <script src="/_layouts/sp.map.view.js"></script>
        <script type="text/javascript">
          _spBodyOnLoadFunctionNames.push('LoadBingMapsApi');
        </script>
        <xsl:call-template name="emit_MapViewInitialization"></xsl:call-template>
       
        <div id="mapContainer" class="ms-positionRelative ms-fullWidth"  style="height:480px;">
          <a id="mapAnchor" href="javascript:;" style="display:block; width:100%; height:100%;" title="Map"></a>
        </div>
        
      </xsl:when>
      <xsl:otherwise>
        <xsl:call-template name="View_Default_RootTemplate"/>
      </xsl:otherwise>
    </xsl:choose>

  </xsl:template>
</xsl:stylesheet>