using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayanihand.DataModel
{
    public class Cart
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public ICollection<CartCommission> Items { get; set; } = new List<CartCommission>();
    }
}
