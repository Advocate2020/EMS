using EmployeeManagementSystem.BusinessLogic;
using EmployeeManagementSystem.Models.Auth;
using EmployeeManagementSystem.Services;
using Firebase.Auth;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers.Auth
{
    public class AuthController : Controller
    {
        private readonly AuthBL _authBL;
        private readonly FirebaseAuthService _firebaseAuthService;

        public AuthController(AuthBL authBL, FirebaseAuthService firebaseAuthService)
        {
            _authBL = authBL;
            _firebaseAuthService = firebaseAuthService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var (success, error) = await _authBL.RegisterUserAsync(model);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, error);
                return View(model);
            }

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var user = await _firebaseAuthService.Login(model.Username, model.Password);

                if (user != null)
                {
                    var idToken = await user.GetIdTokenAsync();

                    // Store token in cookie
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        Expires = model.RememberMe ? DateTimeOffset.UtcNow.AddDays(7) : DateTimeOffset.UtcNow.AddHours(1),
                        SameSite = SameSiteMode.Strict
                    };

                    Response.Cookies.Append("FirebaseToken", idToken, cookieOptions);

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Login failed.");
                return View(model);
            }
            catch (FirebaseAuthException ex)
            {
                var status = FirebaseAuthExceptionHandler.HandleException(ex);
                var message = FirebaseAuthExceptionHandler.GenerateExceptionMessage(status);

                ModelState.AddModelError(string.Empty, message);
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("FirebaseToken");
            _firebaseAuthService.SignOut();
            return RedirectToAction("Login");
        }
    }
}