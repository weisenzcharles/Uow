using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uow.Core.Domain.DataContext
{
    /// <summary>
    /// DbContext 提供程序。
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    public interface IDbContextProvider<out TDbContext>
        where TDbContext : DbContext
    {
        /// <summary>
        /// 获取 DbContext。
        /// </summary>
        /// <returns></returns>
        TDbContext GetDbContext();

        //TDbContext GetDbContext(MultiTenancySides? multiTenancySide);
    }
}
