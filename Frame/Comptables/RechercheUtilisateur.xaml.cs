using AP1_WINUI.Data.Modeles;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AP1_WINUI.Comptables
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RechercheUtilisateur : Page
    {
        public RechercheUtilisateur()
        {
            this.InitializeComponent();
        }

        private async void Valider_Click(object sender, RoutedEventArgs e)
        {
            string Recherche = BarreRecherche.Text;

            if (!string.IsNullOrEmpty(Recherche))
            {
                List<Utilisateur> utilisateurs = await Service.RechercheService.RechercherUtilisateur(Recherche);

                if (utilisateurs != null && utilisateurs.Count > 0)
                {
                    this.Frame.Navigate(typeof(Visiteurs.ListeFiches), new Visiteurs.ListeFichesParametres { user = utilisateurs.FirstOrDefault(), validation = true });
                }
                else
                {
                    var ContentDialog = new ContentDialog
                    {
                        Title = "Erreur",
                        Content = "Aucun utilisateur trouvé",
                        CloseButtonText = "Ok"
                    };
                    await ContentDialog.ShowAsync();
                    return;
                }
            }
            
        }
    }
}
