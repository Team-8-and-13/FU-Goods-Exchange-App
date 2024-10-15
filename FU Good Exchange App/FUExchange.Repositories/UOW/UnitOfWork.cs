using FUExchange.Contract.Repositories.Interface;
using FUExchange.Contract.Repositories.IUOW;
using FUExchange.Core.Base;
using FUExchange.Repositories.Context;

namespace FUExchange.Repositories.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool disposed = false;
        private readonly DatabaseContext _dbContext;


        public UnitOfWork(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }


        public void BeginTransaction()
        {
            _dbContext.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _dbContext.Database.CommitTransaction();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            disposed = true;
        }

        public void RollBack()
        {
            _dbContext.Database.RollbackTransaction();
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        // Phương thức này cần ràng buộc `BaseEntity` để khớp với GenericRepository
        public IGenericRepository<T> GetRepository<T>() where T : BaseEntity
        {
            return new GenericRepository<T>(_dbContext);
        }
        public IProImagesRepository GetProductImagRepository()
        {
            return new ProImagesRepository(_dbContext);

        }
    }
}
