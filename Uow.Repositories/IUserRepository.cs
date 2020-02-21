using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uow.Domain;

namespace Uow.Repositories
{
    public interface IUserRepository
    {
        void Add(UserDomain user);

        IList<UserDomain> GetUsers();
    }
}
