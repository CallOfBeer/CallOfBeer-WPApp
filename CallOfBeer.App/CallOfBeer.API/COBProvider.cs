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
    internal class COBProvider
    {
        private const string BaseUrl = @"http://api.callofbeer.com/app_dev.php/";
        private const string ClientId = "1_uxjcq41muas4g0kwk80g0ww8cscskkgkosgkkk080w4484o0s";
        private const string ClientSecret = "41y6v3z7l0ow4s4wgoog0kkssscsookg0c8kowo0k0gk8wokk0";

        private HttpProvider _httpProvider;
        private Dictionary<string, string> _authHeaders;
        public COBProvider()
        {
            this._httpProvider = new HttpProvider();
        }

        public void UseAccessToken(string accessToken)
        {
            this._authHeaders = this.BuildAuthHeaders(accessToken);
        }

        public void RemoveAccessToken()
        {
            this._authHeaders = null;
        }

        public async Task<string> PostEventAsync(EventPost eventPost)
        {
            string newUrl = string.Concat(BaseUrl, "events");
            string body = JsonConvert.SerializeObject(eventPost);

            HttpResponseMessage responseMessage = await this._httpProvider.PostAsync(newUrl, this._authHeaders, body);

            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception("Un problème est survenu lors de l'envoie de la requête PostEventAsync.");

            string response = await responseMessage.Content.ReadAsStringAsync();

            return response;
        }

        public async Task<string> UpdateEventAsync(EventPost eventPost, int eventId)
        {
            string newUrl = string.Concat(BaseUrl, "events");
            EventUpdate newEvent = new EventUpdate(eventPost) { Id = eventId };

            string body = JsonConvert.SerializeObject(newEvent);

            HttpResponseMessage responseMessage = await this._httpProvider.PostAsync(newUrl, this._authHeaders, body);

            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception("Un problème est survenu lors de l'envoie de la requête UpdateEventAsync.");

            string response = await responseMessage.Content.ReadAsStringAsync();

            return response;
        }

        public async Task<string> GetEventsAsync(double topLat, double botLat, double topLon, double botLon)
        {
            string newUrl = string.Concat(BaseUrl, "events");
            Dictionary<string, string> parameters = new Dictionary<string, string>() {
            { "topLat", topLat.ToString().Replace(',','.') },
            { "topLon", topLon.ToString().Replace(',','.') },
            { "botLat", botLat.ToString().Replace(',','.') },
            { "botLon", botLon.ToString().Replace(',','.') }
            };

            HttpResponseMessage responseMessage = await this._httpProvider.GetAsync(newUrl, this._authHeaders, parameters);

            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception("Un problème est survenu lors de l'envoie de la requête GetEventsAsync.");

            string response = await responseMessage.Content.ReadAsStringAsync();

            return response;
        }

        public async Task<string> GetAddressAsync(double latitude, double longitude)
        {
            string newUrl = string.Concat(BaseUrl, "address");
            Dictionary<string, string> parameters = new Dictionary<string, string>() {
            { "lat", latitude.ToString().Replace(',','.') },
            { "lon", longitude.ToString().Replace(',','.') }
            };
            HttpResponseMessage responseMessage = await this._httpProvider.GetAsync(newUrl, this._authHeaders, parameters);

            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception("Un problème est survenu lors de l'envoie de la requête GetAddressAsync.");

            string response = await responseMessage.Content.ReadAsStringAsync();

            return response;
        }

        public async Task<string> GetEventByIdAsync(int eventId)
        {
            string newUrl = string.Concat(BaseUrl, "event");
            Dictionary<string, string> parameters = new Dictionary<string, string>() {
            { "id", eventId.ToString() }
            };
            HttpResponseMessage responseMessage = await this._httpProvider.GetAsync(newUrl, this._authHeaders, parameters);

            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception("Un problème est survenu lors de l'envoie de la requête GetEventByIdAsync.");

            string response = await responseMessage.Content.ReadAsStringAsync();

            return response;
        }

        public async Task<string> GetGeolocAsync(string address)
        {
            string newUrl = string.Concat(BaseUrl, "geoloc");
            Dictionary<string, string> parameters = new Dictionary<string, string>() {
            { "address", address }
            };
            HttpResponseMessage responseMessage = await this._httpProvider.GetAsync(newUrl, this._authHeaders, parameters);

            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception("Un problème est survenu lors de l'envoie de la requête GetGeolocAsync.");

            string response = await responseMessage.Content.ReadAsStringAsync();

            return response;
        }

        public async Task<string> RegisterAsync(string userName, string pass, string mail)
        {
            string newUrl = string.Concat(BaseUrl, "users");

            RegisterModel register = new RegisterModel()
            {
                ClientId = ClientId,
                ClientSecret = ClientSecret,
                Mail = mail,
                Password = pass,
                UserName = userName
            };

            string body = JsonConvert.SerializeObject(register);

            HttpResponseMessage responseMessage = await this._httpProvider.PostAsync(newUrl, null, body);

            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception("Un problème est survenu lors de l'envoie de la requête RegisterAsync.");

            string response = await responseMessage.Content.ReadAsStringAsync();

            return response;
            // objet de type User
        }

        public async Task<string> GetTokenAsync(string userName, string pass)
        {
            string newUrl = string.Concat(BaseUrl, "oauth/v2/token");

            RegisterModel register = new RegisterModel()
            {
                ClientId = ClientId,
                ClientSecret = ClientSecret,
                Password = pass,
                UserName = userName,
                GrantType = "password"
            };

            string body = JsonConvert.SerializeObject(register);

            HttpResponseMessage responseMessage = await this._httpProvider.PostAsync(newUrl, null, body);

            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception("Un problème est survenu lors de l'envoie de la requête GetTokenAsync.");

            string response = await responseMessage.Content.ReadAsStringAsync();

            return response;
            // objet de type Token
        }

        public async Task<string> RefreshTokenAsync(string refreshToken)
        {
            string newUrl = string.Concat(BaseUrl, "oauth/v2/token");

            RegisterModel register = new RegisterModel()
            {
                ClientId = ClientId,
                ClientSecret = ClientSecret,
                GrantType = "refresh_token",
                RefreshToken = refreshToken
            };

            string body = JsonConvert.SerializeObject(register);

            HttpResponseMessage responseMessage = await this._httpProvider.PostAsync(newUrl, null, body);

            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception("Un problème est survenu lors de l'envoie de la requête RefreshTokenAsync.");

            string response = await responseMessage.Content.ReadAsStringAsync();

            return response;
            // objet de type Token
        }

        public async Task<string> GetUserAsync()
        {
            string newUrl = string.Concat(BaseUrl, "me");
            HttpResponseMessage responseMessage = await this._httpProvider.GetAsync(newUrl, this._authHeaders);

            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception("Un problème est survenu lors de l'envoie de la requête GetUserAsync.");

            string response = await responseMessage.Content.ReadAsStringAsync();

            return response;
            // objet User
        }

        private Dictionary<string, string> BuildAuthHeaders(string accessToken)
        {
            Dictionary<string, string> authHeaders = new Dictionary<string, string>();
            authHeaders.Add("Authorization", string.Concat("Bearer ", accessToken));
            return authHeaders;
        }
    }
}
