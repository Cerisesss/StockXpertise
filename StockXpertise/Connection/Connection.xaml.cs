using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

            // Requête pour vérifier les informations de connexion dans la base de données
            string query = "SELECT * FROM employes WHERE mail = " + mail + " AND mot_de_passe = " + password + " ; ";

            ExecuteQuery(query);

            //recuperer les resultats de la requete
            //result = SqlDataReader.

            if ($result_query->num_rows > 0) {
                //msg : connexion reussie
                // page d'accueil
                
            }
            else 
            {                 
                //msg : connexion échouée
                                              
            }

            /*string connectionString = "YourConnectionString"; // Remplacez ceci par votre chaîne de connexion

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM YourTable"; // Remplacez YourTable par le nom de votre table

                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            // Récupérer les valeurs des colonnes pour chaque ligne
                            int id = reader.GetInt32(0); // Supposons que la première colonne est un entier
                            string name = reader.GetString(1); // Supposons que la deuxième colonne est une chaîne de caractères

                            Console.WriteLine($"ID: {id}, Name: {name}");

                            // Faites quelque chose avec les données récupérées
                        }
                    }
                    else
                    {
                        Console.WriteLine("Aucune ligne trouvée.");
                    }
                }
            }*/
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
