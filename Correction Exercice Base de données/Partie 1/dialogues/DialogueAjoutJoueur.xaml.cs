using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Partie_1.classes;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Partie_1.dialogues
{
    public sealed partial class DialogueAjoutJoueur : ContentDialog
    {
        bool fermerDialogue = true;
        public DialogueAjoutJoueur()
        {
            this.InitializeComponent();
            calendar_dateNaissance.MinDate = DateTime.Now.AddYears(-40);
            calendar_dateNaissance.MaxDate = DateTime.Now.AddYears(-16);
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            //validation du nom
            if (string.IsNullOrWhiteSpace(tbx_nom.Text))
            {
                erreur_nom.Text = "Veuillez entre le nom de l'équipe";
                fermerDialogue = false;
            }
            else if (tbx_nom.Text.Length <= 3 || tbx_nom.Text.Length > 50)
            {
                erreur_nom.Text = "Nom entre 3 et 50 caractères";
                fermerDialogue = false;
            }
            else
            {
                fermerDialogue = true;
            }

            //validation du prenom
            if (string.IsNullOrWhiteSpace(tbx_prenom.Text))
            {
                erreur_prenom.Text = "Veuillez entre le prenom du joueur";
                fermerDialogue = false;
            }
            else if (tbx_prenom.Text.Length <= 3 || tbx_prenom.Text.Length > 50)
            {
                erreur_prenom.Text = "prenom entre 3 et 50 caractères";
                fermerDialogue = false;
            }
            else
            {
                fermerDialogue = true;
            }

            string valeur = calendar_dateNaissance.Date.ToString();

            //validation de la date de naissance
            if (calendar_dateNaissance.Date == null)
            {
                erreur_dateNaissance.Text = "Veuillez choisir une date de naissance";
                fermerDialogue = false;
            }
            else
            {
                fermerDialogue = true;
            }

            //si tout est valide, on insère dans la base de données
            if (fermerDialogue)
            {
                SingletonBD.getInstance().ajouterJoueur(new Joueur()
                {
                    Matricule = genererMatricule(),
                    Nom = tbx_nom.Text,
                    Prenom = tbx_prenom.Text,
                    DateNaissance = (DateTimeOffset)calendar_dateNaissance.Date
                });
            }
                
        }

        private void ContentDialog_Closing(ContentDialog sender, ContentDialogClosingEventArgs args)
        {
            if (args.Result == ContentDialogResult.Primary)
            {
                if (fermerDialogue == false)
                    args.Cancel = true;
            }
            else
                args.Cancel = false;
        }

        private string genererMatricule()
        {
            int nombre = new Random().Next(0, 999);
            string matricule = "";

            //si on a un nombre généré qui est en dessous de 100
            //on ajoute 1 ou 2 zéros
            if (nombre < 10)
                matricule += "00" + nombre;
            else if (nombre >= 10 && nombre < 100)
                matricule += "0" + nombre;
            else
                matricule += nombre;

            //chaine de caractères retournée. On la met en majuscule.
            return $"{tbx_nom.Text.Substring(0,1)}{tbx_prenom.Text.Substring(0, 1)}{matricule}".ToUpper();
        }
    }
}
