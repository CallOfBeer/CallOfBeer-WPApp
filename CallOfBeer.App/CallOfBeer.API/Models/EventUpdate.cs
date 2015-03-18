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
        public int EventId { get; set; }

        public EventUpdate(EventPost eventPost)
        {
            this.AddressAddress = eventPost.AddressAddress;
            this.AddressCity = eventPost.AddressCity;
            this.AddressCountry = eventPost.AddressCountry;
            this.AddressLat = eventPost.AddressLat;
            this.AddressLon = eventPost.AddressLon;
            this.AddressName = eventPost.AddressName;
            this.AddressZip = eventPost.AddressZip;
            this.EventDate = eventPost.EventDate;
            this.EventName = eventPost.EventName;
        }
    }
}
