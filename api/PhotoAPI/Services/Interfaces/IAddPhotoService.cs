using Microsoft.AspNetCore.Http;
using PhotoAPI.Models.Entity;
using System.Threading.Tasks;

namespace PhotoAPI.Services.Interfaces
{
    public interface IAddPhotoService
    {
        Task AddPhotoServiceAsync(IFormFile newImage, ISession session, string sessionkey);
    }
}
