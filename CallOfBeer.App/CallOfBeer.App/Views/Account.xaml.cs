using CallOfBeer.API;
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

// Pour en savoir plus sur le modèle d’élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkID=390556

namespace CallOfBeer.Views
{
    public sealed partial class Account : Page
    {
        private readonly APIService _apiService;

        public Account()
        {
            this.InitializeComponent();
            this._apiService = new APIService();
        }

        private void MainPageLoaded(object sender, RoutedEventArgs e)
        {

        }

        private void SendAccount(object sender, RoutedEventArgs e)
        {
            string pseudo = null, pwd = null, pwd_verif = null, email = null;
            bool require = false;

            // Verification des champs
            if (Pseudo.Text == null || Password == null || Password_check == null || Email.Text == null)
                require = true;
            else
            {
                pseudo = Pseudo.Text;
                pwd = Password.Text;
                pwd_verif = Password_check.Text;
                email = Email.Text;
            }


            // Concordence des mots de passes
                



            throw new NotImplementedException();
        }
    }
}
