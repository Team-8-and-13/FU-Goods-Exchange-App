using Microsoft.EntityFrameworkCore;
using FUExchange.Contract.Repositories.Interface;
using FUExchange.Core.Base;
using FUExchange.Repositories.Context;
using FUExchange.Core;

namespace FUExchange.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly DatabaseContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(DatabaseContext dbContext)
        {
            _context = dbContext;
            _dbSet = _context.Set<T>();
        }

        public IQueryable<T> Entities => _dbSet;

        public IEnumerable<T> GetAll()
        {
            return _dbSet.AsEnumerable();
        }

        public async Task<IList<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public T? GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public async Task<T?> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<BasePaginatedList<T>> GetPagging(IQueryable<T> query, int index, int pageSize)
        {
            query = query.AsNoTracking();
            int count = await query.CountAsync();
            IReadOnlyCollection<T> items = await query.Skip((index - 1) * pageSize).Take(pageSize).ToListAsync();
            return new BasePaginatedList<T>(items, count, index, pageSize);
        }

        public void Insert(T obj)
        {
            _dbSet.Add(obj);
        }

        public async Task InsertAsync(T obj)
        {
            obj.CreatedTime = DateTime.Now;
            obj.LastUpdatedTime = obj.CreatedTime;
            await _dbSet.AddAsync(obj);
            await _context.SaveChangesAsync();
        }

        public void InsertRange(IList<T> obj)
        {
            _dbSet.AddRange(obj);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(T obj)
        {
            _dbSet.Update(obj);
        }

        public Task UpdateAsync(T obj)
        {
            return Task.FromResult(_dbSet.Update(obj));
        }

        public void Delete(object id)
        {
            var entity = _dbSet.Find(id);
            if (entity == null) throw new Exception("Entity not found.");

            _dbSet.Remove(entity);
        }

        public async Task DeleteAsync(object id)
        {
            var entity = await _dbSet.FindAsync(id.ToString());
            if (entity == null) throw new Exception("Entity not found.");

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        //public void Delete(object id, string userId)
        //{
        //    var entity = _dbSet.Find(id);
        //    if (entity == null) throw new Exception("Entity not found.");

        //    entity.DeletedBy = userId;
        //    entity.DeletedTime = DateTime.Now;

        //    _dbSet.Update(entity);
        //}

        //public async Task DeleteAsync(object id, string userId)
        //{
        //    var entity = await _dbSet.FindAsync(id);
        //    if (entity == null) throw new Exception("Entity not found.");

        //    entity.DeletedBy = userId;
        //    entity.DeletedTime = DateTime.Now;

        //    _dbSet.Update(entity);
        //    await _context.SaveChangesAsync();
    //}
}
}
