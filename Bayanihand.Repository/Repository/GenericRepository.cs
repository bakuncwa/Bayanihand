using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Bayanihand.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Bayanihand.Repository.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext db;
        public GenericRepository(AppDbContext context)
        {
            this.db = context;

        }
        public async Task<T> AddAsync(T entity)
        {
            await db.Set<T>().AddAsync(entity);
            await db.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await db.Set<T>().FindAsync(id);
            db.Set<T>().Remove(entity);
            await db.SaveChangesAsync();
        }

        public async Task EditAsync(T entity)
        {
            db.Set<T>().Update(entity);
            await db.SaveChangesAsync();
        }

        public async Task<bool> Exists(int? id)
        {
            T entity = await db.Set<T>().FindAsync(id);
            if (entity != null) return true;
            else return false;
        }

        public async Task<List<T>> GetAllSync()
        {
            return await db.Set<T>().ToListAsync();
        }

        public async Task<T?> GetAsync(int? id)
        {
            T? entity = await db.Set<T>().FindAsync(id);
            return entity;
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null)
        {
            return filter != null
                ? await db.Set<T>().Where(filter).ToListAsync()
                : await db.Set<T>().ToListAsync();
        }

        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter = null)
        {
            return await db.Set<T>().FirstOrDefaultAsync(filter);
        }
    }
}
