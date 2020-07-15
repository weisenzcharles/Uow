using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Uow.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Uow.Core.Domain.DataContext
{
    /// <summary>
    /// 表示一个数据访问程序上下文对象。
    /// </summary>
    public partial interface IDbContext : IDisposable
    {
        /// <summary>
        /// The metadata about the shape of entities, the relationships between them, and how they map to the database.
        /// </summary>
        IModel Model { get; }

        /// <summary>
        /// Creates a DbSet that can be used to query and save instances of entity
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>A set for the given entity type</returns>
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        /// <summary>
        /// Gets an <see cref="EntityEntry"/> for the given entity. The entry provides access to change tracking information and operations for the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity to get the entry for.</param>
        /// <returns>The entry for the given entity.</returns>
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <returns>The number of state entries written to the database</returns>
        int SaveChanges();

        /// <summary>
        /// 将在此上下文中所做的所有更改异步保存到基础数据库。
        /// </summary>
        /// <returns>表示异步保存操作的任务。任务结果包含已写入基础数据库的对象数目。</returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// 将在此上下文中所做的所有更改异步保存到基础数据库。
        /// </summary>
        /// <param name="cancellationToken">等待任务完成期间要观察的 <see cref="CancellationToken"/>。</param>
        /// <returns>表示异步保存操作的任务。任务结果包含已写入基础数据库的对象数目。</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
