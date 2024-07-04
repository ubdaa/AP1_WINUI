using AP1_WINUI.Data.Modeles;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace AP1_WINUI.Service
{
    internal class LoginService
    {
        private static async Task<Utilisateur> UtilisateurBase(string username, string password)
        {
            Utilisateur user = null;

            try
            {
                string query = "SELECT * FROM utilisateur WHERE username = @username AND mdp = @password";
                var reader = await Data.SQL.ExecuteRequete(query, new Dictionary<string, object> { { "@username", username }, { "@password", password } });

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
                else
                {
                    throw new Exception(); 
                }
            }
            catch
            {
                var dialog = new Windows.UI.Popups.MessageDialog("Le nom d'utilisateur ou le mot de passe sont faux.", "Erreur lors de la connexion");
                await dialog.ShowAsync();
            }

            Data.SQL.Deconnect();
            return user;
        }

        public static async Task<Utilisateur> UtilisateurBase(int idUtilisateur)
        {
            Utilisateur user = null;
            try
            {
                string query = "SELECT * FROM utilisateur WHERE id_utilisateur = @id";
                var reader = await Data.SQL.ExecuteRequete(query, new Dictionary<string, object> { { "@id", idUtilisateur } });
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
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                var dialog = new Windows.UI.Popups.MessageDialog("Le nom d'utilisateur ou le mot de passe sont faux.", "Erreur lors de la connexion");
                await dialog.ShowAsync();
            }

            Data.SQL.Deconnect();
            return user;
        }


        public static async Task<Utilisateur> RecupFicheFrais(Utilisateur user)
        {
            try
            {
                string query = "SELECT * FROM fiche_de_frais WHERE utilisateur = @id_utilisateur";
                var reader = await Data.SQL.ExecuteRequete(query, new Dictionary<string, object> { { "@id_utilisateur", user.IdUtilisateur } });
                user.FicheFrais = new List<FicheFrais>();
                while (reader.Read())
                {
                    var fiche = new FicheFrais
                    {
                        IdFicheFrais = reader.GetInt32("id_fiche"),
                        Date = reader.GetDateTime("date_fiche"),
                        IdUtilisateur = reader.GetInt32("utilisateur"),
                        Etat = (EtatFiche)reader.GetInt32("etat"),
                        Forfaits = await FraisServices.RecupForfait(reader.GetInt32("id_fiche"))
                    };

                    user.FicheFrais.Add(fiche);
                } 
                Data.SQL.Deconnect();
                return user;
            }
            catch
            {
                Data.SQL.Deconnect();
                var dialog = new Windows.UI.Popups.MessageDialog("Erreur lors de la récupération des fiches", "Erreur lors de la connexion");
                await dialog.ShowAsync();
                return null;
            }
        }

        public static async Task<Utilisateur> Login(string username, string password)
        {
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                Utilisateur user = null;

                user = await UtilisateurBase(username, password);
                if (user == null) { Data.SQL.Deconnect(); return null; }

                user = await RecupFicheFrais(user);
                if (user == null) { Data.SQL.Deconnect(); return null; }

                return user;
            }
            var dialog2 = new Windows.UI.Popups.MessageDialog("Le nom d'utilisateur et/ou le mot de passe est vide", "Erreur lors de la connexion");
            await dialog2.ShowAsync();
            return null;
        }

        public static async Task<string> NomUtilisateur(int idUtilisateur)
        {
            string nom = "";
            try
            {
                string query = "SELECT username FROM utilisateur WHERE id_utilisateur = @id";
                var reader = await Data.SQL.ExecuteRequete(query, new Dictionary<string, object> { { "@id", idUtilisateur } });
                if (reader.Read())
                {
                    nom = reader.GetString("username");
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                var dialog = new Windows.UI.Popups.MessageDialog("Erreur lors de la récupération du nom de l'utilisateur", "Erreur lors de la connexion");
                await dialog.ShowAsync();
            }

            Data.SQL.Deconnect();
            return nom;
        }
    }
}
