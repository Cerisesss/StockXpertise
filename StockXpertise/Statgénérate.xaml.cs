using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
<<<<<<< HEAD
using MySql.Data.MySqlClient;
=======
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Runtime.Remoting.Messaging;

>>>>>>> 7b7e258 (l'ajout de fonction pour faire un excel a partir du tableau)

namespace StockXpertise
{
    public partial class Statgénérate : Page
    {
        public bool PrixAchat { get; set; }
        public bool PrixVente { get; set; }
        public bool ArticlesVendus { get; set; }
        public bool Marge { get; set; }
        public bool Top10Produits { get; set; }
        public bool StockNegatif { get; set; }
        public bool TotalVentes { get; set; }
        public string date { get; set; }

        public Statgénérate()
        {
            InitializeComponent();
        }

        static void toexcel()
        {
            string command = "python"; // Commande pour exécuter Python
            string scriptPath = "txttoexcel.py"; // Chemin vers ton script Python

            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = command;
            start.Arguments = scriptPath;
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;

            using (Process process = Process.Start(start))
            {
                using (System.IO.StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    Console.Write(result);
                }
            }
        }
        static void ConvertDataReaderToTxt(MySqlDataReader reader)
        {
            string outputPath = "lecturexel1.txt";
            using (StreamWriter writer = new StreamWriter(outputPath, false, System.Text.Encoding.GetEncoding("ISO-8859-1")))
            {
                // Écriture des en-têtes de colonnes
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    writer.Write(reader.GetName(i));
                    if (i < reader.FieldCount - 1)
                    {
                        writer.Write(",");
                    }
                }
                writer.WriteLine();



                // Écriture des données
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        writer.Write(reader[i]);
                        if (i < reader.FieldCount - 1)
                        {
                            writer.Write(",");
                        }
                    }
                    writer.WriteLine();
                } 
            }
            toexcel();
        }



        public void GenerateTable()
        {
            string query="SELECT ";
            if (PrixAchat)
            {
                query += "prix_achat, ";
            }
            if (PrixVente)
            {
                query += "prix_ventes, ";
            }
            if (ArticlesVendus)
            {
                query += "(SELECT COUNT(*) FROM vente where id_articles=id_vente) AS total_ventes, ";

            }
            if (Marge)
            {
                query += "(prix_ventes * (SELECT COUNT(*) FROM vente where id_articles=id_vente) - (prix_achat * (SELECT COUNT(*) FROM vente where id_articles=id_vente))) as marge, ";
            }
           // query += "(select sum(quantite_stock) from produit where id_articles = id_produit) as stock, ";  attendre qu'il y ai du peuplement, parce que il ne peut pas ne pas y avoir de stock sinon la commande bug
            if (!PrixAchat && !PrixVente && !ArticlesVendus && !Marge)
            {
                query += "nom, famille, prix_ht ,prix_ttc ,prix_vente ,prix_achat ,description ,code_barre, ";
            }

            if (query.Length >= 2)
            {
                query = query.Substring(0, query.Length - 2);
                Console.WriteLine(query);
            }



            query += " from articles inner join vente on id_articles = id_produit";

            string date_code = "Day";
            if (date == "Semaine")
            {
                date_code = "* 7 day";
            }
            if (date == "Mois")
            {
                date_code = "month";
            }
            if (date == "Année")
            {
                date_code = "year";
            }

            query += " WHERE date_vente >= DATE_SUB(CURDATE(), INTERVAL 1 "+ date_code +") ";

            if (StockNegatif) {
                query += "AND (SELECT SUM(quantite_stock) FROM produit WHERE id_articles = id_produit) > 10 ";
            }

            if (Marge)
            {
                query += " ORDER BY marge DESC";
            }

            if (Top10Produits)
            {
                query += " LIMIT 10";
            }

            MySqlDataReader result = ConfigurationDB.ExecuteQuery(query);
            ConvertDataReaderToTxt(result);
            result = ConfigurationDB.ExecuteQuery(query);
            dataGrid.ItemsSource = result;
            


            //  ________________________________________________________________________________________________________

            if (PrixAchat || PrixVente || ArticlesVendus || Marge)
            {
                string query2 = "SELECT ";
                if (PrixAchat)
                {
                    query2 += "SUM(prix_achat) AS total_prix_achat, ";
                }
                if (PrixVente)
                {
                    query2 += "SUM(prix_ventes) AS total_prix_ventes, ";
                }
                if (ArticlesVendus)
                {
                    query2 += "SUM((SELECT COUNT(*) FROM vente WHERE id_articles = id_vente)) AS total_ventes, ";

                }
                if (Marge)
                {
                    query2 += "SUM(prix_ventes * (SELECT COUNT(*) FROM vente WHERE id_articles = id_vente) - (prix_achat * (SELECT COUNT(*) FROM vente WHERE id_articles = id_vente))) AS total_marge, ";
                }
                if (!PrixAchat && !PrixVente && !ArticlesVendus && !Marge)
                {
                    query2 += "* ,";
                }

                if (query.Length >= 2)
                {
                    query2 = query2.Substring(0, query2.Length - 2);
                    Console.WriteLine(query2); // Affiche "Exem"
                }



                query2 += " from articles inner join vente on id_articles = id_produit";
                if (Marge)
                {
                    query2 += " ORDER BY total_marge DESC";
                }

                if (Top10Produits)
                {
                    query2 += " LIMIT 10";
                }

                MySqlDataReader result2 = ConfigurationDB.ExecuteQuery(query2);

                //dataGrid2.ItemsSource = result2;
            }
            else {
                //dataGrid2.Visibility = Visibility.Hidden;
            }
        }
    }
}
