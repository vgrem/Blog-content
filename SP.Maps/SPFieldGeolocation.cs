using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace SP.Maps
{
    /// <summary>
    /// Geolocation Field 
    /// </summary>
    public class SPFieldGeolocation : SPField
    {



        public SPFieldGeolocation(SPFieldCollection fields, string fieldName) : base(fields, fieldName)
        {
        }

        public SPFieldGeolocation(SPFieldCollection fields, string typeName, string displayName) : base(fields, typeName, displayName)
        {
        }

        


        public override object GetFieldValue(string value)
        {
            return string.IsNullOrEmpty(value) ? null : GetValidatedGeolocationValue(value);
        }

        public override string GetFieldValueAsText(object value)
        {
            return this.GetValidatedString(value);
        }

        public override string GetValidatedString(object value)
        {
            if (value != null)
                value = GetValidatedGeolocationValue(value);
            return value == null ? string.Empty : value.ToString();
        }


        private SPFieldGeolocationValue GetValidatedGeolocationValue(object value)
        {
            return value == null ? null : new SPFieldGeolocationValue(value.ToString());
        }



        


        public override Type FieldValueType
        {
            get
            {
                return typeof(SPFieldGeolocationValue);
            }
        }

        public override bool Filterable
        {
            get
            {
                return false;
            }
        }

        public override bool Sortable
        {
            get
            {
                return false;
            }
        }

        public override BaseFieldControl FieldRenderingControl
        {
            get
            {
                var baseFieldControl = (BaseFieldControl)new GeolocationFieldControl();
                baseFieldControl.FieldName = InternalName;
                return baseFieldControl;
            }
        }

    }
}
