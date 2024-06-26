using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using AP1_WINUI.Data;
using AP1_WINUI.Data.Modeles;

namespace AP1_WINUI.Service
{
    internal class FraisServices
    {
        public static async Task<FicheFrais> RecupFicheFrais(int idUtilisateur, DateTime date)
        {
            FicheFrais ficheFrais = new FicheFrais();

            try
            {
                string Query = "SELECT * FROM fiche_de_frais WHERE utilisateur = @id_utilisateur AND date_fiche = @date";
                var reader = await Data.SQL.ExecuteQuery(Query, new Dictionary<string, object> { { "@id_utilisateur", idUtilisateur }, { "@date", date } });
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

            try
            {
                string Query = "INSERT INTO fiche_de_frais (date_fiche, utilisateur, etat) VALUES (@date, @utilisateur, @etat)";
                await Data.SQL.ExecuteNonQuery(Query, new Dictionary<string, object> { { "@date", date }, { "@utilisateur", userName }, { "@etat", EtatFiche.COURS } } );
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
            try
            {
                string Query = "SELECT * FROM type_frais";
                var reader = await Data.SQL.ExecuteQuery(Query);
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
            try
            {
                string Query = "INSERT INTO forfait (type_forfait, etat, date, fiche_frais, quantite) VALUES (@id_type_forfait, @etat, @date, @fiche_frais, @quantite)";
                await Data.SQL.ExecuteNonQuery(Query, new Dictionary<string, object> { { "@id_type_forfait", idTypeFrais }, { "@etat", EtatNote.ATTENTE }, { "@date", date }, { "@fiche_frais", idFiche }, { "@quantite", 0 } });
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

            string Query = "SELECT * FROM forfait WHERE fiche_frais = @id_fiche";

            try
            {
                var reader = await Data.SQL.ExecuteQuery(Query, new Dictionary<string, object> { { "@id_fiche", idFiche } });
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
            try
            {
                string Query = "DELETE FROM forfait WHERE id_forfait = @id_forfait";
                await Data.SQL.ExecuteNonQuery(Query, new Dictionary<string, object> { { "@id_forfait", idForfait } });
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
            try
            {
                string Query = "UPDATE forfait SET quantite = @quantite WHERE id_forfait = @id_forfait";
                await Data.SQL.ExecuteNonQuery(Query, new Dictionary<string, object> { { "@quantite", quantite }, { "@id_forfait", idForfait } });
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
            try
            {
                string Query = "INSERT INTO hors_forfait (nom, etat, date, montant, fiche_frais) VALUES (@nom, @etat, @date, @montant, @fiche_frais)";
                await Data.SQL.ExecuteNonQuery(Query, new Dictionary<string, object> { { "@nom", nom }, { "@etat", EtatNote.ATTENTE }, { "@date", date }, { "@montant", montant }, { "@fiche_frais", idFiche } });
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

        public static async Task<List<HorsForfait>> RecupHorsForfait(int idFiche)
        {
            List<HorsForfait> horsForfaits = new List<HorsForfait>();

            string Query = "SELECT * FROM hors_forfait WHERE fiche_frais = @id_fiche";

            try
            {
                var reader = await Data.SQL.ExecuteQuery(Query, new Dictionary<string, object> { { "@id_fiche", idFiche } });
                while (reader.Read())
                {
                    horsForfaits.Add(new HorsForfait
                    {
                        IdHorsForfait = reader.GetInt32("id_hors_forfait"),
                        Nom = reader.GetString("nom"),
                        Date = reader.GetDateTime("date").ToString(),
                        Etat = reader.GetString("etat"),
                        Montant = reader.GetDouble("montant")
                    });
                }
                Data.SQL.Disconnect();
            }
            catch (Exception e)
            {
                Data.SQL.Disconnect();

                var dialog = new Windows.UI.Popups.MessageDialog("Erreur lors de la récupération des hors forfaits " + e.Message, "Erreur");
                await dialog.ShowAsync();

                return null;
            }

            return horsForfaits;
        }

        public static async Task<bool> SupprimerHorsForfait(int idHorsForfait)
        {
            try
            {
                string Query = "DELETE FROM hors_forfait WHERE id_hors_forfait = @id_hors_forfait";
                await Data.SQL.ExecuteNonQuery(Query, new Dictionary<string, object> { { "@id_hors_forfait", idHorsForfait } });
                Data.SQL.Disconnect();
                return true;
            }
            catch (Exception e)
            {
                Data.SQL.Disconnect();

                var dialog = new Windows.UI.Popups.MessageDialog("Erreur lors de la suppression du hors forfait " + e.Message, "Erreur");
                await dialog.ShowAsync();
                return false;
            }
        }

        public static async Task<bool> ModifierHorsForfait(int idHorsForfait, int montant)
        {
            try
            {
                string Query = "UPDATE hors_forfait SET montant = @montant WHERE id_hors_forfait = @id_hors_forfait";
                await Data.SQL.ExecuteNonQuery(Query, new Dictionary<string, object> { { "@montant", montant }, { "@id_hors_forfait", idHorsForfait } });
                Data.SQL.Disconnect();
                return true;
            }
            catch (Exception e)
            {
                Data.SQL.Disconnect();

                var dialog = new Windows.UI.Popups.MessageDialog("Erreur lors de la modification du hors forfait " + e.Message, "Erreur");
                await dialog.ShowAsync();
                return false;
            }
        }

        #endregion
    }
}
