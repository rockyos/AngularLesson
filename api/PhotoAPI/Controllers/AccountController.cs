using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace PhotoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailSender _messageService;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
        IEmailSender messageService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _messageService = messageService;
        }

        [HttpPost]
        //[Produces("application/json")]
        [Route("register")]
        public async Task<IActionResult> Register(string email, string password, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return StatusCode(400, Json(new { Message = "Email and/or password field is empty"}));
            }

            if (password != confirmPassword)
            {
                return StatusCode(400, Json(new { Message = "Passwords don't match!" }));
            }

            var newUser = new IdentityUser
            {
                UserName = email,
                Email = email
            };

            IdentityResult userCreationResult = null;
            try
            {
                userCreationResult = await _userManager.CreateAsync(newUser, password);
            }
            catch (SqlException)
            {
                return StatusCode(500, Json(new { Message = "Error communicating with the database, see logs for more details" }));
            }

            if (!userCreationResult.Succeeded)
            {
                return StatusCode(400, Json(new
                {
                    Message = "An error occurred when creating the user, see nested errors",
                    Errors = userCreationResult.Errors.Select(x =>  new { Message = $"[{x.Code}] {x.Description}" })
                }));
            }

            var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            var tokenVerificationUrl = Url.Action("VerifyEmail", "Account", new
                {
                    Id = newUser.Id,
                    token = emailConfirmationToken
                }, Request.Scheme);

            await _messageService.SendEmailAsync(email,
                "Verify your email", $"Click <a href=\"{tokenVerificationUrl}\">here</a> to verify your email");

            return StatusCode(200, Json(new { Message = $"Registration completed, please verify your email - {email}" }));
        }


        public async Task<IActionResult> VerifyEmail(string id, string token)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new InvalidOperationException();

            var emailConfirmationResult = await _userManager.ConfirmEmailAsync(user, token);
            if (!emailConfirmationResult.Succeeded)
            {
                return new RedirectResult("http://localhost.com:63627/registration.html");
            }

            return new RedirectResult("http://localhost.com:63627/"); 
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return StatusCode(400, Json(new { Message = "email or password is null" }));
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return StatusCode(400, Json(new { Message = "Invalid Login and/or password" }));
            }

            if (!user.EmailConfirmed)
            {
                return StatusCode(400, Json(new { Message = "Email not confirmed, please check your email for confirmation link" }));
            }

            var passwordSignInResult = await _signInManager.PasswordSignInAsync(user, password, isPersistent: true, lockoutOnFailure: false);
            if (!passwordSignInResult.Succeeded)
            {
                return StatusCode(400, Json(new { Message = "Invalid Login and/or password" }));
            }

            return StatusCode(200);
        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return StatusCode(200, Json(new { Message = "You have been successfully logged out" }));
        }

    }
}