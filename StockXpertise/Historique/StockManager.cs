using System;
using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;

namespace StockXpertise
{
    public class StockManager
    {
        public static ObservableCollection<StockModificationItem> ChargerModificationsStock(string selectedItem, int userId = -1)
        {
            string query = "";

            switch (selectedItem)
            {
                case "Quantité des stocks modifiée (tous)":
                    // À remplir avec la logique appropriée
                    query = "SELECT m.date, 'Modification' as description, CONCAT(e.prenom, ' ', e.nom) as utilisateur FROM mouvement m JOIN employes e ON m.id_employes = e.id_employes ORDER BY m.date DESC;";
                    break;

                case "Action d'utilisateur":
                    if (userId != -1)
                    {
                        // À remplir avec la logique appropriée
                        query = $"SELECT m.date, 'Action utilisateur' as description, CONCAT(e.prenom, ' ', e.nom) as utilisateur FROM mouvement m JOIN employes e ON m.id_employes = e.id_employes WHERE e.id_employes = {userId} ORDER BY m.date DESC;";
                    }
                    break;

                case "Rentrez":
                    // Exemple de requête pour les articles entrés (ajustez selon votre logique)
                    query = "SELECT a.date_ajout as date, 'Entrée de stock' as description, a.nom as utilisateur FROM articles a ORDER BY a.date_ajout DESC;";
                    break;

                case "Sortie":
                    // Exemple de requête pour les articles sortis (ajustez selon votre logique)
                    query = "SELECT s.date as date, 'Sortie de stock' as description, a.nom as utilisateur FROM stockage s JOIN produit p ON s.id_produit = p.id_produit JOIN articles a ON p.id_articles = a.id_articles ORDER BY s.date DESC;";
                    break;

                case "Transaction (caissier)":
                    // Exemple de requête pour les transactions des caissiers
                    query = "SELECT v.date_vente as date, 'Vente' as description, CONCAT(e.prenom, ' ', e.nom) as utilisateur FROM vente v JOIN mouvement m ON v.id_mouvement = m.id_mouvement JOIN employes e ON m.id_employes = e.id_employes WHERE e.role = 'Caissier' UNION ALL SELECT a.date_achat, 'Achat' as description, CONCAT(e.prenom, ' ', e.nom) as utilisateur FROM achat a JOIN mouvement m ON a.id_mouvement = m.id_mouvement JOIN employes e ON m.id_employes = e.id_employes WHERE e.role = 'Caissier' ORDER BY date DESC;";
                    break;

                default:
                    break;
            }

            return ExecuteQuery(query);
        }

        public static ObservableCollection<string> ChargerUtilisateurs()
        {
            ObservableCollection<string> utilisateurs = new ObservableCollection<string>();

            string query = "SELECT CONCAT(prenom, ' ', nom) AS utilisateur FROM employes";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConfigurationDB.GetConnectionString("./Configuration/config.xml")))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader["utilisateur"] != DBNull.Value)
                                {
                                    utilisateurs.Add(reader["utilisateur"].ToString());
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in StockManager - ChargerUtilisateurs: {ex.Message}");
            }

            return utilisateurs;
        }

        private static ObservableCollection<StockModificationItem> ExecuteQuery(string query)
        {
            ObservableCollection<StockModificationItem> stockModifications = new ObservableCollection<StockModificationItem>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConfigurationDB.GetConnectionString("./Configuration/config.xml")))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                StockModificationItem item = new StockModificationItem();

                                if (reader["date"] != DBNull.Value)
                                {
                                    item.Date = Convert.ToDateTime(reader["date"]);
                                }

                                if (reader["description"] != DBNull.Value)
                                {
                                    item.Description = reader["description"].ToString();
                                }

                                if (reader["utilisateur"] != DBNull.Value)
                                {
                                    item.Utilisateur = reader["utilisateur"].ToString();
                                }

                                stockModifications.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in StockManager - ExecuteQuery: {ex.Message}");
            }

            return stockModifications;
        }
        public static int GetUserId(string userName)
        {
            string query = "SELECT id_employes FROM employes WHERE CONCAT(prenom, ' ', nom) = @userName";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConfigurationDB.GetConnectionString("./Configuration/config.xml")))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Paramètre pour éviter les injections SQL
                        command.Parameters.AddWithValue("@userName", userName);

                        object result = command.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            return Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in StockManager - GetUserId: {ex.Message}");
            }

            // Retourner -1 si l'utilisateur n'est pas trouvé ou en cas d'erreur
            return -1;
        }
    }
}
