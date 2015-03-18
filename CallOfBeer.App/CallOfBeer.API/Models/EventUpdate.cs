using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallOfBeer.API.Models
{
    class EventUpdate : EventPost
    {
        public int eventId { get; set; }
        public EventUpdate(EventPost eventPost)
        {
            this.addressAddress = eventPost.addressAddress;
            this.addressCity = eventPost.addressCity;
            this.addressCountry = eventPost.addressCountry;
            this.addressLat = eventPost.addressLat;
            this.addressLon = eventPost.addressLon;
            this.addressName = eventPost.addressName;
            this.addressZip = eventPost.addressZip;
            this.eventDate = eventPost.eventDate;
            this.eventName = eventPost.eventName;
        }
    }
}
