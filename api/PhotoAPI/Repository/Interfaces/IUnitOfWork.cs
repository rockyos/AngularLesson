using System.Threading.Tasks;

namespace PhotoAPI.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        IPhotoRepository PhotoRepository { get; }
        Task SubmitChangesAsync();
    }
}
