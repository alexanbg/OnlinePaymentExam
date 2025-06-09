using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePayments.Services.DTOs.Account
{
    public class ShowAccountsResponse
    {
        public bool Success { get; set; }
        public IEnumerable<AccountInfo>? Accounts { get; set; }
        public string? Error { get; set; }
    }
}
