using AP1_WINUI.Visiteurs;
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

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace AP1_WINUI
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class Visiteur : Page
    {
        Data.Modeles.Utilisateur user;

        public Visiteur()
        {
            this.InitializeComponent();
        }

        private void nvVisit_SelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            var selectedItem = (Microsoft.UI.Xaml.Controls.NavigationViewItem)args.SelectedItem;
            string selectedItemContent = (string)selectedItem.Content;

            switch (selectedItemContent)
            {
                case "Éditer":
                    contentFrame.Navigate(typeof(FicheFrais));
                    break;
                case "Listes des fiches":
                    contentFrame.Navigate(typeof(ListeFiches));
                    break;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            user = e.Parameter as Data.Modeles.Utilisateur;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            contentFrame.Navigate(typeof(FicheFrais));
        }
    }
}
