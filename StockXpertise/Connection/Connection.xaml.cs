using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System;
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

namespace StockXpertise.Connection
{
    /// <summary>
    /// Logique d'interaction pour Connection.xaml
    /// </summary>
    public partial class Connection : Page
    {
        public Connection()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string mail = textboxMail.Text;
            string password = textboxPassword.Text;

            string query = "SELECT * FROM employes WHERE mail = '" + mail + "' AND mot_de_passe = '" + password + "' ; ";

            MySqlDataReader reader = ConfigurationDB.ExecuteQuery(query);

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int id_employee = Convert.ToInt32(reader["id_employe"]);

                    User user_connected = new User(id_employee, reader["nom"].ToString(), reader["prenom"].ToString(), password, mail, reader["role"].ToString());
                        
                    Application.Current.Properties["id_employe"] = user_connected.GetIdEmployee();
                    Application.Current.Properties["nom"] = reader["nom"].ToString();
                    Application.Current.Properties["prenom"] = user_connected.GetPrenom();
                    Application.Current.Properties["password"] = reader["mot_de_passe"].ToString();
                    Application.Current.Properties["mail"] = reader["mail"].ToString();
                    Application.Current.Properties["role"] = reader["role"].ToString();
                }

                //msg : connexion reussie
                MessageBox.Show("Connexion réussie.");

                // envoie vers la page d'accueil
                gridConnection.Visibility = Visibility.Collapsed;
                connection.Navigate(new Uri("/Stock/Stock.xaml", UriKind.RelativeOrAbsolute));
            }
            else
            {
                MessageBox.Show("Connexion échouée. Le mail ou le mot de passe est incorrect.");
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
