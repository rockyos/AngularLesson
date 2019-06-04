using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using PhotoAPI.Models.Identity;

namespace PhotoAPI.Controllers
{
    [Route("[controller]")]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailSender _messageService;

        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public readonly string angularURL = "http://localhost:4200";

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
        IEmailSender messageService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _messageService = messageService;
        }

        [HttpGet]
        [Route("Register")]
        public IActionResult Register(string returnUrl = null)
        {
            return new RedirectResult("http://localhost:4200/Account/Register?ReturnUrl=" + returnUrl);
        }


        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code });
                    callbackUrl = $"http://localhost:63627{callbackUrl}";

                    await _messageService.SendEmailAsync(model.Email, "Confirm your email",
                      $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(angularURL);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description); /// json send!!!
                }
            }
            return LocalRedirect(angularURL);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return LocalRedirect(angularURL);
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
               // throw new InvalidOperationException();
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Error confirming email for user with ID '{userId}':");
                //return new RedirectResult(angularURL + "/Account/Login");
            }

            return LocalRedirect(angularURL + "/Account/Confirm"); //page confirm redirect
        }

        [HttpGet]
        [Route("Login")]
        public async Task<IActionResult> Login(string ReturnUrl = null)
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            return new RedirectResult("http://localhost:4200/Account/Login?ReturnUrl=" + ReturnUrl);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    return new RedirectResult(angularURL + model.ReturnUrl);
                } else
                {
                    return StatusCode(400, Json(new { Message = "Invalid login attempt." }));
                }
            }
            return new RedirectResult(angularURL + model.ReturnUrl);
        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return new RedirectResult(angularURL);
        }

    }
}