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
                var reader = await Data.SQL.ExecuteQuery(Query);

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

                Data.SQL.Disconnect();
                return utilisateurs;
            } 
            catch (Exception e)
            {
                Data.SQL.Disconnect();

                var dialog = new Windows.UI.Popups.MessageDialog("Erreur lors de la récupération de la fiche de frais " + e.Message, "Erreur");
                await dialog.ShowAsync();
                return null;
            }
        }

    }
}
