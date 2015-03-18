using CallOfBeer.API;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
using CallOfBeer.App.Class;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls.Maps;
using System.Threading.Tasks;
using CallOfBeer.API.Models;

// Pour en savoir plus sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=391641

namespace CallOfBeer.App
{


    public sealed partial class MainPage : Page
    {
        private BasicGeoposition topLeft;
        private BasicGeoposition bottomRight;
        private ObservableCollection<EventGet> eventListView = new ObservableCollection<EventGet>();
        private APIService _apiService;

        public MainPage()
        {
            this.InitializeComponent();
            this._apiService = new APIService();
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        private async void MainPageLoaded(object sender, RoutedEventArgs e)
        {
            // Liste des évènements à retourner
            List<EventGet> listEventAvailiable = null;
            List<string> frontData = new List<string>();

            // Affichage de la map
            LocationService.LoadMap(MapHome);
            LocationService.GetMapCornerPosition(MapHome);

            // Coordonnées des coins de la map
            topLeft = LocationService.topLeft;
            bottomRight = LocationService.bottomRight;

            // Liste des évènements
            try
            {
                //Task<List<Events>> response = this._apiService.GetEvents(topLeft.Latitude, topLeft.Longitude, bottomRight.Latitude, bottomRight.Longitude);
                //response.Wait();
                IEnumerable<EventGet> machin = await this._apiService.GetEventsAsync(topLeft.Latitude, topLeft.Longitude, bottomRight.Latitude, bottomRight.Longitude);
                listEventAvailiable = machin.ToList();

                if (listEventAvailiable.Count != 0 && listEventAvailiable != null)
                {
                    //EventListView.DataContext = listEventAvailiable;
                    foreach (var item in listEventAvailiable)
                    {
                        LocationService.AddMapLocation(MapHome, item);
                    }
                }

                else
                {
                    //TODO : afficher qu'il n'y a pas d'events
                    frontData.Add("Aucun évènement n'a été détecté.");
                }
            }

            catch (NullReferenceException ex)
            {
                frontData.Add("Aucun évènement n'a été détecté.");
            }
        }

        private void NewCall(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(NewEvent));
        }
    }
}
