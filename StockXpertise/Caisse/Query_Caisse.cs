using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StockXpertise.Caisse
{
    public class Query_Caisse : Caisse
    {
        private string code_barre;

        public Query_Caisse(string code_barre)
        {
            this.code_barre = code_barre;
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

        public MySqlDataReader Compare_Quantite()
        {
            try
            {
                string query = "SELECT * FROM produit p JOIN articles a ON p.id_articles = a.id_articles WHERE code_barre = @CodeBarre";

                MySqlCommand commande = new MySqlCommand(query, ConnectionDB());
                commande.Parameters.AddWithValue("@CodeBarre", code_barre);
                
                return commande.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in ConnectionDB: {ex.Message}");
                return null;
            }
        }

        public void Maj_Quantite_Stock(int decrementValue)
        {
            try
            {
                string query_select = "SELECT * FROM produit p JOIN articles a ON p.id_articles = a.id_articles WHERE code_barre = @CodeBarre";

                MySqlCommand commande_select = new MySqlCommand(query_select, ConnectionDB());
                commande_select.Parameters.AddWithValue("@CodeBarre", code_barre);

                MySqlDataReader result_select = commande_select.ExecuteReader();

                if(result_select.HasRows)
                {
                    while(result_select.Read())
                    {
                        int id_produit = result_select.GetInt32("id_produit");
                        int quantite_stock = result_select.GetInt32("quantite_stock");

                        while(decrementValue >= 0)
                        {
                            if(quantite_stock > 0)
                            {
                                quantite_stock--;
                                decrementValue--;
                            }
                            else
                            {
                                break;
                            }
                        }

                        string query = "UPDATE produit p JOIN articles a ON p.id_articles = a.id_articles SET p.quantite_stock = @QuantiteStock WHERE a.code_barre = @CodeBarre AND id_produit = @IdProduit";

                        MySqlCommand commande = new MySqlCommand(query, ConnectionDB());
                        commande.Parameters.AddWithValue("@QuantiteStock", quantite_stock);
                        commande.Parameters.AddWithValue("@CodeBarre", code_barre);
                        commande.Parameters.AddWithValue("@IdProduit", id_produit);

                        commande.ExecuteReader();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in Maj_Quantite_Stock: {ex.Message}");
                return;
            }
        }


        /*public int Maj_Quantite_Stock(int decrementValue)
        {
            try
            {
                string query = "UPDATE produit p " +
               "JOIN articles a ON p.id_articles = a.id_articles " +
               $"SET p.quantite_stock = p.quantite_stock - @DecrementValue " +
               "WHERE a.code_barre = @CodeBarre";

                MySqlCommand commande = new MySqlCommand(query, ConnectionDB());
                commande.Parameters.AddWithValue("@DecrementValue", decrementValue);
                commande.Parameters.AddWithValue("@CodeBarre", code_barre);

                int rowsAffected = commande.ExecuteNonQuery();

                return rowsAffected;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in Maj_Quantite_Stock: {ex.Message}");
                return -1;
            }
        }*/
    }
}
