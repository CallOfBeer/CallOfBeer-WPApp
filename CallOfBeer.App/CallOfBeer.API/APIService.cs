using CallOfBeer.API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallOfBeer.API
{
    public class APIService
    {
        private COBProvider _cob;
        public APIService()
        {
            this._cob = new COBProvider();
        }

        public async Task<IEnumerable<EventGet>> GetEventsAsync(double topLat, double topLon, double botLat, double botLon)
        {
            IEnumerable<EventGet> result = null;
            result = await this.SafeRequest(async () =>
            {
                string response = await this._cob.GetEventsAsync(topLat, botLat, topLon, botLon);
                result = JsonConvert.DeserializeObject<List<EventGet>>(response);
                return result;
            });
            return result;
        }

        public async Task<bool> PostEventAsync(EventPost eventPost)
        {
            bool result = false;
            result = await this.SafeRequest(async () =>
            {
                string response = await this._cob.PostEventAsync(eventPost);
                result = JsonConvert.DeserializeObject<bool>(response);
                return result;
            });
            return result;
        }

        public async Task<bool> UpdateEventAsync(EventPost eventPost, int eventId)
        {
            bool result = false;
            result = await this.SafeRequest(async () =>
            {
                string response = await this._cob.UpdateEventAsync(eventPost, eventId);
                result = JsonConvert.DeserializeObject<bool>(response);
                return result;
            });
            return result;
        }

        public async Task<AddressModel> GetAddressAsync(double latitude, double longitude)
        {
            AddressModel result = null;
            result = await this.SafeRequest(async () =>
            {
                string response = await this._cob.GetAddressAsync(latitude, longitude);
                result = JsonConvert.DeserializeObject<AddressModel>(response);
                return result;
            });
            return result;
        }

        public async Task<EventGet> GetEventByIdAsync(int eventId)
        {
            EventGet result = null;
            result = await this.SafeRequest(async () =>
            {
                string response = await this._cob.GetEventByIdAsync(eventId);
                result = JsonConvert.DeserializeObject<EventGet>(response);
                return result;
            });
            return result;
        }

        public async Task<AddressModel> GetGeolocAsync(string address)
        {
            AddressModel result = null;
            result = await this.SafeRequest(async () =>
            {
                string response = await this._cob.GetGeolocAsync(address);
                RawAddressModel rawAddress = JsonConvert.DeserializeObject<RawAddressModel>(response);
                result = rawAddress.ConvertToAddressModel();
                return result;
            });
            return result;
        }

        private async Task<T> SafeRequest<T>(Func<Task<T>> function)
        {
            try
            {
                return await function.Invoke();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
