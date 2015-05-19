using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace CallOfBeer.Classes
{
    internal class MapBusiness
    {
        private Geolocator _geolocator;
        private bool _stop;

        // Task.Delay
        public MapBusiness()
        {
            this._geolocator = new Geolocator();
        }

        /// <summary>
        /// Permet de centrer la map sur la position de l'utilisateur
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        public async Task CenterMapOnUserPositionAsync(MapControl map)
        {
            Geoposition position = await this._geolocator.GetGeopositionAsync();
            await this.CenterMapOnPositionAsync(map, position.Coordinate.Point);
        }

        public async void StartFollowUser(MapControl map, string userName = null)
        {
            this._stop = false;
            await FollowUserPositionAsync(map, userName);
        }

        public void StopFollowUser()
        {
            this._stop = true;
        }

        // Méthodes privées
        private async Task CenterMapOnPositionAsync(MapControl map, Geopoint point)
        {
            // zoom 15, heading -> nord, pitch -> vue de dessus
            await map.TrySetViewAsync(point, 15, 0, 0, MapAnimationKind.Bow);
        }

        private async Task FollowUserPositionAsync(MapControl map, string userName)
        {
            Geoposition position = await this._geolocator.GetGeopositionAsync();

            MapIcon userIcon = new MapIcon();
            userIcon.Title = userName == null ? "Utilisateur" : userName;
            userIcon.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/ProfilIcon.png"));
            userIcon.NormalizedAnchorPoint = new Point(0.5, 1);
            userIcon.Location = position.Coordinate.Point;

            map.MapElements.Add(userIcon);

            while (!this._stop)
            {
                position = await this._geolocator.GetGeopositionAsync();
                userIcon.Location = position.Coordinate.Point;

                await Task.Delay(30000);
            }

            map.MapElements.Remove(userIcon);
        }

        public void DisplayEvent(MapControl map, Geopoint position, int eventId, TappedEventHandler tapped)
        {
            Image image = new Image();

            ImageSource source = new BitmapImage(new Uri("ms-appx:///Assets/BeerIcon.png", UriKind.Absolute));

            image.Stretch = Stretch.Uniform;
            image.Source = source;
            image.Name = eventId.ToString();
            image.Tapped += tapped;
            image.Width = 40;

            map.Children.Add(image);
            MapControl.SetLocation(image, position);
            MapControl.SetNormalizedAnchorPoint(image, new Point(0.5, 1));
        }

        public void ClearMap(MapControl map)
        {
            while (map.Children.Count > 0)
            {
                map.Children.RemoveAt(0);
            }
        }

        public GeoboundingBox GetMapBounding(MapControl map)
        {
            GeoboundingBox result = null;

            Geopoint topLeft = null;
            try
            {
                map.GetLocationFromOffset(new Point(0, 0), out topLeft);
            }
            catch
            {
                var topOfMap = new Geopoint(new BasicGeoposition()
                {
                    Latitude = 85,
                    Longitude = 0
                });

                Point topPoint;
                map.GetOffsetFromLocation(topOfMap, out topPoint);
                map.GetLocationFromOffset(new Point(0, topPoint.Y), out topLeft);
            }

            Geopoint bottomRight = null;
            try
            {
                map.GetLocationFromOffset(new Point(map.ActualWidth, map.ActualHeight), out bottomRight);
            }
            catch
            {
                var bottomOfMap = new Geopoint(new BasicGeoposition()
                {
                    Latitude = -85,
                    Longitude = 0
                });

                Point bottomPoint;
                map.GetOffsetFromLocation(bottomOfMap, out bottomPoint);
                map.GetLocationFromOffset(new Point(0, bottomPoint.Y), out bottomRight);
            }

            if (topLeft != null && bottomRight != null)
            {
                result = new GeoboundingBox(topLeft.Position, bottomRight.Position);
            }

            return result;
        }

        public async Task<Geoposition> GetUserPosition()
        {
            Geoposition userPosition = await this._geolocator.GetGeopositionAsync();
            return userPosition;
        }
    }
}
