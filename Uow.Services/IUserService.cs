using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uow.Entities;

namespace Uow.Services
{
    public interface IUserService
    {
        void Add(User user);
        IList<User> GetUsers();
    }
}
