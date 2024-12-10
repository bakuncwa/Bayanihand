using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayanihand.DataModel
{
    public class CartCommission
    {
        public int Id { get; set; }

        public decimal WageRate { get; set; }
        public decimal ServiceCharge { get; set; }

        public decimal MaterialCost { get; set; }

        public decimal TotalPrice { get; set; }

        public string PaymentOption { get; set; } = string.Empty;

        public int? ServiceId { get; set; }
        public virtual Service Service { get; set; }

        public int? ScheduleId { get; set; }
        public virtual Schedule Schedule { get; set; }
        public int? CartId { get; set; }
        public virtual Cart Cart { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }

        public string PhoneNo { get; set; } = string.Empty ;

    }
}
