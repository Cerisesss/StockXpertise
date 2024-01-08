using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockXpertise.Stock
{
    public class DataInventaire
    {
        public int Id_produit { get; set; }
        public string Nom { get; set; }
        public int Quantite_stock { get; set; }
        public int Quantite_stock_reel { get; set; }
        public string Code { get; set; }
        public string Code_reel { get; set; }
    }
}
