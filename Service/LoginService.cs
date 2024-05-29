using AP1_WINUI.Data.Modeles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP1_WINUI.Service
{
    internal class LoginService
    {
        public static async Task<Utilisateur> Login(string username, string password)
        {
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                Data.SQL.Connect();

                string query = "SELECT * FROM utilisateur WHERE username = @username AND mdp = @password";
                var cmd = new MySqlConnector.MySqlCommand(query, Data.SQL.Connection);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);

                try
                {
                    var reader = await cmd.ExecuteReaderAsync();
                    if (reader.Read())
                    {
                        var utilisateur = new Utilisateur
                        {
                            IdUtilisateur = reader.GetInt32("id_utilisateur"),
                            Username = reader.GetString("username"),
                            Password = reader.GetString("mdp"),
                            Role = (Role)reader.GetInt32("id_role")
                        };
                        Data.SQL.Disconnect();
                        return utilisateur;
                    }
                }
                catch
                {
                    Data.SQL.Disconnect();
                }

                var dialog = new Windows.UI.Popups.MessageDialog("Le nom d'utilisateur ou le mot de passe sont faux", "Erreur lors de la connexion");
                await dialog.ShowAsync();
                return null;
            }
            var dialog2 = new Windows.UI.Popups.MessageDialog("Le nom d'utilisateur et/ou le mot de passe est vide", "Erreur lors de la connexion");
            await dialog2.ShowAsync();
            return null;
        }
    }
}
