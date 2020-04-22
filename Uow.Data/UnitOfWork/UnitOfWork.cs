using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Uow.Core.Domain.Collections;
using Uow.Core.Domain.DapperAdapter;
using Uow.Core.Domain.DataContext;
using Uow.Core.Domain.Entities;
using Uow.Core.Domain.Repositories;
using Uow.Core.Domain.UnitOfWork;
using Open.Data.DataContext;
using Open.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Open.Data.UnitOfWork
{
    public class UnitOfWork<TContext> : IRepositoryFactory, IUnitOfWork<TContext> where TContext : DbContext, IDbContext
    {

        #region Private...

        private bool _disposed = false;
        private readonly TContext _context;
        private Dictionary<Type, object> repositories;

        #endregion

        #region Constructors...

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork{TContext}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public UnitOfWork(TContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        #endregion

        #region Dapper...

        #region Sync...

        public IEnumerable<TEntity> Query<TEntity>(string sql, object param = null, IDbContextTransaction trans = null) where TEntity : class
        {
            return DbConnection.Query<TEntity>(sql, param, trans?.GetDbTransaction());
        }

        /// <summary>
        /// Execute a command asynchronously.
        /// </summary>
        /// <param name="sql">The SQL to execute for this query.</param>
        /// <param name="param">The parameters to use for this query.</param>
        /// <param name="trans">The transaction to use for this query.</param>
        /// <returns>The number of rows affected.</returns>
        public int Execute(string sql, object param, IDbContextTransaction trans = null)
        {
            return DbConnection.Execute(sql, param, trans?.GetDbTransaction());
        }

        public PagedList<TEntity> QueryPagedList<TEntity>(int pageIndex, int pageSize, string pageSql, object pageSqlArgs = null) where TEntity : class
        {
            if (pageSize < 1 || pageSize > 5000)
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            if (pageIndex < 1)
                throw new ArgumentOutOfRangeException(nameof(pageIndex));

            var partedSql = PagingUtil.SplitSql(pageSql);
            ISqlAdapter sqlAdapter = null;
            if (Database.IsMySql())
                sqlAdapter = new MySqlAdapter();
            else if (Database.IsSqlServer())
                sqlAdapter = new SqlServerAdapter();
            else if (sqlAdapter == null)
                throw new System.Exception("Unsupported database type");
            pageSql = sqlAdapter.PagingBuild(ref partedSql, pageSqlArgs, (pageIndex - 1) * pageSize, pageSize);
            var sqlCount = PagingUtil.GetCountSql(partedSql);
            var totalCount = DbConnection.ExecuteScalar<int>(sqlCount, pageSqlArgs);
            var items = DbConnection.Query<TEntity>(pageSql, pageSqlArgs);
            var pagedList = new PagedList<TEntity>(items.ToList(), pageIndex - 1, pageSize, totalCount);
            return pagedList;
        }

        #endregion

        #region Async...

        public Task<IEnumerable<TEntity>> QueryAsync<TEntity>(string sql, object param = null, IDbContextTransaction trans = null) where TEntity : class
        {
            return DbConnection.QueryAsync<TEntity>(sql, param, trans?.GetDbTransaction());
        }

        /// <summary>
        /// Execute a command asynchronously using Task.
        /// </summary>
        /// <param name="sql">The SQL to execute for this query.</param>
        /// <param name="param">The parameters to use for this query.</param>
        /// <param name="trans">The transaction to use for this query.</param>
        /// <returns>The number of rows affected.</returns>
        public async Task<int> ExecuteAsync(string sql, object param, IDbContextTransaction trans = null)
        {
            return await DbConnection.ExecuteAsync(sql, param, trans?.GetDbTransaction());
        }

        /// <summary>
        /// Execute a query asynchronously using Task.
        /// </summary>
        /// <typeparam name="TEntity">The type of results to return.</typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageSql"></param>
        /// <param name="pageSqlArgs"></param>
        /// <returns></returns>
        public async Task<PagedList<TEntity>> QueryPagedListAsync<TEntity>(int pageIndex, int pageSize, string pageSql, object pageSqlArgs = null) where TEntity : class
        {
            if (pageSize < 1 || pageSize > 5000)
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            if (pageIndex < 1)
                throw new ArgumentOutOfRangeException(nameof(pageIndex));

            var partedSql = PagingUtil.SplitSql(pageSql);
            ISqlAdapter sqlAdapter = null;
            if (Database.IsMySql())
                sqlAdapter = new MySqlAdapter();
            else if (Database.IsSqlServer())
                sqlAdapter = new SqlServerAdapter();
            else if (sqlAdapter == null)
                throw new System.Exception("Unsupported database type");
            pageSql = sqlAdapter.PagingBuild(ref partedSql, pageSqlArgs, (pageIndex - 1) * pageSize, pageSize);
            var sqlCount = PagingUtil.GetCountSql(partedSql);
            var totalCount = await DbConnection.ExecuteScalarAsync<int>(sqlCount, pageSqlArgs);
            var items = await DbConnection.QueryAsync<TEntity>(pageSql, pageSqlArgs);
            var pagedList = new PagedList<TEntity>(items.ToList(), pageIndex - 1, pageSize, totalCount);
            return pagedList;
        }

        #endregion

        public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return await DbConnection.QueryAsync(sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TReturn>(CommandDefinition command, Func<TFirst, TSecond, TThird, TReturn> map, string splitOn = "Id")
        {
            return await DbConnection.QueryAsync(command, map, splitOn);
        }

        /// <summary>
        /// 使用 SQL 查询自动分页。
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="sqlColumns"></param>
        /// <param name="sqlFrom"></param>
        /// <param name="sqlJoin"></param>
        /// <param name="sqlWhere"></param>
        /// <param name="sqlOrderBy"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="param"></param>
        /// <param name="totalCount"></param>
        /// <param name="queryfunc">查询列表具体方法</param>
        /// <returns></returns>
        public IEnumerable<TReturn> GetPageList<TReturn>(StringBuilder sqlColumns, StringBuilder sqlFrom, StringBuilder sqlJoin, StringBuilder sqlWhere, StringBuilder sqlOrderBy, int pageIndex, int pageSize, DynamicParameters param, out int totalCount, Func<string, IEnumerable<TReturn>> queryfunc)
        {
            StringBuilder sqlTempString = new StringBuilder("SELECT {0} FROM {1} {2} where {3} {4}");
            StringBuilder sqlCount = new StringBuilder();
            sqlCount.AppendFormat(sqlTempString.ToString(), "count(*)", sqlFrom.ToString(), sqlJoin.ToString()
                , sqlWhere.ToString(), "");
            totalCount = DbConnection.ExecuteScalar<int>(sqlCount.ToString(), param);
            IEnumerable<TReturn> list = null;
            if (totalCount > 0)
            {
                StringBuilder sqlPage = new StringBuilder();
                sqlPage.AppendFormat(sqlTempString.ToString(), sqlColumns.ToString(), sqlFrom.ToString(), sqlJoin.ToString()
                , sqlWhere.ToString(), sqlOrderBy.ToString()).Append(" limit @pageindex,@pagesize ");
                if (param == null)
                    param = new DynamicParameters();
                param.Add("pageindex", (pageIndex - 1) * pageSize, System.Data.DbType.Int32);
                param.Add("pagesize", pageSize, System.Data.DbType.Int32);
                list = queryfunc(sqlPage.ToString());
            }

            return list;
        }
        /// <summary>
        /// 分页获取数据列表。
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="sql"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="param"></param>
        /// <param name="queryfuncAsync"></param>
        /// <returns></returns>
        public async Task<(IEnumerable<TReturn> list, int totalCount)> GetPageListAsync<TReturn>(string sql, int pageIndex, int pageSize, DynamicParameters param, Func<string, Task<IEnumerable<TReturn>>> queryfuncAsync)
        {
            //StringBuilder sqlTempString = new StringBuilder("SELECT count(*) FROM ("+ sql + ") as tmp");
            //StringBuilder sqlCount = new StringBuilder();
            //sqlCount.AppendFormat(sqlTempString.ToString(), "count(*)", sql);
            string sqlTotal = "SELECT count(*) FROM (" + sql + ") as tmp";
            var totalCount = await DbConnection.ExecuteScalarAsync<int>(sqlTotal, param);
            IEnumerable<TReturn> list = null;
            if (totalCount > 0)
            {
                StringBuilder sqlPage = new StringBuilder();
                sqlPage.Append(sql).Append(" limit @pageindex,@pagesize ");
                if (param == null)
                    param = new DynamicParameters();
                param.Add("pageindex", (pageIndex - 1) * pageSize, System.Data.DbType.Int32);
                param.Add("pagesize", pageSize, DbType.Int32);
                list = await queryfuncAsync(sqlPage.ToString());
            }

            return (list, totalCount);
        }
        /// <summary>
        /// 分页获取数据列表。
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="sqlColumns"></param>
        /// <param name="sqlFrom"></param>
        /// <param name="sqlJoin"></param>
        /// <param name="sqlWhere"></param>
        /// <param name="sqlOrderBy"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="param"></param>
        /// <param name="queryfuncAsync"></param>
        /// <returns></returns>
        public async Task<(IEnumerable<TReturn> list, int totalCount)> GetPageListAsync<TReturn>(StringBuilder sqlColumns, StringBuilder sqlFrom, StringBuilder sqlJoin, StringBuilder sqlWhere, StringBuilder sqlOrderBy, int pageIndex, int pageSize, DynamicParameters param, Func<string, Task<IEnumerable<TReturn>>> queryfuncAsync)
        {
            StringBuilder sqlTempString = new StringBuilder("SELECT {0} FROM {1} {2} where {3} {4}");
            StringBuilder sqlCount = new StringBuilder();
            sqlCount.AppendFormat(sqlTempString.ToString(), "count(*)", sqlFrom.ToString(), sqlJoin.ToString()
                , sqlWhere.ToString(), "");
            var totalCount = await DbConnection.ExecuteScalarAsync<int>(sqlCount.ToString(), param);
            IEnumerable<TReturn> list = null;
            if (totalCount > 0)
            {
                StringBuilder sqlPage = new StringBuilder();
                sqlPage.AppendFormat(sqlTempString.ToString(), sqlColumns.ToString(), sqlFrom.ToString(), sqlJoin.ToString()
                , sqlWhere.ToString(), sqlOrderBy.ToString()).Append(" limit @pageindex,@pagesize ");
                if (param == null)
                    param = new DynamicParameters();
                param.Add("pageindex", (pageIndex - 1) * pageSize, System.Data.DbType.Int32);
                param.Add("pagesize", pageSize, DbType.Int32);
                list = await queryfuncAsync(sqlPage.ToString());
            }

            return (list, totalCount);
        }

        #endregion

        #region IUnitOfWork...

        /// <summary>
        /// Executes the specified raw SQL command.
        /// </summary>
        /// <param name="sql">The raw SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The number of state entities written to database.</returns>
        public int ExecuteSqlCommand(string sql, params object[] parameters) => _context.Database.ExecuteSqlRaw(sql, parameters);

        /// <summary>
        /// Uses raw SQL queries to fetch the specified <typeparamref name="TEntity" /> data.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="sql">The raw SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>An <see cref="IQueryable{T}" /> that contains elements that satisfy the condition specified by raw SQL.</returns>
        public IQueryable<TEntity> FromSql<TEntity>(string sql, params object[] parameters) where TEntity : class => _context.Set<TEntity>().FromSqlRaw(sql, parameters);

        #endregion

        #region IUnitOfWorkAsync...

        /// <summary>
        /// Executes the specified raw SQL command.
        /// </summary>
        /// <param name="sql">The raw SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The number of state entities written to database.</returns>
        public async Task<int> ExecuteSqlCommandAsync(string sql, params object[] parameters) => await _context.Database.ExecuteSqlRawAsync(sql, parameters);

        #endregion

        #region IUnitOfWorkOfT...

        /// <summary>
        /// Gets the db context.
        /// </summary>
        /// <returns>The instance of type <typeparamref name="TContext"/>.</returns>
        public TContext DbContext => _context;

        /// <summary>
        /// Gets the db context.
        /// </summary>
        /// <returns>The instance of type <typeparamref name="TContext"/>.</returns>
        public DatabaseFacade Database => _context.Database;

        /// <summary>
        /// Gets the db connection.
        /// </summary>
        /// <returns>The instance of type <see cref="IDbConnection"/>.</returns>
        public IDbConnection DbConnection => _context.Database.GetDbConnection();

        #endregion

        #region IGenericRepositoryFactory...

        /// <summary>
        /// Gets the specified repository for the <typeparamref name="TEntity"/>.
        /// </summary>
        /// <param name="hasCustomRepository"><c>True</c> if providing custom repositry</param>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>An instance of type inherited from <see cref="IRepository{TEntity}"/> interface.</returns>
        public IRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false) where TEntity : class, IEntity
        {
            if (repositories == null)
            {
                repositories = new Dictionary<Type, object>();
            }

            // what's the best way to support custom reposity?
            if (hasCustomRepository)
            {
                var customRepository = _context.GetService<IRepository<TEntity>>();
                if (customRepository != null)
                {
                    return customRepository;
                }
            }

            var type = typeof(TEntity);
            if (!repositories.ContainsKey(type))
            {
                repositories[type] = new RepositoryBase<TEntity>(_context);
            }

            return (IRepository<TEntity>)repositories[type];
        }

        IRepositoryAsync<TEntity> IRepositoryFactory.GetRepositoryAsync<TEntity>(bool hasCustomRepository)
        {
            if (repositories == null)
            {
                repositories = new Dictionary<Type, object>();
            }

            // what's the best way to support custom reposity?
            if (hasCustomRepository)
            {
                var customRepository = _context.GetService<IRepositoryAsync<TEntity>>();
                if (customRepository != null)
                {
                    return customRepository;
                }
            }

            var type = typeof(TEntity);
            if (!repositories.ContainsKey(type))
            {
                repositories[type] = new RepositoryBase<TEntity>(_context);
            }

            return (IRepositoryAsync<TEntity>)repositories[type];
        }

        #endregion

        #region SaveChanges...

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }


        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <param name="ensureAutoHistory"><c>True</c> if save changes ensure auto record the change history.</param>
        /// <returns>The number of state entries written to the database.</returns>
        public int SaveChanges(bool ensureAutoHistory = false)
        {
            if (ensureAutoHistory)
            {
                _context.EnsureAutoHistory();
            }

            return _context.SaveChanges();
        }


        /// <summary>
        /// Asynchronously saves all changes made in this unit of work to the database.
        /// </summary>
        /// <param name="ensureAutoHistory"><c>True</c> if save changes ensure auto record the change history.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous save operation. The task result contains the number of state entities written to database.</returns>
        public async Task<int> SaveChangesAsync(bool ensureAutoHistory = false)
        {
            if (ensureAutoHistory)
            {
                _context.EnsureAutoHistory();
            }

            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Saves all changes made in this context to the database with distributed transaction.
        /// </summary>
        /// <param name="ensureAutoHistory"><c>True</c> if save changes ensure auto record the change history.</param>
        /// <param name="unitOfWorks">An optional <see cref="IUnitOfWork"/> array.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous save operation. The task result contains the number of state entities written to database.</returns>
        public async Task<int> SaveChangesAsync(bool ensureAutoHistory = false, params IUnitOfWork[] unitOfWorks)
        {
            using (var ts = new TransactionScope())
            {
                var count = 0;
                foreach (var unitOfWork in unitOfWorks)
                {
                    count += await unitOfWork.SaveChangesAsync(ensureAutoHistory);
                }
                count += await SaveChangesAsync(ensureAutoHistory);

                ts.Complete();
                return count;
            }
        }

        #endregion

        #region Transaction...

        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public void CommitTransaction()
        {
            _context.Database.CommitTransaction();
        }

        public void RollbackTransaction()
        {
            _context.Database.RollbackTransaction();
        }
        #endregion

        #region Dispose...

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        #endregion
    }
}
