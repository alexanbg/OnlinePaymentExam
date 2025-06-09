using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlinePayments.Repositories.Helpers;

namespace OnlinePayments.Repositories.Base
{
    public interface IBaseRepository<T> where T : class
    {
        Task<int> Create(T entity);
        Task<T> Retrieve(int objectId);
        Task<IEnumerable<T>> RetrieveCollection(Filter filter);
        Task<bool> Update(int objectId, UpdateCommand update);
        Task<bool> Delete(int objectId);
    }
}
