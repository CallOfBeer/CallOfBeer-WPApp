using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallOfBeer.API.Models
{
    public class AddressEvent
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("lat")]
        public double Latitude { get; set; }

        [JsonProperty("lon")]
        public double Longitude { get; set; }

        // Longitude / Latitude
        [JsonProperty("geolocation")]
        public Double[] Geolocation { get; set; }
    }

    public class AddressByGeoloc
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("zip")]
        public int Zip { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        // Latitude / Longitude
        [JsonProperty("geolocation")]
        public Double[] Geolocation { get; set; }
    }

    public class GeolocByAddress
    {
        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("street_number")]
        public string StreetNumber { get; set; }

        [JsonProperty("street_name")]
        public string StreetName { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("zipcode")]
        public string ZipCode { get; set; }
    }
}
