using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Partie_1.classes
{
    internal class SingletonBD
    {
        ObservableCollection<Equipe> listeEquipes;
        ObservableCollection<Joueur> listeJoueurs;
        static SingletonBD instance = null;
        MySqlConnection con;

        
        public SingletonBD()
        {
            //Modifier la chaine de connexion pour mettre la votre
            //les tables doivent correspondre
            con = new MySqlConnection("Server=localhost;Database=demo;Uid=root;Pwd=root;");
            listeEquipes = new ObservableCollection<Equipe>();
            listeJoueurs = new ObservableCollection<Joueur>();
        }

        public static SingletonBD getInstance()
        {
            if (instance == null)
                instance = new SingletonBD();

            return instance;
        }


        /**************************** ZONE ÉQUIPES  ***********************/
        public ObservableCollection<Equipe> ListeEquipes{get {return listeEquipes; }}
        public void getEquipes()
        {
            listeEquipes.Clear();

            MySqlCommand commande = new MySqlCommand();
            commande.Connection = con;
            commande.CommandText = "Select * from equipe";

            con.Open();
            MySqlDataReader r = commande.ExecuteReader();

            while (r.Read())
            {
                string nom = r.GetString("nom");
                string ville = r.GetString("ville");
                string logo = r.GetString("logo");

                Equipe e = new Equipe(nom, ville, logo);

                listeEquipes.Add(e);
            }
            r.Close();
            con.Close();


        }

        public List<string> getnomEquipes(string nomEquipe)
        {
            List<string> equipes = new List<string>();

            MySqlCommand commande = new MySqlCommand();
            commande.Connection = con;
            commande.CommandText = $"Select nom from equipe where nom like @nomEquipe";
            commande.Parameters.AddWithValue("nomEquipe", nomEquipe+"%");

            con.Open();
            commande.Prepare();
            MySqlDataReader r = commande.ExecuteReader();
            while (r.Read()) { 
                equipes.Add(r.GetString("nom"));
            }
            
            r.Close();
            con.Close();

            return equipes;
        }

        public string getlogoEquipes(string nomEquipe)
        {

            MySqlCommand commande = new MySqlCommand();
            commande.Connection = con;
            commande.CommandText = $"Select logo from equipe where nom = '{nomEquipe}'";

            try
            {
                con.Open();
                MySqlDataReader r = commande.ExecuteReader();
                r.Read();
                string logo = r.GetString("logo");

                r.Close();
                con.Close();
                return logo;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public void ajouterEquipe(Equipe equipe)
        {
            try
            {
                MySqlCommand commande = new MySqlCommand();
                commande.Connection = con;
                commande.CommandText = "insert into equipe values(@nom, @ville, @logo) ";
                commande.Parameters.AddWithValue("@nom", equipe.Nom);
                commande.Parameters.AddWithValue("@ville", equipe.Ville);
                commande.Parameters.AddWithValue("@logo", equipe.Logo);

                con.Open();
                commande.Prepare();
                commande.ExecuteNonQuery();

                con.Close();

                getEquipes();
            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
            }
        }

        public void supprimerEquipe(Equipe equipe)
        {
            try
            {
                MySqlCommand commande = new MySqlCommand();
                commande.Connection = con;
                commande.CommandText = $"delete from equipe where nom = '{equipe.Nom}'";

                con.Open();
                int i = commande.ExecuteNonQuery();

                con.Close();
                getEquipes();
            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
            }
        }

        public void rechercheParVille(string v)
        {
            listeEquipes.Clear();

            MySqlCommand commande = new MySqlCommand();
            commande.Connection = con;
            commande.CommandText = "Select * from equipe where ville like @ville";  //fait une recherche pour toutes les équipes dont la ville commence par la lettre entrée
            commande.Parameters.AddWithValue("@ville", v + "%");        //dans ce cas-ci, j'ajoute le paramètre avec le signe % vue que nous utilisons "like"

            con.Open();
            MySqlDataReader r = commande.ExecuteReader();

            while (r.Read())
            {
                string nom = r.GetString("nom");
                string ville = r.GetString("ville");
                string logo = r.GetString("logo");

                Equipe e = new Equipe(nom, ville, logo);

                listeEquipes.Add(e);
            }
            r.Close();
            con.Close();
        }

        public bool checkEquipe(string nomEquipe)
        {
            MySqlCommand commande = new MySqlCommand();
            commande.Connection = con;
            commande.CommandText = "select count(*) from equipe where nom = @nomEquipe";
            commande.Parameters.AddWithValue("nomEquipe", nomEquipe);

            con.Open();
            commande.Prepare();
            var nombre = (Int64)commande.ExecuteScalar();
            
            con.Close();

            return nombre != 0;
        }

        /**************************** ZONE JOUEURS  ***********************/
        public ObservableCollection<Joueur> ListeJoueurs { get { return listeJoueurs; } }
        public void getJoueurs()
        {
            listeJoueurs.Clear();

            MySqlCommand commande = new MySqlCommand();
            commande.Connection = con;
            commande.CommandText = "Select * from joueur";

            con.Open();
            MySqlDataReader r = commande.ExecuteReader();

            while (r.Read())
            {
                string matricule = r.GetString("matricule");
                string nom = r.GetString("nom");
                string prenom = r.GetString("prenom");
                DateTime dateNaissance = r.GetDateTime("dateNaissance");
                string nomEquipe = r.GetString("NomEquipe");

                Joueur j = new Joueur{
                    Matricule = matricule,
                    Nom = nom,
                    Prenom = prenom,
                    DateNaissance = dateNaissance,
                    NomEquipe = nomEquipe
                };

                listeJoueurs.Add(j);
            }
            r.Close();
            con.Close();


        }

        public void getJoueursParEquipe(string n)
        {
            listeJoueurs.Clear();

            MySqlCommand commande = new MySqlCommand();
            commande.Connection = con;
            commande.CommandText = $"Select * from joueur where nomEquipe = '{n}'";

            con.Open();
            MySqlDataReader r = commande.ExecuteReader();

            while (r.Read())
            {
                string matricule = r.GetString("matricule");
                string nom = r.GetString("nom");
                string prenom = r.GetString("prenom");
                DateTime dateNaissance = r.GetDateTime("dateNaissance");
                string nomEquipe = r.GetString("NomEquipe");

                Joueur j = new Joueur
                {
                    Matricule = matricule,
                    Nom = nom,
                    Prenom = prenom,
                    DateNaissance = dateNaissance,
                    NomEquipe = nomEquipe
                };

                listeJoueurs.Add(j);
            }
            r.Close();
            con.Close();


        }

        public void ajouterJoueur(Joueur joueur)
        {
            try
            {
                MySqlCommand commande = new MySqlCommand();
                commande.Connection = con;
                commande.CommandText = "insert into joueur(matricule, nom, prenom, dateNaissance) values(@matricule, @nom, @prenom, @dateNaissance) ";
                commande.Parameters.AddWithValue("@matricule", joueur.Matricule);
                commande.Parameters.AddWithValue("@nom", joueur.Nom);
                commande.Parameters.AddWithValue("@prenom", joueur.Prenom);
                commande.Parameters.AddWithValue("@dateNaissance", joueur.DateNaissance.ToString("d"));

                con.Open();
                commande.Prepare();
                commande.ExecuteNonQuery();

                con.Close();

                getJoueurs();
            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
            }
        }

        public void modifierEquipe(string nomEquipe, string matricule)
        {
            MySqlCommand commande = new MySqlCommand();
            commande.Connection = con;
            commande.CommandText = "update joueur set nomEquipe = @nomEquipe where matricule = @matricule";
            commande.Parameters.AddWithValue("@matricule", matricule);
            commande.Parameters.AddWithValue("@nomEquipe", nomEquipe);

            con.Open();
            commande.Prepare();
            commande.ExecuteNonQuery();

            con.Close();

            if(nomEquipe != "")
                getJoueurs();
        }

        public void supprimerJoueur(Joueur joueur)
        {
            try
            {
                MySqlCommand commande = new MySqlCommand();
                commande.Connection = con;
                commande.CommandText = $"delete from joueur where matricule = '{joueur.Matricule}'";

                con.Open();
                int i = commande.ExecuteNonQuery();

                con.Close();

                getJoueurs();
            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
            }
        }

        public void rechercheParNomDeJoueur(string n)
        {
            listeJoueurs.Clear();

            MySqlCommand commande = new MySqlCommand();
            commande.Connection = con;
            commande.CommandText = "Select * from joueur where nom like @nom or prenom like @nom";  //fait une recherche pour toutes les joueurs dont le nom OU le prénom commence par la lettre entrée
            commande.Parameters.AddWithValue("@nom", n + "%");        //dans ce cas-ci, j'ajoute le paramètre avec le signe % vue que nous utilisons "like"

            con.Open();
            MySqlDataReader r = commande.ExecuteReader();

            while (r.Read())
            {
                string matricule = r.GetString("matricule");
                string nom = r.GetString("nom");
                string prenom = r.GetString("prenom");
                DateTime dateNaissance = r.GetDateTime("dateNaissance");
                string nomEquipe = r.GetString("NomEquipe");

                Joueur j = new Joueur
                {
                    Matricule = matricule,
                    Nom = nom,
                    Prenom = prenom,
                    DateNaissance = dateNaissance,
                    NomEquipe = nomEquipe
                };

                listeJoueurs.Add(j);
            }
            r.Close();
            con.Close();
        }

    }
}
