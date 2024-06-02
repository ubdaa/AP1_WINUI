using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using AP1_WINUI.Data.Modeles;

namespace AP1_WINUI.Service
{
    internal class FraisServices
    {
        public static async Task<FicheFrais> RecupFicheFrais(int idUtilisateur, DateTime date)
        {
            FicheFrais ficheFrais = new FicheFrais();

            await Data.SQL.Connect();

            string Query = "SELECT * FROM fiche_de_frais WHERE utilisateur = @id_utilisateur AND date_fiche = @date";
            var cmd = new MySqlConnector.MySqlCommand(Query, Data.SQL.Connection);
            cmd.Parameters.AddWithValue("@id_utilisateur", idUtilisateur);
            cmd.Parameters.AddWithValue("@date", date);

            try
            {
                var reader = await cmd.ExecuteReaderAsync();
                if (reader.Read())
                {
                    ficheFrais.IdFicheFrais = reader.GetInt32("id_fiche");
                    ficheFrais.Date = reader.GetDateTime("date_fiche");
                    ficheFrais.IdUtilisateur = reader.GetInt32("utilisateur");
                    ficheFrais.Etat = (EtatFiche)reader.GetInt32("etat");
                    ficheFrais.Forfaits = await RecupForfait(ficheFrais.IdFicheFrais);
                }
                Data.SQL.Disconnect();
            }
            catch (Exception e)
            {
                Data.SQL.Disconnect();

                var dialog = new Windows.UI.Popups.MessageDialog("Erreur lors de la récupération de la fiche de frais " + e.Message, "Erreur");
                await dialog.ShowAsync();

                return null;
            }

            return ficheFrais;
        }

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

            fiches = await RecupFicheFrais(userName, date);

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


        #region FORFAIT 

        public static async Task<bool> AjoutForfait(int idTypeFrais, DateTime date, int idFiche)
        {
            await Data.SQL.Connect();
            string Query = "INSERT INTO forfait (type_forfait, etat, date, fiche_frais, quantite) VALUES (@id_type_forfait, @etat, @date, @fiche_frais, @quantite)";
            var cmd = new MySqlConnector.MySqlCommand(Query, Data.SQL.Connection);
            cmd.Parameters.AddWithValue("@id_type_forfait", idTypeFrais);
            cmd.Parameters.AddWithValue("@etat", EtatNote.ATTENTE);
            cmd.Parameters.AddWithValue("@date", date);
            cmd.Parameters.AddWithValue("@fiche_frais", idFiche);
            cmd.Parameters.AddWithValue("@quantite", 0);

            try
            {
                await cmd.ExecuteNonQueryAsync();
                Data.SQL.Disconnect();
                return true;
            }
            catch (Exception e)
            {
                Data.SQL.Disconnect();

                var dialog = new Windows.UI.Popups.MessageDialog("Erreur lors de l'ajout du forfait " + e.Message, "Erreur");
                await dialog.ShowAsync();
                return false;
            }

        }

        public static async Task<List<Forfait>> RecupForfait(int idFiche)
        {
            List<Forfait> forfaits = new List<Forfait>();
            List<TypeFrais> typeFrais = await RecupTypeFrais();

            await Data.SQL.Connect();
            string Query = "SELECT * FROM forfait WHERE fiche_frais = @id_fiche";
            var cmd = new MySqlConnector.MySqlCommand(Query, Data.SQL.Connection);
            cmd.Parameters.AddWithValue("@id_fiche", idFiche);

            try
            {
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    forfaits.Add(new Forfait
                    {
                        IdForfait = reader.GetInt32("id_forfait"),
                        IdTypeFrais = reader.GetInt32("type_forfait"),
                        Date = reader.GetDateTime("date").ToString(),
                        Etat = reader.GetString("etat"),
                        Quantite = reader.GetInt32("quantite"),
                        Nom = typeFrais.Find(x => x.IdTypeFrais == reader.GetInt32("type_forfait")).Nom,
                        Montant = typeFrais.Find(x => x.IdTypeFrais == reader.GetInt32("type_forfait")).Montant,
                    });
                }
                Data.SQL.Disconnect();
            }
            catch (Exception e)
            {
                Data.SQL.Disconnect();

                var dialog = new Windows.UI.Popups.MessageDialog("Erreur lors de la récupération des forfaits " + e.Message, "Erreur");
                await dialog.ShowAsync();

                return null;
            }

            return forfaits;
        }

        public static async Task<bool> SupprimerForfait(int idForfait)
        {
            await Data.SQL.Connect();
            string Query = "DELETE FROM forfait WHERE id_forfait = @id_forfait";
            var cmd = new MySqlConnector.MySqlCommand(Query, Data.SQL.Connection);
            cmd.Parameters.AddWithValue("@id_forfait", idForfait);

            try
            {
                await cmd.ExecuteNonQueryAsync();
                Data.SQL.Disconnect();
                return true;
            }
            catch (Exception e)
            {
                Data.SQL.Disconnect();

                var dialog = new Windows.UI.Popups.MessageDialog("Erreur lors de la suppression du forfait " + e.Message, "Erreur");
                await dialog.ShowAsync();
                return false;
            }
        }

        public static async Task<bool> ModifierForfait(int idForfait, int quantite)
        {
            await Data.SQL.Connect();
            string Query = "UPDATE forfait SET quantite = @quantite WHERE id_forfait = @id_forfait";
            var cmd = new MySqlConnector.MySqlCommand(Query, Data.SQL.Connection);
            cmd.Parameters.AddWithValue("@quantite", quantite);
            cmd.Parameters.AddWithValue("@id_forfait", idForfait);

            try
            {
                await cmd.ExecuteNonQueryAsync();
                Data.SQL.Disconnect();
                return true;
            }
            catch (Exception e)
            {
                Data.SQL.Disconnect();

                var dialog = new Windows.UI.Popups.MessageDialog("Erreur lors de la modification du forfait " + e.Message, "Erreur");
                await dialog.ShowAsync();
                return false;
            }
        }

        #endregion

        #region HORS FORFAIT

        public static async Task<bool> AjoutHorsForfait(string nom, DateTime date, double montant, int idFiche)
        {
            await Data.SQL.Connect();
            string Query = "INSERT INTO hors_forfait (nom, etat, date, montant, fiche_frais) VALUES (@nom, @etat, @date, @montant, @fiche_frais)";
            var cmd = new MySqlConnector.MySqlCommand(Query, Data.SQL.Connection);
            cmd.Parameters.AddWithValue("@nom", nom);
            cmd.Parameters.AddWithValue("@etat", EtatNote.ATTENTE);
            cmd.Parameters.AddWithValue("@date", date);
            cmd.Parameters.AddWithValue("@montant", montant);
            cmd.Parameters.AddWithValue("@fiche_frais", idFiche);

            try
            {
                await cmd.ExecuteNonQueryAsync();
                Data.SQL.Disconnect();
                
                return true;
            }
            catch (Exception e)
            {
                Data.SQL.Disconnect();

                var dialog = new Windows.UI.Popups.MessageDialog("Erreur lors de l'ajout du hors forfait " + e.Message, "Erreur");
                await dialog.ShowAsync();

                return false;
            }
        }

        #endregion
    }
}
