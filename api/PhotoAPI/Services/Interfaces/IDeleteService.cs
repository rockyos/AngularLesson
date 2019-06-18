using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using System.Threading.Tasks;

namespace PhotoAPI.Services.Interfaces
{
    public interface IDeleteService
    {
        Task DeleteAsync(string guid, IDistributedCache cache, string authorizationHeader);
    }
}
