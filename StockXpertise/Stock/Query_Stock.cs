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

namespace StockXpertise.Stock
{
    public class Query_Stock
    {
        int id_article;
        string nom;
        string famille;
        int prixHT;
        int prixTTC;
        int prixAchat;
        string description;
        string code_barre;
        int quantite;
        string emplacement;
        string nomFournisseur;
        string prenomFournisseur;
        int id_fournisseur;
        int id_emplacement;
        int id_produit;

        public Query_Stock(string nom, string famille, int prixHT, int prixTTC, string description, string code_barre, int quantite) : this(0, nom, famille, prixHT, prixTTC, 0, description, code_barre, quantite, "", "", "")
        {
        }

        public Query_Stock(string nom, string famille, int prixHT, int prixTTC, int prixAchat, string description, string code_barre, int quantite, string emplacement, string nomFournisseur, string prenomFournisseur) : this(0, nom, famille, prixHT, prixTTC, prixAchat, description, code_barre, quantite, emplacement, nomFournisseur, prenomFournisseur)
        {
        }

        public Query_Stock(int id_article, string nom, string famille, int prixHT, int prixTTC, int prixAchat, string description, string code_barre, int quantite, string emplacement, string nomFournisseur, string prenomFournisseur)
        {
            this.id_article = id_article;
            this.nom = nom;
            this.famille = famille;
            this.prixHT = prixHT;
            this.prixTTC = prixTTC;
            this.prixAchat = prixAchat;
            this.description = description;
            this.code_barre = code_barre;
            this.quantite = quantite;
            this.emplacement = emplacement;
            this.nomFournisseur = nomFournisseur;
            this.prenomFournisseur = prenomFournisseur;
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

        public void Count_id_article()
        {
            MySqlDataReader reader;

            string query = "SELECT COUNT(*) FROM articles;";

            MySqlCommand commande = new MySqlCommand(query, ConnectionDB());

            reader = commande.ExecuteReader();

            while (reader.Read())
            {
                id_article = reader.GetInt32(0) + 1;
            }
        }

        public void Recup_id_fournisseur()
        {
            MySqlDataReader reader;

            try
            {
                // Requête SQL paramétrée
                string query = "SELECT * FROM fournisseur WHERE nom = @NomFournisseur AND prenom = @PrenomFournisseur ;";

                // Crée une commande SQL avec la requête et la connexion
                MySqlCommand commande = new MySqlCommand(query, ConnectionDB());

                // Ajoute les paramètres à la commande pour eviter les injections SQL
                commande.Parameters.AddWithValue("@NomFournisseur", nomFournisseur);
                commande.Parameters.AddWithValue("@PrenomFournisseur", prenomFournisseur);

                // Exécute la commande
                reader = commande.ExecuteReader();

                while (reader.Read())
                {
                    id_fournisseur = Convert.ToInt32(reader["id_fournisseur"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in ConnectionDB: {ex.Message}");
            }
        }

        public void Recup_id_emplacement()
        {
            MySqlDataReader reader;

            try
            {
                // Requête SQL paramétrée
                string query = "SELECT * FROM emplacement WHERE code = @Emplacement ;";

                // Crée une commande SQL avec la requête et la connexion
                MySqlCommand commande = new MySqlCommand(query, ConnectionDB());

                // Ajoute les paramètres à la commande pour eviter les injections SQL
                commande.Parameters.AddWithValue("@Emplacement", emplacement);

                // Exécute la commande
                reader = commande.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        id_emplacement = Convert.ToInt32(reader["id_emplacement"]);
                    }
                }
                else
                {
                    //ajout du nouveau emplacement
                    string query_add_emplacement = "INSERT INTO emplacement (code) VALUES (@Emplacement)";
                    MySqlCommand commande_sql = new MySqlCommand(query_add_emplacement, ConnectionDB());

                    commande_sql.Parameters.AddWithValue("@Emplacement", emplacement);

                    reader = commande_sql.ExecuteReader();

                    //recup l'id de l'emplacement
                    string query_add_new_emplacement = "SELECT * FROM emplacement WHERE code = @Emplacement ;";
                    MySqlCommand query_sql = new MySqlCommand(query_add_new_emplacement, ConnectionDB());

                    query_sql.Parameters.AddWithValue("@Emplacement", emplacement);

                    reader = query_sql.ExecuteReader();

                    while (reader.Read())
                    {
                        id_emplacement = Convert.ToInt32(reader["id_emplacement"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in ConnectionDB: {ex.Message}");
            }
        }

        public void Insert_Stock()
        {
            MySqlDataReader reader;

            Recup_id_fournisseur();
            Recup_id_emplacement();
            Count_id_article();

            try
            {
                // Requête SQL paramétrée
                string query = "INSERT INTO articles (id_fournisseur, nom, famille, prix_ht, prix_ttc, prix_achat, description, code_barre) VALUES (@IdFournisseur, @Nom, @Famille, @PrixHT, @PrixTTC, @PrixAchat, @Description, @CodeBarre);" +
                               "INSERT INTO produit (id_articles, id_emplacement, quantite_stock) VALUES (@IdArticles, @IdEmplacement, @Quantite);";

                // Crée une commande SQL avec la requête et la connexion
                MySqlCommand commande = new MySqlCommand(query, ConnectionDB());

                // Ajoute les paramètres à la commande pour eviter les injections SQL
                commande.Parameters.AddWithValue("@IdFournisseur", id_fournisseur);
                commande.Parameters.AddWithValue("@Nom", nom);
                commande.Parameters.AddWithValue("@Famille", famille);
                commande.Parameters.AddWithValue("@PrixHT", prixHT);
                commande.Parameters.AddWithValue("@PrixTTC", prixTTC);
                commande.Parameters.AddWithValue("@PrixAchat", prixAchat);
                commande.Parameters.AddWithValue("@Description", description);
                commande.Parameters.AddWithValue("@CodeBarre", code_barre);

                commande.Parameters.AddWithValue("@IdArticles", id_article);
                commande.Parameters.AddWithValue("@IdEmplacement", id_emplacement);
                commande.Parameters.AddWithValue("@Quantite", quantite);


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


        //***********************************************************************************  supression d'un article  ***************************************************************************************************\\

        public void Delete_From_Achat()
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
        }
    }
}
