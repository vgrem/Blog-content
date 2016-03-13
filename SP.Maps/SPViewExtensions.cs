using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.SharePoint;

namespace SP.Maps
{
    public static class SPViewExtensions
    {
       

        public static string GetViewProperty(this SPView view, string name)
        {
            var methodInfo = typeof(SPView).GetMethod("GetAttributeValue", BindingFlags.NonPublic | BindingFlags.Instance);
            var attrValue = methodInfo.Invoke(view, new object[] { name });
            return attrValue == null ? string.Empty : attrValue.ToString();
        }

        public static void SetViewProperty(this SPView view, string name, string value)
        {
            var methodInfo = typeof(SPView).GetMethod("SetAttributeValue", BindingFlags.NonPublic | BindingFlags.Instance);
            var res = methodInfo.Invoke(view, new object[] { name, value });
        }


    }
}
