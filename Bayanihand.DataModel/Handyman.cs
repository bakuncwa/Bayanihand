using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayanihand.DataModel
{
    public class Handyman
    {
        public int Id { get; set; }
        public string PhoneNo { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string BarangayNo { get; set; } = string.Empty;
        public string BarangayName { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string ZIPCode { get; set; } = string.Empty;
        public decimal ServiceCharge { get; set; }
        public int YearsOfExperience { get; set; }

    }
}
