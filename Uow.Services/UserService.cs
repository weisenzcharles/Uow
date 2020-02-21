using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uow.Domain;
using Uow.Repositories;

namespace Uow.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;


        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void Add(UserDomain user)
        {
            _userRepository.Add(user);
        }

        public IList<UserDomain> GetUsers()
        {
            return _userRepository.GetUsers();
        }
    }
}
