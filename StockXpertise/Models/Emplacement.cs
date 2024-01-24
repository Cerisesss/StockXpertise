namespace StockXpertise.Models
{
    public class Emplacement : BaseModel<Emplacement>
    {
        public override string TableName => "emplacement";

        public int id_emplacement { get; set; }
         public string code { get; set; }
         public int capacite { get; set; }
         public string code_reel { get; set; }
     }

}
