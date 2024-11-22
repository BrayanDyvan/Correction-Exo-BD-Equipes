using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MySqlX.XDevAPI;
using Partie_1.classes;
using Partie_1.dialogues;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Partie_1.pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PageAffichageJoueurs : Page
    {
        public PageAffichageJoueurs()
        {
            this.InitializeComponent();
            SingletonBD.getInstance().getJoueurs();
            gv_liste.ItemsSource = SingletonBD.getInstance().ListeJoueurs;
        }

        private void tbx_recherche_TextChanged(object sender, TextChangedEventArgs e)
        {
            SingletonBD.getInstance().rechercheParNomDeJoueur(tbx_recherche.Text);
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            DialogueAjoutJoueur dialog = new DialogueAjoutJoueur();
            dialog.XamlRoot = this.XamlRoot;
            dialog.PrimaryButtonText = "Ajouter";
            dialog.CloseButtonText = "Annuler";
            dialog.Title = "nouveau joueur";
            dialog.DefaultButton = ContentDialogButton.Close;

            ContentDialogResult resultat = await dialog.ShowAsync();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            //DataContext représente l'élément parent
            Joueur joueur = button.DataContext as Joueur;

            //permet de s'assurer que nous avons un élément sélectionné
            gv_liste.SelectedItem = joueur;

            ContentDialog dialog = new ContentDialog();
            dialog.XamlRoot = this.XamlRoot;
            dialog.Title = "Suppression de joueur";
            dialog.PrimaryButtonText = "Supprimer";
            dialog.CloseButtonText = "Annuler";
            dialog.DefaultButton = ContentDialogButton.Primary;
            dialog.Content = $"Voulez vous supprimer le joueur: {joueur.Prenom} {joueur.Nom} ?";

            ContentDialogResult resultat = await dialog.ShowAsync();

            if (resultat == ContentDialogResult.Primary)
                SingletonBD.getInstance().supprimerJoueur(joueur);
            else
                gv_liste.SelectedItem = null;
        }

        
        private void btn_choixEquipe_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            AutoSuggestBox autoSuggestBox = sender as AutoSuggestBox;

            if (autoSuggestBox.Text == "")
            {
                autoSuggestBox.ItemsSource = null;
                return;
            }

            List<string> suggestions = SingletonBD.getInstance().getnomEquipes(autoSuggestBox.Text);

            if (suggestions.Count == 0)
                return;

            autoSuggestBox.ItemsSource = suggestions;

        }

        private async void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            AutoSuggestBox autoSuggestBox = sender as AutoSuggestBox;

            Joueur joueur = (Joueur)autoSuggestBox.DataContext;

            if(args.ChosenSuggestion != null)
            {
                string n = args.ChosenSuggestion.ToString();
                if (SingletonBD.getInstance().checkEquipe(n) == true)
                    SingletonBD.getInstance().modifierEquipe(n, joueur.Matricule);
                else
                {
                    ContentDialog dialog = new ContentDialog();

                    dialog.XamlRoot = gv_liste.XamlRoot;
                    dialog.Title = "Avertissement";
                    dialog.CloseButtonText = "Fermer";
                    dialog.Content = "L'équipe indiquée n'existe pas.";

                    var result = await dialog.ShowAsync();

                }
            }
            
        }

        private async void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileSavePicker();

            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(Utilitaires.mainWindow);
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hWnd);

            picker.SuggestedFileName = "test2";
            picker.FileTypeChoices.Add("Fichier CSV", new List<string>() { ".csv" });

            //crée le fichier
            Windows.Storage.StorageFile monFichier = await picker.PickSaveFileAsync();

            if(monFichier != null)
            {
                //on convertit la liste des joueurs du singleton en List pour pouvoir écrire dans le fichier
                List<Joueur> liste = SingletonBD.getInstance().ListeJoueurs.ToList();
            
                await Windows.Storage.FileIO.WriteLinesAsync(monFichier, liste.ConvertAll(x => x.StringCSV), Windows.Storage.Streams.UnicodeEncoding.Utf8);
            }

            
        }
    }
}
