using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
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

namespace Partie_1.pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PageDetailsEquipe : Page
    {
        Equipe Equipe { get; set; }
        public PageDetailsEquipe()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter != null)
            {
                Equipe = (Equipe)e.Parameter;
                SingletonBD.getInstance().getJoueursParEquipe(Equipe.Nom);
                gv_joueurs.ItemsSource = SingletonBD.getInstance().ListeJoueurs;
                
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Joueur joueur = button.DataContext as Joueur;
            SingletonBD.getInstance().modifierEquipe("", joueur.Matricule);
            SingletonBD.getInstance().getJoueursParEquipe(Equipe.Nom);
        }
    }
}
