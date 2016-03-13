<xsl:stylesheet xmlns:x="http://www.w3.org/2001/XMLSchema"
                xmlns:d="http://schemas.microsoft.com/sharepoint/dsp"
                version="1.0"
                exclude-result-prefixes="xsl msxsl ddwrt"
                xmlns:ddwrt="http://schemas.microsoft.com/WebParts/v2/DataView/runtime"
                xmlns:asp="http://schemas.microsoft.com/ASPNET/20"
                xmlns:__designer="http://schemas.microsoft.com/WebParts/v2/DataView/designer"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:SharePoint="Microsoft.SharePoint.WebControls"
                xmlns:ddwrt2="urn:frontpage:internal">

  <xsl:template match="FieldRef[@FieldType='Geolocation']" mode="Text_body">
    <xsl:param name="thisNode" select="."/>
    <xsl:variable name="geolocationvalue" select="normalize-space($thisNode/@*[name()=current()/@Name])" />
    <xsl:variable name="iid">
      <xsl:value-of select="$ViewCounter"/>,<xsl:value-of select="$thisNode/@ID"/>,<xsl:value-of select="$thisNode/@FSObjType"/>
    </xsl:variable>

    <a class="map-fieldIcon">
      <!--<xsl:attribute name="href">javascript:void(0)</xsl:attribute>-->
      <xsl:attribute name="id">viewOnMap_<xsl:value-of select="$iid"/></xsl:attribute>
      <xsl:attribute name="liid">
        <xsl:value-of select="$iid"/>
      </xsl:attribute>
      <xsl:attribute name="fld">
        <xsl:value-of select="$thisNode/@Name"/>
      </xsl:attribute>
      <xsl:attribute name="fldDisplayName">
        <xsl:value-of select="$thisNode/@DisplayName"/>
      </xsl:attribute>
      <xsl:attribute name="fldValue">
        <xsl:value-of select="$geolocationvalue"/>
      </xsl:attribute>
      <img title="" border="0" src="/_layouts/images/ADDR_GETMAP.16x16x32.png"/>
    </a>

    <div class="map-popup">
      <xsl:attribute name="id">popupMap_<xsl:value-of select="$iid"/></xsl:attribute>
      <div>
        <a href="#" class="map-closeButton">
          <!--<xsl:attribute name="id">closeBtn_<xsl:value-of select="$iid"/></xsl:attribute>-->
          <img src="/_layouts/images/wndclose.png" alt=""/>
        </a>
      </div>
      <div style="height:280px;position: relative" >
        <xsl:attribute name="id">mapContainer_<xsl:value-of select="$iid"/></xsl:attribute>
        <a href="javascript:;" style="display:block; width:100%; height:100%;" title="Map">
          <xsl:attribute name="id">mapAnchor_<xsl:value-of select="$iid"/></xsl:attribute>
        </a>
      </div>
    </div>
  </xsl:template>

  

  <xsl:template name="emit_GeolocationInitialization">
    <link rel="stylesheet" type="text/css" href="/_layouts/{$LCID}/styles/mapview.css"/>
    <script src="/_layouts/jquery-1.8.2.min.js"></script>
    <script src="/_layouts/sp.map.js"></script>
    <script src="/_layouts/sp.map.field.js"></script>
    <!--<script src="/_layouts/sp.map.view.js"></script>-->
    <script type="text/javascript">
      _spBodyOnLoadFunctionNames.push('LoadBingMapsApi');
      function InitMapFields(){
        $(".map-fieldIcon").each(function(index, value){
          GeolocationField.InitMapPopup(value);
        });
      }
      _spBodyOnLoadFunctionNames.push("InitMapFields");
    </script>
  </xsl:template>

  <xsl:template ddwrt:dvt_mode="header" match="FieldRef[@FieldType='Geolocation']" mode="header">
    <th class="ms-vh2" nowrap="nowrap" scope="col" onmouseover="OnChildColumn(this)">
      <xsl:call-template name="dvt_headerfield">
        <xsl:with-param name="fieldname">
          <xsl:value-of select="@Name"/>
        </xsl:with-param>
        <xsl:with-param name="fieldtitle">
          <xsl:value-of select="@DisplayName"/>
        </xsl:with-param>
        <xsl:with-param name="displayname">
          <xsl:value-of select="@DisplayName"/>
        </xsl:with-param>
        <xsl:with-param name="fieldtype">x:string</xsl:with-param>
      </xsl:call-template>
      <xsl:call-template name="emit_GeolocationInitialization"/>
    </th>
  </xsl:template>

</xsl:stylesheet>

