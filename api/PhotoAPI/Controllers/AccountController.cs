﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PhotoAPI.Models.Identity;
using PhotoAPI.Services.Interfaces;

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
        private readonly IGenerateJwtTokenService _generateJwtTokenService;
        private readonly IGetExternalLoginService _getExternalLoginService;
        public readonly string angularURL = "http://localhost:4200";
        private static readonly HttpClient client = new HttpClient();

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
        IEmailSender messageService, IConfiguration configuration, IGenerateJwtTokenService generateJwtTokenService,
        IGetExternalLoginService getExternalLoginService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _messageService = messageService;
            _configuration = configuration;
            _generateJwtTokenService = generateJwtTokenService;
            _getExternalLoginService = getExternalLoginService;
        }

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
                    callbackUrl = $"https://localhost:44375{callbackUrl}";

                    await _messageService.SendEmailAsync(model.Email, "Confirm your email",
                      $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return await _generateJwtTokenService.GenerateJwtToken(model.Email, user, _configuration["JwtKey"],
                        _configuration["JwtExpireDays"], _configuration["JwtIssuer"]);
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
        [Route("ConfirmEmail")]
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


        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    return StatusCode(404, "Wrong email!");
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code });
                callbackUrl = $"https://localhost:44375{callbackUrl}";

                await _messageService.SendEmailAsync(
                    model.Email,
                    "Reset Password",
                    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                return StatusCode(200);
            }
            string messages = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return StatusCode(404, messages);
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
                return StatusCode(404, messages);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return StatusCode(404, "Wrong email!");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return StatusCode(200);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            string errmessages = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return StatusCode(404, errmessages);

        }


        [HttpPost]
        [Route("Login")]
        public async Task<object> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    return await _generateJwtTokenService.GenerateJwtToken(model.Email, user, _configuration["JwtKey"],
                        _configuration["JwtExpireDays"], _configuration["JwtIssuer"]);
                } else
                {
                    return StatusCode(401, "Invalid login attempt.");
                }
            }
            string messages = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return StatusCode(401, messages);
        }

        [HttpGet]
        [Route("GoogleGetInfoByToken")]
        public async Task<object> GoogleGetInfoByToken(string token)
        {
            var userInfoResponse = await client.GetStringAsync($"https://www.googleapis.com/plus/v1/people/me?access_token={token}");
            var userInfo = JsonConvert.DeserializeObject<GoogleModel>(userInfoResponse);
            if (userInfo == null)
            {
                return StatusCode(500, "Error loading external login information.");
            }
            var result = await _signInManager.ExternalLoginSignInAsync("Google", userInfo.id, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByLoginAsync("Google", userInfo.id);
                return await _generateJwtTokenService.GenerateJwtToken(user.Email + $"({userInfo.displayName})", user,
                    _configuration["JwtKey"], _configuration["JwtExpireDays"], _configuration["JwtIssuer"]);
            }
            else if (userInfo.emails != null)
            {
                IdentityUser user = null;
                string mail = null;
                foreach (var item in userInfo.emails)
                {
                   mail = item.FirstOrDefault(x => x.Key == "value").Value;
                   user = await _userManager.FindByEmailAsync(mail);
                   if(user != null) break; 
                }
                if (user == null)
                {
                    user = new IdentityUser { UserName = mail, Email = mail, EmailConfirmed = true };
                    await _userManager.CreateAsync(user);
                }
                await _userManager.AddLoginAsync(user, new UserLoginInfo("Google", userInfo.id, "Google"));
                return await _generateJwtTokenService.GenerateJwtToken(user.Email + $"({userInfo.displayName})", user,
                   _configuration["JwtKey"], _configuration["JwtExpireDays"], _configuration["JwtIssuer"]);
            }
            else
            {
                return StatusCode(401, "401 Unauthorized");
            }
        }

        [HttpGet]
        [Route("FacebookGetInfoByToken")]
        public async Task<object> FacebookGetInfoByToken(string token)
        {
            var userInfoResponse = await client.GetStringAsync($"https://graph.facebook.com/v2.8/me?fields=id,email,first_name,last_name,name,gender,locale,birthday,picture&access_token={token}");
            var userInfo = JsonConvert.DeserializeObject<FacebookModel>(userInfoResponse);
            if (userInfo == null)
            {
                return StatusCode(500, "Error loading external login information.");
            }
            var result = await _signInManager.ExternalLoginSignInAsync("Facebook", userInfo.id, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByLoginAsync("Facebook", userInfo.id);
                return await _generateJwtTokenService.GenerateJwtToken(user.Email + $"({userInfo.name})", user,
                    _configuration["JwtKey"], _configuration["JwtExpireDays"], _configuration["JwtIssuer"]);
            }
            else if (userInfo.email != null)
            {
                var user = await _userManager.FindByEmailAsync(userInfo.email);
                if (user == null)
                {
                    user = new IdentityUser { UserName = userInfo.email, Email = userInfo.email, EmailConfirmed = true };
                    await _userManager.CreateAsync(user);
                }
                await _userManager.AddLoginAsync(user, new UserLoginInfo("Facebook", userInfo.id, "Facebook"));
                return await _generateJwtTokenService.GenerateJwtToken(user.Email + $"({userInfo.name})", user,
                   _configuration["JwtKey"], _configuration["JwtExpireDays"], _configuration["JwtIssuer"]);
            }
            else
            {
                return StatusCode(401, "401 Unauthorized");
            }
        }

      

        //[HttpGet]
        //[Route("ExternalLogin")]
        //public IActionResult ExternalLogin(string provider, string redirect_uri = null)
        //{
        //    var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirect_uri);
        //    return new ChallengeResult(provider, properties);
        //}


        //[HttpGet]
        //[Route("Google")]
        //public async Task<object> Google()
        //{
        //    return await _getExternalLoginService.ExternalLoginAsync(_userManager, _signInManager, _generateJwtTokenService, this,
        //         _configuration["JwtKey"], _configuration["JwtExpireDays"], _configuration["JwtIssuer"]);
        //}


        //[HttpGet]
        //[Route("Facebook")]
        //public async Task<object> Facebook()
        //{
        //    return await _getExternalLoginService.ExternalLoginAsync(_userManager, _signInManager, _generateJwtTokenService, this,
        //         _configuration["JwtKey"], _configuration["JwtExpireDays"], _configuration["JwtIssuer"]);
        //}

        //[HttpPost]
        //[Route("ExternalConfirmation")]
        //public async Task<object> OnPostExternalConfirmationAsync(ExternalLogin model)
        //{
        //    // Get the information about the user from the external login provider
        //    var info = await  _signInManager.GetExternalLoginInfoAsync();
        //    if (info == null)
        //    {
        //        return StatusCode(500, "Error loading external login information during confirmation.");
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        var user = new IdentityUser { UserName = model.Email, Email = model.Email };
        //        var result = await _userManager.CreateAsync(user);
        //        if (result.Succeeded) 
        //        {
        //            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        //            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code });
        //            callbackUrl = $"https://localhost:44375{callbackUrl}";

        //            await _messageService.SendEmailAsync(model.Email, "Confirm your email",
        //              $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

        //            result = await _userManager.AddLoginAsync(user, info);
        //            if (result.Succeeded)
        //            {
        //                var name = info.Principal.FindFirstValue(ClaimTypes.Name);
        //                return await _generateJwtTokenService.GenerateJwtToken(model.Email + $"({name})", user, _configuration["JwtKey"],
        //                _configuration["JwtExpireDays"], _configuration["JwtIssuer"]);
        //            }
        //        }
        //        foreach (var error in result.Errors)
        //        {
        //            ModelState.AddModelError(string.Empty, error.Description);
        //        }
        //    }
        //    string messages = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
        //    return StatusCode(401, messages);
        //}


    }
}