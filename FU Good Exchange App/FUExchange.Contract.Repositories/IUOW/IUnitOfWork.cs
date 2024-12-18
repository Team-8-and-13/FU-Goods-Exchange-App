﻿using FUExchange.Contract.Repositories.IUOW;
using FUExchange.Core.Base;

namespace FUExchange.Contract.Repositories.Interface
{
    public interface IUnitOfWork : IDisposable
    {

        IGenericRepository<T> GetRepository<T>() where T : BaseEntity;
        IProImagesRepository GetProductImagRepository();
        void Save();
        Task SaveAsync();
        void BeginTransaction();
        void CommitTransaction();
        void RollBack();
    }
}
