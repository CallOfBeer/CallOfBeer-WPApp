using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallOfBeer.API.Models
{
    public class EventPost
    {
        [JsonProperty("eventName")]
        public string Name { get; set; }

        [JsonProperty("eventDate")]
        public int Date { get; set; }

        [JsonProperty("addressName")]
        public string AddressName { get; set; }

        [JsonProperty("addressAddress")]
        public string Address { get; set; }

        [JsonProperty("addressZip")]
        public int Zip { get; set; }

        [JsonProperty("addressCity")]
        public string City { get; set; }

        [JsonProperty("addressCountry")]
        public string Country { get; set; }

        [JsonProperty("addressLat")]
        public double Latitude { get; set; }

        [JsonProperty("addressLon")]
        public double Longitude { get; set; }
    }
}
