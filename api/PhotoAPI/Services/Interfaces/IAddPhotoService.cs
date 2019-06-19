using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using PhotoAPI.Models.Entity;
using System.Threading.Tasks;

namespace PhotoAPI.Services.Interfaces
{
    public interface IAddPhotoService
    {
        Task AddPhotoServiceAsync(IFormFile newImage, IDistributedCache cache, string authorizationHeader, int lifeTime);
    }
}
