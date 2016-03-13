<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Page Language="C#" Inherits="SPShareVideo.ApplicationPages.RteDialogPage" MasterPageFile="~/_layouts/RteDialog.master"       %> 
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Import Namespace="Microsoft.SharePoint" %> <%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Import Namespace="Microsoft.SharePoint" %> <%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<asp:Content ID="Content1" ContentPlaceHolderId="PlaceHolderRtePageTitle" runat="server">
	<asp:Literal id="PageTitle" runat="server"/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderId="PlaceHolderRteAdditionalOnLoadScript" runat="server">
	document.pageName = "<asp:Literal id="PageName" runat="server"/>";
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderId="PlaceHolderRteOnAfterDialogResize" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderId="PlaceHolderRteAdditionalScript" runat="server">
			function OkButtonClick()
			{
				var videoProperties = new Array();
				var valid = 1;
				if (document.pageName == "CreateVideoLink")
				{
					videoProperties[0] = (document.getElementById("<%= VideoUrlBox.ClientID%>")).value;
                    videoProperties[1] = (document.getElementById("<%= VideoWidthBox.ClientID%>")).value;
                    videoProperties[2] = (document.getElementById("<%= VideoHeightBox.ClientID%>")).value;
				}
				if (valid)
				{
<% if (UseDivDialog) { %>
					commonModalDialogClose(1 , videoProperties);
<% } else { %>
					window.returnValue = videoProperties;
					window.close();
<% } %>
				}
			}
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderId="PlaceHolderRteDialogBody" runat="server">
			<table cellspacing="5" id="DialogTable" class="ms-rtedialog">
				<asp:PlaceHolder runat="server" id="VideoUrlPanel">
					<tr>
						<td align="<SharePoint:EncodedLiteral runat='server' text='<%$Resources:wss,multipages_direction_right_align_value%>' EncodeMethod='HtmlEncode'/>"><label for="VideoUrlBox">Video ID</label></td>
						<td><asp:TextBox id="VideoUrlBox" TextMode="SingleLine" Columns="42" MaxLength="1024" runat="server" CssClass="ms-rtedialog-textfield"></asp:TextBox></td>
					</tr>
                    <tr>
						<td align="<SharePoint:EncodedLiteral runat='server' text='<%$Resources:wss,multipages_direction_right_align_value%>' EncodeMethod='HtmlEncode'/>">Video Size</td>
						<td>
						    <label for="VideoWidthBox">Width</label><asp:TextBox id="VideoWidthBox" Width="80px" TextMode="SingleLine" runat="server" CssClass="ms-rtedialog-textfield"></asp:TextBox>
                            <label for="VideoHeightBox">Height</label><asp:TextBox id="VideoHeightBox" Width="80px" TextMode="SingleLine" runat="server" CssClass="ms-rtedialog-textfield"></asp:TextBox>
                        </td>
					</tr>
				</asp:PlaceHolder>
                <asp:PlaceHolder runat="server" id="VideoEmbedCodePanel">
					<tr>
						<td align="<SharePoint:EncodedLiteral runat='server' text='<%$Resources:wss,multipages_direction_right_align_value%>' EncodeMethod='HtmlEncode'/>"><label for="VideoEmbedCodeBox">Embed Code</label></td>
						<td><asp:TextBox id="VideoEmbedCodeBox" TextMode="MultiLine" Columns="4" MaxLength="1024" runat="server" CssClass="ms-rtedialog-textfield"></asp:TextBox></td>
					</tr>
				</asp:PlaceHolder>
				<tr>
					<td colspan="2">&#160;</td>
				</tr>
			</table>
		<% string buttonDivStyle = GetResourceString("multipages_direction_right_align_value"); %>
		<div style="position: absolute; padding: 5px; bottom: 0px; <%= buttonDivStyle %>: 0px;">
			<input type="button" id="OKButton" class="ms-ButtonHeightWidth" value="<SharePoint:EncodedLiteral runat='server' text='<%$Resources:wss,multipages_okbutton_text%>' EncodeMethod='HtmlEncode'/>" onclick="OkButtonClick()" accesskey="<SharePoint:EncodedLiteral runat='server' text='<%$Resources:wss,okbutton_accesskey%>' EncodeMethod='HtmlEncode'/>"/>
			&#160;
			<input type="button" id="CancelButton" class="ms-ButtonHeightWidth" value="<SharePoint:EncodedLiteral runat='server' text='<%$Resources:wss,multipages_cancelbutton_text%>' EncodeMethod='HtmlEncode'/>" onclick="CancelButtonClick()" accesskey="<SharePoint:EncodedLiteral runat='server' text='<%$Resources:wss,cancelbutton_accesskey%>' EncodeMethod='HtmlEncode'/>"/>
		</div>
</asp:Content>


