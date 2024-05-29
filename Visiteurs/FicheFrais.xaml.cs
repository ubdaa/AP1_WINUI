using AP1_WINUI.Data.Modeles;
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
    public sealed partial class FicheFrais : Page
    {
        List<Forfait> listForfait;

        public FicheFrais()
        {
            this.InitializeComponent();

            listForfait = new List<Forfait>();
            datagridForfait.ItemsSource = listForfait.ToList();


        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            listForfait.Add(new Forfait());
            datagridForfait.ItemsSource = listForfait.ToList();
        }

        private void datagridForfait_AutoGeneratingColumn(object sender, Microsoft.Toolkit.Uwp.UI.Controls.DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "Nom")
            {
                List<TypeFrais> tf = Service.FraisServices.RecupTypeFrais();
                List<string> SourceComboBox = new List<string>();

                foreach (TypeFrais t in tf)
                {
                    SourceComboBox.Add(t.Nom);
                }

                var comboColumn = new DataGridComboBoxColumn
                {
                    Header = e.PropertyName,
                    ItemsSource = SourceComboBox
                };

                e.Column = comboColumn;
            }
        }
    }
}
