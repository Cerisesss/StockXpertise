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
        int id;
        string nom;
        string prenom;
        int numero;
        string mail;
        string adresse;

        public Query_Fournisseur(string nom, string prenom, int numero, string mail, string adresse) : this(0, nom, prenom, numero, mail, adresse)
        {
        }

        public Query_Fournisseur(int id, string nom, string prenom, int numero, string mail, string adresse)
        {
            this.id = id;
            this.nom = nom;
            this.prenom = prenom;
            this.numero = numero;
            this.mail = mail;
            this.adresse = adresse;
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
                commande.Parameters.AddWithValue("@nom", nom);
                commande.Parameters.AddWithValue("@prenom", prenom);
                commande.Parameters.AddWithValue("@numero", numero);
                commande.Parameters.AddWithValue("@mail", mail);
                commande.Parameters.AddWithValue("@adresse", adresse);



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

        //******************************************************************************  Update fournisseur ****************************************************************************************************

        public void Update_Supplier()
        {
            MySqlDataReader reader;

            try
            {
                // Requête SQL paramétrée
                string query = "UPDATE fournisseur SET nom = @Nom, prenom = @Prenom, numero = @Numero, mail = @Mail, adresse = @Adresse WHERE id_fournisseur = @Id ;";

                // Crée une commande SQL avec la requête et la connexion
                MySqlCommand commande = new MySqlCommand(query, ConnectionDB());

                // Ajoute les paramètres à la commande pour eviter les injections SQL
                commande.Parameters.AddWithValue("@Nom", nom);
                commande.Parameters.AddWithValue("@Prenom", prenom);
                commande.Parameters.AddWithValue("@Numero", numero);
                commande.Parameters.AddWithValue("@Mail", mail);
                commande.Parameters.AddWithValue("@Adresse", adresse);
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


        //***********************************************************************************  delete fournisseur  ********************************************************************************************************\\

        public void Delete_Supplier()
        {
            MySqlDataReader reader;

            try
            {
                Delete_From_Articles();

                // Requête SQL paramétrée
                string query = "DELETE FROM fournisseur WHERE id_fournisseur = @Id; ";

                // Crée une commande SQL avec la requête et la connexion
                MySqlCommand command = new MySqlCommand(query, ConnectionDB());

                // Ajoute les paramètres à la commande pour eviter les injections SQL
                command.Parameters.AddWithValue("@Id", id);

                // Exécute la commande
                reader = command.ExecuteReader();

                //message de confirmation
                MessageBox.Show("Supprimé avec succès.");

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in ConnectionDB: {ex.Message}");
            }
        }

           public void Delete_From_Articles()
           {
               MySqlDataReader reader;

               //supprimer de la table achat
               try
               {
                   string query = "SELECT * FROM articles WHERE id_fournisseur = @Id ;";
                   MySqlCommand commande = new MySqlCommand(query, ConnectionDB());

                   commande.Parameters.AddWithValue("@Id", id);

                   reader = commande.ExecuteReader();

                   if (reader.HasRows)
                   {
                       while (reader.Read())
                       {
                           string query_delete = "DELETE FROM articles WHERE id_fournisseur = @Id;";
                           MySqlCommand commande_sql = new MySqlCommand(query_delete, ConnectionDB());

                           commande_sql.Parameters.AddWithValue("@Id", id);

                           commande_sql.ExecuteReader();
                       }
                   }
               }
               catch (Exception ex)
               {
                   MessageBox.Show($"Error in ConnectionDB: {ex.Message}");
               }
           }
    }
}
