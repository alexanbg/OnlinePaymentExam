using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePayments.Services.DTOs.Payments
{
    public class CreatePaymentRequest
    {
        public int SenderAccountId { get; set; }
        public int ReceiverAccountId { get; set; }
        public decimal Sum {  get; set; }
        public string Description { get; set; }
    }
}
