using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallOfBeer.API
{
    public class Events
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("date")]
        public string date { get; set; }

        [JsonProperty("address")]
        public Adress adress { get; set; }
    }   

    //Declaration de la structure d'une adress
    public struct Adress
    {
        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("zip")]
        public string zip { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("geolocation")]
        public float[] Geolocalisation { get; set; }
    }
    
}
    

