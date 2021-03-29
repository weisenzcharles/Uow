using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Uow.Data.DataContext
{
    /// <summary>
    ///     A DbContext instance represents a combination of the Unit Of Work and Repository
    ///     patterns such that it can be used to query from a database and group together
    ///     changes that will then be written back to the store as a unit. DbContext is conceptually
    ///     similar to ObjectContext.
    /// </summary>
    public class UowDbContext : DataContext
    {
        #region Private Fields...

        private bool _disposed;

        #endregion

        /// <summary>
        ///     在完成对派生上下文的模型的初始化后，并在该模型已锁定并用于初始化上下文之前，将调用此方法。
        ///     虽然此方法的默认实现不执行任何操作，但可在派生类中重写此方法，这样便能在锁定模型之前对其进行进一步的配置。
        /// </summary>
        /// <param name="modelBuilder">定义要创建的上下文的模型的生成器。</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // dynamically load all configuration
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => !string.IsNullOrEmpty(type.Namespace))
                .Where(type =>
                    type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() ==
                    typeof(EntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }

            base.OnModelCreating(modelBuilder);
        }

        ///// <summary>
        ///// Returns a <see cref="System.Data.Entity.DbSet{TEntity}"/> instance for access to entities of the given
        ///// type in the context and the underlying store.
        ///// </summary>
        ///// <typeparam name="TEntity">The type entity for which a set should be returned.</typeparam>
        ///// <returns>A set for the given entity type.</returns>
        //public IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity => base.Set<TEntity>();

        /// <summary>
        ///     Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <exception cref="System.Data.Entity.Infrastructure.DbUpdateException">
        ///     An error occurred sending updates to the database.
        /// </exception>
        /// <exception cref="System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
        ///     A database command did not affect the expected number of rows. This usually
        ///     indicates an optimistic concurrency violation; that is, a row has been changed
        ///     in the database since it was queried.
        /// </exception>
        /// <exception cref="System.Data.Entity.Validation.DbEntityValidationException">
        ///     The save was aborted because validation of entity property values failed.
        /// </exception>
        /// <exception cref="System.NotSupportedException">
        ///     An attempt was made to use unsupported behavior such as executing multiple
        ///     asynchronous commands concurrently on the same context instance.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     The context or connection have been disposed.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        ///     Some error occurred attempting to process entities in the context either
        ///     before or after sending commands to the database.
        /// </exception>
        /// <seealso cref="DbContext.SaveChanges" />
        /// <returns>The number of objects written to the underlying database.</returns>
        public override int SaveChanges()
        {
            //SyncObjectsStatePreCommit();
            var changes = base.SaveChanges();
            //SyncObjectsStatePostCommit();
            return changes;
        }

        /// <summary>
        ///     Asynchronously saves all changes made in this context to the underlying database.
        /// </summary>
        /// <exception cref="System.Data.Entity.Infrastructure.DbUpdateException">
        ///     An error occurred sending updates to the database.
        /// </exception>
        /// <exception cref="System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
        ///     A database command did not affect the expected number of rows. This usually
        ///     indicates an optimistic concurrency violation; that is, a row has been changed
        ///     in the database since it was queried.
        /// </exception>
        /// <exception cref="System.Data.Entity.Validation.DbEntityValidationException">
        ///     The save was aborted because validation of entity property values failed.
        /// </exception>
        /// <exception cref="System.NotSupportedException">
        ///     An attempt was made to use unsupported behavior such as executing multiple
        ///     asynchronous commands concurrently on the same context instance.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     The context or connection have been disposed.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        ///     Some error occurred attempting to process entities in the context either
        ///     before or after sending commands to the database.
        /// </exception>
        /// <seealso cref="DbContext.SaveChangesAsync" />
        /// <returns>
        ///     A task that represents the asynchronous save operation.  The
        ///     <see cref="Task.Result">Task.Result</see> contains the number of
        ///     objects written to the underlying database.
        /// </returns>
        public override async Task<int> SaveChangesAsync()
        {
            return await SaveChangesAsync(CancellationToken.None);
        }

        /// <summary>
        ///     Asynchronously saves all changes made in this context to the underlying database.
        /// </summary>
        /// <exception cref="System.Data.Entity.Infrastructure.DbUpdateException">
        ///     An error occurred sending updates to the database.
        /// </exception>
        /// <exception cref="System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
        ///     A database command did not affect the expected number of rows. This usually
        ///     indicates an optimistic concurrency violation; that is, a row has been changed
        ///     in the database since it was queried.
        /// </exception>
        /// <exception cref="System.Data.Entity.Validation.DbEntityValidationException">
        ///     The save was aborted because validation of entity property values failed.
        /// </exception>
        /// <exception cref="System.NotSupportedException">
        ///     An attempt was made to use unsupported behavior such as executing multiple
        ///     asynchronous commands concurrently on the same context instance.
        /// </exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     The context or connection have been disposed.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        ///     Some error occurred attempting to process entities in the context either
        ///     before or after sending commands to the database.
        /// </exception>
        /// <seealso cref="DbContext.SaveChangesAsync" />
        /// <returns>
        ///     A task that represents the asynchronous save operation.  The
        ///     <see cref="Task.Result">Task.Result</see> contains the number of
        ///     objects written to the underlying database.
        /// </returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            //SyncObjectsStatePreCommit();
            var changesAsync = await base.SaveChangesAsync(cancellationToken);
            //SyncObjectsStatePostCommit();
            return changesAsync;
        }

        ///// <summary>
        ///// 同步实体对象的状态。
        ///// </summary>
        ///// <typeparam name="TEntity">指定的实体对象类型。</typeparam>
        ///// <param name="entity">指定的实体对象。</param>
        //public void SyncObjectState<TEntity>(TEntity entity) where TEntity : class, IObjectState
        //{
        //    Entry(entity).State = entity.ObjectState;
        //}

        //private void SyncObjectsStatePreCommit()
        //{
        //    foreach (var dbEntityEntry in ChangeTracker.Entries())
        //    {
        //        dbEntityEntry.State = StateHelper.ConvertState(((IObjectState)dbEntityEntry.Entity).ObjectState);
        //    }
        //}

        //public void SyncObjectsStatePostCommit()
        //{

        //    foreach (var dbEntityEntry in ChangeTracker.Entries())
        //    {
        //        ((IObjectState)dbEntityEntry.Entity).ObjectState = dbEntityEntry.State;

        //        //clear cache
        //        string strCacheEntity = dbEntityEntry.Entity.GetType().Name;
        //        //bool canUseCache = CacheManager.Instance.GetDBCachePolicy().CanUseTotalCache(strCacheEntity);
        //        //if (canUseCache)
        //        //{
        //        string strCacheKey = Database.Connection.ConnectionString + "_" + strCacheEntity;
        //        //CacheManager.Instance.RemoveCache(strCacheKey);
        //        //}
        //    }
        //}

        /// <summary>
        ///     释放上下文。
        ///     在以下情况下也将释放基础 System.Data.Entity.Core.Objects.ObjectContext：它由此上下文创建，或者在创建此上下文时将所有权传递给了此上下文。
        ///     在以下情况下也将释放与数据库的连接（System.Data.Common.DbConnection 对象）：它由此上下文创建，或者在创建此上下文时将所有权传递给了此上下文。
        /// </summary>
        /// <param name="disposing">如果为 true，则同时释放托管资源和非托管资源；如果为 false，则仅释放非托管资源。</param>
        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // free other managed objects that implement
                    // IDisposable only
                }

                // release any unmanaged objects
                // set object references to null

                _disposed = true;
            }

            base.Dispose(disposing);
        }

        #region Constructors

        /// <summary>
        ///     Constructs a new context instance using conventions to create the name of the
        ///     database to which a connection will be made. The by-convention name is the full
        ///     name (namespace + class name) of the derived context class. See the class remarks
        ///     for how this is used to create a connection.
        /// </summary>
        static UowDbContext()
        {
            Database.SetInitializer<UowDbContext>(null);
            //Database.SetInitializer(new CreateDatabaseIfNotExists<UowDbContext>());
        }

        /// <summary>
        ///     Constructs a new context instance using conventions to create the name of the
        ///     database to which a connection will be made. The by-convention name is the full
        ///     name (namespace + class name) of the derived context class. See the class remarks
        ///     for how this is used to create a connection.
        /// </summary>
        public UowDbContext()
            : base("Default")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        /// <summary>
        ///     Constructs a new context instance using the given string as the name or connection
        ///     string for the database to which a connection will be made. See the class remarks
        ///     for how this is used to create a connection.
        /// </summary>
        /// <param name="nameOrConnectionString">Either the database name or a connection string.</param>
        public UowDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        #endregion
    }
}