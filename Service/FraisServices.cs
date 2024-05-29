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
        public static List<TypeFrais> RecupTypeFrais()
        {
            List<TypeFrais> typeFrais = new List<TypeFrais>();

            Data.SQL.Connect();
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
            catch
            {
                Data.SQL.Disconnect();
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
