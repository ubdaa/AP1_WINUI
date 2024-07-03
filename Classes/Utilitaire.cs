using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AP1_WINUI
{
    public class Utilitaire : Page
    {
        public static void SeDeconnecter()
        {
            var frame = Window.Current.Content as Frame;

            if (frame != null)
            {
                frame.Navigate(typeof(Login), null);
            }
        }

        public static string CheminApp()
        {
            return Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads";
        }
    }
}
