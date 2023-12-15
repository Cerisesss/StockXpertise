using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockXpertise
{
    public class ArticleData
    {
        public string Nom { get; set; }
        public string Famille { get; set; }
        public string CodeBarre { get; set; }
        public string Description { get; set; }
        public int QuantiteStock { get; set; }
        public decimal PrixHT { get; set; }
        public decimal PrixTTC { get; set; }
    }

}
