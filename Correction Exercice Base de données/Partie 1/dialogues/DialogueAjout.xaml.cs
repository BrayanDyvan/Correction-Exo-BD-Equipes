using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using Partie_1.classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Partie_1.dialogues
{
    public sealed partial class DialogueAjout : ContentDialog
    {
        bool fermerDialogue = true;
        public DialogueAjout()
        {
            this.InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            //validation du nom
            if (string.IsNullOrWhiteSpace(tbx_nom.Text))
            {
                erreur_nom.Text = "Veuillez entre le nom de l'équipe";
                fermerDialogue = false;
            }
            else if(tbx_nom.Text.Length <=3 || tbx_nom.Text.Length > 50)
            {
                erreur_nom.Text = "Nom entre 3 et 50 caractères";
                fermerDialogue = false;
            }
            else
            {
                fermerDialogue = true;
            }

            //validation de la ville
            if (string.IsNullOrWhiteSpace(tbx_ville.Text))
            {
                erreur_ville.Text = "Veuillez entre la ville de l'équipe";
                fermerDialogue = false;
            }
            else if (tbx_ville.Text.Length <= 3 || tbx_ville.Text.Length > 50)
            {
                erreur_ville.Text = "Ville entre 3 et 50 caractères";
                fermerDialogue = false;
            }
            else
            {
                fermerDialogue = true;
            }

            //validation du lien
            if (string.IsNullOrWhiteSpace(tbx_logo.Text))
            {
                erreur_logo.Text = "Veuillez entre le lien du logo de l'équipe";
                fermerDialogue = false;
            }
            else if(!Uri.IsWellFormedUriString(tbx_logo.Text, UriKind.Absolute))
            {
                erreur_logo.Text = "Veuillez entrer un lien valide";
                fermerDialogue = false;
            }
            else
            {
                fermerDialogue = true;
            }

            //si tout est valide, on insère dans la base de données
            if(fermerDialogue) 
                SingletonBD.getInstance().ajouterEquipe(new Equipe(tbx_nom.Text, tbx_ville.Text, tbx_logo.Text));
        }

        private void ContentDialog_Closing(ContentDialog sender, ContentDialogClosingEventArgs args)
        {
            if (args.Result == ContentDialogResult.Primary)
            {
                if(fermerDialogue == false) 
                    args.Cancel = true;
            }
            else
                args.Cancel = false;
        }



    }
}
