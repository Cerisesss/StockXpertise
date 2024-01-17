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

        int id_mouvement;

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
                MessageBox.Show("Modifié avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in ConnectionDB: {ex.Message}");
            }
        }


        public void Delete_From_Vente(int id_mouvement)
        {
            MySqlDataReader reader;

            //supprimer de la table vente
            try
            {
                //supprimer de la table vente
                string query_vente = "SELECT * FROM vente WHERE id_mouvement = @Id_mouvement ;";
                MySqlCommand commande_vente = new MySqlCommand(query_vente, ConnectionDB());

                commande_vente.Parameters.AddWithValue("@Id_mouvement", id_mouvement);
                reader = commande_vente.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string query = "DELETE FROM vente WHERE id_mouvement = @Id_mouvement ;";
                        MySqlCommand commande = new MySqlCommand(query, ConnectionDB());

                        commande.Parameters.AddWithValue("@Id_mouvement", id_mouvement);

                        commande.ExecuteReader();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in ConnectionDB: {ex.Message}");
            }
        }

        public void Delete_From_Achat(int id_mouvement)
        {
            MySqlDataReader reader;

            try
            {
                //supprimer de la table achat
                string query_achat = "SELECT * FROM achat WHERE id_mouvement = @Id_mouvement ;";
                MySqlCommand commande_achat = new MySqlCommand(query_achat, ConnectionDB());

                commande_achat.Parameters.AddWithValue("@Id_mouvement", id_mouvement);
                reader = commande_achat.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string query = "DELETE FROM achat WHERE id_mouvement = @Id_mouvement ;";
                        MySqlCommand commande = new MySqlCommand(query, ConnectionDB());

                        commande.Parameters.AddWithValue("@Id_mouvement", id_mouvement);

                        commande.ExecuteReader();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in ConnectionDB: {ex.Message}");
            }
        }

        public void Delete_From_Mouvement()
        {
            MySqlDataReader reader;

            //supprimer de la table mouvement
            try
            {
                string query = "SELECT * FROM mouvement WHERE id_employes = @Id ;";
                MySqlCommand commande = new MySqlCommand(query, ConnectionDB());

                commande.Parameters.AddWithValue("@Id", id);

                reader = commande.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        id_mouvement = Convert.ToInt32(reader["id_mouvement"]);

                        //supprimer dans les tables achat et vente
                        Delete_From_Vente(id_mouvement);
                        Delete_From_Achat(id_mouvement);
                    }

                    //supprimer de la table mouvement
                    string query_delete = "DELETE FROM mouvement WHERE id_employes = @Id_employes ;";
                    MySqlCommand commande_sql = new MySqlCommand(query_delete, ConnectionDB());

                    commande_sql.Parameters.AddWithValue("@Id_employes", id);

                    commande_sql.ExecuteReader();
                }
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
                Delete_From_Mouvement();

                // Requête SQL paramétrée
                string query = "DELETE FROM employes WHERE id_employes = @Id; ";

                // Crée une commande SQL avec la requête et la connexion
                MySqlCommand command = new MySqlCommand(query, ConnectionDB());

                // Ajoute les paramètres à la commande pour eviter les injections SQL
                command.Parameters.AddWithValue("@Id", id);

                // Exécute la commande
                reader = command.ExecuteReader();

                //message de confirmation
                MessageBox.Show("Supprimé avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in ConnectionDB: {ex.Message}");
            }
        }

        public void Insert_User()
        {
            MySqlDataReader reader_verif;
            MySqlDataReader reader;

            try
            {
                string query_verif = "SELECT * FROM employes WHERE mail = @Mail;";

                MySqlCommand commande_verif = new MySqlCommand(query_verif, ConnectionDB());
                commande_verif.Parameters.AddWithValue("@Mail", mail);

                reader_verif = commande_verif.ExecuteReader();

                if (reader_verif.HasRows)
                {
                    MessageBox.Show("Mail déjà utilisé.", "Attention", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
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
                    MessageBox.Show("Ajouté avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in ConnectionDB: {ex.Message}");
            }
        }
    }
}
