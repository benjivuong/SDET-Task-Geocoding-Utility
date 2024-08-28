using Newtonsoft.Json;
using System.Text.Json;

namespace SDET_Task_Geocoding_Utility
{
    internal class LocationInformation
    {
        private float _latitude;
        private float _longitude;
        private string _country;
        private string? _name;
        private string? _state;
        private string? _zip;
        [JsonProperty(PropertyName = "lat")]
        public float Latitude { get { return _latitude; } set { _latitude = value; } }
        [JsonProperty(PropertyName = "lon")]
        public float Longitude { get { return _longitude; } set { _longitude = value; } }
        [JsonProperty(PropertyName = "country")]
        public string Country { get { return _country; } set { _country = value; } }
        [JsonProperty(PropertyName = "name")]
        public string? Name { get { return _name; } set { _name = value; } }
        [JsonProperty(PropertyName = "state")]
        public string? State { get { return _state; } set { _state = value; } }
        [JsonProperty(PropertyName = "zip")]
        public string? Zip { get { return _zip; } set { _zip = value; } }

        public LocationInformation(float lat, float lon, string country, string name = "", string state = "", string zip = "") 
        {
            _latitude = lat;
            _longitude = lon;
            _country = country;
            _name = name;
            _state = state;
            _zip = zip;
        }
        public override string ToString()
        {
            string resultString = "Latitude: " + _latitude + "\t";
            resultString += "Longitude: " + _longitude + "\t";
            resultString += "Country: " + _country + "\t";
            if (!String.IsNullOrWhiteSpace(_name)) resultString += "Name: " + _name + "\t";
            if (!String.IsNullOrWhiteSpace(_state)) resultString += "State: " + _state + "\t";
            if (!String.IsNullOrWhiteSpace(_zip)) resultString += "Zip: " + _zip;
            return resultString;
        }
    }
}
