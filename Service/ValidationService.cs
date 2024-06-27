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

            try
            {
                string Query = "SELECT * FROM fiche_de_frais WHERE etat = @etat";
                var reader = await Data.SQL.ExecuteQuery(Query, new Dictionary<string, object> { { "@etat", 1 } });

                while (reader.Read())
                {
                    Data.Modeles.FicheFrais ficheFrais = new Data.Modeles.FicheFrais
                    {
                        IdFicheFrais = reader.GetInt32("id_fiche"),
                        Date = reader.GetDateTime("date_fiche"),
                        IdUtilisateur = reader.GetInt32("utilisateur"),
                        Etat = (Data.Modeles.EtatFiche)reader.GetInt32("etat"),
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

        public static async Task<bool> ChangerEtatFiche(int idFiche, int etat, int etatFrais)
        {
            try
            {
                string Query = "UPDATE fiche_de_frais SET etat = @etat WHERE id_fiche = @id_fiche";
                await Data.SQL.ExecuteNonQuery(Query, new Dictionary<string, object> { { "@etat", etat }, { "@id_fiche", idFiche } });

                Query = "UPDATE forfait SET etat = @etat WHERE fiche_frais = @id_fiche";
                await Data.SQL.ExecuteNonQuery(Query, new Dictionary<string, object> { { "@etat", etatFrais }, { "@id_fiche", idFiche } });

                Query = "UPDATE hors_forfait SET etat = @etat WHERE fiche_frais = @id_fiche";
                await Data.SQL.ExecuteNonQuery(Query, new Dictionary<string, object> { { "@etat", etatFrais }, { "@id_fiche", idFiche } });

                Data.SQL.Disconnect();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Data.SQL.Disconnect();
                return false;
            }
        }
    }
}
