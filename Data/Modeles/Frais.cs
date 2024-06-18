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

    public class TypeFrais
    {
        public int IdTypeFrais { get; set; }
        public string Nom { get; set; }
        public double Montant { get; set; }
    }

    public class Forfait
    {
        public int IdForfait { get; set; }
        private DateTime _date { get; set; }
        public string Date
        {
            get { return _date.ToShortDateString(); }
            set { _date = DateTime.Parse(value); }
        }
        public int IdTypeFrais { get; set; }
        public string Nom { get; set; }
        public double Montant { get; set; }
        public int Quantite { get; set; }
        public string Etat { get; set; }
    }

    public class HorsForfait
    {
        public int IdHorsForfait { get; set; }
        public string Nom { get; set; }
        private DateTime _date { get; set; }
        public string Date
        {
            get { return _date.ToShortDateString(); }
            set { _date = DateTime.Parse(value); }
        }
        public double Montant { get; set; }
        public string Etat { get; set; }
    }
}
