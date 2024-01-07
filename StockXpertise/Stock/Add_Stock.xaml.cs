using MySql.Data.MySqlClient;
using StockXpertise.User;
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
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace StockXpertise.Stock
{
    /// <summary>
    /// Logique d'interaction pour Add_Stock.xaml
    /// </summary>
    public partial class Add_Stock : Page
    {
        List<Fournisseur> fournisseurs = new List<Fournisseur>();

        string nom;
        string famille;
        string prixHT_string;
        string prixTTC_string;
        string prixAchat_string;
        string description;
        string code_barre;
        string quantite_string;
        string emplacement;
        string nomFournisseur;
        string prenomFournisseur;

        public Add_Stock()
        {
            InitializeComponent();

            string query = "SELECT * FROM fournisseur";
            MySqlDataReader reader = ConfigurationDB.ExecuteQuery(query);

            // Rempli la liste de fournisseur
            while (reader.Read())
            {
                Fournisseur fournisseur = new Fournisseur(reader["nom"].ToString(), reader["prenom"].ToString());
                fournisseurs.Add(fournisseur);
            }

            FournisseurListBox.ItemsSource = fournisseurs;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //recuperation des données des champs
            nom = nomStock.Text;
            famille = familleStock.Text;
            prixHT_string = prixHtStock.Text;
            prixTTC_string = prixTtcStock.Text;
            prixAchat_string = prixAchatStock.Text;
            description = descriptionStock.Text;
            code_barre = CodeBarreStock.Text;
            quantite_string = quantiteStock.Text;
            emplacement = emplacementStock.Text;
            //string image;

            

            //condition pour verifier si les champs sont vides
            //si c'est le cas alors on affiche un message
            //sinon on execute la requete
            if (string.IsNullOrEmpty(nom) || string.IsNullOrEmpty(famille) || string.IsNullOrEmpty(code_barre) || string.IsNullOrEmpty(description) || string.IsNullOrEmpty(quantite_string) || string.IsNullOrEmpty(emplacement) || FournisseurListBox.SelectedIndex < 0 || string.IsNullOrEmpty(prixHT_string) || string.IsNullOrEmpty(prixTTC_string) || string.IsNullOrEmpty(prixAchat_string))
            {
                 MessageBox.Show("Veuillez remplir tous les champs.");
            }
            else
            {
                bool result_prixHT = Int32.TryParse(prixHT_string, out var prixHT);
                bool result_prixTTC = Int32.TryParse(prixTTC_string, out var prixTTC);
                bool result_prixAchat = Int32.TryParse(prixAchat_string, out var prixAchat);
                bool result_quantite = Int32.TryParse(quantite_string, out var quantite);


                if (!result_prixHT || !result_prixTTC || !result_prixAchat || !result_quantite)
                {
                    MessageBox.Show("Le prix HT, TTC, achat et la quantité doivent être numérique !");
                }
                else
                {
                    // requete pour ajouter un article
                    Query_Stock query_insert = new Query_Stock(nom, famille, prixHT, prixTTC, prixAchat, description, code_barre, quantite, emplacement, nomFournisseur, prenomFournisseur);
                    query_insert.Insert_Stock();

                    //redirection vers la page affichage_stock.xaml
                    affichage_stock stock = new affichage_stock();
                    Window parentWindow = Window.GetWindow(this);

                    if (parentWindow != null)
                    {
                        parentWindow.Content = stock;
                    }
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //redirection vers la page affichage_stock.xaml
            affichage_stock stock = new affichage_stock();
            Window parentWindow = Window.GetWindow(this);

            if (parentWindow != null)
            {
                parentWindow.Content = stock;
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic selectedFournisseur = (dynamic)FournisseurListBox.SelectedItem;

            // Vérifier si un élément est sélectionné
            if (selectedFournisseur != null)
            {
                // Convertir l'élément en chaîne (si nécessaire)
                nomFournisseur = selectedFournisseur.getNom();
                prenomFournisseur = selectedFournisseur.getPrenom();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
