using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using OnlinePayments.Models;
using OnlinePayments.Repositories.Base;
using OnlinePayments.Repositories.Interfaces;

namespace OnlinePayments.Repositories.Implementation
{
    public class UserAndAccountRepository: BaseRepository<UserAndAccount>, IUserAndAccountRepository
    {
        protected override string GetIdCollumnName()
        {
            return "UserAndAccountId";
        }

        protected override string GetProperties()
        {
            return "UserId, AccountId";
        }

        protected override string GetTableName()
        {
            return "UsersAndAccounts";
        }

        protected override UserAndAccount MapEntity(SqlDataReader record)
        {
            var newFavourite = new UserAndAccount()
            {
                UserAndAccountId = Convert.ToInt32(record["UserAndAccountId"]),
                UserId = Convert.ToInt32(record["UserId"]),
                AccountId = Convert.ToInt32(record["AccountId"])
            };
            return newFavourite;
        }
    }
}
