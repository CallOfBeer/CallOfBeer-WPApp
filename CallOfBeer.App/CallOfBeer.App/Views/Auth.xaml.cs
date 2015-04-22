using CallOfBeer.API;
using CallOfBeer.App;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace CallOfBeer.Views
{
    public sealed partial class Auth : Page
    {
        private readonly APIService _apiService;

        public Auth()
        {
            this.InitializeComponent();
            this._apiService = new APIService();
        }

        private void MainPageLoaded(object sender, RoutedEventArgs e)
        {

        }

        private void NewAccount(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void Authentifiate(object sender, RoutedEventArgs e)
        {
            //TODO Envoyer la requete à l'api


        }
    }
}
