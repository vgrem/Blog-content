using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;

namespace SP.Maps.WebControls
{
    public class MapViewFormatControl : UserControl
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            UrlViewNew = "ViewNew.aspx?List=" + SPHttpUtility.UrlKeyValueEncode(Request.QueryString["List"]);
            string sourceParameter = Request.QueryString["Source"];
            if (!String.IsNullOrEmpty(sourceParameter))
            {
                UrlViewNew += "&amp;Source=" + SPHttpUtility.HtmlUrlAttributeEncode(SPHttpUtility.UrlKeyValueEncode(sourceParameter));
            }
            string nameParameter = Request.QueryString["viewname"];
            if (!String.IsNullOrEmpty(nameParameter))
            {
                UrlViewNew += "&amp;viewname=" + SPHttpUtility.HtmlUrlAttributeEncode(SPHttpUtility.UrlKeyValueEncode(nameParameter));
            }
            string mapViewStatusParameter = Request.QueryString["mapview"];
            if (!String.IsNullOrEmpty(mapViewStatusParameter))
                return;
            



            if (IsMapViewPage)
            {
                var viewctx = SPContext.Current.ViewContext;
                ConfigureMapView(viewctx.View);
                var viewUrl = String.Format("{0}?mapview=true",viewctx.View.ServerRelativeUrl);
                Response.Redirect(viewUrl);
            }
       
        }


        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!IsViewTypeSettingsPage)
                return;
            if (ContainsGeolocationField(SPContext.Current.List))
                RegisterMapViewType();
        }


        private void RegisterMapViewType()
        {
            if (!IsRegistered)
            {
                MapView.Visible = true;
            }

        }

        /// <summary>
        /// Configure Map View for List
        /// </summary>
        /// <param name="view"></param>
        private void ConfigureMapView(SPView view)
        {
            var web = SPContext.Current.Web;
            web.AllowUnsafeUpdates = true;
            view.XslLink = "mapview.xsl";
            var geolocationField = GetGeolocationField(SPContext.Current.List);
            view.ParameterBindings += "<ParameterBinding Name=\"IsMap\" DefaultValue=\"1\" />" +
                string.Format("<ParameterBinding Name=\"GeolocationFieldName\" DefaultValue=\"{0}\" />", (geolocationField == null ? string.Empty : geolocationField.InternalName)) +
                "<ParameterBinding Name=\"BingMapsKey\" DefaultValue=\"\" />" +
                "<ParameterBinding Name=\"IsBingMapBlockedInCurrentRegion\" DefaultValue=\"False\" />";
            view.Update();
            web.AllowUnsafeUpdates = false;
        }

        /// <summary>
        /// Determine if List contains SPFieldGeolocation field
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private static bool ContainsGeolocationField(SPList list)
        {
            return list.Fields.Cast<SPField>().Any(f => f.GetType().Name.Equals("SPFieldGeolocation", StringComparison.OrdinalIgnoreCase));
        }


        /// <summary>
        /// Get SPFieldGeolocation field in List
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private static SPField GetGeolocationField(SPList list)
        {
            return list.Fields.Cast<SPField>().FirstOrDefault(f => f.GetType().Name.Equals("SPFieldGeolocation", StringComparison.OrdinalIgnoreCase));
        }


        /// <summary>
        /// Find control recursivelly 
        /// (http://weblogs.asp.net/eporter/archive/2007/02/24/asp-net-findcontrol-recursive-with-generics.aspx)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controls"></param>
        /// <param name="filter"> </param>
        /// <returns></returns>
        public static T FindControl<T>(System.Web.UI.ControlCollection controls, Func<Control, bool> filter) where T : class
        {
            T found = default(T);

            if (controls != null && controls.Count > 0)
            {
                for (int i = 0; i < controls.Count; i++)
                {
                    if (found != null) break;
                    if (controls[i] is T && filter(controls[i]))
                    {
                        found = controls[i] as T;
                        break;
                    }
                    found = FindControl<T>(controls[i].Controls,filter);
                }
            }

            return found;
        }


        /// <summary>
        /// Is Map View Page
        /// </summary>
        private bool IsMapViewPage
        {
            get
            {
                if (Request.UrlReferrer != null)
                {
                    var isMapView = (Request.UrlReferrer.Query.IndexOf("Map=True", StringComparison.OrdinalIgnoreCase) >= 0);

                    return isMapView && (Request.UrlReferrer.AbsoluteUri.IndexOf(ViewNewSettingsPages, StringComparison.OrdinalIgnoreCase) >= 0);
                }
                return false;
            }
        }


        private bool IsViewNewSettingsPage
        {
            get
            {
                string pageName = Path.GetFileName(Page.Request.PhysicalPath);
                return ViewNewSettingsPages.Equals(pageName, StringComparison.OrdinalIgnoreCase);
            }
        }

        private bool IsViewTypeSettingsPage
        {
            get
            {
                string pageName = Path.GetFileName(Page.Request.PhysicalPath);
                return ViewTypeSettingsPages.Equals(pageName, StringComparison.OrdinalIgnoreCase);
            }
        }


      

        private bool IsRegistered 
        {
            
            get
            {
                var registeredMapView = FindControl<PlaceHolder>(Page.Controls, (c) => c.ID == "MapView" && c.Visible);
                return registeredMapView != null;
            } 
            
        }


        protected PlaceHolder MapView;
        protected string UrlViewNew;
        private const string ViewNewSettingsPages = "viewnew.aspx";
        private const string ViewTypeSettingsPages = "viewtype.aspx";
        private const string ViewEditSettingsPages = "viewedit.aspx";

    }
}
