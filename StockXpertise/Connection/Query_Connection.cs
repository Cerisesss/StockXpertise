using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StockXpertise.Connection
{
    public class Query_Connection
    {
        string mail;
        string password;

        public Query_Connection(string mail, string password)
        {
            this.mail = mail;
            this.password = password;
        }

        public MySqlDataReader Select_Connection()
        {
            MySqlDataReader reader;

            try
            {
                // Requête SQL paramétrée
                string query = "SELECT * FROM employes WHERE mail = @Mail AND mot_de_passe = @Password ;";

                // Connexion à la database
                string configFilePath = "./Configuration/config.xml";
                string connectionString = ConfigurationDB.GetConnectionString(configFilePath);

                MySqlConnection ConnectionDB = new MySqlConnection(connectionString);
                ConnectionDB.Open();

                // Crée une commande SQL avec la requête et la connexion
                MySqlCommand commande = new MySqlCommand(query, ConnectionDB);

                // Ajoute les paramètres à la commande pour eviter les injections SQL
                commande.Parameters.AddWithValue("@Mail", mail);
                commande.Parameters.AddWithValue("@Password", password);

                // Exécute la commande
                return reader = commande.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in ConnectionDB: {ex.Message}");

                return null;
            }
        }
    }
}
