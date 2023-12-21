using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
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
using System.Data;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.X509;

namespace StockXpertise.Stock
{
    /// <summary>
    /// Logique d'interaction pour affichage_stock.xaml
    /// </summary>
    public partial class affichage_stock : Page
    {
        List<Article> articlesDataList = new List<Article>();

        public affichage_stock()
        {
            InitializeComponent();

            comboBoxAffichage.Items.Add(" ");
            comboBoxAffichage.Items.Add("Nom");
            comboBoxAffichage.Items.Add("Famille");
            comboBoxAffichage.Items.Add("Code barre");
            comboBoxAffichage.Items.Add("Quantité");
            comboBoxAffichage.Items.Add("Prix croissant");
            comboBoxAffichage.Items.Add("Prix décroissant");

            string query = "SELECT articles.id_articles, articles.image, articles.nom, articles.famille, articles.code_barre, articles.description, articles.prix_ht, articles.prix_ttc, produit.quantite_stock FROM articles JOIN produit ON articles.id_articles = produit.id_articles";
            MySqlDataReader reader = ConfigurationDB.ExecuteQuery(query);

            remplissage_donnees(reader);
        }

        private void remplissage_donnees(MySqlDataReader reader)
        {
            // Remplissez la liste d'Article
            while (reader.Read())
            {
                var articleData = new Article
                {
                    Id = Convert.ToInt32(reader["id_articles"]),
                    Nom = reader["nom"].ToString(),
                    Famille = reader["famille"].ToString(),
                    CodeBarre = reader["code_barre"].ToString(),
                    Description = reader["description"].ToString(),
                    Quantite = Convert.ToInt32(reader["quantite_stock"]),
                    PrixHT = Convert.ToInt32(reader["prix_ht"]),
                    PrixTTC = Convert.ToInt32(reader["prix_ttc"])
                };

                articlesDataList.Add(articleData);
            }

            // Assigne les données au DataGrid
            MyDataGrid.ItemsSource = articlesDataList;
        }

        private void ComboBox_SelectionChanged_affichage(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxAffichage.SelectedItem != null)
            {
                string selectedValue = comboBoxAffichage.SelectedItem.ToString();
                string query;

                switch (selectedValue)
                {
                    case "Nom":
                        query = "SELECT articles.id_articles, articles.image, articles.nom, articles.famille, articles.code_barre, articles.description, articles.prix_ht, articles.prix_ttc, produit.quantite_stock FROM articles JOIN produit ON articles.id_articles = produit.id_articles ORDER BY articles.nom";
                        break;
                    case "Famille":
                        query = "SELECT articles.id_articles, articles.image, articles.nom, articles.famille, articles.code_barre, articles.description, articles.prix_ht, articles.prix_ttc, produit.quantite_stock FROM articles JOIN produit ON articles.id_articles = produit.id_articles ORDER BY articles.famille";
                        break;
                    case "Code barre":
                        query = "SELECT articles.id_articles, articles.image, articles.nom, articles.famille, articles.code_barre, articles.description, articles.prix_ht, articles.prix_ttc, produit.quantite_stock FROM articles JOIN produit ON articles.id_articles = produit.id_articles ORDER BY articles.code_barre";
                        break;
                    case "Quantité":
                        query = "SELECT articles.id_articles, articles.nom, produit.quantite_stock, articles.image, articles.famille, articles.code_barre, articles.description, articles.prix_ht, articles.prix_ttc FROM articles JOIN produit ON articles.id_articles = produit.id_articles";
                        break;
                    case "Prix croissant":
                        query = "SELECT articles.id_articles, articles.nom, articles.prix_ht, articles.prix_ttc, articles.image, articles.famille, articles.code_barre, articles.description, produit.quantite_stock FROM articles JOIN produit ON articles.id_articles = produit.id_articles ORDER BY prix_ht ASC;";
                        break;
                    case "Prix décroissant":
                        query = "SELECT articles.id_articles, articles.image, articles.nom, articles.famille, articles.code_barre, articles.description, articles.prix_ht, articles.prix_ttc, produit.quantite_stock FROM articles JOIN produit ON articles.id_articles = produit.id_articles ORDER BY prix_ht DESC";
                        break;
                    default:
                        query = "SELECT articles.id_articles, articles.image, articles.nom, articles.famille, articles.code_barre, articles.description, articles.prix_ht, articles.prix_ttc, produit.quantite_stock FROM articles JOIN produit ON articles.id_articles = produit.id_articles ORDER BY articles.nom";

                        break;
                }
                MySqlDataReader reader = ConfigurationDB.ExecuteQuery(query);

                // Assigne les données au DataGrid
                remplissage_donnees(reader);
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MyDataGrid.SelectedItem != null)
            {
                gridStock.Visibility = Visibility.Collapsed;

                if (MyDataGrid.SelectedItem is Article selectedData)
                {
                    // Charger la page dans le Frame
                    StockFrame.Navigate(new modification_stock(selectedData));
                }
            }
        }

        private void generation_pdf(object sender, RoutedEventArgs e)
        {
            // TODO : Générer le PDF
        }
    }
}