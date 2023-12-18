using MySql.Data.MySqlClient;
using StockXpertise.Connection;
using System;
using System.Collections.Generic;
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
    /// Logique d'interaction pour Add_User.xaml
    /// </summary>
    public partial class Add_User : Page
    {
        public Add_User()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string nom = nomTextBox.Text;
            string prenom = prenomTextBox.Text;
            string password = mdpTextBox.Text;
            string mail = mailTextBox.Text;
            string role = null;

            if (radioAdmin.IsChecked == true)
            {
                role = radioAdmin.Content.ToString();
            }
            else if (radioPersonnel.IsChecked == true)
            {
                role = radioPersonnel.Content.ToString();
            }
            else if (radioCaissier.IsChecked == true)
            {
                role = radioCaissier.Content.ToString();
            }

            if (nom == null || prenom == null || password == null || mail == null || role == null)
            {
                MessageBox.Show("Veuillez remplir tous les champs.");
            }
            else
            {
                string query = "INSERT INTO employes (nom, prenom, mot_de_passe, mail, role) VALUES ('" + nom + "', '" + prenom + "', '" + password + "', '" + mail + "', '" + role + "');";

                ConfigurationDB.ExecuteQuery(query);

                MessageBox.Show("Ajouté avec succès.");

                User user = new User();
                Window parentWindow = Window.GetWindow(this);

                if (parentWindow != null)
                {
                    parentWindow.Content = user;
                }

                griAddUser.Visibility = Visibility.Collapsed;
                addUser.Navigate(new Uri("/User/User.xaml", UriKind.RelativeOrAbsolute));
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            User user = new User();
            Window parentWindow = Window.GetWindow(this);

            if (parentWindow != null)
            {
                parentWindow.Content = user;
            }

            griAddUser.Visibility = Visibility.Collapsed;
            addUser.Navigate(new Uri("/User/User.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}
