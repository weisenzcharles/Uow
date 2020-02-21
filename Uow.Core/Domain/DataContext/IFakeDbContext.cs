using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uow.Core.Domain.Entities;

namespace Uow.Core.Domain.DataContext
{
    public interface IFakeDbContext : IDataContextAsync
    {
        DbSet<T> Set<T>() where T : class;

        void AddFakeDbSet<TEntity, TFakeDbSet>()
            where TEntity : EntityBase, new()
            where TFakeDbSet : FakeDbSet<TEntity>, IDbSet<TEntity>, new();
    }
}
