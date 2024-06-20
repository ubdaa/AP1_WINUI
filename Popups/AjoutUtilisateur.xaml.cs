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

namespace AP1_WINUI.Popups
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class AjoutUtilisateur : Page
    {
        public TextBox textBoxUsername
        {
            get { return usernameTextBox; }
            set { usernameTextBox = value; }
        }

        public TextBox textBoxPassword
        {
            get { return passwordTextBox; }
            set { passwordTextBox = value; }
        }

        public ComboBox comboBoxRole
        {
            get { return roleComboBox; }
            set { roleComboBox = value; }
        }

        public AjoutUtilisateur()
        {
            this.InitializeComponent();
        }
    }
}
