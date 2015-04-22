using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallOfBeer.API.Models
{
    class EventUpdate : EventPost
    {
        [JsonProperty("eventId")]
        public int Id { get; set; }

        public EventUpdate(EventPost eventPost)
        {
            this.Address = eventPost.Address;
            this.City = eventPost.City;
            this.Country = eventPost.Country;
            this.Latitude = eventPost.Latitude;
            this.Longitude = eventPost.Longitude;
            this.AddressName = eventPost.AddressName;
            this.Zip = eventPost.Zip;
            this.Date = eventPost.Date;
            this.Name = eventPost.Name;
        }
    }
}
