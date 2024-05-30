﻿using AP1_WINUI.Data.Modeles;
using Microsoft.Toolkit.Uwp.UI.Controls;
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

        #region FRAIS FORFAITS

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog();

            {
                dialog.Title = "Ajout d'un forfait";
                dialog.PrimaryButtonText = "Ajouter";
                dialog.SecondaryButtonText = "Annuler";
                Popups.AjoutForfait ajoutForfait = new Popups.AjoutForfait();
                ajoutForfait.comboBoxTypeFrais.ItemsSource = SourceComboBox;
                ajoutForfait.comboBoxTypeFrais.SelectedIndex = 0;
                dialog.Content = ajoutForfait;
                dialog.DefaultButton = ContentDialogButton.Primary;
            }

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                var dialog1 = new Windows.UI.Popups.MessageDialog("Forfait ajouté", "Succès");
                await dialog1.ShowAsync();
            }
            else
            {
                var dialog1 = new Windows.UI.Popups.MessageDialog("Rien n'a été fait", "Erreur");
                await dialog1.ShowAsync();
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

        #region COMBOBOX

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            comboBox.ItemsSource = SourceComboBox;
            comboBox.SetBinding(ComboBox.SelectedItemProperty, new Binding
            {
                Path = new PropertyPath("Nom"),
                Mode = BindingMode.TwoWay
            }); 
            
            comboBox.SelectionChanged += ComboBox_SelectionChanged;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            string selectedItem = (string)comboBox.SelectedItem;

            DataGridRow dataGridRow = FindParent<DataGridRow>(comboBox);

            int rowIndex = dataGridRow.GetIndex();

        }

        private static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null)
            {
                return null;
            }

            T parent = parentObject as T;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                return FindParent<T>(parentObject);
            }
        }

        #endregion

        #endregion
    }
}
