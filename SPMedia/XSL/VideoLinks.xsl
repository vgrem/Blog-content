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
  <xsl:import href="/_layouts/xsl/fldtypes_VideoLinks.xsl"/>
  <xsl:output method="html" indent="no"/>
  <xsl:template match="View[@BaseViewID='40']" mode="full" ddwrt:ghost="always">
    <tr class="ms-viewheadertr"></tr>
    <tr>
      <td>
        <ul id="embeddeditems" class="videolink-embed">
          <xsl:apply-templates select="." mode="RenderView" />
        </ul>
      </td>
    </tr>
    <xsl:apply-templates mode="footer" select="." />
  </xsl:template>
  <xsl:template mode="Item" match="Row[../../@BaseViewID='40']" ddwrt:ghost="always">
    <xsl:param name="Fields" select="."/>
    <xsl:param name="Collapse" select="."/>
    <xsl:param name="Position" select="1"/>
    <xsl:param name="Last" select="1"/>
    <xsl:variable name="thisNode" select="."/>
    <xsl:variable name="embedId" select="concat('embed_',$thisNode/@ID)"/>
    <li>
      <xsl:attribute name="id">
        <xsl:value-of select="$embedId"/>
      </xsl:attribute>
      <!--<xsl:attribute name="style">
      </xsl:attribute>-->
      <!--Embedded Container -->
      <div>
        <xsl:call-template name="RenderEmbeddedPlayer">
          <xsl:with-param name="thisNode" select="$thisNode"/>
        </xsl:call-template>
      </div>
      <div>
        <xsl:attribute name="class">videolinks-embed-props</xsl:attribute>
        <xsl:value-of select="$thisNode/@Comments" />
      </div>
    </li>
  </xsl:template>
  <xsl:template name="VideoLinksViewOverride" mode="RootTemplate" match="View[List/@TemplateType=10488]" ddwrt:dvt_mode="root">
    <link rel="stylesheet" Type="text/css" href="/_layouts/MediaExtensions/VideoLinks.css"/>
    <xsl:call-template name="View_Default_RootTemplate"/>
  </xsl:template>
</xsl:stylesheet>