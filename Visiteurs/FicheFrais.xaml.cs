using AP1_WINUI.Data.Modeles;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
        bool consultation = true;

        List<Forfait> listForfait;

        public FicheFrais()
        {
            this.InitializeComponent();

            listForfait = new List<Forfait>();
            listForfait.Add(new Forfait() { Nom = "Nuitée" });
            listForfait.Add(new Forfait());
            listForfait.Add(new Forfait());
            datagridForfait.ItemsSource = listForfait.ToList();

        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            tf = await Service.FraisServices.RecupTypeFrais();

            foreach (TypeFrais t in tf)
            {
                SourceComboBox.Add(t.Nom);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ficheFrais = e.Parameter as Data.Modeles.FicheFrais;
        }

        #region FRAIS FORFAITS

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog();

            Popups.AjoutForfait ajoutForfait = new Popups.AjoutForfait();
            {
                dialog.Title = "Ajout d'un forfait";
                dialog.PrimaryButtonText = "Ajouter";
                dialog.SecondaryButtonText = "Annuler";
                ajoutForfait.comboBoxTypeFrais.ItemsSource = SourceComboBox;
                ajoutForfait.comboBoxTypeFrais.SelectedIndex = 0;
                dialog.Content = ajoutForfait;
                dialog.DefaultButton = ContentDialogButton.Primary;
            }

            var result = await dialog.ShowAsync();
            
            if (result == ContentDialogResult.Primary)
            {
                DateTime date = ajoutForfait.datePicker.Date.Date;
                int selection = ajoutForfait.comboBoxTypeFrais.SelectedIndex + 1;
            }
            else
            {
            }
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
