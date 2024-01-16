using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockXpertise.Stock
{
    public class Article
    {
        public int Id { get; set; }
        public int Id_emplacement { get; set; }
        public string ImagePath { get; set; }
        public string Code_emplacement { get; set; }
        public string Nom { get; set; }
        public string Description { get; set; }
        public int Quantite { get; set; }
        public string Famille { get; set; }
        public string CodeBarre { get; set; }
        public string Image { get; set; }
        public int PrixHT { get; set; }
        public int PrixTTC { get; set; }
    }
}