﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PhotoAPI.Models.Identity;

namespace PhotoAPI.Controllers
{
    [Route("[controller]")]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _messageService;

        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public readonly string angularURL = "http://localhost:4200";

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
        IEmailSender messageService, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _messageService = messageService;
            _configuration = configuration;
        }

        //[HttpGet]
        //[Route("Register")]
        //public IActionResult Register()
        //{
        //    return new RedirectResult(angularURL + "/Account/Register");
        //}


        [HttpPost]
        [Route("Register")]
        public async Task<object> Register(RegisterModel model)
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
                    return await GenerateJwtToken(model.Email, user);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description); 
                }
            }
            string messages = string.Join(" ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return StatusCode(401, messages);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return new RedirectResult(angularURL);
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Error confirming email for user with ID '{userId}':");
            }
            return new RedirectResult(angularURL + "/Account/Confirm"); 
        }

        //[HttpGet]
        //[Route("ForgotPassword")]
        //public IActionResult ForgotPassword()
        //{
        //    return new RedirectResult(angularURL + "/Account/ForgotPassword");
        //}

        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    return new RedirectResult(angularURL + "/Account/ForgotPasswordConfirmation");
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code });
                callbackUrl = $"http://localhost:63627{callbackUrl}";

                await _messageService.SendEmailAsync(
                    model.Email,
                    "Reset Password",
                    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                return new RedirectResult(angularURL + "/Account/ForgotPasswordConfirmation");
            }
            string messages = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return StatusCode(200, Json(new { Message = messages }));
        }

        [HttpGet]
        [Route("ResetPassword")]
        public IActionResult ResetPassword(string code = null)
        {
            return new RedirectResult(angularURL + "/Account/ResetPassword?code=" + code);
        }

        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                return StatusCode(200, Json(new { Message = messages }));
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new RedirectResult(angularURL + "Account/ResetPasswordConfirmation");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return new RedirectResult(angularURL + "Account/ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            string errmessages = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return StatusCode(200, Json(new { Message = errmessages }));

        }


        [HttpGet]
        [Route("Login")]
        public async Task<IActionResult> Login(string ReturnUrl = null)
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            return new RedirectResult(angularURL+ "/Account/Login?ReturnUrl=" + ReturnUrl);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<object> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    //var appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
                    return await GenerateJwtToken(model.Email, user);
                    //return new RedirectResult(angularURL + model.ReturnUrl);
                } else
                {
                    return StatusCode(401, "Invalid login attempt.");
                }
            } 
            string messages = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return StatusCode(401, messages);

        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return new RedirectResult(angularURL);
        }

        private async Task<object> GenerateJwtToken(string email, IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}