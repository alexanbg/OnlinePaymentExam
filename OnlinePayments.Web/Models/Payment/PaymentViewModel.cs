using System.ComponentModel.DataAnnotations;

public class PaymentViewModel
{
    public int PaymentId { get; set; }
    public int SendingAccountId { get; set; }
    public string SendingAccountNumber { get; set; }
    public int ReceivingAccountId { get; set; }
    public string ReceivingAccountNumber { get; set; }
    public decimal Sum { get; set; }
    public string Description { get; set; }
    public byte Status { get; set; }
    public string StatusName { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime CreationDate { get; set; }
}

public class PaymentViewModelList
{
    public List<PaymentViewModel> Payments { get; set; }
}
