using iText.Barcodes.Dmcode;
using MySql.Data.MySqlClient;
using StockXpertise.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StockXpertise
{

    public partial class Add_fournisseur : Page
    {
        List<Fournisseur> fournisseurs = new List<Fournisseur>();

        string nom;
        string prenom;
        string numero;
        string mail;
        string adresse;
        public Add_fournisseur()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //recuperation des données des champs
            nom = nomfournisseur.Text;
            prenom = prenomfournisseur.Text;
            numero = numerofournisseur.Text;
            mail = mailfournisseur.Text;
            adresse = adressefournisseur.Text;
            //string image;

            if (!Regex.IsMatch(nom, "^[a-zA-Z]+$") || !Regex.IsMatch(prenom, "^[a-zA-Z]+$"))
            {
                MessageBox.Show("Le nom et prénom doit etre des lettres.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!mail.Contains("@"))
            {
                MessageBox.Show("Votre adresse mail semble incorrecte");
                return;
            }

            //condition pour verifier si les champs sont vides
            //si c'est le cas alors on affiche un message
            //sinon on execute la requete
            if (string.IsNullOrEmpty(nom) || string.IsNullOrEmpty(prenom) || string.IsNullOrEmpty(numero) || string.IsNullOrEmpty(mail) || string.IsNullOrEmpty(adresse))
            {
                MessageBox.Show("Veuillez remplir tous les champs.", "Oups !", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (int.TryParse(numero, out int result_prixHT))
                {
                    // requete pour ajouter un article
                    Query_Fournisseur query_insert = new Query_Fournisseur(nom, prenom, result_prixHT, mail, adresse);
                    query_insert.Insert_Founisseur();

                    //redirection vers la page affichage_stock.xaml
                    Supplier.fournisseur stock = new Supplier.fournisseur();
                    Window parentWindow = Window.GetWindow(this);

                    if (parentWindow != null)
                    {
                        parentWindow.Content = stock;
                    }
                }
                else
                {
                    // La conversion a échoué, numero ne contient pas une valeur entière valide
                    MessageBox.Show("Le numéro ne peut contenir que des chiffres.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //redirection vers la page affichage_stock.xaml
            Supplier.fournisseur fournisseur = new Supplier.fournisseur();
            Window parentWindow = Window.GetWindow(this);

            if (parentWindow != null)
            {
                parentWindow.Content = fournisseur;
            }
        }

        private void addStock_Navigated(object sender, NavigationEventArgs e)
        {

        }
    }
}
