using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockXpertise
{
    public class ImageInfo
    {
        public string nom_article { get; set; }
        public string Famille { get; set; }
        public string CodeBarre { get; set; }
        public string Description { get; set; }
        public int PrixHT { get; set; }
        public int PrixTTC { get; set; }
        public int quantite { get; set; }
        public string image_path { get; set; }
    }
}
