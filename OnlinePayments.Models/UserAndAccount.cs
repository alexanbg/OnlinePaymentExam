using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePayments.Models
{
    public class UserAndAccount
    {
        public int UserAndAccountId { get; set; }

        [Required(ErrorMessage = "UserId is required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "AccountId is required")]
        public int AccountId { get; set; }
    }
}
