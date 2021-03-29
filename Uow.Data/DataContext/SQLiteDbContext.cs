using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using SQLite.CodeFirst;

namespace Uow.Data.DataContext
{
    /// <summary>
    ///     SQLiteDbContext 实例表示工作单元和存储库模式的组合，可用来查询数据库并将更改组合在一起，这些更改稍后将作为一个单元写回存储区中。
    ///     SQLiteDbContext 在概念上与 ObjectContext 类似。
    /// </summary>
    public class SQLiteDbContext : DataContext
    {
        /// <summary>
        ///     在完成对派生上下文的模型的初始化后，并在该模型已锁定并用于初始化上下文之前，将调用此方法。
        ///     虽然此方法的默认实现不执行任何操作，但可在派生类中重写此方法，这样便能在锁定模型之前对其进行进一步的配置。
        /// </summary>
        /// <param name="modelBuilder">定义要创建的上下文的模型的生成器。</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //ModelConfiguration.Configure(modelBuilder);

            //var initializer = new SQLiteDbInitializer(modelBuilder);
            //Database.SetInitializer(initializer);
            Database.SetInitializer<SQLiteDbContext>(null);
            Database.SetInitializer(new SqliteDropCreateDatabaseWhenModelChanges<SQLiteDbContext>(modelBuilder));

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

        #region Constructors

        /// <summary>
        ///     可以将给定字符串用作将连接到的数据库的名称或连接字符串来构造一个新的上下文实例。请参见有关这如何用于创建连接的类备注。
        /// </summary>
        /// <param name="nameOrConnectionString">数据库名称或连接字符串。</param>
        public SQLiteDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        /// <summary>
        ///     通过现有连接来连接到数据库以构造一个新的上下文实例。如果 contextOwnsConnection 是 false，则释放上下文时将不会释放该连接。
        /// </summary>
        /// <param name="existingConnection">要用于新的上下文的现有连接。</param>
        /// <param name="contextOwnsConnection">如果设置为 true，则释放上下文时将释放该连接；否则调用方必须释放该连接。</param>
        public SQLiteDbContext(SQLiteConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {
        }

        #endregion Constructors
    }
}