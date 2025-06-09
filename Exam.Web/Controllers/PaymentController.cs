using Exam.Services.Interfaces.BankAccount;
using Exam.Services.Interfaces.Payment;
using Exam.Services.Interfaces.Status;
using Exam.Services.Interfaces.User;
using Exam.Web.Attributes;
using Exam.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Exam.Web.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {

        private readonly IBankAccountService _bankAccountService;
        private readonly IPaymentService _paymentService;
        private readonly IStatusService _statusService;
        public PaymentController(IBankAccountService bankAccountService, IPaymentService paymentService, IStatusService statusService)
        {
            _bankAccountService = bankAccountService;
            _paymentService = paymentService;
            _statusService = statusService;
        }
        public IActionResult Index()
        {
            int userId = HttpContext.Session.GetInt32("UserId").Value;

            if (!HttpContext.Session.GetInt32("UserId").HasValue)
            {
                return RedirectToAction("Login", "Account");
            }


            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AllPayments(string sort = "id")
        {
            int userId = HttpContext.Session.GetInt32("UserId").Value;

            if (!HttpContext.Session.GetInt32("UserId").HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var userAccounts = await _bankAccountService.GetBankAccountsByUserIdAsync(userId);
            var allPayments = await _paymentService.GetAllPaymentsAsync(userId);
            var statuses = await _statusService.GetAllStatusesAsync();

            var viewModel = allPayments.Select(p =>
            {
                var account = userAccounts.FirstOrDefault(a => a.BankAccountId == p.SenderBankAccountId);

                return new AllPaymentsViewModel
                {
                    SenderIBAN = p.SenderBankAccountIBAN,
                    SenderName = p.SenderName,
                    RecieverIBAN = p.RecieverIBAN,
                    RecieverName = p.RecieverName,
                    Reference = p.Reference,
                    Amount = p.Amount,
                    StatusId = p.StatusId,
                    StatusDescription = statuses.FirstOrDefault(s => s.StatusId == p.StatusId)?.Description ?? "Unknown Status"
                };
            }).ToList();

            if (sort == "status")
            {
                viewModel = viewModel
                    .OrderBy(p => p.StatusId != 1)
                    .ThenByDescending(p => p.PaymentId)
                    .ToList();
            }
            else
            {
                viewModel = viewModel
                    .OrderByDescending(p => p.PaymentId)
                    .ToList();
            }


            return View(viewModel);

        }
    }
}
