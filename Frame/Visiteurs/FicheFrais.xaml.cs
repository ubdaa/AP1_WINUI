using AP1_WINUI.Data.Modeles;
using AP1_WINUI.Popups;
using AP1_WINUI.Service;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

    public class NavigationParamFicheFrais
    {
        public Data.Modeles.FicheFrais ficheFrais;
        public bool Consultation = false;
        public bool Validation = false;
    }

    public sealed partial class FicheFrais : Page
    {

        List<TypeFrais> tf = new List<TypeFrais>();
        List<string> SourceComboBox = new List<string>();

        Data.Modeles.FicheFrais ficheFrais;
        Data.Modeles.Utilisateur utilisateur;

        List<Forfait> listForfait;

        double total = 0;
        double totalForfait = 0;
        double totalHorsForfait = 0;

        bool consultation = false;
        bool validation = false;

        #region METHODES

        #region FRAIS FORFAITS

        private async void AjouterFraisForfait()
        {
            ContentDialog dialog = new ContentDialog();

            Popups.AjoutForfait ajoutForfait = new Popups.AjoutForfait();
            {
                dialog.Title = "Ajout d'un frais forfait";
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
                totalForfait = total;

                if (ficheFrais.Forfaits.Count == 0) TotalForfait.Visibility = Visibility.Collapsed;
                else TotalForfait.Visibility = Visibility.Visible;
            }

            datagridForfait.ItemsSource = ficheFrais.Forfaits.ToList();
            ChangerTotalFiche();
        }

        #endregion

        #region FRAIS HORS FORFAITS

        private async void AjouterFraisHorsForfait()
        {
            ContentDialog dialog = new ContentDialog();

            Popups.AjoutHorsForfait ajoutHorsForfait = new Popups.AjoutHorsForfait();
            {
                dialog.Title = "Ajout d'un frais hors forfait";
                dialog.PrimaryButtonText = "Ajouter";
                dialog.SecondaryButtonText = "Annuler";
                ajoutHorsForfait.datePicker.MinDate = new DateTime(ficheFrais.Date.Year, ficheFrais.Date.Month, 11);
                ajoutHorsForfait.datePicker.MaxDate = new DateTime(ficheFrais.Date.Year, ficheFrais.Date.AddMonths(1).Month, 10);
                dialog.Content = ajoutHorsForfait;
                dialog.DefaultButton = ContentDialogButton.Primary;
            }

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                if (ajoutHorsForfait.datePicker.Date == null)
                {
                    var erreur = new Windows.UI.Popups.MessageDialog("Veuillez renseigner une date", "Erreur");
                    await erreur.ShowAsync();
                    return;
                }
                if (string.IsNullOrEmpty(ajoutHorsForfait.nomHorsForfait.Text))
                {
                    var erreur = new Windows.UI.Popups.MessageDialog("Veuillez renseigner un type de frais", "Erreur");
                    await erreur.ShowAsync();
                    return;
                }

                DateTimeOffset date = (DateTimeOffset)ajoutHorsForfait.datePicker.Date;
                DateTime dateSelect = date.Date;
                await Service.FraisServices.AjoutHorsForfait(ajoutHorsForfait.nomHorsForfait.Text, dateSelect, 0, ficheFrais.IdFicheFrais);
            }

            await RefreshFiche();
            ChargerHorsForfait();
        }

        private async void SupprimerFraisHorsForfait()
        {
            var horsForfait = (HorsForfait)datagridHorsForfait.SelectedItem;

            if (horsForfait != null)
            {
                await Service.FraisServices.SupprimerHorsForfait(horsForfait.IdHorsForfait);
                await RefreshFiche();
                ChargerHorsForfait();
            }
            else
            {
                var dialog = new Windows.UI.Popups.MessageDialog("Aucun hors forfait n'a été sélectionné", "Erreur");
                await dialog.ShowAsync();
            }

            datagridHorsForfait.SelectedIndex = -1;
        }

        private void ChargerHorsForfait()
        {
            datagridHorsForfait.ItemsSource = null;
            ficheFrais.HorsForfaits.Sort((x, y) => DateTime.Parse(x.Date).CompareTo(DateTime.Parse(y.Date)));

            // Calcul du total des frais hors forfaits et visibilité du texte
            {
                total = 0;
                foreach (HorsForfait f in ficheFrais.HorsForfaits) total += f.Montant;
                total = Math.Round(total, 2);
                TotalHorsForfait.Text = "Total des Frais Hors Forfaits : " + total + " €";
                totalHorsForfait = total;

                if (ficheFrais.HorsForfaits.Count == 0) TotalHorsForfait.Visibility = Visibility.Collapsed;
                else TotalHorsForfait.Visibility = Visibility.Visible;
            }

            datagridHorsForfait.ItemsSource = ficheFrais.HorsForfaits.ToList();
            ChangerTotalFiche();
        }

        #endregion

        private async Task RefreshFiche()
        {
            ficheFrais.Forfaits = await Service.FraisServices.RecupForfait(ficheFrais.IdFicheFrais);
            ficheFrais.HorsForfaits = await Service.FraisServices.RecupHorsForfait(ficheFrais.IdFicheFrais);
        }

        private void ChangerTotalFiche()
        {
            TotalFiche.Text = "Total de la fiche : " + (totalForfait + totalHorsForfait) + " €";
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
            Name.Text = "De " + utilisateur.Username + " - Fiche n°" + ficheFrais.IdFicheFrais + " - Etat fiche : " + ficheFrais.Etat.ToString();

            await RefreshFiche();
            ChargerForfait();
            ChargerHorsForfait();

            InitialisationConsultation();
            InitialisationValidation();
        }

        private void InitialisationConsultation()
        {
            if (consultation)
            {
                OutilsForfait.Visibility = Visibility.Collapsed;
                OutilsHorsForfaits.Visibility = Visibility.Collapsed;
            } else
            {
                RetourConsultationBtn.Visibility = Visibility.Collapsed;
            }
        }

        private void InitialisationValidation()
        {
            if (!validation)
            {
                EffectActionBtn.Visibility = Visibility.Collapsed;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            NavigationParamFicheFrais param = e.Parameter as NavigationParamFicheFrais;

            ficheFrais = param.ficheFrais;
            consultation = param.Consultation;
            validation = param.Validation;
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
            e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
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
                case "Quantite":
                    if (consultation) e.Column.IsReadOnly = true;
                    break;
                case "Etat":
                    e.Column.IsReadOnly = true;
                    break;
            }
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

        #endregion

        #region FORFAITS HORS FORFAITS

        private void AjouterHorsForfait_Click(object sender, RoutedEventArgs e)
        {
            AjouterFraisHorsForfait();
        }

        private void SupprimerHorsForfait_Click(object sender, RoutedEventArgs e)
        {
            SupprimerFraisHorsForfait();
        }

        private void datagridHorsForfait_AutoGeneratingColumn(object sender, Microsoft.Toolkit.Uwp.UI.Controls.DataGridAutoGeneratingColumnEventArgs e)
        {
            e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            switch (e.PropertyName)
            {
                case "IdHorsForfait":
                    e.Column.Visibility = Visibility.Collapsed;
                    e.Column.IsReadOnly = true;
                    break;
                case "Date":
                    e.Column.IsReadOnly = true;
                    break;
                case "Nom":
                    e.Column.IsReadOnly = true;
                    break;
                case "Montant":
                    if (consultation) e.Column.IsReadOnly = true;
                    break;
                case "Etat":
                    e.Column.IsReadOnly = true;
                    break;
            }
        }

        private async void datagridHorsForfait_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Column.Header.ToString() == "Montant")
            {
                string input = (e.EditingElement as TextBox).Text;
                int montant;

                try { montant = int.Parse(input); }
                catch (Exception) { return; }

                if (montant < 0)
                {
                    (e.EditingElement as TextBox).Text = "0";
                    e.Cancel = true;
                    var erreur = new Windows.UI.Popups.MessageDialog("Le montant ne peut pas être négatif", "Erreur");
                    await erreur.ShowAsync();
                    return;
                }

                await Service.FraisServices.ModifierHorsForfait((datagridHorsForfait.SelectedItem as HorsForfait).IdHorsForfait, montant);
                await RefreshFiche();
                ChargerHorsForfait();
            }
        }

        #endregion

        private async void RetourConsultationBtn_Click(object sender, RoutedEventArgs e)
        {
            if (validation)
            {
                this.Frame.Navigate(typeof(Comptables.FichesValidation));
            } else
            {
                utilisateur = await Service.LoginService.RecupFicheFrais(utilisateur);
                this.Frame.Navigate(typeof(ListeFiches), utilisateur);
            }
        }

        private async void EffectActionBtn_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog contentDialog = new ContentDialog();
            contentDialog.Title = "Validation de la fiche";

            ActionComptable actionComptable = new ActionComptable();
            {
                contentDialog.PrimaryButtonText = "Valider";
                contentDialog.SecondaryButtonText = "Annuler";
                contentDialog.Content = actionComptable;
                foreach (EtatFiche etat in Enum.GetValues(typeof(EtatFiche)))
                {
                    if (etat == EtatFiche.COURS) continue;
                    actionComptable.ActionsComptables.Items.Add(Enum.GetName(typeof(EtatFiche), etat));
                }
                contentDialog.DefaultButton = ContentDialogButton.Primary;
            }

            contentDialog.PrimaryButtonClick += async (s, args) =>
            {
                if (actionComptable.ActionsComptables.SelectedIndex == -1)
                {
                    var erreur = new Windows.UI.Popups.MessageDialog("Veuillez sélectionner une action", "Erreur");
                    await erreur.ShowAsync();
                    return;
                }

                int etat = actionComptable.ActionsComptables.SelectedIndex == 0 ? 1 : actionComptable.ActionsComptables.SelectedIndex + 2;
                int etatFrais = actionComptable.ActionsComptables.SelectedIndex + 1;

                await ValidationService.ChangerEtatFiche(ficheFrais.IdFicheFrais, etat, etatFrais);
                this.Frame.Navigate(typeof(Comptables.FichesValidation));
            };

            await contentDialog.ShowAsync();
        }

        private void ExpPdfBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
