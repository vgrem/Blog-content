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
  <xsl:template match="View[@BaseViewID='10']" mode="full" ddwrt:ghost="always">
    <tr class="ms-viewheadertr"></tr>
    <tr>
      <td>
        <div id="accordionFAQ">
           <xsl:apply-templates select="." mode="RenderView" />
        </div>   
      </td>
    </tr>
    <xsl:apply-templates mode="footer" select="." />
  </xsl:template>
  <xsl:template mode="Item" match="Row[../../@BaseViewID='10']" ddwrt:ghost="always">
    <xsl:param name="Fields" select="."/>
    <xsl:param name="Collapse" select="."/>
    <xsl:param name="Position" select="1"/>
    <xsl:param name="Last" select="1"/>
    <xsl:variable name="thisNode" select="."/>
    <h3>
      <xsl:value-of select="$thisNode/@Title" />
    </h3>
    <div>
        <xsl:value-of select="$thisNode/@Answer" disable-output-escaping="yes" />
    </div>
  </xsl:template>
  <xsl:template name="FAQViewOverride" mode="RootTemplate" match="View[List/@TemplateType=11999]" ddwrt:dvt_mode="root">
    <link rel="stylesheet" href="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.10/themes/redmond/jquery-ui.css" />
    <script src="http://ajax.aspnetcdn.com/ajax/jquery/jquery-1.8.0.js"></script>
    <script src="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.22/jquery-ui.js"></script>
    <script>
      $(function() {
        $( "#accordionFAQ" ).accordion();
      });
    </script>
    <xsl:call-template name="View_Default_RootTemplate"/>
  </xsl:template>
</xsl:stylesheet>