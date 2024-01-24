using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
