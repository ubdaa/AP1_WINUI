using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP1_WINUI.Data.Modeles
{
    public enum EtatFiche
    {
        ATTENTE = 1,
        COURS = 2,
        ACCEPTE = 3,
        REFUSE = 4,
        REFUS_PARTIEL = 5
    }

    public class FicheFrais
    {
        public int IdFicheFrais { get; set; }
        public DateTime Date { get; set; }
        public int IdUtilisateur { get; set; }
        public EtatFiche Etat { get; set; }
        public List<Forfait> Forfaits { get; set; }
        public List<HorsForfait> HorsForfaits { get; set; }
    }
}
