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
using Windows.Storage;


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
        private bool minPageErrors = false;
        

        public MainPage()
        {
            this.InitializeComponent();
            this._apiService = new APIService();
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        private async void MainPageLoaded(object sender, RoutedEventArgs e)
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            Object value = localSettings.Values["AccessToken"];
            if (value == null)
                Frame.Navigate(typeof(Auth));

            this.IsNetworkConnected = NetworkConnect();

            if (this.IsNetworkConnected)
            {

                // Liste des évènements à retourner
                List<EventGet> listEventAvailiable = null;

                // Affichage de la map
                LocationService.MapLoader(MapHome);
                LocationService.GetMapCornerPosition(MapHome);

                // Coordonnées des coins de la map
                topLeft = LocationService.topLeft;
                bottomRight = LocationService.bottomRight;

                // Récupére la liste des event
                try
                {
                    // Requete à l'Api
                    IEnumerable<EventGet> machin = await this._apiService.GetEventsAsync(topLeft.Latitude, topLeft.Longitude, bottomRight.Latitude, bottomRight.Longitude);
                    listEventAvailiable = machin.ToList();

                    // Vérification de la liste
                    if (listEventAvailiable.Count != 0 && listEventAvailiable != null)
                        foreach (var item in listEventAvailiable)
                            LocationService.AddMapLocation(MapHome, item);
                }
                
                catch (Exception)
                {
                    this.minPageErrors = true;
                }

                // Popup d'erreur lors du lancement de l'application
                if (this.minPageErrors)
                {
                    var messageBoxMainPage = new MessageDialog("Une Erreur est survenu lors de l'execution de l'application.", "Erreur au lancement de l'application");
                    messageBoxMainPage.Commands.Add(new UICommand("Ok"));
                    await messageBoxMainPage.ShowAsync();
                }
            }

            else
            {
                var messageBox = new MessageDialog("La connexion internet n'est pas activée ou a été perdu. Les données affichées risquent de ne pas êtres à jour", "Erreur de connexion au réseau");
                messageBox.Commands.Add(new UICommand("Ok"));
                await messageBox.ShowAsync();
            }
        }

        private void NewCall(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(NewEvent));
        }

        /// <summary>
        /// Vérification de la connexion internet du téléphone
        /// </summary>
        /// <returns>True si la connexion est de type wifi (code : 71 / 6) ou 3g / 4g (code 243 / 244)</returns>
        private static bool NetworkConnect()
        {
            var profile = NetworkInformation.GetInternetConnectionProfile();
            if (profile != null)
            {
                var interfaceType = profile.NetworkAdapter.IanaInterfaceType;
                return (interfaceType == 71 || interfaceType == 6 || interfaceType == 243 || interfaceType == 244) ? true : false;
            }
            return false;
        }
    }
}
