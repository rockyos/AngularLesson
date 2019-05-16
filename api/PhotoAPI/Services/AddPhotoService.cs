using Microsoft.AspNetCore.Http;
using PhotoAPI.Extensions;
using PhotoAPI.Models.Entity;
using PhotoAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoAPI.Services
{
    public class AddPhotoService : IAddPhotoService
    {
        public async Task GetIndexServiceAsync(IFormFile photoFromClient, ISession session, string sessionkey)
        {
            var photosInSession = session.Get<List<Photo>>(sessionkey);
            var photo = new Photo();
            using (var reader = new BinaryReader(photoFromClient.OpenReadStream()))
            {
                var img = reader.ReadBytes((int)photoFromClient.Length);
                photo.PhotoName = photoFromClient.FileName;
                photo.ImageContent = img;
                photo.Guid = Guid.NewGuid().ToString();
            }

            if (photosInSession != null)
            {
                photosInSession.Add(photo);
                session.Set(sessionkey, photosInSession);
            }
            else
            {
                session.Set(sessionkey, new List<Photo>() { photo });
            }
        }
    }
}
