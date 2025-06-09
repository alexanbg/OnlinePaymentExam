using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePayments.Models
{
    public class Account
    {
        public int AccountId { get; set; }

        [Required(ErrorMessage = "AccountNumber is required")]
        public string AccountNumber { get; set; }

        [Required(ErrorMessage = "AvailableSum is required")]
        [Range(0, int.MaxValue, ErrorMessage ="Available sum must be above 0")]
        public decimal AvailableSum { get; set; }

        
    }
}
