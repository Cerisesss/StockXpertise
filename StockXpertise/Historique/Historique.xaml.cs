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
using System.Windows.Media.Animation;
using System.Diagnostics;

namespace StockXpertise
{

    public partial class Historique : Page
    {
        public Historique()
        {
            InitializeComponent();

            comboBoxAffichage.Items.Add(" ");
            comboBoxAffichage.Items.Add("Quantité des stock modifié(tous)");
            comboBoxAffichage.Items.Add("Inventaire(utilisateur)");
            comboBoxAffichage.Items.Add("Ajout d’article(admin)");
            comboBoxAffichage.Items.Add("Suppression d’article(admin)");
            comboBoxAffichage.Items.Add("Transaction(caissier)");
        }

        private void ComboBox_SelectionChanged_affichage(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxAffichage.SelectedItem != null)
            {
                string selectedValue = comboBoxAffichage.SelectedItem.ToString();
                string query;

                switch (selectedValue)
                {

                    case "Quantité des stock modifié(tous)":
                        query = "SELECT * FROM produit ORDER BY date_modification DESC";  
                        break;
                    case "Inventaire(utilisateur)":
                        query = "SELECT * FROM articles WHERE id_employes = <UserId> ORDER BY date_ajout DESC";  
                        break;
                    case "Ajout d’article(admin)":
                        query = "SELECT * FROM articles WHERE role = 'admin' ORDER BY date_ajout DESC";
                        break;
                    case "Suppression d’article(admin)":
                        query = "SELECT * FROM articles WHERE role = 'admin' ORDER BY date_suppression DESC";  
                        break;
                    case "Transaction(caissier)":
                        query = "SELECT * FROM vente ORDER BY date_vente DESC";  
                        break;
                    default:
                        query = "SELECT * FROM articles ORDER BY date_ajout DESC";
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
                gridHistorique.Visibility = Visibility.Collapsed;

                // Charger les données de l'article sélectionné
                var selectedData = MyDataGrid.SelectedItem;

                // Charger la page dans le Frame
                MainFrame.Navigate(new affichageStock(selectedData));
            }
        }

    }
}