using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using MySqlConnector;
using Windows.UI.Xaml;
using System.Collections;


namespace AP1_WINUI.Data
{
    internal class SQL
    {
        public static MySqlConnection Connection;
        private static string ConnectionString = "server=127.0.0.1;uid=root;pwd=;database=ap1_gsb";

        public static async Task Connect()
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

        public static async Task<MySqlDataReader> ExecuteQuery(string query, Dictionary<string, object> parameters = null)
        {
            await Connect();
            MySqlCommand cmd = new MySqlCommand(query, Connection);

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value);
                }
            }

            MySqlDataReader reader = await cmd.ExecuteReaderAsync();
            return reader;
        }

        public static async Task<int> ExecuteNonQuery(string query, Dictionary<string, object> parameters = null)
        {
            await Connect();

            MySqlCommand cmd = new MySqlCommand(query, Connection);

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value);
                }
            }
            return await cmd.ExecuteNonQueryAsync();
        }

    }
}
