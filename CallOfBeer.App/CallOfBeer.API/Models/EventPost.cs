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
        public string EventName { get; set; }

        [JsonProperty("eventDate")]
        public int EventDate { get; set; }

        [JsonProperty("addressName")]
        public string AddressName { get; set; }

        [JsonProperty("addressAddress")]
        public string AddressAddress { get; set; }

        [JsonProperty("addressZip")]
        public int AddressZip { get; set; }

        [JsonProperty("addressCity")]
        public string AddressCity { get; set; }

        [JsonProperty("addressCountry")]
        public string AddressCountry { get; set; }

        [JsonProperty("addressLat")]
        public double AddressLat { get; set; }

        [JsonProperty("addressLon")]
        public double AddressLon { get; set; }
    }
}
