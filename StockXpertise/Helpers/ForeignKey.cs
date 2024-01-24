using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockXpertise.Helpers
{
    public class ForeignKey<T>
    {
        public int Id { get; set; }

        public static implicit operator int(ForeignKey<T> foreignKey)
        {
            return foreignKey.Id;
        }

        public static explicit operator ForeignKey<T>(int id)
        {
            return new ForeignKey<T> { Id = id };
        }

    }
}
