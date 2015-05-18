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

        /// <summary>
        /// Permet de renseigner un access token pour réaliser les requêtes
        /// </summary>
        /// <param name="accessToken"></param>
        public void Connect(string accessToken)
        {
            try
            {
                this._cob.UseAccessToken(accessToken);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Retire l'access token pour les requêtes
        /// </summary>
        public void Disconnect()
        {
            try
            {
                this._cob.RemoveAccessToken();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Permet de récupérer les évenements d'une zone
        /// </summary>
        /// <param name="topLat"></param>
        /// <param name="topLon"></param>
        /// <param name="botLat"></param>
        /// <param name="botLon"></param>
        /// <returns>Liste d'évenements</returns>
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

        /// <summary>
        /// Permet de poster un nouvel évenement
        /// </summary>
        /// <param name="eventPost"></param>
        /// <returns>Evenement créé</returns>
        public async Task<EventGet> PostEventAsync(EventPost eventPost)
        {
            EventGet result = null;
            result = await this.SafeRequest(async () =>
            {
                string response = await this._cob.PostEventAsync(eventPost);
                result = JsonConvert.DeserializeObject<EventGet>(response);
                return result;
            });
            return result;
        }

        /// <summary>
        /// Permet de mettre à jour un évenement
        /// </summary>
        /// <param name="eventPost"></param>
        /// <param name="eventId"></param>
        /// <returns>Evenement mis a jour</returns>
        public async Task<EventGet> UpdateEventAsync(EventPost eventPost, int eventId)
        {
            EventGet result = null;
            result = await this.SafeRequest(async () =>
            {
                string response = await this._cob.UpdateEventAsync(eventPost, eventId);
                result = JsonConvert.DeserializeObject<EventGet>(response);
                return result;
            });
            return result;
        }

        /// <summary>
        /// Permet de récupérer une adresse en fonction de ses coordonnées
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns>Adresse</returns>
        public async Task<AddressByGeoloc> GetAddressAsync(double latitude, double longitude)
        {
            AddressByGeoloc result = null;
            result = await this.SafeRequest(async () =>
            {
                string response = await this._cob.GetAddressAsync(latitude, longitude);
                result = JsonConvert.DeserializeObject<AddressByGeoloc>(response);
                return result;
            });
            return result;
        }

        /// <summary>
        /// Permet de récupérer un évenement en fonction de son id
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns>Evenement</returns>
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

        /// <summary>
        /// Permet de récupérer les coordonnées d'une adresse
        /// </summary>
        /// <param name="address"></param>
        /// <returns>Adresse avec les coordonnées</returns>
        public async Task<GeolocByAddress> GetGeolocAsync(string address)
        {
            GeolocByAddress result = null;
            result = await this.SafeRequest(async () =>
            {
                string response = await this._cob.GetGeolocAsync(address);
                result = JsonConvert.DeserializeObject<GeolocByAddress>(response);
                return result;
            });
            return result;
        }

        /// <summary>
        /// Enregistrement d'un nouvel utilisateur
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pass"></param>
        /// <param name="mail"></param>
        /// <returns>Utilisateur</returns>
        public async Task<User> RegisterAsync(string userName, string pass, string mail)
        {
            User result = null;
            result = await this.SafeRequest(async () =>
            {
                string response = await this._cob.RegisterAsync(userName, pass, mail);
                result = JsonConvert.DeserializeObject<User>(response);
                return result;
            });
            return result;
        }

        /// <summary>
        /// Obtenir un token pour l'utilisateur
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pass"></param>
        /// <returns>Token</returns>
        public async Task<Token> GetTokenAsync(string userName, string pass)
        {
            Token result = null;
            result = await this.SafeRequest(async () =>
            {
                string response = await this._cob.GetTokenAsync(userName, pass);
                result = JsonConvert.DeserializeObject<Token>(response);
                return result;
            });
            return result;
        }

        /// <summary>
        /// Rafraichi un token (utiliser le ResfreshToken pour obtenir un nouveau AccessToken)
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns>Token</returns>
        public async Task<Token> RefreshTokenAsync(string refreshToken)
        {
            Token result = null;
            result = await this.SafeRequest(async () =>
            {
                string response = await this._cob.RefreshTokenAsync(refreshToken);
                result = JsonConvert.DeserializeObject<Token>(response);
                return result;
            });
            return result;
        }

        /// <summary>
        /// Permet d'obtenir l'utilisateur actuellement connecté
        /// </summary>
        /// <returns></returns>
        public async Task<User> GetUserAsync()
        {
            User result = null;
            result = await this.SafeRequest(async () =>
            {
                string response = await this._cob.GetUserAsync();
                result = JsonConvert.DeserializeObject<User>(response);
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
