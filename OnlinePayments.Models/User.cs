using System.ComponentModel.DataAnnotations;

namespace OnlinePayments.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required (ErrorMessage = "FullName is required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
