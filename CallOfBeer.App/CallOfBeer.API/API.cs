using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallOfBeer.API
{
    public class API
    {
        private COBProvider _cob;
        public API()
        {
            this._cob = new COBProvider();
        }

        public async Task<IEnumerable<Events>> GetEvents(double topLat, double topLon, double botLat, double botLon)
        {
            IEnumerable<Events> result = null;
            //Task<string> response = this._cob.GetEventsAsync(topLat, botLat, topLon, botLon);
            //response.Wait();
            string response = await this._cob.GetEventsAsync(topLat, botLat, topLon, botLon);
            result = JsonConvert.DeserializeObject<List<Events>>(response);
            return result;
        }
    }
}
