using Exam.Services.DTOs.Authentication;
using Exam.Services.Interfaces.Authentication;
using Exam.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Exam.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthenticationService _authService;

        public AccountController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = "/")
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _authService.LoginAsync(new LoginRequest
            {
                Username = model.Username,
                Password = model.Password
            });

            if (result.IsSuccess)
            {
                HttpContext.Session.SetInt32("UserId", result.UserId.Value);
                HttpContext.Session.SetString("Name", result.Name);


                if (!string.IsNullOrEmpty(model.ReturnUrl))
                    return Redirect(model.ReturnUrl);

                return RedirectToAction("Index", "Home");
            }

            ViewData["ErrorMessage"] = result.ErrorMessage ?? "Invalid username or password";
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
