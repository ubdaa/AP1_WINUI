using AP1_WINUI.Data.Modeles;
using AP1_WINUI.Service;
using AP1_WINUI.Visiteurs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
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
using Windows.UI.Xaml.Shapes;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace AP1_WINUI.Comptables
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class FichesValidation : Page
    {
        List<Data.Modeles.FicheFrais> listFichesFrais;

        public FichesValidation()
        {
            this.InitializeComponent();

            ListFiches.IsItemClickEnabled = true;
        }

        private async void ListFiches_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clickedItem = e.ClickedItem as StackPanel;
            if (clickedItem != null)
            {
                var textBlock = clickedItem.Children[0] as TextBlock;
                if (textBlock != null)
                {
                    var id = clickedItem.Children[2] as TextBlock;
                    Data.Modeles.FicheFrais ficheFrais = await FraisServices.RecupFicheFrais(int.Parse(id.Text));
                    this.Frame.Navigate(typeof(Visiteurs.FicheFrais), new NavigationParamFicheFrais { ficheFrais = ficheFrais, Consultation = true, Validation = true });
                }
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            listFichesFrais = await ValidationService.RecupFichesFraisAValider();

            if (listFichesFrais.Count != 0)
            {
                TextFichesVide.Visibility = Visibility.Collapsed;
            } else
            {
                return;
            }

            foreach (var fiche in listFichesFrais)
            {
                ListViewItem item = new ListViewItem();
                StackPanel stackPanel = new StackPanel();

                TextBlock textBlock = new TextBlock
                {
                    Text = "Fiche de frais du " + fiche.Date.Month.ToString() + "/" + fiche.Date.Year.ToString(),
                    FontSize = 24
                };
                TextBlock etat = new TextBlock
                {
                    Text = "Etat : " + fiche.Etat.ToString() + ". Par l'utilisateur : " + (await LoginService.NomUtilisateur(fiche.IdUtilisateur)),
                    FontSize = 18
                };
                TextBlock id = new TextBlock
                {
                    Text = fiche.IdFicheFrais.ToString(),
                    Visibility = Visibility.Collapsed
                };

                stackPanel.Children.Add(textBlock);
                stackPanel.Children.Add(etat);
                stackPanel.Children.Add(id);

                item.Content = stackPanel;
                item.Padding = new Thickness(20, 9, 20, 9);

                ListFiches.Items.Add(item);
            }
        }
    }
}
