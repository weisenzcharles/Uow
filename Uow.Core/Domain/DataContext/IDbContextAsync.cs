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
    public interface IDbContextAsync : IDbContext
    {

    }
}
