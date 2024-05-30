using AP1_WINUI.Data.Modeles;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AP1_WINUI.Service
{
    internal class LoginService
    {
        private static async Task<Utilisateur> UtilisateurBase(string username, string password)
        {
            await Data.SQL.Connect();
            Utilisateur user = null;

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

                    user = utilisateur;
                }
            }
            catch
            {
                var dialog = new Windows.UI.Popups.MessageDialog("Le nom d'utilisateur ou le mot de passe sont faux.", "Erreur lors de la connexion");
                await dialog.ShowAsync();
            }

            Data.SQL.Disconnect();
            return user;
        }

        private static async Task<Utilisateur> RecupFicheFrais(Utilisateur user)
        {
            await Data.SQL.Connect();
            string query = "SELECT * FROM fiche_de_frais WHERE utilisateur = @id_utilisateur";
            var cmd = new MySqlConnector.MySqlCommand(query, Data.SQL.Connection);
            cmd.Parameters.AddWithValue("@id_utilisateur", user.IdUtilisateur);

            try
            {
                var reader = await cmd.ExecuteReaderAsync();
                user.FicheFrais = new List<FicheFrais>();
                while (reader.Read())
                {
                    var fiche = new FicheFrais
                    {
                        IdFicheFrais = reader.GetInt32("id_fiche"),
                        Date = reader.GetDateTime("date_fiche"),
                        IdUtilisateur = reader.GetInt32("utilisateur"),
                        Etat = (EtatFiche)reader.GetInt32("etat")
                    };

                    user.FicheFrais.Add(fiche);
                }
            }
            catch
            {
                Data.SQL.Disconnect();
                var dialog = new Windows.UI.Popups.MessageDialog("Erreur lors de la récupération des fiches", "Erreur lors de la connexion");
                await dialog.ShowAsync();
                return null;
            }

            Data.SQL.Disconnect();
            return user;
        }

        public static async Task<Utilisateur> Login(string username, string password)
        {
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                Utilisateur user = null;

                user = await UtilisateurBase(username, password);
                if (user == null) { Data.SQL.Disconnect(); return null; }

                user = await RecupFicheFrais(user);
                if (user == null) { Data.SQL.Disconnect(); return null; }

                return user;
            }
            var dialog2 = new Windows.UI.Popups.MessageDialog("Le nom d'utilisateur et/ou le mot de passe est vide", "Erreur lors de la connexion");
            await dialog2.ShowAsync();
            return null;
        }
    }
}
