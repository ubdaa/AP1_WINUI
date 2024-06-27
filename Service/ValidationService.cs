using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP1_WINUI.Service
{
    public class ValidationService
    {
        public static async Task<List<Data.Modeles.FicheFrais>> RecupFichesFraisAValider()
        {
            List<Data.Modeles.FicheFrais> fichesFrais = new List<Data.Modeles.FicheFrais>();

            string Query = "SELECT * FROM FicheFrais WHERE Etat = 'ATTENTE'";

            try
            {
                var reader = await Data.SQL.ExecuteQuery(Query);

                while (reader.Read())
                {
                    Data.Modeles.FicheFrais ficheFrais = new Data.Modeles.FicheFrais
                    {
                        IdFicheFrais = reader.GetInt32("id_fiche"),
                        Date = reader.GetDateTime("date_fiche"),
                        IdUtilisateur = reader.GetInt32("id_utilisateur"),
                        Etat = (Data.Modeles.EtatFiche)Enum.Parse(typeof(Data.Modeles.EtatFiche), reader.GetString("etat")),
                        Forfaits = null,
                        HorsForfaits = null
                    };

                    fichesFrais.Add(ficheFrais);
                }

                Data.SQL.Disconnect();

            } 
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Data.SQL.Disconnect();
            }

            return fichesFrais;
        }
    }
}
