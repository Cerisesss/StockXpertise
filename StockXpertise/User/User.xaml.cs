using Google.Protobuf.Compiler;
using MySql.Data.MySqlClient;
using StockXpertise.Connection;
using StockXpertise.Stock;
using System;
using System.CodeDom.Compiler;
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
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace StockXpertise.User
{
    /// <summary>
    /// Logique d'interaction pour User.xaml
    /// </summary>
    public partial class User : Page
    {
        List<UserConnected> Users = new List<UserConnected>();
        
        public User()
        {
            InitializeComponent();

            //les elements de la combobox (trier)
            comboBoxAffichage.Items.Add("Aucun");
            comboBoxAffichage.Items.Add("ID");
            comboBoxAffichage.Items.Add("Nom");
            comboBoxAffichage.Items.Add("Prénom");
            comboBoxAffichage.Items.Add("Mail");
            comboBoxAffichage.Items.Add("Rôle");
            comboBoxAffichage.Items.Add("Croissant");
            comboBoxAffichage.Items.Add("Décroissant");

            // execute la requete
            string query = "SELECT * FROM employes ;";
            MySqlDataReader reader = ConfigurationDB.ExecuteQuery(query);

            // Assigne les données au DataGrid
            MyDataGrid.ItemsSource = reader;
        }

        private void ComboBox_SelectionChanged_affichage(object sender, SelectionChangedEventArgs e)
        {
            //si un element est selectionné dans la combobox (trier) alors on execute la requete correspondante
            if (comboBoxAffichage.SelectedItem != null)
            {
                string selectedValue = comboBoxAffichage.SelectedItem.ToString();
                string query;
                switch (selectedValue)
                {
                    case "ID":
                        query = "SELECT * FROM employes ORDER BY id_employes;";
                        break;
                    case "Nom":
                        query = "SELECT * FROM employes ORDER BY nom;";
                        break;
                    case "Prénom":
                        query = "SELECT * FROM employes ORDER BY prenom;";
                        break;
                    case "Mail":
                        query = "SELECT * FROM employes ORDER BY mail;";
                        break;
                    case "Rôle":
                        query = "SELECT * FROM employes ORDER BY Role;";
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

                //execute la requete
                MySqlDataReader reader = ConfigurationDB.ExecuteQuery(query);

                // Assigne les données au DataGrid
                MyDataGrid.ItemsSource = reader;
            }
        }

        private void MyDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MyDataGrid.SelectedItem != null)
            {
                //pour un type dynamique (sans classe)
                dynamic selectedPerson = (dynamic)MyDataGrid.SelectedItem;

                //stocke les valeurs de la ligne selectionnée dans des variables "globales" pour pouvoir les utiliser dans une autre page
                Application.Current.Properties["IdDataGrid"] = selectedPerson.GetInt32(0);
                Application.Current.Properties["NomDataGrid"] = selectedPerson.GetString(1);
                Application.Current.Properties["PrenomDataGrid"] = selectedPerson.GetString(2);
                Application.Current.Properties["MdpDataGrid"] = selectedPerson.GetString(3);
                Application.Current.Properties["MailDataGrid"] = selectedPerson.GetString(4);
                Application.Current.Properties["RoleDataGrid"] = selectedPerson.GetString(5);
                
                //creer une nouvelle page Modify_User
                Modify_User modifyUser = new Modify_User();
                Window parentWindow = Window.GetWindow(this);

                //affiche le contenu de la page suivante
                if (parentWindow != null)
                {
                    parentWindow.Content = modifyUser;
                }

                //gridUser.Visibility = Visibility.Collapsed;
                //user.Navigate(new Uri("/User/Modify_User.xaml", UriKind.RelativeOrAbsolute));
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //creer une nouvelle page Add_User
            Add_User addUser = new Add_User();
            Window parentWindow = Window.GetWindow(this);

            //affiche le contenu de la page suivante
            if (parentWindow != null)
            {
                parentWindow.Content = addUser;
            }

            //gridUser.Visibility = Visibility.Collapsed;
            //user.Navigate(new Uri("/User/Add_User.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}
