using System;
using System.Configuration;
using System.IO;
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

        private static string GetConnectionString(string configFilePath)
        {
            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = configFilePath;

            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

            return config.ConnectionStrings.ConnectionStrings["MyDbConnection"]?.ConnectionString;
        }

        public static void ExecuteQuery(string query)
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

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            // Afficher les résultats dans la console
                            while (reader.Read())
                            {
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    Console.Write($"{reader.GetName(i)}: {reader.GetValue(i)}\t");
                                }
                                Console.WriteLine();
                            }
                        }
                    }

                    Console.WriteLine("SQL query executed successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ExecuteQuery: {ex.Message}");
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
