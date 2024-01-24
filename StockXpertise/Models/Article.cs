using StockXpertise.Helpers;

namespace StockXpertise.Models
{
    public class Article : BaseModel<Article>
    {
        public override string TableName => "articles"; // Override table name

        // Properties representing table columns
        public int id_articles { get; set; }
        public ForeignKey<Fournisseur> id_fournisseur { get; set; }
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
