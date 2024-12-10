using Bayanihand.DataModel;

namespace Bayanihand.App.Models
{
    public class ServiceVM
    {
        public int Id { get; set; }

        public string ServiceName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public bool Available { get; set; }

        public decimal WageRate => 76;
        public decimal ServiceCharge { get; set; }

        public decimal MaterialCost { get; set; }

        public decimal TotalPrice => WageRate + ServiceCharge + MaterialCost;

        public string PaymentOption { get; set; } = string.Empty;

    }
}
