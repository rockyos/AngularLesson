using Microsoft.AspNetCore.Http;
using PhotoAPI.Models.Entity;
using System.Threading.Tasks;

namespace PhotoAPI.Services.Interfaces
{
    public interface IAddPhotoService
    {
        Task GetIndexServiceAsync(IFormFile photoFromClient, ISession session, string sessionkey);
    }
}
