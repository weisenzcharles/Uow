using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Uow.Core.Domain.Repositories;
using Uow.Domain.User;

namespace Uow.Repositories.Interface.User
{
    public interface IUserRepository : IRepository<UserDomain>
    {
        /// <summary>
        /// 查询指定多个用户编号的 <see cref="UserDomain"/> 实体对象集合。
        /// </summary>
        /// <param name="userIds">指定多个用户编号。</param>
        /// <returns>返回一个 <see cref="UserDomain"/> 实体对象集合。</returns>
        Task<List<UserDomain>> QueryAsync(string userIds);


        /// <summary>
        /// 分页获取用户列表。
        /// </summary>
        /// <param name="userName">用户名称。</param>
        /// <param name="pageIndex">分页索引。</param>
        /// <param name="pageSize">分页大小。</param>
        /// <returns>一个用户对象集合。</returns>
        Task<(List<UserDomain> list, int totalRecords)> QueryAsync(string userName, int pageIndex, int pageSize);
    }
}
