using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics; // Pour le Debug.WriteLine
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
//using System.Windows.Forms; // Pour le FolderBrowserDialog
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using MySqlX.XDevAPI.Relational;

namespace StockXpertise.Stock
{
    /// <summary>
    /// Logique d'interaction pour affichage_inventaire.xaml
    /// </summary>
    public partial class affichage_inventaire : Page
    {
        List<DataInventaire> articlesDataList = new List<DataInventaire>();

        public affichage_inventaire()
        {
            InitializeComponent();

            string query = "SELECT produit.id_produit, produit.quantite_stock, produit.quantite_stock_reel, articles.nom, emplacement.code, emplacement.code_reel FROM produit INNER JOIN articles ON produit.id_articles = articles.id_articles INNER JOIN emplacement ON produit.id_emplacement = emplacement.id_emplacement;";
            MySqlDataReader reader = ConfigurationDB.ExecuteQuery(query);

            remplissage_donnees(reader);
        }

        private void remplissage_donnees(MySqlDataReader reader)
        {
            List<int> redIds = new List<int>();
            List<int> greenIds = new List<int>();

            // Remplissage de la liste
            while (reader.Read())
            {
                var articleData = new DataInventaire()
                {
                    Id_produit = Convert.ToInt32(reader["id_produit"]),
                    Nom = reader["nom"].ToString(),
                    Quantite_stock = Convert.ToInt32(reader["quantite_stock"]),
                    Quantite_stock_reel = Convert.ToInt32(reader["quantite_stock_reel"]),
                    Code = reader["code"].ToString(),
                    Code_reel = reader["code_reel"].ToString()
                };

                if(articleData.Quantite_stock_reel == 0)
                {
                    Console.WriteLine("test");
                }
                else if(articleData.Code_reel == null)
                {
                    Console.WriteLine("test2");
                }
                else if ((articleData.Quantite_stock != articleData.Quantite_stock_reel || articleData.Code != articleData.Code_reel))
                {
                    redIds.Add(articleData.Id_produit);
                }
                else if (articleData.Quantite_stock == articleData.Quantite_stock_reel && articleData.Code == articleData.Code_reel)
                {
                    greenIds.Add(articleData.Id_produit);
                }

                articlesDataList.Add(articleData);
            }

            MyDataGrid.ItemsSource = articlesDataList;

            // Attache un gestionnaire d'événements au chargement du DataGrid
            MyDataGrid.Loaded += (sender, args) =>
            {
                // Parcours les lignes du DataGrid pour changer la couleur de fond
                for (int i = 0; i < MyDataGrid.Items.Count; i++)
                {
                    var item = MyDataGrid.Items[i] as DataInventaire;
                    if (item != null)
                    {
                        var row = MyDataGrid.ItemContainerGenerator.ContainerFromIndex(i) as DataGridRow;
                        if (row != null)
                        {
                            if (redIds.Contains(item.Id_produit))
                            {
                                row.Background = Brushes.Red;
                            }
                            else if (greenIds.Contains(item.Id_produit))
                            {
                                row.Background = Brushes.LightGreen;
                            }
                        }
                    }
                }
            };
        }

        private void MyDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MyDataGrid.SelectedItem != null)
            {
                gridStock.Visibility = Visibility.Collapsed;

                if (MyDataGrid.SelectedItem is DataInventaire selectedData)
                {
                    // Charger la page dans le Frame
                    StockInventaireFrame.Navigate(new inventaire(selectedData));
                }
            }
        }

        private void generation_pdf(object sender, RoutedEventArgs e)
        {
            // Ajout d'un titre au document PDF
            string text = "INVENTAIRE";

            // Ajout des données au document PDF : 4 colonnes pour id_produit, nom, quantite_stock, code
            iText.Layout.Element.Table table = new iText.Layout.Element.Table(6);

            // Ajoute des en-têtes de colonnes
            table.AddHeaderCell("ID Produit");
            table.AddHeaderCell("Nom");
            table.AddHeaderCell("Quantité en stock");
            table.AddHeaderCell("Quantité réelle");
            table.AddHeaderCell("Code Emplacement");
            table.AddHeaderCell("Code Emplacement réel");

            // Ajoute des données de la liste articlesDataList au tableau dans le PDF
            foreach (var data in articlesDataList)
            {
                table.AddCell(data.Id_produit.ToString());
                table.AddCell(data.Nom);
                table.AddCell(data.Quantite_stock.ToString());
                table.AddCell(data.Quantite_stock_reel.ToString());
                table.AddCell(data.Code);
                table.AddCell(data.Code_reel);
            }

            PDFGenerator.GeneratePDF(text, table, "inventaire.pdf");
        }
    }
}