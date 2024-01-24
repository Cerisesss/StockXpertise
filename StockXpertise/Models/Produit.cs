using SqlKata.Execution;
using StockXpertise.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockXpertise.Models
{
    public class Produit : BaseModel<Produit>
    {
        public override string TableName => "produit";

        public int id_produit { get; set; }
        public ForeignKey<Article> id_articles { get; set; }
        public int quantite_stock { get; set; }
        public int quantite_stock_reel { get; set; }
        public int id_emplacement { get; set; }
    }

}
