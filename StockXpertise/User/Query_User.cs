using MySql.Data.MySqlClient;
using StockXpertise.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace StockXpertise.User
{
    public class Query_User
    {
        int id;
        string nom;
        string prenom;
        string password;
        string mail;
        string role;

        //id = 0 par défaut pour l'insertion 
        public Query_User(string nom, string prenom, string password, string mail, string role) : this(0, nom, prenom, password, mail, role)
        {
        }

        public Query_User(int id, string nom, string prenom, string password, string mail, string role)
        {
            this.id = id;
            this.nom = nom;
            this.prenom = prenom;
            this.password = password;
            this.mail = mail;
            this.role = role;
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

        public void Update_User()
        {
            MySqlDataReader reader;

            try
            {
                // Requête SQL paramétrée
                string query = "UPDATE employes SET nom = @Nom, prenom = @Prenom, mot_de_passe = @Password, mail = @Mail, role = @Role WHERE id_employes = @Id ;";

                // Crée une commande SQL avec la requête et la connexion
                MySqlCommand commande = new MySqlCommand(query, ConnectionDB());

                // Ajoute les paramètres à la commande pour eviter les injections SQL
                commande.Parameters.AddWithValue("@Nom", nom);
                commande.Parameters.AddWithValue("@Prenom", prenom);
                commande.Parameters.AddWithValue("@Password", password);
                commande.Parameters.AddWithValue("@Mail", mail);
                commande.Parameters.AddWithValue("@Role", role);
                commande.Parameters.AddWithValue("@Id", id);

                // Exécute la commande
                reader = commande.ExecuteReader();

                //message de confirmation
                MessageBox.Show("Modifié avec succès.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in ConnectionDB: {ex.Message}");
            }
        }

        public void Delete_User()
        {
            MySqlDataReader reader;

            try
            {
                // Requête SQL paramétrée
                string query = "DELETE FROM employes WHERE id_employes = @Id; ";

                // Crée une commande SQL avec la requête et la connexion
                MySqlCommand commande = new MySqlCommand(query, ConnectionDB());

                // Ajoute les paramètres à la commande pour eviter les injections SQL
                commande.Parameters.AddWithValue("@Id", id);

                // Exécute la commande
                reader = commande.ExecuteReader();

                //message de confirmation
                MessageBox.Show("Supprimé avec succès.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in ConnectionDB: {ex.Message}");
            }
        }

        public void Insert_User()
        {
            MySqlDataReader reader;

            try
            {
                // Requête SQL paramétrée
                string query = "INSERT INTO employes (nom, prenom, mot_de_passe, mail, role) VALUES (@Nom, @Prenom, @Password, @Mail, @Role);";

                // Crée une commande SQL avec la requête et la connexion
                MySqlCommand commande = new MySqlCommand(query, ConnectionDB());

                // Ajoute les paramètres à la commande pour eviter les injections SQL
                commande.Parameters.AddWithValue("@Nom", nom);
                commande.Parameters.AddWithValue("@Prenom", prenom);
                commande.Parameters.AddWithValue("@Password", password);
                commande.Parameters.AddWithValue("@Mail", mail);
                commande.Parameters.AddWithValue("@Role", role);

                // Exécute la commande
                reader = commande.ExecuteReader();

                //message de confirmation
                MessageBox.Show("Ajouté avec succès.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in ConnectionDB: {ex.Message}");
            }
        }
    }
}
