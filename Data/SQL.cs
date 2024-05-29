using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using MySqlConnector;
using Windows.UI.Xaml;


namespace AP1_WINUI.Data
{
    internal class SQL
    {
        public static MySqlConnection Connection;
        private static string ConnectionString = "server=127.0.0.1;uid=root;pwd=;database=ap1_gsb";

        public static async void Connect()
        {
            string message = "";
            try 
            {
                Connection = new MySqlConnection(ConnectionString);
                await Connection.OpenAsync();
                return;
            } 
            catch (MySqlException e)
            {
                message = "Erreur de connexion à la base de données : " + e.Message;
                var dialog2 = new MessageDialog(message);
                await dialog2.ShowAsync();
            }

        }

        public static void Disconnect()
        {
            Connection.Close();
        }
    }
}
