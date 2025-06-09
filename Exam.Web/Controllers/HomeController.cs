using Exam.Services.Interfaces.BankAccount;
using Exam.Services.Interfaces.User;
using Exam.Web.Attributes;
using Exam.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Exam.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBankAccountService _bankAccountService;

        private readonly IUserService _userService;

        public HomeController(ILogger<HomeController> logger, IUserService userService, IBankAccountService bankAccountService)
        {
            _logger = logger;
            _userService = userService;
            _bankAccountService = bankAccountService;
        }

        public async Task<IActionResult> Index()
        {
            int userId = HttpContext.Session.GetInt32("UserId").Value;

            if (!HttpContext.Session.GetInt32("UserId").HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var viewModel = await _bankAccountService.GetBankAccountsByUserIdAsync(userId);

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
