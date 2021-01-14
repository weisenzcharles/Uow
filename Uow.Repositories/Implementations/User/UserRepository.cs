using Dapper;
using Microsoft.EntityFrameworkCore;
using Open.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uow.Core.Domain.DataContext;
using Uow.Core.Domain.UnitOfWork;
using Uow.Domain.User;
using Uow.Repositories.Interface.User;

namespace Uow.Repositories.Implementations.User
{
    public class UserRepository : Repository<UserDomain>, IUserRepository
    {
        #region Fields...

        private readonly IUnitOfWork _unitOfWork;

        #endregion

        #region Constructors...

        /// <summary>
        /// 初始化 <see cref="UserRepository"/> 类的新实例。
        /// </summary>
        public UserRepository(IDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion


        /// <summary>
        /// 查询指定多个用户编号的 <see cref="UserDomain"/> 实体对象集合。
        /// </summary>
        /// <param name="userIds">指定多个用户编号。</param>
        /// <returns>返回一个 <see cref="UserDomain"/> 实体对象集合。</returns>
        public async Task<List<UserDomain>> QueryAsync(string userIds)
        {
            if (string.IsNullOrEmpty(userIds))
                return new List<UserDomain>();


            DynamicParameters param = new DynamicParameters();

            string sql = string.Format("SELECT * FROM t_user_user where id in ({0})", userIds);

            var list = await _unitOfWork.QueryAsync<UserDomain>(
                sql.ToString(), param);
            if (list != null)
                return list.ToList();
            else
                return new List<UserDomain>();

        }

        /// <summary>
        /// 分页获取用户列表。
        /// </summary>
        /// <param name="userName">用户名称。</param>
        /// <param name="pageIndex">分页索引。</param>
        /// <param name="pageSize">分页大小。</param>
        /// <returns>一个用户对象集合。</returns>
        public async Task<(List<UserDomain> list, int totalRecords)> QueryAsync(string userName, int pageIndex, int pageSize)
        {
            var query = Table;
            if (!string.IsNullOrEmpty(userName))
            {
                query = query.Where(i => i.Name == userName);
            }
            //query = query.OrderByDescending(b => b.CreatedTime);
            var totalRecords = query.Count();
            var dataList = await query.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
            if (dataList != null)
                return (dataList.ToList(), totalRecords);
            else
                return (new List<UserDomain>(), 0);
        }
    }
}
