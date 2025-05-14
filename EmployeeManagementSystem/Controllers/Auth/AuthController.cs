using EmployeeManagementSystem.Models.Auth;
using EmployeeManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers.Auth
{
    public class AuthController : Controller
    {
        private readonly FirebaseAuthService _firebaseAuthService;

        public AuthController(FirebaseAuthService firebaseAuthService)
        {
            _firebaseAuthService = firebaseAuthService;
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
            {
                return View(model);
            }

            try
            {
                var user = await _firebaseAuthService.Login(model.Username, model.Password!);

                if (user != null)
                {
                    // TODO: Set up session, cookies, etc.
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Login failed. Please try again.");
            }
            catch (Firebase.Auth.FirebaseAuthException ex)
            {
                var errorCode = FirebaseAuthExceptionHandler.HandleException(ex);
                var errorMessage = FirebaseAuthExceptionHandler.GenerateExceptionMessage(errorCode);
                ModelState.AddModelError(string.Empty, errorMessage);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Register(RegisterViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    try
        //    {
        //        var user = await _firebaseAuthService.SignUp(model.Email, model.Password!);

        //        if (user != null)
        //        {
        //            // Optionally auto login or redirect
        //            return RedirectToAction("Login", "Auth");
        //        }

        //        ModelState.AddModelError(string.Empty, "Registration failed. Please try again.");
        //    }
        //    catch (Firebase.Auth.FirebaseAuthException ex)
        //    {
        //        var errorCode = FirebaseAuthExceptionHandler.HandleException(ex);
        //        var errorMessage = FirebaseAuthExceptionHandler.GenerateExceptionMessage(errorCode);
        //        ModelState.AddModelError(string.Empty, errorMessage);
        //    }

        //    return View(model);
        //}

        [HttpPost]
        public IActionResult Logout()
        {
            _firebaseAuthService.SignOut();
            return RedirectToAction("Login", "Auth");
        }
    }
}