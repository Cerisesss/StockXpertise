using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities;
using StockXpertise.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
            //recuperation des données des champs
            string nom = nomTextBox.Text;
            string prenom = prenomTextBox.Text;
            string password = mdpTextBox.Text;
            string mail = mailTextBox.Text;
            string role = null;

            //recuperation du role selectionné
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

            //condition pour verifier si les champs sont vides
            //si c'est le cas alors on affiche un message
            //sinon on execute la requete d'ajout d'un utilisateur
            if (string.IsNullOrEmpty(nom) || string.IsNullOrEmpty(prenom) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(mail)|| role == null)
            {
                MessageBox.Show("Veuillez remplir tous les champs.");
            }
            else
            {
                //generation du sel
                string salt = BCrypt.Net.BCrypt.GenerateSalt(15);

                //hash le mot de passe avec le sel
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);

                // requete pour ajouter un utilisateur
                Query_User query_insert = new Query_User(nom, prenom, hashedPassword, mail, role);
                query_insert.Insert_User();

                //redirection vers la page User.xaml
                User user = new User();
                Window parentWindow = Window.GetWindow(this);

                if (parentWindow != null)
                {
                    parentWindow.Content = user;
                }

                //griAddUser.Visibility = Visibility.Collapsed;
                //addUser.Navigate(new Uri("/User/User.xaml", UriKind.RelativeOrAbsolute));
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //redirection vers la page User.xaml
            User user = new User();
            Window parentWindow = Window.GetWindow(this);

            if (parentWindow != null)
            {
                parentWindow.Content = user;
            }

            //griAddUser.Visibility = Visibility.Collapsed;
            //addUser.Navigate(new Uri("/User/User.xaml", UriKind.RelativeOrAbsolute));
        }

    }
}
