using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyModel;
using Uow.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Uow.Core.Domain.DataContext
{
    public class UowDbContext : DbContext, IDbContext
    {

        /// <summary>
        /// Initializes a new instance of the <seealso cref="UowDbContext"/> class.
        /// </summary>
        protected UowDbContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <seealso cref="UowDbContext"/> class.
        /// </summary>
        /// <param name="options">The options for this context.</param>
        public UowDbContext(DbContextOptions options) : base(options)
        {
        }

        /// <summary>
        /// Further configuration the model
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assemblies = GetCurrentPathAssembly();
            foreach (var assembly in assemblies)
            {
                var entityTypes = assembly.GetTypes()
                    .Where(type => !string.IsNullOrWhiteSpace(type.Namespace))
                    .Where(type => type.IsClass)
                    .Where(type => type.BaseType != null)
                    .Where(type => typeof(IEntity).IsAssignableFrom(type));

                foreach (var entityType in entityTypes)
                {
                    if (modelBuilder.Model.FindEntityType(entityType) != null)
                        continue;
                    modelBuilder.Model.AddEntityType(entityType);
                }
            }
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Gets current path assembly.
        /// </summary>
        /// <returns></returns>
        private List<Assembly> GetCurrentPathAssembly()
        {
            var dlls = DependencyContext.Default.CompileLibraries
                .Where(x => !x.Name.StartsWith("Microsoft") && !x.Name.StartsWith("System"))
                .ToList();
            var list = new List<Assembly>();
            if (dlls.Any())
            {
                foreach (var dll in dlls)
                {
                    if (dll.Type == "project")
                    {
                        list.Add(Assembly.Load(dll.Name));
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Detach an entity from the context.
        /// </summary>
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <param name="entity">Entity.</param>
        public virtual void Detach<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var entityEntry = Entry((IEntity)entity);
            if (entityEntry == null)
                return;

            //set the entity is not being tracked by the context
            entityEntry.State = EntityState.Detached;
        }

        /// <summary>
        /// Creates a DbSet that can be used to query and save instances of entity
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>A set for the given entity type</returns>
        public virtual new DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        /// <summary>
        /// 将在此上下文中所做的所有更改异步保存到基础数据库。
        /// </summary>
        /// <returns>表示异步保存操作的任务。任务结果包含已写入基础数据库的对象数目。</returns>
        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
