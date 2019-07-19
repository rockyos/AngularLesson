using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace PhotoAPI.Services.Interfaces
{
    public interface IGenerateJwtTokenService
    {
        Task<object> GenerateJwtToken(string email, IdentityUser user);
    }
}
