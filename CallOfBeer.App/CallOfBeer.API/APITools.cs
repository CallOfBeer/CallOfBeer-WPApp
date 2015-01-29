using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CallOfBeer.API
{
    public class APITools
    {

       

        /// <summary>
        /// GetEvent : récupére la liste des évènement à afficher sur la carte 
        /// </summary>
        /// <param name="topLat">Latitude top du coin de la map</param>
        /// <param name="topLon">Longitude top du coin sup de la map</param>
        /// <param name="botLat">Latitude bottom du coin de la map</param>
        /// <param name="botLon">Longitude bottom du coin sup de la map</param>
        /// <returns>List d'Events àafficher sur la map</Events></returns>
        public async Task<List<Events>> GetEvents(double topLat, double topLon, double botLat, double botLon )
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://api.callofbeer.com/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    string url = string.Format("http://api.callofbeer.com/events?topLat={0}&topLon={1}&botLat={2}&botLon={3}", topLat, topLon, botLat, botLon).Replace(",",".");
                    HttpResponseMessage reponse = await client.GetAsync(new Uri(url));
                    reponse.EnsureSuccessStatusCode();
                    if (reponse.IsSuccessStatusCode)
                    {
                        List<Events> getEvent = await reponse.Content.ReadAsAsync<List<Events>>();                      
                        return getEvent;
                    }
                }
                catch (HttpRequestException e)
                {
                    string error = "Une erreur à été levé : " + e.ToString();
                }
                return null;
            }
        }

        /// <summary>
        /// PostEvent : Envois les données d'un nouveau évènement
        /// </summary>
        /// <param name="newEvent">AddEvents : class avec toute les données chargées</param>
        public async Task<HttpResponseMessage> PostEvent(AddEvents newEvent)
        {
            using (HttpClient client = new HttpClient(new HttpClientHandler()))
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync("http://api.callofbeer.com/events", newEvent);
                    return response;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
