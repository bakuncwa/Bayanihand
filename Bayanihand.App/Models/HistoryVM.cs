using System.ComponentModel.DataAnnotations;

namespace Bayanihand.App.Models
{
    public class HistoryVM
    {
        public decimal WageRate { get; set; }
        public decimal ServiceCharge { get; set; }
        public decimal MaterialCost { get; set; }
        public decimal TotalPrice { get; set; }
        public string PaymentOption { get; set; } = string.Empty;
        public string ScheduleDetails { get; set; } = string.Empty;
        public string ServiceDetails { get; set; } = string.Empty;
        public string Visit { get; set; } = string.Empty;
        public string HandymanPhoneNo { get; set; } = string.Empty;

        [Range(1, 5, ErrorMessage = "Please select a rating between 1 and 5.")]
        public int? Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public int CartCommissionId { get; internal set; }
        public int RatingValue { get; set; }
    }
}
