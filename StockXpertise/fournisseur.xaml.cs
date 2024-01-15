using MySql.Data.MySqlClient;
using StockXpertise.Stock;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

namespace StockXpertise
{
    /// <summary>
    /// Logique d'interaction pour fournisseur.xaml
    /// </summary>
    public partial class fournisseur : Page
    {
        public fournisseur()
        {
            InitializeComponent();

            comboBoxAffichage.Items.Add(" ");
            comboBoxAffichage.Items.Add("Nom");
            comboBoxAffichage.Items.Add("Prenom");
            comboBoxAffichage.Items.Add("Produit");

            string query = "SELECT * from fournisseur";
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
                        query = "SELECT nom FROM fournisseur ORDER BY nom;";
                        break;
                    case "Prenom":
                        query = "SELECT Prenom FROM fournisseur ORDER BY Prenom;";
                        break;
                    case "Produit":
                        query = "SELECT nom AS nom_fournisseur, \r\n       (SELECT GROUP_CONCAT(nom SEPARATOR ', ') \r\n        FROM articles \r\n        WHERE id_fournisseur = fournisseur.id_fournisseur) AS produits_associes \r\nFROM fournisseur \r\nORDER BY produits_associes;\r\n";
                        break;

                    default:
                        query = "SELECT * from fournisseur";
                        break;
                }
                MySqlDataReader reader = ConfigurationDB.ExecuteQuery(query);

                // Assigne les données au DataGrid
                MyDataGrid.ItemsSource = reader;
            }
        }

        private void Add_New_Stock(object sender, RoutedEventArgs e)
        {
            Add_fournisseur newfournisseur = new Add_fournisseur();

            Window parentWindow = Window.GetWindow(this);

            if (parentWindow != null)
            {
                parentWindow.Content = newfournisseur;
            }
        }
    }
}
