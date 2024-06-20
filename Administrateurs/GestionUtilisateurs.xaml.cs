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

        private async void AjoutUtilisateur_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog();

            Popups.AjoutUtilisateur ajoutUtilisateur = new Popups.AjoutUtilisateur();
            {
                dialog.Title = "Ajouter un utilisateur";
                dialog.PrimaryButtonText = "Ajouter";
                dialog.SecondaryButtonText = "Annuler";
                ajoutUtilisateur.comboBoxRole.ItemsSource = Enum.GetValues(typeof(Data.Modeles.Role));
                dialog.Content = ajoutUtilisateur;
                dialog.DefaultButton = ContentDialogButton.Primary;
            }

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                if (string.IsNullOrEmpty(ajoutUtilisateur.textBoxPassword.Text) ||
                    string.IsNullOrEmpty(ajoutUtilisateur.textBoxUsername.Text) ||
                    ajoutUtilisateur.comboBoxRole.SelectedItem == null)
                {
                    ContentDialog dialogErreur = new ContentDialog
                    {
                        Title = "Erreur",
                        Content = "Veuillez remplir tous les champs",
                        CloseButtonText = "Ok"
                    };

                    await dialogErreur.ShowAsync();
                    return;
                }

                Data.Modeles.Utilisateur utilisateur = new Data.Modeles.Utilisateur
                {
                    Username = ajoutUtilisateur.textBoxUsername.Text,
                    Password = ajoutUtilisateur.textBoxPassword.Text,
                    Role = (Data.Modeles.Role)ajoutUtilisateur.comboBoxRole.SelectedItem
                };

                await Service.GestionUtilisateursService.AjouterUtilisateur(utilisateur);
                users = await Service.GestionUtilisateursService.RecupererTousUtilisateurs();
                datagridUtilisateurs.ItemsSource = users.ToList();
            }
        }

        private async void SuppressionUtilisateur_Click(object sender, RoutedEventArgs e)
        {
            if (datagridUtilisateurs.SelectedItem == null)
            {
                ContentDialog dialogErreur = new ContentDialog
                {
                    Title = "Erreur",
                    Content = "Veuillez sélectionner un utilisateur",
                    CloseButtonText = "Ok"
                };

                await dialogErreur.ShowAsync();
                return;
            }

            Data.Modeles.Utilisateur utilisateur = (Data.Modeles.Utilisateur)datagridUtilisateurs.SelectedItem;

            ContentDialog dialog = new ContentDialog
            {
                Title = "Supprimer l'utilisateur",
                Content = "Voulez-vous vraiment supprimer l'utilisateur " + utilisateur.Username + " ?",
                PrimaryButtonText = "Oui",
                SecondaryButtonText = "Non"
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                await Service.GestionUtilisateursService.SupprimerUtilisateur(utilisateur);
                users = await Service.GestionUtilisateursService.RecupererTousUtilisateurs();
                datagridUtilisateurs.ItemsSource = users.ToList();
            }
        }

        private async void ModifierUtilisateur_Click(object sender, RoutedEventArgs e)
        {
            if (datagridUtilisateurs.SelectedItem == null)
            {
                ContentDialog dialogErreur = new ContentDialog
                {
                    Title = "Erreur",
                    Content = "Veuillez sélectionner un utilisateur",
                    CloseButtonText = "Ok"
                };

                await dialogErreur.ShowAsync();
                return;
            }

            Data.Modeles.Utilisateur utilisateur = (Data.Modeles.Utilisateur)datagridUtilisateurs.SelectedItem;

            ContentDialog dialog = new ContentDialog();

            Popups.AjoutUtilisateur modifierUtilisateur = new Popups.AjoutUtilisateur();
            {
                dialog.Title = "Modifier un utilisateur";
                dialog.PrimaryButtonText = "Modifier";
                dialog.SecondaryButtonText = "Annuler";
                modifierUtilisateur.comboBoxRole.ItemsSource = Enum.GetValues(typeof(Data.Modeles.Role));
                modifierUtilisateur.Username.Visibility = Visibility.Collapsed;
                modifierUtilisateur.Password.Visibility = Visibility.Collapsed;
                modifierUtilisateur.comboBoxRole.SelectedIndex = (int)utilisateur.Role - 1;
                dialog.Content = modifierUtilisateur;
                dialog.DefaultButton = ContentDialogButton.Primary;
            }

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                if (modifierUtilisateur.comboBoxRole.SelectedItem == null)
                {
                    ContentDialog dialogErreur = new ContentDialog
                    {
                        Title = "Erreur",
                        Content = "Veuillez choisir quelque chose tous les champs",
                        CloseButtonText = "Ok"
                    };

                    await dialogErreur.ShowAsync();
                    return;
                }

                utilisateur.Role = (Data.Modeles.Role)modifierUtilisateur.comboBoxRole.SelectedItem;

                await Service.GestionUtilisateursService.ModifierUtilisateur(utilisateur);
                users = await Service.GestionUtilisateursService.RecupererTousUtilisateurs();
                datagridUtilisateurs.ItemsSource = users.ToList();
            }
        }

        private async void datagridUtilisateurs_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            Data.Modeles.Utilisateur utilisateur = (Data.Modeles.Utilisateur)datagridUtilisateurs.SelectedItem;

            if (e.EditAction == DataGridEditAction.Commit)
            {
                TextBox textBox = e.EditingElement as TextBox;

                if (!string.IsNullOrEmpty(textBox.Text))
                {
                    switch (e.Column.Header)
                    {
                        case "Username":
                            utilisateur.Username = textBox.Text;
                            break;

                        case "Password":
                            utilisateur.Password = textBox.Text;
                            break;
                    }

                    await Service.GestionUtilisateursService.ModifierUtilisateur(utilisateur);
                    return;
                } 
                else
                {
                    ContentDialog dialogErreur = new ContentDialog
                    {
                        Title = "Erreur",
                        Content = "Erreur lors de la saisie veuillez recommencer",
                        CloseButtonText = "Ok"
                    };

                    await dialogErreur.ShowAsync();
                    e.Cancel = true;
                    users = await Service.GestionUtilisateursService.RecupererTousUtilisateurs();
                    datagridUtilisateurs.ItemsSource = users.ToList();

                    return;
                }
            }
        }
    }
}
