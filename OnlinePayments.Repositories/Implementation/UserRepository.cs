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
    public class UserRepository: BaseRepository<User>, IUserRepository
    {
        protected override string GetIdCollumnName()
        {
            return "UserId";
        }

        protected override string GetProperties()
        {
            return "FullName, Username, Password";
        }

        protected override string GetTableName()
        {
            return "Users";
        }

        protected override User MapEntity(SqlDataReader record)
        {
            var newFavourite = new User()
            {
                UserId = Convert.ToInt32(record["UserId"]),
                FullName = record["FullName"].ToString(),
                Username = record["Username"].ToString(),
                Password = record["Password"].ToString(),
            };
            return newFavourite;
        }
    }
}
