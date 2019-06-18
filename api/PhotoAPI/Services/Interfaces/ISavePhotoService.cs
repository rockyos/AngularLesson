using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using System.Threading.Tasks;

namespace PhotoAPI.Services.Interfaces
{
    public interface ISavePhotoService
    {
        Task SavePhotoAsync(IDistributedCache cache, string authorizationHeader);
    }
}
