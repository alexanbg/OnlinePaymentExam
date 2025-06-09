using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using OnlinePayments.Models;
using OnlinePayments.Repositories.Base;
using OnlinePayments.Repositories.Helpers;
using OnlinePayments.Repositories.Interfaces;

namespace OnlinePayments.Repositories.Implementation
{
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        protected override string GetIdCollumnName()
        {
            return "AccountId";
        }

        protected override string GetProperties()
        {
            return "AccountNumber, AvailableSum";
        }

        protected override string GetTableName()
        {
            return "Accounts";
        }

        protected override Account MapEntity(SqlDataReader record)
        {
            var newFavourite = new Account()
            {
                AccountId = Convert.ToInt32(record["AccountId"]),
                AccountNumber = record["AccountNumber"].ToString(),
                AvailableSum = Convert.ToDecimal(record["AvailableSum"])
            };
            return newFavourite;
        }
    }
}
