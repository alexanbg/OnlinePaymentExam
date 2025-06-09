using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePayments.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }

        [Required(ErrorMessage = "SendingAccountId is required")]
        public int SendingAccountId { get; set; }

        [Required(ErrorMessage = "ReceivingAccountId is required")]
        public int ReceivingAccountId { get; set; }

        [Required(ErrorMessage = "Sum is required")]
        public decimal Sum {  get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(32, ErrorMessage = "Description must be up to 32 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [Range(0,4,ErrorMessage = "Status must be 1, 2 or 3", MaximumIsExclusive = true, MinimumIsExclusive = true)]
        public byte Status { get; set; }

        [Required(ErrorMessage = "CreationDate is required")]
        [DataType(DataType.DateTime)]
        public DateTime CreationDate { get; set; }
    }
}
