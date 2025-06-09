using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePayments.Services.DTOs.Payments
{
    public class PaymentInfo
    {
        public int PaymentId { get; set; }
        public int SendingAccountId { get; set; }
        public string SendingAccountNumber { get; set; }
        public int ReceivingAccountId { get; set; }
        public string ReceivingAccountNumber { get; set; }
        public string Description { get; set; }
        public decimal Sum {  get; set; }
        public byte Status { get; set; }
        public string StatusName { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreationDate { get; set; }
    }
}
