using AutoMapper;
using PhotoAPI.Extensions;
using PhotoAPI.Services.Interfaces;
using PhotoAPI.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using PhotoAPI.Models.Dto;
using PhotoAPI.Models.Entity;
using Microsoft.Extensions.Caching.Distributed;

namespace PhotoAPI.Services
{
    public class GetPhotoService : BaseService, IGetPhotoService
    {
        private readonly IMapper _mapper;
        public GetPhotoService(UnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
        {
            _mapper = mapper;
        }

        public async Task<List<PhotoDTO>> GetPhotoDBandSessionAsync(IDistributedCache cache, string authorizationHeader)
        {
            var photosFromSession = await cache.GetAsync<List<Photo>>(authorizationHeader);
            var photoFromDb = await (await UnitOfWork.PhotoRepository.GetAllAsync()).ToListAsync();
            if (photosFromSession != null)
            {
                var hidePhotoFromSession = new List<Photo>();
                foreach (var item in photosFromSession)
                {
                    var photo = photoFromDb.Find(c => c.Guid == item.Guid);
                    if (photo != null)
                    {
                        photoFromDb.Remove(photo);
                        hidePhotoFromSession.Add(item);
                    }
                }
                photosFromSession.RemoveAll(i => hidePhotoFromSession.Contains(i));
                photoFromDb.AddRange(photosFromSession);
            }
            var photosDto = _mapper.Map<List<Photo>, List<PhotoDTO>>(photoFromDb);
            return photosDto;
        }
    }
}
