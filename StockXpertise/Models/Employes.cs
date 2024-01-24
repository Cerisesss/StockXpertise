namespace StockXpertise.Models
{
    public class Employes : BaseModel<Employes>
    {
        public override string TableName => "employes";

        public int id_employes { get; set; }
         public string nom { get; set; }
          public string prenom { get; set; }
          public string mot_de_passe { get; set; }
          public string mail { get; set; }
          public string role { get; set; }
         public DateTime date { get; set; }
    }

}
