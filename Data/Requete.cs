using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP1_WINUI.Data
{
    public class Requete
    {
        public static async Task<MySqlDataReader> Retour(string query)
        {
            await SQL.Connect();
            var cmd = new MySqlCommand(query, SQL.Connection);
            
            try
            {
                var reader = await cmd.ExecuteReaderAsync();
                return reader;
            } catch (MySqlException)
            {
                return null;
            }
        }

        public static async Task<int> SansRetour(string query)
        {
            await SQL.Connect();
            var cmd = new MySqlCommand(query, SQL.Connection);

            try
            {
                var result = await cmd.ExecuteNonQueryAsync();
                return result;
            } catch (MySqlException)
            {
                return -1;
            }
        }
    }
}
