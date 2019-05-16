using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace PhotoAPI.Services.Interfaces
{
    public interface IDeleteService
    {
        Task DeleteAsync(string guid, ISession session, string sessionkey);
    }
}
