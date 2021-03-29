using System.Data.Entity;
using SQLite.CodeFirst;
using Uow.Core.Infrastructure;

namespace Uow.Data.DataContext
{
    public class SQLiteDbInitializer : SqliteDropCreateDatabaseWhenModelChanges<SQLiteDbContext>
    {
        public SQLiteDbInitializer(DbModelBuilder modelBuilder)
            : base(modelBuilder, typeof(IObjectState))
        {
        }

        protected override void Seed(SQLiteDbContext context)
        {
            // Here you can seed your core data if you have any.
        }
    }
}