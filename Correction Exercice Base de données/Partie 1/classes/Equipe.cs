using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Partie_1.classes
{
    internal class Equipe
    {
        string nom, ville, logo;

        public Equipe(string nom, string ville, string logo)
        {
            this.nom = nom;
            this.ville = ville;
            this.logo = logo;
        }

        public string Nom { get => nom; set => nom = value; }
        public string Ville { get => ville; set => ville = value; }
        public string Logo { get => logo; set => logo = value; }
    }
}
