using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Partie_1.classes
{
    internal class Joueur
    {
        
        public string Matricule {  get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public DateTimeOffset DateNaissance { get; set; }
        public string NomEquipe {  get; set; }

        public string LogoEquipe
        {
            get
            {
                if (NomEquipe != "") //si le joueur est dans une équipe, on va chercher le logo de son équipe
                    return SingletonBD.getInstance().getlogoEquipes(NomEquipe);
                else //sinon on met une image par défaut. celle-ci se trouve dans le dossier assets
                    return "ms-appx:///Assets/StoreLogo.png";
            }
        }

        public string NomPrenom { get{ return Prenom + " " + Nom; } }
        public string DateNaissanceString { get { return DateNaissance.ToString("dd MMMM yyyy"); } }
    }
}
