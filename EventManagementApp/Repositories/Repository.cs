using EventManagementApp.Context;
using EventManagementApp.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace EventManagementApp.Repositories
{
    public abstract class Repository<T, K> : IRepository<T, K> where T : class
    {
        protected readonly EventManagementDBContext _context;

        public Repository(EventManagementDBContext _context) {
            this._context = _context;
        }

        public async Task<List<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetById(K id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual async Task<T> Add(T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        
    }
}
