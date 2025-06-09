using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlinePayments.Models;
using OnlinePayments.Repositories.Helpers;
using OnlinePayments.Repositories.Interfaces;
using OnlinePayments.Services.DTOs.Account;
using OnlinePayments.Services.Interfaces.Account;

namespace OnlinePayments.Services.Implementation.Account
{
    public class AccountService : IAccountService
    {
        private IAccountRepository accountRepository;
        private IUserAndAccountRepository userAndAccountRepository;

        public AccountService(IAccountRepository accountRepository, IUserAndAccountRepository userAndAccountRepository)
        {
            this.accountRepository = accountRepository;
            this.userAndAccountRepository = userAndAccountRepository;
        }

        public async Task<ShowAccountsResponse> ShowAccounts(ShowAccountsRequest request)
        {
            if (request.UserId == null)
            {
                return new ShowAccountsResponse()
                {
                    Success = false,
                    Error = "UserId is required"
                };
            }
            Filter filter = new Filter();
            filter.AddCondition("UserId", request.UserId);
            var accounts = await Task.WhenAll((await userAndAccountRepository.RetrieveCollection(filter)).Select(async x => await accountRepository.Retrieve(x.AccountId)));

            return new ShowAccountsResponse()
            {
                Success = true,
                Accounts = await Task.WhenAll(accounts.Select(async x => await MapToAccountInfo(x)))
            };
        }

        public async Task<AccountInfo> MapToAccountInfo(Models.Account account)
        {
            return new AccountInfo()
            {
                AccountId = account.AccountId,
                AccountNumber = account.AccountNumber,
                AvailableSum = account.AvailableSum
            };
        }

        public async Task<int?> GetAccountIdFromNumber(string number)
        {
            Filter filter = new Filter();
            filter.AddCondition("AccountNumber", number);
            var account = (await accountRepository.RetrieveCollection(filter)).FirstOrDefault();


            return account == null ? 0 : account.AccountId;
        }
    }
}
