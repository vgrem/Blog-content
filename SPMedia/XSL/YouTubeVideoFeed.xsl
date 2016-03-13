<xsl:stylesheet xmlns:x="http://www.w3.org/2001/XMLSchema"
               version="1.0" exclude-result-prefixes="xsl ddwrt msxsl rssaggwrt"
               xmlns:ddwrt="http://schemas.microsoft.com/WebParts/v2/DataView/runtime"
               xmlns:rssaggwrt="http://schemas.microsoft.com/WebParts/v3/rssagg/runtime"
               xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt"
               xmlns:rssFeed="urn:schemas-microsoft-com:sharepoint:RSSAggregatorWebPart"
               xmlns:rdf="http://www.w3.org/1999/02/22-rdf-syntax-ns#" xmlns:dc="http://purl.org/dc/elements/1.1/"
               xmlns:rss1="http://purl.org/rss/1.0/" xmlns:atom="http://www.w3.org/2005/Atom"
               xmlns:itunes="http://www.itunes.com/dtds/podcast-1.0.dtd"
               xmlns:atom2="http://purl.org/atom/ns#" xmlns:ddwrt2="urn:frontpage:internal"
               xmlns:media="http://search.yahoo.com/mrss/"
               xmlns:yt="http://gdata.youtube.com/schemas/2007">

  <xsl:param name="rss_FeedLimit">3</xsl:param>
  <xsl:param name="rss_ExpandFeed">false</xsl:param>
  <xsl:param name="rss_LCID">1033</xsl:param>
  <xsl:param name="rss_WebPartID">RSS_Viewer_WebPart</xsl:param>
  <xsl:param name="rss_alignValue">left</xsl:param>
  <xsl:param name="rss_IsDesignMode">True</xsl:param>


  <xsl:template match="atom:feed">
    <link rel="stylesheet" Type="text/css" href="/_layouts/MediaExtensions/VideoLinks.css"/>
    <xsl:call-template name="ATOMYouTubeTemplate"/>
  </xsl:template>

  <xsl:template name="ATOMYouTubeTemplate" xmlns:ddwrt="http://schemas.microsoft.com/WebParts/v2/DataView/runtime" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt">
    <xsl:variable name="Rows" select="atom:entry"/>
    <xsl:variable name="RowCount" select="count($Rows)"/>
    <div class="channel-browse" >
      <div class="channels-browse-gutter-padding" >
        <ul class="channels-browse-content-grid context-data-container ">
          <xsl:call-template name="ATOMYouTubeTemplate.body">
            <xsl:with-param name="Rows" select="$Rows"/>
            <xsl:with-param name="RowCount" select="count($Rows)"/>
          </xsl:call-template>
        </ul>
      </div>
    </div>
  </xsl:template>


  <xsl:template name="ATOMYouTubeTemplate.body" xmlns:ddwrt="http://schemas.microsoft.com/WebParts/v2/DataView/runtime" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt">
    <xsl:param name="Rows"/>
    <xsl:param name="RowCount"/>
    <xsl:for-each select="$Rows">
      <xsl:variable name="CurPosition" select="position()" />
      <xsl:variable name="RssFeedLink" select="$rss_WebPartID" />
      <xsl:variable name="CurrentElement" select="concat($RssFeedLink,$CurPosition)" />
      <xsl:if test="($CurPosition &lt;= $rss_FeedLimit)">
        <li class="channels-content-item">
          <xsl:call-template name="ATOMYouTubeTemplate.contentitem">
            <xsl:with-param name="CurrentElement" select="$CurrentElement"/>
          </xsl:call-template>
        </li>
      </xsl:if>
    </xsl:for-each>
  </xsl:template>

  <xsl:template name="ATOMYouTubeTemplate.contentitem" xmlns:ddwrt="http://schemas.microsoft.com/WebParts/v2/DataView/runtime" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt">
    <xsl:param name="CurrentElement"/>
    <span class="context-data-item">
      <a onclick="javascript:window.open(this.href, 'YouTube', 'height=600,width=800,resizable');return false;" href="{ddwrt:EnsureAllowedProtocol(string(atom:link/@href))}" class="ux-thumb-wrap" >
        <span class="video-thumb">
          <span class="yt-thumb-clip">
            <span class="yt-thumb-clip-inner">
              <xsl:variable name="ThumbnailUrl" select="media:group/media:thumbnail[@width='120']/@url">
              </xsl:variable>
              <img src="{$ThumbnailUrl}" alt="Thumbnail" width="194" />
              <span class="vertical-align"></span>
            </span>
          </span>
        </span>
        <span class="video-time">
          <xsl:value-of select="media:group/yt:duration/@seconds" /> sec
        </span>
      </a>
      <!--<a href="{ddwrt:EnsureAllowedProtocol(string(atom:link/@href))}" title="{string(atom:title)}" class="content-item-title spf-link" dir="ltr">-->
      <a onclick="javascript:window.open(this.href, 'YouTube', 'height=600,width=800,resizable');return false;" href="{ddwrt:EnsureAllowedProtocol(string(atom:link/@href))}"   title="{string(atom:title)}" class="content-item-title spf-link" dir="ltr">
        <xsl:call-template name="GetSafeHtml">
          <xsl:with-param name="Html" select="substring(atom:title, 1, 28)"/>
        </xsl:call-template>...
      </a>
      <span class="content-item-detail">

        <span class="content-item-view-count">
          <xsl:value-of select="yt:statistics/@viewCount" /> views
        </span>
        <span class="metadata-separator">|</span>
        <span class="content-item-time-created">
          <xsl:value-of select="ddwrt:FormatDate(atom:published, 2057, 3)"/>
        </span>
      </span>
    </span>

  </xsl:template>



  <xsl:template name="GetSafeHtml">
    <xsl:param name="Html"/>
    <xsl:choose>
      <xsl:when test="$rss_IsDesignMode = 'True'">
        <xsl:value-of select="$Html"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="rssaggwrt:MakeSafe($Html)"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>


</xsl:stylesheet>