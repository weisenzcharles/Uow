using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uow.Core.Infrastructure;

namespace Uow.Core.Domain.DataContext
{
    /// <summary>
    /// 表示一个数据访问程序上下文对象。
    /// </summary>
    public interface IDataContext : IDisposable
    {
        /// <summary>
        /// 将在此上下文中所做的所有更改保存到基础数据库。
        /// </summary>
        /// <returns>已写入基础数据库的对象的数目。</returns>
        int SaveChanges();

        /// <summary>
        /// 同步实体对象的状态。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体对象类型。</typeparam>
        /// <param name="entity">指定的实体对象。</param>
        void SyncObjectState<TEntity>(TEntity entity) where TEntity : class, IObjectState;

        //void SyncObjectsStatePostCommit();
    }
}
