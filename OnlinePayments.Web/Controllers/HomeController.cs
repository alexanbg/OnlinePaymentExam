using Microsoft.AspNetCore.Mvc;
using OnlinePayments.Services.DTOs.Account;
using OnlinePayments.Services.Interfaces.Account;

namespace OnlinePayments.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAccountService accountService;

        public HomeController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        public async Task<IActionResult> Index()
        {
            if (!HttpContext.Session.GetInt32("UserId").HasValue)
            {
                return RedirectToAction("Login", "Authentication");
            }

            var userId = HttpContext.Session.GetInt32("UserId").Value;

            var result = await accountService.ShowAccounts(new ShowAccountsRequest { UserId = userId });

            if (!result.Success)
            {
                ViewData["ErrorMessage"] = result.Error ?? "Error showing request";
                return RedirectToAction("Login", "Authentication");
            }

            var viewModel = new HomeViewModel()
            {
                Accounts = result.Accounts.Select(x => new AccountViewModel
                {
                    AccountId = x.AccountId,
                    AccountNumber = x.AccountNumber,
                    AvailableSum = x.AvailableSum
                }).ToList()
            };

            return View(viewModel);
        }
    }
}
