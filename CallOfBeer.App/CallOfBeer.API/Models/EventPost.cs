using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallOfBeer.API.Models
{
    public class EventPost
    {
        public string eventName { get; set; }
        public int eventDate { get; set; }
        public string addressName { get; set; }
        public string addressAddress { get; set; }
        public int addressZip { get; set; }
        public string addressCity { get; set; }
        public string addressCountry { get; set; }
        public double addressLat { get; set; }
        public double addressLon { get; set; }
    }
}
