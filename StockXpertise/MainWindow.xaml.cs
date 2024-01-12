using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace StockXpertise
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            ConfigurationDB.ActualisationDB();
            InitializeComponent();
            
            string nomemployes = "SELECT nom FROM employes"; 
            
            ConfigurationDB.ExecuteQuery(nomemployes);


            //ajout de l'admin
            string password = "admin";

            string salt = BCrypt.Net.BCrypt.GenerateSalt(15);
            password = BCrypt.Net.BCrypt.HashPassword(password, salt);

            string configFilePath = "./Configuration/config.xml";
            string connectionString = ConfigurationDB.GetConnectionString(configFilePath);

            MySqlConnection ConnectionDB = new MySqlConnection(connectionString);
            ConnectionDB.Open();

            string add_first_admin = "INSERT INTO employes (nom, prenom, mot_de_passe, mail, role) VALUES ('Admin', 'Admin', @Password, 'admin@gmail.com', 'Admin');";
            
            MySqlCommand commande = new MySqlCommand(add_first_admin, ConnectionDB);
            commande.Parameters.AddWithValue("@Password", password);
            commande.ExecuteReader();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            gridMainWidow.Visibility = Visibility.Collapsed;
            mainWindow.Navigate(new Uri("/Connection/Connection.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}
