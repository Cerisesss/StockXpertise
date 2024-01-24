namespace StockXpertise.Models
{
    public class Fournisseur : BaseModel<Fournisseur>
    {
        public override string TableName => "fournisseur";

        public int id_fournisseur { get; set; }
         public string nom { get; set; }
          public string prenom { get; set; }
         public int numero { get; set; }
         public string mail { get; set; }
          public string adresse { get; set; }
     }

}
