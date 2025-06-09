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
    public class PaymentRepository: BaseRepository<Payment>, IPaymentRepository
    {
        protected override string GetIdCollumnName()
        {
            return "PaymentId";
        }

        protected override string GetProperties()
        {
            return "SendingAccountId, ReceivingAccountId, Sum, Description, Status, CreationDate";
        }

        protected override string GetTableName()
        {
            return "Payments";
        }

        protected override Payment MapEntity(SqlDataReader record)
        {
            var newFavourite = new Payment()
            {
                PaymentId = Convert.ToInt32(record["PaymentId"]),
                SendingAccountId = Convert.ToInt32(record["SendingAccountId"]),
                ReceivingAccountId = Convert.ToInt32(record["ReceivingAccountId"]),
                Status = Convert.ToByte(record["Status"]),
                Description = record["Description"].ToString(),
                Sum = Convert.ToDecimal(record["Sum"]),
                CreationDate = Convert.ToDateTime(record["CreationDate"])
            };
            return newFavourite;
        }
    }
}
