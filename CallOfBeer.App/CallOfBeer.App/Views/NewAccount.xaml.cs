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

// Pour en savoir plus sur le modèle d’élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkID=390556

namespace CallOfBeer.Views
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class NewAccount : Page
    {
        public NewAccount()
        {
            this.InitializeComponent();
        }



        private void Creat_Account(object sender, RoutedEventArgs e)
        {
            if (User.Text != "" && Email.Text != "" && ConfirmPassword.Text != "" && Password.Text != "")
            {
                if (Conditions.IsChecked.HasValue && Conditions.IsChecked.Value == true)
                {
                   bool checkEmail = Regex.IsMatch(Email.Text.ToLower(),@"^[a-zA-Z0-9._-]+@[a-zA-Z0-9._-]{2,}.[a-z]{2,4}$");
                    if (checkEmail)
                    {
                        if (ConfirmPassword.Text == Password.Text)
                        {
                            //TODO appeler le service à l'api
                            //TODO rediriger l'utilisateur
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
