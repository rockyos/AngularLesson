using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using PhotoAPI.Extensions;
using PhotoAPI.Models.Dto;
using PhotoAPI.Models.Entity;
using PhotoAPI.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace PhotoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PhotoController : ControllerBase
    {
        private readonly IGetPhotoService _getPhotoService;
        private readonly IResizeService _resizer;
        private readonly ISavePhotoService _savePhotoService;
        private readonly IDeleteService _deleteService;
        private readonly IAddPhotoService _photoService;
        private readonly IDistributedCache _cache;
        private readonly IConfiguration _configuration;

        private string tokenKey => HttpContext.Request.Headers["authorization"];
        int lifeTime => Convert.ToInt16(_configuration["DistributeCachingTimeInMinutes"]);

        public PhotoController(IGetPhotoService getPhotoService, IResizeService resizer, ISavePhotoService savePhotoService,
            IDeleteService deleteService, IAddPhotoService photoService, IDistributedCache cache, IConfiguration configuration)
        {
            _getPhotoService = getPhotoService;
            _resizer = resizer;
            _savePhotoService = savePhotoService;
            _deleteService = deleteService;
            _photoService = photoService;
            _configuration = configuration;
            _cache = cache;
        }

        [HttpGet]
        public async Task<IEnumerable<PhotoDTO>> GetAsync()
        {
            var photos = await _getPhotoService.GetPhotoDBandSessionAsync(_cache, tokenKey);
            return photos;
        }

        [HttpGet("{id}")]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 300)]
        public async Task<ActionResult> GetAsync(string id, int width)
        {

            var sessionPhotos = await _cache.GetAsync<List<Photo>>(tokenKey);
            var resizedImage = await _resizer.GetImageAsync(sessionPhotos, id, width);
            return new FileContentResult(resizedImage, "binary/octet-stream");
        }

        [HttpPost]
        [Route("send")]
        public async Task PostAsync(IFormFile newImage)
        {
            await _photoService.AddPhotoServiceAsync(newImage, _cache, tokenKey, lifeTime);
        }

        [HttpPost]
        [Route("save")]
        public async Task PostSaveAsync()
        {
            await _savePhotoService.SavePhotoAsync(_cache, tokenKey);
            await _cache.RemoveAsync(tokenKey);
        }

        [HttpPost]
        [Route("reset")]
        public async Task PostResetAsync()
        {
            await _cache.RemoveAsync(tokenKey);
        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync(string id)
        {
            await _deleteService.DeleteAsync(id, _cache, tokenKey, lifeTime);
        }
    }
}
