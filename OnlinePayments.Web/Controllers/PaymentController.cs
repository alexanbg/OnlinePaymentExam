using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlinePayments.Services.DTOs.Account;
using OnlinePayments.Services.DTOs.Payments;
using OnlinePayments.Services.Implementation.Account;
using OnlinePayments.Services.Interfaces.Account;
using OnlinePayments.Services.Interfaces.Payment;

namespace OnlinePayments.Web.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentService paymentService;
        private readonly IAccountService accountService;

        public PaymentController(IPaymentService paymentService, IAccountService accountService)
        {
            this.paymentService = paymentService;
            this.accountService = accountService;
        }

        public async Task<IActionResult> Index(bool sortByStatus = false)
        {
            var userId = HttpContext.Session.GetInt32("UserId").Value;

            var result = await paymentService.ShowPayments(new ShowPaymentsRequest
            {
                UserId = userId,
                SortByStatus = sortByStatus
            });

            if (!result.Success)
            {
                ViewData["ErrorMessage"] = result.Error ?? "Error showing payments";
                return RedirectToAction("Index", "Home");
            }

            var viewModel = new PaymentViewModelList()
            {
                Payments = result.Payments.Select(x => new PaymentViewModel
                {
                    PaymentId = x.PaymentId,
                    CreationDate = x.CreationDate,
                    SendingAccountId = x.SendingAccountId,
                    SendingAccountNumber = x.SendingAccountNumber,
                    Description = x.Description,
                    Status = x.Status,
                    StatusName = x.StatusName,
                    ReceivingAccountId = x.ReceivingAccountId,
                    ReceivingAccountNumber = x.ReceivingAccountNumber,
                    Sum = x.Sum
                }).ToList()
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Send(int paymentId)
        {
            if(paymentId == null)
            {
                ViewData["ErrorMessage"] = "PaymentId is required";
                return RedirectToAction("Index", "Payment");
            }

            var result = await paymentService.SendPayment(new SendPaymentRequest { PaymentId = paymentId });

            if (!result.Success)
            {
                ViewData["ErrorMessage"] = result.Error ?? "Error sending payment";
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Cancel(int paymentId)
        {
            if (paymentId == null)
            {
                ViewData["ErrorMessage"] = "PaymentId is required";
                return RedirectToAction("Index", "Payment");
            }

            var result = await paymentService.CancelPayment(new CancelPaymentRequest { PaymentId = paymentId });

            if (!result.Success)
            {
                ViewData["ErrorMessage"] = result.Error ?? "Error sending payment";
            }
            return RedirectToAction("Index", "Payment");
        }

        [HttpGet]
        public async Task<IActionResult> CreatePayment()
        {
            var userId = HttpContext.Session.GetInt32("UserId").Value;
            var userAccounts = (await accountService.ShowAccounts(new ShowAccountsRequest
            {
                UserId = userId
            })).Accounts;
            var accountsList = new List<SelectListItem>();
            foreach (var account in userAccounts)
            {
                accountsList.Add(new SelectListItem { Value = account.AccountId.ToString(), Text = account.AccountNumber });
            }
            var model = new CreatePaymentViewModel
            {
                UserAccounts = accountsList
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment(CreatePaymentViewModel model)
        {
            var result = await paymentService.CreatePayment(new CreatePaymentRequest
            {
                Description = model.Description,
                SenderAccountId = model.SendingAccountId,
                ReceiverAccountId = (await accountService.GetAccountIdFromNumber(model.ReceivingAccountNumber)).Value,
                Sum = model.Sum
            });
            if (result.Success)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var userId = HttpContext.Session.GetInt32("UserId").Value;
                var userAccounts = (await accountService.ShowAccounts(new ShowAccountsRequest
                {
                    UserId = userId
                })).Accounts;
                var accountsList = new List<SelectListItem>();
                foreach (var account in userAccounts)
                {
                    accountsList.Add(new SelectListItem { Value = account.AccountId.ToString(), Text = account.AccountNumber });
                }
                var newModel = new CreatePaymentViewModel
                {
                    UserAccounts = accountsList
                };
                ViewData["ErrorMessage"] = result.Error ?? "Error in making the payment";
                return View(newModel);
            }

        }
    }
}
