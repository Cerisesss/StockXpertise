using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockXpertise.Models
{
    public class Mouvement : BaseModel<Mouvement>
    {
        protected override string TableName => "mouvement";

        public int id_mouvement { get; set; }
        public int id_employes { get; set; }
        public DateTime date { get; set; }
    }

}
