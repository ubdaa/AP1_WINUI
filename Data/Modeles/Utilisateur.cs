using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP1_WINUI.Data.Modeles
{
    public enum Role
    {
        VISITEUR = 1,
        COMPTABLE = 2,
        ADMIN = 3
    }

    public class Utilisateur
    {
        public int IdUtilisateur { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }
}
