using SqlKata.Execution;
using StockXpertise.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockXpertise.Models
{
    public class Article : BaseModel<Article>
    {
        protected override string TableName => "articles"; // Override table name
        
        // Properties representing table columns
        public int id_articles { get; set; }
        public int id_fournisseur { get; set; }
        public string Nom { get; set; }
        public string Famille { get; set; }
        public int PrixHt { get; set; }
        public int PrixTtc { get; set; }
        public int PrixVente { get; set; }
        public int PrixAchat { get; set; }
        public string Description { get; set; }
        public DateTime DateAjout { get; set; }
        public string CodeBarre { get; set; }
        public string Image { get; set; }

    }
}
