using CallOfBeer.API;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using CallOfBeer.API.Models;
using Windows.Networking.Connectivity;
using Windows.UI.Popups;
using Windows.Storage;
using CallOfBeer.Classes;
using Windows.UI.Xaml.Controls.Primitives;
using CallOfBeer.Pages;
using CallOfBeer.Models;
using System.Threading.Tasks;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.Services.Maps;


// Pour en savoir plus sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=391641

namespace CallOfBeer.App
{


    public sealed partial class MainPage : Page
    {
        private MapBusiness _mapService;

        private ApplicationDataContainer _localSettings;
        private string _accessToken;

        private APIService _apiService;

        private List<EventGet> _eventList;
        private bool _firstLoad = true;
        private bool _eventRefreshing = false;

        private Popup _popup;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            // On récupère le token s'il existe
            this._localSettings = ApplicationData.Current.LocalSettings;
            this._accessToken = this._localSettings.Values["AccessToken"] as string;

            this._mapService = new MapBusiness();
            this._apiService = new APIService();

            this._eventList = new List<EventGet>();

            this.Loaded += MainPage_Loaded;
        }

        /// <summary>
        /// Une fois la page chargée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (this._firstLoad)
            {
                this._firstLoad = false;
                if (this._accessToken != null)
                {
                    this._apiService.Connect(this._accessToken);

                    this.AddDecoButton();
                }
                else
                {
                    this.AddConnexionButton();
                }
            }
        }

        private void AddConnexionButton()
        {
            AppBarButton conButton = new AppBarButton();
            conButton.Label = "Connexion";
            conButton.Click += conButton_Click;
            CommandBar.SecondaryCommands.Add(conButton);
        }

        private void AddDecoButton()
        {
            AppBarButton decoButton = new AppBarButton();
            decoButton.Label = "Déconnexion";
            decoButton.Click += decoButton_Click;
            CommandBar.SecondaryCommands.Add(decoButton);
        }

        void decoButton_Click(object sender, RoutedEventArgs e)
        {
            this._accessToken = null;
            this._localSettings.Values["AccessToken"] = null;
            this._apiService.Disconnect();

            CommandBar.SecondaryCommands.RemoveAt(0);
            this.AddConnexionButton();
        }

        void conButton_Click(object sender, RoutedEventArgs e)
        {
            this.LeavePage(typeof(LoginPage));
        }

        /// <summary>
        /// Invoqué lorsque cette page est sur le point d'être affichée dans un frame.
        /// </summary>
        /// <param name="e">Données d’événement décrivant la manière dont l’utilisateur a accédé à cette page.
        /// Ce paramètre est généralement utilisé pour configurer la page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: préparer la page pour affichage ici.

            // TODO: si votre application comporte plusieurs pages, assurez-vous que vous
            // gérez le bouton Retour physique en vous inscrivant à l’événement
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed.
            // Si vous utilisez le NavigationHelper fourni par certains modèles,
            // cet événement est géré automatiquement.

            this._mapService.StartFollowUser(Map);
            await this._mapService.CenterMapOnUserPositionAsync(Map);
            await this.DisplayEvents();

            if (e.Parameter != null)
            {
                if (e.Parameter.GetType() == typeof(LoginModel))
                {
                    LoginModel loginModel = e.Parameter as LoginModel;
                    if (loginModel.Type == TypeEnum.Login)
                    {
                        try
                        {
                            await this.Login(loginModel);

                            // pas le bon bouton?
                            CommandBar.SecondaryCommands.RemoveAt(0);
                            this.AddDecoButton();
                        }
                        catch (Exception ex)
                        {
                            this.GenerateMessageDialog("Erreur lors de la connexion, vérifiez vos identifiants de connexion.");
                        }
                    }
                    else
                    {
                        try
                        {
                            User user = await this._apiService.RegisterAsync(loginModel.Login, loginModel.Pass, loginModel.Mail);

                            await this.Login(loginModel);
                            CommandBar.SecondaryCommands.RemoveAt(0);
                            this.AddDecoButton();
                        }
                        catch (Exception ex)
                        {
                            this.GenerateMessageDialog("Erreur lors de l'enregistrement.");
                        }
                    }
                }
                else if (e.Parameter.GetType() == typeof(EventModel))
                {
                    try
                    {
                        EventModel eventModel = e.Parameter as EventModel;
                        // TODO traitement -> création de l'évenement
                        int timestamp = (int)eventModel.Date.Subtract(new DateTime(1970, 1, 1, 0,0,0)).TotalSeconds;

                        string address = string.Format("{0} {1}", eventModel.Address, eventModel.City);

                        GeolocByAddress geoloc = await this._apiService.GetGeolocAsync(address);

                        EventPost eventPost = new EventPost()
                        {
                            Name = eventModel.Name,
                            Date = timestamp,
                            Latitude = geoloc.Latitude,
                            Longitude = geoloc.Longitude,
                            Address = eventModel.Address,
                            City = eventModel.City
                        };
                        EventGet eventGet = await this._apiService.PostEventAsync(eventPost);
                        this.DisplayEvent(eventGet);
                        this._eventList.Add(eventGet);
                    }
                    catch (Exception ex)
                    {
                        this.GenerateMessageDialog("Erreur lors de l'envoi de l'évènement.");
                    }
                }
            }
        }

        private async Task Login(LoginModel loginModel)
        {
            Token token = await this._apiService.GetTokenAsync(loginModel.Login, loginModel.Pass);
            this._accessToken = token.AccessToken;
            this._localSettings.Values["AccessToken"] = token.AccessToken;
            this._apiService.Connect(this._accessToken);
        }

        private async void BTN_UserPosition_Click(object sender, RoutedEventArgs e)
        {
            await this._mapService.CenterMapOnUserPositionAsync(Map);
        }

        private void BTN_AddEvent_Click(object sender, RoutedEventArgs e)
        {
            if (this._accessToken != null)
            {
                this.LeavePage(typeof(CreatePage));
            }
            else
            {
                this.GenerateMessageDialog("Vous devez être identifié pour posté un event.");
            }

        }

        private async void BTN_QuickEvent_Click(object sender, RoutedEventArgs e)
        {
            if (this._accessToken != null)
            {
                try
                {
                    Geoposition position = await this._mapService.GetUserPosition();
                    //AddressByGeoloc address = await this._apiService.GetAddressAsync(position.Coordinate.Point.Position.Latitude,
                    //    position.Coordinate.Point.Position.Longitude);

                    MapLocationFinderResult finderResult = await MapLocationFinder.FindLocationsAtAsync(position.Coordinate.Point);


                    //EventModel eventModel = new EventModel()
                    //{
                    //    City = address.City,
                    //    Address = address.Address,
                    //    Date = DateTime.Now,
                    //    Name = "Beer !"
                    //};

                    EventModel eventModel = new EventModel();

                    if (finderResult.Status == MapLocationFinderStatus.Success)
                    {
                        var selectedLocation = finderResult.Locations.First();
                        eventModel.Address = String.Format("{0} {1}",selectedLocation.Address.StreetNumber, selectedLocation.Address.Street);
                        eventModel.Date = DateTime.Now;
                        eventModel.City = selectedLocation.Address.Town;
                        eventModel.Name = "Beer !";
                    }

                    this.LeavePage(typeof(CreatePage), eventModel);
                }
                catch (Exception ex)
                {
                    this.GenerateMessageDialog("Une erreur est survenue.");
                }
            }
            else
            {
                this.GenerateMessageDialog("Vous devez être identifié pour posté un event.");
            }

        }

        private async Task DisplayEvents()
        {
            GeoboundingBox geoboundingBox = this._mapService.GetMapBounding(Map);
            IEnumerable<EventGet> events = await this._apiService.GetEventsAsync(
                geoboundingBox.NorthwestCorner.Latitude, geoboundingBox.NorthwestCorner.Longitude,
                geoboundingBox.SoutheastCorner.Latitude, geoboundingBox.SoutheastCorner.Longitude);

            foreach (var item in events)
            {
                if (!this._eventList.Any(e => e.Id == item.Id))
                {
                    this._eventList.Add(item);
                    this.DisplayEvent(item);
                }
            }
        }

        // Afficher les informations de l'evenement concerné
        public async void EventPinTapped(object sender, TappedRoutedEventArgs e)
        {
            Image image = e.OriginalSource as Image;
            int eventId = Convert.ToInt32(image.Name);
            EventGet eventGet = await this._apiService.GetEventByIdAsync(eventId);

            Geopoint position = new Geopoint(new BasicGeoposition()
            {
                Latitude = eventGet.Address.Latitude,
                Longitude = eventGet.Address.Longitude
            });

            MapLocationFinderResult finderResult = await MapLocationFinder.FindLocationsAtAsync(position);
            string address = string.Empty;

            if (finderResult.Status == MapLocationFinderStatus.Success)
            {
                var selectedLocation = finderResult.Locations.First();
                address = String.Format("{0} {1} ; {2} ", selectedLocation.Address.StreetNumber, selectedLocation.Address.Street,
                    selectedLocation.Address.Town);
            }
            if (this._popup != null)
            {
                this._popup.IsOpen = false;
            }
            this._popup = this.GeneratePopup(eventGet, address);
            this._popup.IsOpen = true;
        }

        private void DisplayEvent(EventGet eventGet)
        {
            Geopoint eventPosition = new Geopoint(new BasicGeoposition()
            {
                Latitude = eventGet.Address.Latitude,
                Longitude = eventGet.Address.Longitude
            });
            this._mapService.DisplayEvent(Map, eventPosition, eventGet.Id, EventPinTapped);
        }

        private async Task RefreshEventList()
        {
            if (!this._eventRefreshing)
            {
                this._eventRefreshing = true;

                await this.DisplayEvents();

                await Task.Delay(5000);

                this._eventRefreshing = false;
            }
        }

        private async void Map_CenterChanged(MapControl sender, object args)
        {
            await this.RefreshEventList();
        }

        private Popup GeneratePopup(EventGet eventGet, string addressTxt)
        {
            Popup pop = new Popup();
            StackPanel sp = new StackPanel();
            TextBlock title = new TextBlock()
            {
                Text = "Nom de l'évènement : " + eventGet.Name,
                FontSize = 20,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(20, 10, 20, 0)
            };

            TextBlock address = new TextBlock()
            {
                Text = "Adresse : " + addressTxt, // TODO : implémenter
                FontSize = 20,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(20, 10, 20, 0)
            };

            TextBlock date = new TextBlock()
            {
                Text = "Date : " + eventGet.Date.ToString("d/M/yyyy h:mm"),
                FontSize = 20,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(20, 10, 20, 0)
            };
            sp.Children.Add(title);
            sp.Children.Add(address);
            sp.Children.Add(date);
            sp.Margin = new Thickness(0, 4 * MainGrid.ActualHeight / 5, 0, 0);
            sp.Height = 2 * MainGrid.ActualHeight / 5;
            sp.Width = Frame.ActualWidth;
            sp.Background = new SolidColorBrush(Colors.Black);

            pop.Child = sp;

            return pop;
        }

        // Génère un MessageDialog
        private async void GenerateMessageDialog(string message)
        {
            MessageDialog messageBox = new MessageDialog(message);
            await messageBox.ShowAsync();
        }

        private void LeavePage(Type newPageType, Object obj = null)
        {
            if (this._popup != null)
            {
                this._popup.IsOpen = false;
            }
            this._mapService.StopFollowUser();
            if (obj == null)
            {
                Frame.Navigate(newPageType);
            }
            else
            {
                Frame.Navigate(newPageType, obj);
            }
            
        }
    }
}
