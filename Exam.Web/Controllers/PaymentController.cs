using Exam.Models;
using Exam.Services.DTOs.Payment;
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
        private readonly IUserService _userService;
        private readonly IStatusService _statusService;
        public PaymentController(IBankAccountService bankAccountService, IPaymentService paymentService, IStatusService statusService, IUserService userService)
        {
            _bankAccountService = bankAccountService;
            _paymentService = paymentService;
            _statusService = statusService;
            _userService = userService;
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

        [HttpGet]
        public async Task<IActionResult> CreatePayment()
        {
            int userId = HttpContext.Session.GetInt32("UserId").Value;

            if (!HttpContext.Session.GetInt32("UserId").HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var userAccounts = await _bankAccountService.GetBankAccountsByUserIdAsync(userId);
            var user = await _userService.GetByIdAsync(userId);
            var model = new CreatePaymentViewModel
            {
                SenderUserId = userId,
                SenderName = user.Name,
                IBANOptions = userAccounts.Select(acc => new BankAccountOption
                {
                    BankAccountId = acc.BankAccountId,
                    IBAN = acc.IBAN
                }).ToList()
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> CreatePayment(CreatePaymentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var userAccounts = await _bankAccountService.GetBankAccountsByUserIdAsync(model.SenderUserId);
                model.IBANOptions = userAccounts.Select(acc => new BankAccountOption
                {
                    BankAccountId = acc.BankAccountId,
                    IBAN = acc.IBAN
                }).ToList();

                return View(model);
            }


            var userAccountsList = await _bankAccountService.GetBankAccountsByUserIdAsync(model.SenderUserId);
            var selectedAccount = userAccountsList.FirstOrDefault(acc => acc.IBAN == model.SelectedIBAN);

            if (selectedAccount == null)
            {
                ModelState.AddModelError("SelectedIBAN", "Invalid IBAN selected.");
                model.IBANOptions = userAccountsList.Select(acc => new BankAccountOption
                {
                    BankAccountId = acc.BankAccountId,
                    IBAN = acc.IBAN
                }).ToList();

                return View(model);
            }

            var request = new CreatePaymentRequest
            {
                SenderBankAccountId = selectedAccount.BankAccountId,
                SenderUserId = model.SenderUserId,
                SenderIBAN = model.SelectedIBAN,
                RecieverIBAN = model.RecieverIBAN,
                RecieverName = model.RecieverName,
                Reference = model.Reference,
                Amount = model.Amount,
                StatusId = model.StatusId
            };

            var result = await _paymentService.CreatePaymentAsync(request);

            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return View(model);
            }
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public async Task<IActionResult> SendPayments(SendPaymentViewModel model)
        {

            var payment = await _paymentService.GetPaymentByIdAsync(model.PaymentId);

            var request = new UpdatePaymentRequest
            {
                PaymentId = payment.PaymentId,
                StatusId = 2
            };

            await _paymentService.SendPaymentAsync(request);

            return RedirectToAction("AllPayments");
        }

        [HttpPost]
        public async Task<IActionResult> CancelPayments(CancelPaymentViewModel model)
        {

            var payment = await _paymentService.GetPaymentByIdAsync(model.PaymentId);

            var request = new UpdatePaymentRequest
            {
                PaymentId = payment.PaymentId,
                StatusId = 3
            };

            await _paymentService.CancelPaymentAsync(request);

            return RedirectToAction("AllPayments");
        }
    }
}
