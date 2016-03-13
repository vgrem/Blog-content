<%@ Control Language="C#"   AutoEventWireup="false" %>
<%@Assembly Name="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@Register TagPrefix="SharePoint" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" namespace="Microsoft.SharePoint.WebControls"%>
<%@Register TagPrefix="ApplicationPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" namespace="Microsoft.SharePoint.ApplicationPages.WebControls"%>
<%@Register TagPrefix="SPHttpUtility" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" namespace="Microsoft.SharePoint.Utilities"%>
<%@ Register TagPrefix="wssuc" TagName="ToolBar" src="~/_controltemplates/ToolBar.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBarButton" src="~/_controltemplates/ToolBarButton.ascx" %>
<SharePoint:RenderingTemplate id="GeolocationField" runat="server">
	<Template>
		<span class="ms-formdescription"><SharePoint:EncodedLiteral ID="EncodedLiteral1" runat="server" text="<%$Resources:Geolocation,LocationField_Longitude%>" EncodeMethod='HtmlEncode'/></span>
		<asp:TextBox id="GeolocationFieldLongitude" maxlength="255" runat="server" Width="120px"/>
		<span class="ms-formdescription"><SharePoint:EncodedLiteral ID="EncodedLiteral2" runat="server" text="<%$Resources:Geolocation,LocationField_Latitude%>" EncodeMethod='HtmlEncode'/></span>
		<span>
		    <asp:TextBox id="GeolocationFieldLatitude" maxlength="255" runat="server" Width="120px"/>
		</span><br />
	</Template>
</SharePoint:RenderingTemplate>
<SharePoint:RenderingTemplate id="GeolocationFieldDisplay" runat="server">
	<Template>
	    <link rel="stylesheet" type="text/css" href="/_layouts/<%=System.Threading.Thread.CurrentThread.CurrentUICulture.LCID%>/styles/mapview.css"/>
        <script src="/_layouts/jquery-1.8.2.min.js"></script>
        <script src="/_layouts/sp.map.js"></script>
        <script src="/_layouts/sp.map.field.js"></script>
        <script src="/_layouts/sp.map.view.js"></script>
        <script type="text/javascript">
            _spBodyOnLoadFunctionNames.push('LoadBingMapsApi');
            function InitMapField(svalue, fieldName, listItemId) {
                var values = GeolocationField.ParseGeolocationValue(svalue);
                var apiKey = '';
                ExecuteOrDelayUntilEventNotified(function () {
                    GeolocationField.ShowMappyHover(fieldName, listItemId, values, apiKey);
                }, 'bingmapjsloaded');
            }
        </script>
	    <div style="display: none">
		<span class="ms-formdescription"><SharePoint:EncodedLiteral ID="EncodedLiteral3" runat="server" text="<%$Resources:Geolocation,LocationField_Longitude%>" EncodeMethod='HtmlEncode'/></span>
		<asp:Label id="GeolocationFieldLongitudeDisplay" runat="server"/>
		<span class="ms-formdescription"><SharePoint:EncodedLiteral ID="EncodedLiteral4" runat="server" text="<%$Resources:Geolocation,LocationField_Latitude%>" EncodeMethod='HtmlEncode'/></span>
		<asp:Label id="GeolocationFieldLatitudeDisplay" runat="server"/>
        </div>
        <div id="mapContainer_<%=SPContext.Current.ListItem.ID%>" style="height:280px;position: relative">
          <a id="mapAnchor_<%=SPContext.Current.ListItem.ID%>" href="javascript:;" style="display:block; width:100%; height:100%;" title="Map"></a>
        </div>
	</Template>
</SharePoint:RenderingTemplate>