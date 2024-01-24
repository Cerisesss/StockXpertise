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
using StockXpertise.Connection;
using iText.IO.Image;
using System.IO;

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

            string query = "SELECT articles.id_articles, articles.image, articles.nom, articles.famille, articles.code_barre, articles.description, articles.prix_ht, articles.prix_ttc, produit.quantite_stock, produit.id_emplacement, emplacement.code FROM articles JOIN produit ON articles.id_articles = produit.id_articles JOIN emplacement ON produit.id_emplacement = emplacement.id_emplacement";
            MySqlDataReader reader = ConfigurationDB.ExecuteQuery(query);
            remplissage_donnees(reader);

            if (Application.Current.Properties["role"].ToString() == "Admin" || Application.Current.Properties["role"].ToString() == "admin")
            {
                addStock.Visibility = Visibility.Visible;
            }
            else
            {
                addStock.Visibility = Visibility.Hidden;
            }
        }

        private void remplissage_donnees(MySqlDataReader reader)
        {
            // Vide la collection Items du DataGrid
            MyDataGrid.Items.Clear();

            // Vide la liste d'articles
            articlesDataList.Clear();

            // Rempli la liste d'Article
            while (reader.Read())
            {
                var imagePath = reader["image"].ToString().Replace('\\', '/');

                var articleData = new Article
                {
                    Id = Convert.ToInt32(reader["id_articles"]),
                    Image = new BitmapImage(new Uri("C:/Users/paulb/Desktop/travail de fou malade/main/stockxpertise/StockXpertise" + imagePath)),
                    Code_emplacement = reader["code"].ToString(),
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

        private void remplissage_donnees(MySqlDataReader reader, string motRecherche)
        {
            List<Article> matchingArticles = new List<Article>();
            List<Article> otherArticles = new List<Article>();

            while (reader.Read())
            {
                var articleData = new Article
                {
                    Id = Convert.ToInt32(reader["id_articles"]),
                    Code_emplacement = reader["code"].ToString(),
                    Nom = reader["nom"].ToString(),
                    Famille = reader["famille"].ToString(),
                    CodeBarre = reader["code_barre"].ToString(),
                    Description = reader["description"].ToString(),
                    Quantite = Convert.ToInt32(reader["quantite_stock"]),
                    PrixHT = Convert.ToInt32(reader["prix_ht"]),
                    PrixTTC = Convert.ToInt32(reader["prix_ttc"])
                };

                bool isMatching = false;

                // Comparaison du mot de recherche avec chaque propriété de l'article
                foreach (var property in articleData.GetType().GetProperties())
                {
                    var value = property.GetValue(articleData);
                    if (value != null && value.ToString().ToLower().Contains(motRecherche.ToLower()))
                    {
                        isMatching = true;
                        break;
                    }
                }

                if (isMatching)
                {
                    matchingArticles.Add(articleData);
                }
                else
                {
                    otherArticles.Add(articleData);
                }
            }

            // Fusion des listes pour placer les lignes correspondantes en haut
            matchingArticles.AddRange(otherArticles);

            MyDataGrid.ItemsSource = matchingArticles;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MyDataGrid.SelectedItem != null)
            {
                gridStock.Visibility = Visibility.Collapsed;

                if (MyDataGrid.SelectedItem is Article selectedData)
                {
                    // Charge la page dans le Frame
                    StockFrame.Navigate(new modification_stock(selectedData));
                }
            }
        }

        private void generation_pdf(object sender, RoutedEventArgs e)
        {
            var articlesDataList = MyDataGrid.ItemsSource as List<Article>;

            if (articlesDataList != null)
            {
                string text = "STOCK";

                iText.Layout.Element.Table table = new iText.Layout.Element.Table(8);

                table.AddHeaderCell("Nom");
                table.AddHeaderCell("Emplacement");
                table.AddHeaderCell("Famille");
                table.AddHeaderCell("Code barre");
                table.AddHeaderCell("Description");
                table.AddHeaderCell("HT");
                table.AddHeaderCell("TTC");
                table.AddHeaderCell("Quantité en stock");

                foreach (var data in articlesDataList)
                {
                    table.AddCell(data.Nom);
                    table.AddCell(data.Code_emplacement.ToString());
                    table.AddCell(data.Famille);
                    table.AddCell(data.CodeBarre);
                    table.AddCell(data.Description);
                    table.AddCell(data.PrixHT.ToString());
                    table.AddCell(data.PrixTTC.ToString());
                    table.AddCell(data.Quantite.ToString());
                }

                PDFGenerator.GeneratePDF(text, table, "stock.pdf");
            }
        }

        private void Search_TextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            string motRecherche = Search_TextBox.Text;

            string query = "SELECT articles.id_articles, articles.nom, articles.famille, articles.code_barre, articles.description, articles.prix_ht, articles.prix_ttc, produit.quantite_stock, produit.id_emplacement, emplacement.code FROM articles JOIN produit ON articles.id_articles = produit.id_articles JOIN emplacement ON produit.id_emplacement = emplacement.id_emplacement";

            // Si un mot de recherche est saisi, ajuste la requête et exécute la recherche
            if (!string.IsNullOrEmpty(motRecherche))
            {
                query += " WHERE articles.id_articles LIKE @motRecherche OR articles.nom LIKE @motRecherche OR articles.famille LIKE @motRecherche OR articles.code_barre LIKE @motRecherche OR articles.description LIKE @motRecherche OR articles.prix_ht LIKE @motRecherche OR articles.prix_ttc LIKE @motRecherche OR produit.quantite_stock LIKE @motRecherche OR produit.id_emplacement LIKE @motRecherche OR emplacement.code LIKE @motRecherche";
                query = query.Replace("@motRecherche", "'" + "%" + motRecherche + "%" + "'");
                MySqlDataReader reader = ConfigurationDB.ExecuteQuery(query);

                MyDataGrid.ItemsSource = null;

                // Appel de la méthode remplissage_donnees avec le lecteur retourné et le mot de recherche
                remplissage_donnees(reader, motRecherche);
            }
            else
            {
                MyDataGrid.ItemsSource = null;

                // Si aucun mot de recherche n'est saisi, exécute la méthode sans l'argument supplémentaire
                MySqlDataReader reader = ConfigurationDB.ExecuteQuery(query);

                // Appel de la méthode remplissage_donnees sans le deuxième argument
                remplissage_donnees(reader);
            }
        }

        private void Add_New_Stock(object sender, RoutedEventArgs e)
        {
            Add_Stock newStock = new Add_Stock();

            Window parentWindow = Window.GetWindow(this);

            if (parentWindow != null)
            {
                parentWindow.Content = newStock;
            }
        }
    }
}