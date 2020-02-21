using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Uow.Core.Domain.DataContext
{
    /// <summary>
    /// 表示一个支持异步的数据访问程序上下文对象。
    /// </summary>
    public interface IDataContextAsync : IDataContext
    {
        /// <summary>
        /// 将在此上下文中所做的所有更改异步保存到基础数据库。
        /// </summary>
        /// <returns>表示异步保存操作的任务。任务结果包含已写入基础数据库的对象数目。</returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// 将在此上下文中所做的所有更改异步保存到基础数据库。
        /// </summary>
        /// <param name="cancellationToken">等待任务完成期间要观察的 <see cref="System.Threading.CancellationToken"/>。</param>
        /// <returns>表示异步保存操作的任务。任务结果包含已写入基础数据库的对象数目。</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    }
}
