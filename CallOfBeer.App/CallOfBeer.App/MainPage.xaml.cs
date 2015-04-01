using CallOfBeer.API;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using CallOfBeer.App.Class;
using System.Collections.ObjectModel;
using CallOfBeer.API.Models;
using Windows.Networking.Connectivity;
using Windows.UI.Popups;


// Pour en savoir plus sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=391641

namespace CallOfBeer.App
{


    public sealed partial class MainPage : Page
    {
        private BasicGeoposition topLeft;
        private bool IsNetworkConnected = false;
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
            this.IsNetworkConnected = NetworkConnect();
            if (!this.IsNetworkConnected)
            {
                var messageBox = new MessageDialog("La connexion internet n'est pas activée ou a été perdu. Les données affichées risquent de ne pas êtres à jour", "Erreur au lancement de l'application");
                messageBox.Commands.Add(new UICommand("Ok"));
                await messageBox.ShowAsync();
            }

            // Liste des évènements à retourner
            List<EventGet> listEventAvailiable = null;
            List<string> frontData = new List<string>();

            // Affichage de la map
            LocationService.MapLoader(MapHome);
            LocationService.GetMapCornerPosition(MapHome);

            // Coordonnées des coins de la map
            topLeft = LocationService.topLeft;
            bottomRight = LocationService.bottomRight;

            // Liste des évènements
            try
            {
                //Task<List<Events>> response = this._apiService.GetEvents(topLeft.Latitude, topLeft.Longitude, bottomRight.Latitude, bottomRight.Longitude);
                //response.Wait();

                // Lance la requete à l'api si une connexion internet est active
                if (this.IsNetworkConnected)
                {
                    // Requete à l'Api
                    IEnumerable<EventGet> machin = await this._apiService.GetEventsAsync(topLeft.Latitude, topLeft.Longitude, bottomRight.Latitude, bottomRight.Longitude);
                    //IEnumerable<EventGet> machin = await this._apiService.GetEventsAsync(45,-1,44,1);                    
                    listEventAvailiable = machin.ToList();

                    if (listEventAvailiable.Count != 0 && listEventAvailiable != null)
                    {
                        foreach (var item in listEventAvailiable)
                        {
                            LocationService.AddMapLocation(MapHome, item);
                        }
                    }
                    else
                    {
                        frontData.Add("Aucun évènement n'a été détecté.");
                    }
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



        private static bool NetworkConnect()
        {
            var profile = NetworkInformation.GetInternetConnectionProfile();
            if (profile != null)
            {
                var interfaceType = profile.NetworkAdapter.IanaInterfaceType;
                // Verifis que l'inteface type est une connexion de type wifi (71/6) ou de type 3g/4g (243/244)
                return (interfaceType == 71 || interfaceType == 6 || interfaceType == 243 || interfaceType == 244) ? true : false;
            }
            return false;
        }
    }
}
