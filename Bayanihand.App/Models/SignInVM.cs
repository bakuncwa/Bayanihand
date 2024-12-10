using System.ComponentModel.DataAnnotations;

namespace Bayanihand.App.Models
{
    public class SignInVM
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        public string ReturnUrl { get; set; } = string.Empty;
    }
}
