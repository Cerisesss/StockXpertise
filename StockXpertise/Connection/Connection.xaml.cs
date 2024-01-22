using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using StockXpertise.User;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Threading;

namespace StockXpertise.Connection
{
    /// <summary>
    /// Logique d'interaction pour Connection.xaml
    /// </summary>
    public partial class Connection : Page
    {
        private DispatcherTimer timer;

        public Connection()
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

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string mail = textboxMail.Text;
            string password = passwordboxPassword.Password;

            ConnexionProgressBar.Visibility = Visibility.Visible;

            timer.Start();

            await Task.Delay(2500);

            // requete pour ajouter un utilisateur
            Query_Connection query_select = new Query_Connection(mail, password);
            MySqlDataReader reader = query_select.Select_Connection();

            if (reader == null)
            {
                MessageBox.Show("Connexion échouée. Le mail ou le mot de passe est incorrect.", "Oups !", MessageBoxButton.OK, MessageBoxImage.Error);

                ConnexionProgressBar.Visibility = Visibility.Hidden;
                ConnexionProgressBar.Value = 0;
                timer.Stop();

                return;
            }
            else
            {
                ConnexionProgressBar.Value = 100;
                timer.Stop();

                while (reader.Read())
                {
                    int id_employee = Convert.ToInt32(reader["id_employes"]);

                    UserConnected user_connected = new UserConnected(id_employee, reader["nom"].ToString(), reader["prenom"].ToString(), password, mail, reader["role"].ToString());

                    Application.Current.Properties["id_employes"] = user_connected.GetIdEmployee();
                    Application.Current.Properties["nom"] = reader["nom"].ToString();
                    Application.Current.Properties["prenom"] = user_connected.GetPrenom();
                    Application.Current.Properties["mail"] = reader["mail"].ToString();
                    Application.Current.Properties["role"] = reader["role"].ToString();
                    Application.Current.Properties["mdp"] = reader["mot_de_passe"].ToString();
                }

                // envoie vers la page d'accueil
                gridConnection.Visibility = Visibility.Collapsed;
                connection.Navigate(new Uri("/Stock/affichage_stock.xaml", UriKind.RelativeOrAbsolute));
            }
        }

        private void TextBox_TextChanged_Mail(object sender, TextChangedEventArgs e)
        {
            //textboxMail


        }

        private void TextBox_TextChanged_Password(object sender, TextChangedEventArgs e)
        {
            //textboxMdp
        }
    }
}
