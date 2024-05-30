using AP1_WINUI.Data;
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
using AP1_WINUI.Data.Modeles;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AP1_WINUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Login : Page
    {
        public Login()
        {
            this.InitializeComponent();
            
            // on change le titre de la fenêtre
            Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().Title = "Connexion";
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            Utilisateur user = await Service.LoginService.Login(txtUsername.Text, txtPassword.Password);

            if (user == null) return;

            switch (user.Role)
            {
                case Role.VISITEUR:
                    Frame.Navigate(typeof(Visiteur), user);
                    break;
            }
        }
    }
}
