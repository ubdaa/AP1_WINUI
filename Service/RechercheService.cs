using AP1_WINUI.Data.Modeles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP1_WINUI.Service
{
    internal class RechercheService
    {
        public static async Task<List<Data.Modeles.Utilisateur>> RechercherUtilisateur(string recherche)
        {
            List<Data.Modeles.Utilisateur> utilisateurs = new List<Data.Modeles.Utilisateur>();
            string Query = "SELECT * FROM utilisateur WHERE username = @username";

            try
            {
                var reader = await Data.SQL.ExecuteRequete(Query, new Dictionary<string, object> { { "@username", recherche } });

                while (reader.Read())
                {
                    Data.Modeles.Utilisateur utilisateur = new Data.Modeles.Utilisateur
                    {
                        IdUtilisateur = reader.GetInt32("id_utilisateur"),
                        Username = reader.GetString("username"),
                        Password = reader.GetString("mdp"),
                        Role = (Role)reader.GetInt32("id_role"),
                    };

                    utilisateur = await LoginService.RecupFicheFrais(utilisateur);

                    utilisateurs.Add(utilisateur);
                }

                Data.SQL.Deconnect();
                return utilisateurs;
            } 
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Data.SQL.Deconnect();
                return null;
            }
        }

    }
}
