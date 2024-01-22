using MySql.Data.MySqlClient;
using StockXpertise.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using System.Windows;

namespace StockXpertise.Profile
{
    public class Query_Profile
    {
        private string mdp;
        private int id;

        public Query_Profile(int id)
        {
            this.id = id;
        }

        public Query_Profile(string mdp, int id)
        {
            this.mdp = mdp;
            this.id = id;
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

        public string GetPassword()
        {
            MySqlDataReader reader;
            string mdp = null;

            try
            {
                string query = "SELECT * FROM employes WHERE id_employes = @Id;";

                MySqlCommand commande = new MySqlCommand(query, ConnectionDB());
                commande.Parameters.AddWithValue("@Id", id);

                reader = commande.ExecuteReader();

                while (reader.Read())
                {
                    mdp = reader["mot_de_passe"].ToString();
                }
                return mdp;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in ConnectionDB: {ex.Message}");
                return null;
            }
        }

        public void Update_Profile()
        {
            MySqlDataReader reader;

            try
            {
                // Requête SQL paramétrée
                string query = "UPDATE employes SET mot_de_passe = @Mdp WHERE id_employes = @Id ;";

                // Crée une commande SQL avec la requête et la connexion
                MySqlCommand commande = new MySqlCommand(query, ConnectionDB());

                // Ajoute les paramètres à la commande pour eviter les injections SQL
                commande.Parameters.AddWithValue("@Mdp", mdp);
                commande.Parameters.AddWithValue("@Id", id);

                // Exécute la commande
                reader = commande.ExecuteReader();

                //message de confirmation
                MessageBox.Show("Modifié avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in ConnectionDB: {ex.Message}");
            }
        }
    }
}
