using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallOfBeer.API.Models
{
    public class RawAddressModel
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

        public AddressModel ConvertToAddressModel()
        {
            AddressModel result = new AddressModel()
            {
                Geolocation = new GeolocationModel() {
                    Latitude = this.Latitude,
                    Longitude = this.Longitude 
                },
                Address = String.Format("{0} {1}", this.StreetNumber, this.StreetName),
                City = this.City,
                Country = this.Country,
                Zip = Convert.ToInt32(ZipCode.Substring(0, ZipCode.IndexOf(';')))
            };
            return result;
        }
    }
}
