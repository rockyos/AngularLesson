using PhotoAPI.Repository;

namespace PhotoAPI.Services
{
    public class BaseService
    {
        protected UnitOfWork UnitOfWork { get; }

        public BaseService(UnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
    }
}
