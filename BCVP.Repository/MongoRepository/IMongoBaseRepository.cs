using System.Collections.Generic;
using System.Threading.Tasks;

namespace BCVP.Repository.MongoRepository
{

    public interface IMongoBaseRepository<TEntity> where TEntity : class
    {
        Task AddAsync(TEntity entity);
        Task<TEntity> GetAsync(int Id);
        Task<List<TEntity>> GetListAsync();
    }
}
