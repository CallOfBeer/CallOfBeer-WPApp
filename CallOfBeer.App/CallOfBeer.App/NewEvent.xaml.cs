using CallOfBeer.API;
using CallOfBeer.App.Class;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour en savoir plus sur le modèle d’élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkID=390556

namespace CallOfBeer.App
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class NewEvent : Page
    {
        private APITools CallApi = new APITools();

        public NewEvent()
        {
            this.InitializeComponent();
        }

        private void Button_Taped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private async void Launch_Event(object sender, TappedRoutedEventArgs e)
        {

            Geoposition eventPosition = await LocationService.GetUserPosition();

            // Vérification des données saisient
            if (Regex.IsMatch(event_zip.Text, @"^[0-9]{5}$") && event_name.Text != "")
            {
                //Creer un objet datetime avec les deux champs
                DateTime getEventDate = new DateTime(
                    event_date.Date.Year,
                    event_date.Date.Month,
                    event_date.Date.Day,
                    event_time.Time.Hours,
                    event_time.Time.Minutes,
                    0);

                //convertis le DateTime en Time Stamp
                TimeSpan toTimeSpan = getEventDate.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0).ToUniversalTime();

                //Création de l'objet à envoyer
                AddEvents eventToSend = new AddEvents()
                {
                    eventName = event_name.Text.ToString(),
                    eventDate = ((int)toTimeSpan.TotalSeconds).ToString(),
                    addressLon = eventPosition.Coordinate.Longitude.ToString(),
                    addressLat = eventPosition.Coordinate.Latitude.ToString(),
                    addressAddress = event_adress.Text.ToString(),
                    addressZip = event_zip.Text.ToString(),
                    addressCity = event_city.Text.ToString(),
                    addressCountry = event_country.Text.ToString(),
                    addressName = event_adressname.Text.ToString()
                };

                //Envois à l'api
                var response = await CallApi.PostEvent(eventToSend);

                if (response.IsSuccessStatusCode)
                    Frame.Navigate(typeof(MainPage));
            }
        }
    }
}