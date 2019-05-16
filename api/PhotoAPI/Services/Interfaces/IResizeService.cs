using PhotoAPI.Models.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhotoAPI.Services.Interfaces
{
    public interface IResizeService
    {
        Task<byte[]> GetImageAsync(List<Photo> photosInSession, string id, int photoWidthInPixel);
    }
}
