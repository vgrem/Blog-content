using System;
using System.Security.Permissions;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.WebControls;

namespace SPShareVideo.WebControls
{
    public class ShareVideoRibbon : WebControl
    {
        [SharePointPermission(SecurityAction.Demand, ObjectModel = true)]
        public override void RenderBeginTag(HtmlTextWriter writer)
        {
        }

        [SharePointPermission(SecurityAction.Demand, ObjectModel = true)]
        public override void RenderEndTag(HtmlTextWriter writer)
        {
        }


        protected override void OnLoad(EventArgs e)
        {
            ScriptLink.Register(Page, "/_layouts/MediaExtensions/jquery.min.js", false);
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "RibbonInit", RibbonInitScript, true);
            ScriptLink.Register(Page, "/_layouts/MediaExtensions/YTPlayer.js", false);
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "youtube", "registerYTIFrameApi();", true);
            //Page.ClientScript.RegisterClientScriptBlock(GetType(), "youtube_player", "_spBodyOnLoadFunctionNames.push('loadYTPlayers');", true);
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "youtube_player", "loadYTPlayers();", true);
        }

        protected string RibbonInitScript
        {
            get
            {
                return "function _shareVideoRibbonLoad()\r\n" +
                       "{\r\n    " +
                       "   var fnd = function () " +
                       "   {" +
                       "      try {" +
                       "                  ShareVideoInit(); \r\n        " +
                       "          } \r\n        " +
                       "          catch (Ex)\r\n        " +
                       "          {}\r\n    " +
                       "   };\r\n    " +
                       "   RegisterSod('sharevideo_init', '/_layouts/MediaExtensions/ShareVideoRibbon.js');\r\n    " +
                       "   LoadSodByKey('sharevideo_init', fnd);\r\n" +
                       "}\r\n\r\n" +
                       "function shareVideoRibbonLoad()\r\n{ExecuteOrDelayUntilScriptLoaded(_shareVideoRibbonLoad, 'SP.Ribbon.js');\r\n}\r\n\r\n" +
                       "_spBodyOnLoadFunctionNames.push('shareVideoRibbonLoad');\r\n";
            }
        }

       

    }
}
