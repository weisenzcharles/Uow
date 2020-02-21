using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uow.Core.Domain.Repositories;
using Uow.Entities;

namespace Uow.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly IRepositoryAsync<User> _repository;

        /// <summary>
        /// 初始化 <see cref="UserRepository"/> 类的新实例。
        /// </summary>
        public UserRepository(IRepositoryAsync<User> userRepository)
        {
            _repository = userRepository;
        }

        public void Add(User user)
        {
            _repository.Insert(user);
        }

        public IList<User> GetUsers()
        {
            IList<User> users = new List<User>();
            users = _repository.Queryable(false).ToList();
            return users;
        }


    }
}
