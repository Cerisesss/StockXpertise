using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
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

        public MySqlConnection ConnectionDB()
        {
            // Connexion à la database
            string configFilePath = "./Configuration/config.xml";
            string connectionString = ConfigurationDB.GetConnectionString(configFilePath);

            MySqlConnection ConnectionDB = new MySqlConnection(connectionString);
            ConnectionDB.Open();

            return ConnectionDB;
        }

        public MySqlDataReader Select_Connection()
        {
            MySqlDataReader reader;

            string passwordDB = GetPassword(); //recupere le hash du mdp dans la db

            if (passwordDB == null)
            {
                return null;
            }

            bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(password, passwordDB);

            if (!isPasswordCorrect)
            {
                return null;
            }

            try
            {
                // Requête SQL paramétrée
                string query = "SELECT * FROM employes WHERE mail = @Mail ;";

                // Crée une commande SQL avec la requête et la connexion
                MySqlCommand commande = new MySqlCommand(query, ConnectionDB());

                // Ajoute les paramètres à la commande pour eviter les injections SQL
                commande.Parameters.AddWithValue("@Mail", mail);
                commande.Parameters.AddWithValue("@Password", isPasswordCorrect);

                // Exécute la commande
                return reader = commande.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in ConnectionDB: {ex.Message}");

                return null;
            }
        }

        public string GetPassword()
        {
            MySqlDataReader reader;
            string mdp = null;

            try
            {
                string query = "SELECT * FROM employes WHERE mail = @Mail;";

                MySqlCommand commande = new MySqlCommand(query, ConnectionDB());
                commande.Parameters.AddWithValue("@Mail", mail);

                reader = commande.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        mdp = reader["mot_de_passe"].ToString();
                    }
                }
                return mdp;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in ConnectionDB: {ex.Message}");
                return null;
            }
        }
    }
}
