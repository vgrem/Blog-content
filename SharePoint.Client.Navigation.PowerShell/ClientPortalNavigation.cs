using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Microsoft.SharePoint.Client;

namespace SharePoint.Client.Publishing.Navigation
{
    /// <summary>
    /// Represents navigation for portal pages and other portal navigation objects
    /// </summary>
    class ClientPortalNavigation
    {
        public ClientPortalNavigation(Web web)
        {
            _web = web;
        }

        #region CRUD operations

        private void EnsureLoaded()
        {
            if (!_web.IsObjectPropertyInstantiated("AllProperties"))
            {
                Context.Load(_web, w => w.AllProperties);
                Context.ExecuteQuery();
            }
        }


        public void SaveChanges()
        {
            _web.Update();
            Context.ExecuteQuery();
        }


        #endregion



        public void ExcludeFromNavigation(bool useGlobal, Guid item)
        {
            ModifyNavigationExclude(true, useGlobal, item);
        }

        public void IncludeInNavigation(bool useGlobal, Guid item)
        {
            ModifyNavigationExclude(false, useGlobal, item);
        }

        /// <summary>
        /// Controls whether publishing pages in this site will be automatically included in global navigation.
        /// </summary>
        public bool GlobalIncludePages
        {
            get
            {
                return (GlobalIncludeTypes & NodeTypes.Page) == NodeTypes.Page;
            }
            set
            {
                GlobalIncludeTypes = !value ? GlobalIncludeTypes & ~NodeTypes.Page : GlobalIncludeTypes | NodeTypes.Page;
            }
        }


        /// <summary>
        /// Controls whether publishing pages in this site will be automatically included in current navigation.
        /// </summary>
        public bool CurrentIncludePages
        {
            get
            {
                return (CurrentIncludeTypes & NodeTypes.Page) == NodeTypes.Page;
            }
            set
            {
                CurrentIncludeTypes = !value ? CurrentIncludeTypes & ~NodeTypes.Page : CurrentIncludeTypes | NodeTypes.Page;
            }
        }

        /// <summary>
        /// Controls the ordering of navigation items owned by this site.
        /// </summary>
        public OrderingMethod OrderingMethod
        {
            get
            {
                return (OrderingMethod)GetProperty("__NavigationOrderingMethod", 2);
            }
            set
            {
                SetProperty("__NavigationOrderingMethod", (int)value);
            }
        }


        /// <summary>
        /// Controls the property to use when automatically sorting navigation items owned by this site.
        /// </summary>
        public AutomaticSortingMethod AutomaticSortingMethod
        {
            get
            {
                return (AutomaticSortingMethod)GetProperty("__NavigationAutomaticSortingMethod", 0);
            }
            set
            {
                SetProperty("__NavigationAutomaticSortingMethod", (int)value);
            }
        }


        /// <summary>
        /// Sorts items in a portal navigation in ascending alphanumeric order.
        /// </summary>
        public bool SortAscending
        {
            get
            {
                return GetProperty("__NavigationSortAscending", true);
            }
            set
            {
                SetProperty("__NavigationSortAscending", value);
            }
        }

        /// <summary>
        /// Controls whether sub-site's of this site will be automatically included in its global navigation.
        /// </summary>
        public bool GlobalIncludeSubSites
        {
            get
            {
                return (GlobalIncludeTypes & NodeTypes.Area) == NodeTypes.Area;
            }
            set
            {
                GlobalIncludeTypes = !value ? GlobalIncludeTypes & ~NodeTypes.Area : GlobalIncludeTypes | NodeTypes.Area;
            }
        }


        public bool CurrentIncludeSubSites
        {
            get
            {
                return (CurrentIncludeTypes & NodeTypes.Area) == NodeTypes.Area;
            }
            set
            {
                CurrentIncludeTypes = !value ? CurrentIncludeTypes & ~NodeTypes.Area : CurrentIncludeTypes | NodeTypes.Area;
            }
        }


        #region Private Properties

        private NodeTypes CurrentIncludeTypes
        {
            get
            {
                return (NodeTypes)GetProperty("__CurrentNavigationIncludeTypes", 2);
            }
            set
            {
                SetProperty("__CurrentNavigationIncludeTypes", (int)value);
            }
        }

        private NodeTypes GlobalIncludeTypes
        {
            get
            {
                return (NodeTypes)GetProperty("__GlobalNavigationIncludeTypes", 2);
            }
            set
            {
                SetProperty("__GlobalNavigationIncludeTypes", (int)value);
            }
        }


        #endregion


        #region Private methods

        private void ModifyNavigationExclude(bool exclude, bool useGlobal, Guid item)
        {
            Dictionary<Guid, bool> excludes;
            if (useGlobal)
            {
                if (_globalNavigationExcludes == null)
                    InitializeNavigationExcludes(true);
                excludes = _globalNavigationExcludes;
            }
            else
            {
                if (_currentNavigationExcludes == null)
                    InitializeNavigationExcludes(false);
                excludes = _currentNavigationExcludes;
            }
            excludes[item] = exclude;
            SetProperty(useGlobal ? "__GlobalNavigationExcludes" : "__CurrentNavigationExcludes", ConcatenateGuidStrings(excludes));
        }

        internal static string ConcatenateGuidStrings(Dictionary<Guid, bool> excludes)
        {
            var guidStrings = new StringBuilder();
            foreach (var exclude in excludes)
            {
                if (exclude.Value)
                    guidStrings.Append(exclude.Key + ";");
            }
            return guidStrings.ToString();
        }


        private void InitializeNavigationExcludes(bool useGlobal)
        {
            var excludesValue = GetProperty(useGlobal ? "__GlobalNavigationExcludes" : "__CurrentNavigationExcludes", "");
            var excludeGuids = new string[0];
            if (!string.IsNullOrEmpty(excludesValue))
                excludeGuids = excludesValue.Split(new[] { ';' },StringSplitOptions.RemoveEmptyEntries);
            Dictionary<Guid, bool> excludes;
            if (useGlobal)
            {
                _globalNavigationExcludes = new Dictionary<Guid, bool>(excludeGuids.Length);
                excludes = _globalNavigationExcludes;
            }
            else
            {
                _currentNavigationExcludes = new Dictionary<Guid, bool>(excludeGuids.Length);
                excludes = _currentNavigationExcludes;
            }
            foreach (var g in excludeGuids)
            {
                if (!string.IsNullOrEmpty(g) && !g.Equals(true.ToString(CultureInfo.InvariantCulture), StringComparison.OrdinalIgnoreCase))
                    excludes.Add(new Guid(g), true);
            }
        }


        private T GetProperty<T>(string propName, T defaultPropValue)
        {
            EnsureLoaded();
            if (!_web.AllProperties.FieldValues.ContainsKey(propName))
                return defaultPropValue;
            return (T)Convert.ChangeType(_web.AllProperties[propName], typeof(T));
        }

        private void SetProperty<T>(string propName, T propValue)
        {
            _web.AllProperties[propName] = propValue;
        }


        #endregion


        private ClientRuntimeContext Context
        {
            get { return _web.Context; }
        }



        private readonly Web _web;
        private Dictionary<Guid, bool> _globalNavigationExcludes;
        private Dictionary<Guid, bool> _currentNavigationExcludes;
        internal const string PropNameIncludePagesInNavigation = "__IncludePagesInNavigation";
    }

   
    #region NodeTypes

    /// <summary>
    /// Represents the various node types in Microsoft SharePoint Foundation.
    /// </summary>
    [Flags]
    public enum NodeTypes
    {
        None = 0,
        Area = 1,
        Page = 2,
        List = 4,
        ListItem = 8,
        PageLayout = 16,
        Heading = 32,
        AuthoredLinkToPage = 64,
        AuthoredLinkToWeb = 128,
        AuthoredLinkPlain = 256,
        Custom = 512,
        Error = 1024,
        AuthoredLink = AuthoredLinkPlain | AuthoredLinkToWeb | AuthoredLinkToPage,
        Default = AuthoredLink | Heading | Page | Area,
        All = Default | Custom | PageLayout | ListItem | List,
    }

    #endregion


    #region AutomaticSortingMethod

    /// <summary>
    /// Provides options that specify which property to use when automatically sorting navigation items.
    /// </summary>
    public enum AutomaticSortingMethod
    {
        Title,
        CreatedDate,
        LastModifiedDate,
    }

    #endregion


    #region OrderingMethod

    /// <summary>
    /// Options that specify how navigation items are ordered.
    /// </summary>
    public enum OrderingMethod
    {
        Automatic,
        ManualWithAutomaticPageSorting,
        Manual,
    }

    #endregion
}