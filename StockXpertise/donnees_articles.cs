using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockXpertise
{
    internal class donnees_articles
    {
        public string Image { get; set; }
        public string Nom { get; set; }
        public string Famille { get; set; }
        public string CodeBarre { get; set; }
        public string Description { get; set; }
        public int Quantite { get; set; }
        public decimal PrixHT { get; set; }
        public decimal PrixTTC { get; set; }
    }
}