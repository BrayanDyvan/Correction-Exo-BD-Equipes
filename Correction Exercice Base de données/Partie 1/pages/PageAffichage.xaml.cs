using Microsoft.UI;
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
using System.Diagnostics;
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
    public sealed partial class PageAffichage : Page
    {
        public PageAffichage()
        {
            this.InitializeComponent();
            SingletonBD.getInstance().getEquipes();
            gv_liste.ItemsSource = SingletonBD.getInstance().ListeEquipes;
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            DialogueAjout dialog = new DialogueAjout();
            dialog.XamlRoot = this.XamlRoot;
            dialog.PrimaryButtonText = "Ajouter";
            dialog.CloseButtonText = "Annuler";
            dialog.Title = "Nouvelle équipe";
            dialog.DefaultButton = ContentDialogButton.Close;

            ContentDialogResult resultat = await dialog.ShowAsync();
        }

        

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            //DataContext représente l'élément parent
            Equipe equipe = button.DataContext as Equipe;

            //permet de s'assurer que nous avons un élément sélectionné
            gv_liste.SelectedItem = equipe;

            ContentDialog dialog = new ContentDialog();
            dialog.XamlRoot = this.XamlRoot;
            dialog.Title = "Suppression d'équipe";
            dialog.PrimaryButtonText = "Supprimer";
            dialog.CloseButtonText = "Annuler";
            dialog.DefaultButton = ContentDialogButton.Primary;
            dialog.Content = $"Voulez vous supprimer l'équipe: {equipe.Nom} ?";

            ContentDialogResult resultat = await dialog.ShowAsync();

            if (resultat == ContentDialogResult.Primary)
                SingletonBD.getInstance().supprimerEquipe(equipe);
            else
                gv_liste.SelectedItem = null;

        }

        private void tbx_recherche_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbx_recherche.Text.Trim() != "")
                SingletonBD.getInstance().rechercheParVille(tbx_recherche.Text);
            else
                SingletonBD.getInstance().getEquipes();
        }

        private void btn_details_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            Equipe equipe = button.DataContext as Equipe;
            
            Frame.Navigate(typeof(PageDetailsEquipe), equipe);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            

            foreach (var item in Utilitaires.navigationView.MenuItems)
            {
                if(item is NavigationViewItem)
                {
                    NavigationViewItem navItem = (NavigationViewItem)item;
                    if (navItem.Name == "iEquipes")
                    {
                        Utilitaires.navigationView.SelectedItem = navItem;
                        break;
                    }
                }
                
                
            }
        }
    }
}
