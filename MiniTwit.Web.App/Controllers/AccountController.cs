using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniTwit.Entities;
using MiniTwit.Models;
using MiniTwit.Web.App.Models;

namespace MiniTwit.Web.App.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger _logger;
        private readonly IMessageRepository _repository;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<AccountController> logger,
            IMessageRepository repository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _repository = repository;
        }

        [Route("/register")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [Route("/register")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register model, string returnUrl = null)
        {
            Console.WriteLine();
            ViewData["Error"] = "Success";
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid) return View(model);
            var user = new User {UserName = model.UserName, Email = model.Email};
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                await _signInManager.SignInAsync(user, false);
                _logger.LogInformation("User created a new account with password.");
                return RedirectToLocal(returnUrl);
            }

            AddErrors(result);

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [Route("/login")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [Route("/login")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid) return View(model);
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var result =
                await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                return RedirectToLocal(returnUrl);
            }
            /*if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out.");
                return RedirectToAction(nameof(Lockout));
            }*/

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);

            // If we got this far, something failed, redisplay form
        }

        /*[Route("/logout")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }*/

        public IActionResult LogIn()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [Route("/logout")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }


        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        #endregion
    }
}