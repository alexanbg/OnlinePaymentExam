using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePayments.Services.DTOs.Payments
{
    public class CancelPaymentRequest
    {
        public int PaymentId { get; set; }
    }
}
