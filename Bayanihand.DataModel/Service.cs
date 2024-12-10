using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayanihand.DataModel
{
    public class Service
    {
        public int Id { get; set; }

        public string ServiceName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public bool Available { get; set; }

        public decimal WageRate { get; set; }
        public decimal ServiceCharge { get; set; }

        public decimal MaterialCost { get; set; }

        public decimal TotalPrice { get; set; }

        public string PaymentOption { get; set; } = string.Empty;

    }
}
