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
        ACCEPTE = 2,
        REFUSE = 3,
        REFUSPARTIEL = 4
    }

    public class FicheFrais
    {
        public int IdFicheFrais { get; set; }
        public DateTime Date { get; set; }
        public int IdUtilisateur { get; set; }
        public EtatFiche Etat { get; set; }
    }
}
