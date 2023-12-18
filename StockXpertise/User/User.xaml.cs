using MySql.Data.MySqlClient;
using StockXpertise.Connection;
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

namespace StockXpertise.User
{
    /// <summary>
    /// Logique d'interaction pour User.xaml
    /// </summary>
    public partial class User : Page
    {
        public User()
        {
            InitializeComponent();

            comboBoxAffichage.Items.Add("Tous");
            comboBoxAffichage.Items.Add("Nom");
            comboBoxAffichage.Items.Add("Prénom");
            comboBoxAffichage.Items.Add("Mail");
            comboBoxAffichage.Items.Add("Rôle");
            comboBoxAffichage.Items.Add("Croissant");
            comboBoxAffichage.Items.Add("Décroissant");

            string query = "SELECT * FROM employes ;";
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
                        query = "SELECT nom FROM employes ORDER BY nom;";
                        break;
                    case "Prénom":
                        query = "SELECT prenom FROM employes ORDER BY prenom;";
                        break;
                    case "Mail":
                        query = "SELECT mail FROM employes ORDER BY mail;";
                        break;
                    case "Rôle":
                        query = "SELECT role FROM employes ORDER BY Role;";
                        break;
                    case "Croissant":
                        query = "SELECT * FROM employes ORDER BY nom ASC;";
                        break;
                    case "Décroissant":
                        query = "SELECT * FROM employes ORDER BY nom DESC;";
                        break;
                    default:
                        query = "SELECT * FROM employes ;";
                        break;
                }
                MySqlDataReader reader = ConfigurationDB.ExecuteQuery(query);

                // Assigne les données au DataGrid
                MyDataGrid.ItemsSource = reader;
            }
        }
        /*private void MyDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MyDataGrid.SelectedItem != null)
            {
                Modify_User modifyUser = new Modify_User();
                Window parentWindow = Window.GetWindow(this);

                if (parentWindow != null)
                {
                    parentWindow.Content = modifyUser;
                }

                gridUser.Visibility = Visibility.Collapsed;

                var selectedData = MyDataGrid.SelectedItem;
                
                //user.Navigate(new affichageStock(selectedData));

                user.Navigate(new Uri("/User/Modify_User.xaml", UriKind.RelativeOrAbsolute));
            }
        }*/

        private void MyDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MyDataGrid.SelectedItem != null)
            {
                var selectedData = MyDataGrid.SelectedItem;

                Modify_User modifyUser = new Modify_User(selectedData);
                Window parentWindow = Window.GetWindow(this);

                if (parentWindow != null)
                {
                    parentWindow.Content = modifyUser;
                }

                gridUser.Visibility = Visibility.Collapsed;
                //user.Navigate(new Uri("/User/Modify_User.xaml", UriKind.RelativeOrAbsolute));

            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Add_User addUser = new Add_User();
            Window parentWindow = Window.GetWindow(this);

            if (parentWindow != null)
            {
                parentWindow.Content = addUser;
            }

            gridUser.Visibility = Visibility.Collapsed;
            user.Navigate(new Uri("/User/Add_User.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}
