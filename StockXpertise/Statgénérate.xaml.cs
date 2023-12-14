using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        public string affichemod { get; set; }
        public string date { get; set; }



        public Statgénérate()
        {
        InitializeComponent();            
        }

/*        public void GenerateTable()
        {
            string query = "select * from articles";
            ConfigurationDB.ExecuteQuery(query);

        }*/
        public void GenerateTable()
        {
            string query="SELECT ";
            if (PrixAchat)
            {
                query += "prix_achat, ";
            }
            if (PrixVente)
            {
                query += "prix_achat, "; //futurprixvente a modifié quand lucas a push les nv db
            }
            if (ArticlesVendus)
            {
                query += "(SELECT COUNT(*) FROM ventes) AS total_ventes, ";

            }
            if (Marge)
            {
                query += "(prix_achat * (SELECT COUNT(*) FROM ventes)) - (prix_achat * prix_achat) AS marge, ";
            }
            if (!PrixAchat && !PrixVente && !ArticlesVendus && !Marge)
            {
                query += "* ,";
            }

            if (query.Length >= 2)
            {
                query = query.Substring(0, query.Length - 2);
                Console.WriteLine(query); // Affiche "Exem"
            }



            query += " FROM articles";
            if (Marge)
            {
                query += " ORDER BY marge DESC";
            }

            if (Top10Produits)
            {
                query += " LIMIT 1";
            }

            MySqlDataReader result = ConfigurationDB.ExecuteQuery(query);

            dataGrid.ItemsSource = result;
        }

    }

}
