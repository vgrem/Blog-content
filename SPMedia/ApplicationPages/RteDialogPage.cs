using System;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.ApplicationPages;
using Microsoft.SharePoint.Utilities;

namespace SPShareVideo.ApplicationPages
{
    public class RteDialogPage : RteDialogPageBase
    {
        protected override void OnLoad(EventArgs args)
        {
            base.OnLoad(args);
            string commandName = this.Request.QueryString["Dialog"];
            PageName.Text = SPHttpUtility.EcmaScriptStringLiteralEncode(commandName);
            if (commandName != null && string.Equals(commandName, "CreateVideoLink", StringComparison.OrdinalIgnoreCase))
                VideoEmbedCodePanel.Visible = false;
            else
            {
                VideoUrlPanel.Visible = false;
            }
           
        }



        protected Literal PageTitle;
        protected Literal PageName;
        protected PlaceHolder VideoUrlPanel;
        protected PlaceHolder VideoEmbedCodePanel;
        protected TextBox VideoUrlBox;
        protected TextBox VideoEmbedCodeBox;
        protected TextBox VideoWidthBox;
        protected TextBox VideoHeightBox;
    }
}
