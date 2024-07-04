using AP1_WINUI.Data.Modeles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP1_WINUI.Service
{
    public class GestionUtilisateursService
    {
        public static async Task<List<Data.Modeles.Utilisateur>> RecupererTousUtilisateurs()
        {
            List<Data.Modeles.Utilisateur> utilisateurs = new List<Data.Modeles.Utilisateur>();

            string Query = "SELECT * FROM Utilisateur";

            try
            {
                var reader = await Data.SQL.ExecuteRequete(Query);

                while (reader.Read())
                {
                    Data.Modeles.Utilisateur utilisateur = new Data.Modeles.Utilisateur()
                    {
                        IdUtilisateur = reader.GetInt32("id_utilisateur"),
                        Username = reader.GetString("username"),
                        Password = reader.GetString("mdp"),
                        Role = (Data.Modeles.Role)reader.GetInt32("id_role"),
                        FicheFrais = null
                    };
                    utilisateurs.Add(utilisateur);
                };

                Data.SQL.Deconnect();
                return utilisateurs;
            } 
            catch (Exception e)
            {
                Data.SQL.Deconnect();

                var dialog = new Windows.UI.Popups.MessageDialog("Erreur lors de la récupération de la fiche de frais " + e.Message, "Erreur");
                await dialog.ShowAsync();
                return null;
            }
        }

        public static async Task AjouterUtilisateur(Utilisateur utilisateur)
        {
            string Query = "INSERT INTO Utilisateur (username, mdp, id_role) VALUES (@username,  @mdp, @id_role)";

            try
            { 
                await Data.SQL.ExecuteNonRequete(Query, new Dictionary<string, object> { { "@username", utilisateur.Username }, { "@mdp", utilisateur.Password }, { "@id_role", (int)utilisateur.Role } });
                Data.SQL.Deconnect();
            }
            catch (Exception e)
            {
                Data.SQL.Deconnect();

                var dialog = new Windows.UI.Popups.MessageDialog("Erreur lors de l'ajout de l'utilisateur " + e.Message, "Erreur");
                await dialog.ShowAsync();
            }
        }

        public static async Task SupprimerUtilisateur(Utilisateur utilisateur)
        {
            string Query = "DELETE FROM Utilisateur WHERE id_utilisateur = @id_utilisateur";

            try
            {
                await Data.SQL.ExecuteNonRequete(Query, new Dictionary<string, object> { { "@id_utilisateur", utilisateur.IdUtilisateur } });
                Data.SQL.Deconnect();
            }
            catch (Exception e)
            {
                Data.SQL.Deconnect();

                var dialog = new Windows.UI.Popups.MessageDialog("Erreur lors de la suppression de l'utilisateur " + e.Message, "Erreur");
                await dialog.ShowAsync();
            }
        }

        public static async Task ModifierUtilisateur(Utilisateur utilisateur)
        {
            string Query = "UPDATE Utilisateur SET username = @username, mdp = @mdp, id_role = @id_role WHERE id_utilisateur = @id_utilisateur";

            try
            {
                await Data.SQL.ExecuteNonRequete(Query, new Dictionary<string, object> { { "@username", utilisateur.Username }, { "@mdp", utilisateur.Password }, { "@id_role", (int)utilisateur.Role }, { "@id_utilisateur", utilisateur.IdUtilisateur } });
                Data.SQL.Deconnect();
            }
            catch (Exception e)
            {
                Data.SQL.Deconnect();

                var dialog = new Windows.UI.Popups.MessageDialog("Erreur lors de la modification de l'utilisateur " + e.Message, "Erreur");
                await dialog.ShowAsync();
            }
        }
    }
}
