using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PhotoAPI.Services.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Net.Http;
using PhotoAPI.Models.Identity;
using System.Linq;

namespace PhotoAPI.Services
{
    public class GetExternalLoginService : IGetExternalLoginService
    {
        public async Task<object> ExternalLoginAsync(UserManager<IdentityUser> _userManager, SignInManager<IdentityUser> _signInManager,
             IGenerateJwtTokenService _generateJwtTokenService, Controller controller, string JwtKey, string JwtExpireDays,
             string JwtIssuer)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (info == null)
            {
                return controller.StatusCode(500, "Error loading external login information.");
            }
            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
                var name = info.Principal.FindFirstValue(ClaimTypes.Name);
                return await _generateJwtTokenService.GenerateJwtToken(user.Email + $"({name})", user, JwtKey,
                        JwtExpireDays, JwtIssuer);
            }
            else if (email != null)
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    user = new IdentityUser { UserName = email, Email = email, EmailConfirmed = true };
                    await _userManager.CreateAsync(user);
                }
                await _userManager.AddLoginAsync(user, info);
                var name = info.Principal.FindFirstValue(ClaimTypes.Name);
                return await _generateJwtTokenService.GenerateJwtToken(email + $"({name})", user, JwtKey,
                        JwtExpireDays, JwtIssuer);
            }
            else
            {
                return controller.StatusCode(401, "401 Unauthorized");
            }
        }



        public async Task<object> GoogleGetInfoByToken(string token, HttpClient client, SignInManager<IdentityUser> _signInManager,
            UserManager<IdentityUser> _userManager, Controller controller, IGenerateJwtTokenService _generateJwtTokenService,
            string JwtKey, string JwtExpireDays, string JwtIssuer)
        {
            var userInfoResponse = await client.GetStringAsync($"https://www.googleapis.com/plus/v1/people/me?access_token={token}");
            var userInfo = JsonConvert.DeserializeObject<GoogleModel>(userInfoResponse);
            if (userInfo == null)
            {
                return controller.StatusCode(500, "Error loading external login information.");
            }
            var result = await _signInManager.ExternalLoginSignInAsync("Google", userInfo.id, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByLoginAsync("Google", userInfo.id);
                return await _generateJwtTokenService.GenerateJwtToken(user.Email + $"({userInfo.displayName})", user,
                    JwtKey, JwtExpireDays, JwtIssuer);
            }
            else if (userInfo.emails != null)
            {
                IdentityUser user = null;
                string mail = null;
                foreach (var item in userInfo.emails)
                {
                    mail = item.FirstOrDefault(x => x.Key == "value").Value;
                    user = await _userManager.FindByEmailAsync(mail);
                    if (user != null) break;
                }
                if (user == null)
                {
                    user = new IdentityUser { UserName = mail, Email = mail, EmailConfirmed = true };
                    await _userManager.CreateAsync(user);
                }
                await _userManager.AddLoginAsync(user, new UserLoginInfo("Google", userInfo.id, "Google"));
                return await _generateJwtTokenService.GenerateJwtToken(user.Email + $"({userInfo.displayName})", user,
                   JwtKey, JwtExpireDays, JwtIssuer);
            }
            else
            {
                return controller.StatusCode(401, "401 Unauthorized");
            }
        }



        public async Task<object> FacebookGetInfoByToken(string token, HttpClient client, SignInManager<IdentityUser> _signInManager,
            UserManager<IdentityUser> _userManager, Controller controller, IGenerateJwtTokenService _generateJwtTokenService,
            string JwtKey, string JwtExpireDays, string JwtIssuer)
        {
            var userInfoResponse = await client.GetStringAsync($"https://graph.facebook.com/v2.8/me?fields=id,email,name&access_token={token}");
            var userInfo = JsonConvert.DeserializeObject<FacebookModel>(userInfoResponse);
            if (userInfo == null)
            {
                return controller.StatusCode(500, "Error loading external login information.");
            }
            var result = await _signInManager.ExternalLoginSignInAsync("Facebook", userInfo.id, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByLoginAsync("Facebook", userInfo.id);
                return await _generateJwtTokenService.GenerateJwtToken(user.Email + $"({userInfo.name})", user,
                    JwtKey, JwtExpireDays, JwtIssuer);
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
                   JwtKey, JwtExpireDays, JwtIssuer);
            }
            else
            {
                return controller.StatusCode(401, "401 Unauthorized");
            }
        }
    }
}
