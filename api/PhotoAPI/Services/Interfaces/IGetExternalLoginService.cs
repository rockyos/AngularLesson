using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace PhotoAPI.Services.Interfaces
{
    public interface IGetExternalLoginService
    {
        Task<object> ExternalLoginAsync(Controller controller);

        Task<object> GoogleGetInfoByToken(string token, Controller controller);

        Task<object> FacebookGetInfoByToken(string token, Controller controller);
    }
}
