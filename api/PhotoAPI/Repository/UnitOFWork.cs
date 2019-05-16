using PhotoAPI.Models.Entity;
using PhotoAPI.Repository.Interfaces;
using System.Threading.Tasks;

namespace PhotoAPI.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private IPhotoRepository _photoRepository;
        public PhotoContext ApplicationContext { get; }

        public UnitOfWork(PhotoContext сontext)
        {
            ApplicationContext = сontext;
        }

        public IPhotoRepository PhotoRepository => _photoRepository ?? (_photoRepository = new PhotoRepository(ApplicationContext));

        public async Task SubmitChangesAsync()
        {
            await ApplicationContext.SaveChangesAsync();
        }

    }
}
