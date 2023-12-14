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

namespace StockXpertise
{
    /// <summary>
    /// Logique d'interaction pour Stock.xaml
    /// </summary>
    public partial class Stock : Page
    {
        public Stock()
        {
            InitializeComponent();

            comboBoxAffichage.Items.Add(" ");
            comboBoxAffichage.Items.Add("Nom");
            comboBoxAffichage.Items.Add("Famille");
            comboBoxAffichage.Items.Add("Code barre");
            comboBoxAffichage.Items.Add("Quantité");
            comboBoxAffichage.Items.Add("Prix croissant");
            comboBoxAffichage.Items.Add("Prix décroissant");

            string query = "SELECT articles.image, articles.nom, articles.famille, articles.code_barre, articles.description, articles.prix_ht, articles.prix_ttc, produit.quantite_stock FROM articles JOIN produit ON articles.id_articles = produit.id_articles";
            MySqlDataReader reader = ConfigurationDB.ExecuteQuery(query);

            // Assigne les données au DataGrid
            MyDataGrid.ItemsSource = reader; 
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
                        query = "SELECT nom FROM articles ORDER BY nom;";
                        break;
                    case "Famille":
                        query = "SELECT nom, famille FROM articles ORDER BY famille;";
                        break;
                    case "Code barre":
                        query = "SELECT nom, code_barre FROM articles ORDER BY code_barre;";
                        break;
                    case "Quantité":
                        query = "SELECT articles.nom, produit.quantite_stock FROM articles JOIN produit ON articles.id_articles = produit.id_articles;"; 
                        break;
                    case "Prix croissant":
                        query = "SELECT nom, prix_ht, prix_ttc FROM articles ORDER BY prix_ht ASC;";
                        break;
                    case "Prix décroissant":
                        query = "SELECT nom, prix_ht, prix_ttc FROM articles ORDER BY prix_ht DESC;";
                        break;
                    default:
                        query = "SELECT articles.image, articles.nom, articles.famille, articles.code_barre, articles.description, articles.prix_ht, articles.prix_ttc, produit.quantite_stock FROM articles JOIN produit ON articles.id_articles = produit.id_articles";
                        break;
                }
                MySqlDataReader reader = ConfigurationDB.ExecuteQuery(query);

                // Assigne les données au DataGrid
                MyDataGrid.ItemsSource = reader;
            }
        }

        private void MyDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MyDataGrid.SelectedItem != null)
            {
                gridStock.Visibility = Visibility.Collapsed;

                // Charger les données de l'article sélectionné
                var selectedData = MyDataGrid.SelectedItem;

                // Charger la page dans le Frame
                StockFrame.Navigate(new affichageStock(selectedData));
            }
        }

        private void generation_pdf(object sender, RoutedEventArgs e)
        {
            // TODO : Générer le PDF
        }
    }
}