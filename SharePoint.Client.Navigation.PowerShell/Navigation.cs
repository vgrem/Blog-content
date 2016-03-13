using System.Management.Automation;
using System.Net;
using Microsoft.SharePoint.Client;
using SharePoint.Client.Publishing.Navigation;

namespace SharePoint.Client.Navigation
{
    [Cmdlet(VerbsCommon.Get, "SetGlobalNavigation")]
    public class SetGlobalNavigation : Cmdlet
    {
        [Parameter]
        public ICredentials Credentials { get; set; }

        [Parameter]
        public string Url { get; set; }

        [Parameter]
        public bool IncludePages { get; set; }

        [Parameter]
        public bool IncludeSubSites { get; set; }

        protected override void ProcessRecord()
        {
            using (var ctx = new ClientContext(Url))
            {
                ctx.Credentials = Credentials;
                var navigation = new ClientPortalNavigation(ctx.Web);
                navigation.GlobalIncludePages = IncludePages;
                navigation.GlobalIncludeSubSites = IncludeSubSites;
                navigation.SaveChanges();
            }
        }
    }
}
