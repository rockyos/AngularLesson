using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace PhotoAPI.Services.Interfaces
{
    public interface IGetExternalLoginService
    {
        Task<object> ExternalLoginAsync(UserManager<IdentityUser> _userManager, SignInManager<IdentityUser> _signInManager,
            IGenerateJwtTokenService _generateJwtTokenService, Controller controller, string JwtKey, string JwtExpireDays, 
            string JwtIssuer);
    }
}
