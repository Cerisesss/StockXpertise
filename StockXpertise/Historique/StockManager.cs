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
                    query = "SELECT date, description, utilisateur FROM modifications_stock ORDER BY date DESC;";
                    break;

                case "Action d'utilisateur":
                    if (userId != -1)
                    {
                        query = $"SELECT date, description, utilisateur FROM modifications_stock WHERE id_employes = {userId} ORDER BY date DESC;";
                    }
                    break;

                // Ajoutez d'autres cas ici...

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
    }
}
