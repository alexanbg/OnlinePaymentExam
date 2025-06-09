using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;

public class CreatePaymentViewModel
{
    [Required]
    public int SendingAccountId { get; set; }

    [Required]
    [StringLength(9, MinimumLength = 9, ErrorMessage = "The account number must be 9 characters long")]
    public string ReceivingAccountNumber { get; set; }
    public List<SelectListItem> UserAccounts { get; set; }

    [RegularExpression(@"^\d+(\.\d{1,2})?$",ErrorMessage = "Invalid sum")]
    [Range(0.01, 9999999999999d, ErrorMessage = "Sum must be between 0.01 and 9999999999999")]
    public decimal Sum {  get; set; }

    [StringLength(32, ErrorMessage = "The Description must be up to 32 characters long")]
    public string Description { get; set; }
}
