using CallOfBeer.API;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using CallOfBeer.API.Models;
using Windows.UI.Popups;
using CallOfBeer.App;

namespace CallOfBeer.Views
{
    public sealed partial class NewAccount : Page
    {
        private APIService _apiService; 

        public NewAccount()
        {
            this.InitializeComponent();
            this._apiService = new APIService();
        }

        private async void Creat_Account(object sender, RoutedEventArgs e)
        {
            if (User.Text != "" && Email.Text != "" && ConfirmPassword.Password != "" && Password.Password != "")
            {
                if (Conditions.IsChecked.HasValue && Conditions.IsChecked.Value == true)
                {
                   bool checkEmail = Regex.IsMatch(Email.Text.ToLower(),@"^[a-zA-Z0-9._-]+@[a-zA-Z0-9._-]{2,}.[a-z]{2,4}$");
                    if (checkEmail)
                    {
                        if (ConfirmPassword.Password == Password.Password)
                        {
                            // Appeler le service à l'api
                            User newUser = await _apiService.RegisterAsync(User.Text, Password.Password, Email.Text.ToLower());
                            if (newUser == null)
                            {
                                var messageBox = new MessageDialog("Erreur lors de l'envois du nouvelle utilisateur. Veuillez ressayer plus tard.");
                                messageBox.Commands.Add(new UICommand("Ok"));
                                await messageBox.ShowAsync();
                            }
                            Frame.Navigate(typeof(Auth));
                        }
                        else
                        {
                            ConfirmPassword.BorderBrush = new SolidColorBrush(Colors.Red);
                            Password.BorderBrush = new SolidColorBrush(Colors.Red);
                        }
                    }
                    else
                    {
                        Email.BorderBrush = new SolidColorBrush(Colors.Red);
                    }
                }
            }



        }
    }
}
