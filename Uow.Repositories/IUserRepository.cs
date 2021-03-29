using System.Collections.Generic;
using Uow.Domain;

namespace Uow.Repositories
{
    public interface IUserRepository
    {
        void Add(UserDomain user);

        IList<UserDomain> GetUsers();
    }
}