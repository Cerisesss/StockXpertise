using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockXpertise
{
    public class DataInventaire
    {
        public int Id_produit { get; set; }
        public string Nom { get; set; }
        public int Quantite_stock { get; set; }
        public string Code { get; set; }
    }
}
