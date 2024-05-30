using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AP1_WINUI.Data.Modeles;

namespace AP1_WINUI.Service
{
    internal class FraisServices
    {
        public static async Task<FicheFrais> CreationFicheFrais(int userName, DateTime date)
        {
            FicheFrais fiches = new FicheFrais();

            await Data.SQL.Connect();
            string Query = "INSERT INTO fiche_de_frais (date_fiche, utilisateur, etat) VALUES (@date, @utilisateur, @etat)";
            var cmd = new MySqlConnector.MySqlCommand(Query, Data.SQL.Connection);
            cmd.Parameters.AddWithValue("@date", date);
            cmd.Parameters.AddWithValue("@utilisateur", userName);
            cmd.Parameters.AddWithValue("@etat", EtatFiche.ATTENTE);

            try
            {
                await cmd.ExecuteNonQueryAsync();
                Data.SQL.Disconnect();
            }
            catch (Exception e)
            {
                Data.SQL.Disconnect();

                var dialog = new Windows.UI.Popups.MessageDialog("Erreur lors de la création de la fiche de frais " + e.Message, "Erreur");
                await dialog.ShowAsync();

                return null;
            }

            return fiches;
        }

        public static async Task<List<TypeFrais>> RecupTypeFrais()
        {
            List<TypeFrais> typeFrais = new List<TypeFrais>();

            await Data.SQL.Connect();
            string Query = "SELECT * FROM type_frais";
            var cmd = new MySqlConnector.MySqlCommand(Query, Data.SQL.Connection);

            try
            {
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    typeFrais.Add(new TypeFrais
                    {
                        IdTypeFrais = reader.GetInt32("id_type_forfait"),
                        Nom = reader.GetString("nom"),
                        Montant = reader.GetDouble("cout")
                    });
                }
                Data.SQL.Disconnect();
            }
            catch (Exception e)
            {
                Data.SQL.Disconnect();

                var dialog = new Windows.UI.Popups.MessageDialog("Erreur lors de la récupération des types de frais " + e.Message, "Erreur");
                await dialog.ShowAsync();

                return null;
            }

            return typeFrais;
        }

        public static void AddForfaitBDD(Forfait forfait)
        {
            // Ajout du forfait dans la base de données

        }
    }
}
