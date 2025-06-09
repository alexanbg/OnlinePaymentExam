using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlinePayments.Models;
using OnlinePayments.Repositories.Base;

namespace OnlinePayments.Repositories.Interfaces
{
    public interface IUserRepository: IBaseRepository<User>
    {
    }
}
