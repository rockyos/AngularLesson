using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using PhotoAPI.Extensions;
using PhotoAPI.Models.Entity;
using PhotoAPI.Repository;
using PhotoAPI.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhotoAPI.Services
{
    public class SavePhotoService : BaseService, ISavePhotoService
    {
        public SavePhotoService(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task SavePhotoAsync(IDistributedCache cache, string authorizationHeader)
        {
            //var log = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Console().WriteTo.RollingFile("logs\\log-{Date}.txt").CreateLogger();
            var photosInSession = await cache.GetAsync<List<Photo>>(authorizationHeader);
            if (photosInSession != null)
            {
                foreach (var item in photosInSession)
                {
                    var photoDb = await (await UnitOfWork.PhotoRepository.GetAllAsync()).FirstOrDefaultAsync(m => m.Guid == item.Guid);
                    if (photoDb != null)
                    {
                       // Log.Warning("Remove photos from DB: {@PhotoDB}", photoDb);
                        await UnitOfWork.PhotoRepository.RemoveAsync(photoDb);
                    }
                    else
                    {
                       // Log.Warning("Add photos to DB: {@PhotoDB}", item);
                        await UnitOfWork.PhotoRepository.InsertAsync(item);
                    }
                }
                await UnitOfWork.SubmitChangesAsync();
            }
        }
    }
}
