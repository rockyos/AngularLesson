using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace PhotoAPI.Services.Interfaces
{
    public interface IGetExternalLoginService
    {
        Task<object> ExternalLoginAsync(UserManager<IdentityUser> _userManager, SignInManager<IdentityUser> _signInManager,
            IGenerateJwtTokenService _generateJwtTokenService, Controller controller, string JwtKey, string JwtExpireDays, 
            string JwtIssuer);

        Task<object> GoogleGetInfoByToken(string token, HttpClient client, SignInManager<IdentityUser> _signInManager,
            UserManager<IdentityUser> _userManager, Controller controller, IGenerateJwtTokenService _generateJwtTokenService,
            string JwtKey, string JwtExpireDays, string JwtIssuer);

        Task<object> FacebookGetInfoByToken(string token, HttpClient client, SignInManager<IdentityUser> _signInManager,
            UserManager<IdentityUser> _userManager, Controller controller, IGenerateJwtTokenService _generateJwtTokenService,
            string JwtKey, string JwtExpireDays, string JwtIssuer);
    }
}
