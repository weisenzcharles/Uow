using Dapper;
using Microsoft.EntityFrameworkCore.Storage;
using Uow.Core.Domain.Collections;
using Uow.Core.Domain.Entities;
using Uow.Core.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Uow.Core.Domain.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets the db connection.
        /// </summary>
        /// <returns>The instance of type <see cref="IDbConnection"/>.</returns>
        IDbConnection DbConnection { get; }

        #region EF Core...

        /// <summary>
        /// Executes the specified raw SQL command.
        /// </summary>
        /// <param name="sql">The raw SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The number of state entities written to database.</returns>
        int ExecuteSqlCommand(string sql, params object[] parameters);

        /// <summary>
        /// Uses raw SQL queries to fetch the specified <typeparamref name="TEntity"/> data.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="sql">The raw SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>An <see cref="IQueryable{T}"/> that contains elements that satisfy the condition specified by raw SQL.</returns>
        IQueryable<TEntity> FromSql<TEntity>(string sql, params object[] parameters) where TEntity : class;

        #endregion

        #region Dapper...

        #region Command Sql...

        ///// <summary>
        ///// Query
        ///// ag:await _unitOfWork.Query`Demo`("select id,name from school where id = @id", new { id = 1 });
        ///// </summary>
        ///// <typeparam name="TEntity"></typeparam>
        ///// <param name="sql">sql语句</param>
        ///// <returns></returns>
        //IEnumerable<TEntity> Query<TEntity>(string sql, object param = null) where TEntity : class;

        /// <summary>
        /// Query
        /// ag:await _unitOfWork.Query`Demo`("select id,name from school where id = @id", new { id = 1 });
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="trans"></param>
        /// <returns></returns>
        IEnumerable<TEntity> Query<TEntity>(string sql, object param = null, IDbContextTransaction trans = null) where TEntity : class;

        /// <summary>
        /// Execute
        /// ag: _unitOfWork.Execute("update school set name =@name where id =@id", new { name = "", id=1 });
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        int Execute(string sql, object param, IDbContextTransaction trans = null);

        /// <summary>
        /// QueryPagedList, complex sql, use "select * from (your sql) b"
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageSql"></param>
        /// <param name="pageSqlArgs"></param>
        /// <returns></returns>
        PagedList<TEntity> QueryPagedList<TEntity>(int pageIndex, int pageSize, string pageSql, object pageSqlArgs = null) where TEntity : class;

        #endregion

        Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null);

        Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TReturn>(CommandDefinition command, Func<TFirst, TSecond, TThird, TReturn> map, string splitOn = "Id");

        IEnumerable<TReturn> GetPageList<TReturn>(StringBuilder sqlColumns, StringBuilder sqlFrom, StringBuilder sqlJoin, StringBuilder sqlWhere, StringBuilder sqlOrderBy, int pageIndex, int pageSize, DynamicParameters param, out int totalCount, Func<string, IEnumerable<TReturn>> queryfunc);

        Task<(IEnumerable<TReturn> list, int totalCount)> GetPageListAsync<TReturn>(string sql, int pageIndex, int pageSize, DynamicParameters param, Func<string, Task<IEnumerable<TReturn>>> queryfuncAsync);

        Task<(IEnumerable<TReturn> list, int totalCount)> GetPageListAsync<TReturn>(StringBuilder sqlColumns, StringBuilder sqlFrom, StringBuilder sqlJoin, StringBuilder sqlWhere, StringBuilder sqlOrderBy, int pageIndex, int pageSize, DynamicParameters param, Func<string, Task<IEnumerable<TReturn>>> queryfuncAsync);
        #endregion

        #region SaveChanges...

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <param name="ensureAutoHistory"><c>True</c> if sayve changes ensure auto record the change history.</param>
        /// <returns>The number of state entries written to the database.</returns>
        int SaveChanges(bool ensureAutoHistory = false);

        #endregion

        #region Transaction...

        /// <summary>
        /// BeginTransaction
        /// </summary>
        /// <returns></returns>
        IDbContextTransaction BeginTransaction();

        void CommitTransaction();

        void RollbackTransaction();

        #endregion

        #region GetRepository...

        /// <summary>
        /// Gets the specified repository for the <typeparamref name="TEntity"/>.
        /// </summary>
        /// <param name="hasCustomRepository"><c>True</c> if providing custom repositry</param>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>An instance of type inherited from <see cref="IRepository{TEntity}"/> interface.</returns>
        IRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false) where TEntity : class, IEntity;

        #endregion

        #region Async...

        #region EF Core...
        /// <summary>
        /// Executes the specified raw SQL command.
        /// </summary>
        /// <param name="sql">The raw SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The number of state entities written to database.</returns>
        Task<int> ExecuteSqlCommandAsync(string sql, params object[] parameters);

        #endregion

        #region SaveChanges...

        /// <summary>
        /// Asynchronously saves all changes made in this unit of work to the database.
        /// </summary>
        /// <param name="ensureAutoHistory"><c>True</c> if save changes ensure auto record the change history.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous save operation. The task result contains the number of state entities written to database.</returns>
        Task<int> SaveChangesAsync(bool ensureAutoHistory = false);

        #endregion

        #region Dapper...

        #region Command Sql...

        /// <summary>
        /// QueryAsync
        /// ag:await _unitOfWork.QueryAsync`Demo`("select id,name from school where id = @id", new { id = 1 });
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="trans"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> QueryAsync<TEntity>(string sql, object param = null, IDbContextTransaction trans = null) where TEntity : class;

        /// <summary>
        /// ExecuteAsync
        /// ag:await _unitOfWork.ExecuteAsync("update school set name =@name where id =@id", new { name = "", id=1 });
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        Task<int> ExecuteAsync(string sql, object param, IDbContextTransaction trans = null);

        /// <summary>
        /// QueryPagedListAsync, complex sql, use "select * from (your sql) b"
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageSql"></param>
        /// <param name="pageSqlArgs"></param>
        /// <returns></returns>
        Task<PagedList<TEntity>> QueryPagedListAsync<TEntity>(int pageIndex, int pageSize, string pageSql, object pageSqlArgs = null) where TEntity : class;

        #endregion

        #endregion

        #region Transaction...

        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

        #endregion

        #endregion

    }
}
