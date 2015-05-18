using CallOfBeer.API;
using CallOfBeer.API.Models;
using CallOfBeer.App;
using CallOfBeer.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace CallOfBeer.App
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
            Frame.Navigate(typeof(NewAccount));
        }

        private async void Authentifiate(object sender, RoutedEventArgs e)
        {
            try
            {
                Token tok = await this._apiService.GetTokenAsync(Login.Text, Password.Text);
                var setting = ApplicationData.Current.LocalSettings;
                setting.Values["AccessToken"] = tok.AccessToken;
                setting.Values["RefreshToken"] = tok.RefreshToken;
            }
            catch (Exception ex)
            {
                Frame.Navigate(typeof(Auth));
            }

            Frame.Navigate(typeof(MainPage));
        }
    }
}
