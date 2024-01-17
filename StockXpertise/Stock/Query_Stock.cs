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
        string imagePath;

        public Query_Stock(int id_article, int quantite, string emplacement) : this(id_article, "", "", 0, 0, 0, "", "", quantite, emplacement, "", "")
        {
            this.emplacement = emplacement;
        }

        public Query_Stock(int quantite, string emplacement, int id_produit) : this(0, "", "", 0, 0, 0, "", "", quantite, "", "", "")
        {
            this.emplacement = emplacement;
            this.id_produit = id_produit;
        }

        public Query_Stock(int id_article, string nom, string famille, int prixHT, int prixTTC, string description, string code_barre, int quantite, string imagePath, string emplacement, int id_emplacement) : this(id_article, nom, famille, prixHT, prixTTC, 0, description, code_barre, quantite, "", "", "")
        {
            this.imagePath = imagePath;
            this.emplacement = emplacement;
            this.id_emplacement = id_emplacement;
        }

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

        //***********************************************************************************  update article  ********************************************************************************************************\\

        public void Update_Name()
        {
            try
            {
                string query = "UPDATE articles SET nom = @Nom WHERE id_articles = @Id_Article;";

                // Crée une commande SQL avec la requête et la connexion
                MySqlCommand commande = new MySqlCommand(query, ConnectionDB());

                // Ajoute les paramètres à la commande pour eviter les injections SQL
                commande.Parameters.AddWithValue("@Nom", nom);
                commande.Parameters.AddWithValue("@Id_Article", id_article);

                // Exécute la commande
                commande.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in ConnectionDB: {ex.Message}");
            }
        }

        public void Update_Famille()
        {
            try
            {
                string query = "UPDATE articles SET famille = @Famille WHERE id_articles = @Id_Article;";

                // Crée une commande SQL avec la requête et la connexion
                MySqlCommand commande = new MySqlCommand(query, ConnectionDB());

                // Ajoute les paramètres à la commande pour eviter les injections SQL
                commande.Parameters.AddWithValue("@Famille", famille);
                commande.Parameters.AddWithValue("@Id_Article", id_article);

                // Exécute la commande
                commande.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in ConnectionDB: {ex.Message}");
            }
        }

        public void Update_Description()
        {
            try
            {
                string query = "UPDATE articles SET description = @Description WHERE id_articles = @Id_Article;" ;

                // Crée une commande SQL avec la requête et la connexion
                MySqlCommand commande = new MySqlCommand(query, ConnectionDB());

                // Ajoute les paramètres à la commande pour eviter les injections SQL
                commande.Parameters.AddWithValue("@Description", description);
                commande.Parameters.AddWithValue("@Id_Article", id_article);

                // Exécute la commande
                commande.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in ConnectionDB: {ex.Message}");
            }
        }

        public void Update_Quantite()
        {
            try
            {
                //modif 
                string query = "UPDATE produit SET quantite_stock = @Quantite WHERE id_articles = @Id_Article AND id_emplacement = @Id_Emplacement ;";

                // Crée une commande SQL avec la requête et la connexion
                MySqlCommand commande = new MySqlCommand(query, ConnectionDB());

                // Ajoute les paramètres à la commande pour eviter les injections SQL
                commande.Parameters.AddWithValue("@Quantite", quantite);
                commande.Parameters.AddWithValue("@Id_Article", id_article);
                commande.Parameters.AddWithValue("@Id_Emplacement", id_emplacement);

                // Exécute la commande
                commande.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in ConnectionDB: {ex.Message}");
            }
        }

        public void Update_PrixHT()
        {
            try
            {
                string query = "UPDATE articles SET prix_ht = @PrixHT WHERE id_articles = @Id_Article;";

                // Crée une commande SQL avec la requête et la connexion
                MySqlCommand commande = new MySqlCommand(query, ConnectionDB());

                // Ajoute les paramètres à la commande pour eviter les injections SQL
                commande.Parameters.AddWithValue("@PrixHT", prixHT);
                commande.Parameters.AddWithValue("@Id_Article", id_article);

                // Exécute la commande
                commande.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in ConnectionDB: {ex.Message}");
            }
        }

        public void Update_PrixTTC()
        {
            try
            {
                string query = "UPDATE articles SET prix_ttc = @PrixTTC WHERE id_articles = @Id_Article;";

                // Crée une commande SQL avec la requête et la connexion
                MySqlCommand commande = new MySqlCommand(query, ConnectionDB());

                // Ajoute les paramètres à la commande pour eviter les injections SQL
                commande.Parameters.AddWithValue("@PrixTTC", prixTTC);
                commande.Parameters.AddWithValue("@Id_Article", id_article);

                // Exécute la commande
                commande.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in ConnectionDB: {ex.Message}");
            }
        }

        public void Update_CodeBarre()
        {
            try
            {
                string query = "UPDATE articles SET code_barre = @CodeBarre WHERE id_articles = @Id_Article;";

                // Crée une commande SQL avec la requête et la connexion
                MySqlCommand commande = new MySqlCommand(query, ConnectionDB());

                // Ajoute les paramètres à la commande pour eviter les injections SQL
                commande.Parameters.AddWithValue("@CodeBarre", code_barre);
                commande.Parameters.AddWithValue("@Id_Article", id_article);

                // Exécute la commande
                commande.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in ConnectionDB: {ex.Message}");
            }
        }

        public void Update_ImagePath()
        {
            try
            {
                string query = "UPDATE articles SET image = @ImagePath WHERE id_articles = @Id_Article;";

                // Crée une commande SQL avec la requête et la connexion
                MySqlCommand commande = new MySqlCommand(query, ConnectionDB());

                // Ajoute les paramètres à la commande pour eviter les injections SQL
                commande.Parameters.AddWithValue("@ImagePath", imagePath);
                commande.Parameters.AddWithValue("@Id_Article", id_article);

                // Exécute la commande
                commande.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in ConnectionDB: {ex.Message}");
            }
        }

        public void Update_Emplacement()
        {
            MySqlDataReader reader;
            MySqlDataReader reader_id_emplacement;
            int IdEmplacement = 0;


            try
            {
                //recup id_emplacement
                string query_emplacement = "SELECT * FROM emplacement WHERE code = @Emplacement;";

                MySqlCommand command = new MySqlCommand(query_emplacement, ConnectionDB());

                command.Parameters.AddWithValue("@Emplacement", emplacement);

                reader = command.ExecuteReader();

                if (!reader.HasRows)
                {
                    //ajout du nouveau emplacement
                    string query_add_emplacement = "INSERT INTO emplacement (code) VALUES (@Emplacement)";
                    MySqlCommand commande_sql = new MySqlCommand(query_add_emplacement, ConnectionDB());

                    commande_sql.Parameters.AddWithValue("@Emplacement", emplacement);

                    commande_sql.ExecuteReader();

                    MessageBox.Show("L'emplacement n'existe pas, il a été ajouté.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

                    //recup l'id de l'emplacement

                    string query_id_emplacement = "SELECT * FROM emplacement WHERE code = @Emplacement ;";

                    MySqlCommand query_sql = new MySqlCommand(query_id_emplacement, ConnectionDB());

                    query_sql.Parameters.AddWithValue("@Emplacement", emplacement);

                    reader_id_emplacement = query_sql.ExecuteReader();

                    while (reader_id_emplacement.Read())
                    {
                        IdEmplacement = Convert.ToInt32(reader_id_emplacement["id_emplacement"]);
                    }

                }
                else
                {
                    while (reader.Read())
                    {
                         IdEmplacement = Convert.ToInt32(reader["id_emplacement"]);
                    }
                }

                string query = "UPDATE produit SET id_emplacement = @IdEmplacement WHERE id_articles = @Id_Article AND quantite_stock = @Quantite;";

                // Crée une commande SQL avec la requête et la connexion
                MySqlCommand commande = new MySqlCommand(query, ConnectionDB());

                // Ajoute les paramètres à la commande pour eviter les injections SQL
                commande.Parameters.AddWithValue("@IdEmplacement", IdEmplacement);
                commande.Parameters.AddWithValue("@Id_Article", id_article);
                commande.Parameters.AddWithValue("@Quantite", quantite);


                // Exécute la commande
                commande.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in ConnectionDB: {ex.Message}");
            }
        }

        public int Count_IdEmplacement()
        { 
            MySqlDataReader reader;
            int count = 0;

            try
            {
                string query = "SELECT id_emplacement FROM emplacement DESC;";

                MySqlCommand commande = new MySqlCommand(query, ConnectionDB());

                reader = commande.ExecuteReader();

                while (reader.Read())
                {
                    count = Convert.ToInt32(reader["id_emplacement"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in ConnectionDB: {ex.Message}");
            }

            return count + 1;
        }


        //***********************************************************************************  update inventaire  ***************************************************************************************************\\

        public void Update_Quantite_Reel()
        {
            try
            {
                string query = "UPDATE produit SET quantite_stock_reel = @Quantite WHERE id_produit = @Id_Produit;";

                // Crée une commande SQL avec la requête et la connexion
                MySqlCommand commande = new MySqlCommand(query, ConnectionDB());

                // Ajoute les paramètres à la commande pour eviter les injections SQL
                commande.Parameters.AddWithValue("@Quantite", quantite);
                commande.Parameters.AddWithValue("@Id_Produit", id_produit);

                // Exécute la commande
                commande.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in ConnectionDB: {ex.Message}");
            }
        }

        public void Update_Code_Reel()
        {
            MySqlDataReader reader;

            try
            {
                //recup id_emplacement
                string query_emplacement = "SELECT id_emplacement FROM produit WHERE id_produit = @Id_Produit;";

                MySqlCommand command = new MySqlCommand(query_emplacement, ConnectionDB());

                command.Parameters.AddWithValue("@Id_Produit", id_produit);

                reader = command.ExecuteReader();

                while(reader.Read())
                {
                    id_emplacement = Convert.ToInt32(reader["id_emplacement"]);
                }

                //modif
                string query = "UPDATE emplacement SET code_reel = @Emplacement WHERE id_emplacement = @Id_Emplacement;";

                MySqlCommand commande = new MySqlCommand(query, ConnectionDB());

                commande.Parameters.AddWithValue("@Emplacement", emplacement);
                commande.Parameters.AddWithValue("@Id_Emplacement", id_emplacement);

                commande.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in ConnectionDB: {ex.Message}");
            }
        }

        //*********************************************************************************** add new article  ***************************************************************************************************\\

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
                MessageBox.Show("Ajouté avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in ConnectionDB: {ex.Message}");
            }
        }


        //***********************************************************************************  delete article  ********************************************************************************************************\\

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
                MessageBox.Show("Supprimé avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in ConnectionDB: {ex.Message}");
            }
        }
    }
}
