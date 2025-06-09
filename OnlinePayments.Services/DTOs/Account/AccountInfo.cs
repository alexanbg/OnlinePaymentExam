using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePayments.Services.DTOs.Account
{
    public class AccountInfo
    {
        public int AccountId { get; set; }
        public string AccountNumber { get; set; }
        public decimal AvailableSum { get; set; }
    }
}
