using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayanihand.DataModel
{
    public class Rating
    {
        public int Id { get; set; }

        public int RatingValue { get; set; }

        public string Comment { get; set; } = string.Empty;

        public virtual CartCommission CartCommission { get; set; }

        public virtual int CartCommissionId { get; set; }
    }
}
