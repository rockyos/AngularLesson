using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PhotoAPI.Extensions;
using PhotoAPI.Models.Entity;
using PhotoAPI.Repository;
using PhotoAPI.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhotoAPI.Services
{
    public class DeleteService : BaseService, IDeleteService
    {
        public DeleteService(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task DeleteAsync(string guid, ISession session, string sessionkey)
        {
            var photosInSession = session.Get<List<Photo>>(sessionkey);
            var photoDb = await (await UnitOfWork.PhotoRepository.GetAllAsync()).FirstOrDefaultAsync(m => m.Guid == guid);
            if (photoDb != null)
            {
                if (photosInSession == null)
                {
                    photosInSession = new List<Photo>();
                }
                photosInSession.Add(photoDb);
            }
            else
            {
                foreach (var item in photosInSession)
                {
                    if (item.Guid == guid)
                    {
                        photosInSession.Remove(item);
                        break;
                    }
                }
            }
            session.Set(sessionkey, photosInSession);
        }
    }
}
