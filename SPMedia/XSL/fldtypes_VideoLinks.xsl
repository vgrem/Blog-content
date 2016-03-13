<xsl:stylesheet xmlns:x="http://www.w3.org/2001/XMLSchema"
                xmlns:d="http://schemas.microsoft.com/sharepoint/dsp"
                version="1.0" exclude-result-prefixes="xsl msxsl ddwrt"
                xmlns:ddwrt="http://schemas.microsoft.com/WebParts/v2/DataView/runtime"
                xmlns:asp="http://schemas.microsoft.com/ASPNET/20"
                xmlns:__designer="http://schemas.microsoft.com/WebParts/v2/DataView/designer"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:SharePoint="Microsoft.SharePoint.WebControls"
                xmlns:ddwrt2="urn:frontpage:internal">
  <xsl:template name ="RenderEmbeddedPlayer" match ="FieldRef[@Name='EmbeddedVideoOnForm']" mode="Computed_body" >
    <xsl:param name="thisNode" select="."/>
    <xsl:variable name="width">
      <xsl:call-template name="ensureVideoPlayerSize">
        <xsl:with-param name="videoSize" select="$thisNode/@VideoWidth"/>
        <xsl:with-param name="defaultSize" select="560"/>
      </xsl:call-template>
    </xsl:variable>
    <xsl:variable name="height">
      <xsl:call-template name="ensureVideoPlayerSize">
        <xsl:with-param name="videoSize" select="$thisNode/@VideoHeight"/>
        <xsl:with-param name="defaultSize" select="315"/>
      </xsl:call-template>
    </xsl:variable>
    <xsl:variable name="frameborder">
      <xsl:value-of select="$thisNode/@FrameBorder"/>
    </xsl:variable>
    <xsl:variable name="src">
      <xsl:value-of select="$thisNode/@URL"/>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="$thisNode/@EmbeddingMode ='IFrame'">
        <iframe width="{$width}" height="{$height}" src="{$src}" frameborder="{$frameborder}" allowfullscreen=""></iframe>
      </xsl:when>
      <xsl:when test="$thisNode/@EmbeddingMode ='Object(YouTube)'">
        <object width="{$width}" height="{$height}">
          <param name="movie" value="{$src}"></param>
          <param name="allowFullScreen" value="true"></param>
          <param name="allowscriptaccess" value="always"></param>
          <embed src="{$src}" type="application/x-shockwave-flash" width="{$width}" height="{$height}" allowscriptaccess="always" allowfullscreen="true"></embed>
        </object>
      </xsl:when>
      <xsl:when test="$thisNode/@EmbeddingMode ='Object(Qik)'">
        <object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,115,0" width="{$width}" height="{$height}" align="middle">
          <param name="allowScriptAccess" value="sameDomain" />
          <param name="allowFullScreen" value="true" />
          <param name="movie" value="{$src}" />
          <param name="quality" value="high" />
          <param name="bgcolor" value="#000000" />
          <param name="FlashVars" value="streamID=9d0242b2912a444e84a31c2ca3249268&amp;autoplay=false" />
          <embed src="{$src}" quality="high" bgcolor="#000000" width="{$width}" height="{$height}" name="qikPlayer" align="middle" allowScriptAccess="sameDomain" allowFullScreen="true" type="application/x-shockwave-flash" pluginspage="http://www.macromedia.com/go/getflashplayer" FlashVars="streamID=9d0242b2912a444e84a31c2ca3249268&amp;autoplay=false"></embed>
        </object>
      </xsl:when>
      <xsl:when test="$thisNode/@EmbeddingMode ='Object(CollegeHumor)'">
        <object type="application/x-shockwave-flash" data="{$src}" width="{$width}" height="{$height}">
          <param name="allowfullscreen" value="true"/>
          <param name="wmode" value="transparent"/>
          <param name="allowScriptAccess" value="always"/>
          <param name="movie" quality="best" value="{$src}"/>
          <embed src="{$src}" type="application/x-shockwave-flash" wmode="transparent" width="{$width}" height="{$height}" allowScriptAccess="always"></embed>
        </object>
      </xsl:when>
      <xsl:when test="$thisNode/@EmbeddingMode ='Object(Jest)'">
        <object type="application/x-shockwave-flash" data="{$src}" width="{$width}" height="{$height}">
          <param name="allowfullscreen" value="true"/>
          <param name="wmode" value="transparent"/>
          <param name="allowScriptAccess" value="always"/>
          <param name="movie" quality="best" value="{$src}"/>
          <embed src="{$src}" type="application/x-shockwave-flash" wmode="transparent" width="{$width}" height="{$height}" allowScriptAccess="always"></embed>
        </object>
      </xsl:when>
      <xsl:otherwise>
        <xsl:call-template name="renderEmbeddedCodeAsHtml">
          <xsl:with-param name="embedId" select="$thisNode/@ID" />
          <xsl:with-param name="embeddedCode" select="$thisNode/@EmbedCode" />
        </xsl:call-template>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template match ="FieldRef[@Name='EmbedCode']" ddwrt:dvt_mode="body"  mode="Note_body">
    <xsl:param name="thisNode" select="."/>
    <div class="videolink-embedcode">
      <xsl:value-of select="$thisNode/@EmbedCode" disable-output-escaping="yes"  />
    </div>
  </xsl:template>
  <xsl:template name="ensureVideoPlayerSize">
    <xsl:param name="videoSize"/>
    <xsl:param name="defaultSize"/>
    <xsl:choose>
      <xsl:when test="$videoSize &gt; 0">
        <xsl:value-of select="$videoSize"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$defaultSize"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template name="renderEmbeddedCodeAsHtml">
    <xsl:param name="embedId"/>
    <xsl:param name="embeddedCode"/>
    <xsl:variable name="embeddedCodeUrl">
      <xsl:value-of select="substring-before(substring-after($embeddedCode,'&gt;'),'&lt;/a&gt;')"/>
    </xsl:variable>
    <xsl:variable name="embeddedCodeFixed">
      <xsl:value-of select="substring-before($embeddedCode,'&lt;a')"/>
      <xsl:value-of select="$embeddedCodeUrl" />
      <xsl:value-of select="substring-after($embeddedCode,'&lt;/a&gt;')"/>
    </xsl:variable>
    <xsl:variable name="embeddedCodeHtml">
      <xsl:value-of select="$embeddedCodeFixed"   disable-output-escaping="yes" />   
    </xsl:variable>
    <div id="embeddedPlayerContainer{$embedId}">
    </div>
    <!--<xsl:value-of select="$embeddedCodeHtml" />-->
    <script type="text/javascript">
      var playerHost = 'embeddedPlayerContainer<xsl:value-of select="$embedId" />';
      var player = '<xsl:value-of select="$embeddedCodeFixed"  />';
      <![CDATA[
      player = player.replace(/&quot;/g, "'").replace(/&lt;/g, "<").replace(/&gt;/g, ">");
      document.getElementById(playerHost).innerHTML = player;
      ]]>
    </script>
  </xsl:template>
</xsl:stylesheet>