using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uow.Entities;

namespace Uow.Repositories
{
    public interface IUserRepository
    {
        void Add(User user);

        IList<User> GetUsers();
    }
}
