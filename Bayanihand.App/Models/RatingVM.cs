using Bayanihand.DataModel;
using System.ComponentModel.DataAnnotations;

namespace Bayanihand.App.Models
{
    public class RatingVM
    {
        public int Id { get; set; }

        public int RatingValue { get; set; }

        public string Comment { get; set; }

        public virtual CartCommission CartCommission { get; set; }

        public virtual int CartCommissionId { get; set; }
    }
}

