namespace StockXpertise.Models
{
    public class Achat : BaseModel<Achat>
    {
        public override string TableName => "achat";

        public int id_achat { get; set; }
        public int id_produit { get; set; }
        public int prix_achat { get; set; }
        public DateTime date_achat { get; set; }
         public string facture { get; set; }
         public int id_mouvement { get; set; }
    }

}
