using System;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;

namespace SP.Maps
{
   
    public class GeolocationFieldControl : BaseFieldControl
    {

        protected override void CreateChildControls()
        {

            ScriptManager.RegisterStartupScript(this, GetType(), "InitMapField", string.Format("InitMapField('{0}','{1}','{2}');", Value, this.Field.InternalName, SPContext.Current.ListItem.ID), true);

            if (this.IsFieldValueCached)
            {
                base.CreateChildControls();
            }
            else
            {
                if ((SPFieldGeolocation)this.Field == null)
                    return;
                base.CreateChildControls();
                if (this.ControlMode == SPControlMode.Display)
                {
                    m_longitudeLabel = (Label)TemplateContainer.FindControl("GeolocationFieldLongitudeDisplay");
                    m_latitudeLabel = (Label)TemplateContainer.FindControl("GeolocationFieldLatitudeDisplay");
                    if (m_longitudeLabel == null || m_latitudeLabel == null)
                        throw new ArgumentException(SPResource.GetString(Thread.CurrentThread.CurrentCulture, "InvalidControlTemplate", "GeolocationFieldDisplay"));
                    else
                        this.Value = this.ItemFieldValue;
                }
                else
                {
                    this.m_longitudeBox = (TextBox)TemplateContainer.FindControl("GeolocationFieldLongitude");
                    this.m_latitudeBox = (TextBox)TemplateContainer.FindControl("GeolocationFieldLatitude");
                    if (this.m_longitudeBox == null || this.m_latitudeBox == null)
                    {
                        throw new ArgumentException(SPResource.GetString(Thread.CurrentThread.CurrentCulture, "InvalidControlTemplate", "GeolocationField"));
                    }
                    else
                    {
                        this.m_latitudeBox.TabIndex = this.TabIndex;
                        this.m_longitudeBox.TabIndex = this.TabIndex;
                        this.m_longitudeBox.CssClass = this.CssClass;
                        this.m_latitudeBox.CssClass = this.CssClass;
                        if (string.IsNullOrEmpty(this.m_longitudeBox.ToolTip))
                            this.m_longitudeBox.ToolTip = SPResource.GetString(Thread.CurrentThread.CurrentCulture, "URLFieldDescriptionToolTip", new object[0]);
                        if (string.IsNullOrEmpty(this.m_latitudeBox.ToolTip))
                            this.m_latitudeBox.ToolTip = SPResource.GetString(Thread.CurrentThread.CurrentCulture, "URLFieldDescriptionToolTip", new object[0]);
                        if (this.DisplaySize > 0)
                        {
                            string str = Convert.ToString(this.DisplaySize, (IFormatProvider)CultureInfo.InvariantCulture);
                            this.m_longitudeBox.Attributes["Size"] = str;
                            this.m_latitudeBox.Attributes["Size"] = str;
                        }
                        if (!string.IsNullOrEmpty(this.Field.IMEMode))
                        {
                            this.m_longitudeBox.Style.Add("ime-mode", this.Field.IMEMode);
                            this.m_latitudeBox.Style.Add("ime-mode", this.Field.IMEMode);
                        }
                        if (!this.Field.Required)
                            return;
                        this.TemplateContainer.Controls.Add((Control)this.CreateRequiredFieldValidator((Control)this.m_latitudeBox));
                        this.TemplateContainer.Controls.Add((Control)this.CreateRequiredFieldValidator((Control)this.m_longitudeBox));
                    }
                }
            }
        }

        public override void Validate()
        {
            if (this.ControlMode == SPControlMode.Display || !this.IsValid || (SPFieldGeolocation)this.Field == null)
                return;
            this.ValidateInputValues();
        }

        private RequiredFieldValidator CreateRequiredFieldValidator(Control control)
        {
            RequiredFieldValidator requiredFieldValidator = new RequiredFieldValidator();
            requiredFieldValidator.ControlToValidate = control.ID;
            requiredFieldValidator.Display = ValidatorDisplay.Dynamic;
            requiredFieldValidator.ForeColor = Color.Empty;
            requiredFieldValidator.CssClass = "ms-formvalidation";
            string @string = SPResource.GetString(Thread.CurrentThread.CurrentCulture, "MissingRequiredField", new object[0]);
            requiredFieldValidator.ErrorMessage = "<span role=\"alert\" >" + SPHttpUtility.HtmlEncode(@string) + "</span><br/>";
            requiredFieldValidator.SetFocusOnError = true;
            requiredFieldValidator.EnableClientScript = false;
            return requiredFieldValidator;
        }

        private void ValidateInputValues()
        {
            if ((!this.Latitude.HasValue || this.Longitude.HasValue) && (this.Latitude.HasValue || !this.Longitude.HasValue) || (!this.IsValid || this.Field.Required))
                return;
            this.IsValid = false;
            this.ErrorMessage = SPResource.GetString(Thread.CurrentThread.CurrentCulture, "GeolocationAllValues");
        }

        public override object Value
        {
            get
            {
                if (!this.Latitude.HasValue || !this.Longitude.HasValue)
                    return (object)null;
                return (object)new SPFieldGeolocationValue()
                {
                    Latitude = this.Latitude.Value,
                    Longitude = this.Longitude.Value
                };
            }
            set
            {
                if (value == null)
                    return;
                SPFieldGeolocationValue geolocationValue = value as SPFieldGeolocationValue;
                if (geolocationValue == null)
                {
                    geolocationValue = new SPFieldGeolocationValue(value as string);
                    if (geolocationValue == null)
                        throw new ArgumentException(SPResource.GetString(Thread.CurrentThread.CurrentCulture, "InvalidGeolocationWkt", new object[0]));
                }
                Longitude = new double?(geolocationValue.Longitude);
                Latitude = new double?(geolocationValue.Latitude);
            }
        }

        public override void Focus()
        {
            if (this.InDesign)
                return;
            this.EnsureChildControls();
            if (this.ControlMode == SPControlMode.Display || this.m_longitudeBox == null)
                return;
            this.m_longitudeBox.Focus();
        }


        private bool ParseDouble(string numberText, out double number)
        {
            number = 0.0;
            try
            {
                double num = double.Parse(numberText, (IFormatProvider)Thread.CurrentThread.CurrentCulture);
                number = num;
                return true;
            }
            catch (FormatException ex)
            {
                this.IsValid = false;
                this.ErrorMessage = SPResource.GetString(Thread.CurrentThread.CurrentCulture, "NumberIncorrectFormat");
                return false;
            }
            catch (OverflowException ex)
            {
                this.IsValid = false;
                this.ErrorMessage = SPResource.GetString(Thread.CurrentThread.CurrentCulture, "NumberFieldValueOverflow");
                return false;
            }
        }

        public override string CssClass
        {
            get
            {
                if (this.m_cssClass == null)
                    this.m_cssClass = this.DisplaySize > 0 || this.IsInInlineEditMode ? "ms-input" : "ms-long";
                return this.m_cssClass;
            }
            set
            {
                this.m_cssClass = value;
            }
        }

        protected override string DefaultTemplateName
        {
            get
            {
                return "GeolocationField";
            }
        }

        public override string DisplayTemplateName
        {
            get
            {
                return "GeolocationFieldDisplay";
            }
        }

        internal virtual double? Longitude
        {
            get
            {
                this.EnsureChildControls();
                if (this.ControlMode == SPControlMode.Display)
                {
                    object itemFieldValue = this.ItemFieldValue;
                    if (itemFieldValue == null)
                        return new double?();
                    if (itemFieldValue is SPFieldGeolocationValue)
                        return new double?(((SPFieldGeolocationValue)this.ItemFieldValue).Longitude);
                }
                else
                {
                    double number;
                    if (this.m_longitudeBox != null && !string.IsNullOrEmpty(this.m_longitudeBox.Text.Trim()) && this.ParseDouble(this.m_longitudeBox.Text.Trim(), out number))
                        return new double?(number);
                }
                return new double?();
            }
            set
            {
                this.EnsureChildControls();
                if (!value.HasValue)
                    return;
                if (this.ControlMode != SPControlMode.Display)
                {
                    if (this.m_longitudeBox == null)
                        return;
                    this.m_longitudeBox.Text = value.Value.ToString(Thread.CurrentThread.CurrentCulture);
                }
                else
                {
                    if (this.m_longitudeLabel == null)
                        return;
                    this.m_longitudeLabel.Text = value.Value.ToString(Thread.CurrentThread.CurrentCulture);
                }
            }
        }

        internal virtual double? Latitude
        {
            get
            {
                this.EnsureChildControls();
                if (this.ControlMode == SPControlMode.Display)
                {
                    object itemFieldValue = this.ItemFieldValue;
                    if (itemFieldValue == null)
                        return new double?();
                    if (itemFieldValue is SPFieldGeolocationValue)
                        return new double?(((SPFieldGeolocationValue)this.ItemFieldValue).Latitude);
                }
                else
                {
                    double number;
                    if (this.m_latitudeBox != null && !string.IsNullOrEmpty(this.m_latitudeBox.Text.Trim()) && this.ParseDouble(this.m_latitudeBox.Text.Trim(), out number))
                        return new double?(number);
                }
                return new double?();
            }
            set
            {
                this.EnsureChildControls();
                if (!value.HasValue)
                    return;
                if (this.ControlMode != SPControlMode.Display)
                {
                    if (this.m_latitudeBox == null)
                        return;
                    this.m_latitudeBox.Text = value.Value.ToString(Thread.CurrentThread.CurrentCulture);
                }
                else
                {
                    if (this.m_latitudeLabel == null)
                        return;
                    this.m_latitudeLabel.Text = value.Value.ToString(Thread.CurrentThread.CurrentCulture);
                }
            }
        }





        protected TextBox m_latitudeBox;
        protected TextBox m_longitudeBox;
        protected Label m_longitudeLabel;
        protected Label m_latitudeLabel;
        private string m_cssClass;
    }
}
