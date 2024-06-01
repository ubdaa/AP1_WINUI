using AP1_WINUI.Data.Modeles;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace AP1_WINUI.Visiteurs
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    /// 

    public sealed partial class FicheFrais : Page
    {

        List<TypeFrais> tf = new List<TypeFrais>();
        List<string> SourceComboBox = new List<string>();

        Data.Modeles.FicheFrais ficheFrais;
        Data.Modeles.Utilisateur utilisateur;

        List<Forfait> listForfait;

        double total = 0;

        #region METHODES

        private async void AjouterFraisForfait()
        {
            ContentDialog dialog = new ContentDialog();

            Popups.AjoutForfait ajoutForfait = new Popups.AjoutForfait();
            {
                dialog.Title = "Ajout d'un forfait";
                dialog.PrimaryButtonText = "Ajouter";
                dialog.SecondaryButtonText = "Annuler";
                ajoutForfait.comboBoxTypeFrais.ItemsSource = SourceComboBox;
                ajoutForfait.comboBoxTypeFrais.SelectedIndex = 0;
                ajoutForfait.datePicker.MinDate = new DateTime(ficheFrais.Date.Year, ficheFrais.Date.Month, 11);
                ajoutForfait.datePicker.MaxDate = new DateTime(ficheFrais.Date.Year, ficheFrais.Date.AddMonths(1).Month, 10);
                dialog.Content = ajoutForfait;
                dialog.DefaultButton = ContentDialogButton.Primary;
            }

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                if (ajoutForfait.datePicker.Date == null)
                {
                    var erreur = new Windows.UI.Popups.MessageDialog("Veuillez renseigner une date", "Erreur");
                    await erreur.ShowAsync();
                    return;
                }
                if (ajoutForfait.comboBoxTypeFrais.SelectedIndex == -1)
                {
                    var erreur = new Windows.UI.Popups.MessageDialog("Veuillez renseigner un type de frais", "Erreur");
                    await erreur.ShowAsync();
                    return;
                }

                DateTimeOffset date = (DateTimeOffset)ajoutForfait.datePicker.Date;
                DateTime dateSelect = date.Date;
                await Service.FraisServices.AjoutForfait(ajoutForfait.comboBoxTypeFrais.SelectedIndex + 1, dateSelect, ficheFrais.IdFicheFrais);
            }

            await RefreshFiche();
            ChargerForfait();
        }

        private async void SupprimerFraisForfait()
        {
            var forfait = (Forfait)datagridForfait.SelectedItem;

            if (forfait != null)
            {
                await Service.FraisServices.SupprimerForfait(forfait.IdForfait);
                await RefreshFiche();
                ChargerForfait();
            }
            else
            {
                var dialog = new Windows.UI.Popups.MessageDialog("Aucun forfait n'a été sélectionné", "Erreur");
                await dialog.ShowAsync();
            }

            datagridForfait.SelectedIndex = -1;
        }

        private async Task RefreshFiche()
        {
            ficheFrais.Forfaits = await Service.FraisServices.RecupForfait(ficheFrais.IdFicheFrais);
        }

        private void ChargerForfait()
        {
            datagridForfait.ItemsSource = null;
            ficheFrais.Forfaits.Sort((x, y) => DateTime.Parse(x.Date).CompareTo(DateTime.Parse(y.Date)));

            // Calcul du total des frais forfaits et visibilité du texte
            {
                total = 0;
                foreach (Forfait f in ficheFrais.Forfaits) total += (f.Montant * f.Quantite);
                total = Math.Round(total, 2);
                TotalForfait.Text = "Total des Frais Forfaits : " + total + " €";

                if (ficheFrais.Forfaits.Count == 0) TotalForfait.Visibility = Visibility.Collapsed;
                else TotalForfait.Visibility = Visibility.Visible;
            }

            datagridForfait.ItemsSource = ficheFrais.Forfaits.ToList();
        }

        #endregion

        public FicheFrais()
        {
            this.InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            tf = await Service.FraisServices.RecupTypeFrais();

            foreach (TypeFrais t in tf)
            {
                SourceComboBox.Add(t.Nom);
            }

            utilisateur = await Service.LoginService.UtilisateurBase(ficheFrais.IdUtilisateur);

            SubTitle.Text = "11 " + ficheFrais.Date.ToString("MMMM yyyy") + " - 10 " + ficheFrais.Date.AddMonths(1).ToString("MMMM yyyy");
            Name.Text = "De " + utilisateur.Username + " - Fiche n°" + ficheFrais.IdFicheFrais;

            await RefreshFiche();
            ChargerForfait();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ficheFrais = e.Parameter as Data.Modeles.FicheFrais;
        }

        private async void datagridForfait_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Column.Header.ToString() == "Quantite")
            {
                string input = (e.EditingElement as TextBox).Text;
                int quantite;
                
                try { quantite = int.Parse(input); }
                catch (Exception) { return; }

                if (quantite < 0)
                {
                    (e.EditingElement as TextBox).Text = "0";
                    e.Cancel = true;
                    var erreur = new Windows.UI.Popups.MessageDialog("La quantité ne peut pas être négative", "Erreur");
                    await erreur.ShowAsync();
                    return;
                }

                await Service.FraisServices.ModifierForfait((datagridForfait.SelectedItem as Forfait).IdForfait, quantite);
                await RefreshFiche();
                ChargerForfait();
            }
        }

        #region FRAIS FORFAITS

        private void AjouterForfait_Click(object sender, RoutedEventArgs e)
        {
            AjouterFraisForfait();
        }

        private void SupprimerForfait_Click(object sender, RoutedEventArgs e)
        {
            SupprimerFraisForfait();
        }

        private void datagridForfait_AutoGeneratingColumn(object sender, Microsoft.Toolkit.Uwp.UI.Controls.DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "IdForfait":
                    e.Column.Visibility = Visibility.Collapsed;
                    e.Column.IsReadOnly = true;
                    break;
                case "Date":
                    e.Column.IsReadOnly = true;
                    break;
                case "IdTypeFrais":
                    e.Column.Visibility = Visibility.Collapsed;
                    e.Column.IsReadOnly = true;
                    break;
                case "Nom":
                    e.Column.IsReadOnly = true;
                    break;
                case "Montant":
                    e.Column.IsReadOnly = true;
                    break;
                case "Etat":
                    e.Column.IsReadOnly = true;
                    break;
            }
        }

        #endregion
    }
}
