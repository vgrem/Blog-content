using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;

namespace SharePointGMapsGeolocation
{
    public class GMapFieldGeolocation : SPFieldGeolocation
    {
        #region ctors

        public GMapFieldGeolocation(SPFieldCollection fields, string fieldName)
            : base(fields, fieldName)
        {
        }


        public GMapFieldGeolocation(SPFieldCollection fields, string typeName, string displayName)
            : base(fields, typeName, displayName)
        {
        }

        #endregion

        public override object GetFieldValue(string value)
        {
            return base.GetFieldValue(value);
        }


        public override string GetValidatedString(object value)
        {
            return base.GetValidatedString(value);
        }

        #region Properties

        public override string JSLink
        {
            get
            {
                return "GMapsGeolocatioFieldTemplate.js";
            }
            set
            {
                base.JSLink = value;
            }
        }


        #endregion



    }
}
