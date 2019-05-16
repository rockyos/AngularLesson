using PhotoAPI.Models.Entity;
using PhotoAPI.Repository.Interfaces;


namespace PhotoAPI.Repository
{
    public class PhotoRepository : BaseRepository<Photo>, IPhotoRepository
    {
        public PhotoRepository(PhotoContext context) : base(context)
        {
        }
    }
}
