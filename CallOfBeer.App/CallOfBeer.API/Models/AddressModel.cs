using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallOfBeer.API.Models
{
    public class AddressModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        // Latitude/Longitude
        [JsonProperty("geolocation")]
        public GeolocationModel Geolocation { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("zip")]
        public int Zip { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }
    }

    public class GeolocationModel
    {
        [JsonProperty("lat")]
        public double Latitude { get; set; }

        [JsonProperty("lon")]
        public double Longitude { get; set; }
    }
}
