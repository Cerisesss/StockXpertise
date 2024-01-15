using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using System.Windows;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using System.Collections;

namespace StockXpertise
{
    public class Query_Fournisseur
    {
        string noms;
        string prenoms;
        int numeros;
        string mails;
        string adresses;

        public Query_Fournisseur(string nom, string prenom, int numero, string mail, string adresse)
        {
            this.noms = nom;
            this.prenoms = prenom; 
            this.numeros = numero;
            this.mails = mail;
            this.adresses = adresse;
        }



        //***********************************************************************************  connection DB  ********************************************************************************************************\\


        public MySqlConnection ConnectionDB()
        {
            // Connexion à la database
            string configFilePath = "./Configuration/config.xml";
            string connectionString = ConfigurationDB.GetConnectionString(configFilePath);

            MySqlConnection ConnectionDB = new MySqlConnection(connectionString);
            ConnectionDB.Open();

            return ConnectionDB;
        }



        public void Insert_Founisseur()
        {
            MySqlDataReader reader;

            try
            {
                // Requête SQL paramétrée
                string query = "INSERT INTO fournisseur (nom, prenom, numero, mail, adresse) VALUES (@nom, @prenom, @numero, @mail, @adresse);";

                // Crée une commande SQL avec la requête et la connexion
                MySqlCommand commande = new MySqlCommand(query, ConnectionDB());

                // Ajoute les paramètres à la commande pour eviter les injections SQL
                commande.Parameters.AddWithValue("@nom", noms);
                commande.Parameters.AddWithValue("@prenom", prenoms);
                commande.Parameters.AddWithValue("@numero", numeros);
                commande.Parameters.AddWithValue("@mail", mails);
                commande.Parameters.AddWithValue("@adresse", adresses);



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


        //***********************************************************************************  delete article  ********************************************************************************************************\\

     /*   public void Delete_From_Achat()
        {
            MySqlDataReader reader;

            //supprimer de la table achat
            try
            {
                string query_getIDProduit = "SELECT * FROM produit WHERE id_articles = @IdArticle ;";
                MySqlCommand commande = new MySqlCommand(query_getIDProduit, ConnectionDB());

                commande.Parameters.AddWithValue("@IdArticle", id_article);

                reader = commande.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        id_produit = Convert.ToInt32(reader["id_produit"]);

                        string query_produit = "DELETE FROM achat WHERE id_produit = @Id_Produit ;";
                        MySqlCommand commande_sql = new MySqlCommand(query_produit, ConnectionDB());

                        commande_sql.Parameters.AddWithValue("@Id_Produit", id_produit);

                        commande_sql.ExecuteReader();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in ConnectionDB: {ex.Message}");
            }
        }

        public void Delete_From_Vente()
        {
            MySqlDataReader reader;

            //supprimer de la table vente
            try
            {
                string query_getIDProduit = "SELECT * FROM produit WHERE id_articles = @IdArticle ;";
                MySqlCommand commande = new MySqlCommand(query_getIDProduit, ConnectionDB());

                commande.Parameters.AddWithValue("@IdArticle", id_article);

                reader = commande.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        id_produit = Convert.ToInt32(reader["id_produit"]);

                        string query_produit = "DELETE FROM vente WHERE id_produit = @Id_Produit ;";
                        MySqlCommand commande_sql = new MySqlCommand(query_produit, ConnectionDB());

                        commande_sql.Parameters.AddWithValue("@Id_Produit", id_produit);

                        commande_sql.ExecuteReader();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in ConnectionDB: {ex.Message}");
            }
        }

        public void Delete_Stock()
        {
            MySqlDataReader reader;

            try
            {
                // recupere l'id article 
                string query_id_article = "SELECT id_articles FROM articles WHERE nom = @Nom AND famille = @Famille AND prix_ht = @PrixHT AND prix_ttc = @PrixTTC AND description = @Description AND code_barre = @CodeBarre ;";
                MySqlCommand commande_id_article = new MySqlCommand(query_id_article, ConnectionDB());

                commande_id_article.Parameters.AddWithValue("@Nom", nom);
                commande_id_article.Parameters.AddWithValue("@Famille", famille);
                commande_id_article.Parameters.AddWithValue("@PrixHT", prixHT);
                commande_id_article.Parameters.AddWithValue("@PrixTTC", prixTTC);
                commande_id_article.Parameters.AddWithValue("@Description", description);
                commande_id_article.Parameters.AddWithValue("@CodeBarre", code_barre);

                reader = commande_id_article.ExecuteReader();

                while (reader.Read())
                {
                    id_article = reader.GetInt32(0);
                }


                Delete_From_Achat();
                Delete_From_Vente();

                // supprimer de la table produit
                string query_produit = "DELETE FROM produit WHERE id_articles = @IdArticles ;";
                MySqlCommand commande_sql = new MySqlCommand(query_produit, ConnectionDB());

                commande_sql.Parameters.AddWithValue("@IdArticles", id_article);
                commande_sql.Parameters.AddWithValue("@Quantite", quantite);

                commande_sql.ExecuteReader();

                // supprimer de la table article
                string query_article = "DELETE FROM articles WHERE nom = @Nom AND famille = @Famille AND prix_ht = @PrixHT AND prix_ttc = @PrixTTC AND description = @Description AND code_barre = @CodeBarre ;";
                MySqlCommand commande = new MySqlCommand(query_article, ConnectionDB());

                commande.Parameters.AddWithValue("@Nom", nom);
                commande.Parameters.AddWithValue("@Famille", famille);
                commande.Parameters.AddWithValue("@PrixHT", prixHT);
                commande.Parameters.AddWithValue("@PrixTTC", prixTTC);
                commande.Parameters.AddWithValue("@Description", description);
                commande.Parameters.AddWithValue("@CodeBarre", code_barre);

                commande.ExecuteReader();

                //message de confirmation
                MessageBox.Show("Supprimé avec succès.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in ConnectionDB: {ex.Message}");
            }
        }*/
    }
}
