using Microsoft.Extensions.Caching.Distributed;
using PhotoAPI.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhotoAPI.Services.Interfaces
{
    public interface IGetPhotoService
    {
        Task<List<PhotoDTO>> GetPhotoDBandSessionAsync(IDistributedCache cache, string authorizationHeader);
    }
}
