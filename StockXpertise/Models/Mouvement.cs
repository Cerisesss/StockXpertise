namespace StockXpertise.Models
{
    public class Mouvement : BaseModel<Mouvement>
    {
        public override string TableName => "mouvement";

        public int id_mouvement { get; set; }
        public int id_employes { get; set; }
        public DateTime date { get; set; }
    }

}
