using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities;
using StockXpertise.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Windows.Threading;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace StockXpertise.User
{
    /// <summary>
    /// Logique d'interaction pour Add_User.xaml
    /// </summary>
    public partial class Add_User : Page
    {
        private DispatcherTimer timer;

        public Add_User()
        {
            InitializeComponent();

            ConnexionProgressBar.Visibility = Visibility.Hidden;

            ConnexionProgressBar.Value = 0;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Incrémentez la barre de progression
            ConnexionProgressBar.Value += (200.0 / (3500 / timer.Interval.TotalMilliseconds));
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
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



            //verification pour savoir si le nom et prenom sont des lettres et si le mail est bien un mail
            if (!Regex.IsMatch(nom, "^[a-zA-Z]+$") || !Regex.IsMatch(prenom, "^[a-zA-Z]+$"))
            {
                MessageBox.Show("Le nom et prénom doit etre des lettres.", "Attention", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            /*le regex ne marche pas
             * if (!Regex.IsMatch(mail, @"^\S+@\S+\.\S+$"))
            {
                MessageBox.Show("Ce mail n'est pas valide");
                return;
            }*/

            //barre de progression
            ConnexionProgressBar.Visibility = Visibility.Visible;

            timer.Start();

            await Task.Delay(2500);

            //condition pour verifier si les champs sont vides
            //si c'est le cas alors on affiche un message
            //sinon on execute la requete d'ajout d'un utilisateur
            if (string.IsNullOrEmpty(nom) || string.IsNullOrEmpty(prenom) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(mail)|| role == null)
            {
                MessageBox.Show("Veuillez remplir tous les champs.", "Attention", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            if (!mail.Contains("@"))
            {
                MessageBox.Show("Votre adresse mail semble incorrecte");
                return;
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

                ConnexionProgressBar.Value = 100;
                timer.Stop();

                //redirection vers la page User.xaml
                User user = new User();
                Window parentWindow = Window.GetWindow(this);

                if (parentWindow != null)
                {
                    parentWindow.Content = user;
                }
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
