using Microsoft.EntityFrameworkCore;
using PhotoAPI.Models.Entity;
using PhotoAPI.Repository;
using PhotoAPI.Services.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace PhotoAPI.Services
{
    public class ResizeService : BaseService, IResizeService
    {
        public ResizeService(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<byte[]> GetImageAsync(List<Photo> photosInSession, string id, int photoWidthInPixel)
        {
            var photoDb = await (await UnitOfWork.PhotoRepository.GetAllAsync()).FirstOrDefaultAsync(m => m.Guid == id);
            if (photoDb == null)
            {
                foreach (var item in photosInSession)
                {
                    if (item.Guid != id) continue;
                    photoDb = item;
                    break;
                }
            }

            if (photoWidthInPixel != 0)
            {
                var memoryStream = new MemoryStream();
                const long quality = 50; // picture quality max 100
                if (photoDb != null)
                {
                    using (var ms = new MemoryStream(photoDb.ImageContent))
                    {
                        //System.Drawing.Ima

                        var bmp = new Bitmap(ms);
                        var imageHeight = bmp.Height;
                        var imageWidth = bmp.Width;
                        if (imageWidth > photoWidthInPixel)
                        {
                            var ratio = imageWidth / (float)imageHeight;
                            var resizedBitmap = new Bitmap(photoWidthInPixel, (int)(photoWidthInPixel / ratio));
                            using (var graphics = Graphics.FromImage(resizedBitmap))
                            {
                                graphics.CompositingQuality = CompositingQuality.HighSpeed;
                                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                graphics.CompositingMode = CompositingMode.SourceCopy;
                                graphics.DrawImage(bmp, 0, 0, photoWidthInPixel, photoWidthInPixel / ratio);
                                var qualityParamId = Encoder.Quality;
                                var encoderParameters = new EncoderParameters(1);
                                encoderParameters.Param[0] = new EncoderParameter(qualityParamId, quality);
                                resizedBitmap.Save(memoryStream, ImageFormat.Jpeg);
                            }
                        }
                        else
                        {
                            bmp.Save(memoryStream, ImageFormat.Jpeg);
                        }
                    }
                }
                return memoryStream.ToArray();
            }
            return photoDb.ImageContent;
        }
    }
}
