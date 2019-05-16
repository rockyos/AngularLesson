using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace PhotoAPI.Services.Interfaces
{
    public interface ISavePhotoService
    {
        Task SavePhotoAsync(ISession session, string sessionkey);
    }
}
