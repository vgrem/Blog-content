<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" Inherits="SP.Maps.WebControls.MapViewFormatControl" %>
<%@ Import Namespace="Microsoft.SharePoint.Utilities" %>
<script src="/_layouts/jquery-1.8.2.min.js"></script>
<script  type="text/javascript">
    function initCustomViewFormats() {
        $("#customViewFormats").insertAfter($("div[id$='MSO_ContentDiv'] > table:nth-child(3)"));
        $("#customViewFormats").show();
    }
    _spBodyOnLoadFunctionNames.push("initCustomViewFormats");
</script>
<asp:PlaceHolder runat="server" ID="MapView" Visible="False">
<table id="customViewFormats" style="display: none">    
    <tbody>
        <tr>
            <td valign="top" width="1%">
                <table border="0" cellspacing="0" cellpadding="0" style="padding-top: 2px">
                    <tbody>
                        <tr>
                            <td>
                                <a href='<%SPHttpUtility.NoEncode(UrlViewNew, Response.Output);%>&amp;Map=True&amp;'
                                    id="onetCategoryMapimg" tabindex="-1">
                                    <img src="/_layouts/images/Mapview.31x22x32.png" border="0" alt="View list data that includes a geographical location on a map.">
                                </a>
                            </td>
                            <td width="4px">
                                <img src="/_layouts/images/blank.gif" width="4" height="1" alt="">
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
            <td class="ms-vb" width="99%" valign="top" id="_spFocusHere">
                <a href='<%SPHttpUtility.NoEncode(UrlViewNew, Response.Output);%>&amp;Map=True&amp;'
                    id="onetCategoryMap">Map View </a>
                <br/>
                View list data that includes a geographical location on a map.
                <br/>
                &nbsp;
            </td>
        </tr>
    </tbody>
</table>
</asp:PlaceHolder>
