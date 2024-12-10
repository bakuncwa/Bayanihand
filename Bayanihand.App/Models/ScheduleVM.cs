using System.ComponentModel.DataAnnotations;

namespace Bayanihand.App.Models
{
    public class ScheduleVM
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
