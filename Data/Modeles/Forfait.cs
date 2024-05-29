using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP1_WINUI.Data.Modeles
{
    public enum EtatNote
    {
        ATTENTE = 1,
        ACCEPTE = 2,
        REFUSE = 3
    }

    public class Forfait
    {
        public int IdForfait { get; set; }
        public DateTime Date { get; set; }
        public int IdTypeFrais { get; set; }
        public string Nom { get; set; }
        public double Montant { get; set; }
        public int Quantite { get; set; }
        public EtatNote Etat { get; set; }
    }
}
