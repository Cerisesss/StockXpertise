using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics; // Pour le Debug.WriteLine
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

            string query = "SELECT produit.id_produit, produit.quantite_stock, articles.nom, emplacement.code FROM produit INNER JOIN articles ON produit.id_articles = articles.id_articles INNER JOIN emplacement ON produit.id_emplacement = emplacement.id_emplacement;";
            MySqlDataReader reader = ConfigurationDB.ExecuteQuery(query);

            remplissage_donnees(reader);
        }

        private void remplissage_donnees(MySqlDataReader reader)
        {
            // Remplir la liste
            while (reader.Read())
            {
                var articleData = new DataInventaire()
                {
                    Id_produit = Convert.ToInt32(reader["id_produit"]),
                    Nom = reader["nom"].ToString(),
                    Quantite_stock = Convert.ToInt32(reader["quantite_stock"]),
                    Code = reader["code"].ToString()
                };

                articlesDataList.Add(articleData);
            }
            // Assigne les données au DataGrid
            MyDataGrid.ItemsSource = articlesDataList;
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
            // TODO : Générer le PDF
        }
    }
}