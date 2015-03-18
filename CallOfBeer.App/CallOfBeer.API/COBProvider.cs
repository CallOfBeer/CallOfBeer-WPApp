using CallOfBeer.API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CallOfBeer.API
{
    public class COBProvider
    {
        private const string baseUrl = @"http://api.callofbeer.com/app_dev.php/";
        private HttpProvider _httpProvider;
        public COBProvider()
        {
            this._httpProvider = new HttpProvider();
        }

        public async Task<string> PostEventAsync(EventPost eventPost)
        {
            string newUrl = string.Concat(baseUrl, "events");
            string body = JsonConvert.SerializeObject(eventPost);

            Task<HttpResponseMessage> responseMessage = this._httpProvider.PostAsync(newUrl, body);
            responseMessage.Wait();
            string response = await responseMessage.Result.Content.ReadAsStringAsync();

            return response;
        }

        public async Task<string> UpdateEventAsync(EventPost eventPost, int eventId)
        {
            string newUrl = string.Concat(baseUrl, "events");
            EventUpdate newEvent = new EventUpdate(eventPost) { EventId = eventId };

            string body = JsonConvert.SerializeObject(newEvent);

            Task<HttpResponseMessage> responseMessage = this._httpProvider.PostAsync(newUrl, body);
            responseMessage.Wait();
            string response = await responseMessage.Result.Content.ReadAsStringAsync();

            return response;
        }

        public async Task<string> GetEventsAsync(double topLat, double botLat, double topLon, double botLon)
        {
            string newUrl = string.Concat(baseUrl, "events");
            Dictionary<string, string> parameters = new Dictionary<string, string>() {
            { "topLat", topLat.ToString().Replace(',','.') },
            { "topLon", topLon.ToString().Replace(',','.') },
            { "botLat", botLat.ToString().Replace(',','.') },
            { "botLon", botLon.ToString().Replace(',','.') }
            };
            HttpResponseMessage result = await this._httpProvider.GetAsync(newUrl, parameters);
            string response = await result.Content.ReadAsStringAsync();

            return response;
        }

        public async Task<string> GetAddressAsync(double latitude, double longitude)
        {
            string newUrl = string.Concat(baseUrl, "address");
            Dictionary<string, string> parameters = new Dictionary<string, string>() {
            { "lat", latitude.ToString().Replace(',','.') },
            { "lon", longitude.ToString().Replace(',','.') }
            };
            HttpResponseMessage result = await this._httpProvider.GetAsync(newUrl, parameters);
            string response = await result.Content.ReadAsStringAsync();

            return response;
        }

        public async Task<string> GetEventByIdAsync(int eventId)
        {
            string newUrl = string.Concat(baseUrl, "event");
            Dictionary<string, string> parameters = new Dictionary<string, string>() {
            { "id", eventId.ToString() }
            };
            HttpResponseMessage result = await this._httpProvider.GetAsync(newUrl, parameters);
            string response = await result.Content.ReadAsStringAsync();

            return response;
        }

        public async Task<string> GetGeolocAsync(string address)
        {
            string newUrl = string.Concat(baseUrl, "geoloc");
            Dictionary<string, string> parameters = new Dictionary<string, string>() {
            { "address", address }
            };
            HttpResponseMessage result = await this._httpProvider.GetAsync(newUrl, parameters);
            string response = await result.Content.ReadAsStringAsync();

            return response;
        }
    }
}
