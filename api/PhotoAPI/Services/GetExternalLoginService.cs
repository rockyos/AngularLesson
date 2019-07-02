using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PhotoAPI.Services.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;

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
    }
}
