using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePayments.Services.DTOs.Payments
{
    public class ShowPaymentsResponse
    {
        public bool Success { get; set; }
        public IEnumerable<PaymentInfo>? Payments { get; set; }
        public string? Error { get; set; }
    }
}
