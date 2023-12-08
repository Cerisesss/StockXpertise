using System;
using System.Configuration;
using System.IO;
using System.Windows;
using MySql.Data.MySqlClient;

namespace StockXpertise
{
    public class ConfigurationDB
    {
        public static void ConnectionDB()
        {
            try
            {
                string configFilePath = "./Configuration/config.xml";
                string connectionString = GetConnectionString(configFilePath);

                if (string.IsNullOrEmpty(connectionString))
                {
                    Console.WriteLine("Connection string is null or empty.");
                    return;
                }

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    Console.WriteLine("Connection opened successfully.");

                    string scriptFilePath = "./Configuration/construct.MySql";
                    ExecuteSqlScript(connection, scriptFilePath);
                    string insertFilePath = "./Configuration/insert.MySql";
                    ExecuteSqlScript(connection, insertFilePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ConnectionDB: {ex.Message}");
            }
        }

        public static string GetConnectionString(string configFilePath)
        {
            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = configFilePath;

            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

            return config.ConnectionStrings.ConnectionStrings["MyDbConnection"]?.ConnectionString;
        }

        public static MySqlDataReader ExecuteQuery(string query)
        {
            MySqlDataReader reader;

            try
            {
                string configFilePath = "./Configuration/config.xml";
                string connectionString = GetConnectionString(configFilePath);
                MySqlConnection ConnectionDB = new MySqlConnection(connectionString);
                ConnectionDB.Open();

                // Requête pour vérifier les informations de connexion dans la base de données
                MySqlCommand commande = new MySqlCommand(query, ConnectionDB);
                return reader = commande.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in ConnectionDB: {ex.Message}");
                return reader = null;
            }
        }

        private static void ExecuteSqlScript(MySqlConnection connection, string scriptFilePath)
        {
            try
            {
                string script = File.ReadAllText(scriptFilePath);

                using (MySqlCommand command = new MySqlCommand(script, connection))
                {
                    command.ExecuteNonQuery();
                }

                Console.WriteLine("SQL script executed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing SQL script: {ex.Message}");
            }
        }
    }
}
