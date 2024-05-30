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

// Pour plus d'informations sur le modèle d'élément Boîte de dialogue de contenu, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace AP1_WINUI.Popups
{
    public sealed partial class AjoutForfait : Page
    {
        public ComboBox comboBoxTypeFrais 
        { 
            get { return typeFrais; } 
            set { typeFrais = value; }
        }

        public DatePicker datePicker
        {
            get { return dateFrais; } 
            set { dateFrais = value; }
        }

        public AjoutForfait()
        {
            this.InitializeComponent();
        }
    }
}
