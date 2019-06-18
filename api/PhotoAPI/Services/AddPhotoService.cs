using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using PhotoAPI.Extensions;
using PhotoAPI.Models.Entity;
using PhotoAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PhotoAPI.Services
{
    public class AddPhotoService : IAddPhotoService
    {
        public async Task AddPhotoServiceAsync(IFormFile newImage, IDistributedCache cache, string authorizationHeader)
        {
            var photosInSession = await cache.GetAsync<List<Photo>>(authorizationHeader);
            var photo = new Photo();
            using (var reader = new BinaryReader(newImage.OpenReadStream()))
            {
                var img = reader.ReadBytes((int)newImage.Length);
                photo.PhotoName = newImage.FileName;
                photo.ImageContent = img;
                photo.Guid = Guid.NewGuid().ToString();
            }

            if (photosInSession != null)
            {
                photosInSession.Add(photo);
                await cache.SetAsync<List<Photo>>(authorizationHeader, photosInSession, 
                    new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(20) });
            } else
            {
                await cache.SetAsync<List<Photo>>(authorizationHeader, new List<Photo>() { photo },
                    new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(20) });
            }
        }
    }
}
