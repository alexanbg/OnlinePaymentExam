using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlinePayments.Models;
using OnlinePayments.Services.DTOs.Account;

namespace OnlinePayments.Services.Interfaces.Account
{
    public interface IAccountService
    {
        public Task<AccountInfo> MapToAccountInfo(Models.Account account);
        public Task<ShowAccountsResponse> ShowAccounts(ShowAccountsRequest request);
        public Task<int?> GetAccountIdFromNumber(string number);
    }
}
