using System.ComponentModel;
using System.Management.Automation;

namespace SharePoint.Client.Navigation
{
    [RunInstaller(true)]
    public class PowerShell : PSSnapIn
    {
        public override string Name { get { return "SharePoint.Client.Navigation.PowerShell"; } }
        public override string Description { get { return "SharePoint Client Navigation cmdlets"; } }
        public override string Vendor { get { return "Vadim Gremyachev (http://blog.vgrem.com)"; } }
    }
}
