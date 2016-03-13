using System;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SP.Maps
{

    /// <summary>
    /// Geolocation value
    /// </summary>
    public class SPFieldGeolocationValue
    {
        #region ctors

        public SPFieldGeolocationValue()
        {
        }

        public SPFieldGeolocationValue(string fieldValue)
        {
            SetPointFromWellKnownText(fieldValue);
        }

        public SPFieldGeolocationValue(double latitude, double longitude)
        {
            _latitude = latitude;
            _longitude = longitude;
        }

        #endregion


        private void SetPointFromWellKnownText(string value)
        {
            var regexWKT = new Regex(PointWKTValuePattern);
            var res = regexWKT.Matches(value);
            if (res.Count > 0)
            {
                _latitude = Convert.ToDouble(res[0].Groups["Lat"].Value, CultureInfo.InvariantCulture);
                _longitude = Convert.ToDouble(res[0].Groups["Long"].Value, CultureInfo.InvariantCulture);
            }
        }


        private string GetPointAsWellKnownText()
        {
            if (_altitude.HasValue || _measure.HasValue)
                return string.Format(CultureInfo.InvariantCulture, "Point ({0} {1} {2} {3})", Longitude, Latitude, Altitude, Measure);
            else
                return string.Format(CultureInfo.InvariantCulture, "Point ({0} {1})", Longitude,Latitude);
        }


        public override string ToString()
        {
            return GetPointAsWellKnownText();
        }

        #region Properties

        [DefaultValue(-99999.0)]
        public double Latitude
        {
            get { return !_latitude.HasValue ? -99999.0 : _latitude.Value; }
            set
            {
                _latitude = value;
            }
        }

 
        [DefaultValue(-99999.0)]
        public double Longitude
        {
            get { return !_longitude.HasValue ? -99999.0 : _longitude.Value; }
            set
            {
                _longitude = value;
            }
        }

        public double Altitude
        {
            get {
                return !_altitude.HasValue ? 0.0 : _altitude.Value;
            }
            set
            {
                _altitude = value;
            }
        }

        public double Measure
        {
            get {
                return !_measure.HasValue ? 0.0 : _measure.Value;
            }
            set
            {
                _measure = value;
            }
        }


        #endregion

        private const string PointZMWKTValueFormat = "Point ({0} {1} {2} {3})";
        private const string PointWKTValueFormat = "Point ({0} {1})";
        //private const string PointWKTValuePattern = @"Point\s\((?<Lat>[0-9]+(\.[0-9]+))\s(?<Long>[0-9]+(\.[0-9]+))\)";
        private const string PointWKTValuePattern = @"Point\s\((?<Lat>[\-\+]?[0-9]+(\.[0-9]+)?)\s(?<Long>[\-\+]?[0-9]+(\.[0-9]+)?)\)";

        private double? _latitude = new double?();
        private double? _longitude = new double?();
        private double? _measure = new double?();
        private double? _altitude = new double?();
    }
}
