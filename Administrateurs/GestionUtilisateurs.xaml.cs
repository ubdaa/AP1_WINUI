using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.UserActivities;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace AP1_WINUI.Administrateurs
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class GestionUtilisateurs : Page
    {
        List<Data.Modeles.Utilisateur> users;

        public GestionUtilisateurs()
        {
            this.InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            users = await Service.GestionUtilisateursService.RecupererTousUtilisateurs();

            datagridUtilisateurs.ItemsSource = users.ToList();
        }

        private void datagridUtilisateurs_AutoGeneratingColumn(object sender, Microsoft.Toolkit.Uwp.UI.Controls.DataGridAutoGeneratingColumnEventArgs e)
        {
            e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            switch (e.Column.Header)
            {
                case "IdUtilisateur":
                    e.Column.Header = "Identifiant";
                    e.Column.IsReadOnly = true;
                    break;

                case "Role":
                    e.Column.Header = "Rôle";
                    e.Column.IsReadOnly = true;
                    break;

                case "FicheFrais":
                    e.Column.Visibility = Visibility.Collapsed;
                    break;
            } 
        }

        private void AJoutUtilisateur_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
