using System.Collections.Generic;
using System.Linq;
using Uow.Core.Domain.Repositories;
using Uow.Domain;

namespace Uow.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IRepositoryAsync<UserDomain> _repository;

        /// <summary>
        ///     初始化 <see cref="UserRepository" /> 类的新实例。
        /// </summary>
        public UserRepository(IRepositoryAsync<UserDomain> userRepository)
        {
            _repository = userRepository;
        }

        public void Add(UserDomain user)
        {
            _repository.Insert(user);
        }

        public IList<UserDomain> GetUsers()
        {
            IList<UserDomain> users = new List<UserDomain>();
            users = _repository.Queryable(false).ToList();
            return users;
        }
    }
}