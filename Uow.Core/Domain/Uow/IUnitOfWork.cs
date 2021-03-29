using System;
using System.Data;
using Uow.Core.Domain.Repositories;
using Uow.Core.Infrastructure;

namespace Uow.Core.Domain.Uow
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();
        void Dispose(bool disposing);
        IRepository<TEntity> Repository<TEntity>() where TEntity : class, IObjectState;
        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified);
        bool Commit();
        void Rollback();
    }
}