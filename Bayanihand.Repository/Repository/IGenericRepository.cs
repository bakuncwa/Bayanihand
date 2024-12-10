using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bayanihand.Repository.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);
        Task<List<T>> GetAllSync();
        Task DeleteAsync(int id);
        Task<T?> GetAsync(int? id);
        Task EditAsync(T entity);
        Task<bool> Exists(int? id);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null);
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter = null);
    }
}
