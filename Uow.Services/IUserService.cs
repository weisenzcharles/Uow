using System.Collections.Generic;
using Uow.Domain;

namespace Uow.Services
{
    public interface IUserService
    {
        void Add(UserDomain user);
        IList<UserDomain> GetUsers();
    }
}