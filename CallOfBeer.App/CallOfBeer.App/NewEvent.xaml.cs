using CallOfBeer.API;
using CallOfBeer.API.Models;
using CallOfBeer.App.Class;
using System;
using System.Text.RegularExpressions;
using Windows.Devices.Geolocation;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// Pour en savoir plus sur le modèle d’élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkID=390556

namespace CallOfBeer.App
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class NewEvent : Page
    {
        private APIService _apiService;

        public NewEvent()
        {
            this.InitializeComponent();
            this._apiService = new APIService();

            //this.event_name.KeyUp += new KeyEventHandler(CloseKeyBoard);
            //this.event_adressname.KeyUp += new KeyEventHandler(CloseKeyBoard);
            //this.event_adress.KeyUp += new KeyEventHandler(CloseKeyBoard);
            //this.event_city.KeyUp += new KeyEventHandler(CloseKeyBoard);
            //this.event_country.KeyUp += new KeyEventHandler(CloseKeyBoard);
            
        }

        private void IndexReturn(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private async void SendEvent(object sender, RoutedEventArgs e)
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
                //TimeSpan toTimeSpan = getEventDate.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0).ToUniversalTime();
                int timeSpan = (int)getEventDate.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                Geoposition test;

                double localAdressLong = eventPosition.Coordinate.Longitude;
                double localAdressLat = eventPosition.Coordinate.Latitude;

                //Création de l'objet à envoyer
                EventPost eventToSend = new EventPost()
                {
                    Name = event_name.Text,
                    Date = timeSpan,
                    Longitude = localAdressLong,
                    Latitude = localAdressLat,
                    Address = event_adress.Text,
                    Zip = Convert.ToInt32(event_zip.Text),
                    City = event_city.Text,
                    Country = event_country.Text,
                    AddressName = event_adressname.Text
                };

                //Envois à l'api
                bool response = await this._apiService.PostEventAsync(eventToSend);

                if (response)
                    Frame.Navigate(typeof(MainPage));
            }
        }
            
        private void CloseKeyBoard(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                //Definit l'évènement comme traité
                var control = sender as Control;
                LoseFocus(sender);
                
            }
        }

        private void LoseFocus(object sender)
        {
            var control = sender as Control;
            var isTabStop = control.IsTabStop;
            control.IsTabStop = false;
            control.IsEnabled = false;
            control.IsEnabled = true;
            control.IsTabStop = isTabStop;
        }
    }
}