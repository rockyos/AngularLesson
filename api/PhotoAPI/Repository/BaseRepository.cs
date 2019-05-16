using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhotoAPI.Repository.Interfaces;

namespace PhotoAPI.Repository
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext Context;

        protected BaseRepository(DbContext context)
        {
            Context = context;
        }

        public virtual async Task<IQueryable<T>> GetAllAsync()
        {
            return await Task.FromResult(Context.Set<T>());
        }

        public virtual async Task InsertAsync(T entity)
        {
            await Task.Run(() => { Context.Set<T>().Add(entity); });
        }

        public virtual async Task SubmitChangesAsync()
        {
            await Context.SaveChangesAsync().ConfigureAwait(true);
        }
        public virtual async Task RemoveAsync(T entity)
        {
            await Task.Run(() => { Context.Set<T>().Remove(entity); });
        }
    }
}
