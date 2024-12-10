using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayanihand.DataModel
{
    public class Schedule
    {
        [Key]
        public int Id { get; set; }

        public string Visit { get; set; } = string.Empty;

        public string HandymanPhoneNo { get; set; } = string.Empty;

        public string MeetUpLocation { get; set; } = string.Empty;

        public string MeetUpDay { get; set; } = string.Empty;

        public string MeetUpTime { get; set; } = string.Empty;

        public DateTime DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}
