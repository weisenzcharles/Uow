using SQLite.CodeFirst;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uow.Core.Infrastructure;

namespace Uow.Data.DataContext
{
    public class SQLiteDbInitializer : SqliteDropCreateDatabaseWhenModelChanges<SQLiteDbContext>
    {
        public SQLiteDbInitializer(DbModelBuilder modelBuilder)
          : base(modelBuilder, typeof(IObjectState))
        { }

        protected override void Seed(SQLiteDbContext context)
        {
            // Here you can seed your core data if you have any.
        }
    }
}
