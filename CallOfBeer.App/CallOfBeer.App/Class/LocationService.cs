using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using System.Device.Location;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.Foundation;
using Windows.Storage.Streams;
using CallOfBeer.API;

namespace CallOfBeer.App.Class
{
    public static class LocationService
    {
        public static BasicGeoposition topLeft;
        public static BasicGeoposition bottomRight;
        public static Geoposition localPosition;

        /// <summary>
        /// Initialise la map
        /// </summary>
        /// <param name="appMap">nom de la map dans la vue</param>
        public static async void LoadMap(MapControl appMap)
        {
            Geoposition returnedPosition = await GetUserPosition();
            MapIcon userMapIcon = new MapIcon();

            //TODO Initialiser les paramétre de la map
            appMap.ZoomLevel = 13;

            //TODO Definir la position de l'utilisateur
            appMap.Center = returnedPosition.Coordinate.Point;

            //TODO Afficher les élements de la carte
            userMapIcon.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/customicon.png"));
            userMapIcon.Location = appMap.Center;
            userMapIcon.Title = "Votre position";
            userMapIcon.NormalizedAnchorPoint = new Point(0.5, 0.5);
        }

        /// <summary>
        /// Determine la position géographique de l'utilisateur
        /// </summary>
        /// <returns>Géoposition de l'utilisateur</returns>
        private static async Task<Geoposition> UserPosition()
        {
            Geoposition userPosition = null;
            Geolocator gpsValuePosition = new Geolocator();

            try
            {
                userPosition = await gpsValuePosition.GetGeopositionAsync();
                return userPosition;
            }
            catch (Exception ex)
            {
                throw new Exception("Une erreure est survenue : " + ex.Message);
            }
        }


        public static async Task<Geoposition> GetUserPosition()
        {
            Geoposition userPosition = await UserPosition();
            return userPosition;
        }


        /// <summary>
        /// Retourne les coordonnées des coté de la map
        /// </summary>
        /// <param name="mapControl"></param>
        public static void GetMapCornerPosition(MapControl mapControl)
        {
            Geopoint geoP;
            mapControl.GetLocationFromOffset(new Point(0, 0), out geoP);
            LocationService.topLeft = geoP.Position;

            mapControl.GetLocationFromOffset(new Point(mapControl.ActualWidth, mapControl.ActualHeight), out geoP);
            LocationService.bottomRight = geoP.Position;
        }
    }
}